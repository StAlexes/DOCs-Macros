// Макрос оставлен в глобальном пространстве имен, поскольку загрузчик T-FLEX DOCs
// находит MacroProvider по типу сборки, а не по имени пространства имен.
//
// Архитектура макроса:
//   XmiSchemaExporterMacro  — точка входа (MacroProvider): стартовый диалог и оркестрация.
//   ModelCollector          — сбор модели DOCs (2 режима выгрузки, прямой вызов API).
//   XmiDocumentWriter       — изоляция генерации готового XMI 2.1 / Enterprise Architect.
//
// Вся работа с моделью данных T-FLEX DOCs выполняется ТОЛЬКО через прямые свойства API
// (ReferenceCatalog.GetReferences, ReferenceInfo.Classes.AllClasses, ClassObject.ParameterGroups,
// ParameterInfo.Type.IsInt и т.д.). Универсальные методы-обёртки рефлексии удалены.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using TFlex.DOCs.Model;
using TFlex.DOCs.Model.Classes;
using TFlex.DOCs.Model.Macros;
using TFlex.DOCs.Model.Macros.ObjectModel;
using TFlex.DOCs.Model.References.Events;
using TFlex.DOCs.Model.Structure;

/// <summary>Режим выгрузки схемы справочников T-FLEX DOCs в XMI.</summary>
public enum ExportMode
{
    /// <summary>1. Выбор справочников (штатный элемент выборки со встроенным «Выбрать все»).</summary>
    SelectedReferences,

    /// <summary>2. Выбор конкретных типов и их параметров.</summary>
    SelectedTypes
}

/// <summary>Параметры выгрузки, собираемые в стартовом диалоге макроса.</summary>
public sealed class ExportOptions
{
    /// <summary>Выбранный режим выгрузки.</summary>
    public ExportMode Mode { get; set; } = ExportMode.SelectedTypes;

    /// <summary>Флаг выгрузки комментариев (ownedComment/documentation).</summary>
    public bool ExportComments { get; set; } = true;

    /// <summary>Флаг выгрузки событий (UML Signal).</summary>
    public bool ExportEvents { get; set; } = false;

    /// <summary>Флаг выгрузки опций настройки структуры (раздел tags в xmi:Extension).</summary>
    public bool ExportSettings { get; set; } = true;
}

/// <summary>Выбор одного справочника, его типов, параметров и параметров подключения.</summary>
public sealed class CatalogSelection
{
    /// <summary>Справочник DOCs, выбранный для экспорта.</summary>
    public ReferenceInfo Reference { get; set; }

    /// <summary>Типы объектов выбранного справочника (прямые объекты ClassObject модели).</summary>
    public List<ClassObject> Classes { get; set; } = new List<ClassObject>();

    /// <summary>Параметры, выбранные для включения в экспорт.</summary>
    public List<ParameterInfo> Parameters { get; set; } = new List<ParameterInfo>();

    /// <summary>Группа структурного подключения сложной иерархии, если она присутствует.</summary>
    public ParameterGroup ConnectionGroup { get; set; }

    /// <summary>Отдельно выбранные параметры группы структурного подключения.</summary>
    public List<ParameterInfo> ConnectionParameters { get; set; } = new List<ParameterInfo>();

    /// <summary>Автоматически собранные связи справочника (режим выбора справочников целиком).</summary>
    public List<ParameterGroup> SelectedLinks { get; set; } = new List<ParameterGroup>();

    /// <summary>Параметры всех автоматически собранных связей справочника.</summary>
    public List<ParameterInfo> SelectedLinkParameters { get; set; } = new List<ParameterInfo>();
}

/// <summary>Связь DOCs между выбранными каталогами с зафиксированными master/slave-контекстами.</summary>
public sealed class SelectedRelation
{
    /// <summary>Группа параметров, описывающая связь в DOCs.</summary>
    public ParameterGroup Link { get; set; }

    /// <summary>Каталог-владелец (master) связи.</summary>
    public CatalogSelection Master { get; set; }

    /// <summary>Целевой каталог (slave) связи.</summary>
    public CatalogSelection Slave { get; set; }

    /// <summary>Признак, что связь относится к группе структурных подключений каталога.</summary>
    public bool IsConnectionRelation { get; set; }
}

/// <summary>Контекстное представление связи, полученное при опросе одного справочника.</summary>
public sealed class CatalogRelationItem
{
    /// <summary>Группа связи DOCs с именем в контексте опрашиваемого справочника.</summary>
    public ParameterGroup Link { get; set; }

    /// <summary>Справочник, у которого запрошена связь.</summary>
    public CatalogSelection SourceCatalog { get; set; }

    /// <summary>Связанный справочник, являющийся целью связи.</summary>
    public CatalogSelection TargetCatalog { get; set; }

    /// <summary>Признак связи, полученной из группы структурного подключения.</summary>
    public bool IsConnectionRelation { get; set; }

    /// <summary>Ключ флажка для чтения выбора из диалога.</summary>
    public string DialogKey { get; set; }
}

/// <summary>Готовая UML/EA-модель выгрузки, передаваемая генератору XML.</summary>
public sealed class ExportModel
{
    /// <summary>Использованные параметры выгрузки.</summary>
    public ExportOptions Options { get; set; }

    /// <summary>Ключ, по которому вычисляется детерминированный GUID корневого пакета.</summary>
    public string PackageKey { get; set; }

    /// <summary>Экспортируемые справочники-компоненты.</summary>
    public List<CatalogModel> Catalogs { get; set; } = new List<CatalogModel>();

    /// <summary>Экспортируемые типы (UML-классы) со своими параметрами.</summary>
    public List<ClassModel> Classes { get; set; } = new List<ClassModel>();

    /// <summary>Ассоциации между типами выбранных справочников.</summary>
    public List<AssocModel> Assocs { get; set; } = new List<AssocModel>();

    /// <summary>Ассоциации «справочник содержит тип».</summary>
    public List<CatalogTypeAssocModel> CatalogTypeAssocs { get; set; } = new List<CatalogTypeAssocModel>();
}

/// <summary>Промежуточная UML-модель типа DOCs и его атрибутов/ассоциаций.</summary>
public sealed class ClassModel
{
    /// <summary>Идентификатор XMI/EA класса.</summary>
    public string Guid { get; set; }
    /// <summary>Исходный GUID сущности DOCs (ClassObject или ParameterGroup «Подключение»).</summary>
    public Guid DocsGuid { get; set; }
    /// <summary>Имя типа DOCs.</summary>
    public string Name { get; set; }
    /// <summary>Комментарий типа.</summary>
    public string Comment { get; set; }
    /// <summary>Идентификатор справочника-владельца.</summary>
    public string CatalogGuid { get; set; }
    /// <summary>Указывает, что класс является псевдотипом структурного подключения.</summary>
    public bool IsConnection { get; set; }
    /// <summary>Исходный объект модели T-FLEX DOCs (только для вычисления связей и наследования).</summary>
    public ClassObject SourceClass { get; set; }
    /// <summary>Атрибуты (параметры) класса.</summary>
    public List<ParamModel> Parameters { get; set; } = new List<ParamModel>();
    /// <summary>Ассоциации, выходящие из класса.</summary>
    public List<AssocModel> Associations { get; set; } = new List<AssocModel>();
}

/// <summary>Промежуточная UML-модель справочника, его настроек и событий.</summary>
public sealed class CatalogModel
{
    /// <summary>Идентификатор XMI/EA компонента-справочника.</summary>
    public string Guid { get; set; }
    /// <summary>Исходный GUID сущности DOCs (ReferenceInfo).</summary>
    public Guid DocsGuid { get; set; }
    /// <summary>Имя справочника.</summary>
    public string Name { get; set; }
    /// <summary>Комментарий справочника.</summary>
    public string Comment { get; set; }
    /// <summary>Пользовательские настройки структуры для tagged values.</summary>
    public List<KeyValuePair<string, string>> Settings { get; set; } = new List<KeyValuePair<string, string>>();
    /// <summary>События (UML Signal) справочника.</summary>
    public List<EventModel> Events { get; set; } = new List<EventModel>();
}

/// <summary>Промежуточное представление события DOCs.</summary>
public sealed class EventModel
{
    /// <summary>Идентификатор XMI/EA события.</summary>
    public string Guid { get; set; }
    /// <summary>Исходный GUID сущности DOCs (ParameterGroupEvent).</summary>
    public Guid DocsGuid { get; set; }
    /// <summary>Имя события.</summary>
    public string Name { get; set; }
    /// <summary>Комментарий события.</summary>
    public string Comment { get; set; }
    /// <summary>Идентификатор справочника-владельца события.</summary>
    public string CatalogGuid { get; set; }
}

/// <summary>Промежуточное представление параметра DOCs как UML Property.</summary>
public sealed class ParamModel
{
    /// <summary>Идентификатор XMI/EA атрибута.</summary>
    public string Guid { get; set; }
    /// <summary>Исходный GUID сущности DOCs (ParameterInfo).</summary>
    public Guid DocsGuid { get; set; }
    /// <summary>Имя параметра.</summary>
    public string Name { get; set; }
    /// <summary>XMI idref стандартного типа UML.</summary>
    public string TypeRef { get; set; }
    /// <summary>Тип атрибута Enterprise Architect.</summary>
    public string EaType { get; set; }
}

/// <summary>Промежуточное представление ассоциации между классами и ролей её концов.</summary>
public sealed class AssocModel
{
    public string AssocGuid { get; set; }
    public string SrcPropGuid { get; set; }
    public string DstPropGuid { get; set; }
    public string Name { get; set; }
    public string SourceClassGuid { get; set; }
    public string SourceClassName { get; set; }
    public string TargetClassGuid { get; set; }
    public string TargetClassName { get; set; }
    /// <summary>Роль на конце источника, например имя связи со стороны Master.</summary>
    public string SourceRole { get; set; }
    /// <summary>Роль на конце приёмника, например имя связи со стороны Slave.</summary>
    public string TargetRole { get; set; }
    public string SourceLower { get; set; } = "0";
    public string SourceUpper { get; set; } = "-1";
    public string TargetLower { get; set; } = "0";
    public string TargetUpper { get; set; } = "-1";
    /// <summary>Направление ассоциации; по умолчанию стрелка не задаётся.</summary>
    public string Direction { get; set; } = "Unspecified";
}

/// <summary>Ассоциация EA, связывающая компонент справочника с UML-классом его типа.</summary>
public sealed class CatalogTypeAssocModel
{
    public string Guid { get; set; }
    public string CatalogGuid { get; set; }
    public string TypeGuid { get; set; }
    public string CatalogName { get; set; }
    public string TypeName { get; set; }
}


/// <summary>
/// Макрос T-FLEX DOCs, который экспортирует выбранные справочники, типы, параметры и связи
/// в XMI 2.1 с метаданными и профилем, совместимыми с Enterprise Architect.
/// </summary>
public class XmiSchemaExporterMacro : MacroProvider
{
    /// <summary>Создаёт макрос экспорта модели DOCs в XMI.</summary>
    /// <param name="context">Контекст выполнения макроса и подключения к DOCs.</param>
    public XmiSchemaExporterMacro(MacroContext context) : base(context) { }

    internal const string ModeField = "Режим выгрузки";
    internal const string ModeOptionRefs = "1. Выгрузка указанных справочников";
    internal const string ModeOptionTypes = "2. Выгрузка указанных типов";
    // Заголовок группы ОБЯЗАН отличаться от ModeField: поиск элемента в диалоге идёт по имени
    // и возвращает первый совпавший, поэтому одноимённая группа перекрывает поле выбора режима.
    internal const string ModeGroupCaption = "Выбор режима";
    internal const string CommentsKey = "Выгружать комментарии";
    internal const string EventsKey = "Выгружать события";
    internal const string SettingsKey = "Выгружать опции настройки";

    /// <summary>Запускает стартовый диалог, сбор модели данных и генерацию XMI-файла.</summary>
    public override void Run()
    {
        ExportOptions options = ShowStartupDialog();
        if (options == null) return;

        var collector = new ModelCollector(this, options);
        ExportModel model = collector.CollectData();
        if (model == null) return;

        var writer = new XmiDocumentWriter(this, model);
        writer.WriteToFile();
    }

    /// <summary>
    /// Показывает первичный диалог выбора режима выгрузки и флагов настроек.
    /// </summary>
    /// <returns>Заполненные параметры либо null, если пользователь отменил ввод.</returns>
    private ExportOptions ShowStartupDialog()
    {
        var dialog = СоздатьДиалогВвода("Экспорт схемы T-FLEX DOCs в XMI");
        dialog.Ширина = 540;
        dialog.Высота = 340;

        dialog.ДобавитьГруппу(ModeGroupCaption);
        // ВАЖНО: перегрузка без значения по умолчанию оставляет поле пустым (GetValue вернёт null),
        // поэтому явно задаём предустановленный режим и делаем поле обязательным.
        dialog.ДобавитьВыборИзСписка(ModeField, ModeOptionRefs, true, new object[]
        {
            ModeOptionRefs,
            ModeOptionTypes
        });

        dialog.ДобавитьГруппу("Настройки");
        dialog.ДобавитьФлаг(CommentsKey, true, false, false);
        dialog.ДобавитьФлаг(EventsKey, false, false, false);
        dialog.ДобавитьФлаг(SettingsKey, true, false, false);

        if (!dialog.Показать()) return null;

        return new ExportOptions
        {
            Mode = ParseMode(dialog[ModeField]),
            ExportComments = ReadFlag(dialog, CommentsKey, true),
            ExportEvents = ReadFlag(dialog, EventsKey, false),
            ExportSettings = ReadFlag(dialog, SettingsKey, true)
        };
    }

    /// <summary>Сопоставляет выбранный пункт списка режимов с режимом выгрузки.</summary>
    /// <param name="value">Значение поля режима из диалога (текст пункта либо его индекс).</param>
    /// <returns>Режим выгрузки; при нераспознанном значении — выбор справочников.</returns>
    private static ExportMode ParseMode(object value)
    {
        string text = Convert.ToString(value)?.Trim() ?? string.Empty;

        if (string.Equals(text, ModeOptionRefs, StringComparison.OrdinalIgnoreCase)) return ExportMode.SelectedReferences;
        if (string.Equals(text, ModeOptionTypes, StringComparison.OrdinalIgnoreCase)) return ExportMode.SelectedTypes;

        // Некоторые сборки DOCs возвращают из выпадающего списка индекс пункта, а не его текст.
        if (int.TryParse(text, out int index))
        {
            switch (index)
            {
                case 0: return ExportMode.SelectedReferences;
                case 1: return ExportMode.SelectedTypes;
            }
        }

        // Пустое значение трактуем как предустановленный (первый) пункт списка — выбор справочников.
        return ExportMode.SelectedReferences;
    }

