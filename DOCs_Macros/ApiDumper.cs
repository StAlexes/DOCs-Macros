using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

public class ApiDumper
{
    /// <summary>
    /// Флаг глубокой очистки шума для RAG-индексации.
    /// true  — дополнительно отсекает внутренние сериализаторы настроек, типы подсистемы
    ///         обмена данными и избыточные перегрузки, различающиеся только CancellationToken.
    /// false — выгружает полный снимок сборок без дополнительной фильтрации.
    /// Базовые фильтры (служебные члены, skippedNamespaces, суффиксы имён) действуют
    /// в обоих режимах: флаг управляет только дополнительной очисткой.
    /// </summary>
    public static bool CleanRagNoise = true;

    /// <summary>
    /// Флаг вывода оси `api_domain` (`**Область API:**`), реестра сценариев и
    /// раздела-разделителя «как не перепутать».
    /// true  — справочник дополняется разметкой области API;
    /// false — генератор работает ровно так, как до появления оси: страховка для
    ///         быстрого отката, если новая разметка помешает RAG-конвейеру.
    /// </summary>
    public static bool EmitDomainMarkers = true;

    /// <summary>
    /// Флаг записи компактного манифеста области видимости (tflex_docs_api_scope.json)
    /// рядом со справочником. Манифест содержит только сценарии, явные переопределения
    /// и счётчики — без путей и времени, поэтому остаётся детерминированным.
    /// </summary>
    public static bool EmitScopeManifest = true;

    /// <summary>Значение оси api_domain: администрирование модели данных.</summary>
    private const string DomainStructure = "structure";

    /// <summary>Значение оси api_domain: работа с пользовательскими данными.</summary>
    private const string DomainObject = "object";

    /// <summary>Значение оси api_domain: общие материалы, применимые к обеим задачам.</summary>
    private const string DomainBoth = "both";

    /// <summary>
    /// Дополнительные шумовые пространства имён, отсекаемые только при CleanRagNoise == true.
    /// Это внутренние настройки конфигуратора, сериализаторы пакетов обмена и слой
    /// совместимости: прикладного API, пригодного для генерации кода макроязыка, в них нет.
    /// </summary>
    private static readonly string[] AdditionalNoiseNamespaces = {
        "TFlex.DOCs.Model.Configuration.ControlSettings",
        "TFlex.DOCs.Model.DataExchange.Settings.Serialization",
        "TFlex.DOCs.Model.Configuration.Compatibility"
    };

    // Прежний массив готовых строк EntryPointScenarios удалён намеренно.
    //
    // Причина замены: текст сценария и список проверяемых ссылок жили в разных местах —
    // строки Markdown печатались как есть, а проверить их рефлексией было нечем. Любая
    // правка формулировки («создать тип объектов» → «создать тип объекта») молча
    // разъезжалась с реальным API, и справочник начинал ссылаться на несуществующие
    // сигнатуры. Теперь источник один — ScenarioRegistry: из него рендерится Markdown
    // (BuildScenarioSections) и по нему же идёт валидация (ValidateScenarioMembers).
    // Формулировки прежней секции сохранены и перенесены в соответствующие сценарии.

    /// <summary>
    /// Вступительный абзац реестра сценариев. Текст сохранён дословно из прежней
    /// секции EntryPointScenarios: он объясняет читателю, зачем нужен реестр.
    /// </summary>
    private const string ScenarioRegistryIntro =
        "Типовые задачи генерации и изменения модели данных и соответствующие им точки входа в API. Если запрос звучит как «создать справочник», «создать тип объектов», «добавить параметр» — начинай именно с этих членов.";

    /// <summary>
    /// Заключительная строка реестра сценариев. Сохранена дословно из прежней секции:
    /// это общий набор поисковых слов, применимый ко всем сценариям сразу.
    /// </summary>
    private const string ScenarioRegistryFooter =
        "- **Ключевые слова для поиска сценариев:** создать, создание, добавить, новый, конструктор, сформировать, настроить, изменить, справочник, тип объектов, параметр, builder, factory, create, add, new, build, generate.";

    /// <summary>
    /// Фиксированный раздел-разделитель «как не перепутать» из шапки документа.
    ///
    /// Текст задан константой, а не собирается из рефлексии: это курируемое знание
    /// о предметной области (чем администрирование модели отличается от работы
    /// с данными), а не вывод из метаданных сборки. Раздел относится к области `both`.
    /// </summary>
    private static readonly string[] DomainGuideSection = {
        "## Модель данных и пользовательские объекты: как не перепутать",
        "- **Модель данных (structure):** создание и изменение справочников, типов объектов, параметров, групп параметров, списков 1:N, связей, индексов, форматов. Точки входа — `*Builder`/`*Factory` из `TFlex.DOCs.Model.Structure.Builders`, `ReferenceCatalog`.",
        "- **Пользовательские объекты (object):** создание и изменение экземпляров в уже существующих справочниках и списках, заполнение параметров, связи между объектами, ревизии. Точки входа — `Reference.CreateReferenceObject`, `СоздатьОбъект`, `ObjectAccessor.СоздатьОбъектСписка`, `Parameter.Value`.",
        "- **Правило:** «создать тип объекта» — это structure (`ClassObjectBuilder`); «создать объект» — это object (`CreateReferenceObject` / `СоздатьОбъект`). «Создать список объектов» (табличная часть 1:N) — structure (`ParameterGroupBuilder(ParameterGroupType.TableOneToMany, …)`); «создать объект в списке» — object (`СоздатьОбъектСписка`).",
        ""
    };

    /// <summary>
    /// Одна проверяемая рефлексией ссылка сценария: тип и его член.
    ///
    /// Зачем отдельная сущность: сценарий ссылается на несколько членов сразу
    /// (точка входа, продолжение, результат), и каждый нужно проверить независимо.
    /// Пропуск сценария целиком делается только тогда, когда не найдена точка входа;
    /// отсутствие продолжения лишь убирает один пункт из строки «Далее».
    /// </summary>
    private sealed class ScenarioMember
    {
        /// <summary>Полное имя типа, полученное из рефлексии (не «на глаз»).</summary>
        public string TypeFullName;

        /// <summary>Имя метода или свойства; null означает конструктор.</summary>
        public string Member;

        /// <summary>Человекочитаемое представление для строки отчёта о пропуске.</summary>
        public string Display;
    }

    /// <summary>
    /// Сценарий работы с API: то, что пользователь формулирует словами, и точные
    /// члены, которыми он реализуется.
    ///
    /// Структурированная форма нужна ровно потому, что строка Markdown не поддаётся
    /// проверке: рефлексия не может провалидировать текст. Здесь текст и проверяемые
    /// ссылки лежат рядом, поэтому формулировка и API не могут разъехаться молча.
    /// </summary>
    private sealed class ScenarioDefinition
    {
        /// <summary>Стабильный ключ сценария в kebab-case (используется как ключ в RAG).</summary>
        public string Id;

        /// <summary>Название сценария для секции `**Сценарий:**`.</summary>
        public string Title;

        /// <summary>Область API сценария: structure | object | both.</summary>
        public string Domain;

        /// <summary>Уровень API сценария: macro | core .</summary>
        public string Level;

        /// <summary>Готовая строка «точка входа» — только проверенные сигнатуры.</summary>
        public string EntryPoint;

        /// <summary>Результат сценария (имя типа) либо null, если результат не выражен типом.</summary>
        public string Result;

        /// <summary>Продолжения сценария: строки, часть которых может быть удалена валидацией.</summary>
        public string[] Next;

        /// <summary>Поисковые формулировки сценария.</summary>
        public string[] Keywords;

        /// <summary>Пояснения «не путать с» — предметная анти-подсказка для RAG.</summary>
        public string[] ConfusableWith;

        /// <summary>Ссылки сценария, проверяемые рефлексией при генерации.</summary>
        public ScenarioMember[] Members;
    }

    /// <summary>
    /// Создаёт проверяемую ссылку сценария. Вынесено в метод, чтобы записи реестра
    /// не разрастались повторами полного имени типа в строке отчёта.
    /// </summary>
    private static ScenarioMember Sm(string typeFullName, string member)
    {
        return new ScenarioMember
        {
            TypeFullName = typeFullName,
            Member = member,
            Display = member == null ? typeFullName + ".#ctor" : typeFullName + "." + member
        };
    }

    /// <summary>
    /// Полные имена типов и членов, на которые опирается реестр сценариев.
    /// Собраны в константы, чтобы опечатка в длинном имени не расползлась по реестру
    /// и не превратилась в «несуществующий API» после подстановки.
    /// </summary>
    private const string TBuilders = "TFlex.DOCs.Model.Structure.Builders.";
    private const string TClassObjectBuilder = TBuilders + "ClassObjectBuilder";
    private const string TParameterGroupBuilder = TBuilders + "ParameterGroupBuilder";
    private const string TParameterInfoBuilder = TBuilders + "ParameterInfoBuilder";
    private const string TLinkGroupBuilder = TBuilders + "LinkGroupBuilder";
    private const string TUniqueIndexBuilder = TBuilders + "UniqueIndexBuilder";
    private const string TReferenceBuilder = TBuilders + "ReferenceBuilder";
    private const string TExtendedStorageBuilder = TBuilders + "ExtendedParametersStorageBuilder";
    private const string TRefCatalog = "TFlex.DOCs.Model.ReferenceCatalog";
    private const string TRefCatalogFolder = "TFlex.DOCs.Model.ReferenceCatalogFolder";
    private const string TClassTree = "TFlex.DOCs.Model.Classes.ClassTree";
    private const string TClassObject = "TFlex.DOCs.Model.Classes.ClassObject";
    private const string TParameterGroup = "TFlex.DOCs.Model.Structure.ParameterGroup";
    private const string TParameterGroupType = "TFlex.DOCs.Model.Structure.ParameterGroupType";
    private const string TUniqueIndex = "TFlex.DOCs.Model.Structure.UniqueIndex";
    private const string TParameterInfo = "TFlex.DOCs.Model.Structure.ParameterInfo";
    private const string TReference = "TFlex.DOCs.Model.References.Reference";
    private const string TReferenceObject = "TFlex.DOCs.Model.References.ReferenceObject";
    private const string TDesktopObject = "TFlex.DOCs.Model.References.DesktopObject";
    private const string TMacroProvider = "TFlex.DOCs.Model.Macros.MacroProvider";
    private const string TObjectAccessor = "TFlex.DOCs.Model.Macros.ObjectModel.ObjectAccessor";
    private const string TParameter = "TFlex.DOCs.Model.Parameters.Parameter";
    private const string TParameterCollection = "TFlex.DOCs.Model.Parameters.ParameterCollection";

    /// <summary>
    /// Курируемый реестр сценариев API.
    ///
    /// Сценарий — это то, как задачу формулирует человек («создать тип объекта»),
    /// вместе с точными членами, которыми она решается. Прежний массив строк
    /// EntryPointScenarios содержал только текст, поэтому ни одна формулировка
    /// не была связана с реальным API и не проверялась.
    ///
    /// Все перечисленные ниже члены подтверждены рефлексией по сборкам поставки
    /// T-FLEX DOCs 18. Ни один не «вероятен».
    /// </summary>
    private static readonly ScenarioDefinition[] ScenarioRegistry = {
        new ScenarioDefinition {
            Id = "apply-changes",
            Title = "Сохранить изменения",
            Domain = DomainBoth,
            Level = "both",
            EntryPoint = "`ReferenceObject.EndChanges()`, `ReferenceObject.ApplyChanges()`, `ObjectAccessor.Save()`; для модели данных — `ClassObjectBuilder.Save()`, `ParameterGroupBuilder.Save()`, `ReferenceBuilder.Save(bool)`",
            Result = null,
            Next = new[] {
                "`ReferenceObject.BeginChanges()` — начать пакет изменений",
                "`ReferenceObject.CancelChanges()` — отменить пакет изменений",
                "`DesktopObject.EndChanges()` — зафиксировать изменения контейнера"
            },
            Keywords = new[] {
                "сохранить изменения", "зафиксировать изменения", "применить изменения", "commit", "save changes", "apply changes"
            },
            ConfusableWith = new[] {
                "`Save()` (structure) — сохранение элемента модели данных (builder); `EndChanges()` (object) — фиксация изменений экземпляра объекта"
            },
            Members = new[] {
                Sm(TReferenceObject, "EndChanges"), Sm(TReferenceObject, "ApplyChanges"),
                Sm(TReferenceObject, "BeginChanges"), Sm(TReferenceObject, "CancelChanges"),
                Sm(TObjectAccessor, "Save"), Sm(TDesktopObject, "EndChanges"),
                Sm(TClassObjectBuilder, "Save"), Sm(TParameterGroupBuilder, "Save"), Sm(TReferenceBuilder, "Save")
            }
        },
        new ScenarioDefinition {
            Id = "create-class-object",
            Title = "Создать тип объекта (класс объектов)",
            Domain = DomainStructure,
            Level = "core",
            EntryPoint = "`new TFlex.DOCs.Model.Structure.Builders.ClassObjectBuilder(ClassTree)` → `ClassObjectBuilder.Save()`. Ключевые слова: создать тип объектов, новый тип объектов, класс объектов, create class object, type builder",
            Result = "ClassObject",
            Next = new[] {
                "`ClassObjectBuilder.ParameterGroups` — группы параметров нового типа",
                "`ParameterInfoBuilder` — параметры типа",
                "`LinkGroupBuilder` — связи типа объектов"
            },
            Keywords = new[] {
                "создать тип объекта", "создать тип объектов", "новый тип объектов", "класс объектов", "create class object", "type builder"
            },
            ConfusableWith = new[] {
                "`create-reference-object` (core, object) — создание объекта справочника: `Reference.CreateReferenceObject(...)`",
                "`СоздатьОбъект` (macro, object) — создание экземпляра объекта в макроязыке"
            },
            Members = new[] {
                Sm(TClassObjectBuilder, null), Sm(TClassObjectBuilder, "Save"),
                Sm(TClassObjectBuilder, "ParameterGroups"), Sm(TClassTree, "Find")
            }
        },
        new ScenarioDefinition {
            Id = "create-derived-class-object",
            Title = "Создать производный тип объекта",
            Domain = DomainStructure,
            Level = "core",
            EntryPoint = "`new ClassObjectBuilder(ClassTree, ClassObject baseType)` — тип, порождённый от указанного базового типа",
            Result = "ClassObject",
            Next = new[] {
                "`ClassObjectBuilder.Base` — базовый тип, от которого порождён новый",
                "`ClassObjectBuilder.Save()` — сохранение типа"
            },
            Keywords = new[] {
                "создать производный тип объекта", "наследник типа объекта", "базовый тип объекта", "derived class object"
            },
            ConfusableWith = new[] {
                "`create-class-object` (structure) — создание типа объектов «с нуля», без базового типа"
            },
            Members = new[] {
                Sm(TClassObjectBuilder, null), Sm(TClassObjectBuilder, "Base"),
                Sm(TClassObjectBuilder, "Save"), Sm(TClassObject, "Name")
            }
        },
        new ScenarioDefinition {
            Id = "create-extended-storage",
            Title = "Создать хранилище расширенных параметров",
            Domain = DomainStructure,
            Level = "core",
            EntryPoint = "`new TFlex.DOCs.Model.Structure.Builders.ExtendedParametersStorageBuilder(ServerConnection connection)` → задать `Name` и `Comment`",
            Result = "ExtendedParametersStorage",
            Next = new[] {
                "`ExtendedParametersStorageBuilder.ValuesStorage` — созданное хранилище",
                "`ExtendedParametersManager.AttachExtendedParameterToParameterGroup(ParameterInfo, ParameterGroup)` — подключение расширенного параметра к группе"
            },
            Keywords = new[] {
                "создать хранилище параметров", "расширенные параметры справочника", "хранилище расширенных параметров"
            },
            ConfusableWith = new[] {
                "`create-parameter` (structure) — создание обычного параметра справочника; расширенный параметр подключается к уже существующей группе"
            },
            Members = new[] {
                Sm(TExtendedStorageBuilder, null), Sm(TExtendedStorageBuilder, "ValuesStorage"),
                Sm(TExtendedStorageBuilder, "Name"), Sm(TRefCatalog, "Find")
            }
        },
        new ScenarioDefinition {
            Id = "create-link-group",
            Title = "Создать связь (ссылочное свойство) типа объектов",
            Domain = DomainStructure,
            Level = "core",
            EntryPoint = "`new TFlex.DOCs.Model.Structure.Builders.LinkGroupBuilder(ParameterGroup group)` → `LinkGroupBuilder.Save()`",
            Result = "ParameterGroup",
            Next = new[] {
                "`LinkGroupBuilder.LinkType` — вид связи",
                "`LinkGroupBuilder.SlaveGroup` — группа-приёмник связи"
            },
            Keywords = new[] {
                "создать связь", "создать ссылочное свойство", "добавить связь типа объектов", "link group"
            },
            ConfusableWith = new[] {
                "`link-objects` (object) — связывание двух существующих объектов: `ReferenceObject.CreateChildLink`, `ReferenceObject.CreateParentLink`, `DesktopObject.SetLinkedObject`"
            },
            Members = new[] {
                Sm(TLinkGroupBuilder, null), Sm(TLinkGroupBuilder, "Save"),
                Sm(TLinkGroupBuilder, "LinkType"), Sm(TLinkGroupBuilder, "SlaveGroup"),
                Sm(TParameterGroup, "Type")
            }
        },
        new ScenarioDefinition {
            Id = "create-list-object",
            Title = "Создать объект в существующем списке",
            Domain = DomainObject,
            Level = "both",
            EntryPoint = "`ObjectAccessor.СоздатьОбъектСписка(String имяСпискаОбъектов)` → `ObjectAccessor.Save()`; в макроязыке — `RefObj CreateListObject(String)`",
            Result = "RefObj",
            Next = new[] {
                "`ObjectAccessor.СоздатьОбъектСписка(String, String)` — с указанием имени списка и типа",
                "`ObjectAccessor.Save()` — сохранение созданного объекта"
            },
            Keywords = new[] {
                "создать объект в списке", "создать строку списка", "добавить объект в список объектов", "СоздатьОбъектСписка"
            },
            ConfusableWith = new[] {
                "`create-object-list` (structure) — создание самой табличной части 1:N: `ParameterGroupBuilder(ParameterGroupType.TableOneToMany, ...)`, `ParameterGroupBuilder.Save()`"
            },
            Members = new[] {
                Sm(TObjectAccessor, "СоздатьОбъектСписка"), Sm(TObjectAccessor, "Save"),
                Sm(TObjectAccessor, "CreateListObject"), Sm(TDesktopObject, "CreateListObject")
            }
        },
        new ScenarioDefinition {
            Id = "create-macro-object",
            Title = "Создать объект в макроязыке",
            Domain = DomainObject,
            Level = "macro",
            EntryPoint = "`MacroProvider.СоздатьОбъект(String справочник)`, `MacroProvider.CreateObject(String referenceName)` → `ObjectAccessor.Save()`",
            Result = "RefObj",
            Next = new[] {
                "`MacroProvider.СоздатьОбъект(String, String имяТипа)` — с указанием типа объекта",
                "`MacroProvider.СоздатьОбъект(String, ТипОбъекта)` — с указанием типа как значения",
                "`ObjectAccessor.Тип` / `ObjectAccessor.Reference` — проверка созданного объекта"
            },
            Keywords = new[] {
                "создать объект", "создать объект справочника в макроязыке", "СоздатьОбъект", "CreateObject", "macro object"
            },
            ConfusableWith = new[] {
                "`create-reference-object` (core, object) — низкоуровневое создание объекта: `Reference.CreateReferenceObject(...)`",
                "`create-class-object` (core, structure) — создание типа объекта (класса), а не экземпляра"
            },
            Members = new[] {
                Sm(TMacroProvider, "СоздатьОбъект"), Sm(TMacroProvider, "CreateObject"),
                Sm(TObjectAccessor, "Save")
            }
        },
        new ScenarioDefinition {
            Id = "create-object-list",
            Title = "Создать подчинённый список объектов 1:N (табличная часть, список строк)",
            Domain = DomainStructure,
            Level = "core",
            EntryPoint = "`new ParameterGroupBuilder(ParameterGroupType.TableOneToMany, ParameterGroup parentGroup, ClassObject ownerClass)` → `ParameterGroupBuilder.Save()`; далее `ParameterGroupBuilder.ParameterGroup`, `ParameterGroup.Classes`, `new ClassObjectBuilder(ClassObjectCollection)` → `Save()`, `new ParameterInfoBuilder(ParameterGroup)` → `Save()`",
            Result = "ParameterGroup (список), ClassObject (класс строк)",
            Next = new[] {
                "`ParameterInfoBuilder` для каждой колонки",
                "`ClassObjectBuilder` для класса строк"
            },
            Keywords = new[] {
                "создать список объектов", "подчинённый список", "табличная часть", "список строк", "строки таблицы",
                "таблица 1:N", "один ко многим", "TableOneToMany", "ParameterGroupBuilder", "ParameterInfoBuilder", "ClassObjectBuilder"
            },
            ConfusableWith = new[] {
                "`create-list-object` (object) — создание экземпляра строки в уже существующем списке: `ObjectAccessor.СоздатьОбъектСписка`, `DesktopObject.CreateListObject`"
            },
            Members = new[] {
                Sm(TParameterGroupBuilder, null), Sm(TParameterGroupBuilder, "Save"),
                Sm(TParameterGroupBuilder, "ParameterGroup"), Sm(TParameterGroup, "Classes"),
                Sm(TClassObjectBuilder, null), Sm(TClassObjectBuilder, "Save"),
                Sm(TParameterInfoBuilder, null), Sm(TParameterInfoBuilder, "Save"),
                Sm(TParameterGroupType, "TableOneToMany")
            }
        },
        new ScenarioDefinition {
            Id = "create-parameter",
            Title = "Создать параметр справочника",
            Domain = DomainStructure,
            Level = "core",
            EntryPoint = "`TFlex.DOCs.Model.Structure.Builders.ParameterInfoBuildersFactory.CreateParameterInfoBuilder(...)`, `new TFlex.DOCs.Model.Structure.Builders.ParameterInfoBuilder(ParameterGroup)` → `ParameterInfoBuilder.Save()`",
            Result = "ParameterInfo",
            Next = new[] {
                "`ParameterInfoBuilder.Type` — тип параметра (`ParameterType`)",
                "`ParameterInfoBuilder.Name` — имя параметра",
                "`ParameterInfo.Type` — тип созданного параметра"
            },
            Keywords = new[] {
                "создать параметр", "создать параметр справочника", "добавить параметр", "создание параметра", "create parameter", "parameter builder"
            },
            ConfusableWith = new[] {
                "`fill-parameter` (object) — запись значения уже существующего параметра экземпляра объекта: `Parameter.Value`",
                "`read-parameter` (object) — чтение значения: `Parameter.Value`, `Parameter.GetString()`"
            },
            Members = new[] {
                Sm(TParameterInfoBuilder, null), Sm(TParameterInfoBuilder, "Save"),
                Sm(TParameterGroupBuilder, "ParameterGroup"), Sm(TParameterInfo, "Type"),
                Sm(TParameterGroup, "Parameters")
            }
        },
        new ScenarioDefinition {
            Id = "create-parameter-group",
            Title = "Создать группу параметров",
            Domain = DomainStructure,
            Level = "core",
            EntryPoint = "`new TFlex.DOCs.Model.Structure.Builders.ParameterGroupBuilder(ParameterGroup masterGroup, ClassObject tableClass, ParameterGroup sourceTable)` → `ParameterGroupBuilder.Save()`. Ключевые слова: создать группу параметров, группа параметров, create parameter group",
            Result = "ParameterGroup",
            Next = new[] {
                "`new ParameterGroupBuilder(ParameterGroupType type, ParameterGroup masterGroup, ClassObject classObject)` — группа заданного вида",
                "`ParameterInfoBuilder` — параметры внутри группы",
                "`ParameterGroup.Parameters` — параметры созданной группы"
            },
            Keywords = new[] {
                "создать группу параметров", "добавить группу параметров", "создание группы параметров", "create parameter group", "parameter group builder"
            },
            ConfusableWith = new[] {
                "`create-object-list` (structure) — подчинённый список 1:N: та же точка входа с `ParameterGroupType.TableOneToMany`"
            },
            Members = new[] {
                Sm(TParameterGroupBuilder, null), Sm(TParameterGroupBuilder, "Save"),
                Sm(TParameterGroupBuilder, "ParameterGroup"), Sm(TParameterGroup, "Parameters"),
                Sm(TClassObjectBuilder, "ParameterGroups")
            }
        },
        new ScenarioDefinition {
            Id = "create-reference",
            Title = "Создать справочник",
            Domain = DomainStructure,
            Level = "core",
            EntryPoint = "`TFlex.DOCs.Model.ReferenceCatalog.CreateReferenceBuilder(ReferenceCatalogFolder)` → настроить `ReferenceBuilder` → `ReferenceBuilder.Save(bool)`. Ключевые слова: создать справочник, новый справочник, генерация справочника, create reference",
            Result = "Reference",
            Next = new[] {
                "`ReferenceBuilder.RevisionNamingRule` — правило именования ревизий",
                "`ReferenceBuilder.HierarchyType` — вид иерархии",
                "`ClassObjectBuilder` — типы объектов нового справочника"
            },
            Keywords = new[] {
                "создать справочник", "новый справочник", "генерация справочника", "create reference", "reference builder"
            },
            ConfusableWith = new[] {
                "`create-reference-object` (object) — создание объекта внутри уже существующего справочника"
            },
            Members = new[] {
                Sm(TRefCatalog, "CreateReferenceBuilder"), Sm(TReferenceBuilder, null),
                Sm(TReferenceBuilder, "Save"), Sm(TReferenceBuilder, "Folder")
            }
        },
        new ScenarioDefinition {
            Id = "create-reference-folder",
            Title = "Создать папку каталога справочников",
            Domain = DomainStructure,
            Level = "core",
            EntryPoint = "`ReferenceCatalog.CreateFolder(string name, ReferenceCatalogFolder parent)`, `ReferenceCatalogFolder.CreateFolder(String)`",
            Result = "ReferenceCatalogFolder",
            Next = new[] {
                "`ReferenceCatalogFolder.CreateFolder(String)` — вложенная папка",
                "`ReferenceCatalogFolder.Rename(String)` — переименование папки",
                "`ReferenceCatalog.MoveFolder(ReferenceCatalogFolder, ReferenceCatalogFolder)` — перемещение папки"
            },
            Keywords = new[] {
                "создать папку справочников", "папка каталога справочников", "создать папку каталога"
            },
            ConfusableWith = new[] {
                "`create-reference` (structure) — создание самого справочника, а не папки каталога"
            },
            Members = new[] {
                Sm(TRefCatalog, "CreateFolder"), Sm(TRefCatalogFolder, "CreateFolder"),
                Sm(TRefCatalog, "MoveFolder"), Sm(TRefCatalogFolder, "Rename"),
                Sm(TRefCatalogFolder, "Name")
            }
        },
        new ScenarioDefinition {
            Id = "create-reference-object",
            Title = "Создать объект справочника",
            Domain = DomainObject,
            Level = "core",
            EntryPoint = "`Reference.CreateReferenceObject()`, `Reference.CreateReferenceObject(ClassObject)`, `Reference.CreateReferenceObject(ReferenceObject)`, `Reference.CreateReferenceObject(ReferenceObject, ClassObject)`",
            Result = "ReferenceObject",
            Next = new[] {
                "`ReferenceObject.BeginChanges()` / `ReferenceObject.EndChanges()` — запись значений параметров",
                "`ReferenceObject.ParameterValues` — параметры созданного объекта",
                "`ReferenceObject.ApplyChanges()` — применение изменений"
            },
            Keywords = new[] {
                "создать объект справочника", "создать экземпляр объекта", "новый объект справочника", "CreateReferenceObject"
            },
            ConfusableWith = new[] {
                "`create-class-object` (structure) — создание типа объекта, а не экземпляра",
                "`create-macro-object` (macro, object) — то же действие через макроязык: `СоздатьОбъект`"
            },
            Members = new[] {
                Sm(TReference, "CreateReferenceObject"), Sm(TReferenceObject, "BeginChanges"),
                Sm(TReferenceObject, "EndChanges"), Sm(TReferenceObject, "ApplyChanges"),
                Sm(TReference, "Classes")
            }
        },
        new ScenarioDefinition {
            Id = "create-revision",
            Title = "Создать ревизию объекта",
            Domain = DomainObject,
            Level = "core",
            EntryPoint = "`ReferenceObject.CreateRevision(RevisionLevelObject level, ReferenceObject source, Boolean recursive, Boolean copyApplicability)`",
            Result = "ReferenceObject",
            Next = new[] {
                "`ReferenceObject.GetRevisions()` — список ревизий объекта",
                "`ReferenceBuilder.RevisionNamingRule` — правило именования ревизий справочника"
            },
            Keywords = new[] {
                "создать ревизию", "новая ревизия объекта", "create revision"
            },
            ConfusableWith = new[] {
                "`apply-changes` (both) — сохранение изменений объекта; ревизия создаётся отдельным вызовом `CreateRevision`"
            },
            Members = new[] {
                Sm(TReferenceObject, "CreateRevision"), Sm(TReferenceObject, "GetRevisions"),
                Sm(TReferenceBuilder, "RevisionNamingRule")
            }
        },
        new ScenarioDefinition {
            Id = "create-unique-index",
            Title = "Задать условие уникальности",
            Domain = DomainStructure,
            Level = "core",
            EntryPoint = "`new TFlex.DOCs.Model.Structure.Builders.UniqueIndexBuilder(ParameterGroup parameterGroup)` → `UniqueIndexBuilder.Save()`. Ключевые слова: условие уникальности, уникальный индекс, unique index",
            Result = "UniqueIndex",
            Next = new[] {
                "`UniqueIndexBuilder.Parameters` — набор параметров условия",
                "`UniqueIndex.Parameters` — параметры созданного индекса",
                "`UniqueIndex.Group` — группа параметров индекса"
            },
            Keywords = new[] {
                "условие уникальности", "уникальный индекс", "уникальность типа объекта", "unique index"
            },
            ConfusableWith = new[] {
                "`create-parameter-group` (structure) — создание группы параметров, к которой затем привязывается индекс"
            },
            Members = new[] {
                Sm(TUniqueIndexBuilder, null), Sm(TUniqueIndexBuilder, "Save"),
                Sm(TUniqueIndexBuilder, "Parameters"), Sm(TUniqueIndex, "Parameters"),
                Sm(TUniqueIndex, "Group"), Sm(TClassObjectBuilder, "UniqueIndex")
            }
        },
        new ScenarioDefinition {
            Id = "edit-class-object",
            Title = "Изменить существующий тип объекта",
            Domain = DomainStructure,
            Level = "core",
            EntryPoint = "`new ClassObjectBuilder(ClassTree, ClassObject existingType)` → изменить свойства → `Save()`",
            Result = "ClassObject",
            Next = new[] {
                "`ClassObjectBuilder.Name` — переименование типа",
                "`ClassObjectBuilder.Comment` — описание типа",
                "`ClassObjectBuilder.Save()` — сохранение изменений"
            },
            Keywords = new[] {
                "изменить тип объекта", "изменить тип объектов", "редактировать класс объектов", "rename class object"
            },
            ConfusableWith = new[] {
                "`create-derived-class-object` (structure) — та же точка входа, но тип создаётся порождённым от базового, а не редактируется"
            },
            Members = new[] {
                Sm(TClassObjectBuilder, null), Sm(TClassObjectBuilder, "Name"),
                Sm(TClassObjectBuilder, "Comment"), Sm(TClassObjectBuilder, "Save"),
                Sm(TClassTree, "Find")
            }
        },
        new ScenarioDefinition {
            Id = "fill-parameter",
            Title = "Заполнить параметр",
            Domain = DomainObject,
            Level = "both",
            EntryPoint = "`Parameter.Value` (запись); в макроязыке — `ObjectAccessor.Parameter` → `ParameterAccessor`",
            Result = null,
            Next = new[] {
                "`ReferenceObject.BeginChanges()` — начать изменения",
                "`ReferenceObject.EndChanges()` — зафиксировать значение",
                "`Parameter.SetModified()` — пометить параметр изменённым"
            },
            Keywords = new[] {
                "заполнить параметр", "записать значение параметра", "изменить значение параметра", "set parameter value"
            },
            ConfusableWith = new[] {
                "`create-parameter` (structure) — создание самого параметра в модели данных, а не запись его значения",
                "`read-parameter` (object) — чтение значения того же параметра"
            },
            Members = new[] {
                Sm(TParameter, "Value"), Sm(TParameter, "SetModified"),
                Sm(TParameterCollection, "GetParameter"), Sm(TReferenceObject, "EndChanges")
            }
        },
        new ScenarioDefinition {
            Id = "find-object",
            Title = "Найти объект",
            Domain = DomainObject,
            Level = "both",
            EntryPoint = "`MacroProvider.FindObject(String referenceName, String filter)`, `Reference.Find(String)`, `Reference.Find(ParameterInfo, Object)`",
            Result = "ReferenceObject",
            Next = new[] {
                "`MacroProvider.FindObjects(String referenceName, String filter)` — список найденных объектов",
                "`Reference.Find(Filter, MacroContext)` — поиск по фильтру",
                "`DesktopObject.GetObject(Guid)` — объект из контейнера"
            },
            Keywords = new[] {
                "найти объект", "поиск объекта", "найти объект по параметру", "find object"
            },
            ConfusableWith = new[] {
                "`find-reference` (structure) — поиск самого справочника в каталоге, а не объекта в нём"
            },
            Members = new[] {
                Sm(TMacroProvider, "FindObject"), Sm(TMacroProvider, "FindObjects"),
                Sm(TReference, "Find"), Sm(TDesktopObject, "GetObject")
            }
        },
        new ScenarioDefinition {
            Id = "find-reference",
            Title = "Найти существующий справочник",
            Domain = DomainStructure,
            Level = "core",
            EntryPoint = "`ReferenceCatalog.Find(string)`, `ReferenceCatalog.FindReference(string)`, `ReferenceCatalog.FindFolderByName(string)`",
            Result = "ReferenceInfo",
            Next = new[] {
                "`ReferenceCatalogFolder.References` — справочники внутри найденной папки",
                "`ReferenceCatalog.GetAllReferences()` — полный список справочников"
            },
            Keywords = new[] {
                "найти справочник", "найти существующий справочник", "поиск справочника"
            },
            ConfusableWith = new[] {
                "`find-object` (object) — поиск объекта внутри справочника, а не самого справочника"
            },
            Members = new[] {
                Sm(TRefCatalog, "Find"), Sm(TRefCatalog, "FindReference"),
                Sm(TRefCatalog, "FindFolderByName"), Sm(TRefCatalog, "GetAllReferences"),
                Sm(TRefCatalogFolder, "References")
            }
        },
        new ScenarioDefinition {
            Id = "link-objects",
            Title = "Связать объекты",
            Domain = DomainObject,
            Level = "core",
            EntryPoint = "`ReferenceObject.CreateChildLink(ReferenceObject)`, `ReferenceObject.CreateParentLink(ReferenceObject)`, `DesktopObject.SetLinkedObject(ParameterGroup, ReferenceObject)`",
            Result = "ComplexHierarchyLink",
            Next = new[] {
                "`ReferenceObject.GetChildLink(ReferenceObject)` — получить существующее дочернее подключение",
                "`DesktopObject.AddLinkedObject(ParameterGroup, ReferenceObject)` — добавить связанный объект",
                "`ReferenceObject.DeleteLink(ComplexHierarchyLink)` — удалить подключение"
            },
            Keywords = new[] {
                "связать объекты", "создать связь между объектами", "подключить объект", "link objects"
            },
            ConfusableWith = new[] {
                "`create-link-group` (structure) — создание связи как элемента модели данных: `LinkGroupBuilder`, а не связывание двух экземпляров"
            },
            Members = new[] {
                Sm(TReferenceObject, "CreateChildLink"), Sm(TReferenceObject, "CreateParentLink"),
                Sm(TDesktopObject, "SetLinkedObject"), Sm(TDesktopObject, "AddLinkedObject"),
                Sm(TReferenceObject, "DeleteLink"), Sm(TParameterGroup, "Type")
            }
        },
        new ScenarioDefinition {
            Id = "read-parameter",
            Title = "Прочитать значение параметра",
            Domain = DomainObject,
            Level = "both",
            EntryPoint = "`Parameter.Value` (чтение); `Parameter.GetString()`, `Parameter.GetValue()`",
            Result = null,
            Next = new[] {
                "`ParameterCollection.GetParameter(ParameterInfo)` — получить параметр по описанию",
                "`ParameterCollection.GetParameter(Int32)` — получить параметр по идентификатору",
                "`ReferenceObject.ParameterValues` — коллекция параметров объекта"
            },
            Keywords = new[] {
                "прочитать значение параметра", "получить значение параметра", "значение параметра", "read parameter value"
            },
            ConfusableWith = new[] {
                "`fill-parameter` (object) — запись значения того же параметра",
                "`create-parameter` (structure) — создание параметра в модели данных"
            },
            Members = new[] {
                Sm(TParameter, "Value"), Sm(TParameter, "GetString"),
                Sm(TParameter, "GetValue"), Sm(TParameterCollection, "GetParameter")
            }
        }
    };

