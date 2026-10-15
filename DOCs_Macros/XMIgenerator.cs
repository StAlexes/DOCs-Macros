// Макрос оставлен в глобальном пространстве имен, поскольку загрузчик T-FLEX DOCs
// находит MacroProvider по типу сборки, а не по имени пространства имен.
//
// Архитектура макроса:
//   XmiSchemaExporterMacro  — точка входа (MacroProvider): стартовый диалог и оркестрация.
//   ModelCollector          — сбор модели DOCs (2 режима выгрузки, прямой вызов API).
//   DrawIoDocumentWriter    — генерация диаграммы draw.io (mxGraph XML) из той же модели.
//   XmiDocumentWriter       — изоляция генерации готового XMI 2.1 / Enterprise Architect.
//   Xmi11DocumentWriter     — генерация XMI 1.1 (OMG UML 1.3) для импорта в старые версии EA.
//                             Справочники выгружаются как UML:Component (стереотип catalog),
//                             типы — как UML:Class (стереотип type) в общем корневом пакете,
//                             вхождение типа в справочник — связью «Содержит».
//
// Вся работа с моделью данных T-FLEX DOCs выполняется ТОЛЬКО через прямые свойства API
// (ReferenceCatalog.GetReferences, ReferenceInfo.Classes.AllClasses, ClassObject.ParameterGroups,
// ParameterInfo.Type.IsInt и т.д.)
//
// Все три экспортёра работают по одной модели ExportModel, поэтому состав данных (включая настройки
// структуры AddStructureSettings и кратности ассоциаций) в XMI 2.1, XMI 1.1 и draw.io совпадает.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
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

/// <summary>Формат результирующей диаграммы/файла.</summary>
public enum ExportFormat
{
    /// <summary>XMI 2.1 с профилем Enterprise Architect (основной формат обмена).</summary>
    Xmi21,

    /// <summary>XMI 1.1 (OMG UML 1.3) — формат импорта старых версий Enterprise Architect.</summary>
    Xmi11,

    /// <summary>Диаграмма draw.io (mxGraph XML).</summary>
    DrawIo
}

/// <summary>Параметры выгрузки, собираемые в стартовом диалоге макроса.</summary>
public sealed class ExportOptions
{
    /// <summary>Выбранный режим выгрузки.</summary>
    public ExportMode Mode { get; set; } = ExportMode.SelectedTypes;

    /// <summary>Формат результирующей выгрузки.</summary>
    public ExportFormat Format { get; set; } = ExportFormat.DrawIo;

    /// <summary>Флаг выгрузки комментариев (ownedComment/documentation).</summary>
    public bool ExportComments { get; set; } = true;

    /// <summary>Флаг выгрузки событий справочника (UML:Class со стереотипом «event»).</summary>
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
    /// <summary>Тип иерархии справочника DOCs (None, Simple, Complex).</summary>
    public string Hierarchy { get; set; } = "None";
    /// <summary>Пользовательские настройки структуры для tagged values.</summary>
    public List<KeyValuePair<string, string>> Settings { get; set; } = new List<KeyValuePair<string, string>>();
    /// <summary>События справочника; в XMI 1.1 выгружаются как UML:Class со стереотипом «event».</summary>
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
    internal const string FormatField = "Формат экспорта";
    internal const string DrawIoFormatText = "draw.io Diagram (*.drawio)";
    internal const string XmiFormatText = "XMI 2.1 (*.xmi)";
    internal const string Xmi11FormatText = "XMI 1.1 (*.xmi)";
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

        if (options.Format == ExportFormat.DrawIo)
        {
            DrawIoDocumentWriter.Write(this, model);
            return;
        }

        if (options.Format == ExportFormat.Xmi11)
        {
            Xmi11DocumentWriter.Write(this, model);
            return;
        }