    /// <summary>Считывает значение флажка из диалога ввода с безопасным значением по умолчанию.</summary>
    /// <param name="dialog">Диалог ввода DOCs.</param>
    /// <param name="key">Имя поля-флажка.</param>
    /// <param name="fallback">Значение при недоступности поля.</param>
    /// <returns>Состояние флажка.</returns>
    internal static bool ReadFlag(ДиалогВвода dialog, string key, bool fallback)
    {
        try
        {
            object value = dialog.Значение(key);
            return value == null ? fallback : Convert.ToBoolean(value);
        }
        catch
        {
            return fallback;
        }
    }
}


/// <summary>
/// Собирает промежуточную UML/EA-модель из справочников, типов, параметров и связей T-FLEX DOCs.
/// Работает с моделью только через прямые свойства API, без универсальных обёрток рефлексии.
/// </summary>
internal sealed class ModelCollector
{
    private readonly MacroProvider provider;
    private readonly MacroContext context;
    private readonly ExportOptions options;

    /// <summary>Создаёт сборщик данных для указанного MacroProvider и параметров выгрузки.</summary>
    /// <param name="provider">Экземпляр макроса, предоставляющий штатные диалоги DOCs.</param>
    /// <param name="options">Параметры выгрузки из стартового диалога.</param>
    public ModelCollector(MacroProvider provider, ExportOptions options)
    {
        this.provider = provider;
        this.context = provider.Context;
        this.options = options ?? new ExportOptions();
    }

    /// <summary>Выполняет сбор данных согласно выбранному режиму выгрузки.</summary>
    /// <returns>Готовая модель выгрузки либо null при отмене ввода или пустом выборе.</returns>
    public ExportModel CollectData()
    {
        var data = new CollectorData();

        switch (options.Mode)
        {
            case ExportMode.SelectedReferences:
                // Режим 1: только диалог выбора справочников (со встроенным «Выбрать все»);
                // типы, параметры и связи — автоматически.
                CollectSelectedReferencesModel(data);
                break;

            case ExportMode.SelectedTypes:
            default:
                // Режим 2: пошаговый интерактивный выбор справочника, типов, параметров и связей.
                CollectInteractiveModel(data);
                break;
        }

        if (data.Selections.Count == 0) return null;
        return BuildModel(data.Selections, data.Relations);
    }

    /// <summary>Накопитель результатов сбора данных, общий для всех режимов выгрузки.</summary>
    private sealed class CollectorData
    {
        /// <summary>Собранные контексты справочников (типы, параметры, параметры подключения).</summary>
        public List<CatalogSelection> Selections { get; } = new List<CatalogSelection>();

        /// <summary>Собранные связи между справочниками с зафиксированными master/slave-контекстами.</summary>
        public List<SelectedRelation> Relations { get; } = new List<SelectedRelation>();
    }

    /// <summary>Режим 1: выбор справочников одним диалогом (со встроенным «Выбрать все»); типы, параметры и связи — автоматически.</summary>
    /// <param name="data">Накопитель, куда записываются выбранные справочники, типы, параметры и связи.</param>
    private void CollectSelectedReferencesModel(CollectorData data)
    {
        List<ReferenceInfo> chosenReferences = SelectReferencesMultiselect();
        if (chosenReferences.Count == 0)
        {
            Inform("Не выбран ни один справочник.");
            return;
        }

        // Диалоги выбора типов (SelectObjectTypes) и параметров (SelectParameters) НЕ вызываются:
        // берутся все типы Classes.AllClasses и все параметры Description.Parameters.
        data.Selections.AddRange(BuildFullSelections(chosenReferences));
        if (data.Selections.Count == 0)
        {
            Inform("Не найден ни один справочник с типами объектов для экспорта.");
            return;
        }

        // ИСПРАВЛЕНИЕ: в режиме выбора справочников целиком связи не выбираются вручную,
        // поэтому автоматически собираем ВСЕ связи каждого справочника и их параметры.
        PopulateSelectedLinks(data.Selections);

        data.Relations.AddRange(CollectRelations(data.Selections, false));
    }

    /// <summary>
    /// Автоматически наполняет <see cref="CatalogSelection.SelectedLinks"/> всеми связями
    /// справочника и собирает параметры этих связей. Используется в режиме выбора справочников целиком.
    /// </summary>
    /// <param name="selections">Контексты выгружаемых справочников.</param>
    private static void PopulateSelectedLinks(IEnumerable<CatalogSelection> selections)
    {
        if (selections == null) return;

        foreach (CatalogSelection catalog in selections)
        {
            if (catalog?.Reference?.Description == null)
            {
                catalog.SelectedLinks = new List<ParameterGroup>();
                catalog.SelectedLinkParameters = new List<ParameterInfo>();
                continue;
            }

            // 1. Забираем все связи справочника через прямой API Description.GetLinks().
            var allLinks = catalog.Reference.Description.GetLinks();
            catalog.SelectedLinks = allLinks?.Where(link => link != null).ToList()
                                    ?? new List<ParameterGroup>();

            // 2. Для каждой связи включаем ВСЕ её параметры.
            catalog.SelectedLinkParameters = new List<ParameterInfo>();
            var seenParameters = new HashSet<Guid>();
            foreach (ParameterGroup link in catalog.SelectedLinks)
            {
                if (link.Parameters == null) continue;
                foreach (ParameterInfo parameter in link.Parameters)
                {
                    if (parameter == null || !seenParameters.Add(parameter.Guid)) continue;
                    catalog.SelectedLinkParameters.Add(parameter);
                }
            }
        }
    }

    /// <summary>Режим 2: пошаговый интерактивный выбор справочников, типов, параметров и связей.</summary>
    /// <param name="data">Накопитель, куда записывается результат ручного выбора.</param>
    private void CollectInteractiveModel(CollectorData data)
    {
        List<CatalogSelection> selections = CollectSelectedTypes();
        if (selections.Count == 0)
        {
            Inform("Не выбран ни один справочник с типами объектов для экспорта.");
            return;
        }

        data.Selections.AddRange(selections);
        data.Relations.AddRange(CollectRelations(selections, true));
    }

    /// <summary>Возвращает все загруженные справочники системы.</summary>
    /// <returns>Список справочников без null-элементов.</returns>
    private List<ReferenceInfo> GetAvailableReferences()
    {
        return context.Connection.ReferenceCatalog.GetReferences()
            ?.Where(item => item != null).ToList() ?? new List<ReferenceInfo>();
    }


    /// <summary>Собирает полный набор типов и параметров указанных справочников без диалогов выбора.</summary>
    /// <param name="references">Справочники, для которых берутся все типы и параметры.</param>
    /// <returns>Полные контексты выбранных справочников.</returns>
    private List<CatalogSelection> BuildFullSelections(IEnumerable<ReferenceInfo> references)
    {
        var result = new List<CatalogSelection>();
        foreach (ReferenceInfo reference in references?.Where(item => item != null) ?? Enumerable.Empty<ReferenceInfo>())
        {
            ClassTree classTree = reference.Classes;
            List<ClassObject> classes = classTree?.AllClasses?.Where(item => item != null).ToList()
                                       ?? new List<ClassObject>();
            ParameterGroup connectionGroup = FindConnectionGroup(reference);

            result.Add(new CatalogSelection
            {
                Reference = reference,
                Classes = classes,
                Parameters = CollectAllParameters(reference.Description, connectionGroup),
                ConnectionGroup = connectionGroup,
                ConnectionParameters = connectionGroup != null
                    ? CollectVisibleGroupParameters(connectionGroup)
                    : new List<ParameterInfo>()
            });
        }
        return result;
    }

    /// <summary>Показывает единственный многовыборный диалог выбора справочников (чекбоксы).</summary>
    /// <returns>Выбранные справочники; пустой список при отмене.</returns>
    private List<ReferenceInfo> SelectReferencesMultiselect()
    {
        List<ReferenceInfo> all = GetAvailableReferences();
        if (all.Count == 0) return new List<ReferenceInfo>();

        List<ReferenceInfo> ordered = all
            .OrderBy(item => item.Name, StringComparer.CurrentCultureIgnoreCase)
            .ToList();
        object[] names = ordered.Select(item => (object)item.Name).ToArray();

        var dialog = provider.СоздатьДиалогВвода("Шаг 1: Выберите справочники для выгрузки");
        dialog.Ширина = 560;
        dialog.Высота = 480;
        dialog.ОтобразитьПолосыПрокрутки(true, false);
        const string key = "Справочники";
        dialog.ДобавитьМножественныйВыборИзСписка(key, names, false);
        if (!dialog.Показать()) return new List<ReferenceInfo>();

        var selectedNames = new HashSet<string>(StringComparer.Ordinal);
        object value = dialog.Значение(key);
        if (value is object[] arrayValue)
        {
            foreach (object item in arrayValue)
            {
                string name = Convert.ToString(item);
                if (!string.IsNullOrEmpty(name)) selectedNames.Add(name);
            }
        }
        else if (value is System.Collections.IEnumerable enumerable && !(value is string))
        {
            foreach (object item in enumerable)
            {
                string name = Convert.ToString(item);
                if (!string.IsNullOrEmpty(name)) selectedNames.Add(name);
            }
        }

        return ordered.Where(item => selectedNames.Contains(item.Name)).ToList();
    }

    /// <summary>Показывает диалог выбора одного справочника.</summary>
    /// <param name="isFirstSelection">true для первого шага: список не ограничивается.</param>
    /// <param name="selectedReferences">Справочники, уже выбранные в текущем цикле.</param>
    /// <param name="allReferences">Полный список справочников системы.</param>
    /// <param name="linkedCache">Кэш структурных связей справочников.</param>
    /// <returns>Выбранный справочник либо null при отмене.</returns>
    private ReferenceInfo SelectReference(
        bool isFirstSelection,
        IList<ReferenceInfo> selectedReferences,
        IList<ReferenceInfo> allReferences,
        IDictionary<Guid, HashSet<Guid>> linkedCache)
    {
        bool useOnlyDependent = false;
        if (!isFirstSelection && selectedReferences != null && selectedReferences.Count > 0)
            useOnlyDependent = provider.Вопрос("Выбирать только из списка зависимых/связанных справочников?");

        var dialog = provider.СоздатьДиалогВвода(
            useOnlyDependent ? "Выбор зависимого справочника" : "Выбор справочника");
        dialog.Ширина = 560;
        dialog.Высота = 320;
        const string key = "Справочник";

        List<ReferenceInfo> dependent = null;
        if (useOnlyDependent)
        {
            dependent = GetDependentReferences(selectedReferences, allReferences, linkedCache);
            if (dependent.Count == 0)
            {
                Inform("Зависимые справочники не найдены. Будет открыт полный список справочников.");
                dialog.ДобавитьВыборСправочника(key, null, true);
            }
            else
            {
                object[] dependentNames = dependent.Select(item => (object)item.Name).ToArray();
                dialog.ДобавитьВыборИзСписка(key, null, true, dependentNames);
            }
        }
        else
        {
            dialog.ДобавитьВыборСправочника(key, null, true);
        }

        if (!dialog.Показать()) return null;

        object value = dialog.Значение(key);
        if (value is ReferenceInfo info) return info;

        string selectedName = Convert.ToString(value);
        if (string.IsNullOrWhiteSpace(selectedName)) return null;

        // Поле-список возвращает имя строкой, поэтому сначала ищем в уже отобранных зависимых,
        // и только затем обращаемся к каталогу (там имя может совпасть у разных справочников).
        ReferenceInfo byName = dependent?.FirstOrDefault(item =>
            string.Equals(item.Name, selectedName, StringComparison.Ordinal));
        return byName ?? context.Connection.ReferenceCatalog.Find(selectedName);
    }

    /// <summary>Отбирает справочники, структурно связанные с уже выбранными.</summary>
    /// <param name="selectedReferences">Справочники, уже выбранные в текущем цикле.</param>
    /// <param name="allReferences">Полный список справочников системы.</param>
    /// <param name="linkedCache">Кэш структурных связей справочников.</param>
    /// <returns>Связанные справочники, исключая уже выбранные, отсортированные по имени.</returns>
    private static List<ReferenceInfo> GetDependentReferences(
        IList<ReferenceInfo> selectedReferences,
        IList<ReferenceInfo> allReferences,
        IDictionary<Guid, HashSet<Guid>> linkedCache)
    {
        var result = new List<ReferenceInfo>();
        if (selectedReferences == null || allReferences == null || linkedCache == null) return result;

        var selectedGuids = new HashSet<Guid>(
            selectedReferences.Where(item => item != null).Select(item => item.Guid));
        var allowedGuids = new HashSet<Guid>();
        foreach (Guid selectedGuid in selectedGuids)
            if (linkedCache.TryGetValue(selectedGuid, out var neighbors) && neighbors != null)
                allowedGuids.UnionWith(neighbors);

        foreach (ReferenceInfo reference in allReferences.Where(item => item != null))
        {
            if (selectedGuids.Contains(reference.Guid)) continue;
            if (allowedGuids.Contains(reference.Guid)) result.Add(reference);
        }

        return result
            .OrderBy(item => item.Name, StringComparer.CurrentCultureIgnoreCase)
            .ToList();
    }


    /// <summary>Пошаговый ручной выбор справочников, типов, параметров и параметров подключения.</summary>
    /// <returns>Контексты выбранных справочников с типами и параметрами.</returns>
    private List<CatalogSelection> CollectSelectedTypes()
    {
        var selections = new List<CatalogSelection>();
        List<ReferenceInfo> allReferences = context.Connection.ReferenceCatalog.GetReferences()
            ?.Where(item => item != null).ToList() ?? new List<ReferenceInfo>();
        Dictionary<Guid, HashSet<Guid>> linkedCache = BuildReferenceLinkCache(allReferences);

        while (true)
        {
            if (selections.Count > 0)
            {
                bool? addMore = provider.ВопросСОтменой(BuildAddMoreTypesPrompt(selections));
                if (addMore != true) break;
            }

            ReferenceInfo reference = SelectReference(
                selections.Count == 0,
                selections.Select(item => item.Reference).ToList(),
                allReferences,
                linkedCache);
            if (reference == null) break;

            if (selections.Any(item => item.Reference.Guid == reference.Guid))
            {
                Inform($"Справочник \"{reference.Name}\" уже выбран. Выберите другой справочник.");
                continue;
            }

            ТипОбъекта[] types = SelectObjectTypes(reference.Name,
                $"Шаг 2: Выберите типы объектов справочника '{reference.Name}'");
            if (types.Length > 0)
            {
                ParameterGroup connectionGroup = FindConnectionGroup(reference);
                selections.Add(new CatalogSelection
                {
                    Reference = reference,
                    Classes = MapTypesToClasses(reference, types),
                    Parameters = SelectParameters(reference, connectionGroup,
                        $"Шаг 3: Выберите параметры справочника '{reference.Name}'"),
                    ConnectionGroup = connectionGroup,
                    ConnectionParameters = connectionGroup != null
                        ? SelectConnectionParameters(reference, connectionGroup,
                            $"Шаг 3.1: Выберите параметры подключения для '{reference.Name}'")
                        : new List<ParameterInfo>()
                });
            }

            if (types.Length == 0 && selections.Count == 0)
            {
                bool? tryAgain = provider.ВопросСОтменой("Не выбраны типы объектов. Выбрать другой справочник?");
                if (tryAgain != true) break;
            }
        }
        return selections;
    }