    /// <summary>
    /// Явная таблица доменов по ПОЛНОМУ имени типа — высший приоритет классификации (§5.1).
    ///
    /// Сопоставление идёт по полному имени, а не по простому: короткие имена в этих
    /// сборках неоднозначны (`Parameter` существует ещё и в `TFlex.DOCs.Common.Expressions`,
    /// а `Reference`/`ReferenceObject` — во вложенных типах `SystemAccessGroups`).
    /// Ключ по полному имени исключает привязку к чужому типу.
    /// </summary>
    private static readonly Dictionary<string, string> ExplicitTypeDomains =
        new Dictionary<string, string>(StringComparer.Ordinal)
        {
            { "TFlex.DOCs.Model.ReferenceCatalog", DomainStructure },
            { "TFlex.DOCs.Model.ReferenceCatalogFolder", DomainStructure },

            { TBuilders + "ReferenceBuilder", DomainStructure },
            { TBuilders + "ClassObjectBuilder", DomainStructure },
            { TBuilders + "ParameterGroupBuilder", DomainStructure },
            { TBuilders + "ParameterInfoBuilder", DomainStructure },
            { TBuilders + "ExtendedParameterInfoBuilder", DomainStructure },
            { TBuilders + "AliasedParameterInfoBuilder", DomainStructure },
            { TBuilders + "LinkGroupBuilder", DomainStructure },
            { TBuilders + "UniqueIndexBuilder", DomainStructure },
            { TBuilders + "ExtendedParametersStorageBuilder", DomainStructure },
            { TBuilders + "ObjectFormatBuilder", DomainStructure },
            { TBuilders + "ObjectParameterFormatBuilder", DomainStructure },

            { "TFlex.DOCs.Model.Classes.ClassTree", DomainStructure },
            { "TFlex.DOCs.Model.Classes.ClassObject", DomainStructure },
            { "TFlex.DOCs.Model.Structure.ParameterInfo", DomainStructure },
            { "TFlex.DOCs.Model.Structure.UniqueIndex", DomainStructure },
            { "TFlex.DOCs.Model.Structure.ParameterGroupType", DomainStructure },

            // Группа параметров описывает модель, но через неё же читаются значения,
            // поэтому она осознанно отнесена к общим материалам.
            { "TFlex.DOCs.Model.Structure.ParameterGroup", DomainBoth },

            { "TFlex.DOCs.Model.References.Reference", DomainObject },
            { "TFlex.DOCs.Model.References.ReferenceObject", DomainObject },
            { "TFlex.DOCs.Model.References.Files.FileReferenceObject", DomainObject },

            { "TFlex.DOCs.Model.Parameters.Parameter", DomainObject },
            { "TFlex.DOCs.Model.Parameters.ParameterCollection", DomainObject },
            { "TFlex.DOCs.Model.Macros.ObjectModel.ParameterAccessor", DomainObject },

            { "TFlex.DOCs.Model.Macros.ObjectModel.ObjectAccessor", DomainObject },
            { "TFlex.DOCs.Model.Macros.ObjectModel.RefObj", DomainObject },
            { "TFlex.DOCs.Model.Macros.ObjectModel.RefObjList", DomainObject },

            { "TFlex.DOCs.Model.Access.AccessManager", DomainBoth }
        };

    /// <summary>
    /// Явные переопределения домена на уровне члена: ключ «ПолноеИмяТипа.Член» (§5.2, п. 3 и 5).
    ///
    /// Нужны там, где правило по имени дало бы неверный ответ: `ReferenceObject.*`
    /// и `ParameterCollection.*` формально подпадают под «имя содержит Model/Settings»,
    /// но по смыслу это работа с данными. Каждая запись обязана существовать в сборке —
    /// несуществующая печатает предупреждение (см. ValidateMemberDomainOverrides).
    /// </summary>
    private static readonly Dictionary<string, string> ExplicitMemberDomains =
        new Dictionary<string, string>(StringComparer.Ordinal)
        {
            { TReference + ".CreateReferenceObject", DomainObject },
            { TParameter + ".Value", DomainObject },
            { TObjectAccessor + ".СоздатьОбъектСписка", DomainObject },
            { TDesktopObject + ".CreateListObject", DomainObject },
            { TDesktopObject + ".SetLinkedObject", DomainObject },
            { TDesktopObject + ".AddLinkedObject", DomainObject },
            { TReferenceObject + ".CreateRevision", DomainObject },
            { TReferenceObject + ".CreateChildLink", DomainObject },
            { TReferenceObject + ".CreateParentLink", DomainObject },
            { TReferenceObject + ".GetRevisions", DomainObject },
            { TReferenceObject + ".DeleteLink", DomainObject },
            { TReferenceObject + ".SetSignature", DomainObject },
            { TMacroProvider + ".СоздатьОбъект", DomainObject },
            { TMacroProvider + ".CreateObject", DomainObject },
            { TMacroProvider + ".FindObject", DomainObject },
            { TMacroProvider + ".FindObjects", DomainObject },
            { TParameterCollection + ".GetParameter", DomainObject },
            { TParameter + ".GetString", DomainObject },
            { TParameter + ".GetValue", DomainObject },
            { TParameter + ".SetModified", DomainObject }
        };

    /// <summary>
    /// Префиксы пространств имён, целиком относящихся к модели данных (правило §5.1 по namespace).
    /// </summary>
    private static readonly string[] StructureNamespacePrefixes = {
        "TFlex.DOCs.Model.Structure"
    };

    /// <summary>
    /// Префиксы пространств имён макроязыка: это пользовательские данные, а не модель (§5.1).
    /// </summary>
    private static readonly string[] ObjectNamespacePrefixes = {
        "TFlex.DOCs.Model.Macros"
    };

    /// <summary>
    /// Имена методов базовых builder-классов, которые подмешиваются в блок производного типа.
    /// Ограничение списка намеренно: без него в справочник утекли бы десятки служебных
    /// унаследованных членов и размыли бы векторный поиск.
    /// </summary>
    private static readonly string[] InheritedBuilderMemberNames = {
        "Save", "SaveAsync", "Delete", "DeleteAsync", "Build", "Create", "Update",
        "Validate", "Reload", "Load", "LoadAsync"
    };