        var writer = new XmiDocumentWriter(this, model);
        writer.WriteToFile();
    }

    /// <summary>
    /// Показывает первичный диалог выбора режима выгрузки и флагов настроек.
    /// </summary>
    /// <returns>Заполненные параметры либо null, если пользователь отменил ввод.</returns>
    private ExportOptions ShowStartupDialog()
    {
        var dialog = СоздатьДиалогВвода("Экспорт схемы T-FLEX DOCs");
        dialog.Ширина = 540;
        dialog.Высота = 380;

        dialog.ДобавитьГруппу(ModeGroupCaption);
        // ВАЖНО: перегрузка без значения по умолчанию оставляет поле пустым (GetValue вернёт null),
        // поэтому явно задаём предустановленный режим и делаем поле обязательным.
        dialog.ДобавитьВыборИзСписка(ModeField, ModeOptionRefs, true, new object[]
        {
            ModeOptionRefs,
            ModeOptionTypes
        });

        dialog.ДобавитьГруппу("Формат");
        dialog.ДобавитьВыборИзСписка(FormatField, DrawIoFormatText, true, new object[]
        {
            DrawIoFormatText,
            XmiFormatText,
            Xmi11FormatText
        });

        dialog.ДобавитьГруппу("Настройки");
        dialog.ДобавитьФлаг(CommentsKey, true, false, false);
        dialog.ДобавитьФлаг(EventsKey, false, false, false);
        dialog.ДобавитьФлаг(SettingsKey, true, false, false);

        if (!dialog.Показать()) return null;

        return new ExportOptions
        {
            Mode = ParseMode(dialog[ModeField]),
            Format = ParseFormat(dialog[FormatField]),
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

    /// <summary>Сопоставляет выбранный формат экспорта с перечислением форматов.</summary>
    private static ExportFormat ParseFormat(object value)
    {
        string text = Convert.ToString(value)?.Trim() ?? string.Empty;

        if (string.Equals(text, XmiFormatText, StringComparison.OrdinalIgnoreCase)) return ExportFormat.Xmi21;
        if (string.Equals(text, Xmi11FormatText, StringComparison.OrdinalIgnoreCase)) return ExportFormat.Xmi11;
        if (string.Equals(text, DrawIoFormatText, StringComparison.OrdinalIgnoreCase)) return ExportFormat.DrawIo;

        if (int.TryParse(text, out int index))
        {
            switch (index)
            {
                case 0: return ExportFormat.DrawIo;
                case 1: return ExportFormat.Xmi21;
                case 2: return ExportFormat.Xmi11;
            }
        }

        return ExportFormat.DrawIo;
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
            Comment = reference.Description?.Comment,
            Hierarchy = Convert.ToString(reference.HierarchyType)
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
                .Select(grouping => grouping.First())
                // Отбор по имени: у системных параметров DOCs встречаются разные GUID при
                // одинаковом имени, а два одноимённых атрибута в одном классе EA отвергает
                // как «повторяющиеся значения в индексе, ключевых полях или связях».
                .GroupBy(parameter => parameter.Name ?? string.Empty, StringComparer.OrdinalIgnoreCase)
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

            foreach (ParameterInfo parameter in (selection.ConnectionParameters ?? new List<ParameterInfo>())
                .GroupBy(item => item.Guid)
                .Select(grouping => grouping.First())
                // Псевдотип подключения тоже не должен содержать одноимённых атрибутов.
                .GroupBy(item => item.Name ?? string.Empty, StringComparer.OrdinalIgnoreCase)
                .Select(grouping => grouping.First()))
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

    /// <summary>
    /// Добавляет события владельца в накопитель экспорта, устраняя повторы по GUID сущности DOCs.
    /// </summary>
    /// <remarks>
    /// Проверка <paramref name="seen"/> отбрасывает повторные обходы одного и того же владельца.
    /// Идентификатор элемента модели при этом вычисляется из пары «справочник + событие»: одно
    /// системное событие DOCs может входить в несколько справочников, и без участия GUID
    /// справочника в ключе его xmi.id совпал бы у разных справочников.
    /// </remarks>
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

            // GUID сущности DOCs: у системных событий («Сохранение объекта», «Взятие на
            // редактирование») он одинаков во всех справочниках, поэтому служит только тегом
            // T-FLEX DOCs GUID и ключом устранения повторов внутри одного справочника.
            Guid docsGuid = eventObject.Guid != Guid.Empty
                ? eventObject.Guid
                : XmiSupport.CreateDeterministicGuid($"Event_{catalog.Guid}_{eventObject.Name}");
            if (!seen.Add(docsGuid)) continue;

            events.Add(new EventModel
            {
                // xmi.id события считается по паре «справочник + событие»: иначе одно и то же
                // системное событие даёт совпадающие xmi.id у разных справочников и EA падает
                // на импорте с ошибкой повторяющихся значений в индексе.
                Guid = XmiSupport.FormatEaId(
                    XmiSupport.CreateDeterministicGuid($"Event_{catalog.Guid}_{docsGuid}")),
                DocsGuid = docsGuid,
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

    /// <summary>Форматирует значение кратности для XMI/EA.</summary>
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
/// Генерирует диаграмму draw.io на основе подготовленной модели экспорта DOCs.
/// Справочники рисуются контейнерами <c>&lt;&lt;catalog&gt;&gt;</c>, их типы объектов — вложенными
/// карточками <c>&lt;&lt;type&gt;&gt;</c>; связи выводятся в ER-нотации Crow's Foot.
/// Кратности на концах рёбер берутся из кратностей ассоциаций модели, поэтому сложная
/// иерархия справочника (несколько родителей у типа) и простое дерево рисуются по-разному.
/// </summary>
public static class DrawIoDocumentWriter
{
    /// <summary>Общие параметры рёбер в ER-нотации; маркеры кратностей задаются отдельно для каждого конца.</summary>
    private const string EdgeStyleBase = "edgeStyle=orthogonalEdgeStyle;rounded=0;orthogonalLoop=1;jettySize=auto;html=1;curved=1;";

    /// <summary>Собирает стиль ребра ER-нотации с маркерами кратности на требуемых концах.</summary>
    /// <param name="strokeColor">Цвет линии ребра.</param>
    /// <param name="startArrow">Маркер со стороны источника либо null, если маркер не нужен.</param>
    /// <param name="endArrow">Маркер со стороны цели либо null, если маркер не нужен.</param>
    /// <returns>Значение атрибута style ячейки ребра.</returns>
    private static string BuildEdgeStyle(string strokeColor, string startArrow, string endArrow)
    {
        var builder = new StringBuilder(EdgeStyleBase);
        if (!string.IsNullOrWhiteSpace(startArrow))
            builder.Append("startArrow=").Append(startArrow).Append(";startFill=0;");
        if (!string.IsNullOrWhiteSpace(endArrow))
            builder.Append("endArrow=").Append(endArrow).Append(";endFill=0;");
        builder.Append("strokeColor=").Append(strokeColor).Append(';');
        return builder.ToString();
    }

    /// <summary>Переводит кратности конца ассоциации в маркер ER-нотации draw.io.</summary>
    /// <param name="lower">Нижняя граница кратности.</param>
    /// <param name="upper">Верхняя граница кратности либо * / -1 для неограниченной кратности.</param>
    /// <returns>Имя маркера draw.io: ERzeroToOne, ERone, ERzeroToMany либо ERoneToMany.</returns>
    private static string BuildErArrow(string lower, string upper)
    {
        string normalizedLower = string.IsNullOrWhiteSpace(lower) ? "0" : lower.Trim();
        string normalizedUpper = string.IsNullOrWhiteSpace(upper) ? "1" : upper.Trim();
        if (normalizedUpper == "-1") normalizedUpper = "*";

        if (normalizedUpper == "*")
            return normalizedLower == "1" ? "ERoneToMany" : "ERzeroToMany";
        return normalizedLower == "0" ? "ERzeroToOne" : "ERone";
    }

    /// <summary>Запрашивает путь и сохраняет диаграмму в формате draw.io.</summary>
    public static void Write(MacroProvider provider, ExportModel model)
    {
        var saveDialog = provider.СоздатьДиалогСохраненияФайла("Сохранение файла draw.io");
        saveDialog.Заголовок = "Сохранение файла draw.io";
        saveDialog.Фильтр = "Файлы draw.io (*.drawio)|*.drawio";
        saveDialog.РасширениеПоУмолчанию = "drawio";
        if (!saveDialog.Показать()) return;

        Write(saveDialog.ИмяФайла, model);
    }

    /// <summary>Сохраняет готовую модель в файл draw.io XML.</summary>
    /// <param name="filePath">Путь к создаваемому файлу .drawio.</param>
    /// <param name="model">Подготовленная модель экспорта; настройки структуры выгружаются, если разрешено <see cref="ExportOptions.ExportSettings"/>.</param>
    public static void Write(string filePath, ExportModel model)
    {
        bool exportSettings = model?.Options?.ExportSettings ?? true;
        var settings = new XmlWriterSettings { Encoding = new UTF8Encoding(false), Indent = true };
        using (var writer = XmlWriter.Create(filePath, settings))
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("mxfile");
            writer.WriteAttributeString("host", "Electron");
            writer.WriteAttributeString("type", "device");

            writer.WriteStartElement("diagram");
            writer.WriteAttributeString("id", "Diagram_TFLEX_Docs");
            writer.WriteAttributeString("name", "Схема T-FLEX DOCs");

            writer.WriteStartElement("mxGraphModel");
            writer.WriteAttributeString("dx", "1400");
            writer.WriteAttributeString("dy", "900");
            writer.WriteAttributeString("grid", "1");
            writer.WriteAttributeString("gridSize", "10");

            writer.WriteStartElement("root");

            writer.WriteStartElement("mxCell");
            writer.WriteAttributeString("id", "0");
            writer.WriteEndElement();

            writer.WriteStartElement("mxCell");
            writer.WriteAttributeString("id", "1");
            writer.WriteAttributeString("parent", "0");
            writer.WriteEndElement();

            var catalogCellMap = new Dictionary<string, string>(StringComparer.Ordinal);
            var catalogRectMap = new Dictionary<string, int[]>(StringComparer.Ordinal);
            var classCellMap = new Dictionary<string, string>(StringComparer.Ordinal);

            // Карточки справочников раскладываются полосами, типы — в строку под своим справочником.
            // Ширина блока считается по числу типов, поэтому строки разных справочников не пересекаются,
            // а увеличенные промежутки (catalogGapX/blockGapY) не дают блокам слипаться на диаграмме.
            const int startX = 60;
            const int startY = 60;
            const int cardWidth = 260;
            const int minCatalogHeight = 120;
            const int typeHeight = 190;
            const int catalogGapX = 160;
            const int catalogTypeGapY = 100;
            const int typeGapX = 80;
            const int blockGapY = 220;
            const int maxBandWidth = 1800;
            const int settingLineHeight = 18;

            foreach (CatalogModel catalog in model?.Catalogs ?? new List<CatalogModel>())
            {
                if (catalog == null || string.IsNullOrWhiteSpace(catalog.Guid)) continue;

                int typeCount = model.Classes.Count(item => item != null &&
                    string.Equals(item.CatalogGuid, catalog.Guid, StringComparison.Ordinal) &&
                    !string.IsNullOrWhiteSpace(item.Guid));

                string catalogCellId = "catalog_" + SafeId(catalog.Guid);
                catalogCellMap[catalog.Guid] = catalogCellId;

                int columns = Math.Max(typeCount, 1);
                // Блок справочника занимает всю ширину своих карточек типов, иначе следующий
                // справочник встал бы поверх них.
                int blockWidth = columns * cardWidth + (columns - 1) * typeGapX;
                int cardHeight = minCatalogHeight;
                if (exportSettings && catalog.Settings != null && catalog.Settings.Count > 0)
                {
                    // Блок настроек удлиняет карточку, иначе он наехал бы на карточки типов.
                    cardHeight += catalog.Settings.Count * settingLineHeight;
                }

                catalogRectMap[catalog.Guid] = new[] { 0, 0, blockWidth, cardHeight };
            }

            // Раскладываем блоки слева направо, переходя на новую полосу, когда полоса заполнена.
            int bandTop = startY;
            int bandHeight = 0;
            int cursorX = startX;

            foreach (CatalogModel catalog in model?.Catalogs ?? new List<CatalogModel>())
            {
                if (catalog == null || string.IsNullOrWhiteSpace(catalog.Guid)) continue;

                int[] rect = catalogRectMap[catalog.Guid];
                int blockWidth = rect[2];
                int cardHeight = rect[3];

                if (cursorX > startX && cursorX + blockWidth > startX + maxBandWidth)
                {
                    bandTop += bandHeight + blockGapY;
                    bandHeight = 0;
                    cursorX = startX;
                }

                rect[0] = cursorX;
                rect[1] = bandTop;
                cursorX += blockWidth + catalogGapX;
                bandHeight = Math.Max(bandHeight, cardHeight + catalogTypeGapY + typeHeight);
            }

            foreach (CatalogModel catalog in model?.Catalogs ?? new List<CatalogModel>())
            {
                if (catalog == null || string.IsNullOrWhiteSpace(catalog.Guid)) continue;
                if (!catalogRectMap.TryGetValue(catalog.Guid, out var rect)) continue;

                writer.WriteStartElement("mxCell");
                writer.WriteAttributeString("id", catalogCellMap[catalog.Guid]);
                writer.WriteAttributeString("value", BuildCatalogValue(catalog, exportSettings));
                writer.WriteAttributeString("style", "align=left;overflow=fill;html=1;dropTarget=0;whiteSpace=wrap;fillColor=#e1d5e7;strokeColor=#9673a6;");
                writer.WriteAttributeString("vertex", "1");
                writer.WriteAttributeString("parent", "1");

                writer.WriteStartElement("mxGeometry");
                writer.WriteAttributeString("x", rect[0].ToString());
                writer.WriteAttributeString("y", rect[1].ToString());
                writer.WriteAttributeString("width", cardWidth.ToString());
                writer.WriteAttributeString("height", rect[3].ToString());
                writer.WriteAttributeString("as", "geometry");
                writer.WriteEndElement();
                writer.WriteEndElement();
            }

            foreach (CatalogModel catalog in model?.Catalogs ?? new List<CatalogModel>())
            {
                if (catalog == null || string.IsNullOrWhiteSpace(catalog.Guid)) continue;
                if (!catalogRectMap.TryGetValue(catalog.Guid, out var catalogRect)) continue;

                int localTypeIndex = 0;
                foreach (ClassModel classModel in model.Classes.Where(item => item != null && string.Equals(item.CatalogGuid, catalog.Guid, StringComparison.Ordinal)))
                {
                    if (classModel == null || string.IsNullOrWhiteSpace(classModel.Guid)) continue;

                    string classCellId = "type_" + SafeId(classModel.Guid);
                    classCellMap[classModel.Guid] = classCellId;

                    writer.WriteStartElement("mxCell");
                    writer.WriteAttributeString("id", classCellId);
                    writer.WriteAttributeString("value", BuildClassValue(classModel));
                    writer.WriteAttributeString("style", "verticalAlign=top;align=left;overflow=fill;html=1;whiteSpace=wrap;fillColor=#f5f5f5;strokeColor=#666666;");
                    writer.WriteAttributeString("vertex", "1");
                    writer.WriteAttributeString("parent", "1");

                    writer.WriteStartElement("mxGeometry");
                    writer.WriteAttributeString("x", (catalogRect[0] + localTypeIndex * (cardWidth + typeGapX)).ToString());
                    writer.WriteAttributeString("y", (catalogRect[1] + catalogRect[3] + catalogTypeGapY).ToString());
                    writer.WriteAttributeString("width", cardWidth.ToString());
                    writer.WriteAttributeString("height", typeHeight.ToString());
                    writer.WriteAttributeString("as", "geometry");
                    writer.WriteEndElement();
                    writer.WriteEndElement();

                    localTypeIndex++;
                }
            }

            foreach (CatalogTypeAssocModel catalogTypeAssoc in model?.CatalogTypeAssocs ?? new List<CatalogTypeAssocModel>())
            {
                if (catalogTypeAssoc == null) continue;
                if (!catalogCellMap.TryGetValue(catalogTypeAssoc.CatalogGuid ?? string.Empty, out var catalogCellId)) continue;
                if (!classCellMap.TryGetValue(catalogTypeAssoc.TypeGuid ?? string.Empty, out var typeCellId)) continue;

                writer.WriteStartElement("mxCell");
                writer.WriteAttributeString("id", "contains_" + SafeId(catalogTypeAssoc.Guid ?? string.Empty));
                writer.WriteAttributeString("value", "Содержит");
                // Стрелка направлена к типу объектов; со стороны справочника маркер не рисуется.
                writer.WriteAttributeString("style", BuildEdgeStyle("#9673a6", null, "block"));
                writer.WriteAttributeString("source", catalogCellId);
                writer.WriteAttributeString("target", typeCellId);
                writer.WriteAttributeString("edge", "1");
                writer.WriteAttributeString("parent", "1");

                writer.WriteStartElement("mxGeometry");
                writer.WriteAttributeString("relative", "1");
                writer.WriteAttributeString("as", "geometry");
                writer.WriteEndElement();
                writer.WriteEndElement();
            }

            foreach (AssocModel assoc in model?.Assocs ?? new List<AssocModel>())
            {
                if (assoc == null || string.IsNullOrWhiteSpace(assoc.SourceClassGuid) || string.IsNullOrWhiteSpace(assoc.TargetClassGuid)) continue;
                if (!classCellMap.TryGetValue(assoc.SourceClassGuid, out var sourceCellId)) continue;
                if (!classCellMap.TryGetValue(assoc.TargetClassGuid, out var targetCellId)) continue;

                if (string.Equals(assoc.Name, "Состоит из", StringComparison.OrdinalIgnoreCase))
                {
                    writer.WriteStartElement("mxCell");
                    writer.WriteAttributeString("id", "hierarchy_" + SafeId(assoc.AssocGuid ?? string.Empty));
                    writer.WriteAttributeString("value", BuildHierarchyValue(assoc));
                    // Сложная иерархия допускает несколько родителей у типа, дерево — только одного.
                    writer.WriteAttributeString("style", BuildEdgeStyle(
                        "#B85450",
                        BuildErArrow(assoc.SourceLower, assoc.SourceUpper),
                        BuildErArrow(assoc.TargetLower, assoc.TargetUpper)));
                    writer.WriteAttributeString("source", sourceCellId);
                    writer.WriteAttributeString("target", targetCellId);
                    writer.WriteAttributeString("edge", "1");
                    writer.WriteAttributeString("parent", "1");

                    writer.WriteStartElement("mxGeometry");
                    writer.WriteAttributeString("relative", "1");
                    writer.WriteAttributeString("as", "geometry");
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                    continue;
                }

                writer.WriteStartElement("mxCell");
                writer.WriteAttributeString("id", "assoc_" + SafeId(assoc.AssocGuid ?? string.Empty));
                writer.WriteAttributeString("value", BuildLinkValue(assoc));
                writer.WriteAttributeString("style", BuildEdgeStyle(
                    "#0052CC",
                    BuildErArrow(assoc.SourceLower, assoc.SourceUpper),
                    BuildErArrow(assoc.TargetLower, assoc.TargetUpper)));
                writer.WriteAttributeString("source", sourceCellId);
                writer.WriteAttributeString("target", targetCellId);
                writer.WriteAttributeString("edge", "1");
                writer.WriteAttributeString("parent", "1");

                writer.WriteStartElement("mxGeometry");
                writer.WriteAttributeString("relative", "1");
                writer.WriteAttributeString("as", "geometry");
                writer.WriteEndElement();
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndDocument();
        }
    }

    private static string BuildCatalogValue(CatalogModel catalog, bool includeSettings)
    {
        if (catalog == null) return "<i>&lt;&lt;catalog&gt;&gt;</i>";

        var builder = new StringBuilder();
        builder.Append("<p style=\"margin:0px;margin-top:6px;text-align:center;\"><i>&lt;&lt;catalog&gt;&gt;</i><b></b></p>");
        builder.Append("<p style=\"margin:0px;margin-top:6px;text-align:center;\"><b>")
            .Append(SecurityElement.Escape(catalog.Name ?? "(без имени)"))
            .Append("</b></p>");
        builder.Append("<hr size=\"1\" style=\"border-style:solid;\">");
        builder.Append("<p style=\"margin:0px;margin-left:8px;\">+ T-FLEX DOCs GUID: ")
            .Append(SecurityElement.Escape(catalog.DocsGuid != Guid.Empty ? catalog.DocsGuid.ToString() : string.Empty))
            .Append("<br>+ Hierarchy: ")
            .Append(SecurityElement.Escape(string.IsNullOrWhiteSpace(catalog.Hierarchy) ? "None" : catalog.Hierarchy))
            .Append("</p>");
        if (includeSettings) AppendSettingsBlock(builder, catalog.Settings, "margin:0px;margin-left:8px;");
        return builder.ToString();
    }

    /// <summary>Добавляет секцию настроек структуры справочника с тем же составом, что и в XMI.</summary>
    /// <param name="builder">Накопитель HTML-разметки ячейки.</param>
    /// <param name="settings">Настройки структуры, собранные <see cref="ModelCollector"/>.</param>
    /// <param name="paragraphStyle">Значение атрибута style для абзаца настроек.</param>
    private static void AppendSettingsBlock(StringBuilder builder, List<KeyValuePair<string, string>> settings, string paragraphStyle)
    {
        if (builder == null || settings == null || settings.Count == 0) return;

        builder.Append("<hr size=\"1\" style=\"border-style:solid;\">");
        builder.Append("<p style=\"").Append(paragraphStyle).Append("\">");
        bool first = true;
        foreach (var setting in settings)
        {
            if (!first) builder.Append("<br>");
            first = false;
            builder.Append(SecurityElement.Escape(setting.Key ?? string.Empty))
                .Append(": ")
                .Append(SecurityElement.Escape(setting.Value ?? string.Empty));
        }
        builder.Append("</p>");
    }

    private static string BuildClassValue(ClassModel classModel)
    {
        if (classModel == null) return string.Empty;

        var builder = new StringBuilder();
        builder.Append("<p style=\"margin:0px;margin-top:4px;text-align:center;\"><i>&lt;&lt;</i><i>type</i><i>&gt;&gt;</i><br><b>")
            .Append(SecurityElement.Escape(classModel.Name ?? "(без имени)"))
            .Append("</b></p>");
        builder.Append("<hr size=\"1\" style=\"border-style: solid;\">");

        builder.Append("<p style=\"margin: 0px 0px 0px 4px;\">");
        if (classModel.Parameters != null)
        {
            foreach (var parameter in classModel.Parameters)
            {
                if (parameter == null) continue;
                string parameterName = string.IsNullOrWhiteSpace(parameter.Name) ? "(без имени)" : parameter.Name;
                string parameterType = string.IsNullOrWhiteSpace(parameter.EaType) ? "string" : parameter.EaType;
                builder.Append("+ ")
                    .Append(SecurityElement.Escape(parameterName))
                    .Append(": ")
                    .Append(SecurityElement.Escape(parameterType))
                    .Append("<br>");
            }
        }
        builder.Append("</p>");

        builder.Append("<hr size=\"1\" style=\"border-style:solid;\">");
        builder.Append("<p style=\"margin:0px;margin-left:4px;\">+ T-FLEX DOCs GUID: ")
            .Append(SecurityElement.Escape(classModel.DocsGuid != Guid.Empty ? classModel.DocsGuid.ToString() : string.Empty))
            .Append("</p>");

        return builder.ToString();
    }

    private static string BuildHierarchyValue(AssocModel assoc)
    {
        return SecurityElement.Escape(string.IsNullOrWhiteSpace(assoc?.Name) ? "Состоит из" : assoc.Name);
    }

    /// <summary>
    /// Формирует HTML-безопасную подпись связи для диаграммы draw.io.
    /// Если у межсправочной связи заданы наименования с обеих сторон
    /// (<see cref="AssocModel.SourceRole"/> и <see cref="AssocModel.TargetRole"/>),
    /// выводит оба наименования в формате «Наименование А &lt;--&gt; Наименование Б».
    /// Иначе используется общее имя ассоциации <see cref="AssocModel.Name"/> либо единственная заданная роль.
    /// </summary>
    /// <param name="assoc">Модель ассоциации между типами.</param>
    /// <returns>Экранированная строка подписи ребра draw.io.</returns>
    private static string BuildLinkValue(AssocModel assoc)
    {
        if (assoc == null) return string.Empty;

        string name;
        // Если связь имеет наименования с обеих сторон (со стороны обоих справочников),
        // выводим двустороннюю подпись вида «Наименование А <--> Наименование Б».
        // Для односторонних связей (например, «Родительский объект» / «Дочерний объект»
        // у подключения) сохраняется прежний выбор: Name -> SourceRole -> TargetRole.
        if (!string.IsNullOrWhiteSpace(assoc.SourceRole) && !string.IsNullOrWhiteSpace(assoc.TargetRole))
        {
            name = $"{assoc.SourceRole.Trim()} <--> {assoc.TargetRole.Trim()}";
        }
        else
        {
            name = !string.IsNullOrWhiteSpace(assoc.Name)
                ? assoc.Name
                : (!string.IsNullOrWhiteSpace(assoc.SourceRole) ? assoc.SourceRole : assoc.TargetRole);
        }

        if (string.IsNullOrWhiteSpace(name)) name = "Связь";
        return SecurityElement.Escape(name);
    }

    private static string SafeId(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return "cell";
        return value.Replace("-", "_").Replace("{", "_").Replace("}", "_").Replace(" ", "_");
    }
}


/// <summary>
/// Формирует и сохраняет XMI 1.1 (OMG UML 1.3) — формат, который понимают старые версии
/// Enterprise Architect и большинство UML-инструментов, работающих с XMI 1.x.
/// Работает по той же модели <see cref="ExportModel"/>, что и остальные экспортёры, поэтому состав
/// данных (типы, параметры, кратности концов ассоциаций и настройки структуры) не расходится с XMI 2.1
/// и draw.io. Отличия формата: вместо xmi:Extension с профилем EA здесь используются вложенные
/// UML:Package / UML:Component / UML:Class / UML:Classifier.feature и теги UML:ModelElement.taggedValue.
/// </summary>
/// <remarks>
/// Разметка рассчитана на строгую проверку UML 1.3 со стороны EA, поэтому у каждого элемента есть
/// обязательный набор атрибутов (visibility/isSpecification/isRoot/isLeaf/isAbstract и т. п.),
/// типы атрибутов задаются ссылками на объявленные в модели UML:DataType (eaxmiid0…eaxmiid4),
/// а все теги одного элемента собраны в единственный UML:ModelElement.taggedValue.
/// Справочники объявляются элементами UML:Component со стереотипом «catalog» (а не пакетами):
/// только Classifier может быть концом ассоциации, поэтому связь «Содержит» с типом выполнима
/// лишь для компонента. События выгружаются как UML:Class со стереотипом «event» (ea_stype = Signal),
/// поскольку тег UML:Signal EA импортирует без записи в t_stereotypes.
/// Все компоненты, классы и ассоциации лежат в одном корневом пакете
/// <c>EAPK_ROOT_DOCS</c> (у каждого элемента задан атрибут namespace="EAPK_ROOT_DOCS"),
/// а у каждого стереотипа есть блок UML:ModelElement.stereotype —
/// без него EA падает на t_stereotypes.AppliesTo.
/// Разбор выгрузки самим EA (файл XMI 1.1, сохранённый из EA) показал ещё четыре правила, которые
/// здесь соблюдены, иначе импорт падает или теряет данные:
/// 1) у элемента допускается ровно один xmi.id на всю модель — иначе СУБД отвечает ошибкой
///    «повторяющиеся значения в индексе, ключевых полях или связях» (поэтому события выводятся
///    один раз, см. WriteEvents);
/// 2) вхождение элемента в пакет EA определяет по тегам package / package_name / owner,
///    а не по вложенности Namespace.ownedElement (поэтому эти теги есть у каждого элемента);
/// 3) элемент (в том числе справочник) входит в пакет только если лежит внутри единственного
///    вложенного Namespace.ownedElement корневого пакета; несколько таких контейнеров подряд
///    EA «проглатывает» — остаётся лишь один, и часть элементов пропадает;
/// 4) кратность конца ассоциации задаётся атрибутом multiplicity («0..1»), а не структурой
///    UML:Multiplicity, а идентификаторов у концов нет вовсе.
/// Описания (documentation) выгружаются тегом documentation у самого элемента — именно так
/// приходят описания справочников и типов в выгрузке EA; отдельные UML:Comment для этого не нужны.
/// </remarks>
internal static class Xmi11DocumentWriter
{
    private const string Exporter = "Enterprise Architect";
    /// <summary>Версия экспортёра, которую объявляет сам EA в XMI 1.1.</summary>
    private const string ExporterVersion = "2.5";
    /// <summary>Идентификатор версии формата XMI, который объявляет EA.</summary>
    private const string ExporterId = "1554";
    /// <summary>Строка расширений XMI: совпадает с XMI.extensions в выгрузке EA.</summary>
    private const string EaExtender = "Enterprise Architect 2.5";
    /// <summary>
    /// URI пространства имён UML 1.3. Значение обязано совпадать с тем, что пишет сам EA
    /// (omg.org/UML1.3): по нему разборщик выбирает таблицу элементов UML 1.3. Спецификация OMG
    /// упоминает и вариант org.omg.xmi.namespace.UML, но EA его не распознаёт как свой.
    /// </summary>
    private const string UmlNamespace = "omg.org/UML1.3";
    // Идентификатор самой модели: EA использует префикс MX_EAID_ и 32-значный hex
    // (MX_EAID_7087B82CBF63C3DEBF62AC33AC91BADE). Литеральное «MX_EAID_MODEL» не проходит
    // проверку формата идентификаторов при импорте, поэтому генерируется такой же вид.
    private const string ModelXmiId = "MX_EAID_3A7C1B4D5E6F708192A3B4C5D6E7F809";
    private const string GuidTag = "T-FLEX DOCs GUID";
    private const string DocumentationTag = "documentation";
    private const string EaTypeTag = "ea_type";
    private const string EaStyleTag = "ea_stype";
    /// <summary>Направление связи в терминах EA: первый конец — источник, второй — приёмник.</summary>
    private const string EaDirection = "Source -> Destination";
    /// <summary>Направление не задано: у равнозначных межсправочных связей стрелок нет.</summary>
    private const string EaUnspecifiedDirection = "Unspecified";

    // Корневой пакет: в него складываются все сущности выгрузки, чтобы EA получил одно дерево
    // модели. Идентификатор задан строкой (а не EAID_…), как это делает сам EA для пакетов.
    private const string RootPackageName = "Справочники T-FLEX DOCs";
    private const string RootPackageId = "EAPK_4B8D2C6E9F017283A4B5C6D7E8F90123";

    // Имена стереотипов профиля: справочник выгружается как UML:Component, тип — как UML:Class,
    // событие — тоже как UML:Class, но со стереотипом event (тег UML:Signal в UML 1.3
    // EA импортирует как неполноценный элемент, поэтому события идут через класс).
    private const string CatalogStereotype = "catalog";
    private const string TypeStereotype = "type";
    private const string EventStereotype = "event";
    private const string ComponentStyle = "Component";
    private const string ClassStyle = "Class";
    /// <summary>Значение ea_stype для события: EA заводит его как Signal, хотя узел — UML:Class.</summary>
    private const string SignalStyle = "Signal";
    private const string StereotypeTag = "stereotype";
    /// <summary>Тег типа параметра; EA ожидает строчное имя тега.</summary>
    private const string TypeTag = "type";
    private const string OwnerTag = "owner";
    private const string ContainsAssociationName = "Содержит";
    // Тег direction описывает направление связи в терминах EA: концом-источником считается
    // справочник, концом-приёмником — его тип.
    private const string DirectionTag = "direction";

    // --- Служебные теги EA, по которым он раскладывает элементы по дереву модели ---------------
    // Разбор эталонной выгрузки EA (XMI11.xmi) показал, что вхождение элемента в пакет EA
    // определяет по этим тегам, а не по вложенности Namespace.ownedElement. Без package/owner
    // тип или событие импортируется «висящим» в корне, вне справочника.
    /// <summary>Тег идентификатора пакета, в который EA помещает элемент.</summary>
    private const string PackageTag = "package";
    /// <summary>Тег отображаемого имени пакета (EA показывает его в дереве модели).</summary>
    private const string PackageNameTag = "package_name";
    private const string EaNTypeTag = "ea_ntype";
    private const string VersionTag = "version";
    private const string VersionValue = "1.0";
    private const string IsActiveTag = "isActive";
    private const string PhaseTag = "phase";
    private const string PhaseValue = "1.0";
    private const string AuthorTag = "author";
    /// <summary>Автор элементов выгрузки: имя макроса, чтобы EA не показывал пустое поле.</summary>
    private const string AuthorValue = "DOCs Macros";
    private const string ComplexityTag = "complexity";
    private const string ComplexityValue = "1";
    private const string StatusTag = "status";
    private const string StatusValue = "Proposed";
    private const string GentypeTag = "gentype";
    private const string GentypeValue = "Java";
    private const string TaggedTag = "tagged";
    private const string TposTag = "tpos";

    // --- Оформление линий связи: значения повторяют выгрузку EA один в один ---------------------
    // Стиль 3 = прямая линия; без этих тегов EA рисует связь стрелками с обеих сторон,
    // потому что направление конца берётся из тега sourcestyle/deststyle, а не из isNavigable.
    private const string StyleTag = "style";
    private const string AssociationStyleValue = "3";
    private const string LineModeTag = "linemode";
    private const string LineColorTag = "linecolor";
    private const string LineWidthTag = "linewidth";
    private const string SeqNoTag = "seqno";
    private const string HeadStyleTag = "headStyle";
    private const string LineStyleTag = "lineStyle";
    private const string ZeroValue = "0";
    private const string EaSourceNameTag = "ea_sourceName";
    private const string EaTargetNameTag = "ea_targetName";
    private const string EaSourceTypeTag = "ea_sourceType";
    private const string EaTargetTypeTag = "ea_targetType";
    /// <summary>Имя связи на диаграмме (EA показывает его посередине линии).</summary>
    private const string MidNameTag = "mt";
    /// <summary>Роль конца-источника, которую EA показывает у начала линии (например, «+Связанные документы»).</summary>
    private const string LtTag = "lt";
    /// <summary>Роль конца-приёмника, которую EA показывает у конца линии.</summary>
    private const string RtTag = "rt";
    /// <summary>Кратность со стороны источника/приёмника в виде «0..*».</summary>
    private const string LowerBoundTag = "lb";
    private const string UpperBoundTag = "rb";
    /// <summary>Стиль конца-источника/приёмника: именно он задаёт, где рисовать стрелку.</summary>
    private const string SourceStyleTag = "sourcestyle";
    private const string DestStyleTag = "deststyle";
    private const string EaEndTag = "ea_end";
    private const string EaEndSource = "source";
    private const string EaEndTarget = "target";
    /// <summary>Ненавигируемый конец (стрелка не рисуется).</summary>
    private const string NonNavigableValue = "Owned=0;Navigable=Non-Navigable;";
    /// <summary>Навигируемый конец — на нём EA и рисует единственную стрелку.</summary>
    private const string NavigableValue = "Owned=0;Navigable=Navigable;";
    /// <summary>Направление не задано: стрелки нет вовсе (так оформлена связь «Содержит»).</summary>
    private const string UnspecifiedNavigableValue = "Owned=0;Navigable=Unspecified;";

    // --- Атрибуты концов ассоциации --------------------------------------------------------------
    // Кратность конца задаётся XML-атрибутом multiplicity. В выгрузке самого EA кратность
    // записана строкой «0..*»/«1», а не структурой UML:Multiplicity, поэтому её можно
    // переносить в теги lb/rb без числового преобразования.
    private const string MultiplicityAttribute = "multiplicity";
    /// <summary>Кратность «много»: сторона связи, на которую указывает стрелка.</summary>
    private const string ManyMultiplicityValue = "0..*";
    /// <summary>Кратность «ровно один».</summary>
    private const string OneMultiplicityValue = "1";
    /// <summary>Идентификаторы концов связи в служебных тегах EA.</summary>
    private const string EaSourceIdTag = "ea_sourceID";
    private const string EaTargetIdTag = "ea_targetID";

    // --- Служебные элементы, которые EA ожидает увидеть в каждом файле XMI 1.1 ------------------
    /// <summary>Корневой класс модели: EA заводит его при импорте и без него ругается на структуру.</summary>
    private const string EaRootClassName = "EARootClass";
    private const string EaRootClassId = "EAID_11111111_5487_4080_A7F4_41526CB0AA00";
    /// <summary>Стиль отрисовки прямоугольного элемента (значения по умолчанию самого EA).</summary>
    private const string ElementStyleValue =
        "BackColor=-1;BorderColor=-1;BorderWidth=-1;FontColor=-1;" +
        "VSwimLanes=1;HSwimLanes=1;BorderStyle=0;";

    // --- Служебные теги пакета: без них EA создаёт пакет, но не связывает его с моделью ----------
    // В выгрузке EA пакет описан этими тегами; отсутствие parent/created/modified не мешает
    // импорту, но пакет теряет признак контролируемости и настройки CRC.
    private const string ParentTag = "parent";
    // У элемента модели метки времени называются date_created/date_modified, а у пакета —
    // created/modified: EA берёт разные колонки t_object и t_package, поэтому имена совпадать
    // не должны (иначе метки элемента остаются пустыми в свойствах).
    private const string CreatedTag = "created";
    private const string ModifiedTag = "modified";
    private const string DateCreatedTag = "date_created";
    private const string DateModifiedTag = "date_modified";
    private const string IsControlledTag = "iscontrolled";
    private const string IsProtectedTag = "isprotected";
    private const string UsedTdTag = "usedtd";
    private const string LogXmlTag = "logxml";
    private const string PackageFlagsTag = "packageFlags";
    private const string FalseValue = "FALSE";
    private const string PackageFlagsValue = "CRC=0;";

    // --- Служебные теги элементов: их пишет EA, и по ним он различает повторные импорты --------
    // ea_guid позволяет EA сопоставить элемент уже имеющейся записи, а ea_localid/ea_eleType
    // заполняют служебные столбцы t_object. Без них импорт проходит, но элементы дублируются.
    private const string EaGuidTag = "ea_guid";
    private const string EaLocalIdTag = "ea_localid";
    private const string EaElementTypeTag = "ea_eleType";
    private const string ElementElementValue = "element";
    private const string DerivedTag = "derived";
    private const string OrderedTag = "ordered";
    private const string StaticTag = "static";
    private const string CollectionTag = "collection";
    private const string CollectionFalseValue = "false";
    private const string PositionTag = "position";
    /// <summary>Нижняя граница атрибута (тег lowerBound); у связи ту же роль играет тег lb.</summary>
    private const string AttrLowerBoundTag = "lowerBound";
    /// <summary>Верхняя граница атрибута (тег upperBound).</summary>
    private const string AttrUpperBoundTag = "upperBound";
    private const string UpperBoundOneValue = "1";
    private const string StyleExTag = "styleex";
    private const string StyleExValue = "IsLiteral=0;";

    /// <summary>Разделитель при склейке нескольких описаний одного элемента в одно примечание.</summary>
    private const string ListSeparator = "||";

    // Идентификаторы базовых типов UML, к которым привязываются атрибуты через
    // UML:StructuralFeature.type. Числа выбраны так же, как их назначает сам EA,
    // чтобы повторный импорт не создавал дубликаты типов данных.
    private const string StringDataTypeId = "eaxmiid0";
    private const string DoubleDataTypeId = "eaxmiid1";
    private const string BooleanDataTypeId = "eaxmiid2";
    private const string IntegerDataTypeId = "eaxmiid3";
    private const string DateTimeDataTypeId = "eaxmiid4";

    /// <summary>Описание одного базового типа UML: xmi.id и отображаемое имя.</summary>
    private struct DataTypeDefinition
    {
        public string Id;
        public string Name;
    }

    /// <summary>Базовые типы, публикуемые в модели для привязки типов атрибутов.</summary>
    private static readonly DataTypeDefinition[] DataTypes =
    {
        new DataTypeDefinition { Id = StringDataTypeId, Name = "string" },
        new DataTypeDefinition { Id = DoubleDataTypeId, Name = "double" },
        new DataTypeDefinition { Id = BooleanDataTypeId, Name = "boolean" },
        new DataTypeDefinition { Id = IntegerDataTypeId, Name = "int" },
        new DataTypeDefinition { Id = DateTimeDataTypeId, Name = "DateTime" }
    };

    // --- Состояние одной выгрузки ---------------------------------------------------------------
    // Все элементы пакета записываются плоским списком (EA строит дерево модели по тегам),
    // поэтому положение элемента в списке и окружение его справочника запоминаются в статике
    // на время вызова Write.

    /// <summary>Порядковый номер очередного элемента пакета (значение тега tpos).</summary>
    private static int elementIndex;

    /// <summary>
    /// Метка времени выгрузки. Записывается и в атрибут timestamp корневого элемента, и в теги
    /// created/modified элементов, поэтому вычисляется один раз на выгрузку, а не для каждого тега.
    /// </summary>
    private static string ExportTimestamp;

    /// <summary>Запрашивает файл и сохраняет в него XMI 1.1.</summary>
    /// <param name="provider">Экземпляр макроса для показа диалога сохранения файла.</param>
    /// <param name="model">Готовая UML/EA-модель выгрузки.</param>
    public static void Write(MacroProvider provider, ExportModel model)
    {
        var saveDialog = provider.СоздатьДиалогСохраненияФайла("Сохранение файла XMI 1.1");
        saveDialog.Заголовок = "Сохранение файла XMI 1.1";
        saveDialog.Фильтр = "Файлы XMI (*.xmi)|*.xmi";
        saveDialog.РасширениеПоУмолчанию = "xmi";
        if (!saveDialog.Показать()) return;

        Write(saveDialog.ИмяФайла, model);
        provider.Сообщение("Завершение", $"Схема успешно сохранена:\n{saveDialog.ИмяФайла}");
    }

    /// <summary>Сохраняет готовую модель в файл XMI 1.1 (OMG UML 1.3).</summary>
    /// <param name="filePath">Путь к создаваемому файлу .xmi.</param>
    /// <param name="model">Готовая UML/EA-модель выгрузки.</param>
    public static void Write(string filePath, ExportModel model)
    {
        if (string.IsNullOrWhiteSpace(filePath) || model == null) return;

        bool exportComments = model.Options?.ExportComments ?? true;
        bool exportEvents = model.Options?.ExportEvents ?? false;
        bool exportSettings = model.Options?.ExportSettings ?? true;

        // Кодировка без BOM — как в остальных экспортёрах макроса, чтобы EA не спотыкался о префикс.
        var settings = new XmlWriterSettings { Encoding = new UTF8Encoding(false), Indent = true };
        using (XmlWriter writer = XmlWriter.Create(filePath, settings))
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("XMI");
            writer.WriteAttributeString("xmi.version", "1.1");
            // Префиксное объявление пространства имён задаётся через overload с prefix/localName:
            // XmlWriter не принимает строку "xmlns:UML" как обычное имя атрибута.
            writer.WriteAttributeString("xmlns", "UML", null, UmlNamespace);
            // Метка времени выгрузки: EA показывает её в свойствах модели. Значение хранится в
            // поле, чтобы теги created/modified элементов не отличались от атрибута timestamp.
            ExportTimestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            writer.WriteAttributeString("timestamp", ExportTimestamp);

            WriteHeader(writer);

            writer.WriteStartElement("XMI.content");
            writer.WriteStartElement("UML", "Model", UmlNamespace);
            writer.WriteAttributeString("name", "EA Model");
            writer.WriteAttributeString("xmi.id", ModelXmiId);

            writer.WriteStartElement("UML", "Namespace.ownedElement", UmlNamespace);

            // Корневой класс модели: EA ожидает его в любом файле XMI 1.1 и по нему определяет
            // начало дерева. Без него элементы импортируются в «мусорный» пакет.
            writer.WriteStartElement("UML", "Class", UmlNamespace);
            writer.WriteAttributeString("name", EaRootClassName);
            writer.WriteAttributeString("xmi.id", EaRootClassId);
            writer.WriteAttributeString("isRoot", "true");
            writer.WriteAttributeString("isLeaf", "false");
            writer.WriteAttributeString("isAbstract", "false");
            writer.WriteEndElement(); // UML:Class (EARootClass)

            // Все сущности выгрузки лежат в одном корневом пакете: EA строит по файлу одно
            // дерево модели, а справочники-компоненты не могут быть контейнерами для типов.
            writer.WriteStartElement("UML", "Package", UmlNamespace);
            writer.WriteAttributeString("name", RootPackageName);
            writer.WriteAttributeString("xmi.id", RootPackageId);

            // Обязательный для UML 1.3 набор атрибутов пакета.
            writer.WriteAttributeString("isRoot", "false");
            writer.WriteAttributeString("isLeaf", "false");
            writer.WriteAttributeString("isAbstract", "false");
            writer.WriteAttributeString("visibility", "public");

            // Теги пакета: EA заводит по ним сам пакет в t_package, а дерево модели строит
            // по тегам package / package_name / owner. Пакет без этих тегов импортируется,
            // но его содержимое в дерево не попадает.
            WriteTaggedValues(
                writer,
                Tag(PackageTag, RootPackageId),
                Tag(PackageNameTag, RootPackageName),
                Tag(EaStyleTag, "Public"),
                Tag(VersionTag, VersionValue),
                Tag(PhaseTag, PhaseValue),
                Tag(StatusTag, StatusValue),
                Tag(AuthorTag, AuthorValue),
                Tag(ComplexityTag, ComplexityValue),
                Tag(TposTag, ZeroValue),
                Tag(GentypeTag, GentypeValue));

            // Единственный вложенный Namespace.ownedElement: несколько контейнеров подряд EA
            // разбирает как один, и элементы из «лишних» пропадают при импорте.
            writer.WriteStartElement("UML", "Namespace.ownedElement", UmlNamespace);

            // Пакет выгрузки в терминах EA — просто контейнер; вложенность типов и событий
            // задаётся тегами package/owner, а визуально — связью «Содержит».
            elementIndex = 0;
            foreach (CatalogModel catalog in model.Catalogs)
                WriteCatalog(writer, catalog, exportComments, elementIndex++);

            // Типы идут на одном уровне со справочниками: UML:Component не является контейнером
            // для классов, поэтому вхождение типа в справочник выражено связью «Содержит».
            foreach (ClassModel cls in model.Classes)
                WriteClass(writer, cls, exportComments, elementIndex++);

            if (exportEvents) WriteEvents(writer, model, exportComments);

            // Связь «справочник содержит тип»: справочник — UML:Component, поэтому его можно
            // указать типом конца ассоциации (в UML 1.3 AssociationEnd.type требует Classifier).
            foreach (CatalogTypeAssocModel catalogTypeAssoc in model.CatalogTypeAssocs)
                WriteContainsAssociation(writer, catalogTypeAssoc, elementIndex++);

            foreach (AssocModel association in model.Assocs)
                WriteAssociation(writer, association, elementIndex++);

            writer.WriteEndElement(); // UML:Namespace.ownedElement (корневой пакет)

            writer.WriteEndElement(); // UML:Package (EAPK_ROOT_DOCS)

            // Базовые типы UML объявляются ПОСЛЕ закрытия пакета, на уровне модели: в выгрузке
            // самого EA они стоят между </UML:Package> и </UML:Namespace.ownedElement> модели.
            // Атрибуты ссылаются на них через UML:StructuralFeature.type/xmi.idref.
            foreach (DataTypeDefinition dataType in DataTypes)
                WriteDataType(writer, dataType.Id, dataType.Name);

            writer.WriteEndElement(); // UML:Namespace.ownedElement (модель)
            writer.WriteEndElement(); // UML:Model

            // Пользовательские теги элементов (GUID DOCs и настройки структуры) записываются
            // на уровне модели, а не внутри UML:Component/UML:Class: во вложенном
            // ModelElement.taggedValue EA читает только системные свойства t_object.
            // В выгрузке самого EA эти теги лежат плоским списком сразу после </UML:Model>,
            // поэтому relation к элементу задаётся атрибутом modelElement.
            WriteModelLevelTaggedValues(writer, model, exportEvents, exportSettings);

            writer.WriteEndElement(); // XMI.content

            // Закрывающая секция XMI 1.1: без XMI.difference и XMI.extensions EA считает файл
            // неполным и может отказаться импортировать расширения профиля.
            writer.WriteStartElement("XMI.difference");
            writer.WriteEndElement(); // XMI.difference

            writer.WriteStartElement("XMI.extensions");
            writer.WriteAttributeString("xmi.extender", EaExtender);
            writer.WriteStartElement("EAModel.paramSub");
            writer.WriteEndElement(); // EAModel.paramSub
            writer.WriteEndElement(); // XMI.extensions

            writer.WriteEndElement(); // XMI
            writer.WriteEndDocument();
        }
    }

    /// <summary>Записывает секцию XMI.header с описанием экспортёра.</summary>
    /// <param name="writer">Открытый XML-писатель.</param>
    private static void WriteHeader(XmlWriter writer)
    {
        writer.WriteStartElement("XMI.header");
        writer.WriteStartElement("XMI.documentation");
        writer.WriteElementString("XMI.exporter", Exporter);
        writer.WriteElementString("XMI.exporterVersion", ExporterVersion);
        // exporterID — номер версии формата XMI, которым пользуется экспортёр; EA читает его
        // при импорте, чтобы выбрать разборщик 1.1 (значение то же, что у самого EA).
        writer.WriteElementString("XMI.exporterID", ExporterId);
        writer.WriteEndElement(); // XMI.documentation
        writer.WriteEndElement(); // XMI.header
    }

    /// <summary>
    /// Формирует набор тегов, который EA проставляет любому элементу модели.
    /// </summary>
    /// <remarks>
    /// Список и порядок тегов повторяют выгрузку самого EA: без блока справочник или тип
    /// импортируется, но теряет привязку к пакету, поэтому элементарные теги идут первыми,
    /// а стиль отрисовки — последним.
    /// </remarks>
    /// <param name="elementType">Значение тега ea_stype (Class, Component, Signal или Note).</param>
    /// <param name="documentation">Описание элемента; пустое значение означает отсутствие тега.</param>
    /// <param name="ownerId">Идентификатор владельца (справочника) или пустая строка.</param>
    /// <param name="position">Порядковый номер элемента внутри пакета (тег tpos).</param>
    private static List<KeyValuePair<string, string>> ElementTags(
        string elementType,
        string documentation,
        string ownerId,
        int position)
    {
        var tags = new List<KeyValuePair<string, string>>();
        if (!string.IsNullOrWhiteSpace(documentation)) tags.Add(Tag(DocumentationTag, documentation));

        tags.Add(Tag("isSpecification", "false"));
        tags.Add(Tag(EaStyleTag, elementType));
        tags.Add(Tag(EaNTypeTag, ZeroValue));
        tags.Add(Tag(VersionTag, VersionValue));
        tags.Add(Tag(IsActiveTag, "false"));
        tags.Add(Tag(PackageTag, RootPackageId));
        if (!string.IsNullOrWhiteSpace(ownerId)) tags.Add(Tag(OwnerTag, ownerId));
        tags.Add(Tag(DateCreatedTag, ExportTimestamp));
        tags.Add(Tag(DateModifiedTag, ExportTimestamp));
        tags.Add(Tag(GentypeTag, GentypeValue));
        tags.Add(Tag(TaggedTag, ZeroValue));
        tags.Add(Tag(PackageNameTag, RootPackageName));
        tags.Add(Tag(PhaseTag, PhaseValue));
        tags.Add(Tag(AuthorTag, AuthorValue));
        tags.Add(Tag(ComplexityTag, ComplexityValue));
        tags.Add(Tag(StatusTag, StatusValue));
        tags.Add(Tag(TposTag, position.ToString()));
        // ea_localid и ea_eleType заполняют служебные столбцы t_object: по ним EA отличает
        // элемент модели от диаграммы. Порядковый номер уникален в пределах одной выгрузки.
        tags.Add(Tag(EaLocalIdTag, (position + 1).ToString()));
        tags.Add(Tag(EaElementTypeTag, ElementElementValue));
        tags.Add(Tag(StyleTag, ElementStyleValue));
        return tags;
    }

    /// <summary>Записывает UML:Component справочника со стереотипом «catalog».</summary>
    /// <remarks>
    /// Внутри ModelElement.taggedValue компонента остаются только служебные теги EA: GUID DOCs
    /// и настройки структуры выгружаются на уровне модели методом
    /// <see cref="WriteModelLevelTaggedValues"/>, потому что EA читает их только по атрибуту
    /// modelElement, а не из вложенного блока тегов самого элемента.
    /// </remarks>
    /// <param name="writer">Открытый XML-писатель.</param>
    /// <param name="catalog">Справочник DOCs.</param>
    /// <param name="exportComments">Флаг выгрузки комментариев.</param>
    /// <param name="position">Порядковый номер элемента в пакете (тег tpos).</param>
    private static void WriteCatalog(
        XmlWriter writer,
        CatalogModel catalog,
        bool exportComments,
        int position)
    {
        writer.WriteStartElement("UML", "Component", UmlNamespace);
        writer.WriteAttributeString("name", catalog.Name ?? string.Empty);
        writer.WriteAttributeString("xmi.id", ModelId(catalog.Guid, $"Catalog_{catalog.Name}"));

        // namespace указывает на корневой пакет: EA по этому атрибуту раскладывает элементы
        // по дереву модели, а не полагается только на вложенность Namespace.ownedElement.
        writer.WriteAttributeString("namespace", RootPackageId);

        // Обязательный для UML 1.3 набор атрибутов: isSpecification в выгрузке самого EA
        // у компонента идёт тегом, а не атрибутом элемента, поэтому здесь его нет.
        writer.WriteAttributeString("isRoot", "false");
        writer.WriteAttributeString("isLeaf", "false");
        writer.WriteAttributeString("isAbstract", "false");
        writer.WriteAttributeString("visibility", "public");

        // Объявление стереотипа обязательно: EA заводит запись в t_stereotypes для каждого
        // применения стереотипа и без имени в блоке UML:Stereotype падает на AppliesTo.
        WriteStereotype(writer, CatalogStereotype);

        // Все теги компонента обязаны лежать в одном UML:ModelElement.taggedValue:
        // повторные контейнеры у одного элемента EA разбирает как ошибку структуры.
        // Первыми идут теги, по которым EA строит дерево модели и заполняет свойства элемента.
        var catalogTags = ElementTags(
            ComponentStyle,
            exportComments ? catalog.Comment : null,
            null,
            position);
        catalogTags.Add(Tag(StereotypeTag, CatalogStereotype));

        WriteTaggedValues(writer, catalogTags);

        writer.WriteEndElement(); // UML:Component
    }

    /// <summary>Записывает UML:Class типа объектов с его атрибутами.</summary>
    /// <remarks>
    /// Теги GUID DOCs и настроек структуры у класса не выгружаются: настройки принадлежат
    /// справочнику, а GUID DOCs и настройки записываются на уровне модели методом
    /// <see cref="WriteModelLevelTaggedValues"/>.
    /// </remarks>
    /// <param name="writer">Открытый XML-писатель.</param>
    /// <param name="cls">Тип объектов DOCs.</param>
    /// <param name="exportComments">Флаг выгрузки комментариев.</param>
    /// <param name="position">Порядковый номер элемента в пакете (тег tpos).</param>
    private static void WriteClass(
        XmlWriter writer,
        ClassModel cls,
        bool exportComments,
        int position)
    {
        if (cls == null) return;

        writer.WriteStartElement("UML", "Class", UmlNamespace);
        writer.WriteAttributeString("name", cls.Name ?? string.Empty);
        writer.WriteAttributeString("xmi.id", ModelId(cls.Guid, $"Class_{cls.Name}"));

        // namespace указывает на корневой пакет — тип лежит в том же пакете, что и справочники.
        writer.WriteAttributeString("namespace", RootPackageId);

        // Обязательный для UML 1.3 набор атрибутов класса. isSpecification у самого EA идёт
        // тегом, а не атрибутом элемента, поэтому здесь записывается только isActive.
        writer.WriteAttributeString("visibility", "public");
        writer.WriteAttributeString("isRoot", "false");
        writer.WriteAttributeString("isLeaf", "false");
        writer.WriteAttributeString("isAbstract", "false");
        // isActive — стандартный признак активного класса UML 1.3; типы DOCs активными не являются.
        writer.WriteAttributeString("isActive", "false");

        // Объявление стереотипа «type» — тот же обязательный блок, что и у справочника.
        WriteStereotype(writer, TypeStereotype);

        // Стандартный набор тегов EA. owner ссылается на компонент справочника: по нему EA
        // показывает тип внутри справочника, поэтому вложенность Namespace.ownedElement не нужна.
        var classTags = ElementTags(
            ClassStyle,
            exportComments ? cls.Comment : null,
            ModelId(cls.CatalogGuid, $"Catalog_{cls.CatalogGuid}"),
            position);
        classTags.Add(Tag(StereotypeTag, TypeStereotype));

        WriteTaggedValues(writer, classTags);

        if (cls.Parameters.Count > 0)
        {
            writer.WriteStartElement("UML", "Classifier.feature", UmlNamespace);
            int parameterIndex = 0;
            // Последний рубеж защиты от одноимённых атрибутов: EA отвергает два атрибута с
            // одинаковым именем в одном классе ошибкой «повторяющиеся значения в индексе,
            // ключевых полях или связях». У системных параметров DOCs встречаются разные GUID
            // при одинаковом имени, поэтому дубликаты отбрасываются и здесь.
            var uniqueParameters = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (ParamModel parameter in cls.Parameters)
            {
                if (parameter == null) continue;
                if (!uniqueParameters.Add(parameter.Name ?? string.Empty)) continue;
                WriteAttribute(writer, parameter, parameterIndex++);
            }
            writer.WriteEndElement(); // UML:Classifier.feature
        }

        writer.WriteEndElement(); // UML:Class
    }

    /// <summary>Записывает UML:Attribute — параметр типа объектов.</summary>
    /// <remarks>
    /// Порядок вложенных элементов повторяет выгрузку EA: значение по умолчанию, ссылка на
    /// базовый тип UML и только затем блок тегов. Атрибут не имеет xmi.id — EA ссылается на
    /// параметры по тегу ea_guid, а лишний идентификатор создаёт дубликат в t_attribute.
    /// Значение ea_guid берётся из ParamModel.Guid (идентификатор пары «класс + параметр»),
    /// а не из ParameterInfo.Guid: у общих параметров справочника последний одинаков во всех
    /// типах, и EA роняет импорт на повторяющихся значениях в индексе. Тот же ключ «класс +
    /// параметр» используется и для xmi.id тега <c>T-FLEX DOCs GUID</c>.
    /// </remarks>
    /// <param name="writer">Открытый XML-писатель.</param>
    /// <param name="parameter">Параметр (атрибут) типа объектов.</param>
    /// <param name="position">Порядковый номер параметра внутри класса (тег position).</param>
    private static void WriteAttribute(XmlWriter writer, ParamModel parameter, int position)
    {
        if (parameter == null) return;

        // Уникальный для пары «класс + параметр» hex-идентификатор атрибута: ParamModel.Guid
        // уже посчитан в BuildClasses как Property_{cls.Guid}_{parameter.Guid} и потому
        // различает одноимённые общие параметры разных типов.
        string uniqueAttrHex =
            !string.IsNullOrWhiteSpace(parameter.Guid) &&
            parameter.Guid.StartsWith("EAID_", StringComparison.OrdinalIgnoreCase)
                ? parameter.Guid.Substring(5)
                : XmiSupport.CreateDeterministicGuid($"Attr_{position}_{parameter.DocsGuid}")
                    .ToString("N").ToUpperInvariant();

        // EA хранит ea_guid в виде «{XXXXXXXX-....}», поэтому hex приводится к формату «D».
        string eaGuidFormatted = Guid.TryParseExact(uniqueAttrHex, "N", out Guid parsedAttrGuid)
            ? "{" + parsedAttrGuid.ToString("D").ToUpperInvariant() + "}"
            : "{" + uniqueAttrHex + "}";

        writer.WriteStartElement("UML", "Attribute", UmlNamespace);
        writer.WriteAttributeString("name", parameter.Name ?? string.Empty);

        // Обязательный для UML 1.3 набор атрибутов атрибута.
        writer.WriteAttributeString("changeable", "none");
        writer.WriteAttributeString("visibility", "public");
        writer.WriteAttributeString("ownerScope", "instance");
        writer.WriteAttributeString("targetScope", "instance");

        // Пустое значение по умолчанию: EA ожидает этот узел у каждого атрибута.
        writer.WriteStartElement("UML", "Attribute.initialValue", UmlNamespace);
        writer.WriteStartElement("UML", "Expression", UmlNamespace);
        writer.WriteEndElement(); // UML:Expression
        writer.WriteEndElement(); // UML:Attribute.initialValue

        // Тип атрибута задаётся ссылкой на базовый тип UML и дублируется тегом type
        // (имя тега строчными — как ожидает EA) для схем, разбирающих только теги.
        string eaType = !string.IsNullOrWhiteSpace(parameter.EaType) ? parameter.EaType : parameter.TypeRef;
        WriteAttributeType(writer, DataTypeIdFor(eaType));

        // Все теги атрибута обязаны лежать в одном UML:ModelElement.taggedValue: повторные
        // контейнеры у одного атрибута EA разбирает как ошибку структуры UML 1.3.
        var attributeTags = new List<KeyValuePair<string, string>>
        {
            Tag(TypeTag, eaType),
            Tag(DerivedTag, ZeroValue),
            Tag(OrderedTag, ZeroValue),
            Tag(StaticTag, ZeroValue),
            Tag(CollectionTag, CollectionFalseValue),
            Tag(PositionTag, position.ToString()),
            Tag(AttrLowerBoundTag, UpperBoundOneValue),
            Tag(AttrUpperBoundTag, UpperBoundOneValue),
            // ea_guid — постоянный идентификатор атрибута для EA (атрибуты не имеют xmi.id).
            // Значение обязано быть уникальным в пределах базы: общие параметры справочника
            // («Наименование», «Обозначение») встречаются во всех его типах с одинаковым
            // ParameterInfo.Guid, поэтому берётся идентификатор пары «класс + параметр» —
            // уже вычисленный в BuildClasses ParamModel.Guid.
            Tag(EaGuidTag, eaGuidFormatted),
            Tag(StyleExTag, StyleExValue)
        };

        // Тег T-FLEX DOCs GUID, как в выгрузке EA, несёт собственный xmi.id, поэтому пишется
        // в том же контейнере, но отдельной записью с атрибутом идентификатора. Ключом служит
        // уже вычисленный uniqueAttrHex («класс + параметр»): привязка к parameter.Guid давала
        // одинаковый xmi.id всем атрибутам с пустым или общим GUID, и EA ронял импорт на
        // повторяющихся значениях в индексе.
        WriteTaggedValuesWithId(
            writer,
            attributeTags,
            GuidTag,
            parameter.DocsGuid.ToString(),
            XmiSupport.FormatEaId(XmiSupport.CreateDeterministicGuid($"GuidTag_{uniqueAttrHex}")));

        writer.WriteEndElement(); // UML:Attribute
    }

    /// <summary>Записывает все события выгрузки одним проходом без повторов.</summary>
    /// <remarks>
    /// Событие DOCs может входить сразу в несколько справочников, поэтому обход пар
    /// «справочник — событие» писал один и тот же xmi.id несколько раз, и EA на импорте падал
    /// с ошибкой «повторяющиеся значения в индексе, ключевых полях или связях». Здесь каждое
    /// событие записывается один раз, а вхождение в справочник выражается тегом owner.
    /// </remarks>
    /// <param name="writer">Открытый XML-писатель.</param>
    /// <param name="model">Готовая модель выгрузки.</param>
    /// <param name="exportComments">Флаг выгрузки комментариев.</param>
    private static void WriteEvents(
        XmlWriter writer,
        ExportModel model,
        bool exportComments)
    {
        if (model == null) return;

        var written = new HashSet<string>(StringComparer.Ordinal);
        foreach (CatalogModel catalog in model.Catalogs)
        {
            if (catalog.Events == null) continue;

            foreach (EventModel eventModel in catalog.Events)
            {
                if (eventModel == null) continue;

                // Ключом служит xmi.id события: он вычисляется из GUID сущности DOCs и потому
                // одинаков у одного события, попавшего в несколько справочников.
                string eventId = ModelId(eventModel.Guid, $"Event_{eventModel.Name}");
                if (!written.Add(eventId)) continue;

                WriteEvent(writer, eventModel, exportComments, elementIndex++);
            }
        }
    }

    /// <summary>Записывает событие справочника как UML:Class со стереотипом «event».</summary>
    /// <remarks>
    /// Тег UML:Signal в выгрузке не используется: EA импортирует события как класс со стереотипом,
    /// иначе элемент не получает запись в t_stereotypes и теряет привязку к справочнику.
    /// Признак сигнала выражается служебным тегом ea_stype = Signal.
    /// </remarks>
    /// <param name="writer">Открытый XML-писатель.</param>
    /// <param name="eventModel">Событие DOCs.</param>
    /// <param name="exportComments">Флаг выгрузки комментариев.</param>
    /// <param name="position">Порядковый номер элемента в пакете (тег tpos).</param>
    private static void WriteEvent(
        XmlWriter writer,
        EventModel eventModel,
        bool exportComments,
        int position)
    {
        if (eventModel == null) return;

        writer.WriteStartElement("UML", "Class", UmlNamespace);
        writer.WriteAttributeString("name", eventModel.Name ?? string.Empty);
        writer.WriteAttributeString("xmi.id", ModelId(eventModel.Guid, $"Event_{eventModel.Name}"));

        // namespace указывает на корневой пакет — событие лежит на одном уровне с типами.
        writer.WriteAttributeString("namespace", RootPackageId);

        // Обязательный для UML 1.3 набор атрибутов класса; isSpecification здесь идёт тегом.
        writer.WriteAttributeString("visibility", "public");
        writer.WriteAttributeString("isRoot", "false");
        writer.WriteAttributeString("isLeaf", "false");
        writer.WriteAttributeString("isAbstract", "false");
        writer.WriteAttributeString("isActive", "false");

        // Объявление стереотипа «event» — без этого блока EA падает на t_stereotypes.AppliesTo.
        WriteStereotype(writer, EventStereotype);

        // owner ссылается на справочник, в который событие входит: по этому тегу EA помещает
        // событие внутрь компонента-справочника.
        var eventTags = ElementTags(
            SignalStyle,
            exportComments ? eventModel.Comment : null,
            ModelId(eventModel.CatalogGuid, $"Catalog_{eventModel.CatalogGuid}"),
            position);
        eventTags.Add(Tag(StereotypeTag, EventStereotype));

        WriteTaggedValues(writer, eventTags);

        writer.WriteEndElement(); // UML:Class (событие)
    }

    /// <summary>Записывает UML:Association с двумя концами и кратностями из модели DOCs.</summary>
    /// <remarks>
    /// Оформление повторяет выгрузку EA. Направление берётся из <see cref="AssocModel.Direction"/>:
    /// у направленных связей (иерархия «Состоит из», подключение «Родительский объект» /
    /// «Дочерний объект») оно равно «Source -> Destination», и стрелка ставится только на конце
    /// приёмника. У равнозначных межсправочных связей направление равно «Unspecified», и это
    /// значение обязательно пишется в теге direction: без него парсер XMI 1.1 в EA сам
    /// вычисляет направление и дорисовывает стрелку к целевому классу. Оба конца такой связи
    /// ненавигируемы и получают Navigable=Unspecified, поэтому EA рисует линию без стрелок.
    /// Атрибут name не пишется при пустом имени связи — так же, как в выгрузке EA у связей,
    /// которые опознаются только по ролям концов. Теги lt/rt переносят роли концов, а теги
    /// style/linemode и sourcestyle/deststyle задают оформление линии.
    /// </remarks>
    /// <param name="writer">Открытый XML-писатель.</param>
    /// <param name="association">Ассоциация между типами (связь, подключение или иерархия).</param>
    /// <param name="position">Порядковый номер связи в пакете (значение тега ea_localid).</param>
    private static void WriteAssociation(XmlWriter writer, AssocModel association, int position)
    {
        if (association == null) return;

        string sourceId = ModelId(association.SourceClassGuid, $"Class_{association.SourceClassName}");
        string targetId = ModelId(association.TargetClassGuid, $"Class_{association.TargetClassName}");

        // Признак направленной связи: у равнозначных межсправочных связей Direction равен
        // «Unspecified», и стрелки на такой связи быть не должно.
        bool isDirected = string.Equals(association.Direction, EaDirection, StringComparison.Ordinal);

        writer.WriteStartElement("UML", "Association", UmlNamespace);

        // EA не пишет пустой атрибут name: у равнозначных межсправочных связей имя связи
        // отсутствует, а роль каждого конца задаётся отдельно в UML:AssociationEnd.
        if (!string.IsNullOrWhiteSpace(association.Name))
            writer.WriteAttributeString("name", association.Name);

        writer.WriteAttributeString("xmi.id", ModelId(association.AssocGuid, $"Association_{association.Name}"));

        // Обязательный для UML 1.3 набор атрибутов ассоциации; visibility="public" EA
        // записывает в t_connector, без него связь помечается как скрытая.
        writer.WriteAttributeString("visibility", "public");
        writer.WriteAttributeString("isRoot", "false");
        writer.WriteAttributeString("isLeaf", "false");
        writer.WriteAttributeString("isAbstract", "false");

        // Полный набор служебных тегов связи: без ea_sourceName/ea_targetName EA теряет имена
        // концов, без ea_sourceID/ea_targetID — не связывает концы с уже созданными классами.
        var associationTags = new List<KeyValuePair<string, string>>
        {
            Tag(StyleTag, AssociationStyleValue),
            Tag(EaTypeTag, "Association")
        };

        // Для направленных связей (иерархия, подключение) ставится "Source -> Destination",
        // а для равнозначных межсправочных связей обязательно передаётся "Unspecified" —
        // без этого тега EA при импорте XMI 1.1 сам выставляет направление и рисует стрелку.
        associationTags.Add(Tag(DirectionTag, isDirected ? EaDirection : EaUnspecifiedDirection));

        associationTags.Add(Tag(LineModeTag, AssociationStyleValue));
        associationTags.Add(Tag(LineColorTag, ZeroValue));
        associationTags.Add(Tag(LineWidthTag, ZeroValue));
        associationTags.Add(Tag(SeqNoTag, ZeroValue));
        associationTags.Add(Tag(HeadStyleTag, ZeroValue));
        associationTags.Add(Tag(LineStyleTag, ZeroValue));
        associationTags.Add(Tag(EaLocalIdTag, (position + 1).ToString()));
        associationTags.Add(Tag(EaSourceNameTag, association.SourceClassName));
        associationTags.Add(Tag(EaTargetNameTag, association.TargetClassName));
        associationTags.Add(Tag(EaSourceTypeTag, ClassStyle));
        associationTags.Add(Tag(EaTargetTypeTag, ClassStyle));
        associationTags.Add(Tag(EaSourceIdTag, sourceId));
        associationTags.Add(Tag(EaTargetIdTag, targetId));
        associationTags.Add(Tag(
            LowerBoundTag,
            XmiSupport.FormatMultiplicity(association.SourceLower, association.SourceUpper)));

        // lt/rt дублируют роли концов в виде, который EA показывает у линии связи.
        if (!string.IsNullOrWhiteSpace(association.SourceRole))
            associationTags.Add(Tag(LtTag, "+" + association.SourceRole));
        if (!string.IsNullOrWhiteSpace(association.TargetRole))
            associationTags.Add(Tag(RtTag, "+" + association.TargetRole));

        associationTags.Add(Tag(MidNameTag, association.Name));
        associationTags.Add(Tag(
            UpperBoundTag,
            XmiSupport.FormatMultiplicity(association.TargetLower, association.TargetUpper)));

        WriteTaggedValues(writer, associationTags);

        writer.WriteStartElement("UML", "Association.connection", UmlNamespace);

        // Первым идёт конец источника, вторым — конец приёмника: так направление связи
        // сохраняется при чтении файла без секции xmi:Extension. У направленной связи источник
        // остаётся ненавигируемым (стрелка только у приёмника), у равнозначной ненавигируемы оба
        // конца, а оформление обоих задано как Unspecified — именно это и убирает стрелки.
        WriteAssociationEnd(
            writer,
            association.SourceClassGuid,
            association.SourceRole,
            true,
            false,
            UnspecifiedNavigableValue,
            association.SourceLower,
            association.SourceUpper);

        WriteAssociationEnd(
            writer,
            association.TargetClassGuid,
            association.TargetRole,
            false,
            isDirected,
            isDirected ? NavigableValue : UnspecifiedNavigableValue,
            association.TargetLower,
            association.TargetUpper);

        writer.WriteEndElement(); // UML:Association.connection
        writer.WriteEndElement(); // UML:Association
    }

    /// <summary>Записывает UML:AssociationEnd в оформлении выгрузки EA.</summary>
    /// <remarks>
    /// EA задаёт кратность XML-атрибутом multiplicity строкой («0..*», «1»), а не структурой
    /// UML:Multiplicity, и не присваивает концам xmi.id. Стрелку рисует тег sourcestyle/deststyle:
    /// конец со значением Navigable получает стрелку, Unspecified и Non-Navigable — нет.
    /// Значение тега согласовано с атрибутом isNavigable: у направленной связи навигируемым
    /// (и единственным со стрелкой) остаётся конец-приёмник, у равнозначной оба конца
    /// ненавигируемы и оформлены как Unspecified.
    /// Признак <paramref name="isSource"/> определяет, в какой из двух тегов попадёт
    /// <paramref name="endStyle"/>, а также значение служебного тега ea_end.
    /// </remarks>
    /// <param name="writer">Открытый XML-писатель.</param>
    /// <param name="classGuid">Идентификатор класса, на который ссылается конец.</param>
    /// <param name="role">Роль конца ассоциации; может быть пустой.</param>
    /// <param name="isSource">Признак конца-источника (иначе конец-приёмник).</param>
    /// <param name="isNavigable">Признак навигируемого конца.</param>
    /// <param name="endStyle">Стиль конца: Navigable, Non-Navigable или Unspecified.</param>
    /// <param name="lower">Нижняя граница кратности.</param>
    /// <param name="upper">Верхняя граница кратности (-1 или «*» означают неограниченную).</param>
    private static void WriteAssociationEnd(
        XmlWriter writer,
        string classGuid,
        string role,
        bool isSource,
        bool isNavigable,
        string endStyle,
        string lower,
        string upper)
    {
        writer.WriteStartElement("UML", "AssociationEnd", UmlNamespace);
        if (!string.IsNullOrWhiteSpace(role)) writer.WriteAttributeString("name", role);
        writer.WriteAttributeString("type", ModelId(classGuid, $"AssociationEndClass_{role}"));

        // Кратность пишется строкой, как в выгрузке EA: 0..*, 1 и подобные значения он
        // переносит в свойства конца ассоциации без дополнительной структуры.
        writer.WriteAttributeString(
            MultiplicityAttribute,
            XmiSupport.FormatMultiplicity(lower, upper));

        // Обязательный для UML 1.3 набор атрибутов конца ассоциации.
        writer.WriteAttributeString("visibility", "public");
        writer.WriteAttributeString("aggregation", "none");
        writer.WriteAttributeString("isOrdered", "false");
        writer.WriteAttributeString("isNavigable", isNavigable ? "true" : "false");

        // sourcestyle задаёт оформление конца-источника, deststyle — конца-приёмника; значение
        // тега обязательно согласовано с isNavigable, иначе стрелка рисуется не с той стороны.
        WriteTaggedValues(
            writer,
            Tag(isSource ? SourceStyleTag : DestStyleTag, endStyle),
            Tag(EaEndTag, isSource ? EaEndSource : EaEndTarget));

        writer.WriteEndElement(); // UML:AssociationEnd
    }

    /// <summary>Создаёт пару «тег — значение» для передачи в <see cref="WriteTaggedValues"/>.</summary>
    /// <param name="tag">Имя тега.</param>
    /// <param name="value">Значение тега.</param>
    private static KeyValuePair<string, string> Tag(string tag, string value)
    {
        return new KeyValuePair<string, string>(tag, value);
    }

    /// <summary>Записывает связь «Содержит» между компонентом справочника и классом его типа.</summary>
    /// <remarks>
    /// В UML 1.3 AssociationEnd.type требует Classifier, поэтому концом ассоциации может быть
    /// UML:Component справочника, но не UML:Package. Оформление повторяет выгрузку EA: конец
    /// справочника помечен как Navigable, конец типа — как Non-Navigable, оба ненавигируемые
    /// и без кратности, поэтому связь получается без стрелок.
    /// </remarks>
    /// <param name="writer">Открытый XML-писатель.</param>
    /// <param name="catalogTypeAssoc">Пара «справочник — тип» из модели выгрузки.</param>
    /// <param name="position">Порядковый номер связи в пакете (значение тега ea_localid).</param>
    private static void WriteContainsAssociation(
        XmlWriter writer,
        CatalogTypeAssocModel catalogTypeAssoc,
        int position)
    {
        if (catalogTypeAssoc == null) return;

        string catalogId = ModelId(catalogTypeAssoc.CatalogGuid, $"Catalog_{catalogTypeAssoc.CatalogName}");
        string typeId = ModelId(catalogTypeAssoc.TypeGuid, $"Class_{catalogTypeAssoc.TypeName}");
        string associationId = ModelId(
            catalogTypeAssoc.Guid,
            $"Contains_{catalogTypeAssoc.CatalogGuid}_{catalogTypeAssoc.TypeGuid}");

        writer.WriteStartElement("UML", "Association", UmlNamespace);
        writer.WriteAttributeString("name", ContainsAssociationName);
        writer.WriteAttributeString("xmi.id", associationId);

        // Обязательный для UML 1.3 набор атрибутов ассоциации; visibility="public" EA
        // записывает в t_connector, без него связь помечается как скрытая.
        writer.WriteAttributeString("visibility", "public");
        writer.WriteAttributeString("isRoot", "false");
        writer.WriteAttributeString("isLeaf", "false");
        writer.WriteAttributeString("isAbstract", "false");

        // Направление у связи «Содержит» не задаётся, поэтому и стрелки на ней нет.
        WriteTaggedValues(
            writer,
            Tag(StyleTag, AssociationStyleValue),
            Tag(EaTypeTag, "Association"),
            Tag(LineModeTag, AssociationStyleValue),
            Tag(LineColorTag, ZeroValue),
            Tag(LineWidthTag, ZeroValue),
            Tag(SeqNoTag, ZeroValue),
            Tag(HeadStyleTag, ZeroValue),
            Tag(LineStyleTag, ZeroValue),
            Tag(EaLocalIdTag, (position + 1).ToString()),
            Tag(EaSourceNameTag, catalogTypeAssoc.TypeName),
            Tag(EaTargetNameTag, catalogTypeAssoc.CatalogName),
            Tag(EaSourceTypeTag, ClassStyle),
            Tag(EaTargetTypeTag, ComponentStyle),
            Tag(EaSourceIdTag, typeId),
            Tag(EaTargetIdTag, catalogId),
            Tag(MidNameTag, ContainsAssociationName));

        writer.WriteStartElement("UML", "Association.connection", UmlNamespace);

        // Конец типа: его имя EA пишет в ea_sourceName, поэтому конец идёт первым.
        WriteContainerEnd(writer, typeId, NavigableValue, true);

        // Конец справочника: приёмник, стиль Non-Navigable как в выгрузке EA.
        WriteContainerEnd(writer, catalogId, NonNavigableValue, false);

        writer.WriteEndElement(); // UML:Association.connection
        writer.WriteEndElement(); // UML:Association
    }

    /// <summary>Записывает конец связи «Содержит» в оформлении выгрузки EA.</summary>
    /// <remarks>
    /// Кратность у концов не пишется, оба конца ненавигируемые — так EA рисует связь
    /// контейнирования без стрелок, как и в выгрузке XMI 2.1.
    /// </remarks>
    /// <param name="writer">Открытый XML-писатель.</param>
    /// <param name="elementId">Идентификатор элемента на конце связи (класс типа или компонент).</param>
    /// <param name="navigableStyle">Стиль конца из выгрузки EA (Navigable или Non-Navigable).</param>
    /// <param name="isSource">Признак конца-источника (иначе конец-приёмник).</param>
    private static void WriteContainerEnd(
        XmlWriter writer,
        string elementId,
        string navigableStyle,
        bool isSource)
    {
        writer.WriteStartElement("UML", "AssociationEnd", UmlNamespace);
        writer.WriteAttributeString("visibility", "public");
        writer.WriteAttributeString("aggregation", "none");
        writer.WriteAttributeString("isOrdered", "false");
        writer.WriteAttributeString("isNavigable", "false");
        writer.WriteAttributeString("type", elementId);

        // sourcestyle задаёт оформление конца-источника, deststyle — конца-приёмника.
        WriteTaggedValues(
            writer,
            Tag(isSource ? SourceStyleTag : DestStyleTag, navigableStyle),
            Tag(EaEndTag, isSource ? EaEndSource : EaEndTarget));

        writer.WriteEndElement(); // UML:AssociationEnd
    }

    /// <summary>Записывает объявление стереотипа элемента UML:ModelElement.stereotype → UML:Stereotype.</summary>
    /// <remarks>
    /// EA при импорте заводит по этому блоку запись в t_stereotypes и связывает её с элементом
    /// через AppliesTo. Если элемент помечен тегом stereotype, но блока UML:Stereotype нет,
    /// импорт падает с ошибкой столбца AppliesTo, поэтому блок обязателен для каждого стереотипа.
    /// </remarks>
    /// <param name="writer">Открытый XML-писатель.</param>
    /// <param name="stereotypeName">Имя стереотипа профиля (catalog или type).</param>
    private static void WriteStereotype(XmlWriter writer, string stereotypeName)
    {
        if (string.IsNullOrWhiteSpace(stereotypeName)) return;

        writer.WriteStartElement("UML", "ModelElement.stereotype", UmlNamespace);
        writer.WriteStartElement("UML", "Stereotype", UmlNamespace);
        writer.WriteAttributeString("name", stereotypeName);
        writer.WriteEndElement(); // UML:Stereotype
        writer.WriteEndElement(); // UML:ModelElement.stereotype
    }

    /// <summary>
    /// Записывает все переданные теги в единственном контейнере UML:ModelElement.taggedValue.
    /// </summary>
    /// <remarks>
    /// Один элемент UML не может иметь несколько контейнеров taggedValue — EA воспринимает это
    /// как нарушение структуры UML 1.3, поэтому теги копятся в списке и выгружаются одним блоком.
    /// Экранирование значений выполняет XmlWriter, SecurityElement.Escape здесь применять нельзя.
    /// </remarks>
    /// <param name="writer">Открытый XML-писатель.</param>
    /// <param name="tags">Теги элемента; теги с пустым именем или значением пропускаются.</param>
    private static void WriteTaggedValues(XmlWriter writer, IEnumerable<KeyValuePair<string, string>> tags)
    {
        if (tags == null) return;

        bool containerStarted = false;
        foreach (KeyValuePair<string, string> tag in tags)
        {
            if (string.IsNullOrWhiteSpace(tag.Key) || string.IsNullOrWhiteSpace(tag.Value)) continue;

            if (!containerStarted)
            {
                writer.WriteStartElement("UML", "ModelElement.taggedValue", UmlNamespace);
                containerStarted = true;
            }

            writer.WriteStartElement("UML", "TaggedValue", UmlNamespace);
            writer.WriteAttributeString("tag", tag.Key);
            writer.WriteAttributeString("value", tag.Value);
            writer.WriteEndElement(); // UML:TaggedValue
        }

        if (containerStarted) writer.WriteEndElement(); // UML:ModelElement.taggedValue
    }

    /// <summary>Записывает теги элемента, заданные перечислением.</summary>
    /// <param name="writer">Открытый XML-писатель.</param>
    /// <param name="tags">Теги элемента.</param>
    private static void WriteTaggedValues(XmlWriter writer, params KeyValuePair<string, string>[] tags)
    {
        WriteTaggedValues(writer, (IEnumerable<KeyValuePair<string, string>>)tags);
    }

    /// <summary>
    /// Записывает теги элемента вместе с одним дополнительным тегом, у которого есть xmi.id.
    /// </summary>
    /// <remarks>
    /// В выгрузке EA тег T-FLEX DOCs GUID, в отличие от остальных тегов атрибута, объявлен с
    /// собственным xmi.id. Все записи обязаны лежать в единственном контейнере
    /// UML:ModelElement.taggedValue, поэтому общий список и «именованный» тег пишутся одной
    /// операцией, а не двумя вызовами.
    /// </remarks>
    /// <param name="writer">Открытый XML-писатель.</param>
    /// <param name="tags">Основные теги элемента.</param>
    /// <param name="idTagName">Имя тега с идентификатором.</param>
    /// <param name="idTagValue">Значение тега с идентификатором.</param>
    /// <param name="idTagId">Значение xmi.id этого тега.</param>
    private static void WriteTaggedValuesWithId(
        XmlWriter writer,
        IEnumerable<KeyValuePair<string, string>> tags,
        string idTagName,
        string idTagValue,
        string idTagId)
    {
        if (tags == null) return;

        writer.WriteStartElement("UML", "ModelElement.taggedValue", UmlNamespace);

        foreach (KeyValuePair<string, string> tag in tags)
        {
            if (string.IsNullOrWhiteSpace(tag.Key) || string.IsNullOrWhiteSpace(tag.Value)) continue;

            writer.WriteStartElement("UML", "TaggedValue", UmlNamespace);
            writer.WriteAttributeString("tag", tag.Key);
            writer.WriteAttributeString("value", tag.Value);
            writer.WriteEndElement(); // UML:TaggedValue
        }

        if (!string.IsNullOrWhiteSpace(idTagName) && !string.IsNullOrWhiteSpace(idTagValue))
        {
            writer.WriteStartElement("UML", "TaggedValue", UmlNamespace);
            writer.WriteAttributeString("tag", idTagName);
            writer.WriteAttributeString("value", idTagValue);
            if (!string.IsNullOrWhiteSpace(idTagId)) writer.WriteAttributeString("xmi.id", idTagId);
            writer.WriteEndElement(); // UML:TaggedValue
        }

        writer.WriteEndElement(); // UML:ModelElement.taggedValue
    }

    /// <summary>
    /// Записывает пользовательские теги элементов на уровне модели.
    /// </summary>
    /// <remarks>
    /// В XMI 1.1 (UML 1.3) EA читает пользовательские теги только из плоского списка
    /// UML:TaggedValue: во вложенном блоке ModelElement.taggedValue компонента или класса он
    /// разбирает лишь системные свойства t_object. Привязка к элементу задаётся атрибутом
    /// modelElement, поэтому теги справочников, типов и событий собраны здесь — сразу после
    /// </UML:Model>, как в выгрузке самого EA.
    /// </remarks>
    /// <param name="writer">Открытый XML-писатель.</param>
    /// <param name="model">Готовая модель выгрузки.</param>
    /// <param name="exportEvents">Флаг выгрузки событий.</param>
    /// <param name="exportSettings">Флаг выгрузки опций настройки структуры.</param>
    private static void WriteModelLevelTaggedValues(
        XmlWriter writer,
        ExportModel model,
        bool exportEvents,
        bool exportSettings)
    {
        if (model == null) return;

        // GUID DOCs и настройки структуры справочника.
        foreach (CatalogModel catalog in model.Catalogs)
        {
            if (catalog == null) continue;

            string catalogId = ModelId(catalog.Guid, $"Catalog_{catalog.Name}");
            WriteModelTaggedValue(writer, catalogId, GuidTag, catalog.DocsGuid.ToString());

            if (!exportSettings || catalog.Settings == null) continue;

            foreach (KeyValuePair<string, string> setting in catalog.Settings)
                WriteModelTaggedValue(writer, catalogId, setting.Key, setting.Value);
        }

        // GUID DOCs типов: настройки структуры им не принадлежат и здесь не повторяются.
        foreach (ClassModel cls in model.Classes)
        {
            if (cls == null) continue;

            WriteModelTaggedValue(
                writer,
                ModelId(cls.Guid, $"Class_{cls.Name}"),
                GuidTag,
                cls.DocsGuid.ToString());
        }

        if (!exportEvents) return;

        // Событие может входить в несколько справочников, поэтому его тег пишется один раз —
        // обход повторяет устранение повторов из WriteEvents.
        var written = new HashSet<string>(StringComparer.Ordinal);
        foreach (CatalogModel catalog in model.Catalogs)
        {
            if (catalog?.Events == null) continue;

            foreach (EventModel eventModel in catalog.Events)
            {
                if (eventModel == null) continue;

                string eventId = ModelId(eventModel.Guid, $"Event_{eventModel.Name}");
                if (!written.Add(eventId)) continue;

                WriteModelTaggedValue(writer, eventId, GuidTag, eventModel.DocsGuid.ToString());
            }
        }
    }

    /// <summary>Записывает один тег уровня модели с привязкой к элементу через modelElement.</summary>
    /// <remarks>
    /// Тег пишется плоским элементом в XMI.content рядом с UML:Model: контейнер
    /// ModelElement.taggedValue на этом уровне EA не создаёт, а определяет принадлежность тега
    /// исключительно по атрибуту modelElement.
    /// </remarks>
    /// <param name="writer">Открытый XML-писатель.</param>
    /// <param name="modelElementId">xmi.id элемента, к которому относится тег.</param>
    /// <param name="tagName">Имя тега.</param>
    /// <param name="tagValue">Значение тега; null заменяется пустой строкой.</param>
    private static void WriteModelTaggedValue(
        XmlWriter writer,
        string modelElementId,
        string tagName,
        string tagValue)
    {
        if (string.IsNullOrWhiteSpace(modelElementId) || string.IsNullOrWhiteSpace(tagName)) return;

        // Идентификатор тега детерминирован парой «элемент + тег», чтобы повторный импорт
        // не создавал второй экземпляр того же пользовательского тега.
        string tagId = XmiSupport.FormatEaId(
            XmiSupport.CreateDeterministicGuid($"ModelTag_{modelElementId}_{tagName}"));

        writer.WriteStartElement("UML", "TaggedValue", UmlNamespace);
        writer.WriteAttributeString("tag", tagName);
        writer.WriteAttributeString("xmi.id", tagId);
        writer.WriteAttributeString("value", tagValue ?? string.Empty);
        writer.WriteAttributeString("modelElement", modelElementId);
        writer.WriteEndElement(); // UML:TaggedValue
    }

    /// <summary>Привязывает атрибут к базовому типу UML через UML:StructuralFeature.type.</summary>
    /// <param name="writer">Открытый XML-писатель.</param>
    /// <param name="dataTypeId">Идентификатор базового типа (eaxmiid0…eaxmiid4).</param>
    private static void WriteAttributeType(XmlWriter writer, string dataTypeId)
    {
        writer.WriteStartElement("UML", "StructuralFeature.type", UmlNamespace);
        writer.WriteStartElement("UML", "Classifier", UmlNamespace);
        writer.WriteAttributeString("xmi.idref", dataTypeId);
        writer.WriteEndElement(); // UML:Classifier
        writer.WriteEndElement(); // UML:StructuralFeature.type
    }

    /// <summary>Записывает объявление базового типа UML (UML:DataType).</summary>
    /// <param name="writer">Открытый XML-писатель.</param>
    /// <param name="dataTypeId">Идентификатор типа (eaxmiid0…eaxmiid4).</param>
    /// <param name="name">Отображаемое имя типа.</param>
    private static void WriteDataType(XmlWriter writer, string dataTypeId, string name)
    {
        writer.WriteStartElement("UML", "DataType", UmlNamespace);
        writer.WriteAttributeString("xmi.id", dataTypeId);
        writer.WriteAttributeString("name", name);
        writer.WriteAttributeString("visibility", "private");
        writer.WriteAttributeString("isRoot", "false");
        writer.WriteAttributeString("isLeaf", "false");
        writer.WriteAttributeString("isAbstract", "false");
        writer.WriteEndElement(); // UML:DataType
    }

    /// <summary>Сопоставляет тип атрибута EA с идентификатором базового типа UML.</summary>
    /// <param name="eaType">Тип из модели выгрузки (EaType или TypeRef).</param>
    /// <returns>Идентификатор базового типа; для неизвестных типов — string (eaxmiid0).</returns>
    private static string DataTypeIdFor(string eaType)
    {
        if (string.IsNullOrWhiteSpace(eaType)) return StringDataTypeId;

        // Проверка по вхождению подстроки покрывает сразу оба вида значений: короткий тип EA
        // («double», «int») и ссылку XMI 2.1 («EAC__double», «EAC__date»).
        string type = eaType.Trim();
        if (type.IndexOf("date", StringComparison.OrdinalIgnoreCase) >= 0) return DateTimeDataTypeId;
        if (type.IndexOf("bool", StringComparison.OrdinalIgnoreCase) >= 0) return BooleanDataTypeId;
        if (type.IndexOf("double", StringComparison.OrdinalIgnoreCase) >= 0 ||
            type.IndexOf("decimal", StringComparison.OrdinalIgnoreCase) >= 0 ||
            type.IndexOf("money", StringComparison.OrdinalIgnoreCase) >= 0 ||
            type.IndexOf("number", StringComparison.OrdinalIgnoreCase) >= 0) return DoubleDataTypeId;
        if (type.IndexOf("int", StringComparison.OrdinalIgnoreCase) >= 0) return IntegerDataTypeId;
        return StringDataTypeId;
    }

    /// <summary>Возвращает готовый xmi.id либо детерминированно вычисленный, если он не задан.</summary>
    /// <param name="id">Значение идентификатора из модели выгрузки.</param>
    /// <param name="fallbackKey">Ключ для детерминированной подстановки.</param>
    /// <returns>Корректный для XMI идентификатор вида EAID_XXXXXXXX.</returns>
    private static string ModelId(string id, string fallbackKey)
    {
        return string.IsNullOrWhiteSpace(id)
            ? XmiSupport.FormatEaId(XmiSupport.CreateDeterministicGuid(fallbackKey ?? "unknown"))
            : id;
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
        string filter = options.Format == ExportFormat.DrawIo
            ? "Файлы draw.io (*.drawio)|*.drawio"
            : "Файлы XMI (*.xmi)|*.xmi";
        string defaultExtension = options.Format == ExportFormat.DrawIo ? "drawio" : "xmi";

        ДиалогСохраненияФайла saveDialog = provider.СоздатьДиалогСохраненияФайла(options.Format == ExportFormat.DrawIo ? "Сохранение файла draw.io" : "Сохранение файла XMI");
        saveDialog.Заголовок = options.Format == ExportFormat.DrawIo ? "Сохранение файла draw.io" : "Сохранение файла XMI";
        saveDialog.Фильтр = filter;
        saveDialog.РасширениеПоУмолчанию = defaultExtension;
        if (!saveDialog.Показать()) return;

        if (options.Format == ExportFormat.DrawIo)
        {
            DrawIoDocumentWriter.Write(saveDialog.ИмяФайла, model);
            provider.Сообщение("Завершение", $"Схема успешно сохранена:\n{saveDialog.ИмяФайла}");
            return;
        }

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