    /// <summary>Показывает штатный диалог выбора типов объектов справочника.</summary>
    /// <param name="referenceName">Имя справочника, чьи типы предлагаются.</param>
    /// <param name="title">Заголовок диалога выбора.</param>
    /// <returns>Выбранные типы или пустой массив при отмене.</returns>
    private ТипОбъекта[] SelectObjectTypes(string referenceName, string title)
    {
        ДиалогВыбораТипов dialog = provider.СоздатьДиалогВыбораТипов(referenceName);
        dialog.Заголовок = title;
        dialog.ВыборАбстрактныхТипов = true;
        dialog.ВыборФлажками = true;
        dialog.АвтоВыборФлажками = false;
        return dialog.Показать() ? (dialog.ВыбранныеТипы ?? new ТипОбъекта[0]) : new ТипОбъекта[0];
    }

    /// <summary>Сопоставляет выбранные обёртки типов с объектами ClassObject модели DOCs.</summary>
    /// <param name="reference">Справочник-владелец типов.</param>
    /// <param name="types">Выбранные обёртки типов ТипОбъекта.</param>
    /// <returns>Найденные типы ClassObject без повторов по GUID.</returns>
    private static List<ClassObject> MapTypesToClasses(ReferenceInfo reference, ТипОбъекта[] types)
    {
        var result = new List<ClassObject>();
        var source = reference?.Classes?.AllClasses;
        if (source == null || types == null) return result;

        foreach (ТипОбъекта type in types)
        {
            if (type == null) continue;
            Guid typeGuid = type.Guid;
            string typeName = type.Name;
            ClassObject match = source.FirstOrDefault(candidate => candidate != null &&
                ((typeGuid != Guid.Empty && candidate.Guid == typeGuid) ||
                 (!string.IsNullOrEmpty(typeName) && string.Equals(candidate.Name, typeName, StringComparison.Ordinal))));
            if (match != null && !result.Any(item => item.Guid == match.Guid))
                result.Add(match);
        }
        return result;
    }


    /// <summary>Собирает видимые параметры всех групп справочника, исключая группу подключения.</summary>
    /// <param name="description">Корневая группа описания справочника.</param>
    /// <param name="connectionGroup">Группа подключения, которую следует исключить.</param>
    /// <returns>Уникальные видимые параметры справочника.</returns>
    private static List<ParameterInfo> CollectVisibleParameters(ParameterGroup description, ParameterGroup connectionGroup)
    {
        ParameterGroupCollection groups = description?.GetAllGroups();
        if (groups == null) return new List<ParameterInfo>();

        Guid connectionGuid = connectionGroup?.Guid ?? Guid.Empty;
        return groups
            .Where(group => group != null && (connectionGuid == Guid.Empty || group.Guid != connectionGuid))
            .SelectMany(IterateParameters)
            .Where(parameter => parameter != null && parameter.IsVisible)
            .GroupBy(parameter => parameter.Guid)
            .Select(group => group.First())
            .ToList();
    }

    /// <summary>Собирает видимые параметры одной группы.</summary>
    /// <param name="group">Группа параметров DOCs.</param>
    /// <returns>Уникальные видимые параметры группы.</returns>
    private static List<ParameterInfo> CollectVisibleGroupParameters(ParameterGroup group)
    {
        return IterateParameters(group)
            .Where(parameter => parameter != null && parameter.IsVisible)
            .GroupBy(parameter => parameter.Guid)
            .Select(grouping => grouping.First())
            .ToList();
    }

    /// <summary>Собирает ВСЕ параметры справочника (корневой группы и всех групп), исключая группу подключения.</summary>
    /// <param name="description">Корневая группа описания справочника.</param>
    /// <param name="connectionGroup">Группа подключения, выносимая в отдельный класс подключения.</param>
    /// <returns>Уникальные параметры справочника без фильтра видимости.</returns>
    private static List<ParameterInfo> CollectAllParameters(ParameterGroup description, ParameterGroup connectionGroup)
    {
        var result = new List<ParameterInfo>();
        if (description == null) return result;

        Guid connectionGuid = connectionGroup?.Guid ?? Guid.Empty;
        var seen = new HashSet<Guid>();

        Action<ParameterGroup> append = group =>
        {
            if (group?.Parameters == null) return;
            if (connectionGuid != Guid.Empty && group.Guid == connectionGuid) return;
            foreach (ParameterInfo parameter in group.Parameters)
            {
                if (parameter == null || !seen.Add(parameter.Guid)) continue;
                result.Add(parameter);
            }
        };

        append(description);
        ParameterGroupCollection groups = description.GetAllGroups();
        if (groups != null)
        {
            foreach (ParameterGroup group in groups)
                append(group);
        }

        return result;
    }

    /// <summary>Возвращает коллекцию параметров группы как последовательность.</summary>
    /// <param name="group">Группа параметров DOCs.</param>
    /// <returns>Параметры группы либо пустая последовательность.</returns>
    private static IEnumerable<ParameterInfo> IterateParameters(ParameterGroup group)
    {
        if (group?.Parameters == null) return Enumerable.Empty<ParameterInfo>();
        return group.Parameters;
    }

    /// <summary>Показывает диалог выбора параметров справочника.</summary>
    /// <param name="reference">Справочник, чьи параметры выбираются.</param>
    /// <param name="connectionGroup">Группа подключения, исключаемая из списка.</param>
    /// <param name="title">Заголовок диалога.</param>
    /// <returns>Отмеченные параметры.</returns>
    private List<ParameterInfo> SelectParameters(ReferenceInfo reference, ParameterGroup connectionGroup, string title)
    {
        List<ParameterInfo> parameters = CollectVisibleParameters(reference?.Description, connectionGroup);
        return ShowParameterSelectionDialog(parameters, title, "[v] Выбрать все параметры", reference?.Name);
    }

    /// <summary>Показывает отдельный диалог выбора параметров группы подключения.</summary>
    /// <param name="reference">Справочник-владелец группы подключения (для приоритета базовой группы).</param>
    /// <param name="connectionGroup">Группа подключения сложной иерархии.</param>
    /// <param name="title">Заголовок диалога.</param>
    /// <returns>Отмеченные параметры подключения.</returns>
    private List<ParameterInfo> SelectConnectionParameters(ReferenceInfo reference, ParameterGroup connectionGroup, string title)
    {
        List<ParameterInfo> parameters = CollectVisibleGroupParameters(connectionGroup);
        // Для параметров подключения флаг «выбрать все» не выводится: выбор выполняется
        // ТОЛЬКО элементом множественного выбора из списка (selectAllKey = null).
        return ShowParameterSelectionDialog(parameters, title, null, reference?.Name);
    }

    /// <summary>Блок параметров одной группы параметров для построения диалога выбора.</summary>
    private sealed class ParameterGroupBlock
    {
        /// <summary>Исходная группа параметров DOCs.</summary>
        public ParameterGroup Group { get; set; }
        /// <summary>Отображаемое имя группы.</summary>
        public string Name { get; set; }
        /// <summary>Отсортированные параметры группы.</summary>
        public List<ParameterInfo> Parameters { get; set; }
    }


    /// <summary>Строит диалог с группами параметров и элементами множественного выбора из списка.</summary>
    /// <param name="parameters">Разрешённые для диалога параметры.</param>
    /// <param name="title">Заголовок диалога выбора.</param>
    /// <param name="selectAllKey">Подпись мастер-флажка; null или пустая строка — флаг не выводится.</param>
    /// <param name="mainGroupName">Имя базовой группы (совпадает с именем справочника), выводимой первой.</param>
    /// <returns>Отмеченные параметры без повторов по GUID.</returns>
    private List<ParameterInfo> ShowParameterSelectionDialog(List<ParameterInfo> parameters, string title, string selectAllKey, string mainGroupName)
    {
        var selected = new List<ParameterInfo>();
        if (parameters == null || parameters.Count == 0) return selected;

        var dialog = provider.СоздатьДиалогВвода(title);
        dialog.Высота = 700;
        dialog.Ширина = 620;
        dialog.ОтобразитьПолосыПрокрутки(true, false);

        // Мастер-флаг выводится только если задана его подпись (для параметров подключения он не нужен).
        bool hasSelectAll = !string.IsNullOrWhiteSpace(selectAllKey);
        if (hasSelectAll)
            dialog.ДобавитьФлаг(selectAllKey, false, false, false);

        // Базовая группа (одноимённая справочнику) всегда идёт первой, остальные — по алфавиту.
        List<ParameterGroupBlock> blocks = parameters
            .GroupBy(parameter => parameter.Group?.Guid ?? Guid.Empty)
            .Select(grouping => new ParameterGroupBlock
            {
                Group = grouping.First().Group,
                Name = grouping.First().Group?.Name ?? "Общие параметры",
                Parameters = grouping.OrderBy(item => item.Name, StringComparer.CurrentCultureIgnoreCase).ToList()
            })
            .OrderBy(block => string.Equals(block.Name, mainGroupName, StringComparison.OrdinalIgnoreCase) ? 0 : 1)
            .ThenBy(block => block.Name, StringComparer.CurrentCultureIgnoreCase)
            .ToList();

        var listKeys = new Dictionary<Guid, string>();

        foreach (ParameterGroupBlock block in blocks)
        {
            // Группа-разделитель визуально отделяет набор параметров очередной группы.
            dialog.ДобавитьГруппу(block.Name);

            string listKey = "Параметры: " + block.Name;
            listKeys[block.Group?.Guid ?? Guid.Empty] = listKey;

            // Элемент множественного выбора из списка для параметров текущей группы.
            string[] allParamNames = block.Parameters
                .Select(parameter => ParameterDisplayName(parameter))
                .ToArray();
            dialog.ДобавитьМножественныйВыборИзСписка(listKey, allParamNames, false);
        }

        if (!dialog.Показать()) return selected;

        bool selectAll = hasSelectAll && XmiSchemaExporterMacro.ReadFlag(dialog, selectAllKey, false);
        foreach (ParameterGroupBlock block in blocks)
        {
            if (selectAll)
            {
                selected.AddRange(block.Parameters);
                continue;
            }

            string listKey = listKeys[block.Group?.Guid ?? Guid.Empty];
            List<string> selectedNames = ReadSelectedStrings(dialog.Значение(listKey));
            if (selectedNames.Count == 0) continue;

            var selectedSet = new HashSet<string>(selectedNames, StringComparer.Ordinal);
            selected.AddRange(block.Parameters.Where(parameter =>
                selectedSet.Contains(ParameterDisplayName(parameter))));
        }
        return selected;
    }

    /// <summary>Формирует отображаемое имя параметра для элемента множественного выбора.</summary>
    /// <param name="parameter">Параметр модели DOCs.</param>
    /// <returns>Имя параметра либо подстановка для параметра без имени.</returns>
    private static string ParameterDisplayName(ParameterInfo parameter)
    {
        return parameter?.Name ?? "(параметр без имени)";
    }

    /// <summary>Считывает строковые значения множественного выбора из результата диалога.</summary>
    /// <param name="value">Значение поля диалога (массив строк, перечислимое или одиночное значение).</param>
    /// <returns>Список выбранных строк без пустых элементов.</returns>
    private static List<string> ReadSelectedStrings(object value)
    {
        var result = new List<string>();
        if (value == null) return result;

        if (value is object[] arrayValue)
        {
            foreach (object item in arrayValue)
            {
                string name = Convert.ToString(item);
                if (!string.IsNullOrEmpty(name)) result.Add(name);
            }
        }
        else if (value is System.Collections.IEnumerable enumerable && !(value is string))
        {
            foreach (object item in enumerable)
            {
                string name = Convert.ToString(item);
                if (!string.IsNullOrEmpty(name)) result.Add(name);
            }
        }
        else
        {
            string name = Convert.ToString(value);
            if (!string.IsNullOrEmpty(name)) result.Add(name);
        }

        return result;
    }


    /// <summary>Определяет группу структурного подключения сложной иерархии справочника.</summary>
    /// <param name="reference">Справочник, настройки которого исследуются.</param>
    /// <returns>Группа подключения либо null для простой иерархии.</returns>
    private static ParameterGroup FindConnectionGroup(ReferenceInfo reference)
    {
        if (reference == null || reference.HierarchyType != ReferenceHierarchyType.Complex) return null;

        ParameterGroup description = reference.Description;
        ParameterGroup hierarchyGroup = description?.HierarchyGroup;
        if (hierarchyGroup != null && (description == null || hierarchyGroup.Guid != description.Guid))
            return hierarchyGroup;

        ParameterGroupCollection allGroups = description?.GetAllGroups();
        return allGroups?.FirstOrDefault(group => group != null && group.IsHierarchyTable);
    }

    /// <summary>Возвращает группы связей владельца через метод GetLinks модели DOCs.</summary>
    /// <param name="owner">Группа параметров или тип объектов DOCs.</param>
    /// <returns>Группы связей; пустая последовательность при их отсутствии.</returns>
    private static IEnumerable<ParameterGroup> GetLinkGroups(ParameterGroup owner)
    {
        if (owner == null) return Enumerable.Empty<ParameterGroup>();
        ParameterGroupCollection links = owner.GetLinks();
        return links ?? Enumerable.Empty<ParameterGroup>();
    }

    /// <summary>Возвращает группы связей типа объектов.</summary>
    /// <param name="owner">Тип объектов DOCs.</param>
    /// <returns>Группы связей; пустая последовательность при их отсутствии.</returns>
    private static IEnumerable<ParameterGroup> GetLinkGroups(ClassObject owner)
    {
        if (owner == null) return Enumerable.Empty<ParameterGroup>();
        ParameterGroupCollection links = owner.GetLinks();
        return links ?? Enumerable.Empty<ParameterGroup>();
    }

    /// <summary>Кэширует входящие и исходящие структурные связи всех доступных справочников.</summary>
    /// <param name="references">Полный список загруженных справочников DOCs.</param>
    /// <returns>Словарь GUID справочника к GUID всех непосредственно связанных справочников.</returns>
    private static Dictionary<Guid, HashSet<Guid>> BuildReferenceLinkCache(IList<ReferenceInfo> references)
    {
        var cache = new Dictionary<Guid, HashSet<Guid>>();
        if (references == null) return cache;
        foreach (ReferenceInfo reference in references.Where(item => item != null))
            cache[reference.Guid] = new HashSet<Guid>();

        foreach (ReferenceInfo source in references.Where(item => item != null))
        {
            foreach (ParameterGroup link in GetLinkGroups(source.Description))
            {
                ReferenceInfo target = link?.SlaveGroup?.ReferenceInfo;
                if (target == null || target.Guid == source.Guid) continue;

                if (cache.TryGetValue(source.Guid, out var outgoing)) outgoing.Add(target.Guid);
                if (cache.TryGetValue(target.Guid, out var incoming)) incoming.Add(source.Guid);
            }
        }
        return cache;
    }