    public static void ExportTFlexApiToMarkdown()
    {
        Console.WriteLine("=== ГЕНЕРАЦИЯ СПРАВОЧНИКА API ИЗ DLL ===");

        // 1. Берем единый путь из Program.cs
        string tflexFolder = Program.TFlexFolder;
        Console.WriteLine($"Папка DLL: {tflexFolder}");

        if (!Directory.Exists(tflexFolder))
        {
            Console.WriteLine($" ОШИБКА: Папка не найдена: {tflexFolder}");
            return;
        }

        // Точный перечень DLL, которые нужно обработать (можно закомментировать и использовать поиск по шаблону)
        // Ядро API данных: TFlex.DOCs.Model.dll содержит не только типы пространства имён
        // TFlex.DOCs.Model, но и кластер справочников TFlex.DOCs.Model.References.*
        // (Reference, ReferenceObject, FileReferenceObject, LoadSettings и др.).
        // Отдельной сборки TFlex.DOCs.Model.References.dll в поставке T-FLEX DOCs 18 нет —
        // в списке она оставлена на случай других редакций/версий, а отсутствующие сборки
        // не прерывают выгрузку.
        string[] targetAssemblies = {
            "TFlex.DOCs.Model.dll",
            "TFlex.DOCs.Common.dll",
            //"TFlex.DOCs.UI.Client.dll",
            //"TFlex.DOCs.UI.Client.Common.dll"
        };

        // 2. Или ищем все DLL по шаблону "TFlex.DOCs.Model*.dll" в указанной папке
        // string[] targetAssemblies = Directory.GetFiles(tflexFolder, "TFlex.DOCs.Model*.dll")
        //     .Select(Path.GetFileName)
        //     .ToArray();

        // Служебные члены, не несущие прикладной нагрузки в справочнике API.
        // BeginInvoke/EndInvoke — методы, которые компилятор генерирует для делегатов;
        // они не являются частью ручного API и только засоряют векторный поиск RAG.
        string[] ignoredMethods = { 
            "Equals", "GetHashCode", "GetType", "ToString", "MemberwiseClone", 
            "ReferenceEquals", "InitializeLifetimeService", "Dispose", "Clone",
            "BeginInvoke", "EndInvoke"
        };

        // Служебные пространства имён, засоряющие векторный поиск RAG: логирование,
        // внутренняя инфраструктура, превью файлов, диагностика и внутренние структуры
        // конфигуратора. Типы из этих пространств (и всех вложенных в них) не выгружаются.
        string[] skippedNamespaces = {
            "TFlex.DOCs.Model.Logging",
            "TFlex.DOCs.Model.Internal",
            "TFlex.DOCs.Model.FilePreview",
            "TFlex.DOCs.Model.Diagnostics",
            "TFlex.DOCs.Model.Configuration.ControlSettings.Diagram",
            "TFlex.DOCs.Model.Configuration.Configurations.WebConfigurationStructure"
        };

        var sb = new StringBuilder();
        sb.AppendLine("# Справочник API T-FLEX DOCs (с пространствами имён)");
        sb.AppendLine("> Используй указанные `Namespace` для добавления в `using`. Строка `**Уровень API:**` отделяет прикладной Macro DSL (доступно внутри MacroProvider) от низкоуровневого Core Open API; пометка `Core Open API (генерация модели данных)` — это типы-конструкторы (Builder/Factory), через которые собирается модель данных. Строка `**Область API:**` задаёт вторую независимую ось: `structure` — администрирование модели данных, `object` — работа с пользовательскими объектами, `both` — общее для обеих задач. Пометка [RU: …] — русскоязычный алиас метода или свойства, найденный зеркалированием по идентичному тексту `<summary>` в XML-документации; [RU only] — член, существующий только в русскоязычном алиасе; [унаследовано] — член, полученный от базового класса коллекции макромодели. Каждый метод выводится отдельной секцией `#### [Method]`, каждый конструктор — секцией `#### [Constructor]`; в чанке доступны строки `**Алиасы:**`, `**RU only**`, `**Async**`, `**Возвращает:**` (выводится только когда текст `<returns>` описывает результат, а не пересказывает summary), `**Назначение:**`, `**Ключевые слова:**`, `**Как получить:**`, `**Как получить (из кода):**`, `**Прямое создание:**` и `**Унаследовано от:**`.\n");

        // Раздел-разделитель и реестр сценариев идут ДО первого `## Сборка:`:
        // сценарий — это отдельная секция верхнего уровня, и внутри блоков типов
        // он попал бы в чужой чанк, разорвав разметку RAG.
        if (EmitDomainMarkers)
        {
            sb.AppendLine($"- **Область API документа:** {DomainBoth}");
            sb.AppendLine();

            foreach (var guideLine in DomainGuideSection)
                sb.AppendLine(guideLine);
        }

        // Курируемые сценарии генерации модели данных — раньше говорящий запрос
        // «Создать справочник» / «Создать тип объекта» не имел ни одной лексической точки
        // опоры в справочнике: описания builder-типов сформулированы через «создает экземпляр
        // класса», и векторный поиск не выводил их в топ.
        // Секции рендерятся из реестра: тот же реестр затем валидируется рефлексией,
        // поэтому текст сценария и проверяемые ссылки не могут разъехаться.
        var skippedScenarios = new List<string>();
        var knownScenarioIds = new HashSet<string>(StringComparer.Ordinal);

        // Границы секции сценариев в буфере. Валидация ссылок возможна только после
        // загрузки всех сборок, то есть уже после того, как текст секции записан.
        // Запомненные границы позволяют перерисовать секцию без сценариев, у которых
        // не нашлась точка входа, не перестраивая весь документ.
        int scenarioStart = sb.Length;
        int scenarioEnd = scenarioStart;
        if (EmitDomainMarkers)
        {
            BuildScenarioSections(sb, skippedScenarios, knownScenarioIds, null);
            scenarioEnd = sb.Length;
        }

        // Индекс «полное имя типа → Type» по всем обрабатываемым сборкам. Нужен для двух
        // проверок, которые нельзя выполнить внутри цикла по одной сборке: валидация ссылок
        // реестра сценариев и валидация явных таблиц домена. Обе обязаны видеть все сборки
        // сразу, иначе ссылка на тип из второй сборки считалась бы несуществующей.
        var typeIndex = new Dictionary<string, Type>(StringComparer.Ordinal);

        // Перечисления, выведенные в справочник: набор общий для всех сборок, чтобы
        // одно и то же перечисление не появилось дважды в разных секциях.
        var emittedEnums = new HashSet<string>(StringComparer.Ordinal);

        // Итоговое распределение по областям и уровням API — для консольной сводки (§5.2)
        // и для манифеста (§11).
        var domainCounts = new Dictionary<string, int>(StringComparer.Ordinal);
        var levelCounts = new Dictionary<string, int>(StringComparer.Ordinal);

        // Карта «builder-тип → сценарий, для которого его конструктор является точкой входа».
        var ctorEntryPointScenarios = BuildCtorEntryPointScenarios();

        // Предупреждения о неклассифицированных типах и членах печатаются один раз
        // на полное имя: иначе консоль утонула бы в повторах по каждому вызову.
        var domainWarned = new HashSet<string>(StringComparer.Ordinal);

        int totalTypes = 0;

        foreach (var dllName in targetAssemblies)
        {
            string fullPath = Path.Combine(tflexFolder, dllName);

            // Отсутствующая сборка пропускается, но обязательно попадает в журнал:
            // молчаливый пропуск ранее скрывал, что запрошенного файла нет в поставке.
            if (!File.Exists(fullPath))
            {
                Console.WriteLine($" [Пропуск] Сборка не найдена: {dllName}");
                continue;
            }

            // 3. ПАРНЫЙ XML-ФАЙЛ ДОКУМЕНТАЦИИ (TFlex.DOCs.Model.xml рядом с TFlex.DOCs.Model.dll).
            //    Если XML нет — дамп продолжает штатную работу, формируя сигнатуры без описаний.
            var xmlDoc = new XmlDocProvider();
            string xmlPath = Path.ChangeExtension(fullPath, ".xml");
            if (File.Exists(xmlPath))
            {
                if (xmlDoc.LoadXml(xmlPath))
                    Console.WriteLine($" [XML Docs] Загружена документация: {Path.GetFileName(xmlPath)} (описаний: {xmlDoc.Count})");
                else
                    Console.WriteLine($" [XML Docs] Файл найден, но описания не извлечены: {Path.GetFileName(xmlPath)}");
            }

            try
            {
                var asm = Assembly.LoadFrom(fullPath);
                sb.AppendLine($"## Сборка: {dllName}\n");

                var types = asm.GetExportedTypes()
                    .Where(t => t.IsPublic && (t.IsClass || t.IsInterface))
                    .Where(t => !skippedNamespaces.Any(ns => t.Namespace != null && (t.Namespace == ns || t.Namespace.StartsWith(ns + "."))))
                    // Очистка шума (только при CleanRagNoise): внутренние настройки конфигуратора,
                    // сериализаторы пакетов обмена и слой совместимости. Отсекается и само
                    // пространство имён, и все вложенные в него.
                    .Where(t => !CleanRagNoise || t.Namespace == null || !AdditionalNoiseNamespaces.Any(ns => t.Namespace == ns || t.Namespace.StartsWith(ns + ".")))
                    // Типы подсистемы обмена данными (DataExchange*) — это преимущественно DTO-модели
                    // и служебные описатели: 113 типов, из которых XML-описание есть лишь у 13.
                    // Единственная точка входа, реально нужная для генерации кода макроязыка, —
                    // DataExchangeGateway; она сохраняется намеренно.
                    // ВНИМАНИЕ: суффикс "Settings" отсекать нельзя — вместе с настройками обмена
                    // под фильтр попал бы прикладной TFlex.DOCs.Model.References.LoadSettings.
                    .Where(t => !CleanRagNoise || !t.Name.StartsWith("DataExchange") || t.Name == "DataExchangeGateway")
                    .Where(t => !t.Name.EndsWith("EventArgs") &&
                                !t.Name.EndsWith("EventHandler") &&
                                !t.Name.EndsWith("Exception") &&
                                // Типы-конструкторы (Builder/Factory) и их ВЛОЖЕННЫЕ типы
                                // освобождаются от суффиксных фильтров целиком. Это ключевые
                                // точки входа генерации модели данных: вложенный
                                // ClassObjectBuilder.ClassObjectCollection отсекался по суффиксу
                                // Collection, а ParameterGroupBuilder+ParameterGroupCollection
                                // вместе с ним терял методы CanAddItem/CanRemoveItem.
                                (IsBuilderRelatedType(t) ||
                                 IsScenarioReferencedType(t) ||
                                 (!t.Name.EndsWith("Collection") &&
                                  !t.Name.EndsWith("Description") &&
                                  !t.Name.EndsWith("Dto") &&
                                  !t.Name.EndsWith("Activity") &&
                                  !t.Name.Contains("Xml") &&
                                  !t.Name.Contains("Log") &&
                                  !t.Name.Contains("Enumerator") &&
                                  !t.Name.Contains("Internal") &&
                                  !t.Name.StartsWith("<"))))
                    .OrderBy(t => t.Namespace)
                    .ThenBy(t => t.Name)
                    .ToList();

                int addedForDll = 0;

                // Предварительный проход строит обратные связи «builder-тип → методы, которые
                // его возвращают» и «builder-тип → методы, которые его принимают».
                // Раньше связь была только прямой (метод → тип) и работала в одну сторону:
                // в блоке ClassObjectBuilder не было ни одной ссылки на способ его получить,
                // поэтому запрос «создать тип объекта» не мог дойти от типа-конструктора
                // обратно к точке входа и сценарий распадался на несвязанные фрагменты.
                var builderCreators = new Dictionary<string, List<string>>(StringComparer.Ordinal);
                var builderConsumers = new Dictionary<string, List<string>>(StringComparer.Ordinal);
                BuildBuilderLinkMaps(types, builderCreators, builderConsumers);

                // Индекс пополняется до обхода: к моменту валидации сценариев он должен
                // содержать типы всех сборок, а не только той, что обрабатывается сейчас.
                foreach (var indexed in types)
                {
                    string indexedName = indexed.FullName ?? indexed.Name;
                    if (!typeIndex.ContainsKey(indexedName))
                        typeIndex[indexedName] = indexed;
                }

                // Перечисления собираются по тем же типам, что попадут в справочник:
                // это гарантирует, что описанные значения действительно используются
                // в приведённых рядом сигнатурах (§9, п. 1).
                var enumCandidates = new HashSet<Type>();

                foreach (var type in types)
                {
                    if (EmitDomainMarkers)
                        CollectReferencedEnums(type, enumCandidates, skippedNamespaces);

                    // 1. Свойства
                    var allProps = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly).ToList();
                    var engProps = allProps.Where(p => !Regex.IsMatch(p.Name, @"\p{IsCyrillic}")).ToList();
                    var ruProps = allProps.Where(p => Regex.IsMatch(p.Name, @"\p{IsCyrillic}")).ToList();

                    var propLines = new List<PropEntry>();

                    // Зеркалирование свойств выполняется одним проходом по классу: строится
                    // карта «латинское свойство → кириллический алиас» по идентичности текста
                    // <summary> (см. BuildPropertyMirrors), что полностью заменяет прежнее
                    // сопоставление по одному лишь совпадению типа.
                    var propMirrors = BuildPropertyMirrors(engProps, ruProps, xmlDoc);

                    // Обратная карта «кириллический алиас → латинское свойство»: нужна, чтобы
                    // распознать уже закреплённые алиасы при обходе в порядке объявления членов.
                    var aliasOwner = new Dictionary<PropertyInfo, PropertyInfo>();
                    foreach (var pair in propMirrors)
                        aliasOwner[pair.Value] = pair.Key;

                    // Обход в исходном порядке объявления свойств сохраняет естественную
                    // последовательность чтения: RAG-сплиттеры не разрывают логически
                    // связанные свойства на два удалённых блока.
                    foreach (var p in allProps)
                    {
                        bool isCyrillic = Regex.IsMatch(p.Name, @"\p{IsCyrillic}");

                        // Русский алиас уже показан в строке латинского свойства как [RU: ...].
                        // Отдельной строкой его выводить не нужно.
                        if (isCyrillic && aliasOwner.ContainsKey(p))
                            continue;

                        PropertyInfo ruMatch;
                        propMirrors.TryGetValue(p, out ruMatch);

                        string note;
                        if (isCyrillic)
                        {
                            // Кириллическое свойство аннотируется только если оно не стало алиасом.
                            note = aliasOwner.ContainsKey(p) ? "" : " [RU only]";
                        }
                        else
                        {
                            note = ruMatch != null ? $" [RU: {ruMatch.Name}]" : "";
                        }

                        // Параметры индексатора являются частью сигнатуры свойства и не должны теряться в RAG-дампе.
                        string propName = BuildPropertySignature(p);

                        string propSummary = xmlDoc.GetPropertySummary(p);
                        propLines.Add(new PropEntry
                        {
                            TypeName = FormatTypeName(p.PropertyType),
                            Name = propName,
                            RuNote = note,
                            DocNote = string.IsNullOrEmpty(propSummary) ? "" : $" — {propSummary}"
                        });
                    }

                    // ГАРАНТИРОВАННЫЙ ВЫВОД ЧЛЕНОВ КОЛЛЕКЦИЙ МАКРОМОДЕЛИ.
                    // У списков макромодели (RefObjList, Объекты и им подобных) индексатор
                    // Item[...] и счётчик Count объявлены в базовом generic-классе
                    // (ObjectAccessorList<T>), поэтому выборка DeclaredOnly их не возвращает,
                    // и в справочнике пропадали ключевые способы доступа к элементам коллекции.
                    // Тип свойства при этом уже подставлен конструированным generic-аргументом
                    // базового типа (например, T → RefObj), поэтому повторного разбора generic
                    // не требуется.
                    if (IsMacroDslType(type))
                        AddInheritedCollectionMembers(type, allProps, propLines, xmlDoc);

                    // 2. Методы
                    var allMethods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
                        .Where(m => !m.IsSpecialName && !ignoredMethods.Contains(m.Name))
                        .ToList();

                    // Очистка шума (только при CleanRagNoise): убираются избыточные перегрузки,
                    // отличающиеся от соседнего метода исключительно завершающим CancellationToken.
                    // Метод, существующий ТОЛЬКО с токеном отмены, сохраняется — иначе из справочника
                    // пропал бы единственный способ вызова (например, Prepare(..., token)).
                    if (CleanRagNoise)
                        allMethods = FilterRedundantCancellationTokens(allMethods);

                    // Кириллические методы сразу упорядочиваются по числу параметров:
                    // на этот порядок опирается ранний выход в FindMirroredMethod.
                    // Методы с недоступными метаданными уводятся в конец списка.
                    var ruMethods = allMethods
                        .Where(m => Regex.IsMatch(m.Name, @"\p{IsCyrillic}"))
                        .OrderBy(m =>
                        {
                            try { return m.GetParameters().Length; }
                            catch (Exception) { return int.MaxValue; }
                        })
                        .ToList();

                    // Каждая перегрузка выводится отдельной записью: прежняя группировка
                    // по имени склеивала перегрузки в одну строку с маркером (+N) и отбрасывала
                    // сигнатуры, из-за чего векторный поиск не находил конкретные варианты вызова.
                    var methodEntries = new List<MethodEntry>();

                    // Кириллические методы, уже использованные как алиасы латинских методов.
                    // Во втором проходе выводятся только «осиротевшие» кириллические методы,
                    // чтобы уже показанный алиас не превратился в отдельную строку [RU only].
                    var matchedRuMethods = new HashSet<MethodInfo>();

                    // ПЕРВЫЙ ПРОХОД: латинские методы. Async-методы тоже включаются —
                    // иначе поиск по RunAsync никогда не найдёт описание.
                    foreach (var method in allMethods
                        .Where(m => !Regex.IsMatch(m.Name, @"\p{IsCyrillic}"))
                        .OrderBy(m => m.Name)
                        .ThenBy(m => m.GetParameters().Length))
                    {
                        string summary = xmlDoc.GetMethodSummary(method);

                        // Русскоязычный алиас ищется строго по идентичному тексту <summary>
                        // и совпадающей арности; закреплённый ранее алиас повторно не занимается.
                        var ruMatch = FindMirroredMethod(ruMethods, method, summary, xmlDoc);
                        if (ruMatch != null && matchedRuMethods.Contains(ruMatch))
                            ruMatch = null;

                        if (ruMatch != null)
                            matchedRuMethods.Add(ruMatch);

                        methodEntries.Add(BuildMethodEntry(
                            type,
                            method,
                            summary,
                            ruMatch != null ? ruMatch.Name : null,
                            false,
                            xmlDoc.GetMethodReturns(method),
                            xmlDoc.GetMethodParams(method)));
                    }

                    // ВТОРОЙ ПРОХОД: самостоятельные кириллические методы.
                    // Сюда гарантированно попадают ВСЕ методы чисто кириллических классов
                    // макроязыка (ДиалогВвода, Объект, Подключение, ДиалогВыбораОбъектов и т.п.),
                    // у которых латинских методов нет вовсе, а также русскоязычные члены
                    // двуязычных классов, не нашедшие латинской пары по тексту <summary>.
                    // Порядок сортировки: кириллица по алфавиту — это осмысленная группировка
                    // только для своей языковой ветви, латиница сортируется отдельно выше.
                    var remainingRuMethods = ruMethods
                        .Where(m => !matchedRuMethods.Contains(m))
                        .OrderBy(m => m.Name, StringComparer.CurrentCulture)
                        .ThenBy(m => m.GetParameters().Length)
                        .ToList();

                    foreach (var method in remainingRuMethods)
                    {
                        methodEntries.Add(BuildMethodEntry(
                            type,
                            method,
                            xmlDoc.GetMethodSummary(method),
                            null,
                            true,
                            xmlDoc.GetMethodReturns(method),
                            xmlDoc.GetMethodParams(method)));
                    }

                    // КОНСТРУКТОРЫ КАК ТОЧКИ ВХОДА.
                    // Раньше они отбрасывались фильтром «!IsSpecialName»: рефлексия помечает
                    // конструкторы как special-name наравне с аксессорами свойств. Между тем
                    // генерация модели данных начинается именно с конструктора builder-типа.
                    // Выборка идёт до проверки «есть ли что выводить», иначе типы, у которых
                    // есть только конструкторы, бесследно исчезали из справочника.
                    List<ConstructorEntry> ctorEntries;
                    try
                    {
                        ctorEntries = type
                            .GetConstructors(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                            .Select(c => BuildConstructorEntry(type, c, xmlDoc))
                            .OrderBy(c => c.Parameters.Count)
                            .ToList();
                    }
                    catch (Exception)
                    {
                        ctorEntries = new List<ConstructorEntry>();
                    }

                    // Унаследованные методы завершения сценария (Save/Delete/Build) добавляются
                    // ДО дедупликации, чтобы пройти те же проверки уникальности.
                    if (IsBuilderRelatedType(type))
                        AddInheritedBuilderMembers(type, methodEntries);

                    // Описание типа (класса/интерфейса) из парного XML-файла документации
                    string typeSummary = xmlDoc.GetTypeSummary(type);

                    // Редкие артефакты рефлексии: сборка может вернуть два полностью идентичных
                    // MethodInfo (одинаковые сигнатура, описание и параметры). Такие записи
                    // неразличимы для читателя и для индекса RAG, поэтому дубликаты убираются.
                    var seenSignatures = new HashSet<string>();
                    methodEntries = methodEntries
                        .Where(e => seenSignatures.Add(e.Signature + "\u0000" + e.Summary))
                        .ToList();

                    // Оставшиеся коллизии — методы, различающиеся ТОЛЬКО возвращаемым типом
                    // (в IL это допустимо, в C#-коде невозможно). Полное имя уточняется
                    // возвращаемым типом, чтобы ключ оставался уникальным.
                    var fullNameCounts = new Dictionary<string, int>();
                    foreach (var e in methodEntries)
                    {
                        int c;
                        fullNameCounts.TryGetValue(e.FullName, out c);
                        fullNameCounts[e.FullName] = c + 1;
                    }

                    foreach (var e in methodEntries.Where(e => fullNameCounts[e.FullName] > 1))
                        e.FullName = $"{e.FullName} -> {e.ReturnType}";

                    if (!propLines.Any() && !methodEntries.Any() && !ctorEntries.Any()) continue;

                    // САМОДОСТОЯТЕЛЬНАЯ ШАПКА БЛОКА: полное имя типа, пространство имён и сборка
                    // дублируются в каждом блоке, чтобы сплиттеры RAG не теряли контекст фрагмента.
                    string ns = string.IsNullOrEmpty(type.Namespace) ? "Global" : type.Namespace;

                    // Арность generic-типа в метаданных записывается обратной кавычкой (`1),
                    // которая внутри Markdown-строки в обратных кавычках обрывает code-span.
                    // Нормализация приведена к тому же виду, что и в полных именах методов.
                    string displayFullName = (type.FullName ?? type.Name).Replace('`', '\'');

                    sb.AppendLine($"### [Type] {displayFullName}");
                    sb.AppendLine($"- **Namespace:** `{ns}`");
                    sb.AppendLine($"- **Assembly:** `{dllName}`");

                    // Явно отделяем прикладной Macro DSL от низкоуровневого Core Open API.
                    // Для builder/factory-типов уровень уточняется: это низкоуровневый Core,
                    // но именно через него собирается модель данных, о чём поиск должен знать.
                    string apiLevel = DescribeApiLevel(type);
                    sb.AppendLine($"- **Уровень API:** {apiLevel}");

                    // Вторая, независимая ось: к какой задаче относится тип — администрирование
                    // модели данных или работа с пользовательскими объектами (§5.1).
                    string typeDomain = EmitDomainMarkers ? ResolveTypeDomain(type, domainWarned) : null;
                    if (EmitDomainMarkers)
                    {
                        sb.AppendLine($"- **Область API:** {typeDomain}");
                        domainCounts[typeDomain] = CountOf(domainCounts, typeDomain) + 1;
                        string levelKey = IsMacroDslType(type) ? "macro" : "core";
                        levelCounts[levelKey] = CountOf(levelCounts, levelKey) + 1;
                    }

                    if (!string.IsNullOrEmpty(typeSummary))
                        sb.AppendLine($"- **Описание:** {typeSummary}");

                    // Псевдо-перечисления (`ParameterGroupType`, `ParameterType`) объявлены
                    // классами со статическими полями-значениями. Без строки «Значения»
                    // сценарий создания табличной части 1:N не собирается: читатель не видит,
                    // что в конструктор ParameterGroupBuilder передаётся TableOneToMany.
                    if (EmitDomainMarkers && Array.IndexOf(PseudoEnumTypes, type.FullName ?? type.Name) >= 0)
                    {
                        var pseudoValues = GetEnumValueNames(type);
                        if (pseudoValues.Any())
                            sb.AppendLine($"- **Значения:** {string.Join(", ", pseudoValues.Select(v => "`" + v + "`"))}");
                    }

                    // Типы-конструкторы модели данных описываются в XML-документации без слов
                    // «создать»/«справочник», поэтому в блок добавляются назначение и ключевые
                    // слова: они дают лексический якорь для запросов «создать справочник»,
                    // «создать тип объекта», «добавить параметр».
                    string builderSubject = DescribeBuilderSubject(type);
                    if (!string.IsNullOrEmpty(builderSubject))
                    {
                        sb.AppendLine($"- **Назначение:** Конструктор (builder) для создания и изменения элементов модели данных: {builderSubject}");
                        var builderPhrases = GetBuilderSearchPhrases(type).ToList();
                        sb.AppendLine("- **Ключевые слова:** создать, создание, добавить, новый, конструктор, " +
                                      "сформировать, настроить, изменить, builder, factory, create, add, new, build, generate, " +
                                      $"{builderSubject}, создать {builderSubject}, изменить {builderSubject}" +
                                      (builderPhrases.Any() ? ", " + string.Join(", ", builderPhrases) : ""));

                        // Обратные ссылки в блоке самого builder-типа: без них запрос
                        // «создать тип объекта» упирался в блок ClassObjectBuilder, где не было
                        // ни одного имени метода-точки входа, и пользователь не мог собрать сценарий.
                        List<string> creators;
                        if (builderCreators.TryGetValue(type.Name, out creators) && creators.Any())
                            sb.AppendLine($"- **Как получить (из кода):** {string.Join("; ", creators)}");
                        else if (!builderConsumers.ContainsKey(type.Name))
                        {
                            // У ClassObjectBuilder/UniqueIndexBuilder/LinkGroupBuilder фабрики нет
                            // вовсе — они создаются только оператором new. Молчание в этом месте
                            // читалось как отсутствие сценария, поэтому перечисляются конструкторы.
                            string noFactoryHint = BuildNoFactoryHint(type);
                            if (!string.IsNullOrEmpty(noFactoryHint))
                                sb.AppendLine($"- **Как получить (из кода):** {noFactoryHint}");
                        }

                        List<string> consumers;
                        if (builderConsumers.TryGetValue(type.Name, out consumers) && consumers.Any())
                            sb.AppendLine($"- **Прямое создание:** этот конструктор принимается в {string.Join("; ", consumers)}");
                    }

                    sb.AppendLine();

                    if (propLines.Any())
                    {
                        sb.AppendLine("**Свойства:**");
                        foreach (var p in propLines)
                            sb.AppendLine($"- `{p.TypeName} {p.Name}`{p.RuNote}{p.DocNote}");
                        sb.AppendLine();
                    }

                    // КОНСТРУКТОРЫ ВЫВОДЯТСЯ ПЕРВЫМИ: это точка входа сценария генерации модели,
                    // и именно с неё начинается любой код создания справочника или типа объектов.
                    if (ctorEntries.Any())
                    {
                        foreach (var c in ctorEntries)
                        {
                            // Отдельная секция H4: parse() закрывает секцию на заголовке,
                            // поэтому конструктор становится самостоятельным RAG-чанком.
                            sb.AppendLine($"#### [Constructor] `{c.Signature}`");
                            sb.AppendLine($"- **Полное имя:** `{c.FullName}`");
                            sb.AppendLine($"- **Тип:** `{type.Name}`");

                            // Область API дублируется в чанке конструктора: RAG-сплиттер
                            // не сохраняет родительский заголовок, и без этой строки
                            // конструктор терял привязку к задаче (модель или данные).
                            if (EmitDomainMarkers)
                                sb.AppendLine($"- **Область API:** {ResolveMemberDomain(type, null, domainWarned)}");

                            if (!string.IsNullOrEmpty(c.Summary))
                                sb.AppendLine($"- **Описание:** {c.Summary}");

                            if (!string.IsNullOrEmpty(c.Purpose))
                                sb.AppendLine($"- **Назначение:** {c.Purpose}");

                            // «Возвращает» выводится только для информативного текста <returns>.
                            // У конструктора нет XML-описания результата, если он не документирован,
                            // а синтетическое «Созданный экземпляр X» дублировало заголовок секции
                            // и уже покрыто строкой «**Назначение:**».
                            if (IsInformativeReturns(c.Returns, c.Summary))
                                sb.AppendLine($"- **Возвращает:** {c.Returns}");

                            if (c.Parameters.Any())
                            {
                                sb.AppendLine("- **Параметры:**");
                                foreach (var p in c.Parameters)
                                    sb.AppendLine(FormatParameterLine(p, c.ParamDocs));
                            }

                            string keywordLine = BuildConstructorKeywords(type, c);
                            if (!string.IsNullOrEmpty(keywordLine))
                            {
                                // Конструктор, являющийся единственной точкой входа сценария,
                                // получает id сценария и его фразы: иначе запрос «создать тип
                                // объекта» находит описание типа, но не его конструктор (§10).
                                if (EmitDomainMarkers)
                                    keywordLine = AppendScenarioKeywords(keywordLine, type, ctorEntryPointScenarios);

                                sb.AppendLine($"- **Ключевые слова:** {keywordLine}");
                            }

                            sb.AppendLine();
                        }
                    }

                    if (methodEntries.Any())
                    {
                        foreach (var m in methodEntries)
                        {
                            // H4 важен: parse() закрывает секцию на заголовке,
                            // поэтому каждый метод станет отдельным чанком.
                            sb.AppendLine($"#### [Method] `{m.Signature}`");
                            sb.AppendLine($"- **Полное имя:** `{m.FullName}`");

                            // Тип заново называется внутри каждого чанка: сплиттер RAG
                            // не сохраняет родительский заголовок, и без этой строки
                            // метод терял связь со своим типом при извлечении фрагмента.
                            if (!string.IsNullOrEmpty(typeSummary))
                                sb.AppendLine($"- **Тип:** `{type.Name}` — {typeSummary}");
                            else
                                sb.AppendLine($"- **Тип:** `{type.Name}`");

                            // Область и уровень дублируются в чанке метода: сплиттер RAG
                            // не сохраняет родительский заголовок, поэтому без этих строк
                            // метод терял и задачу, и уровень API при извлечении фрагмента.
                            if (EmitDomainMarkers)
                            {
                                sb.AppendLine($"- **Область API:** {ResolveMemberDomain(type, m.Name, domainWarned)}");
                                sb.AppendLine($"- **Уровень API:** {DescribeApiLevel(type)}");
                            }

                            if (!string.IsNullOrEmpty(m.Summary))
                                sb.AppendLine($"- **Описание:** {m.Summary}");

                            // Только информативный <returns>: пересказ summary дублировал
                            // предыдущую строку «**Описание:**» и не описывал результат.
                            if (IsInformativeReturns(m.Returns, m.Summary))
                                sb.AppendLine($"- **Возвращает:** {m.Returns}");

                            if (m.Parameters.Any())
                            {
                                sb.AppendLine("- **Параметры:**");
                                foreach (var p in m.Parameters)
                                    sb.AppendLine(FormatParameterLine(p, m.ParamDocs));
                            }

                            if (!string.IsNullOrEmpty(m.Alias))
                                sb.AppendLine($"- **Алиасы:** `{m.Alias}`");

                            if (m.RuOnly)
                                sb.AppendLine("- **RU only**");

                            if (m.IsAsync)
                                sb.AppendLine("- **Async**");

                            // Обратная ссылка: если метод возвращает builder-тип, сценарий
                            // продолжается в блоке этого типа. Без строки «Как получить»
                            // точка входа CreateReferenceBuilder не связывалась с ReferenceBuilder.
                            string builderReturn = DescribeBuilderSubject(m.ReturnType);
                            if (!string.IsNullOrEmpty(builderReturn))
                            {
                                sb.AppendLine($"- **Как получить:** этот метод возвращает конструктор `{m.ReturnType}` ({builderReturn}) — далее настраивай и сохраняй его.");

                                // Метод-точка входа тоже получает лексический якорь: без него
                                // запрос «создать справочник» не совпадал с текстом описания
                                // «Создает конструктор для создания справочника» из-за разных
                                // словоформ глагола, и метод находился только по точной сигнатуре.
                                var entryPhrases = GetBuilderSearchPhrases(m.ReturnType).ToList();
                                sb.AppendLine("- **Ключевые слова:** создать, создание, добавить, новый, конструктор, " +
                                              $"builder, factory, create, new, {m.ReturnType}, {builderReturn}" +
                                              (entryPhrases.Any() ? ", " + string.Join(", ", entryPhrases) : ""));
                            }

                            sb.AppendLine();
                        }
                    }
                    sb.AppendLine();
                    addedForDll++;
                }

                // Перечисления выводятся последними в секции сборки: это сохраняет
                // детерминированный порядок и не разрывает существующую последовательность
                // блоков типов, которая уже служит baseline для контроля диффа.
                if (EmitDomainMarkers)
                {
                    var ownEnums = enumCandidates
                        .Where(t => (t.Assembly.GetName().Name + ".dll") == dllName)
                        .ToList();

                    // Псевдо-перечисления объявлены классами, поэтому в кандидаты из сигнатур
                    // они не попадают — добавляются отдельно по явной таблице (§9, п. 1).
                    foreach (var pseudoName in PseudoEnumTypes)
                    {
                        Type pseudo;
                        if (typeIndex.TryGetValue(pseudoName, out pseudo) &&
                            (pseudo.Assembly.GetName().Name + ".dll") == dllName)
                            ownEnums.Add(pseudo);
                    }

                    EmitEnumBlocks(sb, dllName, ownEnums, emittedEnums, xmlDoc);
                }

                Console.WriteLine($"[Успешно] {dllName}: классов: {addedForDll}");
                totalTypes += addedForDll;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Ошибка] {dllName}: {ex.Message}");
            }
        }