    /// <summary>Формирует текст подтверждения с перечнем уже выбранных справочников и типов.</summary>
    /// <param name="catalogs">Контексты справочников, накопленные в текущем цикле.</param>
    /// <returns>Многострочная подпись для диалога продолжения выбора.</returns>
    private static string BuildAddMoreTypesPrompt(IEnumerable<CatalogSelection> catalogs)
    {
        var lines = catalogs.Select(catalog =>
            $"• Справочник \"{catalog.Reference.Name}\": {string.Join(", ", catalog.Classes.Select(type => type.Name))}");
        return "Уже выбрано:\n" + string.Join("\n", lines) + "\n\nДобавить еще типы из справочника?";
    }

    /// <summary>Показывает пользователю информационное сообщение DOCs.</summary>
    /// <param name="text">Текст сообщения.</param>
    private void Inform(string text)
    {
        provider.Сообщение("Информация", text);
    }


    /// <summary>Находит связи между выбранными справочниками и при необходимости даёт выбрать их вручную.</summary>
    /// <param name="selections">Каталоги, выбранные для экспорта.</param>
    /// <param name="interactive">Показывать ли диалог выбора связей.</param>
    /// <returns>Выбранные связи с зафиксированными master/slave-контекстами.</returns>
    private List<SelectedRelation> CollectRelations(List<CatalogSelection> selections, bool interactive)
    {
        var selectedReferenceGuids = new HashSet<Guid>(selections.Select(item => item.Reference.Guid));
        var items = new List<CatalogRelationItem>();
        var seen = new HashSet<string>(StringComparer.Ordinal);

        foreach (CatalogSelection current in selections)
        {
            var owners = new List<ParameterGroup>();
            ParameterGroup description = current.Reference?.Description;
            if (description != null) owners.Add(description);
            if (current.ConnectionGroup != null && current.ConnectionGroup.Guid != (description?.Guid ?? Guid.Empty))
                owners.Add(current.ConnectionGroup);

            foreach (ParameterGroup owner in owners)
            {
                bool isConnection = current.ConnectionGroup != null && owner.Guid == current.ConnectionGroup.Guid;
                // Для режима выбора справочников используем заранее собранные связи (SelectedLinks),
                // иначе — прямое чтение групп связей владельца через GetLinks().
                IEnumerable<ParameterGroup> linkGroups =
                    !isConnection && current.SelectedLinks != null && current.SelectedLinks.Count > 0
                        ? current.SelectedLinks
                        : GetLinkGroups(owner);
                foreach (ParameterGroup link in linkGroups)
                {
                    if (link == null) continue;
                    ReferenceInfo targetReference = link.SlaveGroup?.ReferenceInfo;
                    if (targetReference == null || !selectedReferenceGuids.Contains(targetReference.Guid) ||
                        targetReference.Guid == current.Reference.Guid) continue;

                    CatalogSelection target = selections.FirstOrDefault(item => item.Reference.Guid == targetReference.Guid);
                    if (target == null) continue;

                    string dedup = $"{link.Guid}_{current.Reference.Guid}_{targetReference.Guid}";
                    if (!seen.Add(dedup)) continue;

                    items.Add(new CatalogRelationItem
                    {
                        Link = link,
                        SourceCatalog = current,
                        TargetCatalog = target,
                        IsConnectionRelation = isConnection
                    });
                }
            }
        }

        if (items.Count == 0)
        {
            // В автоматическом режиме (SelectedReferences) информационные диалоги не показываются.
            if (interactive) Inform("Связи между выбранными справочниками не найдены.");
            return new List<SelectedRelation>();
        }

        if (!interactive)
            return DeduplicateRelations(items);

        var dialog = provider.СоздатьДиалогВвода("Шаг 4: Выберите связи между справочниками");
        dialog.ОтобразитьПолосыПрокрутки(true, false);
        // Флаг «выбрать все связи»: при установке в выгрузку попадают все найденные связи.
        const string selectAllLinksKey = "Выбрать все связи";
        dialog.ДобавитьФлаг(selectAllLinksKey, false, false, false);
        var itemKeys = new Dictionary<CatalogRelationItem, string>();

        foreach (var group in items.GroupBy(item => item.SourceCatalog.Reference.Name)
                     .OrderBy(group => group.Key, StringComparer.CurrentCultureIgnoreCase))
        {
            dialog.ДобавитьГруппу(group.Key);
            foreach (CatalogRelationItem item in group
                         .OrderBy(entry => entry.Link.Name, StringComparer.CurrentCultureIgnoreCase))
            {
                string sourceName = item.IsConnectionRelation
                    ? $"{item.SourceCatalog.Reference.Name} (Подключение)"
                    : item.SourceCatalog.Reference.Name;
                string label = $"{item.Link.Name} [{sourceName} -> {item.TargetCatalog.Reference.Name}]";
                if (!string.IsNullOrWhiteSpace(item.Link.Comment)) label += $" [{item.Link.Comment}]";
                label += $" · r_{item.Link.Guid}";
                item.DialogKey = label;
                itemKeys[item] = label;
                dialog.ДобавитьФлаг(label, false, false, false);
            }
        }

        var selectedItems = new List<CatalogRelationItem>();
        if (dialog.Показать())
        {
            bool selectAllLinks = XmiSchemaExporterMacro.ReadFlag(dialog, selectAllLinksKey, true);
            selectedItems = selectAllLinks
                ? items.ToList()
                : items
                    .Where(item => itemKeys.TryGetValue(item, out var key) &&
                                   XmiSchemaExporterMacro.ReadFlag(dialog, key, false))
                    .ToList();
        }

        return DeduplicateRelations(selectedItems);
    }

    /// <summary>Устраняет повторы связей по GUID и преобразует их в экспортируемые отношения.</summary>
    /// <param name="items">Кандидаты связей.</param>
    /// <returns>Связи с уникальными GUID.</returns>
    private static List<SelectedRelation> DeduplicateRelations(IEnumerable<CatalogRelationItem> items)
    {
        var result = new List<SelectedRelation>();
        var handledGuids = new HashSet<Guid>();
        foreach (CatalogRelationItem item in items)
        {
            if (item?.Link == null || !handledGuids.Add(item.Link.Guid)) continue;
            result.Add(new SelectedRelation
            {
                Link = item.Link,
                Master = item.SourceCatalog,
                Slave = item.TargetCatalog,
                IsConnectionRelation = item.IsConnectionRelation
            });
        }
        return result;
    }


    /// <summary>Строит итоговую UML/EA-модель из выбранных справочников и связей.</summary>
    /// <param name="selections">Контексты выбранных справочников.</param>
    /// <param name="relations">Выбранные связи между справочниками.</param>
    /// <returns>Готовая модель выгрузки.</returns>
    private ExportModel BuildModel(List<CatalogSelection> selections, List<SelectedRelation> relations)
    {
        var catalogByReferenceGuid = new Dictionary<Guid, CatalogModel>();
        foreach (CatalogSelection selection in selections)
            catalogByReferenceGuid[selection.Reference.Guid] = BuildCatalog(selection.Reference);
        List<CatalogModel> catalogs = selections.Select(item => catalogByReferenceGuid[item.Reference.Guid]).ToList();

        var classesByReferenceGuid = new Dictionary<Guid, List<ClassModel>>();
        foreach (CatalogSelection selection in selections)
            classesByReferenceGuid[selection.Reference.Guid] =
                BuildClasses(selection, catalogByReferenceGuid[selection.Reference.Guid].Guid);
        List<ClassModel> allClasses = classesByReferenceGuid.Values.SelectMany(items => items).ToList();

        List<CatalogTypeAssocModel> catalogTypeAssocs = allClasses.Select(cls =>
        {
            CatalogModel catalog = catalogs.First(item => item.Guid == cls.CatalogGuid);
            return new CatalogTypeAssocModel
            {
                Guid = XmiSupport.FormatEaId(XmiSupport.CreateDeterministicGuid($"Contains_{catalog.Guid}_{cls.Guid}")),
                CatalogGuid = catalog.Guid,
                CatalogName = catalog.Name,
                TypeGuid = cls.Guid,
                TypeName = cls.Name
            };
        }).ToList();

        if (options.ExportEvents)
        {
            foreach (CatalogSelection selection in selections)
                catalogByReferenceGuid[selection.Reference.Guid].Events.AddRange(
                    BuildEvents(catalogByReferenceGuid[selection.Reference.Guid], selection));
        }

        var allAssocs = new List<AssocModel>();
        BuildExplicitAssociations(relations, classesByReferenceGuid, allAssocs);
        AddComplexHierarchyAssociations(selections, classesByReferenceGuid, allAssocs);
        AddIntraCatalogHierarchyAssociations(selections, classesByReferenceGuid, allAssocs);

        string combinedKeys = string.Join("_", selections
            .OrderBy(item => item.Reference.Guid)
            .Select(item => item.Reference.Guid));

        return new ExportModel
        {
            Options = options,
            PackageKey = "ModelPackage_" + combinedKeys,
            Catalogs = catalogs,
            Classes = allClasses,
            Assocs = allAssocs,
            CatalogTypeAssocs = catalogTypeAssocs
        };
    }


    /// <summary>Строит ассоциации для связей, выбранных пользователем или автоматическим режимом.</summary>
    /// <param name="relations">Выбранные связи.</param>
    /// <param name="classesByReferenceGuid">Классы, сгруппированные по GUID справочника.</param>
    /// <param name="allAssocs">Общий список ассоциаций для UML и EA Extension.</param>
    private static void BuildExplicitAssociations(
        List<SelectedRelation> relations,
        IDictionary<Guid, List<ClassModel>> classesByReferenceGuid,
        List<AssocModel> allAssocs)
    {
        var handledLinkGuids = new HashSet<Guid>();
        foreach (SelectedRelation relation in relations)
        {
            if (relation?.Link == null || relation.Master?.Reference == null || relation.Slave?.Reference == null)
                continue;
            if (!handledLinkGuids.Add(relation.Link.Guid)) continue;

            // Безопасный поиск классов Master-справочника: справочник мог не войти в текущую выгрузку.
            if (!classesByReferenceGuid.TryGetValue(relation.Master.Reference.Guid, out var masterCatalogClasses) ||
                masterCatalogClasses == null)
                continue;

            // Безопасный поиск классов Slave-справочника.
            if (!classesByReferenceGuid.TryGetValue(relation.Slave.Reference.Guid, out var slaveCatalogClasses) ||
                slaveCatalogClasses == null)
                continue;

            List<ClassModel> masterClasses = masterCatalogClasses
                .Where(classModel => relation.IsConnectionRelation
                    ? classModel.IsConnection && IsLinkAttachedToMasterClass(classModel, relation.Link)
                    : !classModel.IsConnection && IsLinkAttachedToMasterClass(classModel, relation.Link))
                .ToList();
            List<ClassModel> slaveClasses = slaveCatalogClasses
                .Where(classModel => !classModel.IsConnection && IsLinkAllowedForSlaveClass(classModel, relation.Link))
                .ToList();

            foreach (ClassModel masterClass in masterClasses)
            {
                foreach (ClassModel slaveClass in slaveClasses)
                {
                    var multiplicity = XmiSupport.GetAssociationMultiplicity(relation.Link);
                    string sourceRole = relation.Link.Name;
                    string targetRole = GetRelationNameForCatalog(relation.Slave, relation.Link.Guid);
                    Guid assocRawGuid = XmiSupport.CreateDeterministicGuid(
                        $"Assoc_{relation.Link.Guid}_{masterClass.Guid}_{slaveClass.Guid}");
                    var assoc = new AssocModel
                    {
                        AssocGuid = XmiSupport.FormatEaId(assocRawGuid),
                        SrcPropGuid = XmiSupport.FormatEaId(assocRawGuid, "EAID_src_"),
                        DstPropGuid = XmiSupport.FormatEaId(assocRawGuid, "EAID_dst_"),
                        Name = !string.IsNullOrWhiteSpace(sourceRole) && !string.IsNullOrWhiteSpace(targetRole)
                            ? null
                            : relation.Link.Name,
                        SourceClassGuid = masterClass.Guid,
                        SourceClassName = masterClass.Name,
                        TargetClassGuid = slaveClass.Guid,
                        TargetClassName = slaveClass.Name,
                        SourceRole = sourceRole,
                        TargetRole = targetRole,
                        SourceLower = multiplicity.sourceLower,
                        SourceUpper = multiplicity.sourceUpper,
                        TargetLower = multiplicity.targetLower,
                        TargetUpper = multiplicity.targetUpper
                    };
                    masterClass.Associations.Add(assoc);
                    allAssocs.Add(assoc);
                }
            }
        }
    }

    /// <summary>Извлекает документацию и настройки справочника для UML и EA extension.</summary>
    /// <param name="reference">Справочник T-FLEX DOCs.</param>
    /// <returns>Промежуточная модель справочника.</returns>
    private CatalogModel BuildCatalog(ReferenceInfo reference)
    {
        var catalog = new CatalogModel
        {
            Guid = XmiSupport.FormatEaId(reference.Guid != Guid.Empty
                ? reference.Guid
                : XmiSupport.CreateDeterministicGuid($"Catalog_{reference.Name}")),
            DocsGuid = reference.Guid,
            Name = reference.Name,
            Comment = reference.Description?.Comment
        };

        if (options.ExportSettings)
            AddStructureSettings(catalog.Settings, reference.Description);
        return catalog;
    }


    /// <summary>Создаёт модели выбранных типов и, для сложной иерархии, псевдотип подключения.</summary>
    /// <param name="selection">Выбранный справочник, его типы и параметры.</param>
    /// <param name="catalogGuid">XMI ID UML-компонента справочника.</param>
    /// <returns>Типы с параметрами, подтверждённо принадлежащими каждому типу.</returns>
    private static List<ClassModel> BuildClasses(CatalogSelection selection, string catalogGuid)
    {
        var list = new List<ClassModel>();
        ReferenceInfo reference = selection?.Reference;
        if (reference == null) return list;

        foreach (ClassObject classObject in selection.Classes ?? new List<ClassObject>())
        {
            if (classObject == null) continue;

            Guid classGuid = classObject.Guid != Guid.Empty
                ? classObject.Guid
                : XmiSupport.CreateDeterministicGuid($"Class_{reference.Guid}_{classObject.Name}");
            var cls = new ClassModel
            {
                Guid = XmiSupport.FormatEaId(classGuid),
                DocsGuid = classObject.Guid,
                Name = classObject.Name,
                CatalogGuid = catalogGuid,
                SourceClass = classObject,
                Comment = classObject.Comment
            };

            IEnumerable<ParameterInfo> attachedParameters = (selection.Parameters ?? new List<ParameterInfo>())
                .Where(parameter => IsParameterAttachedToType(parameter, classObject, reference))
                .GroupBy(parameter => parameter.Guid)
                .Select(grouping => grouping.First());

            foreach (ParameterInfo parameter in attachedParameters)
            {
                var (typeRef, eaType) = XmiSupport.MapType(parameter.Type);
                cls.Parameters.Add(new ParamModel
                {
                    Guid = XmiSupport.FormatEaId(XmiSupport.CreateDeterministicGuid($"Property_{cls.Guid}_{parameter.Guid}")),
                    DocsGuid = parameter.Guid,
                    Name = parameter.Name,
                    TypeRef = typeRef,
                    EaType = eaType
                });
            }
            list.Add(cls);
        }

        ParameterGroup connectionGroup = selection.ConnectionGroup ?? FindConnectionGroup(reference);
        if (connectionGroup != null)
        {
            var connection = new ClassModel
            {
                Guid = XmiSupport.FormatEaId(connectionGroup.Guid != Guid.Empty
                    ? connectionGroup.Guid
                    : XmiSupport.CreateDeterministicGuid($"Connection_{reference.Guid}")),
                DocsGuid = connectionGroup.Guid,
                Name = "Подключение",
                CatalogGuid = catalogGuid,
                IsConnection = true,
                Comment = connectionGroup.Comment
            };

            foreach (ParameterInfo parameter in selection.ConnectionParameters ?? new List<ParameterInfo>())
            {
                var (typeRef, eaType) = XmiSupport.MapType(parameter.Type);
                connection.Parameters.Add(new ParamModel
                {
                    Guid = XmiSupport.FormatEaId(XmiSupport.CreateDeterministicGuid(
                        $"ConnectionProperty_{connection.Guid}_{parameter.Guid}")),
                    DocsGuid = parameter.Guid,
                    Name = parameter.Name,
                    TypeRef = typeRef,
                    EaType = eaType
                });
            }
            list.Add(connection);
        }

        return list;
    }


    /// <summary>Проверяет принадлежность параметра типу с учётом общей группы и наследования.</summary>
    /// <param name="parameter">Параметр, выбранный пользователем.</param>
    /// <param name="classObject">Тип объектов DOCs.</param>
    /// <param name="reference">Справочник-владелец типов и главной группы параметров.</param>
    /// <returns>true, если параметр принадлежит типу напрямую, через базовый тип или как общий параметр.</returns>
    private static bool IsParameterAttachedToType(ParameterInfo parameter, ClassObject classObject, ReferenceInfo reference)
    {
        if (parameter == null || classObject == null) return false;
        if (parameter.Group == null) return true;

        // Description справочника — его главная группа с общими параметрами всех типов.
        ParameterGroup mainGroup = reference?.Description;
        if (mainGroup != null && parameter.Group.Guid == mainGroup.Guid) return true;

        try
        {
            ClassObjectCollection classes = reference?.Classes?.GetParameterGroupClasses(parameter.Group, true);
            return classes != null && classes.Any(candidate => candidate != null && candidate.Guid == classObject.Guid);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Проверяет, подключена ли связь к конкретному типу-источнику (Master) через его группы параметров.
    /// </summary>
    /// <param name="classModel">Экспортируемый класс типа DOCs.</param>
    /// <param name="linkGroup">Связь, принадлежность которой проверяется.</param>
    /// <returns>true, если связь явно привязана к типу или является структурной связью подключения.</returns>
    private static bool IsLinkAttachedToMasterClass(ClassModel classModel, ParameterGroup linkGroup)
    {
        if (classModel == null || linkGroup == null) return false;
        if (classModel.IsConnection) return true;

        ClassObject classObject = classModel.SourceClass;
        if (classObject == null) return false;

        // Связь привязана к типу, если совпадает с одной из групп его параметров
        // либо присутствует среди групп связей, возвращаемых прямым API GetLinks() этого типа.
        if (classObject.ParameterGroups != null &&
            classObject.ParameterGroups.Any(group => group != null && group.Guid == linkGroup.Guid))
            return true;

        return GetLinkGroups(classObject).Any(link => link != null && link.Guid == linkGroup.Guid);
    }

    /// <summary>Проверяет допустимость типа-приёмника через разрешённые классы самой связи.</summary>
    /// <param name="slaveClassModel">Экспортируемый класс типа-приёмника.</param>
    /// <param name="linkGroup">Связь, настройки которой задают допустимые типы-приёмники.</param>
    /// <returns>true, если тип разрешён связью либо у связи не задано ограничение по типам.</returns>
    private static bool IsLinkAllowedForSlaveClass(ClassModel slaveClassModel, ParameterGroup linkGroup)
    {
        if (slaveClassModel?.SourceClass == null || linkGroup == null) return false;

        ClassObjectCollection allowedClasses;
        try
        {
            allowedClasses = linkGroup.GetAllowedClassesToLink();
        }
        catch
        {
            allowedClasses = null;
        }

        // Если целевые типы не заданы явно на уровне связи, используем базовые типы
        // ведомой группы связи (linkGroup.SlaveGroup.Classes.AllClasses).
        if (allowedClasses == null || allowedClasses.Count == 0)
            allowedClasses = linkGroup.SlaveGroup?.Classes?.AllClasses;
        if (allowedClasses == null || allowedClasses.Count == 0) return true;

        ClassObject slaveClass = slaveClassModel.SourceClass;
        return allowedClasses.Any(allowed => allowed != null &&
            (allowed.Guid == slaveClass.Guid || allowed.IsBaseClassFor(slaveClass)));
    }

    /// <summary>Получает имя той же связи в контексте второго справочника.</summary>
    /// <param name="catalog">Справочник, у которого запрашивается имя роли.</param>
    /// <param name="linkGuid">GUID связи, найденной со стороны первого справочника.</param>
    /// <returns>Контекстное имя из link.Name либо пустая строка, если связь не найдена.</returns>
    private static string GetRelationNameForCatalog(CatalogSelection catalog, Guid linkGuid)
    {
        if (catalog?.Reference == null) return string.Empty;

        var owners = new List<ParameterGroup>();
        ParameterGroup description = catalog.Reference.Description;
        if (description != null) owners.Add(description);
        if (catalog.ConnectionGroup != null && catalog.ConnectionGroup.Guid != (description?.Guid ?? Guid.Empty))
            owners.Add(catalog.ConnectionGroup);

        foreach (ParameterGroup owner in owners)
        {
            ParameterGroup match = GetLinkGroups(owner).FirstOrDefault(link => link != null && link.Guid == linkGuid);
            if (match != null) return match.Name ?? string.Empty;
        }
        return string.Empty;
    }


    /// <summary>Собирает события справочника, его типов и их групп параметров.</summary>
    /// <param name="catalog">Экспортируемый справочник; может быть null.</param>
    /// <param name="selection">Контекст выбранного справочника; может быть null.</param>
    /// <returns>События без повторов по GUID.</returns>
    private static List<EventModel> BuildEvents(CatalogModel catalog, CatalogSelection selection)
    {
        var events = new List<EventModel>();
        var seen = new HashSet<Guid>();

        // Явные проверки на null всех входящих объектов: без инициализированного менеджера
        // событий прямое чтение .Events падает NullReferenceException в ядре DOCs.
        if (catalog == null || selection == null) return events;

        ReferenceInfo reference = selection.Reference;
        if (reference == null) return events;

        ParameterGroup description = reference.Description;
        if (description != null)
            AddEventsFromOwner(events, seen, catalog, description);

        if (selection.ConnectionGroup != null)
            AddEventsFromOwner(events, seen, catalog, selection.ConnectionGroup);

        foreach (ClassObject classObject in selection.Classes ?? new List<ClassObject>())
        {
            if (classObject == null) continue;
            AddEventsFromOwner(events, seen, catalog, classObject);

            if (classObject.ParameterGroups == null) continue;
            foreach (ParameterGroup group in classObject.ParameterGroups)
            {
                if (group == null) continue;
                AddEventsFromOwner(events, seen, catalog, group);
            }
        }

        return events;
    }

    /// <summary>
    /// Безопасно извлекает события у владельца (группа параметров описания/подключения, тип объекта).
    /// Для групп параметров читается свойство EventHandlers (EventHandlerCollection) вместо Events:
    /// прямое чтение Events падает с NullReferenceException в ядре DOCs
    /// (TFlex.DOCs.Model.ParameterGroupManager.get_Events()), если менеджер событий не инициализирован.
    /// При любом сбое возвращается пустая коллекция.
    /// </summary>
    /// <param name="owner">Владелец событий; может быть null.</param>
    /// <returns>События владельца либо пустая коллекция.</returns>
    private static IEnumerable<ParameterGroupEvent> GetEventsSafe(object owner)
    {
        if (owner == null) return Array.Empty<ParameterGroupEvent>();
        try
        {
            switch (owner)
            {
                // Группы параметров (ReferenceInfo/Description, группа подключения, группы типа) — через EventHandlers.
                case ParameterGroup group:
                    return GetEventsFromHandlers(group.EventHandlers);
                // У типа объекта (ClassObject) свойства EventHandlers нет — читаем Events под защитой try-catch.
                case ClassObject classObject:
                    return classObject.Events ?? (IEnumerable<ParameterGroupEvent>)Array.Empty<ParameterGroupEvent>();
                case EventCollection collection:
                    return collection;
                default:
                    return Array.Empty<ParameterGroupEvent>();
            }
        }
        catch (NullReferenceException)
        {
            // Менеджер событий владельца может быть не инициализирован (например, для новых групп параметров).
            return Array.Empty<ParameterGroupEvent>();
        }
    }

    /// <summary>Извлекает события из безопасной коллекции обработчиков EventHandlerCollection без повторов по GUID.</summary>
    /// <param name="handlers">Коллекция обработчиков событий; может быть null.</param>
    /// <returns>Уникальные события владельца.</returns>
    private static IEnumerable<ParameterGroupEvent> GetEventsFromHandlers(EventHandlerCollection handlers)
    {
        if (handlers == null) return Array.Empty<ParameterGroupEvent>();

        var result = new List<ParameterGroupEvent>();
        var seen = new HashSet<Guid>();
        foreach (ParameterGroupEventHandler handler in handlers)
        {
            if (handler == null) continue;
            ParameterGroupEvent eventObject = handler.Event;
            if (eventObject == null) continue;
            if (eventObject.Guid != Guid.Empty && !seen.Add(eventObject.Guid)) continue;
            result.Add(eventObject);
        }
        return result;
    }

    /// <summary>Добавляет события владельца в накопитель экспорта, устраняя повторы по GUID.</summary>
    /// <param name="events">Накопитель событий экспорта.</param>
    /// <param name="seen">GUID уже обработанных событий для устранения повторов.</param>
    /// <param name="catalog">Справочник, содержащий событие.</param>
    /// <param name="owner">Владелец событий (группа параметров или тип объекта); может быть null.</param>
    private static void AddEventsFromOwner(List<EventModel> events, HashSet<Guid> seen, CatalogModel catalog, object owner)
    {
        if (events == null || seen == null || catalog == null || owner == null) return;

        foreach (ParameterGroupEvent eventObject in GetEventsSafe(owner))
        {
            if (eventObject == null) continue;

            Guid guid = eventObject.Guid != Guid.Empty
                ? eventObject.Guid
                : XmiSupport.CreateDeterministicGuid($"Event_{catalog.Guid}_{eventObject.Name}");
            if (!seen.Add(guid)) continue;

            events.Add(new EventModel
            {
                Guid = XmiSupport.FormatEaId(guid),
                DocsGuid = guid,
                Name = eventObject.Name,
                Comment = null,
                CatalogGuid = catalog.Guid
            });
        }
    }

    /// <summary>Добавляет одну пару «настройка — значение» в коллекцию tagged values.</summary>
    /// <param name="settings">Коллекция настроек.</param>
    /// <param name="caption">Русская подпись настройки.</param>
    /// <param name="value">Значение настройки.</param>
    private static void AddSetting(List<KeyValuePair<string, string>> settings, string caption, string value)
    {
        settings.Add(new KeyValuePair<string, string>(caption, value ?? string.Empty));
    }

    /// <summary>Преобразует логическое значение настройки в читаемый текст.</summary>
    /// <param name="value">Значение флага DOCs.</param>
    /// <returns>«Да» для истины, иначе «Нет».</returns>
    private static string BoolText(bool value)
    {
        return value ? "Да" : "Нет";
    }


    /// <summary>Читает набор настроек структуры справочника напрямую из свойств ParameterGroup.</summary>
    /// <param name="settings">Коллекция настроек, предназначенная для tagged values.</param>
    /// <param name="group">Главная группа справочника DOCs с настройками структуры.</param>
    private static void AddStructureSettings(List<KeyValuePair<string, string>> settings, ParameterGroup group)
    {
        if (settings == null) return;
        if (group == null)
        {
            settings.Add(new KeyValuePair<string, string>("Структура", "недоступно"));
            return;
        }

        // === Вкладка «Основные» ===
        AddSetting(settings, "Имя таблицы в БД", group.TableName);
        AddSetting(settings, "Видимость справочника в интерфейсе", Convert.ToString(group.Visibility));

        // === Вкладка «Дополнительные» (левый столбец чекбоксов) ===
        AddSetting(settings, "Поддержка истории изменений", BoolText(group.SupportsDataChangeLog));
        AddSetting(settings, "Поддержка работы с прототипами", BoolText(group.SupportsPrototypes));
        AddSetting(settings, "Поддержка механизма подписей", BoolText(group.SupportsSignature));
        AddSetting(settings, "Поддержка параметра \"Владелец\"", BoolText(group.SupportsOwner));
        AddSetting(settings, "Допускается изменение типа объектов", BoolText(group.CanChangeClass));
        AddSetting(settings, "Поддержка экземпляров изделий", BoolText(group.SupportsObjectsInstances));
        AddSetting(settings, "Поддержка дат действия", BoolText(group.SupportsActivityDates));
        AddSetting(settings, "Поддержка структур изделия", BoolText(group.SupportsStructureTypes));
        AddSetting(settings, "Поддержка механизма контекстных замен", BoolText(group.SupportsSubstitutesInContext));

        // === Вкладка «Дополнительные» (правый столбец чекбоксов) ===
        AddSetting(settings, "Поддержка корзины", BoolText(group.SupportsRecycleBin));
        AddSetting(settings, "Поддержка сортировки", BoolText(group.SupportsOrder));
        AddSetting(settings, "Поддержка ревизий", BoolText(group.SupportsRevisions));
        AddSetting(settings, "Поддержка дополнительных параметров", BoolText(group.SupportsExtendedParameters));
        AddSetting(settings, "Поддержка применимости", BoolText(group.SupportsApplicability));
        AddSetting(settings, "Поддержка контекстов проектирования", BoolText(group.SupportsDesignContexts));
        AddSetting(settings, "Поддержка конфигурирования", BoolText(group.SupportsConfigurationSettings));

        // === Вкладка «Дополнительные» (выпадающие списки и группы) ===
        AddSetting(settings, "Иерархия объектов", Convert.ToString(group.HierarchyType));
        AddSetting(settings, "Поддержка управления доступом на объекты", BoolText(group.SupportsMandatoryAccess));
        AddSetting(settings, "Поддержка стадий", BoolText(group.SupportsStages));
        AddSetting(settings, "Конфигуратор", Convert.ToString(group.ConfiguratorGuid));

        // === Вкладка «Типы подписей» ===
        AddSetting(settings, "Использовать все типы подписей", BoolText(group.UseAllSignatureTypes));
    }


    /// <summary>Создаёт физические связи сложной иерархии через псевдокласс «Подключение».</summary>
    /// <param name="selections">Выбранные справочники и их типы объектов.</param>
    /// <param name="classesByReferenceGuid">UML-классы, сгруппированные по GUID справочника.</param>
    /// <param name="allAssocs">Общий список ассоциаций для UML и EA Extension.</param>
    private static void AddComplexHierarchyAssociations(
        IEnumerable<CatalogSelection> selections,
        IDictionary<Guid, List<ClassModel>> classesByReferenceGuid,
        List<AssocModel> allAssocs)
    {
        if (selections == null || classesByReferenceGuid == null || allAssocs == null) return;

        foreach (CatalogSelection selection in selections.Where(item => item?.Reference != null))
        {
            if (!classesByReferenceGuid.TryGetValue(selection.Reference.Guid, out var catalogClasses) || catalogClasses == null)
                continue;

            ClassModel connectionClass = catalogClasses.FirstOrDefault(classModel => classModel?.IsConnection == true);
            if (connectionClass == null) continue;

            foreach (ClassModel objectClass in catalogClasses.Where(classModel => classModel != null && !classModel.IsConnection))
            {
                AddConnectionAssociation(connectionClass, objectClass, "Родительский объект", allAssocs);
                AddConnectionAssociation(connectionClass, objectClass, "Дочерний объект", allAssocs);
            }
        }
    }

    /// <summary>Добавляет одну физическую ассоциацию от подключения к объектному типу.</summary>
    /// <param name="connectionClass">Псевдокласс «Подключение» — источник ассоциации.</param>
    /// <param name="objectClass">Объектный класс — цель ассоциации.</param>
    /// <param name="associationName">Имя роли «Родительский объект» или «Дочерний объект».</param>
    /// <param name="allAssocs">Общий список ассоциаций для UML и EA Extension.</param>
    private static void AddConnectionAssociation(
        ClassModel connectionClass,
        ClassModel objectClass,
        string associationName,
        List<AssocModel> allAssocs)
    {
        if (connectionClass == null || objectClass == null || allAssocs == null) return;
        if (connectionClass.Associations.Any(association =>
                association != null && association.Name == associationName && association.TargetClassGuid == objectClass.Guid))
            return;

        string associationSeed = associationName == "Родительский объект" ? "Parent" : "Child";
        Guid associationGuid = XmiSupport.CreateDeterministicGuid(
            $"{associationSeed}_{connectionClass.Guid}_{objectClass.Guid}");
        var association = new AssocModel
        {
            AssocGuid = XmiSupport.FormatEaId(associationGuid),
            SrcPropGuid = XmiSupport.FormatEaId(associationGuid, "EAID_src_"),
            DstPropGuid = XmiSupport.FormatEaId(associationGuid, "EAID_dst_"),
            Name = associationName,
            // Стрелка начинается у подключения и направлена к объекту справочника.
            SourceClassGuid = connectionClass.Guid,
            SourceClassName = connectionClass.Name,
            TargetClassGuid = objectClass.Guid,
            TargetClassName = objectClass.Name,
            SourceLower = "0",
            SourceUpper = "*",
            TargetLower = "1",
            TargetUpper = "1",
            Direction = "Source -> Destination"
        };
        connectionClass.Associations.Add(association);
        allAssocs.Add(association);
    }

    /// <summary>Проверяет, поддерживает ли справочник иерархию объектов.</summary>
    /// <param name="reference">Справочник, настройки которого проверяются.</param>
    /// <returns>true, если справочник использует дерево либо явно поддерживает иерархию.</returns>
    private static bool IsHierarchicalCatalog(ReferenceInfo reference)
    {
        if (reference == null) return false;
        return reference.HasHierarchy ||
               reference.HierarchyType == ReferenceHierarchyType.Simple ||
               reference.HierarchyType == ReferenceHierarchyType.Complex;
    }


    /// <summary>Добавляет ассоциации «Состоит из» по допустимым дочерним классам дерева.</summary>
    /// <param name="selections">Выбранные справочники и их типы объектов.</param>
    /// <param name="classesByReferenceGuid">UML-классы, сгруппированные по GUID справочника.</param>
    /// <param name="allAssocs">Общий список ассоциаций для UML и EA Extension.</param>
    private static void AddIntraCatalogHierarchyAssociations(
        IEnumerable<CatalogSelection> selections,
        IDictionary<Guid, List<ClassModel>> classesByReferenceGuid,
        List<AssocModel> allAssocs)
    {
        if (selections == null || classesByReferenceGuid == null || allAssocs == null) return;

        foreach (CatalogSelection selection in selections.Where(item =>
                     item?.Reference != null && IsHierarchicalCatalog(item.Reference)))
        {
            if (!classesByReferenceGuid.TryGetValue(selection.Reference.Guid, out var catalogClasses)) continue;
            if (catalogClasses == null) continue;

            // Сложная (сетевая) иерархия допускает несколько родителей, поэтому кратность
            // со стороны родителя тоже неограниченна; дерево допускает только одного родителя.
            bool isComplex = selection.Reference.HierarchyType == ReferenceHierarchyType.Complex;
            string parentUpper = isComplex ? "*" : "1";

            foreach (ClassModel parentClass in catalogClasses.Where(classModel => classModel != null && !classModel.IsConnection))
            {
                foreach (ClassObject childObjectClass in GetChildClasses(parentClass))
                {
                    ClassModel childClass = ResolveSelectedChildClass(childObjectClass, catalogClasses);
                    if (childClass == null || childClass.Guid == parentClass.Guid) continue;
                    if (parentClass.Associations.Any(association =>
                            association != null && association.Name == "Состоит из" &&
                            association.TargetClassGuid == childClass.Guid)) continue;

                    Guid associationGuid = XmiSupport.CreateDeterministicGuid(
                        $"ConsistsOf_{parentClass.Guid}_{childClass.Guid}");
                    var association = new AssocModel
                    {
                        AssocGuid = XmiSupport.FormatEaId(associationGuid),
                        SrcPropGuid = XmiSupport.FormatEaId(associationGuid, "EAID_src_"),
                        DstPropGuid = XmiSupport.FormatEaId(associationGuid, "EAID_dst_"),
                        Name = "Состоит из",
                        SourceClassGuid = parentClass.Guid,
                        SourceClassName = parentClass.Name,
                        TargetClassGuid = childClass.Guid,
                        TargetClassName = childClass.Name,
                        SourceLower = "0",
                        SourceUpper = parentUpper,
                        TargetLower = "0",
                        TargetUpper = "*",
                        Direction = "Source -> Destination"
                    };
                    parentClass.Associations.Add(association);
                    allAssocs.Add(association);
                }
            }
        }
    }

    /// <summary>Получает дочерние типы через прямое свойство ChildObjectClasses исходного класса.</summary>
    /// <param name="parentClass">Класс-родитель, дочерние типы которого требуется прочитать.</param>
    /// <returns>Объекты дочерних классов либо пустая последовательность.</returns>
    private static IEnumerable<ClassObject> GetChildClasses(ClassModel parentClass)
    {
        ClassObjectCollection children = parentClass?.SourceClass?.ChildObjectClasses;
        return children == null
            ? Enumerable.Empty<ClassObject>()
            : children.Where(child => child != null);
    }

    /// <summary>Находит выбранный UML-класс по GUID или имени объекта ChildObjectClasses.</summary>
    /// <param name="childObjectClass">Дочерний ClassObject из модели DOCs.</param>
    /// <param name="catalogClasses">Типы и псевдоклассы текущего справочника.</param>
    /// <returns>Совпавший выбранный класс либо null.</returns>
    private static ClassModel ResolveSelectedChildClass(ClassObject childObjectClass, IEnumerable<ClassModel> catalogClasses)
    {
        if (childObjectClass == null || catalogClasses == null) return null;

        if (childObjectClass.Guid != Guid.Empty)
        {
            ClassModel byGuid = catalogClasses.FirstOrDefault(classModel =>
                classModel?.SourceClass != null && classModel.SourceClass.Guid == childObjectClass.Guid);
            if (byGuid != null) return byGuid;
        }

        return catalogClasses.FirstOrDefault(classModel => classModel != null &&
            string.Equals(classModel.Name, childObjectClass.Name, StringComparison.Ordinal));
    }
}


/// <summary>Низкоуровневые помощники, общие для сборщика модели и генератора XMI.</summary>
internal static class XmiSupport
{
    /// <summary>Создаёт детерминированный GUID на основе стабильного строкового ключа DOCs.</summary>
    /// <param name="sourceKey">Стабильный ключ исходной или производной сущности.</param>
    /// <returns>GUID, вычисленный через MD5; для пустого ключа возвращается Guid.Empty.</returns>
    public static Guid CreateDeterministicGuid(string sourceKey)
    {
        if (string.IsNullOrEmpty(sourceKey)) return Guid.Empty;
        using (var md5 = System.Security.Cryptography.MD5.Create())
        {
            byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(sourceKey));
            return new Guid(hash);
        }
    }

    /// <summary>Форматирует GUID как стабильный XMI/EA идентификатор.</summary>
    /// <param name="guid">GUID исходной или производной сущности.</param>
    /// <param name="prefix">Префикс идентификатора EA.</param>
    /// <returns>Идентификатор с заданным префиксом и GUID без разделителей.</returns>
    public static string FormatEaId(Guid guid, string prefix = "EAID_")
    {
        return prefix + guid.ToString("N").ToUpperInvariant();
    }

    /// <summary>Преобразует тип параметра DOCs в стандартный тип UML и тип атрибута EA.</summary>
    /// <param name="parameterType">Тип значения параметра в модели DOCs.</param>
    /// <returns>Пара ссылок для UML type и Enterprise Architect attribute type.</returns>
    public static (string typeRef, string eaType) MapType(ParameterType parameterType)
    {
        if (parameterType == null) return ("EAC__string", "string");
        if (parameterType.IsBoolean) return ("EAC__boolean", "boolean");
        if (parameterType.IsDateTime) return ("EAC__date", "date");
        if (parameterType.IsFloat || parameterType.IsMoney || parameterType.IsNumber) return ("EAC__double", "double");
        if (parameterType.IsInt) return ("EAC__int", "int");
        return ("EAC__string", "string");
    }

    /// <summary>Форматирует нижнюю и верхнюю границы кратности для EA-коннектора.</summary>
    /// <param name="lower">Нижняя граница кратности.</param>
    /// <param name="upper">Верхняя граница или -1/* для неограниченной кратности.</param>
    /// <returns>Значение кратности в формате EA.</returns>
    public static string FormatMultiplicity(string lower, string upper)
    {
        string normalizedUpper = upper == "-1" ? "*" : upper;
        if (lower == normalizedUpper) return lower;
        return $"{lower}..{normalizedUpper}";
    }

    /// <summary>Переводит тип связи DOCs в кратности концов UML-ассоциации.</summary>
    /// <param name="link">Группа связи DOCs между master и slave каталогами.</param>
    /// <returns>Нижние и верхние границы для master- и slave-концов.</returns>
    public static (string sourceLower, string sourceUpper, string targetLower, string targetUpper) GetAssociationMultiplicity(ParameterGroup link)
    {
        switch (link.LinkType.ToString())
        {
            case "OneToMany":
                return ("1", "1", "0", "*");
            case "ManyToOne":
                return ("0", "*", "1", "1");
            case "ManyToMany":
                return ("0", "*", "0", "*");
            case "OneToOne":
                return ("1", "1", "1", "1");
            default:
                return ("1", "1", "1", "1");
        }
    }
}


/// <summary>
/// Формирует и сохраняет XMI 2.1 с профилем и метаданными, совместимыми с Enterprise Architect.
/// Отвечает за сборку packagedElement, ownedAttribute, memberEnd, ownedEnd и секции xmi:Extension
/// и учитывает флаги выгрузки комментариев, событий и опций настройки.
/// </summary>
internal sealed class XmiDocumentWriter
{
    private static readonly XNamespace xmi = "http://schema.omg.org/spec/XMI/2.1";
    private static readonly XNamespace uml = "http://schema.omg.org/spec/UML/2.1";
    private static readonly XNamespace profile = "http://www.sparxsystems.com/profiles/thecustomprofile/1.0";