        // Валидация выполняется после загрузки всех сборок и ДО записи файла: индекс типов
        // к этому моменту полон, а решение о пропуске сценария ещё можно учесть в выводе.
        if (EmitDomainMarkers)
        {
            ValidateDomainOverrideTables(typeIndex);

            var scenarioProblems = ValidateScenarioMembers(typeIndex);

            // Сценарий, у которого не нашлась точка входа, из вывода удаляется (§1.2):
            // ссылка на несуществующий член API вводила бы RAG в заблуждение сильнее,
            // чем отсутствие сценария. Секция перерисовывается по тому же реестру
            // с явным списком исключений, поэтому остальные сценарии не меняются.
            var droppedScenarios = new HashSet<string>(
                scenarioProblems
                    .Where(p => p.StartsWith("пропущен ", StringComparison.Ordinal))
                    .Select(p => p.Substring("пропущен ".Length).Split(':')[0]),
                StringComparer.Ordinal);

            if (droppedScenarios.Count > 0)
            {
                sb.Remove(scenarioStart, scenarioEnd - scenarioStart);
                // Набор выведенных id пересобирается с нуля: первый проход включал и те
                // сценарии, что теперь отброшены, и манифест описал бы несуществующую секцию.
                knownScenarioIds.Clear();
                skippedScenarios.Clear();
                BuildScenarioSections(sb, skippedScenarios, knownScenarioIds, droppedScenarios);
                Console.WriteLine($" [Сценарии] Сценариев пропущено целиком: {droppedScenarios.Count}");
            }

            Console.WriteLine($" [Область API] Распределение типов — {DomainStructure}: {CountOf(domainCounts, DomainStructure)}, " +
                              $"{DomainObject}: {CountOf(domainCounts, DomainObject)}, " +
                              $"{DomainBoth}: {CountOf(domainCounts, DomainBoth)}");
        }

        string projectDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", ".."));
        string outPath = Path.Combine(projectDir, "tflex_docs_api_smart.md");

        // BOM намеренно не записывается: новейший UTF8Encoding без флага
        // encoderShouldEmitUTF8Identifier не добавляет байты EF BB BF в начало файла.
        // Генератор не должен искусственно создавать проблему: лишний BOM ломает
        // заголовки Markdown, идущие первой строкой, в части инструментов RAG-пайплайна.
        File.WriteAllText(
            outPath,
            sb.ToString(),
            new UTF8Encoding(encoderShouldEmitUTF8Identifier: false)
        );
        Console.WriteLine($"\n Готово! Всего классов: {totalTypes}");
        Console.WriteLine($" Режим очистки шума (CleanRagNoise): {CleanRagNoise}");
        Console.WriteLine($" Файл сохранён: {outPath}");