    private readonly MacroProvider provider;
    private readonly ExportModel model;
    private readonly ExportOptions options;

    /// <summary>Создаёт генератор XMI для подготовленной модели выгрузки.</summary>
    /// <param name="provider">Экземпляр макроса для показа диалога сохранения файла.</param>
    /// <param name="model">Готовая UML/EA-модель выгрузки.</param>
    public XmiDocumentWriter(MacroProvider provider, ExportModel model)
    {
        this.provider = provider;
        this.model = model;
        this.options = model?.Options ?? new ExportOptions();
    }

    /// <summary>Запрашивает файл и сохраняет в него XMI-представление модели.</summary>
    public void WriteToFile()
    {
        ДиалогСохраненияФайла saveDialog = provider.СоздатьДиалогСохраненияФайла("Сохранение файла XMI");
        saveDialog.Заголовок = "Сохранение файла XMI";
        saveDialog.Фильтр = "Файлы XML (*.xml)|*.xml";
        saveDialog.РасширениеПоУмолчанию = "xml";
        if (!saveDialog.Показать()) return;

        XDocument document = BuildDocument();
        var writerSettings = new XmlWriterSettings { Encoding = new UTF8Encoding(false), Indent = true };
        using (XmlWriter writer = XmlWriter.Create(saveDialog.ИмяФайла, writerSettings))
            document.Save(writer);

        provider.Сообщение("Завершение", $"Схема успешно сохранена:\n{saveDialog.ИмяФайла}");
    }

    /// <summary>Собирает полный XMI-документ из модели выгрузки.</summary>
    /// <returns>Готовый XML-документ XMI 2.1.</returns>
    private XDocument BuildDocument()
    {
        string packageGuid = XmiSupport.FormatEaId(
            XmiSupport.CreateDeterministicGuid(model.PackageKey), prefix: "EAPK_");
        string exportTimestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        var package = new XElement("packagedElement",
            new XAttribute(xmi + "type", "uml:Package"),
            new XAttribute(xmi + "id", packageGuid),
            new XAttribute("name", $"Справочники T-FLEX DOCs {exportTimestamp}"),
            new XAttribute("visibility", "public"));

        foreach (CatalogModel catalog in model.Catalogs)
        {
            var catalogElement = new XElement("packagedElement",
                new XAttribute(xmi + "type", "uml:Component"),
                new XAttribute(xmi + "id", catalog.Guid),
                new XAttribute("name", catalog.Name),
                new XAttribute("visibility", "public"));
            AddOwnedComment(catalogElement, catalog.Comment);

            foreach (ClassModel cls in model.Classes.Where(item => item.CatalogGuid == catalog.Guid))
            {
                var classElement = new XElement("packagedElement",
                    new XAttribute(xmi + "type", "uml:Class"),
                    new XAttribute(xmi + "id", cls.Guid),
                    new XAttribute("name", cls.Name),
                    new XAttribute("visibility", "public"));
                AddOwnedComment(classElement, cls.Comment);

                foreach (ParamModel parameter in cls.Parameters)
                    classElement.Add(CreateProperty(parameter.Guid, parameter.Name, parameter.TypeRef));

                catalogElement.Add(classElement);
            }

            if (options.ExportEvents)
            {
                foreach (EventModel eventModel in catalog.Events)
                {
                    XElement signal = CreateSignal(eventModel);
                    AddOwnedComment(signal, eventModel.Comment);
                    catalogElement.Add(signal);
                }
            }

            package.Add(catalogElement);
        }

        foreach (AssocModel assoc in model.Assocs)
        {
            package.Add(new XElement("packagedElement",
                new XAttribute(xmi + "type", "uml:Association"),
                new XAttribute(xmi + "id", assoc.AssocGuid),
                string.IsNullOrEmpty(assoc.Name) ? null : new XAttribute("name", assoc.Name),
                new XAttribute("visibility", "public"),
                new XElement("memberEnd", new XAttribute(xmi + "idref", assoc.DstPropGuid)),
                new XElement("memberEnd", new XAttribute(xmi + "idref", assoc.SrcPropGuid)),
                CreateOwnedEnd(assoc.DstPropGuid, assoc.AssocGuid, assoc.TargetClassGuid, assoc.TargetRole,
                    assoc.TargetLower, assoc.TargetUpper),
                CreateOwnedEnd(assoc.SrcPropGuid, assoc.AssocGuid, assoc.SourceClassGuid, assoc.SourceRole,
                    assoc.SourceLower, assoc.SourceUpper)));
        }

        foreach (CatalogTypeAssocModel association in model.CatalogTypeAssocs)
            package.Add(CreateCatalogTypeAssociation(association));

        XElement extension = BuildEaExtension(packageGuid);
        IEnumerable<XElement> stereotypeApplications = BuildStereotypeApplications();

        return new XDocument(
            new XDeclaration("1.0", "utf-8", "yes"),
            new XElement(xmi + "XMI",
                new XAttribute(XNamespace.Xmlns + "xmi", xmi.NamespaceName),
                new XAttribute(XNamespace.Xmlns + "uml", uml.NamespaceName),
                new XAttribute(XNamespace.Xmlns + "thecustomprofile", profile.NamespaceName),
                new XAttribute(xmi + "version", "2.1"),
                new XElement(xmi + "Documentation",
                    new XAttribute("exporter", "Enterprise Architect"),
                    new XAttribute("exporterVersion", "6.5"),
                    new XAttribute("exporterID", "1554")),
                new XElement(uml + "Model",
                    new XAttribute(xmi + "type", "uml:Model"),
                    new XAttribute("name", "EA_Model"),
                    new XAttribute("visibility", "public"),
                    package,
                    stereotypeApplications),
                extension));
    }


    /// <summary>Создаёт XMI UML Property для атрибута класса и задаёт его тип.</summary>
    /// <param name="id">Глобальный идентификатор свойства.</param>
    /// <param name="name">Имя параметра.</param>
    /// <param name="typeRef">XMI idref стандартного типа.</param>
    /// <returns>XML-элемент ownedAttribute.</returns>
    private static XElement CreateProperty(string id, string name, string typeRef)
    {
        return new XElement("ownedAttribute",
            new XAttribute(xmi + "type", "uml:Property"), new XAttribute(xmi + "id", id),
            new XAttribute("name", name), new XAttribute("visibility", "public"),
            new XElement("lowerValue", new XAttribute(xmi + "type", "uml:LiteralInteger"), new XAttribute("value", "1")),
            new XElement("upperValue", new XAttribute(xmi + "type", "uml:LiteralInteger"), new XAttribute("value", "1")),
            new XElement("type", new XAttribute(xmi + "idref", typeRef)));
    }

    /// <summary>Создаёт UML Association между компонентом справочника и его типом.</summary>
    /// <param name="association">Идентификаторы и концы связи справочник—тип.</param>
    /// <returns>Пакетированный UML-элемент ассоциации.</returns>
    private static XElement CreateCatalogTypeAssociation(CatalogTypeAssocModel association)
    {
        Guid containsGuid = XmiSupport.CreateDeterministicGuid($"Contains_{association.CatalogGuid}_{association.TypeGuid}");
        association.Guid = XmiSupport.FormatEaId(containsGuid);
        string catalogEndId = XmiSupport.FormatEaId(containsGuid, "EAID_src_");
        string typeEndId = XmiSupport.FormatEaId(containsGuid, "EAID_dst_");
        return new XElement("packagedElement",
            new XAttribute(xmi + "type", "uml:Association"),
            new XAttribute(xmi + "id", association.Guid),
            new XAttribute("name", "Содержит"),
            new XAttribute("visibility", "public"),
            new XElement("memberEnd", new XAttribute(xmi + "idref", catalogEndId)),
            new XElement("memberEnd", new XAttribute(xmi + "idref", typeEndId)),
            new XElement("ownedEnd",
                new XAttribute(xmi + "type", "uml:Property"),
                new XAttribute(xmi + "id", catalogEndId),
                new XAttribute("association", association.Guid),
                new XElement("type", new XAttribute(xmi + "idref", association.CatalogGuid))),
            new XElement("ownedEnd",
                new XAttribute(xmi + "type", "uml:Property"),
                new XAttribute(xmi + "id", typeEndId),
                new XAttribute("association", association.Guid),
                new XElement("type", new XAttribute(xmi + "idref", association.TypeGuid))));
    }

    /// <summary>Создаёт UML ownedEnd для одного конца ассоциации с его ролью и кратностью.</summary>
    /// <param name="id">Идентификатор конца ассоциации.</param>
    /// <param name="assocId">Идентификатор ассоциации-владельца.</param>
    /// <param name="classGuid">GUID класса, являющегося типом конца.</param>
    /// <param name="role">Имя роли на этом конце.</param>
    /// <param name="lower">Нижняя граница кратности.</param>
    /// <param name="upper">Верхняя граница кратности или -1/* для неограниченной.</param>
    /// <returns>XML-элемент ownedEnd.</returns>
    private static XElement CreateOwnedEnd(string id, string assocId, string classGuid, string role, string lower, string upper)
    {
        bool unlimited = upper == "-1" || upper == "*";
        return new XElement("ownedEnd",
            new XAttribute(xmi + "type", "uml:Property"), new XAttribute(xmi + "id", id),
            string.IsNullOrEmpty(role) ? null : new XAttribute("name", role),
            new XAttribute("visibility", "public"), new XAttribute("association", assocId),
            new XAttribute("isStatic", "false"),
            new XAttribute("isReadOnly", "true"),
            new XAttribute("aggregation", "none"),
            new XElement("type", new XAttribute(xmi + "idref", classGuid)),
            new XElement("lowerValue",
                new XAttribute(xmi + "type", "uml:LiteralInteger"),
                new XAttribute(xmi + "id", $"{id}_lower"),
                new XAttribute("value", lower)),
            new XElement("upperValue",
                new XAttribute(xmi + "type", unlimited ? "uml:LiteralUnlimitedNatural" : "uml:LiteralInteger"),
                new XAttribute(xmi + "id", $"{id}_upper"),
                new XAttribute("value", unlimited ? "-1" : upper)));
    }

    /// <summary>Создаёт packagedElement UML Signal для события DOCs.</summary>
    /// <param name="eventModel">Модель события с именем и GUID.</param>
    /// <returns>Элемент сигнала XMI 2.1.</returns>
    private static XElement CreateSignal(EventModel eventModel)
    {
        return new XElement("packagedElement",
            new XAttribute(xmi + "type", "uml:Signal"),
            new XAttribute(xmi + "id", eventModel.Guid),
            new XAttribute("name", eventModel.Name),
            new XAttribute("visibility", "public"));
    }

    /// <summary>Добавляет UML Comment к элементу только при включённой выгрузке комментариев.</summary>
    /// <param name="umlElement">Элемент UML-модели, к которому прикрепляется комментарий.</param>
    /// <param name="comment">Описание объекта из DOCs.</param>
    private void AddOwnedComment(XElement umlElement, string comment)
    {
        if (!options.ExportComments || string.IsNullOrWhiteSpace(comment)) return;

        string ownerGuid = Convert.ToString(umlElement.Attribute(xmi + "id"));
        umlElement.Add(new XElement("ownedComment",
            new XAttribute(xmi + "type", "uml:Comment"),
            new XAttribute(xmi + "id", XmiSupport.FormatEaId(
                XmiSupport.CreateDeterministicGuid($"Comment_{ownerGuid}"))),
            new XElement("body", comment)));
    }


    /// <summary>Формирует EA extension с каталогами, типами, атрибутами и коннекторами.</summary>
    /// <param name="packageGuid">XMI ID корневого UML-пакета.</param>
    /// <returns>Extension-узел Enterprise Architect для файла XMI.</returns>
    private XElement BuildEaExtension(string packageGuid)
    {
        string created = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        var elements = new XElement("elements");
        int localId = 0;

        AppendCatalogElements(elements, packageGuid, created, ref localId);
        AppendClassElements(elements, packageGuid, created, ref localId);
        if (options.ExportEvents) AppendSignalElements(elements, packageGuid, ref localId);

        var extension = new XElement(xmi + "Extension",
            new XAttribute("extender", "Enterprise Architect"),
            new XAttribute("extenderID", "6.5"));
        extension.Add(elements);
        extension.Add(BuildConnectors());
        extension.Add(new XElement("profiles", CreateCustomProfile()));
        return extension;
    }

    /// <summary>Добавляет EA-элементы справочников с их тегами настройки.</summary>
    /// <param name="elements">Контейнер elements секции xmi:Extension.</param>
    /// <param name="packageGuid">XMI ID корневого пакета.</param>
    /// <param name="created">Метка времени создания.</param>
    /// <param name="localId">Текущий локальный идентификатор EA.</param>
    private void AppendCatalogElements(XElement elements, string packageGuid, string created, ref int localId)
    {
        foreach (CatalogModel catalog in model.Catalogs)
        {
            var properties = new XElement("properties",
                new XAttribute("isSpecification", "false"),
                new XAttribute("sType", "Component"),
                new XAttribute("nType", "0"),
                new XAttribute("scope", "public"),
                new XAttribute("stereotype", "catalog"),
                new XAttribute("isRoot", "false"),
                new XAttribute("isLeaf", "false"),
                new XAttribute("isAbstract", "false"));
            if (options.ExportComments && !string.IsNullOrWhiteSpace(catalog.Comment))
                properties.Add(new XAttribute("documentation", catalog.Comment));

            var element = new XElement("element",
                new XAttribute(xmi + "idref", catalog.Guid),
                new XAttribute(xmi + "type", "uml:Component"),
                new XAttribute("name", catalog.Name),
                properties);
            element.Add(new XElement("model",
                new XAttribute("package", packageGuid),
                new XAttribute("tpos", localId),
                new XAttribute("ea_localid", localId++),
                new XAttribute("ea_eleType", "element")));
            element.Add(CreateEaProjectMetadata(created));
            element.Add(new XElement("code", new XAttribute("gentype", "Java")));
            element.Add(new XElement("style", new XAttribute("appearance",
                "BackColor=-1;BorderColor=-1;BorderWidth=-1;FontColor=-1;VSwimLanes=1;HSwimLanes=1;BorderStyle=0;")));
            element.Add(CreateTagsElement(catalog.Guid, catalog.DocsGuid,
                options.ExportSettings ? catalog.Settings : null));
            element.Add(new XElement("xrefs"));
            element.Add(new XElement("extendedProperties",
                new XAttribute("tagged", "0"), new XAttribute("package_name", catalog.Name)));

            IEnumerable<XElement> catalogLinks = model.CatalogTypeAssocs
                .Where(item => item.CatalogGuid == catalog.Guid)
                .Select(item => new XElement("Association",
                    new XAttribute(xmi + "id", item.Guid),
                    new XAttribute("start", item.CatalogGuid),
                    new XAttribute("end", item.TypeGuid)));
            element.Add(new XElement("links", catalogLinks));
            elements.Add(element);
        }
    }


    /// <summary>Добавляет EA-элементы типов с их атрибутами и связями.</summary>
    /// <param name="elements">Контейнер elements секции xmi:Extension.</param>
    /// <param name="packageGuid">XMI ID корневого пакета.</param>
    /// <param name="created">Метка времени создания.</param>
    /// <param name="localId">Текущий локальный идентификатор EA.</param>
    private void AppendClassElements(XElement elements, string packageGuid, string created, ref int localId)
    {
        foreach (ClassModel cls in model.Classes)
        {
            var properties = new XElement("properties",
                new XAttribute("isSpecification", "false"),
                new XAttribute("sType", "Class"),
                new XAttribute("nType", "0"),
                new XAttribute("scope", "public"),
                new XAttribute("stereotype", "type"),
                new XAttribute("isRoot", "false"),
                new XAttribute("isLeaf", "false"),
                new XAttribute("isAbstract", "false"));
            if (options.ExportComments && !string.IsNullOrWhiteSpace(cls.Comment))
                properties.Add(new XAttribute("documentation", cls.Comment));

            var element = new XElement("element",
                new XAttribute(xmi + "idref", cls.Guid),
                new XAttribute(xmi + "type", "uml:Class"),
                new XAttribute("name", cls.Name),
                properties);
            element.Add(new XElement("model",
                new XAttribute("package", packageGuid),
                new XAttribute("owner", cls.CatalogGuid),
                new XAttribute("tpos", localId),
                new XAttribute("ea_localid", localId++),
                new XAttribute("ea_eleType", "element")));
            element.Add(CreateEaProjectMetadata(created));
            element.Add(new XElement("code", new XAttribute("gentype", "Java")));
            element.Add(new XElement("style", new XAttribute("appearance",
                "BackColor=-1;BorderColor=-1;BorderWidth=-1;FontColor=-1;VSwimLanes=1;HSwimLanes=1;BorderStyle=0;")));

            // Обязательный тэг GUID типа DOCs (ClassObject или псевдокласс «Подключение»).
            AddTags(element, cls.Guid, cls.DocsGuid);

            if (cls.Parameters.Count > 0)
            {
                var attributes = new XElement("attributes");
                foreach (ParamModel parameter in cls.Parameters)
                {
                    var attributeElement = new XElement("attribute",
                        new XAttribute(xmi + "idref", parameter.Guid),
                        new XAttribute("name", parameter.Name),
                        new XElement("properties", new XAttribute("type", parameter.EaType)));
                    // Обязательный тэг GUID параметра DOCs (ParameterInfo).
                    AddTags(attributeElement, parameter.Guid, parameter.DocsGuid);
                    attributes.Add(attributeElement);
                }
                element.Add(attributes);
            }

            element.Add(new XElement("extendedProperties",
                new XAttribute("tagged", "0"),
                new XAttribute("package_name", model.Catalogs.First(item => item.Guid == cls.CatalogGuid).Name)));

            IEnumerable<XElement> classLinks = model.CatalogTypeAssocs
                .Where(item => item.TypeGuid == cls.Guid)
                .Select(item => new XElement("Association",
                    new XAttribute(xmi + "id", item.Guid),
                    new XAttribute("start", item.CatalogGuid),
                    new XAttribute("end", item.TypeGuid)))
                .Concat(model.Assocs
                    .Where(association => association.SourceClassGuid == cls.Guid ||
                                          association.TargetClassGuid == cls.Guid)
                    .Select(association => new XElement("Association",
                        new XAttribute(xmi + "id", association.AssocGuid),
                        new XAttribute("start", association.SourceClassGuid),
                        new XAttribute("end", association.TargetClassGuid))));
            element.Add(new XElement("links", classLinks));
            element.Add(new XElement("xrefs"));
            elements.Add(element);
        }
    }