        // Манифест пишется последним: он описывает то, что уже выведено, поэтому
        // его содержимое не может разойтись с фактическим справочником.
        if (EmitDomainMarkers && EmitScopeManifest)
            WriteScopeManifest(projectDir, domainCounts, levelCounts, knownScenarioIds);
    }

    /// <summary>
    /// Формирует секцию реестра сценариев (## Сценарии (реестр) + блоки ### [Scenario]).
    ///
    /// Вызывается ДО обхода сборок: сценарий — секция верхнего уровня, и внутри блоков
    /// типов он разорвал бы чужие чанки. Порядок строго по id — это требование
    /// детерминированности вывода: реестр объявлен в исходнике вручную, и без сортировки
    /// порядок зависел бы от порядка правок в файле.
    /// </summary>
    /// <param name="sb">Буфер вывода справочника.</param>
    /// <param name="skipped">Коллекция, куда попадут причины отброшенных сценариев.</param>
    /// <param name="knownIds">Множество id выведенных сценариев (нужно для отчёта).</param>
    /// <param name="excluded">
    /// Сценарии, которые нужно не выводить (точка входа не подтверждена рефлексией).
    /// Пусто при первом проходе, когда валидация ещё не выполнялась.
    /// </param>
    private static void BuildScenarioSections(
        StringBuilder sb,
        List<string> skipped,
        HashSet<string> knownIds,
        HashSet<string> excluded)
    {
        sb.AppendLine("## Сценарии (реестр)");
        sb.AppendLine();
        sb.AppendLine(ScenarioRegistryIntro);
        sb.AppendLine();

        foreach (var scenario in ScenarioRegistry.OrderBy(s => s.Id, StringComparer.Ordinal))
        {
            // Сценарий без точки входа или без описания бессмысленен: молчаливый
            // пропуск скрыл бы ошибку в реестре, поэтому причина попадает в консоль.
            if (string.IsNullOrEmpty(scenario.EntryPoint))
            {
                skipped.Add($"{scenario.Id}: не задана точка входа");
                continue;
            }

            // Сценарий, точка входа которого не подтверждена рефлексией, в справочник
            // не попадает: причина уже напечатана валидацией.
            if (excluded != null && excluded.Contains(scenario.Id))
                continue;

            knownIds.Add(scenario.Id);

            sb.AppendLine($"### [Scenario] {scenario.Id}");
            sb.AppendLine($"- **Сценарий:** {scenario.Title}");
            sb.AppendLine($"- **Область API:** {scenario.Domain}");
            sb.AppendLine($"- **Уровень API:** {scenario.Level}");
            sb.AppendLine($"- **Точка входа:** {scenario.EntryPoint}");

            if (!string.IsNullOrEmpty(scenario.Result))
                sb.AppendLine($"- **Результат:** `{scenario.Result}`");

            if (scenario.Next != null && scenario.Next.Any())
                sb.AppendLine($"- **Далее:** {string.Join("; ", scenario.Next)}");

            if (scenario.Keywords != null && scenario.Keywords.Any())
                sb.AppendLine($"- **Ключевые слова:** {string.Join(", ", scenario.Keywords)}");

            // Поле обязательно для пар «тип объекта / объект», «список объектов /
            // объект в списке», «параметр модели / значение параметра» (§3).
            if (scenario.ConfusableWith != null && scenario.ConfusableWith.Any())
                sb.AppendLine($"- **Не путать с:** {string.Join("; ", scenario.ConfusableWith)}");

            sb.AppendLine("- **Проверено по DLL:** да");
            sb.AppendLine();
        }

        sb.AppendLine(ScenarioRegistryFooter);
        sb.AppendLine();
    }

    /// <summary>
    /// Определяет область API для типа: `structure`, `object` или `both` (§5.1).
    ///
    /// Порядок применения строго задан: явная таблица → правило по пространству имён →
    /// правило по имени → `both`. Именно такой порядок не даёт правилу по имени
    /// («Object» в `ClassObject`) перебить явную таблицу, где `ClassObject` — это structure.
    /// </summary>
    /// <param name="type">Тип, для которого определяется домен.</param>
    /// <param name="warned">Множество уже выданных предупреждений (без дублей).</param>
    /// <returns>Одно из значений DomainStructure/DomainObject/DomainBoth.</returns>
    private static string ResolveTypeDomain(Type type, HashSet<string> warned)
    {
        if (type == null)
            return DomainBoth;

        string fullName = type.FullName ?? type.Name;

        string domain;
        if (ExplicitTypeDomains.TryGetValue(fullName, out domain))
            return domain;

        // Ключ таблицы для вложенных типов записан через имя сборки, а полное имя
        // вложенного типа содержит «+»: ClassObjectBuilder+ParameterGroupCollection.
        // Домен объявляющего типа — правильный ответ и для вложенного типа.
        if (type.DeclaringType != null &&
            ExplicitTypeDomains.TryGetValue(type.DeclaringType.FullName ?? type.DeclaringType.Name, out domain))
            return domain;

        string ns = type.Namespace ?? "";

        if (StructureNamespacePrefixes.Any(p => ns == p || ns.StartsWith(p + ".", StringComparison.Ordinal)))
            return DomainStructure;

        if (ObjectNamespacePrefixes.Any(p => ns == p || ns.StartsWith(p + ".", StringComparison.Ordinal)))
            return DomainObject;

        // Правило по имени — только если namespace ответа не дал.
        string name = SimpleTypeName(fullName) ?? type.Name;
        if (name.Contains("Builder") || name.Contains("Factory") ||
            name.Contains("Structure") || name.Contains("Schema") || name.Contains("Metadata"))
            return DomainStructure;

        if (name.Contains("Object") || name.Contains("Reference") || name.Contains("Parameter") ||
            name.Contains("Link") || name.Contains("Revision") || name.Contains("File"))
            return DomainObject;

        // Неопределённый тип осознанно попадает в `both`, а не в `object`:
        // при фильтрации по области ничего не должно потеряться.
        if (warned != null && WarnUnknownTypeDomains && warned.Add(fullName))
            Console.WriteLine($" [Область API] Тип не классифицирован явно, используется both: {fullName}");

        return DomainBoth;
    }

    /// <summary>
    /// Определяет область API для члена типа (§5.2).
    ///
    /// Домен члена по умолчанию равен домену объявляющего типа; переопределяется
    /// явной таблицей `Type.Member` и правилом по имени. Правило по имени применяется
    /// ПОСЛЕ явной таблицы, иначе `ClassObject` внутри имени метода перебил бы
    /// зафиксированный домен `Reference.CreateReferenceObject`.
    /// </summary>
    /// <param name="type">Объявляющий тип члена.</param>
    /// <param name="memberName">Имя метода, свойства или конструктора (null для конструктора).</param>
    /// <param name="warned">Множество уже выданных предупреждений (без дублей).</param>
    /// <returns>Одно из значений DomainStructure/DomainObject/DomainBoth.</returns>
    private static string ResolveMemberDomain(Type type, string memberName, HashSet<string> warned)
    {
        if (type == null)
            return DomainBoth;

        string typeFullName = type.FullName ?? type.Name;
        string baseDomain = ResolveTypeDomain(type, warned);

        if (string.IsNullOrEmpty(memberName))
            return baseDomain;

        string key = typeFullName + "." + memberName;
        string domain;
        if (ExplicitMemberDomains.TryGetValue(key, out domain))
            return domain;

        // Явная таблица для базового типа действует и на унаследованный член:
        // Reference.CreateReferenceObject объявлен именно на Reference, но правило
        // «домен типа» обязано работать и для переопределений в наследниках.
        for (Type baseType = type.BaseType; baseType != null; baseType = baseType.BaseType)
        {
            string baseKey = (baseType.FullName ?? baseType.Name) + "." + memberName;
            if (ExplicitMemberDomains.TryGetValue(baseKey, out domain))
                return domain;
        }

        if (memberName.IndexOf("Structure", StringComparison.Ordinal) >= 0 ||
            memberName.IndexOf("Schema", StringComparison.Ordinal) >= 0 ||
            memberName.IndexOf("ClassObject", StringComparison.Ordinal) >= 0 ||
            memberName.IndexOf("ParameterInfo", StringComparison.Ordinal) >= 0 ||
            memberName.IndexOf("LinkGroup", StringComparison.Ordinal) >= 0 ||
            memberName.IndexOf("UniqueIndex", StringComparison.Ordinal) >= 0)
            return DomainStructure;

        return baseDomain;
    }

    /// <summary>
    /// Проверяет, что каждая запись явных таблиц домена существует в реальных сборках.
    ///
    /// Требование §5.2, п. 5: явная таблица приоритетнее всех правил, поэтому ошибка
    /// в её ключе тихо «повисала» бы в коде — правило никогда не срабатывало бы,
    /// и никто об этом не узнал бы. Проверка печатает предупреждение по каждой
    /// несуществующей записи.
    /// </summary>
    /// <param name="typeIndex">Индекс «полное имя типа → Type» по всем загруженным сборкам.</param>
    private static void ValidateDomainOverrideTables(Dictionary<string, Type> typeIndex)
    {
        int missing = 0;

        foreach (var pair in ExplicitTypeDomains)
        {
            if (!typeIndex.ContainsKey(pair.Key))
            {
                Console.WriteLine($" [Область API] Предупреждение: запись таблицы типов не найдена — {pair.Key}");
                missing++;
            }
        }

        foreach (var pair in ExplicitMemberDomains)
        {
            int lastDot = pair.Key.LastIndexOf('.');
            if (lastDot <= 0)
                continue;

            string typeName = pair.Key.Substring(0, lastDot);
            string memberName = pair.Key.Substring(lastDot + 1);

            Type owner;
            if (!typeIndex.TryGetValue(typeName, out owner))
            {
                Console.WriteLine($" [Область API] Предупреждение: запись таблицы членов ссылается на неизвестный тип — {pair.Key}");
                missing++;
                continue;
            }

            if (!HasPublicMember(owner, memberName))
            {
                Console.WriteLine($" [Область API] Предупреждение: запись таблицы членов не найдена — {pair.Key}");
                missing++;
            }
        }

        Console.WriteLine($" [Область API] Проверка явных таблиц: записей {ExplicitTypeDomains.Count + ExplicitMemberDomains.Count}, не найдено {missing}");
    }

    /// <summary>
    /// Проверяет существование публичного члена у типа: метода, свойства или поля.
    /// Просматриваются и унаследованные члены — таблица может ссылаться на член,
    /// объявленный в базовом классе.
    /// </summary>
    private static bool HasPublicMember(Type type, string memberName)
    {
        const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance |
                                   BindingFlags.Static | BindingFlags.FlattenHierarchy;

        try
        {
            // Конструктор помечается в реестре как "#ctor" и через GetMethods не находится:
            // рефлексия отдаёт его только отдельным списком конструкторов.
            if (memberName == null || memberName == "#ctor" || memberName.EndsWith(".#ctor", StringComparison.Ordinal))
                return type.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Length > 0;

            if (type.GetMethods(flags).Any(m => m.Name == memberName))
                return true;

            if (type.GetProperties(flags).Any(p => p.Name == memberName))
                return true;

            if (type.GetFields(flags).Any(f => f.Name == memberName))
                return true;
        }
        catch (Exception)
        {
            // Часть типов сторонних сборок не отдаёт метаданные: такой член
            // считается непроверяемым, а не отсутствующим.
            return true;
        }

        return false;
    }

    /// <summary>
    /// Проверяет все ссылки реестра сценариев рефлексией по загруженным сборкам и
    /// возвращает список сценариев, у которых не найдена ТОЧКА ВХОДА (§4).
    ///
    /// Разделение важно: отсутствие продолжения («Далее») — не повод выбрасывать
    /// сценарий, у него достаточно убрать один пункт. Пропуск целиком оправдан только
    /// тогда, когда сценарий не с чего начать.
    /// </summary>
    /// <param name="typeIndex">Индекс «полное имя типа → Type» по всем загруженным сборкам.</param>
    /// <returns>Список причин пропуска сценариев; пустой, если пропусков нет.</returns>
    private static List<string> ValidateScenarioMembers(Dictionary<string, Type> typeIndex)
    {
        var skippedScenarios = new List<string>();
        int checkedRefs = 0;
        int missingRefs = 0;

        foreach (var scenario in ScenarioRegistry.OrderBy(s => s.Id, StringComparer.Ordinal))
        {
            if (scenario.Members == null || !scenario.Members.Any())
                continue;

            bool entryPointFound = false;

            foreach (var member in scenario.Members)
            {
                checkedRefs++;

                Type owner;
                if (!typeIndex.TryGetValue(member.TypeFullName, out owner) || !HasPublicMember(owner, member.Member))
                {
                    missingRefs++;
                    if (!entryPointFound && IsEntryPoint(scenario, member))
                    {
                        // Точка входа не найдена — сценарий выводить нельзя: он отправил бы
                        // читателя в несуществующий член API.
                        skippedScenarios.Add($"пропущен {scenario.Id}: не найден {member.Display}");
                    }
                    else
                    {
                        skippedScenarios.Add($"уточнение {scenario.Id}: не найден {member.Display}");
                    }
                    continue;
                }

                if (IsEntryPoint(scenario, member))
                    entryPointFound = true;
            }
        }

        Console.WriteLine($" [Сценарии] всего: {ScenarioRegistry.Length}, проверено: {checkedRefs} ссылка, пропущено: {missingRefs}");
        foreach (var line in skippedScenarios)
            Console.WriteLine($" [Сценарии] {line}");

        return skippedScenarios;
    }

    /// <summary>
    /// Определяет, является ли проверяемый член точкой входа сценария.
    ///
    /// Точка входа — это первый член списка Members без имени (конструктор) либо член,
    /// названный в строке «Точка входа» сценария. Обе эвристики применяются вместе:
    /// у сценария создания модели данных точка входа — всегда конструктор builder-типа,
    /// а у сценария работы с данными — именованный метод.
    /// </summary>
    private static bool IsEntryPoint(ScenarioDefinition scenario, ScenarioMember member)
    {
        if (member.Member == null)
            return scenario.Members.Length > 0 && ReferenceEquals(scenario.Members[0], member);

        if (string.IsNullOrEmpty(scenario.EntryPoint))
            return false;

        // Имя члена ищется как подстрока в тексте точки входа: текст содержит
        // проверенные сигнатуры, поэтому совпадение имени означает, что член там упомянут.
        return scenario.EntryPoint.IndexOf("." + member.Member + "(", StringComparison.Ordinal) >= 0 ||
               scenario.EntryPoint.IndexOf("." + member.Member + "`", StringComparison.Ordinal) >= 0;
    }

    /// <summary>
    /// Формирует список проверяемых ссылок сценария для отчёта о валидации.
    /// </summary>
    private static string DescribeScenarioMembers(ScenarioDefinition scenario)
    {
        if (scenario.Members == null || !scenario.Members.Any())
            return "нет ссылок";

        return string.Join(", ", scenario.Members.Select(m => m.Display));
    }

    /// <summary>
    /// Проверка домен-таблиц и сценариев выполняется только при включённой оси:
    /// при EmitDomainMarkers == false генератор должен работать как прежде,
    /// в том числе не тратить время на обход типов и не печатать новые строки.
    /// </summary>
    private static readonly bool WarnUnknownTypeDomains = false;

    /// <summary>
    /// Типы-перечисления платформы, которые в сборке объявлены классами со статическими
    /// полями-значениями (`ParameterGroupType`, `ParameterType`), но по смыслу являются
    /// перечислениями: их значения используются как `ParameterGroupType.TableOneToMany`.
    ///
    /// Без них сценарий создания табличной части 1:N не собирается: в справочнике нет
    /// ни одного описания значения `TableOneToMany`, и читатель не может понять,
    /// что передать в конструктор `ParameterGroupBuilder`.
    /// </summary>
    private static readonly string[] PseudoEnumTypes = {
        "TFlex.DOCs.Model.Structure.ParameterGroupType",
        "TFlex.DOCs.Model.Structure.ParameterType"
    };

    /// <summary>
    /// Возвращает список значений перечисления в порядке объявления.
    ///
    /// Для настоящего `enum` используется `Enum.GetNames`. Для псевдо-перечислений
    /// (`ParameterGroupType`, `ParameterType`) значения читаются из публичных статических
    /// полей самого типа и сортируются по `MetadataToken`: рефлексия отдаёт такие поля
    /// в порядке хеш-таблицы, а токен метаданных сохраняет порядок объявления в исходнике.
    /// </summary>
    /// <param name="type">Тип-перечисление или псевдо-перечисление.</param>
    /// <returns>Значения в порядке объявления; пустой список, если тип не является перечислением.</returns>
    private static List<string> GetEnumValueNames(Type type)
    {
        var values = new List<string>();
        if (type == null)
            return values;

        if (type.IsEnum)
        {
            try
            {
                values.AddRange(Enum.GetNames(type));
            }
            catch (Exception ex)
            {
                // Перечисление из сборки, метаданные которой недоступны, не должно
                // обрушивать обработку всей сборки: блок просто не выводится.
                Console.WriteLine($" [Перечисления] Пропущено {type.FullName}: {ex.Message}");
            }

            return values;
        }

        if (Array.IndexOf(PseudoEnumTypes, type.FullName ?? type.Name) < 0)
            return values;

        try
        {
            values.AddRange(type
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                .Where(f => f.FieldType == type)
                .OrderBy(f => f.MetadataToken)
                .Select(f => f.Name));
        }
        catch (Exception)
        {
            // Метаданные недоступны — значений нет, блок остаётся без строки «Значения».
        }

        return values;
    }

    /// <summary>
    /// Выводит блоки найденных перечислений после блоков типов своей сборки (§9).
    ///
    /// Зачем это нужно: генератор отбирает только классы и интерфейсы, поэтому
    /// `ParameterGroupType.TableOneToMany` и `ParameterType.NVarChar` в справочнике
    /// не описаны вовсе, и сценарий создания табличной части не собирается.
    /// Блоки печатаются последними в секции сборки — так порядок остаётся детерминированным
    /// и не разрывает уже существующую последовательность блоков типов.
    /// </summary>
    /// <param name="sb">Буфер вывода.</param>
    /// <param name="dllName">Имя обрабатываемой сборки (для строки Assembly).</param>
    /// <param name="candidates">Типы-кандидаты, собранные из сигнатур отобранных членов.</param>
    /// <param name="alreadyEmitted">Полные имена уже выведенных перечислений (без дублей между сборками).</param>
    /// <param name="xmlDoc">Поставщик XML-документации (может быть null).</param>
    /// <returns>Количество выведенных блоков перечислений.</returns>
    private static int EmitEnumBlocks(
        StringBuilder sb,
        string dllName,
        IEnumerable<Type> candidates,
        HashSet<string> alreadyEmitted,
        XmlDocProvider xmlDoc)
    {
        int emitted = 0;

        foreach (var enumType in candidates
            .Where(t => t != null)
            // Только настоящие перечисления: псевдо-перечисления (`ParameterGroupType`,
            // `ParameterType`) объявлены классами и уже выводятся обычным блоком типа,
            // поэтому их значения добавляются в этот существующий блок, а не дублируются
            // вторым заголовком `### [Type]` с тем же полным именем.
            .Where(t => t.IsEnum)
            .GroupBy(t => t.FullName ?? t.Name, StringComparer.Ordinal)
            .Select(g => g.First())
            .OrderBy(t => t.FullName ?? t.Name, StringComparer.Ordinal))
        {
            string fullName = enumType.FullName ?? enumType.Name;
            if (!alreadyEmitted.Add(fullName))
                continue;

            var values = GetEnumValueNames(enumType);
            // Перечисление без значений в справочник не попадает: блок с пустой строкой
            // «Значения» бесполезен для RAG и только засоряет выдачу.
            if (!values.Any())
                continue;

            sb.AppendLine($"### [Type] {fullName}");
            sb.AppendLine($"- **Namespace:** `{enumType.Namespace}`");
            sb.AppendLine($"- **Assembly:** `{dllName}`");
            sb.AppendLine($"- **Уровень API:** {DescribeApiLevel(enumType)}");
            sb.AppendLine($"- **Область API:** {ResolveTypeDomain(enumType, null)}");

            string summary = xmlDoc == null ? null : xmlDoc.GetTypeSummary(enumType);
            sb.AppendLine(!string.IsNullOrEmpty(summary)
                ? $"- **Описание:** {summary}"
                : $"- **Описание:** Перечисление значений для работы с API T-FLEX DOCs: {enumType.Name}.");

            // Значения — в порядке объявления, без XML-описаний (§9, п. 4).
            sb.AppendLine($"- **Значения:** {string.Join(", ", values.Select(v => "`" + v + "`"))}");
            sb.AppendLine();

            emitted++;
        }

        return emitted;
    }

    /// <summary>
    /// Определяет уровень API (`Macro DSL` / `Core Open API`) в едином виде для блоков
    /// типов и перечислений: логика уровня не должна дублироваться и расходиться.
    /// </summary>
    private static string DescribeApiLevel(Type type)
    {
        if (IsMacroDslType(type))
            return "Macro DSL (доступно внутри MacroProvider)";

        return IsBuilderRelatedType(type)
            ? "Core Open API (генерация модели данных)"
            : "Core Open API";
    }

    /// <summary>
    /// Добавляет в набор перечисление, если тип (или его generic-аргумент, или элемент
    /// массива) является перечислением и относится к обрабатываемым сборкам (§9, п. 2).
    ///
    /// Системные и шумовые пространства имён отбрасываются: `System.DayOfWeek` в справочнике
    /// API платформы не нужен и только раздувает выдачу.
    /// </summary>
    private static void AddEnumCandidate(HashSet<Type> target, Type candidate, string[] skipped)
    {
        if (target == null || candidate == null)
            return;

        if (candidate.IsArray)
            candidate = candidate.GetElementType();

        if (candidate == null)
            return;

        if (candidate.IsGenericType)
        {
            foreach (var argument in candidate.GetGenericArguments())
                AddEnumCandidate(target, argument, skipped);
            return;
        }

        if (!candidate.IsEnum || !candidate.IsPublic)
            return;

        // Generic-параметр с ограничением `where T : Enum` тоже отвечает IsEnum == true,
        // но это не тип-перечисление: Enum.GetNames на нём падает с «Должен быть задан
        // тип Enum». Такие параметры в справочник значений не попадают.
        if (candidate.IsGenericParameter || candidate.ContainsGenericParameters)
            return;

        string ns = candidate.Namespace ?? "";
        if (ns.StartsWith("System", StringComparison.Ordinal) || ns.Length == 0)
            return;

        if (skipped != null && skipped.Any(s => ns == s || ns.StartsWith(s + ".", StringComparison.Ordinal)))
            return;

        target.Add(candidate);
    }

    /// <summary>
    /// Собирает перечисления, реально встречающиеся в сигнатурах отобранных членов типа:
    /// типы свойств, возвращаемые типы и типы параметров методов и конструкторов (§9, п. 1).
    /// </summary>
    private static void CollectReferencedEnums(
        Type type,
        HashSet<Type> target,
        string[] skippedNamespaces)
    {
        if (type == null)
            return;

        const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance |
                                   BindingFlags.Static | BindingFlags.DeclaredOnly;

        try
        {
            foreach (var property in type.GetProperties(flags))
            {
                AddEnumCandidate(target, property.PropertyType, skippedNamespaces);
                foreach (var index in property.GetIndexParameters())
                    AddEnumCandidate(target, index.ParameterType, skippedNamespaces);
            }

            foreach (var method in type.GetMethods(flags))
            {
                AddEnumCandidate(target, method.ReturnType, skippedNamespaces);
                foreach (var parameter in method.GetParameters())
                    AddEnumCandidate(target, parameter.ParameterType, skippedNamespaces);
            }

            foreach (var ctor in type.GetConstructors(flags))
            {
                foreach (var parameter in ctor.GetParameters())
                    AddEnumCandidate(target, parameter.ParameterType, skippedNamespaces);
            }
        }
        catch (Exception)
        {
            // Часть типов не отдаёт метаданные: перечисления такого типа просто
            // не попадут в справочник, уже выведенные блоки от этого не пострадают.
        }
    }

    /// <summary>
    /// Определяет, является ли тип точкой входа сценария создания модели данных:
    /// ключ — полное имя типа, значение — первый сценарий реестра, чья точка входа
    /// начинается с конструктора этого типа.
    ///
    /// Нужно для §10: конструктор-единственная-точка-входа получает в «Ключевые слова»
    /// id сценария и его фразы, иначе запрос «создать тип объекта» находит только
    /// описание типа, но не его конструктор.
    /// </summary>
    private static Dictionary<string, ScenarioDefinition> BuildCtorEntryPointScenarios()
    {
        var map = new Dictionary<string, ScenarioDefinition>(StringComparer.Ordinal);

        foreach (var scenario in ScenarioRegistry)
        {
            if (scenario.Members == null || scenario.Members.Length == 0)
                continue;

            var first = scenario.Members[0];
            if (first.Member != null)
                continue;

            if (!map.ContainsKey(first.TypeFullName))
                map[first.TypeFullName] = scenario;
        }

        return map;
    }

    /// <summary>
    /// Формирует ключевые слова конструктора-точки-входа: к обычным словам добавляются
    /// id сценария и его ключевые фразы (§10).
    /// </summary>
    private static string AppendScenarioKeywords(
        string keywordLine,
        Type type,
        Dictionary<string, ScenarioDefinition> ctorScenarios)
    {
        if (type == null || ctorScenarios == null)
            return keywordLine;

        ScenarioDefinition scenario;
        if (!ctorScenarios.TryGetValue(type.FullName ?? type.Name, out scenario))
            return keywordLine;

        var extra = new List<string> { scenario.Id };
        if (scenario.Keywords != null)
            extra.AddRange(scenario.Keywords.Where(k => !string.IsNullOrEmpty(k)));

        string addition = string.Join(", ", extra);
        if (string.IsNullOrEmpty(keywordLine))
            return "- **Ключевые слова:** " + addition;

        return keywordLine + ", " + addition;
    }

    /// <summary>
    /// Записывает компактный манифест области видимости `tflex_docs_api_scope.json`
    /// рядом со справочником (§11).
    ///
    /// Манифест содержит только сценарии, явные переопределения и счётчики — без путей,
    /// времени и полной карты типов, поэтому два прогона дают побайтово одинаковый файл.
    /// JSON собирается вручную: это избавляет от зависимости от сериализатора и от
    /// неопределённости в порядке ключей.
    /// </summary>
    /// <param name="projectDir">Каталог рядом со справочником.</param>
    /// <param name="domainCounts">Итоговое распределение типов по областям API.</param>
    /// <param name="levelCounts">Итоговое распределение типов по уровням API.</param>
    private static void WriteScopeManifest(
        string projectDir,
        Dictionary<string, int> domainCounts,
        Dictionary<string, int> levelCounts,
        HashSet<string> emittedScenarioIds)
    {
        var json = new StringBuilder();
        json.AppendLine("{");
        json.AppendLine("  \"version\": 1,");
        json.AppendLine($"  \"document_domain\": \"{DomainBoth}\",");
        json.AppendLine("  \"scenarios\": [");

        // В манифест попадают только фактически выведенные сценарии: иначе он описывал бы
        // секцию, которой в справочнике нет, и расхождение было бы незаметно.
        var scenarios = ScenarioRegistry
            .Where(s => emittedScenarioIds == null || emittedScenarioIds.Contains(s.Id))
            .OrderBy(s => s.Id, StringComparer.Ordinal)
            .ToList();

        for (int i = 0; i < scenarios.Count; i++)
        {
            var scenario = scenarios[i];
            string comma = i == scenarios.Count - 1 ? "" : ",";
            json.AppendLine($"    {{ \"id\": \"{scenario.Id}\", \"domain\": \"{scenario.Domain}\", \"level\": \"{scenario.Level}\" }}{comma}");
        }

        json.AppendLine("  ],");
        json.AppendLine("  \"overrides\": {");

        var overrides = ExplicitMemberDomains
            .OrderBy(p => p.Key, StringComparer.Ordinal)
            .ToList();

        for (int i = 0; i < overrides.Count; i++)
        {
            string comma = i == overrides.Count - 1 ? "" : ",";
            json.AppendLine($"    \"{overrides[i].Key}\": \"{overrides[i].Value}\"{comma}");
        }

        json.AppendLine("  },");
        json.AppendLine("  \"counts\": {");
        json.AppendLine($"    \"structure\": {CountOf(domainCounts, DomainStructure)},");
        json.AppendLine($"    \"object\": {CountOf(domainCounts, DomainObject)},");
        json.AppendLine($"    \"both\": {CountOf(domainCounts, DomainBoth)},");
        json.AppendLine($"    \"macro\": {CountOf(levelCounts, "macro")},");
        json.AppendLine($"    \"core\": {CountOf(levelCounts, "core")}");
        json.AppendLine("  }");
        json.AppendLine("}");

        string scopePath = Path.Combine(projectDir, "tflex_docs_api_scope.json");
        File.WriteAllText(
            scopePath,
            json.ToString(),
            new UTF8Encoding(encoderShouldEmitUTF8Identifier: false)
        );

        Console.WriteLine($" Манифест области API сохранён: {scopePath}");
    }

    /// <summary>Безопасно читает счётчик из словаря: отсутствующий ключ означает ноль.</summary>
    private static int CountOf(Dictionary<string, int> counts, string key)
    {
        if (counts == null)
            return 0;

        int value;
        return counts.TryGetValue(key, out value) ? value : 0;
    }

    /// <summary>
    /// Полные имена типов, на которые ссылается реестр сценариев или явные таблицы домена.
    ///
    /// Такие типы освобождаются от суффиксных фильтров имени (`Collection`, `Dto`, `Xml`…):
    /// `TFlex.DOCs.Model.Parameters.ParameterCollection` отсекался по суффиксу `Collection`,
    /// из-за чего сценарии чтения и заполнения параметра ссылались на отсутствующий в
    /// справочнике тип. Ссылка реестра — более сильный признак нужности, чем суффикс имени.
    /// </summary>
    private static readonly HashSet<string> ScenarioReferencedTypeNames = BuildScenarioReferencedTypeNames();

    /// <summary>Собирает имена типов, упомянутых в реестре сценариев и явных таблицах домена.</summary>
    private static HashSet<string> BuildScenarioReferencedTypeNames()
    {
        var names = new HashSet<string>(StringComparer.Ordinal);

        foreach (var key in ExplicitTypeDomains.Keys)
            names.Add(key);

        foreach (var scenario in ScenarioRegistry)
        {
            if (scenario.Members == null)
                continue;

            foreach (var member in scenario.Members)
                names.Add(member.TypeFullName);
        }

        return names;
    }

    /// <summary>
    /// Проверяет, упомянут ли тип в реестре сценариев или в явных таблицах домена.
    /// Для вложенных типов проверяется и объявляющий тип.
    /// </summary>
    private static bool IsScenarioReferencedType(Type type)
    {
        if (type == null)
            return false;

        if (ScenarioReferencedTypeNames.Contains(type.FullName ?? type.Name))
            return true;

        return type.DeclaringType != null && IsScenarioReferencedType(type.DeclaringType);
    }

    /// <summary>
    /// Исключает перегрузки, отличающиеся от другого метода этого же типа только
    /// завершающим параметром CancellationToken.
    ///
    /// Ключевое требование безопасности: методы, существующие ТОЛЬКО с токеном отмены,
    /// сохраняются. Перегрузка отсекается лишь тогда, когда рядом уже есть полностью
    /// совпадающая по имени и предшествующим параметрам версия без токена. Иначе
    /// из справочника исчез бы единственный способ вызова (например, Prepare(..., token))
    /// вместе с его описанием из XML-документации.
    /// </summary>
    /// <param name="methods">Полный список методов типа, полученный через Reflection.</param>
    /// <returns>Список без избыточных токен-перегрузок в исходном порядке следования.</returns>
    private static List<MethodInfo> FilterRedundantCancellationTokens(List<MethodInfo> methods)
    {
        var result = new List<MethodInfo>(methods.Count);

        foreach (var method in methods)
        {
            if (HasRedundantCancellationToken(method, methods))
                continue;

            result.Add(method);
        }

        return result;
    }

    /// <summary>
    /// Проверяет, есть ли в наборе метод с тем же именем и идентичными типами параметров,
    /// но без завершающего CancellationToken. Наличие такой версии делает текущую
    /// перегрузку избыточной для справочника RAG.
    /// </summary>
    /// <param name="method">Проверяемая перегрузка.</param>
    /// <param name="methods">Все методы типа, среди которых ищется упрощённая версия.</param>
    /// <returns>true, если перегрузка дублирует версию без токена отмены.</returns>
    private static bool HasRedundantCancellationToken(MethodInfo method, List<MethodInfo> methods)
    {
        ParameterInfo[] parameters;

        // Метаданные конкретной перегрузки могут быть недоступны (например, из-за
        // отсутствующей зависимости). В этом случае метод НЕ отсекается.
        try
        {
            parameters = method.GetParameters();
        }
        catch (Exception)
        {
            return false;
        }

        // Токен учитывается только в последней позиции: именно вариация "метод + token"
        // является распространённым шумом асинхронных API.
        if (parameters.Length == 0 || !IsCancellationToken(parameters[parameters.Length - 1].ParameterType))
            return false;

        int expectedCount = parameters.Length - 1;

        foreach (var candidate in methods)
        {
            if (ReferenceEquals(candidate, method) || candidate.Name != method.Name)
                continue;

            ParameterInfo[] candidateParameters;

            try
            {
                candidateParameters = candidate.GetParameters();
            }
            catch (Exception)
            {
                continue;
            }

            if (candidateParameters.Length != expectedCount)
                continue;

            // Все параметры, кроме завершающего токена, должны совпадать по типам и порядку.
            bool same = true;
            for (int i = 0; i < expectedCount; i++)
            {
                // Если у кандидата на этом месте тоже токен, он не является упрощённой версией.
                if (IsCancellationToken(candidateParameters[i].ParameterType))
                {
                    same = false;
                    break;
                }

                if (!string.Equals(GetTypeSignature(candidateParameters[i].ParameterType), GetTypeSignature(parameters[i].ParameterType), StringComparison.Ordinal))
                {
                    same = false;
                    break;
                }
            }

            if (same)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Возвращает текстовую подпись типа для сравнения сигнатур перегрузок.
    ///
    /// Сравнение через Type-идентичность здесь неприменимо: у каждого generic-метода
    /// собственный экземпляр типа-параметра (T в RunWithSaveSet&lt;T&gt;(...) и в
    /// RunWithSaveSet&lt;T&gt;(..., token) — это два разных объекта Type), а в поставке
    /// T-FLEX встречаются одноимённые типы из разных сборок. Текстовая подпись
    /// (FullName, а для generic-параметров — их краткое имя) даёт устойчивое сравнение.
    /// </summary>
    private static string GetTypeSignature(Type type)
    {
        if (type == null)
            return string.Empty;

        // Для параметра generic-метода FullName равен null — используется краткое имя ("T").
        return type.FullName ?? type.Name;
    }

    /// <summary>
    /// Определяет, является ли тип токеном отмены. Сравнение идёт по имени типа,
    /// а не только по строгому равенству: в поставке T-FLEX могут оказаться
    /// несовместимые копии System.Threading.CancellationToken из разных сборок.
    /// </summary>
    private static bool IsCancellationToken(Type parameterType)
    {
        if (parameterType == null)
            return false;

        if (parameterType.FullName == "System.Threading.CancellationToken")
            return true;

        // Nullable&lt;CancellationToken&gt; и прочие копии типа из другой сборки.
        Type underlying = Nullable.GetUnderlyingType(parameterType);
        if (underlying != null && underlying.FullName == "System.Threading.CancellationToken")
            return true;

        return parameterType.Name == "CancellationToken";
    }

    /// <summary>
    /// Элемент списка свойств класса для итогового Markdown.
    /// Хранит уже нормализованное имя типа, имя свойства и готовые текстовые
    /// примечания (русскоязычный алиас и описание из XML), чтобы форматирование
    /// вывода не смешивалось со сбором метаданных через Reflection.
    /// </summary>
    private sealed class PropEntry
    {
        /// <summary>Читаемое имя C#-типа свойства (например, "List&lt;string&gt;", "void", "int").</summary>
        public string TypeName;

        /// <summary>Имя свойства в метаданных сборки.</summary>
        public string Name;

        /// <summary>Примечание с русскоязычным алиасом: " [RU: ИмяАлиаса]" либо " [RU only]", иначе пусто.</summary>
        public string RuNote;

        /// <summary>Описание из тега summary XML-документации: " — текст" либо пусто.</summary>
        public string DocNote;
    }

    /// <summary>
    /// Элемент отдельного метода класса для итогового Markdown.
    ///
    /// В отличие от прежнего строкового представления, каждая перегрузка метода
    /// (и каждый async-вариант) хранится самостоятельной записью: это позволяет
    /// вывести метод отдельной секцией H4 и тем самым превратить его в отдельный
    /// чанк при разборе справочника RAG-конвейером.
    /// </summary>
    private sealed class MethodEntry
    {
        /// <summary>Читаемая сигнатура: "RefObj FindObject(string referenceName, ...)".</summary>
        public string Signature;

        /// <summary>Полное имя метода с типами параметров для точного поиска и дедупликации.</summary>
        public string FullName;

        /// <summary>
        /// Простое имя метода без типа и параметров.
        /// Нужно для классификации области API члена (§5.2): правила таблицы
        /// переопределений заданы парой «полное имя типа + имя члена».
        /// </summary>
        public string Name;

        /// <summary>Описание из тега summary XML-документации; пусто, если описания нет.</summary>
        public string Summary;

        /// <summary>Имя русскоязычного алиаса, найденного зеркалированием по summary; иначе null.</summary>
        public string Alias;

        /// <summary>Список параметров в виде "string referenceName" для построчного вывода.</summary>
        public List<string> Parameters;

        /// <summary>Признак метода, существующего только в русскоязычном API (маркер «RU only»).</summary>
        public bool RuOnly;

        /// <summary>Признак асинхронного варианта метода (суффикс Async).</summary>
        public bool IsAsync;

        /// <summary>Нормализованное имя возвращаемого типа — для различения метаданных-коллизий.</summary>
        public string ReturnType;

        /// <summary>Описание возвращаемого значения из тега &lt;returns&gt;; пусто, если его нет.</summary>
        public string Returns;

        /// <summary>
        /// Описания параметров из тегов &lt;param name="..."&gt;: имя параметра → текст.
        /// Нужны, чтобы строка параметра в чанке содержала предметные слова
        /// («folder — Папка каталога справочников») и находилась по смыслу.
        /// </summary>
        public Dictionary<string, string> ParamDocs;
    }

    /// <summary>
    /// Формирует запись метода для вывода: нормализует типы через FormatTypeName
    /// (устраняя артефакты рефлексии вида List`1, Nullable`1 и корректно печатая void),
    /// раздельно собирает читаемую сигнатуру, полное имя с типами параметров,
    /// список параметров и признаки алиаса/RU only/Async.
    /// </summary>
    /// <param name="type">Объявляющий тип — используется для полного имени метода.</param>
    /// <param name="method">Метод, полученный через Reflection.</param>
    /// <param name="summary">Описание метода из XML-документации (может быть пустым).</param>
    /// <param name="alias">Имя русскоязычного алиаса либо null, если алиас не найден.</param>
    /// <param name="ruOnly">true, если метод существует только в русскоязычном API.</param>
    /// <returns>Готовая запись метода для последующего вывода.</returns>
    private static MethodEntry BuildMethodEntry(
        Type type,
        MethodInfo method,
        string summary,
        string alias,
        bool ruOnly)
    {
        return BuildMethodEntry(type, method, summary, alias, ruOnly, null, null);
    }

    /// <summary>
    /// Перегрузка BuildMethodEntry с дополнительными данными XML-документации:
    /// текстом тега &lt;returns&gt; и описаниями параметров. Тег &lt;returns&gt; часто
    /// содержит предметные слова, которых нет ни в имени метода, ни в его summary
    /// («Конструктор справочника» у CreateReferenceBuilder), поэтому он выводится
    /// отдельной строкой и участвует в поиске.
    /// </summary>
    private static MethodEntry BuildMethodEntry(
        Type type,
        MethodInfo method,
        string summary,
        string alias,
        bool ruOnly,
        string returns,
        Dictionary<string, string> paramDocs)
    {
        var parameters = method.GetParameters()
            .Select(p => $"{FormatTypeName(p.ParameterType)} {p.Name}")
            .ToList();

        // Полное имя типа экранируется от обратной кавычки: арность generic-типа
        // записывается в метаданных как `1/`2, а внутри Markdown-строки, обрамлённой
        // обратными кавычками, такой символ обрывает code-span и ломает разметку.
        // Кавычка-арности заменяется апострофом: List`1 → List'1.
        string typeName = (type.FullName ?? type.Name).Replace('`', '\'');

        string parameterTypes = string.Join(
            ", ",
            method.GetParameters().Select(p => FormatTypeName(p.ParameterType)));

        return new MethodEntry
        {
            Signature = $"{FormatTypeName(method.ReturnType)} {method.Name}(" +
                        $"{string.Join(", ", parameters)})",
            FullName = $"{typeName}.{method.Name}({parameterTypes})",
            Name = method.Name,
            Summary = summary,
            Alias = alias,
            Parameters = parameters,
            RuOnly = ruOnly,
            IsAsync = method.Name.EndsWith("Async", StringComparison.Ordinal),
            ReturnType = FormatTypeName(method.ReturnType),
            Returns = returns,
            ParamDocs = paramDocs
        };
    }

    /// <summary>
    /// Элемент списка конструкторов типа для итогового Markdown.
    ///
    /// Зачем отдельная сущность: рефлексия помечает конструкторы как special-name,
    /// и фильтр «!IsSpecialName» молча выбрасывал их из дампа. Между тем именно
    /// конструкторы являются документированными точками входа генерации модели данных
    /// («Конструктор типа объектов справочника», «Конструктор справочника»):
    /// без них пользователь не мог собрать ни одного сценария создания модели.
    /// </summary>
    private sealed class ConstructorEntry
    {
        /// <summary>Читаемая сигнатура вида "new ClassObjectBuilder(ClassTree classes)".</summary>
        public string Signature;

        /// <summary>Полное имя с типами параметров: для точного поиска по сигнатуре.</summary>
        public string FullName;

        /// <summary>Описание из тега summary XML-документации; пусто, если описания нет.</summary>
        public string Summary;

        /// <summary>Список параметров в виде "ClassTree classes".</summary>
        public List<string> Parameters;

        /// <summary>Описания параметров из тегов &lt;param&gt;: имя параметра → текст.</summary>
        public Dictionary<string, string> ParamDocs;

        /// <summary>Описание возвращаемого значения из тега &lt;returns&gt;.</summary>
        public string Returns;

        /// <summary>Назначение конструктора, выведенное из текста summary и имени типа.</summary>
        public string Purpose;
    }

    /// <summary>
    /// Формирует запись конструктора для вывода: сигнатуру в C#-нотации,
    /// полное имя с типами параметров, список параметров и тексты XML-документации.
    /// </summary>
    /// <param name="type">Объявляющий тип — используется для полного имени конструктора.</param>
    /// <param name="ctor">Конструктор, полученный через Reflection.</param>
    /// <param name="xmlDoc">Поставщик XML-документации (может быть null).</param>
    /// <returns>Готовая запись конструктора для последующего вывода.</returns>
    private static ConstructorEntry BuildConstructorEntry(
        Type type,
        ConstructorInfo ctor,
        XmlDocProvider xmlDoc)
    {
        var parameters = ctor.GetParameters()
            .Select(p => $"{FormatTypeName(p.ParameterType)} {p.Name}")
            .ToList();

        string typeName = (type.FullName ?? type.Name).Replace('`', '\'');

        string parameterTypes = string.Join(
            ", ",
            ctor.GetParameters().Select(p => FormatTypeName(p.ParameterType)));

        string summary = xmlDoc == null ? null : xmlDoc.GetConstructorSummary(ctor);
        string returns = xmlDoc == null ? null : xmlDoc.GetConstructorReturns(ctor);
        var paramDocs = xmlDoc == null
            ? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            : xmlDoc.GetConstructorParams(ctor);

        return new ConstructorEntry
        {
            Signature = $"new {type.Name}({string.Join(", ", parameters)})",
            FullName = $"{typeName}.#ctor({parameterTypes})",
            Summary = summary,
            Parameters = parameters,
            ParamDocs = paramDocs,
            Returns = returns,
            Purpose = BuildConstructorPurpose(type, ctor, summary)
        };
    }

    /// <summary>
    /// Выводит назначение конструктора в терминах предметной области.
    ///
    /// Проблема, которую решает метод: описания builder-конструкторов в XML-документации
    /// говорят «Конструктор типа объектов справочника», но не содержат глаголов
    /// «создать»/«добавить» и не упоминают имени типа в том виде, как его ищет пользователь.
    /// Формулировка собирается из трёх источников: имя builder-типа (ClassObjectBuilder →
    /// «тип объектов справочника»), текст summary и позиция параметров (конструктор
    /// с существующим объектом предназначен для редактирования, без него — для создания).
    /// </summary>
    private static string BuildConstructorPurpose(Type type, ConstructorInfo ctor, string summary)
    {
        string subject = DescribeBuilderSubject(type);
        bool isEdit = !string.IsNullOrEmpty(summary) &&
                      (summary.IndexOf("для редактирования", StringComparison.OrdinalIgnoreCase) >= 0 ||
                       summary.IndexOf("существующ", StringComparison.OrdinalIgnoreCase) >= 0);

        if (string.IsNullOrEmpty(subject))
            return isEdit ? "Изменение существующего элемента модели данных" : null;

        return isEdit
            ? $"Изменение существующего элемента модели данных: {subject}"
            : $"Создание элемента модели данных: {subject}";
    }

    /// <summary>
    /// Переводит имя типа-конструктора в предметную формулировку на русском языке.
    /// Возвращает null для типов, не относящихся к генерации модели данных.
    /// Соответствие задаётся явной таблицей: автоматический перевод имён
    /// (Builder → «конструктор») давал бы бессмысленные фразы и бесполезный
    /// для поиска текст.
    /// </summary>
    private static string DescribeBuilderSubject(Type type)
    {
        if (type == null)
            return null;

        string name = type.Name;
        int tickIndex = name.IndexOf('`');
        if (tickIndex > 0)
            name = name.Substring(0, tickIndex);

        switch (name)
        {
            case "ReferenceBuilder": return "справочник";
            case "ClassObjectBuilder": return "тип объектов (класс объектов) справочника";
            case "ParameterInfoBuilder": return "параметр справочника";
            case "ExtendedParameterInfoBuilder": return "расширенный параметр справочника";
            // Формулировка уточнена: ParameterGroupBuilder собирает не только группу
            // параметров, но и подчинённый список 1:N (табличную часть). Без этих слов
            // запрос «создать список объектов» не находил точку входа сценария.
            case "ParameterGroupBuilder": return "группа параметров или подчинённый список 1:N (табличная часть) справочника";
            case "ExtendedParametersStorageBuilder": return "хранилище параметров справочника";
            case "LinkGroupBuilder": return "группа связей типа объектов";
            case "UniqueIndexBuilder": return "индекс уникальности справочника";
            case "ObjectFormatBuilder": return "формат отображения объекта";
            case "ObjectParameterFormatBuilder": return "формат отображения параметра";
            case "AliasedParameterInfoBuilder": return "параметр с алиасом";
        }

        if (name.EndsWith("Builder", StringComparison.Ordinal) ||
            name.EndsWith("BuilderBase", StringComparison.Ordinal))
            return "элемент модели данных";

        if (name.EndsWith("Factory", StringComparison.Ordinal))
            return "фабрика создания элементов модели данных";

        return null;
    }

    /// <summary>
    /// Возвращает готовые поисковые формулировки для builder-типа — в тех словоформах,
    /// в которых их реально пишет пользователь.
    ///
    /// Отдельная таблица нужна из-за русского словообразования: предметная формулировка
    /// типа «тип объектов (класс объектов) справочника» не совпадает с пользовательским
    /// запросом «создать тип объекта» — падеж и число других слов не выводятся из имени
    /// типа программно. Без таблицы запросы в единственном числе не находили конструктор
    /// даже при наличии назначения и ключевых слов.
    /// </summary>
    private static IEnumerable<string> GetBuilderSearchPhrases(Type type)
    {
        return type == null
            ? Enumerable.Empty<string>()
            : GetBuilderSearchPhrases(SimpleTypeName(type.Name));
    }

    /// <summary>
    /// Строковая перегрузка: возвращаемый тип метода известен только как отформатированный
    /// текст, поэтому имя приводится к простому виду тем же способом, что и в
    /// <see cref="DescribeBuilderSubject(string)"/> — отбрасываются generic-аргументы,
    /// пространство имён и арность.
    /// </summary>
    private static IEnumerable<string> GetBuilderSearchPhrases(string simpleName)
    {
        if (string.IsNullOrEmpty(simpleName))
            return Enumerable.Empty<string>();

        switch (SimpleTypeName(simpleName))
        {
            case "ReferenceBuilder":
                return new[]
                {
                    "создать справочник, создание справочника, новый справочник, генерация справочника",
                    "create reference, reference builder, создать reference"
                };
            case "ClassObjectBuilder":
                return new[]
                {
                    "создать тип объекта, создать тип объектов, создание типа объектов, новый тип объекта",
                    "класс объектов, создать класс объектов, create class object, type builder"
                };
            case "ParameterInfoBuilder":
            case "ExtendedParameterInfoBuilder":
            case "AliasedParameterInfoBuilder":
                return new[]
                {
                    "создать параметр, создать параметр справочника, добавить параметр, создание параметра",
                    "create parameter, parameter builder"
                };
            case "ParameterGroupBuilder":
                return new[]
                {
                    "создать группу параметров, добавить группу параметров, создание группы параметров",
                    // Добавлено для сценария create-object-list: пользователь формулирует
                    // задачу как «создать список объектов» / «табличная часть», а не как
                    // «создать группу параметров», и без этих фраз точка входа не находилась.
                    "создать список объектов, подчинённый список, табличная часть, TableOneToMany",
                    "create object list, one-to-many table",
                    "create parameter group, parameter group builder"
                };
            case "UniqueIndexBuilder":
                return new[] { "задать условие уникальности, создать индекс уникальности, уникальность типа объекта, уникальный индекс" };
            case "LinkGroupBuilder":
                return new[] { "создать связь, создать ссылочное свойство, добавить связь типа объектов" };
            case "ObjectFormatBuilder":
            case "ObjectParameterFormatBuilder":
                return new[] { "настроить формат отображения, формат отображения объекта, формат параметра" };
            case "ExtendedParametersStorageBuilder":
                return new[] { "создать хранилище параметров, расширенные параметры справочника" };
        }

        return Enumerable.Empty<string>();
    }

    /// <summary>
    /// Приводит имя типа к простому виду: без generic-аргументов, пространства имён и арности.
    /// Нужен для сравнения имён типов, известных только как отформатированный текст
    /// («TFlex.DOCs.Model.Structure.Builders.ReferenceBuilder`1» → «ReferenceBuilder»).
    /// </summary>
    private static string SimpleTypeName(string typeName)
    {
        if (string.IsNullOrEmpty(typeName))
            return null;

        string name = typeName;

        int genericIndex = name.IndexOf('<');
        if (genericIndex > 0)
            name = name.Substring(0, genericIndex);

        int lastDot = name.LastIndexOf('.');
        if (lastDot >= 0 && lastDot < name.Length - 1)
            name = name.Substring(lastDot + 1);

        int tickIndex = name.IndexOf('`');
        if (tickIndex > 0)
            name = name.Substring(0, tickIndex);

        return name;
    }

    /// <summary>
    /// Перегрузка DescribeBuilderSubject для строкового имени типа.
    /// Нужна в местах, где тип известен только как текст: например, возвращаемый тип метода
    /// уже отформатирован в C#-нотацию и обратно в Type не преобразуется.
    /// </summary>
    private static string DescribeBuilderSubject(string typeName)
    {
        if (string.IsNullOrEmpty(typeName))
            return null;

        string name = SimpleTypeName(typeName);

        switch (name)
        {
            case "ReferenceBuilder": return "справочник";
            case "ClassObjectBuilder": return "тип объектов (класс объектов) справочника";
            case "ParameterInfoBuilder": return "параметр справочника";
            case "ExtendedParameterInfoBuilder": return "расширенный параметр справочника";
            case "ParameterGroupBuilder": return "группа параметров или подчинённый список 1:N (табличная часть) справочника";
            case "ExtendedParametersStorageBuilder": return "хранилище параметров справочника";
            case "LinkGroupBuilder": return "группа связей типа объектов";
            case "UniqueIndexBuilder": return "индекс уникальности справочника";
            case "ObjectFormatBuilder": return "формат отображения объекта";
            case "ObjectParameterFormatBuilder": return "формат отображения параметра";
            case "AliasedParameterInfoBuilder": return "параметр с алиасом";
        }

        if (name.EndsWith("Builder", StringComparison.Ordinal) ||
            name.EndsWith("BuilderBase", StringComparison.Ordinal) ||
            name.EndsWith("Factory", StringComparison.Ordinal))
            return "элемент модели данных";

        return null;
    }

    /// <summary>
    /// Извлекает имя параметра из его отформатированного представления "ClassTree classes".
    /// Имя параметра — последнее слово строки; для ref/out-параметров оно выглядит
    /// как "value&amp;" и очищается от знака ссылки.
    /// </summary>
    private static string ExtractParameterName(string formattedParameter)
    {
        if (string.IsNullOrEmpty(formattedParameter))
            return null;

        int spaceIndex = formattedParameter.LastIndexOf(' ');
        if (spaceIndex < 0 || spaceIndex >= formattedParameter.Length - 1)
            return null;

        string name = formattedParameter.Substring(spaceIndex + 1).Trim();
        if (name.EndsWith("&", StringComparison.Ordinal))
            name = name.Substring(0, name.Length - 1);

        return name.Length == 0 ? null : name;
    }

    /// <summary>
    /// Собирает строку ключевых слов для секции конструктора.
    /// Ключевые слова выводятся из предметной формулировки типа и из описания,
    /// чтобы запрос «создать тип объекта с нуля» находил конструктор без параметров.
    /// </summary>
    private static string BuildConstructorKeywords(Type type, ConstructorEntry ctor)
    {
        if (type == null || ctor == null)
            return null;

        var keywords = new List<string> { "создать", "создание", "новый", "конструктор" };
        keywords.Add(type.Name);

        string subject = DescribeBuilderSubject(type);
        if (!string.IsNullOrEmpty(subject))
        {
            keywords.Add(subject);
            keywords.Add($"создать {subject}");
        }

        keywords.AddRange(GetBuilderSearchPhrases(type));

        if (!string.IsNullOrEmpty(ctor.Summary))
            keywords.Add(ctor.Summary);

        // Конструктор без параметров открывает сценарий «с нуля», поэтому получает
        // дополнительный поисковый якорь. Overload с параметром «существующий объект»
        // этого якоря не получает: он предназначен для редактирования, а не для создания,
        // и лишний якорь уводил бы запрос «создать с нуля» на конструктор редактирования.
        if (ctor.Parameters.Count == 0)
            keywords.Add("с нуля, впервые, пустой");

        return string.Join(", ", keywords.Where(k => !string.IsNullOrEmpty(k)));
    }

    /// <summary>
    /// Глаголы, которыми начинается текст &lt;summary&gt;. Если текст &lt;returns&gt;
    /// начинается с такого же глагола, он лишь пересказывает summary и не добавляет
    /// информации о возвращаемом значении.
    /// </summary>
    private static readonly string[] SummaryVerbPrefixes =
    {
        "Возвращает", "Создает", "Создаёт", "Получает", "Удаляет", "Изменяет", "Находит",
        "Выполняет", "Задает", "Задаёт", "Устанавливает", "Проверяет", "Загружает",
        "Сохраняет", "Добавляет", "Копирует", "Перемещает", "Открывает", "Закрывает",
        "Обновляет", "Формирует", "Вычисляет", "Преобразует", "Ищет"
    };

    /// <summary>
    /// Определяет, несёт ли текст тега &lt;returns&gt; информацию о возвращаемом значении.
    ///
    /// Проблема, которую решает метод: в вывод попадало summary вместо returns (поиск
    /// жёстко читал словарь summary), поэтому строки «**Описание:**» и «**Возвращает:**»
    /// дублировали друг друга в 73 % случаев. Дополнительно отсекается пересказ summary,
    /// начинающийся с того же глагола, — такой текст тоже не сообщает тип и смысл
    /// результата, а лишь утяжеляет чанк и размывает вектор запроса.
    ///
    /// Значимые формулировки вида «Ссылка на экземпляр AccessManager», «Список групп
    /// доступа» или «Созданная папка каталога справочников» проверку проходят:
    /// существительное-результат важнее глагола-действия.
    /// </summary>
    /// <param name="returns">Текст тега returns (может быть null).</param>
    /// <param name="summary">Текст тега summary той же сигнатуры (может быть null).</param>
    /// <returns>true, если строку «**Возвращает:**» нужно вывести.</returns>
    private static bool IsInformativeReturns(string returns, string summary)
    {
        if (string.IsNullOrEmpty(returns))
            return false;

        string value = returns.Trim();
        if (value.Length == 0)
            return false;

        if (!string.IsNullOrEmpty(summary) &&
            string.Equals(value, summary.Trim(), StringComparison.Ordinal))
            return false;

        foreach (var verb in SummaryVerbPrefixes)
        {
            if (value.StartsWith(verb, StringComparison.OrdinalIgnoreCase))
                return false;
        }

        return true;
    }

    /// <summary>
    /// Слова, которые в описании параметра лишь повторяют его тип и потому не выводятся.
    /// </summary>
    private static readonly HashSet<string> DegenerateParamDocs = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "Объект", "Тип", "Параметр", "Значение", "Имя", "Список", "Коллекция",
        "Индекс", "Ключ", "Ссылка", "Флаг", "Признак"
    };

    /// <summary>
    /// Формирует строку параметра с описанием из XML либо без него.
    /// Описание-«пустышка» (например «— Объект») опускается: оно не различает параметры
    /// и только утяжеляет чанк.
    /// </summary>
    /// <param name="formattedParameter">Параметр в отформатированном виде «Тип имя».</param>
    /// <param name="paramDocs">Карта «имя параметра → описание».</param>
    /// <returns>Строка для вывода в списке «**Параметры:**».</returns>
    private static string FormatParameterLine(string formattedParameter, Dictionary<string, string> paramDocs)
    {
        string doc = null;
        string paramName = ExtractParameterName(formattedParameter);

        if (!string.IsNullOrEmpty(paramName) && paramDocs != null)
            paramDocs.TryGetValue(paramName, out doc);

        if (string.IsNullOrEmpty(doc))
            return $"  - `{formattedParameter}`";

        string trimmedDoc = doc.Trim().TrimEnd('.');
        if (DegenerateParamDocs.Contains(trimmedDoc))
            return $"  - `{formattedParameter}`";

        return $"  - `{formattedParameter}` — {doc}";
    }

    /// <summary>
    /// Строит обратные карты связей для типов-конструкторов модели данных:
    ///   creators  — «builder-тип → методы этого дампа, которые его возвращают»;
    ///   consumers — «builder-тип → методы, которые принимают его параметром».
    ///
    /// Проход выполняется до генерации текста, потому что в блоках builder-типов нужно
    /// назвать чужие методы (CreateReferenceBuilder, ParameterGroupBuilder.AddParameter),
    /// а они уже отфильтрованы и агрегированы. Без карт связь оставалась односторонней:
    /// от метода к типу, но не от типа к методу, и сценарий генерации модели данных нельзя
    /// было пройти ни в поиске, ни чтением.
    ///
    /// Учитываются только типы, реально попадающие в дамп, иначе в текст ссылок попали бы
    /// методы отфильтрованных служебных классов.
    /// </summary>
    private static void BuildBuilderLinkMaps(
        List<Type> types,
        Dictionary<string, List<string>> creators,
        Dictionary<string, List<string>> consumers)
    {
        var emitted = new HashSet<string>(StringComparer.Ordinal);
        foreach (var type in types)
        {
            string name = SimpleTypeName(type.Name);
            if (!string.IsNullOrEmpty(name))
                emitted.Add(name);
        }

        foreach (var type in types)
        {
            IEnumerable<MethodInfo> methods;
            try
            {
                methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance |
                                          BindingFlags.Static | BindingFlags.DeclaredOnly);
            }
            catch
            {
                continue;
            }

            foreach (var method in methods)
            {
                try
                {
                    // Прямая связь: метод возвращает builder — значит он и есть точка входа.
                    // Разворачивание ValueTask<T>/Task<T> обязательно: фабрики вроде
                    // ParameterGroupBuilder.CreateForEditAsync объявлены как ValueTask<ParameterGroupBuilder>,
                    // и без этого шага у maker-свойств не находится источник.
                    // Добавочные/снимающие аксессоры свойств пропускаются: они уже описаны
                    // строкой свойства, а «CalendarCategoryManager.get_SettingsFactory()»
                    // в подсказке выглядит как внутренний вызов, а не как точка входа.
                    if (method.IsSpecialName)
                        continue;

                    Type returnType = UnwrapAsyncReturnType(method.ReturnType);
                    string returnName = SimpleTypeName(returnType == null ? null : returnType.Name);
                    if (!string.IsNullOrEmpty(returnName) && IsEmittedBuilder(returnType, emitted))
                        AddLink(creators, returnName, BuildCreatorSignature(type, method));

                    // Связь «builder передаётся в метод»: так находятся места, где собранный
                    // конструктор применяется, например ClassObjectBuilder.ParameterGroups.AddParameter.
                    foreach (var parameter in method.GetParameters())
                        AddConsumerLink(consumers, emitted, type, method.Name, parameter.ParameterType);
                }
                catch
                {
                    // Разрешение типов параметров у некоторых членов может падать на
                    // отсутствующих зависимостях загрузчика: одна такая точка не должна
                    // прерывать построение остальных связей.
                }
            }

            // ВАЖНО: у ClassObjectBuilder, ParameterGroupBuilder, UniqueIndexBuilder и
            // LinkGroupBuilder нет ни одного метода-фабрики — они создаются ТОЛЬКО через
            // оператор new. Если сканировать одни методы, эти типы остаются вообще без
            // обратных ссылок и запрос «создать тип объекта» упирается в тупик.
            IEnumerable<ConstructorInfo> typeCtors;
            try
            {
                typeCtors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            }
            catch
            {
                typeCtors = new ConstructorInfo[0];
            }

            foreach (var ctor in typeCtors)
            {
                try
                {
                    foreach (var parameter in ctor.GetParameters())
                        AddConsumerLink(consumers, emitted, type, "#ctor", parameter.ParameterType);
                }
                catch
                {
                }
            }

            // Свойства тоже создают builders: Parameter.Builder и ClassObjectBase.Builder
            // возвращают готовый конструктор, и это ровно тот путь, которым пользуется
            // прикладной код при редактировании существующего элемента модели.
            IEnumerable<PropertyInfo> properties;
            try
            {
                properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance |
                                                BindingFlags.Static | BindingFlags.DeclaredOnly);
            }
            catch
            {
                properties = new PropertyInfo[0];
            }

            foreach (var property in properties)
            {
                try
                {
                    if (property.GetIndexParameters().Length != 0)
                        continue;

                    Type propertyType = property.PropertyType;
                    string propertyTypeName = SimpleTypeName(propertyType == null ? null : propertyType.Name);
                    if (string.IsNullOrEmpty(propertyTypeName) || !IsEmittedBuilder(propertyType, emitted))
                        continue;

                    string receiver = SimpleTypeName(type.Name);
                    AddLink(creators, propertyTypeName,
                            $"{propertyTypeName} {receiver}.{property.Name}");
                }
                catch
                {
                }
            }
        }
    }

    /// <summary>
    /// Формирует сигнатуру точки получения (точки входа) с реальными именами типов
    /// параметров: «CreateReferenceBuilder(ReferenceCatalogFolder folder, bool, ...)».
    /// Тип параметра обязателен — прикладной код без него не знает, что подставлять.
    /// Асинхронные фабрики помечаются многоточием, чтобы было видно необходимость await.
    /// </summary>
    private static string BuildCreatorSignature(Type owner, MethodInfo method)
    {
        bool isAsync = method.ReturnType != null && method.ReturnType.IsGenericType &&
                       (method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>) ||
                        method.ReturnType.GetGenericTypeDefinition() == typeof(ValueTask<>));

        var parts = new List<string>();
        foreach (var parameter in method.GetParameters())
        {
            string parameterType = SimpleTypeName(parameter.ParameterType == null ? null : parameter.ParameterType.Name);
            parts.Add(string.IsNullOrEmpty(parameterType) ? "?" : parameterType);
        }

        string suffix = parts.Count == 0 ? "()" : "(" + string.Join(", ", parts) + ")";
        if (isAsync)
            suffix += "…";

        return $"{SimpleTypeName(owner.Name)}.{method.Name}{suffix}";
    }

    /// <summary>
    /// Список типов параметров через запятую: «ClassTree, ClassObject».
    /// Собственная копия нужна потому, что BuildParameterList объявлен внутри
    /// XmlDocProvider и использует XML-нормализацию имён, а здесь достаточно
    /// коротких имён типов для читателя справочника.
    /// </summary>
    private static string DescribeParameters(ParameterInfo[] parameters)
    {
        if (parameters == null || parameters.Length == 0)
            return string.Empty;

        var parts = new List<string>(parameters.Length);
        foreach (var parameter in parameters)
        {
            string name = SimpleTypeName(parameter.ParameterType == null ? null : parameter.ParameterType.Name);
            parts.Add(string.IsNullOrEmpty(name) ? "?" : name);
        }

        return string.Join(", ", parts);
    }

    /// <summary>
    /// Считает builder «достижимым из прикладного кода»: базовый тип без собственной
    /// документации (System.*, внешние сборки) не должен попадать в обратные ссылки,
    /// иначе они уводят к несуществующим в справочнике блокам.
    /// </summary>
    private static bool IsEmittedBuilder(Type t, HashSet<string> emitted)
    {
        if (t == null || string.IsNullOrEmpty(DescribeBuilderSubject(t)))
            return false;

        string simpleName = SimpleTypeName(t.Name);
        return !string.IsNullOrEmpty(simpleName) && emitted.Contains(simpleName);
    }

    /// <summary>
    /// Добавляет обратную связь «builder принимается параметром». Проверяется и сам тип
    /// параметра, и тип элемента для массивов/ByRef: без этого методы вида
    /// AddParameter(params ParameterInfoBuilder[] builders) теряют связь.
    /// </summary>
    private static void AddConsumerLink(
        Dictionary<string, List<string>> consumers,
        HashSet<string> emitted,
        Type owner,
        string memberName,
        Type parameterType)
    {
        if (parameterType == null)
            return;

        if (parameterType.IsByRef || parameterType.IsArray)
            parameterType = parameterType.GetElementType();

        string parameterName = SimpleTypeName(parameterType == null ? null : parameterType.Name);
        if (string.IsNullOrEmpty(parameterName) || !IsEmittedBuilder(parameterType, emitted))
            return;

        string ownerName = SimpleTypeName(owner.Name);
        string linkedMember = memberName == "#ctor" ? $"new {ownerName}" : $"{ownerName}.{memberName}";
        AddLink(consumers, parameterName, $"{linkedMember}(...)");
    }

    /// <summary>
    /// Типы без единого метода-фабрики и без свойств-источников создаются только
    /// оператором new: ClassObjectBuilder, UniqueIndexBuilder, LinkGroupBuilder.
    ///
    /// Для них обратной ссылки «как получить» не существует по устройству API, и её
    /// отсутствие в блоке читается как потеря сценария. Строка с перечислением
    /// публичных конструкторов закрывает этот пробел и служит лексическим якорем
    /// для запроса «создать тип объекта».
    /// </summary>
    private static string BuildNoFactoryHint(Type type)
    {
        try
        {
            var ctors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            if (ctors.Length == 0)
                return null;

            var signatures = ctors
                .OrderBy(c => c.GetParameters().Length)
                .Select(c => $"new {SimpleTypeName(type.Name)}({DescribeParameters(c.GetParameters())})")
                .Distinct()
                .Take(4)
                .ToList();

            if (signatures.Count == 0)
                return null;

            return "у этого типа нет метода-фабрики — " + string.Join("; ", signatures);
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Разворачивает Task&lt;T&gt;/ValueTask&lt;T&gt; до T. Асинхронные фабрики
    /// (ReferenceBuilder.CreateForEditAsync, ParameterGroupBuilder.CreateForEditAsync)
    /// без этого не распознаются как источник builder-типа.
    /// </summary>
    private static Type UnwrapAsyncReturnType(Type returnType)
    {
        if (returnType == null || !returnType.IsGenericType)
            return returnType;

        Type definition = returnType.GetGenericTypeDefinition();
        if (definition == typeof(Task<>) || definition == typeof(ValueTask<>))
            return returnType.GetGenericArguments()[0];

        return returnType;
    }

    private static void AddLink(Dictionary<string, List<string>> map, string key, string value)
    {
        List<string> list;
        if (!map.TryGetValue(key, out list))
        {
            list = new List<string>();
            map[key] = list;
        }

        if (list.Count < 6 && !list.Contains(value))
            list.Add(value);
    }

    /// <summary>
    /// Определяет, является ли тип «конструктором» модели данных: builder/factory,
    /// через который собираются справочники, типы объектов, параметры, связи и индексы.
    ///
    /// Проверка рекурсивна по объявляющему типу: вложенные типы вроде
    /// ClassObjectBuilder.ClassObjectCollection или ParameterGroupBuilder.ParameterGroupCollection
    /// сами не оканчиваются на Builder, но принадлежат builder-семейству и содержат
    /// методы настройки (CanAddItem, CanRemoveItem), без которых сценарий не собрать.
    /// </summary>
    private static bool IsBuilderRelatedType(Type t)
    {
        if (t == null)
            return false;

        string name = t.Name;

        // Отсекаем generic-арность (ParameterInfoBuilder`1 → ParameterInfoBuilder),
        // иначе проверка суффикса не сработает.
        int tickIndex = name.IndexOf('`');
        if (tickIndex > 0)
            name = name.Substring(0, tickIndex);

        if (name.EndsWith("Builder", StringComparison.Ordinal) ||
            name.EndsWith("BuilderBase", StringComparison.Ordinal) ||
            name.EndsWith("Factory", StringComparison.Ordinal))
            return true;

        // Namespace тоже учитывается: TFlex.DOCs.Model.Structure.Builders — выделенное
        // пространство имён конструкторов модели данных, и всё в нём является точкой входа.
        string ns = t.Namespace ?? "";
        if (ns.EndsWith(".Builders", StringComparison.Ordinal))
            return true;

        return t.DeclaringType != null && IsBuilderRelatedType(t.DeclaringType);
    }

    /// <summary>
    /// Определяет, относится ли тип к высокоуровневой макромодели (Macro DSL)
    /// или к низкоуровневому Core Open API платформы.
    /// </summary>
    private static bool IsMacroDslType(Type type)
    {
        if (type == null) return false;

        string ns = type.Namespace ?? "";
        if (ns.StartsWith("TFlex.DOCs.Model.Macros", StringComparison.Ordinal))
            return true;

        if (type.Name == "MacroProvider" || type.Name == "RefObj" || type.Name == "Объект" ||
            type.Name == "Подключение" || type.Name == "ДиалогВвода" || type.Name == "Объекты")
            return true;

        for (Type baseType = type.BaseType; baseType != null; baseType = baseType.BaseType)
        {
            if (baseType.Name == "MacroProvider" || baseType.Name == "ObjectAccessor")
                return true;
        }

        return false;
    }

    /// <summary>
    /// Формирует сигнатуру свойства для справочника API.
    ///
    /// Для обычного свойства возвращается его имя. Для свойства-индексатора
    /// (например, Item[string parameter] у ParameterAccessor или Item[int index]
    /// у коллекций макромодели) в квадратных скобках добавляются параметры
    /// индексации с типами: без этого RAG не сможет отличить индексатор
    /// от обычного свойства и потеряет способ обращения к элементу.
    /// </summary>
    /// <param name="property">Свойство, полученное через Reflection.</param>
    /// <returns>Имя свойства либо "Имя[тип параметр, ...]" для индексатора.</returns>
    private static string BuildPropertySignature(PropertyInfo property)
    {
        if (property == null)
            return string.Empty;

        ParameterInfo[] indexParams;
        try
        {
            indexParams = property.GetIndexParameters();
        }
        catch (Exception)
        {
            // Метаданные индексатора могут быть недоступны (например, из-за
            // отсутствующей зависимости): свойство выводится как обычное.
            indexParams = new ParameterInfo[0];
        }

        if (indexParams.Length == 0)
            return property.Name;

        string indexArgs = string.Join(", ", indexParams.Select(
            ip => $"{FormatTypeName(ip.ParameterType)} {ip.Name}"));

        return $"{property.Name}[{indexArgs}]";
    }

    /// <summary>
    /// Дополняет список свойств типа унаследованными членами коллекций макромодели.
    ///
    /// Причина отдельного прохода: свойства собираются с флагом DeclaredOnly, поэтому
    /// у списков макромодели (RefObjList, Объекты и их наследники) в справочник не
    /// попадали унаследованные от ObjectAccessorList&lt;T&gt; индексатор Item[int] и
    /// свойство Count — ключевые способы работы с коллекцией в макроязыке.
    ///
    /// Наследуются только члены коллекций: индексаторы и счётчик элементов. Прочие
    /// унаследованные члены намеренно не подмешиваются, чтобы не дублировать в справочнике
    /// десятки методов базовых типов и не размывать векторный поиск. Уже выведенные
    /// собственные свойства повторно не добавляются.
    /// </summary>
    /// <param name="type">Описываемый тип макромодели.</param>
    /// <param name="declaredProps">Свойства, собранные с флагом DeclaredOnly.</param>
    /// <param name="propLines">Накопитель строк секции «Свойства» для текущего типа.</param>
    /// <param name="xmlDoc">Поставщик XML-документации для поиска описаний.</param>
    private static void AddInheritedCollectionMembers(
        Type type,
        List<PropertyInfo> declaredProps,
        List<PropEntry> propLines,
        XmlDocProvider xmlDoc)
    {
        if (type == null || declaredProps == null || propLines == null)
            return;

        // Индексатор и Count ищутся только среди public-свойств экземпляра.
        // Свойства статики и непубличные члены для коллекции макроязыка несущественны.
        const BindingFlags inheritedFlags =
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy;

        // Имена собственных свойств нужны, чтобы не задваивать члены, которые
        // переопределены в самом типе (Type.GetProperty вернул бы их повторно).
        var declaredNames = new HashSet<string>(
            declaredProps.Where(p => p != null).Select(p => p.Name));

        // Уже попавшие в вывод сигнатуры: защита от повторной вставки, если тип
        // переобъявляет члены базового класса с тем же именем и арностью.
        var emitted = new HashSet<string>(propLines.Select(p => p.Name));

        foreach (var memberName in new[] { "Item", "Count" })
        {
            if (declaredNames.Contains(memberName))
                continue;

            PropertyInfo inherited;
            try
            {
                inherited = type.GetProperty(memberName, inheritedFlags);
            }
            catch (Exception)
            {
                // Неоднозначный или недоступный член пропускается без прерывания выгрузки.
                continue;
            }

            if (inherited == null)
                continue;

            string signature = BuildPropertySignature(inherited);
            if (!emitted.Add(signature))
                continue;

            // Описание ищется по XML-документации: унаследованный член документирован
            // в базовом типе, и XmlDocProvider корректно находит его по DeclaringType.
            string summary = xmlDoc == null ? null : xmlDoc.GetPropertySummary(inherited);

            propLines.Add(new PropEntry
            {
                TypeName = FormatTypeName(inherited.PropertyType),
                Name = signature,
                RuNote = " [унаследовано]",
                DocNote = string.IsNullOrEmpty(summary) ? "" : $" — {summary}"
            });
        }
    }

    /// <summary>
    /// Дополняет блок builder-типа унаследованными терминальными методами из его базового
    /// класса. Причина: рабочие методы завершения сценария (Save, Delete, Build) объявлены
    /// в базовом типе (ParameterGroupBuilderBase, ObjectFormatBuilderBase и т.п.), а выборка
    /// с флагом DeclaredOnly их не возвращает. В справочнике от ParameterGroupBuilder
    /// оставались только его собственные настройки без единого способа сохранить результат.
    ///
    /// Список имён намеренно ограничен InheritedBuilderMemberNames: без него в блок
    /// утекли бы десятки служебных унаследованных членов и размыли векторный поиск.
    /// </summary>
    /// <param name="type">Тип-конструктор, для которого собирается блок.</param>
    /// <param name="methodEntries">Уже собранные собственные методы типа.</param>
    private static void AddInheritedBuilderMembers(
        Type type,
        List<MethodEntry> methodEntries)
    {
        if (type == null || methodEntries == null)
            return;

        const BindingFlags inheritedFlags =
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy;

        // Защита от задвоения: методы, уже показанные в блоке (в том числе собственные
        // переопределения), повторно не добавляются.
        var emitted = new HashSet<string>(methodEntries.Select(e => e.Signature));

        for (Type baseType = type.BaseType; baseType != null; baseType = baseType.BaseType)
        {
            // Поднимаемся ровно до конца цепочки builder-баз: за её пределами лежат
            // служебные типы (Object, IDisposable-обёртки), члены которых не нужны.
            string baseName = baseType.Name;
            int tickIndex = baseName.IndexOf('`');
            if (tickIndex > 0)
                baseName = baseName.Substring(0, tickIndex);

            MethodInfo[] methods;
            try
            {
                methods = baseType.GetMethods(inheritedFlags);
            }
            catch (Exception)
            {
                continue;
            }

            foreach (var method in methods)
            {
                if (!InheritedBuilderMemberNames.Contains(method.Name))
                    continue;

                // Служебные члены (Dispose от IDisposable и т.п.) в справочнике API не нужны.
                if (method.IsSpecialName || method.Name == "Dispose")
                    continue;

                try
                {
                    if (method.GetParameters().Length == 0 && method.ReturnType == typeof(void) &&
                        method.Name != "Save" && method.Name != "Delete" && method.Name != "Validate")
                        continue;
                }
                catch (Exception)
                {
                    continue;
                }

                var signature = $"{FormatTypeName(method.ReturnType)} {method.Name}(" +
                                string.Join(", ", method.GetParameters()
                                    .Select(p => $"{FormatTypeName(p.ParameterType)} {p.Name}")) +
                                ") [унаследовано от " + baseType.Name + "]";

                if (!emitted.Add(signature))
                    continue;

                // Полное имя объявляется по БАЗОВОМУ типу: именно там метод и находится,
                // поэтому строка служит корректным указанием для поиска в документации.
                string baseTypeFullName = (baseType.FullName ?? baseType.Name).Replace('`', '\'');

                methodEntries.Add(new MethodEntry
                {
                    Signature = signature,
                    FullName = $"{baseTypeFullName}.{method.Name}(" +
                               string.Join(", ", method.GetParameters()
                                   .Select(p => FormatTypeName(p.ParameterType))) + ")",
                    Summary = $"Унаследованный метод {baseType.Name}.{method.Name}: доступен у " +
                              $"{type.Name} и выполняет ту же роль в сценарии.",
                    Alias = null,
                    Parameters = method.GetParameters()
                        .Select(p => $"{FormatTypeName(p.ParameterType)} {p.Name}")
                        .ToList(),
                    RuOnly = false,
                    IsAsync = method.Name.EndsWith("Async", StringComparison.Ordinal),
                    ReturnType = FormatTypeName(method.ReturnType),
                    Returns = null,
                    ParamDocs = null
                });
            }

            // Цепочка обрывается на первом же типе, не относящемся к builder-семейству:
            // дальше лежат служебные базовые классы платформы.
            if (!baseName.EndsWith("BuilderBase", StringComparison.Ordinal) &&
                !baseName.EndsWith("FactoryBase", StringComparison.Ordinal) &&
                !baseName.EndsWith("Builder", StringComparison.Ordinal) &&
                !baseName.EndsWith("Factory", StringComparison.Ordinal))
                break;
        }
    }

    /// <summary>
    /// Приводит тип .NET к читаемой нотации C#, устраняя артефакты рефлексии.
    ///
    /// Что нормализуется:
    ///   * void и null                       → "void";
    ///   * псевдонимы CLR в нотацию C#       → "string", "int", "bool", "double", "long", "object";
    ///   * открытые/закрытые generic-типы    → "List`1" превращается в "List&lt;string&gt;",
    ///     артефакт обратной кавычки с арностью убирается, аргументы форматируются рекурсивно;
    ///   * nullable-типы                     → "Nullable`1" превращается в "int?" (см. GetNullableSuffix);
    ///   * массивы и указатели               → суффиксы "[]" и "*";
    ///   * ref/out-параметры                 → суффикс "&amp;".
    /// </summary>
    /// <param name="type">Тип, полученный через Reflection (может быть null).</param>
    /// <returns>Строка с C#-нотацией типа; для null и void возвращается "void".</returns>
    private static string FormatTypeName(Type type)
    {
        if (type == null) return "void";
        if (type == typeof(void)) return "void";
        if (type == typeof(string)) return "string";
        if (type == typeof(int)) return "int";
        if (type == typeof(bool)) return "bool";
        if (type == typeof(double)) return "double";
        if (type == typeof(long)) return "long";
        if (type == typeof(object)) return "object";

        // ref/out-параметры в метаданных представлены типом с модификатором ByRef:
        // печатаем базовый тип с суффиксом "&", как это принято в C#-нотации.
        if (type.IsByRef)
            return FormatTypeName(type.GetElementType()) + "&";

        // Указатели (небезопасный код) — артефакт рефлексии Pointer, заменяется на "*".
        if (type.IsPointer)
            return FormatTypeName(type.GetElementType()) + "*";

        if (type.IsArray)
        {
            // Массивы форматируются рекурсивно: поддерживаются и многомерные (ранг > 1).
            string suffix = "[" + new string(',', type.GetArrayRank() - 1) + "]";
            return FormatTypeName(type.GetElementType()) + suffix;
        }

        string shortName = type.Name;

        if (type.IsGenericTypeDefinition || type.IsGenericType)
        {
            // Срезаем артефакт обратной кавычки с арностью: "List`1" → "List".
            int tickIndex = shortName.IndexOf('`');
            string genericName = tickIndex >= 0 ? shortName.Substring(0, tickIndex) : shortName;

            // Nullable<T> в C#-нотации печатается как "T?" и не раскрывается в угловые скобки.
            if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
                return FormatTypeName(type.GetGenericArguments()[0]) + "?";

            if (type.IsGenericType)
            {
                var args = string.Join(", ", type.GetGenericArguments().Select(FormatTypeName));
                return $"{genericName}<{args}>";
            }

            return genericName;
        }

        return shortName;
    }

    /// <summary>
    /// Строит карту соответствия «латинское свойство → кириллический алиас» для одного класса.
    ///
    /// Главный и единственный критерий доказательства алиаса — посимвольная идентичность
    /// текста summary из XML-документации внутри одного и того же класса. Именно так
    /// формулируется правило зеркалирования: никакого угадывания по длине списков,
    /// по порядку объявления или по «похожести» имён.
    ///
    /// Совпадение типов используется не как фильтр, а как признак ранжирования кандидатов,
    /// потому что в T-FLEX DOCs латинские и кириллические типы образуют полностью
    /// параллельные иерархии (RefObj ↔ Объект, UserRefObj ↔ Пользователь,
    /// RefObjList ↔ Объекты), и строгое равенство PropertyType заблокировало бы
    /// заведомо корректные алиасы с идентичным описанием.
    ///
    /// Приоритет выбора кандидата с одинаковым summary:
    ///   1. точное совпадение System.Type;
    ///   2. совпадение короткого имени типа и арности generic-аргументов;
    ///   3. родство по цепочке наследования (параллельные типы с общим базовым типом);
    ///   4. любой оставшийся кандидат — идентичность описания уже является достаточным
    ///      доказательством, а порядок объявления членов в документации стабилен.
    ///
    /// На любом уровне дополнительно требуется совпадение арности параметров индексатора
    /// и непустой текст summary у обоих свойств. Каждое кириллическое свойство может быть
    /// назначено алиасом только один раз — сопоставление взаимно-однозначное.
    /// </summary>
    /// <param name="engProps">Латинские свойства класса.</param>
    /// <param name="ruProps">Кириллические свойства того же класса.</param>
    /// <param name="xmlDoc">Поставщик XML-документации.</param>
    /// <returns>Словарь найденных пар; для свойств без пары записи отсутствуют.</returns>
    private static Dictionary<PropertyInfo, PropertyInfo> BuildPropertyMirrors(
        List<PropertyInfo> engProps, List<PropertyInfo> ruProps, XmlDocProvider xmlDoc)
    {
        var mirrors = new Dictionary<PropertyInfo, PropertyInfo>();
        if (engProps == null || ruProps == null || xmlDoc == null) return mirrors;

        // Кэшируем тексты summary: словарь документации разбирается один раз на класс,
        // а не при каждой проверке кандидата.
        var enSummaries = new Dictionary<PropertyInfo, string>();
        foreach (var p in engProps)
            enSummaries[p] = xmlDoc.GetPropertySummary(p);

        var ruSummaries = new Dictionary<PropertyInfo, string>();
        foreach (var r in ruProps)
            ruSummaries[r] = xmlDoc.GetPropertySummary(r);

        // Уже назначенные алиасы: гарантирует взаимно-однозначное сопоставление.
        var takenRu = new HashSet<PropertyInfo>();

        // Четыре прохода по уровням приоритета: на проходе N закрепляются только те пары,
        // для которых доступен кандидат уровня N. Это исключает ситуацию, когда менее
        // подходящий по типу кандидат «забирает» свойство раньше более точного.
        for (int level = 1; level <= 4; level++)
        {
            foreach (var engProp in engProps)
            {
                if (mirrors.ContainsKey(engProp)) continue;

                string engSummary = enSummaries[engProp];
                if (string.IsNullOrEmpty(engSummary)) continue;

                foreach (var ruProp in ruProps)
                {
                    if (takenRu.Contains(ruProp)) continue;

                    string ruSummary = ruSummaries[ruProp];
                    if (string.IsNullOrEmpty(ruSummary)) continue;
                    if (!string.Equals(engSummary, ruSummary, StringComparison.Ordinal)) continue;
                    if (!HasSameIndexArity(engProp, ruProp)) continue;

                    // На уровнях 1–3 требуется дополнительная совместимость типов,
                    // на уровне 4 достаточно идентичного описания.
                    if (level < 4 && !ArePropertyTypesCompatible(engProp, ruProp, level)) continue;

                    mirrors[engProp] = ruProp;
                    takenRu.Add(ruProp);
                    break;
                }
            }
        }

        return mirrors;
    }

    /// <summary>
    /// Проверяет совпадение арности параметров индексатора: обычные свойства имеют ноль
    /// параметров, индексаторы — один и более. Свойства с разной арностью алиасами быть не могут.
    /// </summary>
    /// <param name="engProp">Латинское свойство.</param>
    /// <param name="ruProp">Кириллическое свойство.</param>
    /// <returns>true, если арность параметров совпадает.</returns>
    private static bool HasSameIndexArity(PropertyInfo engProp, PropertyInfo ruProp)
    {
        try
        {
            return engProp.GetIndexParameters().Length == ruProp.GetIndexParameters().Length;
        }
        catch (Exception)
        {
            // Для свойств, недоступных для интроспекции, совпадение арности не подтверждается.
            return false;
        }
    }

    /// <summary>
    /// Проверяет совместимость типов двух свойств на заданном уровне строгости
    /// (см. описание уровней в BuildPropertyMirrors).
    /// </summary>
    /// <param name="engProp">Латинское свойство.</param>
    /// <param name="ruProp">Кириллическое свойство.</param>
    /// <param name="level">Уровень сопоставления: 1 — точный тип, 2 — короткое имя, 3 — наследование.</param>
    /// <returns>true, если типы совместимы на указанном уровне.</returns>
    private static bool ArePropertyTypesCompatible(PropertyInfo engProp, PropertyInfo ruProp, int level)
    {
        Type engType = engProp.PropertyType;
        Type ruType = ruProp.PropertyType;

        if (level <= 1)
            return engType == ruType;

        if (level == 2)
        {
            // Короткое имя типа у артефактов рефлексии содержит обратную кавычку с арностью
            // ("List`1"), поэтому дополнительно сверяется арность generic-аргументов.
            if (engType.Name != ruType.Name) return false;

            int engArity = engType.IsGenericType ? engType.GetGenericArguments().Length : 0;
            int ruArity = ruType.IsGenericType ? ruType.GetGenericArguments().Length : 0;
            return engArity == ruArity;
        }

        // Уровень 3: параллельные иерархии типов T-FLEX DOCs (RefObj ↔ Объект и т.п.).
        return IsTypeRelated(engType, ruType) || IsTypeRelated(ruType, engType);
    }

    /// <summary>
    /// Проверяет родство типов по цепочке наследования или по совместимости присваивания.
    /// Исключение составляют System.Object и типы из пространства имён System:
    /// без этой защиты любые два ссылочных типа оказались бы «совместимыми» через object.
    /// </summary>
    /// <param name="candidate">Тип, который проверяется как базовый/совместимый.</param>
    /// <param name="target">Тип, для которого ищется родство.</param>
    /// <returns>true, если типы связаны наследованием.</returns>
    private static bool IsTypeRelated(Type candidate, Type target)
    {
        if (candidate == null || target == null) return false;
        if (candidate == typeof(object) || target == typeof(object)) return false;

        // Значимые типы и строки сопоставляются только по точному равенству (уровень 1),
        // на уровне наследования для них родство не считается доказательством алиаса.
        if (candidate.IsValueType || target.IsValueType) return false;
        if (candidate == typeof(string) || target == typeof(string)) return false;

        if (candidate.IsAssignableFrom(target)) return true;

        // Явный обход цепочки BaseType: IsAssignableFrom не покрывает открытые generic-определения.
        for (Type t = target.BaseType; t != null; t = t.BaseType)
        {
            if (t == candidate) return true;
        }

        return false;
    }

    /// <summary>
    /// Ищет кириллический алиас метода строго по идентичному тексту summary из XML-документации.
    ///
    /// Правило сопоставления (полностью исключает прежнюю ложную склейку по случайной длине
    /// параметров — например, Run ↔ Прервать):
    ///   1. текст summary латинского метода должен существовать и быть непустым;
    ///   2. текст summary кириллического метода должен совпадать с ним посимвольно;
    ///   3. количество параметров методов должно совпадать (защита от одноимённых
    ///      по описанию, но разных по арности методов).
    ///
    /// Если совпадений нет, возвращается null — маркер "[RU: …]" не добавляется.
    /// </summary>
    /// <param name="ruMethods">Список кириллических методов того же класса.</param>
    /// <param name="latinMethod">Латинский метод (лучшая перегрузка), для которого ищется алиас.</param>
    /// <param name="latinSummary">Уже полученный текст summary латинского метода (может быть пустым).</param>
    /// <param name="xmlDoc">Поставщик XML-документации.</param>
    /// <returns>Найденный кириллический метод или null.</returns>
    private static MethodInfo FindMirroredMethod(List<MethodInfo> ruMethods, MethodInfo latinMethod, string latinSummary, XmlDocProvider xmlDoc)
    {
        if (ruMethods == null || latinMethod == null || xmlDoc == null) return null;

        // Без описания доказывать зеркальность нечем: ложные склейки запрещены.
        if (string.IsNullOrEmpty(latinSummary)) return null;

        int latinParamCount;
        try
        {
            latinParamCount = latinMethod.GetParameters().Length;
        }
        catch (Exception)
        {
            return null;
        }

        foreach (var ruMethod in ruMethods)
        {
            // Арность кандидата обёрнута в try/catch: у некоторых типов метаданные
            // недоступны (например, сборка подгружена частично), и GetParameters()
            // бросает исключение. Такой кандидат пропускается, выгрузка продолжается.
            int ruParamCount;
            try
            {
                ruParamCount = ruMethod.GetParameters().Length;
            }
            catch (Exception)
            {
                continue;
            }

            if (ruParamCount != latinParamCount) continue;

            // Кандидаты упорядочены по числу параметров, поэтому после прохода
            // всех методов с совпадающей арностью дальнейший поиск бессмыслен:
            // методы с другим числом параметров алиасами быть не могут.
            if (ruParamCount > latinParamCount) break;

            string ruSummary;
            try
            {
                ruSummary = xmlDoc.GetMethodSummary(ruMethod);
            }
            catch (Exception)
            {
                continue;
            }

            if (string.IsNullOrEmpty(ruSummary)) continue;

            if (string.Equals(ruSummary, latinSummary, StringComparison.Ordinal))
                return ruMethod;
        }

        return null;
    }
}

/// <summary>
/// Встроенный (без сторонних библиотек) поставщик XML-документации .NET.
/// Читает парный XML-файл, который компилятор C# формирует рядом со сборкой
/// (например, TFlex.DOCs.Model.xml для TFlex.DOCs.Model.dll), и предоставляет
/// быстрый поиск текста тега summary по идентификаторам членов.
///
/// Формат идентификаторов членов (member id) в XML-документации:
///   T:Namespace.TypeName            — класс, структура, интерфейс, делегат, перечисление;
///   P:Namespace.TypeName.Property   — свойство (или индексатор);
///   M:Namespace.TypeName.Method(...) — метод, включая список типов параметров.
/// Для generic-типов арность обозначается обратной кавычкой: T:Namespace.TypeName`1,
/// а для вложенных типов вместо символа '+' используется точка: Ns.Outer.Inner.
/// </summary>
public class XmlDocProvider
{
    /// <summary>
    /// Словарь описаний: ключ — полный идентификатор члена документации
    /// (например "T:TFlex.DOCs.Model.Classes.ClassObject" либо
    /// "M:TFlex.DOCs.Model.Classes.ClassObject.Find(System.String)"),
    /// значение — нормализованный (однострочный) текст тега summary.
    /// Сравнение ключей регистронезависимое.
    /// </summary>
    private readonly Dictionary<string, string> _docs =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Словарь текстов тега &lt;returns&gt;: ключ — идентификатор метода, значение — описание
    /// возвращаемого значения. Именно этот тег содержит самую ценную для поиска информацию
    /// о builder-методах: "Конструктор справочника", "Созданный справочник" и т.п.
    /// Раньше тег не читался вовсе, и точка входа генерации модели данных
    /// (CreateReferenceBuilder) описывалась только глаголом "Создает...".
    /// </summary>
    private readonly Dictionary<string, string> _returns =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Словарь описаний параметров: ключ — идентификатор метода, значение — карта
    /// «имя параметра → описание из &lt;param name="..."&gt;». Используется для вывода
    /// смысла параметров рядом с их сигнатурой ("folder — Папка каталога справочников").
    /// </summary>
    private readonly Dictionary<string, Dictionary<string, string>> _params =
        new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);

    /// <summary>Количество успешно извлечённых описаний. Удобно для диагностики в консоли.</summary>
    public int Count { get { return _docs.Count; } }

    /// <summary>
    /// Загружает XML-файл документации и наполняет словарь описаний.
    /// Файл, которого нет, или битый XML не приводят к исключению: дамп
    /// просто продолжит работу, формируя сигнатуры без описаний.
    /// </summary>
    /// <param name="xmlPath">Полный путь к парному XML-файлу документации сборки.</param>
    /// <returns>true, если файл прочитан и хотя бы одно описание извлечено.</returns>
    public bool LoadXml(string xmlPath)
    {
        if (string.IsNullOrEmpty(xmlPath) || !File.Exists(xmlPath))
            return false;

        _docs.Clear();
        _returns.Clear();
        _params.Clear();

        try
        {
            XDocument doc = XDocument.Load(xmlPath);
            XElement root = doc.Root;
            if (root == null)
                return false;

            // Структура файла: <doc><members><member name="T:..."><summary>текст</summary></member>...</members></doc>
            foreach (XElement member in root.Descendants("member"))
            {
                if (member == null)
                    continue;

                XAttribute nameAttr = member.Attribute("name");
                if (nameAttr == null || string.IsNullOrEmpty(nameAttr.Value))
                    continue;

                // Теги разбираются НЕЗАВИСИМО друг от друга. Раньше отсутствие <summary>
                // приводило к continue, и вместе с ним терялись <param> и <returns> —
                // а у builder-методов именно они содержат ключевые слова предметной области.
                XElement summaryElement = member.Element("summary");
                if (summaryElement != null)
                {
                    string summaryText = CleanText(summaryElement.Value);
                    if (!string.IsNullOrEmpty(summaryText) && !_docs.ContainsKey(nameAttr.Value))
                        _docs.Add(nameAttr.Value, summaryText);
                }

                XElement returnsElement = member.Element("returns");
                if (returnsElement != null)
                {
                    string returnsText = CleanText(returnsElement.Value);
                    if (!string.IsNullOrEmpty(returnsText) && !_returns.ContainsKey(nameAttr.Value))
                        _returns.Add(nameAttr.Value, returnsText);
                }

                var paramMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                foreach (XElement paramElement in member.Elements("param"))
                {
                    XAttribute paramName = paramElement.Attribute("name");
                    if (paramName == null || string.IsNullOrEmpty(paramName.Value))
                        continue;

                    string paramText = CleanText(paramElement.Value);
                    if (!string.IsNullOrEmpty(paramText))
                        paramMap[paramName.Value] = paramText;
                }

                if (paramMap.Count > 0 && !_params.ContainsKey(nameAttr.Value))
                    _params.Add(nameAttr.Value, paramMap);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($" [XML Docs] Не удалось прочитать {Path.GetFileName(xmlPath)}: {ex.Message}");
            _docs.Clear();
            _returns.Clear();
            _params.Clear();
            return false;
        }

        return _docs.Count > 0;
    }

    /// <summary>
    /// Находит описание класса/структуры/интерфейса по ключу "T:{FullName}".
    /// В идентификаторах документации вложенные типы разделяются точкой, а не '+',
    /// поэтому символ '+' в полном имени типа заменяется на '.'.
    /// </summary>
    /// <param name="type">Тип, для которого нужно получить текст summary.</param>
    /// <returns>Однострочное описание или null, если описания нет.</returns>
    public string GetTypeSummary(Type type)
    {
        if (type == null)
            return null;

        string fullName = NormalizeTypeName(type.FullName);
        if (string.IsNullOrEmpty(fullName))
            return null;

        return Lookup($"T:{fullName}");
    }

    /// <summary>
    /// Находит описание свойства по ключу "P:{DeclaringType}.{PropertyName}".
    /// </summary>
    /// <param name="prop">Свойство, для которого нужно получить текст summary.</param>
    /// <returns>Однострочное описание или null, если описания нет.</returns>
    public string GetPropertySummary(PropertyInfo prop)
    {
        if (prop == null || prop.DeclaringType == null)
            return null;

        string typeName = NormalizeTypeName(prop.DeclaringType.FullName);
        if (string.IsNullOrEmpty(typeName))
            return null;

        return Lookup($"P:{typeName}.{prop.Name}");
    }

    /// <summary>
    /// Находит описание метода по ключу "M:{DeclaringType}.{MethodName}({параметры})".
    /// Сначала выполняется попытка точного совпадения по списку типов параметров,
    /// затем — fallback-поиск по префиксу идентификатора: берётся первое описание,
    /// ключ которого начинается с "M:{DeclaringType}.{MethodName}".
    /// Fallback необходим, потому что форматирование ref/out и generic-параметров
    /// в сигнатуре документации может отличаться от рефлексии (например,
    /// "System.String" против "System.String@", "`0" против "T").
    /// </summary>
    /// <param name="method">Метод, для которого нужно получить текст summary.</param>
    /// <returns>Однострочное описание или null, если описания нет.</returns>
    public string GetMethodSummary(MethodInfo method)
    {
        if (method == null || method.DeclaringType == null)
            return null;

        string typeName = NormalizeTypeName(method.DeclaringType.FullName);
        if (string.IsNullOrEmpty(typeName))
            return null;

        string baseKey = $"M:{typeName}.{method.Name}";

        // 1. Точное совпадение: M:Namespace.Type.Method(System.String,System.Int32)
        string exactKey = baseKey + "(" + BuildParameterList(method) + ")";
        string exact = Lookup(exactKey);
        if (!string.IsNullOrEmpty(exact))
            return exact;

        // 2. Fallback: первое описание любой перегрузки этого метода.
        //    Граница имени метода обязательна: для метода, отсутствующего в документации
        //    (например, "Run"), префиксный поиск без границы подхватил бы описание чужого
        //    метода с тем же началом имени ("RunMacro") и выдал бы ложное описание.
        return LookupByPrefix(baseKey + "(", baseKey + "``");
    }

    /// <summary>
    /// Находит описание возвращаемого значения метода по тегу &lt;returns&gt;.
    /// Логика поиска ключа повторяет GetMethodSummary: сначала точное совпадение
    /// по списку типов параметров, затем fallback по префиксу идентификатора.
    /// </summary>
    /// <param name="method">Метод, для которого нужно получить текст тега returns.</param>
    /// <returns>Однострочное описание возвращаемого значения или null.</returns>
    public string GetMethodReturns(MethodInfo method)
    {
        return GetReturnsByMember(method == null ? null : method.DeclaringType,
                                  method == null ? null : method.Name,
                                  method);
    }

    /// <summary>
    /// Находит описание возвращаемого значения конструктора по тегу &lt;returns&gt;.
    /// </summary>
    /// <param name="ctor">Конструктор, для которого нужно получить текст тега returns.</param>
    /// <returns>Однострочное описание возвращаемого значения или null.</returns>
    public string GetConstructorReturns(ConstructorInfo ctor)
    {
        if (ctor == null || ctor.DeclaringType == null)
            return null;

        string typeName = NormalizeTypeName(ctor.DeclaringType.FullName);
        if (string.IsNullOrEmpty(typeName))
            return null;

        string baseKey = $"M:{typeName}.#ctor";

        string exact = Lookup(baseKey + "(" + BuildParameterList(ctor.GetParameters()) + ")", _returns);
        if (!string.IsNullOrEmpty(exact))
            return exact;

        // Границы "#ctor(" и "#ctor``" обязательны: голый префикс ".#ctor" вернул бы
        // первое попавшееся описание из словаря в неопределённом порядке обхода.
        return LookupByPrefix(_returns, baseKey + "(", baseKey + "``");
    }

    /// <summary>
    /// Находит описание конструктора по тегу &lt;summary&gt;.
    /// </summary>
    /// <param name="ctor">Конструктор, для которого нужно получить текст summary.</param>
    /// <returns>Однострочное описание или null.</returns>
    public string GetConstructorSummary(ConstructorInfo ctor)
    {
        return GetSummaryByMember(ctor == null ? null : ctor.DeclaringType, "#ctor", ctor);
    }

    /// <summary>
    /// Находит описания параметров метода по тегам &lt;param name="..."&gt;.
    /// </summary>
    /// <param name="method">Метод, параметры которого нужно описать.</param>
    /// <returns>Карта «имя параметра → описание»; пустая, если описаний нет.</returns>
    public Dictionary<string, string> GetMethodParams(MethodInfo method)
    {
        return GetParamsByMember(method == null ? null : method.DeclaringType, method == null ? null : method.Name, method);
    }

    /// <summary>
    /// Находит описания параметров конструктора по тегам &lt;param name="..."&gt;.
    /// </summary>
    /// <param name="ctor">Конструктор, параметры которого нужно описать.</param>
    /// <returns>Карта «имя параметра → описание»; пустая, если описаний нет.</returns>
    public Dictionary<string, string> GetConstructorParams(ConstructorInfo ctor)
    {
        return GetParamsByMember(ctor == null ? null : ctor.DeclaringType, "#ctor", ctor);
    }

    /// <summary>
    /// Общая реализация поиска текста &lt;returns&gt; для метода или конструктора.
    /// Дополнительный параметр methodOrCtor нужен только для построения списка типов
    /// параметров в идентификаторе документации.
    /// </summary>
    private string GetReturnsByMember(Type declaringType, string memberName, MethodBase methodOrCtor)
    {
        if (declaringType == null || string.IsNullOrEmpty(memberName) || methodOrCtor == null)
            return null;

        string typeName = NormalizeTypeName(declaringType.FullName);
        if (string.IsNullOrEmpty(typeName))
            return null;

        string baseKey = $"M:{typeName}.{memberName}";

        ParameterInfo[] parameters;
        try
        {
            parameters = methodOrCtor.GetParameters();
        }
        catch (Exception)
        {
            parameters = new ParameterInfo[0];
        }

        string exact = Lookup(baseKey + "(" + BuildParameterList(parameters) + ")", _returns);
        if (!string.IsNullOrEmpty(exact))
            return exact;

        return LookupByPrefix(_returns, baseKey + "(", baseKey + "``");
    }

    /// <summary>
    /// Общая реализация поиска текста &lt;summary&gt; для метода или конструктора.
    /// </summary>
    private string GetSummaryByMember(Type declaringType, string memberName, MethodBase methodOrCtor)
    {
        if (declaringType == null || string.IsNullOrEmpty(memberName) || methodOrCtor == null)
            return null;

        string typeName = NormalizeTypeName(declaringType.FullName);
        if (string.IsNullOrEmpty(typeName))
            return null;

        string baseKey = $"M:{typeName}.{memberName}";

        ParameterInfo[] parameters;
        try
        {
            parameters = methodOrCtor.GetParameters();
        }
        catch (Exception)
        {
            parameters = new ParameterInfo[0];
        }

        string exact = Lookup(baseKey + "(" + BuildParameterList(parameters) + ")", _docs);
        if (!string.IsNullOrEmpty(exact))
            return exact;

        return LookupByPrefix(_docs, baseKey + "(", baseKey + "``");
    }

    /// <summary>
    /// Общая реализация поиска описаний параметров для метода или конструктора.
    /// </summary>
    private Dictionary<string, string> GetParamsByMember(Type declaringType, string memberName, MethodBase methodOrCtor)
    {
        var empty = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        if (declaringType == null || string.IsNullOrEmpty(memberName) || methodOrCtor == null)
            return empty;

        string typeName = NormalizeTypeName(declaringType.FullName);
        if (string.IsNullOrEmpty(typeName))
            return empty;

        string baseKey = $"M:{typeName}.{memberName}";

        ParameterInfo[] parameters;
        try
        {
            parameters = methodOrCtor.GetParameters();
        }
        catch (Exception)
        {
            parameters = new ParameterInfo[0];
        }

        Dictionary<string, string> map;
        if (_params.TryGetValue(baseKey + "(" + BuildParameterList(parameters) + ")", out map))
            return map;

        // Fallback: первая перегрузка с описанием параметров. Порядок обхода словаря
        // не определён, поэтому для конструкторов вызывающий код передаёт "#ctor" —
        // ключи конструктора уникальны и совпадение с чужой перегрузкой невозможно.
        foreach (var pair in _params)
        {
            if (pair.Key.StartsWith(baseKey + "(", StringComparison.OrdinalIgnoreCase) ||
                pair.Key.StartsWith(baseKey + "``", StringComparison.OrdinalIgnoreCase))
                return pair.Value;
        }

        return empty;
    }

    /// <summary>
    /// Формирует список типов параметров метода в нотации идентификаторов XML-документации:
    /// полное имя типа, для массивов — суффикс "[]", для ref/out — суффикс "@",
    /// для generic-параметров — обратная кавычка с индексом ("`0").
    /// </summary>
    private static string BuildParameterList(MethodInfo method)
    {
        try
        {
            return BuildParameterList(method.GetParameters());
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// Формирует список типов параметров по массиву ParameterInfo.
    /// Отдельная перегрузка нужна конструкторам: MethodInfo для них недоступен,
    /// а идентификатор члена в XML-документации строится точно так же.
    /// </summary>
    private static string BuildParameterList(ParameterInfo[] parameters)
    {
        if (parameters == null || parameters.Length == 0)
            return string.Empty;

        var parts = new List<string>(parameters.Length);
        foreach (var p in parameters)
        {
            string parameterTypeName = NormalizeTypeName(p.ParameterType.FullName) ?? p.ParameterType.Name;

            if (p.ParameterType.IsByRef)
                parameterTypeName += "@";
            if (p.ParameterType.IsArray)
                parameterTypeName += "[]";

            parts.Add(parameterTypeName);
        }

        return string.Join(",", parts);
    }

    /// <summary>
    /// Приводит полное имя типа к виду, используемому в идентификаторах документации:
    /// вложенные типы разделяются точкой, а не символом '+'.
    /// Для закрытых generic-типов отбрасывается часть аргументов после обратной кавычки
    /// (рефлексия возвращает "Ns.Type`1[[System.String, mscorlib]]").
    /// </summary>
    private static string NormalizeTypeName(string fullName)
    {
        if (string.IsNullOrEmpty(fullName))
            return null;

        string result = fullName.Replace('+', '.');

        int bracketIndex = result.IndexOf('[');
        if (bracketIndex >= 0)
            result = result.Substring(0, bracketIndex);

        return result;
    }

    /// <summary>
    /// Возвращает описание по точному ключу либо null, если ключа нет.
    /// </summary>
    private string Lookup(string key)
    {
        return Lookup(key, _docs);
    }

    /// <summary>
    /// Возвращает первое описание, ключ которого начинается с одного из допустимых префиксов.
    /// Используется как fallback при поиске описания перегрузки метода, сигнатура которой
    /// в документации записана иначе, чем её представляет рефлексия.
    /// Префиксы задают границу имени метода, чтобы не подхватить описание похожего
    /// по началу имени члена (см. GetMethodSummary).
    /// </summary>
    /// <param name="prefixes">Допустимые префиксы идентификаторов документации.</param>
    /// <returns>Текст описания или null, если совпадений нет.</returns>
    private string LookupByPrefix(params string[] prefixes)
    {
        if (prefixes == null || prefixes.Length == 0)
            return null;

        return LookupByPrefix(_docs, prefixes);
    }

    /// <summary>
    /// Возвращает описание по точному ключу из указанного словаря XML-документации.
    /// Параметр source обязателен: ранее поиск был жёстко привязан к _docs,
    /// из-за чего текст тега &lt;returns&gt; физически не мог быть прочитан из _returns
    /// и в вывод вместо него попадало summary (см. GetReturnsByMember).
    /// </summary>
    private static string Lookup(string key, Dictionary<string, string> source)
    {
        if (source == null)
            return null;

        string value;
        return source.TryGetValue(key, out value) ? value : null;
    }

    /// <summary>
    /// Возвращает первое описание из указанного словаря, ключ которого начинается
    /// с одного из допустимых префиксов. Перегрузка с явным словарём нужна для поиска
    /// текста &lt;returns&gt; в _returns; прежняя версия без параметра всегда читала _docs.
    /// </summary>
    /// <param name="source">Словарь XML-документации, в котором выполняется поиск.</param>
    /// <param name="prefixes">Допустимые префиксы идентификаторов документации.</param>
    /// <returns>Текст описания или null, если совпадений нет.</returns>
    private static string LookupByPrefix(Dictionary<string, string> source, params string[] prefixes)
    {
        if (source == null || prefixes == null || prefixes.Length == 0)
            return null;

        foreach (var pair in source)
        {
            foreach (var prefix in prefixes)
            {
                if (string.IsNullOrEmpty(prefix))
                    continue;

                if (pair.Key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    return pair.Value;
            }
        }

        return null;
    }

    /// <summary>
    /// Нормализует текст тега summary в одну строку: убирает переносы строк,
    /// табуляции и повторяющиеся пробелы, обрезает краевые пробелы.
    /// Дополнительно удаляется префикс "cref:" и разделители generic-аргументов,
    /// которые могут попасть в текст из вложенных XML-тегов (paramref/see cref).
    /// </summary>
    private static string CleanText(string summaryText)
    {
        if (string.IsNullOrEmpty(summaryText))
            return null;

        string cleaned = Regex.Replace(summaryText, @"\s+", " ").Trim();

        // Краткие "мусорные" описания вида "To be added." не несут пользы для RAG — отбрасываем.
        if (cleaned.Length == 0 || cleaned.Equals("To be added.", StringComparison.OrdinalIgnoreCase))
            return null;

        return cleaned;
    }
}