    /// <summary>Добавляет EA-элементы событий (Signal).</summary>
    /// <param name="elements">Контейнер elements секции xmi:Extension.</param>
    /// <param name="packageGuid">XMI ID корневого пакета.</param>
    /// <param name="localId">Текущий локальный идентификатор EA.</param>
    private void AppendSignalElements(XElement elements, string packageGuid, ref int localId)
    {
        foreach (CatalogModel catalog in model.Catalogs)
        {
            foreach (EventModel eventModel in catalog.Events)
            {
                var properties = new XElement("properties",
                    new XAttribute("isSpecification", "false"),
                    new XAttribute("sType", "Signal"),
                    new XAttribute("nType", "0"),
                    new XAttribute("scope", "public"),
                    new XAttribute("stereotype", "event"));
                if (options.ExportComments && !string.IsNullOrWhiteSpace(eventModel.Comment))
                    properties.Add(new XAttribute("documentation", eventModel.Comment));

                var signalElement = new XElement("element",
                    new XAttribute(xmi + "idref", eventModel.Guid),
                    new XAttribute(xmi + "type", "uml:Signal"),
                    new XAttribute("name", eventModel.Name),
                    properties,
                    new XElement("model",
                        new XAttribute("package", packageGuid),
                        new XAttribute("owner", catalog.Guid),
                        new XAttribute("tpos", localId),
                        new XAttribute("ea_localid", localId++),
                        new XAttribute("ea_eleType", "element")));
                // Обязательный тэг GUID события DOCs (ParameterGroupEvent).
                AddTags(signalElement, eventModel.Guid, eventModel.DocsGuid);
                // Гарантированная простановка стереотипа «event» для события в секции xmi:Extension
                // (в дополнение к properties/@stereotype="event").
                signalElement.Add(CreateEventStereotype(eventModel.Guid));
                elements.Add(signalElement);
            }
        }
    }


    /// <summary>Формирует секцию connectors EA extension для всех ассоциаций модели.</summary>
    /// <returns>XML-элемент connectors.</returns>
    private XElement BuildConnectors()
    {
        var connectors = new XElement("connectors");
        foreach (AssocModel assoc in model.Assocs)
        {
            string sourceMultiplicity = XmiSupport.FormatMultiplicity(assoc.SourceLower, assoc.SourceUpper);
            string targetMultiplicity = XmiSupport.FormatMultiplicity(assoc.TargetLower, assoc.TargetUpper);

            connectors.Add(new XElement("connector",
                new XAttribute(xmi + "idref", assoc.AssocGuid),
                string.IsNullOrEmpty(assoc.Name) ? null : new XAttribute("name", assoc.Name),
                new XElement("source",
                    new XAttribute(xmi + "idref", assoc.SourceClassGuid),
                    new XElement("model", new XAttribute("type", "Class"), new XAttribute("name", assoc.SourceClassName)),
                    string.IsNullOrEmpty(assoc.SourceRole) ? null : new XElement("role",
                        new XAttribute("name", assoc.SourceRole), new XAttribute("visibility", "Public")),
                    new XElement("type", new XAttribute("multiplicity", sourceMultiplicity), new XAttribute("aggregation", "none")),
                    new XElement("modifiers", new XAttribute("isOrdered", "false"), new XAttribute("isNavigable", "true")),
                    new XElement("style", new XAttribute("value", "Owned=0;Navigable=Unspecified;"))),
                new XElement("target",
                    new XAttribute(xmi + "idref", assoc.TargetClassGuid),
                    new XElement("model", new XAttribute("type", "Class"), new XAttribute("name", assoc.TargetClassName)),
                    string.IsNullOrEmpty(assoc.TargetRole) ? null : new XElement("role",
                        new XAttribute("name", assoc.TargetRole), new XAttribute("visibility", "Public")),
                    new XElement("type", new XAttribute("multiplicity", targetMultiplicity), new XAttribute("aggregation", "none")),
                    new XElement("modifiers", new XAttribute("isOrdered", "false"), new XAttribute("isNavigable", "false")),
                    new XElement("style", new XAttribute("value", "Owned=0;Navigable=Unspecified;"))),
                new XElement("properties",
                    new XAttribute("ea_type", "Association"),
                    new XAttribute("direction", assoc.Direction ?? "Unspecified")),
                new XElement("labels",
                    new XAttribute("lb", sourceMultiplicity),
                    string.IsNullOrEmpty(assoc.SourceRole) ? null : new XAttribute("lt", $"+{assoc.SourceRole}"),
                    new XAttribute("rb", targetMultiplicity),
                    string.IsNullOrEmpty(assoc.TargetRole) ? null : new XAttribute("rt", $"+{assoc.TargetRole}"))));
        }

        foreach (CatalogTypeAssocModel association in model.CatalogTypeAssocs)
        {
            connectors.Add(new XElement("connector",
                new XAttribute(xmi + "idref", association.Guid),
                new XAttribute("name", "Содержит"),
                new XElement("source", new XAttribute(xmi + "idref", association.CatalogGuid)),
                new XElement("target", new XAttribute(xmi + "idref", association.TypeGuid)),
                new XElement("properties", new XAttribute("ea_type", "Association"))));
        }

        return connectors;
    }

    /// <summary>Создаёт блок служебных EA-свойств и временных меток.</summary>
    /// <param name="timestamp">Значение created/modified в ожидаемом EA формате.</param>
    /// <returns>XML-элемент project.</returns>
    private static XElement CreateEaProjectMetadata(string timestamp)
    {
        return new XElement("project",
            new XAttribute("author", "DOCs Macros"),
            new XAttribute("version", "1.0"),
            new XAttribute("phase", "1.0"),
            new XAttribute("created", timestamp),
            new XAttribute("modified", timestamp),
            new XAttribute("complexity", "1"),
            new XAttribute("status", "Proposed"));
    }

    /// <summary>Добавляет в EA-раздел tagged values обязательный тэг GUID сущности DOCs.</summary>
    /// <param name="element">Extension element сущности (справочник, тип, атрибут, событие).</param>
    /// <param name="modelElement">XMI ID элемента модели.</param>
    /// <param name="entityGuid">Исходный GUID сущности модели данных T-FLEX DOCs.</param>
    private static void AddTags(XElement element, string modelElement, Guid entityGuid)
    {
        element.Add(CreateTagsElement(modelElement, entityGuid, null));
    }

    /// <summary>Создаёт раздел tags с тэгом «T-FLEX DOCs GUID» и, при наличии, настройками.</summary>
    /// <param name="modelElement">XMI ID элемента модели.</param>
    /// <param name="entityGuid">Исходный GUID сущности DOCs.</param>
    /// <param name="settings">Пары русской подписи настройки и её значения; может быть null.</param>
    /// <returns>XML-элемент tags.</returns>
    private static XElement CreateTagsElement(
        string modelElement, Guid entityGuid, IEnumerable<KeyValuePair<string, string>> settings)
    {
        var tags = new XElement("tags", CreateGuidTag(modelElement, entityGuid));
        if (settings != null)
        {
            foreach (var setting in settings)
            {
                tags.Add(new XElement("tag",
                    new XAttribute(xmi + "id", XmiSupport.FormatEaId(
                        XmiSupport.CreateDeterministicGuid($"Tag_{modelElement}_{setting.Key}"))),
                    new XAttribute("name", setting.Key),
                    new XAttribute("value", setting.Value ?? string.Empty),
                    new XAttribute("modelElement", modelElement)));
            }
        }
        return tags;
    }

    /// <summary>Создаёт тэг «T-FLEX DOCs GUID» с исходным GUID сущности модели данных.</summary>
    /// <param name="modelElement">XMI ID элемента модели, к которому привязан тэг.</param>
    /// <param name="entityGuid">GUID сущности T-FLEX DOCs.</param>
    /// <returns>XML-элемент tag.</returns>
    private static XElement CreateGuidTag(string modelElement, Guid entityGuid)
    {
        return new XElement("tag",
            new XAttribute(xmi + "id", XmiSupport.FormatEaId(
                XmiSupport.CreateDeterministicGuid($"GuidTag_{modelElement}"))),
            new XAttribute("name", "T-FLEX DOCs GUID"),
            new XAttribute("value", entityGuid.ToString()),
            new XAttribute("modelElement", modelElement));
    }

    /// <summary>Формирует EA-элемент стереотипа «event» для события DOCs.</summary>
    /// <param name="elementXmiId">XMI ID элемента события.</param>
    /// <returns>XML-элемент stereotype с именем «event».</returns>
    private static XElement CreateEventStereotype(string elementXmiId)
    {
        return new XElement("stereotype",
            new XAttribute(xmi + "id", XmiSupport.FormatEaId(
                XmiSupport.CreateDeterministicGuid($"Stereo_event_{elementXmiId}"))),
            new XAttribute("name", "event"),
            new XAttribute("modelElement", elementXmiId));
    }


    /// <summary>Создаёт применения пользовательских стереотипов и тегов профиля.</summary>
    /// <returns>Узлы профиля, вкладываемые в UML Model.</returns>
    private IEnumerable<XElement> BuildStereotypeApplications()
    {
        IEnumerable<XElement> result = model.Catalogs
            .Select(catalog => new XElement(profile + "catalog", new XAttribute("base_Component", catalog.Guid)))
            .Concat(model.Classes.Select(cls => new XElement(profile + "type", new XAttribute("base_Class", cls.Guid))));

        if (options.ExportEvents)
        {
            result = result.Concat(model.Catalogs.SelectMany(catalog => catalog.Events.Select(eventModel =>
                new XElement(profile + "event", new XAttribute("base_Signal", eventModel.Guid)))));
        }

        if (options.ExportSettings)
        {
            result = result.Concat(model.Catalogs.SelectMany(catalog => catalog.Settings.Select(setting =>
                CreateProfileTag("Component", catalog.Guid, setting.Key, setting.Value))));
        }

        return result;
    }

    /// <summary>Создаёт профильное tagged value с безопасным XML-именем узла.</summary>
    /// <param name="baseType">Базовый UML-метакласс элемента.</param>
    /// <param name="elementId">Идентификатор элемента модели.</param>
    /// <param name="tagName">Отображаемое имя tagged value.</param>
    /// <param name="tagValue">Текстовое значение tagged value.</param>
    /// <returns>Узел профиля thecustomprofile.</returns>
    private static XElement CreateProfileTag(string baseType, string elementId, string tagName, string tagValue)
    {
        string profileName = System.Xml.XmlConvert.EncodeLocalName(tagName.Replace(' ', '_'));
        return new XElement(profile + profileName,
            new XAttribute("base_" + baseType, elementId),
            new XAttribute("__EAStereoName", tagName),
            new XAttribute(profileName, tagValue ?? string.Empty));
    }

    /// <summary>Определяет профиль Enterprise Architect и стереотипы каталога, типа и события.</summary>
    /// <returns>Определение UML Profile для включения в XMI Extension.</returns>
    private static XElement CreateCustomProfile()
    {
        var profileElement = new XElement(uml + "Profile",
            new XAttribute(xmi + "version", "2.1"),
            new XAttribute(xmi + "id", "thecustomprofile"),
            new XAttribute("nsPrefix", "thecustomprofile"),
            new XAttribute("name", "thecustomprofile"),
            new XAttribute("metamodelReference", "mmref01"),
            new XElement("packageImport",
                new XAttribute(xmi + "id", "mmref01"),
                new XElement("importedPackage", new XAttribute("href", "http://schema.omg.org/spec/UML/2.1/"))));

        AddProfileStereotype(profileElement, "catalog", "Component");
        AddProfileStereotype(profileElement, "type", "Class");
        AddProfileStereotype(profileElement, "event", "Signal");
        return profileElement;
    }

    /// <summary>Добавляет стереотип и его Extension к базовому UML-классу.</summary>
    /// <param name="profileElement">Профиль, который пополняется стереотипом.</param>
    /// <param name="stereotypeName">Имя стереотипа профиля.</param>
    /// <param name="baseType">UML-тип, к которому применим стереотип.</param>
    private static void AddProfileStereotype(XElement profileElement, string stereotypeName, string baseType)
    {
        string associationId = baseType + "_" + stereotypeName;
        string baseEndId = stereotypeName + "-base_" + baseType;
        string extensionEndId = "extension_" + stereotypeName;
        profileElement.Add(new XElement(uml + "packagedElement",
            new XAttribute(xmi + "type", "uml:Stereotype"),
            new XAttribute(xmi + "id", stereotypeName),
            new XAttribute("name", stereotypeName),
            new XElement(uml + "ownedAttribute",
                new XAttribute(xmi + "type", "uml:Property"),
                new XAttribute(xmi + "id", baseEndId),
                new XAttribute("name", "base_" + baseType),
                new XAttribute("association", associationId),
                new XElement("type", new XAttribute("href", "http://schema.omg.org/spec/UML/2.1/" + baseType)))));
        profileElement.Add(new XElement(uml + "packagedElement",
            new XAttribute(xmi + "type", "uml:Extension"),
            new XAttribute(xmi + "id", associationId),
            new XAttribute("name", "A_" + baseType + "_" + stereotypeName),
            new XAttribute("memberEnd", extensionEndId + " " + baseEndId),
            new XElement(uml + "ownedEnd",
                new XAttribute(xmi + "id", extensionEndId),
                new XAttribute("name", extensionEndId),
                new XAttribute("type", stereotypeName),
                new XAttribute("isComposite", "true"),
                new XAttribute("lower", "0"),
                new XAttribute("upper", "1"),
                new XAttribute("memberEnd", extensionEndId + " " + baseEndId))));
    }
}

