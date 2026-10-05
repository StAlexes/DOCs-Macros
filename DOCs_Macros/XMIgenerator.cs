// Макрос оставлен в глобальном пространстве имен, поскольку загрузчик T-FLEX DOCs
// находит MacroProvider по типу сборки, а не по имени пространства имен.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using BindingFlags = System.Reflection.BindingFlags;
using TFlex.DOCs.Model;
using TFlex.DOCs.Model.Classes;
using TFlex.DOCs.Model.Macros;
using TFlex.DOCs.Model.Macros.ObjectModel;
using TFlex.DOCs.Model.Structure;

/// <summary>
/// Макрос T-FLEX DOCs, который экспортирует выбранные справочники, типы, параметры и связи
/// в XMI 2.1 с метаданными и профилем, совместимыми с Enterprise Architect.
/// </summary>
public class XmiSchemaExporterMacro : MacroProvider
{
    /// <summary>Создаёт макрос экспорта модели DOCs в XMI.</summary>
    /// <param name="context">Контекст выполнения макроса и подключения к DOCs.</param>
    public XmiSchemaExporterMacro(MacroContext context) : base(context) { }

    /// <summary>Данные выбора одного справочника, набора типов и параметров.</summary>
    private class SelectedCatalogContext
    {
        /// <summary>Справочник, выбранный в DOCs.</summary>
        public ReferenceInfo Reference { get; set; }
        /// <summary>Типы объектов, выбранные для экспорта из справочника.</summary>
        public ТипОбъекта[] Types { get; set; } = new ТипОбъекта[0];
        /// <summary>Параметры справочника, выбранные для включения в экспорт.</summary>
        public List<ParameterInfo> Parameters { get; set; } = new List<ParameterInfo>();
        /// <summary>Отдельно выбранные параметры группы структурного подключения.</summary>
        public List<ParameterInfo> ConnectionParameters { get; set; } = new List<ParameterInfo>();
        /// <summary>Найденная DOCs группа структурных подключений сложной иерархии.</summary>
        public object ConnectionGroup { get; set; }
    }

    /// <summary>Связь DOCs с однозначно определёнными каталогами master и slave.</summary>
    private class SelectedRelation
    {
        /// <summary>Группа параметров, описывающая связь в DOCs.</summary>
        public ParameterGroup Link { get; set; }
        /// <summary>Контекст каталога-владельца (master) связи.</summary>
        public SelectedCatalogContext Master { get; set; }
        /// <summary>Контекст целевого каталога (slave) связи.</summary>
        public SelectedCatalogContext Slave { get; set; }
        /// <summary>Признак, что связь относится к группе структурных подключений каталога.</summary>
        public bool IsConnectionRelation { get; set; }
    }

    /// <summary>Контекстное представление связи, полученное при опросе одного справочника.</summary>
    private class CatalogRelationItem
    {
        /// <summary>Группа связи DOCs с именем в контексте опрашиваемого справочника.</summary>
        public ParameterGroup Link { get; set; }
        /// <summary>Справочник, у которого запрошена связь.</summary>
        public SelectedCatalogContext SourceCatalog { get; set; }
        /// <summary>Связанный справочник, являющийся целью связи.</summary>
        public SelectedCatalogContext TargetCatalog { get; set; }
        /// <summary>Признак связи, полученной из группы структурного подключения.</summary>
        public bool IsConnectionRelation { get; set; }
        /// <summary>Ключ флажка для чтения выбора из диалога.</summary>
        public string DialogKey { get; set; }
    }

    /// <summary>Промежуточная UML-модель типа DOCs и его атрибутов/ассоциаций.</summary>
    private class ClassModel
    {
        public string Guid { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public string CatalogGuid { get; set; }
        public object SourceClass { get; set; }
        /// <summary>Исходный тип DOCs или pseudo-object, использованный для фильтрации параметров и связей.</summary>
        public object SourceType { get; set; }
        /// <summary>Указывает, что класс является псевдотипом структурного подключения.</summary>
        public bool IsConnection { get; set; }
        public List<ParamModel> Parameters { get; set; } = new List<ParamModel>();
        public List<AssocModel> Associations { get; set; } = new List<AssocModel>();
    }

    /// <summary>Промежуточная UML-модель справочника, его настроек и событий.</summary>
    private class CatalogModel
    {
        public string Guid { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public List<KeyValuePair<string, string>> Settings { get; set; } = new List<KeyValuePair<string, string>>();
        public List<EventModel> Events { get; set; } = new List<EventModel>();
    }

    /// <summary>Промежуточное представление пользовательского или системного события DOCs.</summary>
    private class EventModel
    {
        public string Guid { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public string CatalogGuid { get; set; }
    }

    /// <summary>Промежуточное представление параметра DOCs как UML Property.</summary>
    private class ParamModel
    {
        public string Guid { get; set; }
        public string Name { get; set; }
        public string TypeRef { get; set; }
        public string EaType { get; set; }
    }

    /// <summary>Промежуточное представление ассоциации между классами и ролей её концов.</summary>
    private class AssocModel
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
    private class CatalogTypeAssocModel
    {
        public string Guid { get; set; }
        public string CatalogGuid { get; set; }
        public string TypeGuid { get; set; }
        public string CatalogName { get; set; }
        public string TypeName { get; set; }
    }

    private static readonly XNamespace xmi = "http://schema.omg.org/spec/XMI/2.1";
    private static readonly XNamespace uml = "http://schema.omg.org/spec/UML/2.1";
    private static readonly XNamespace profile = "http://www.sparxsystems.com/profiles/thecustomprofile/1.0";

    /// <summary>Запрашивает произвольное количество справочников и запускает экспорт.</summary>
    public override void Run()
    {
        var selectedCatalogs = new List<SelectedCatalogContext>();
        var allReferences = Context.Connection.ReferenceCatalog.GetReferences()
            .Where(reference => reference != null)
            .ToList();
        var linkedReferenceGuids = BuildReferenceLinkCache(allReferences);

        while (true)
        {
            bool onlyLinked = false;
            if (selectedCatalogs.Count > 0)
            {
                bool? addMore = QuestionWithCancel(BuildAddMoreTypesPrompt(selectedCatalogs));
                if (addMore != true) break;

                string selectedCatalogNames = string.Join(", ", selectedCatalogs.Select(item => item.Reference.Name));
                bool? searchOnlyLinked = QuestionWithCancel(
                    $"Искать следующий справочник только среди связанных с уже выбранными ({selectedCatalogNames})?");
                if (!searchOnlyLinked.HasValue) break;
                onlyLinked = searchOnlyLinked.Value;
            }

            var excludedGuids = new HashSet<Guid>(selectedCatalogs.Select(item => item.Reference.Guid));
            IEnumerable<ReferenceInfo> candidates = allReferences.Where(reference => !excludedGuids.Contains(reference.Guid));
            if (onlyLinked)
            {
                var allowedGuids = new HashSet<Guid>();
                foreach (Guid selectedGuid in excludedGuids)
                    if (linkedReferenceGuids.TryGetValue(selectedGuid, out var neighbors))
                        allowedGuids.UnionWith(neighbors);
                candidates = candidates.Where(reference => allowedGuids.Contains(reference.Guid));
            }

            string title = $"Шаг 1: Выберите справочник ({selectedCatalogs.Count + 1})";
            string referenceName = SelectReference(
                title,
                candidates.ToList(),
                useNativePicker: !onlyLinked);
            if (string.IsNullOrWhiteSpace(referenceName)) break;

            var reference = Context.Connection.ReferenceCatalog.Find(referenceName);
            if (reference == null)
            {
                Сообщение("Ошибка", $"Справочник '{referenceName}' не найден.");
                if (selectedCatalogs.Count == 0) return;
                break;
            }

            var types = SelectObjectTypes(referenceName, $"Шаг 2: Выберите типы объектов справочника '{referenceName}'");
            if (types.Length > 0)
            {
                var parameters = SelectParameters(reference, $"Шаг 3: Выберите параметры справочника '{referenceName}'");
                var connectionGroup = FindConnectionGroup(reference);
                var connectionParameters = connectionGroup != null
                    ? SelectConnectionParameters(reference, connectionGroup,
                        $"Шаг 3.1: Выберите параметры подключения для '{referenceName}'")
                    : new List<ParameterInfo>();
                selectedCatalogs.Add(new SelectedCatalogContext
                {
                    Reference = reference,
                    Types = types,
                    Parameters = parameters,
                    ConnectionGroup = connectionGroup,
                    ConnectionParameters = connectionParameters
                });
            }

            if (types.Length == 0 && selectedCatalogs.Count == 0)
            {
                bool? tryAgain = QuestionWithCancel("Не выбраны типы объектов. Выбрать другой справочник?");
                if (tryAgain != true) return;
                continue;
            }
        }

        if (selectedCatalogs.Count == 0)
        {
            Сообщение("Информация", "Не выбран ни один справочник с типами объектов для экспорта.");
            return;
        }

        var selectedRelations = SelectRelations(selectedCatalogs);
        GenerateXmiFile(selectedCatalogs, selectedRelations);
    }

    /// <summary>Показывает штатный выбор справочника и при необходимости использует список-кандидатов.</summary>
    /// <param name="title">Заголовок диалога выбора справочника.</param>
    /// <param name="candidates">Справочники, разрешённые на текущем шаге выбора.</param>
    /// <returns>Имя выбранного каталога или null при отмене/отсутствии вариантов.</returns>
    private string SelectReference(string title, IList<ReferenceInfo> candidates, bool useNativePicker)
    {
        var references = candidates?.Where(reference => reference != null).ToList() ?? new List<ReferenceInfo>();
        if (references.Count == 0) return null;

        const string referenceKey = "Справочник";
        if (useNativePicker)
        {
            var dlg = СоздатьДиалогВвода(title);
            dlg.Высота = 300;
            dlg.Ширина = 560;
            if (TryAddReferencePicker(dlg, referenceKey, references, out var pickerResult))
            {
                if (!dlg.Показать()) return null;

                object selection = null;
                try { selection = dlg[referenceKey]; }
                catch { }
                var selectedReference = ResolveSelectedReference(selection, references)
                    ?? ResolveSelectedReference(pickerResult, references);
                if (selectedReference != null) return selectedReference.Name;
            }
        }

        var names = references.OrderBy(reference => reference.Name, StringComparer.CurrentCultureIgnoreCase)
            .Select(reference => reference.Name)
            .ToArray();
        var fallbackDialog = СоздатьДиалогВвода(title);
        fallbackDialog.Высота = 300;
        fallbackDialog.Ширина = 560;
        fallbackDialog.ДобавитьВыборИзСписка(referenceKey, names);
        if (!fallbackDialog.Показать()) return null;

        object fallbackSelection = null;
        try { fallbackSelection = fallbackDialog[referenceKey]; }
        catch { }
        return ResolveSelectedReference(fallbackSelection, references)?.Name;
    }

    /// <summary>Пытается добавить штатный селектор справочника с учётом сигнатур разных версий DOCs.</summary>
    /// <param name="dialog">Диалог DOCs, в который добавляется элемент выбора.</param>
    /// <param name="fieldName">Имя поля выбора.</param>
    /// <param name="candidates">Разрешённые справочники текущего шага.</param>
    /// <param name="pickerResult">Значение, возвращённое методом добавления селектора.</param>
    /// <returns>true, если метод селектора найден и успешно вызван.</returns>
    private static bool TryAddReferencePicker(object dialog, string fieldName, IList<ReferenceInfo> candidates, out object pickerResult)
    {
        pickerResult = null;
        if (dialog == null) return false;

        var methods = dialog.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public)
            .Where(method => method.Name == "ДобавитьВыборСправочника" && !method.IsGenericMethodDefinition);
        foreach (var method in methods)
        {
            var parameters = method.GetParameters();
            if (parameters.Length == 0 || parameters[0].ParameterType != typeof(string)) continue;

            var arguments = new object[parameters.Length];
            arguments[0] = fieldName;
            for (int index = 1; index < parameters.Length; index++)
            {
                var parameter = parameters[index];
                var parameterType = parameter.ParameterType;
                if (parameterType.IsInstanceOfType(candidates))
                    arguments[index] = candidates;
                else if (parameterType.IsArray && parameterType.GetElementType().IsAssignableFrom(typeof(ReferenceInfo)))
                    arguments[index] = candidates.ToArray();
                else if (parameterType.IsArray && parameterType.GetElementType() == typeof(string))
                    arguments[index] = candidates.Select(reference => reference.Name).ToArray();
                else if (parameterType.IsAssignableFrom(typeof(string[])))
                    arguments[index] = candidates.Select(reference => reference.Name).ToArray();
                else if (parameter.HasDefaultValue)
                    arguments[index] = parameter.DefaultValue;
                else if (parameterType == typeof(bool))
                    arguments[index] = false;
                else if (parameterType.IsValueType)
                    arguments[index] = Activator.CreateInstance(parameterType);
                else
                    arguments[index] = null;
            }

            try
            {
                object result = method.Invoke(dialog, arguments);
                pickerResult = method.ReturnType == typeof(void) ? null : result;
                return true;
            }
            catch
            {
                pickerResult = null;
            }
        }
        return false;
    }

    /// <summary>Разрешает результат селектора как объект справочника, его GUID или имя.</summary>
    /// <param name="value">Значение, полученное из диалога или из метода выбора.</param>
    /// <param name="candidates">Допустимые справочники текущего шага.</param>
    /// <returns>Совпавший справочник либо null, если результат не удалось однозначно сопоставить.</returns>
    private static ReferenceInfo ResolveSelectedReference(object value, IEnumerable<ReferenceInfo> candidates)
    {
        if (value == null) return null;
        if (value is ReferenceInfo referenceInfo)
            return candidates.FirstOrDefault(reference => reference.Guid == referenceInfo.Guid);

        object nestedReference = GetPropertyValue(value, "ReferenceInfo") ?? GetPropertyValue(value, "Reference");
        if (nestedReference != null && !ReferenceEquals(nestedReference, value))
        {
            var nestedResult = ResolveSelectedReference(nestedReference, candidates);
            if (nestedResult != null) return nestedResult;
        }

        Guid selectedGuid = value is Guid guid ? guid : ReadGuid(value, "Guid", "GUID", "ReferenceGuid");
        if (selectedGuid != Guid.Empty)
        {
            var byGuid = candidates.FirstOrDefault(reference => reference.Guid == selectedGuid);
            if (byGuid != null) return byGuid;
        }

        string selectedName = value is string text ? text : Convert.ToString(GetPropertyValue(value, "Name"));
        if (Guid.TryParse(selectedName, out selectedGuid))
        {
            var byGuid = candidates.FirstOrDefault(reference => reference.Guid == selectedGuid);
            if (byGuid != null) return byGuid;
        }
        return string.IsNullOrWhiteSpace(selectedName)
            ? null
            : candidates.FirstOrDefault(reference => string.Equals(reference.Name, selectedName, StringComparison.Ordinal));
    }

    /// <summary>Кэширует входящие и исходящие структурные связи всех доступных справочников.</summary>
    /// <param name="references">Полный список загруженных справочников DOCs.</param>
    /// <returns>Словарь GUID справочника к GUID всех непосредственно связанных справочников.</returns>
    private static Dictionary<Guid, HashSet<Guid>> BuildReferenceLinkCache(IList<ReferenceInfo> references)
    {
        var cache = references.ToDictionary(reference => reference.Guid, _ => new HashSet<Guid>());
        foreach (var source in references)
        {
            foreach (var link in GetLinksFromOwner(source.Description))
            {
                var target = GetSlaveReference(link);
                if (target == null || target.Guid == source.Guid) continue;

                if (cache.TryGetValue(source.Guid, out var outgoing)) outgoing.Add(target.Guid);
                if (cache.TryGetValue(target.Guid, out var incoming)) incoming.Add(source.Guid);
            }
        }
        return cache;
    }

    /// <summary>Формирует текст подтверждения с перечнем уже выбранных каталогов и типов.</summary>
    /// <param name="catalogs">Контексты справочников, накопленные в текущем цикле.</param>
    /// <returns>Многострочная подпись для диалога продолжения выбора.</returns>
    private static string BuildAddMoreTypesPrompt(IEnumerable<SelectedCatalogContext> catalogs)
    {
        var lines = catalogs.Select(catalog =>
            $"• Справочник \"{catalog.Reference.Name}\": {string.Join(", ", catalog.Types.Select(type => type.Имя))}");
        return "Уже выбрано:\n" + string.Join("\n", lines) + "\n\nДобавить еще типы из справочника?";
    }

    /// <summary>Определяет сложную иерархию по enum-настройке DOCs и ищет группу подключений.</summary>
    /// <param name="reference">Справочник, настройки которого исследуются.</param>
    /// <returns>Объект группы подключений либо null для обычной иерархии/недоступного API.</returns>
    private static object FindConnectionGroup(ReferenceInfo reference)
    {
        var description = reference?.Description;
        if (description == null || !IsComplexHierarchy(GetPropertyValue(description, "HierarchyType")))
            return null;

        foreach (string propertyName in new[] { "ConnectionGroup", "HierarchyGroup", "ComplexHierarchyGroup" })
        {
            var connectionGroup = GetPropertyValue(description, propertyName);
            if (connectionGroup != null) return connectionGroup;
        }

        try
        {
            return description.GetAllGroups()?.FirstOrDefault(group =>
                group != null && (IsTrue(GetPropertyValue(group, "IsConnectionGroup")) ||
                                  string.Equals(group.Name, "Подключение", StringComparison.CurrentCultureIgnoreCase)));
        }
        catch
        {
            return null;
        }
    }

    /// <summary>Проверяет значение enum и локализованное имя сложной иерархии.</summary>
    /// <param name="hierarchyType">Значение настройки иерархии справочника.</param>
    /// <returns>true, если настройка указывает на сложную иерархию.</returns>
    private static bool IsComplexHierarchy(object hierarchyType)
    {
        string name = Convert.ToString(hierarchyType);
        return string.Equals(name, "Complex", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(name, "Сложная", StringComparison.CurrentCultureIgnoreCase) ||
               string.Equals(name, "Сложная иерархия", StringComparison.CurrentCultureIgnoreCase);
    }

    /// <summary>Определяет справочник с простой или сложной иерархией объектов.</summary>
    /// <param name="reference">Справочник, настройки которого проверяются.</param>
    /// <returns>true, если справочник использует дерево либо явно поддерживает иерархию.</returns>
    private static bool IsHierarchicalCatalog(ReferenceInfo reference)
    {
        var description = reference?.Description;
        if (description == null) return false;

        string hierarchyName = Convert.ToString(GetPropertyValue(description, "HierarchyType"));
        if (IsComplexHierarchy(hierarchyName) ||
            new[] { "Simple", "Tree", "Дерево", "Простая", "Простая иерархия" }
            .Any(name => string.Equals(name, hierarchyName, StringComparison.OrdinalIgnoreCase) ||
                         string.Equals(name, hierarchyName, StringComparison.CurrentCultureIgnoreCase)))
            return true;

        return IsTrue(GetPropertyValue(description, "HasHierarchy"));
    }

    /// <summary>Показывает стандартный диалог DOCs выбора типов объектов справочника.</summary>
    /// <param name="refName">Имя справочника, чьи типы предлагаются.</param>
    /// <param name="title">Заголовок диалога выбора.</param>
    /// <returns>Выбранные типы или пустой массив при отмене.</returns>
    private ТипОбъекта[] SelectObjectTypes(string refName, string title)
    {
        var dlg = СоздатьДиалогВыбораТипов(refName);
        dlg.Заголовок = title;
        dlg.ВыборАбстрактныхТипов = true;
        dlg.ВыборФлажками = true;
        dlg.АвтоВыборФлажками = false;
        return dlg.Показать() ? dlg.ВыбранныеТипы : new ТипОбъекта[0];
    }

    /// <summary>Строит диалог выбора параметров с флажками всего набора и отдельных групп.</summary>
    /// <param name="reference">Справочник DOCs, содержащий выбираемые параметры.</param>
    /// <param name="title">Заголовок окна выбора.</param>
    /// <returns>Видимые параметры, отмеченные индивидуально или групповым флажком.</returns>
    private List<ParameterInfo> SelectParameters(ReferenceInfo reference, string title)
    {
        if (reference?.Description == null) return new List<ParameterInfo>();

        var groups = reference.Description.GetAllGroups()?.Where(group => group != null).ToList()
                     ?? new List<ParameterGroup>();
        var connectionGroup = FindConnectionGroup(reference);
        Guid connectionGroupGuid = ReadGuid(connectionGroup, "Guid", "GUID", "GroupGuid");
        var parameters = groups
            .Where(group => !ReferenceEquals(group, connectionGroup) &&
                            (connectionGroupGuid == Guid.Empty || group.Guid != connectionGroupGuid))
            .SelectMany(group => group.Parameters ?? Enumerable.Empty<ParameterInfo>())
            .Where(p => p != null && p.IsVisible)
            .GroupBy(p => p.Guid)
            .Select(group => group.First())
            .ToList();
        return ShowParameterSelectionDialog(reference, parameters, title, "[v] Выбрать все параметры");
    }

    /// <summary>Показывает отдельный диалог выбора только параметров группы подключения.</summary>
    /// <param name="reference">Справочник, чьи параметры подключения выбираются.</param>
    /// <param name="connectionGroup">Найденная группа подключения сложной иерархии.</param>
    /// <param name="title">Заголовок отдельного диалога.</param>
    /// <returns>Выбранные видимые параметры подключения или пустой список.</returns>
    private List<ParameterInfo> SelectConnectionParameters(ReferenceInfo reference, object connectionGroup, string title)
    {
        if (reference == null || connectionGroup == null) return new List<ParameterInfo>();

        var parameters = GetEnumerableProperty(connectionGroup, "Parameters")
            .OfType<ParameterInfo>()
            .Where(parameter => parameter.IsVisible)
            .GroupBy(parameter => parameter.Guid)
            .Select(group => group.First())
            .ToList();
        return ShowParameterSelectionDialog(reference, parameters, title, "[v] Выбрать все параметры подключения");
    }

    /// <summary>Строит единый диалог с мастер-флажком, флажками групп и отдельными параметрами.</summary>
    /// <param name="reference">Справочник для порядка групп; может отсутствовать для изолированной группы.</param>
    /// <param name="parameters">Только параметры, разрешённые для текущего диалога.</param>
    /// <param name="title">Заголовок диалога выбора.</param>
    /// <param name="selectAllKey">Подпись мастер-флажка, соответствующая типу параметров.</param>
    /// <returns>Отмеченные параметры без повторов по GUID.</returns>
    private List<ParameterInfo> ShowParameterSelectionDialog(
        ReferenceInfo reference,
        List<ParameterInfo> parameters,
        string title,
        string selectAllKey)
    {
        if (parameters == null || parameters.Count == 0) return new List<ParameterInfo>();

        var dlg = СоздатьДиалогВвода(title);
        dlg.Высота = 700;
        dlg.Ширина = 620;
        dlg.ОтобразитьПолосыПрокрутки(true, false);

        dlg.ДобавитьФлаг(selectAllKey, false);

        var parameterGroups = parameters
            .GroupBy(parameter => parameter.Group?.Guid ?? Guid.Empty)
            .Select(group => new
            {
                Group = group.First().Group,
                Name = group.First().Group?.Name ?? "Общие параметры",
                Parameters = group.OrderBy(parameter => parameter.Name, StringComparer.CurrentCultureIgnoreCase).ToList()
            })
            .ToList();
        Guid? primaryGroupGuid = reference == null
            ? (Guid?)null
            : GetPrimaryGroupGuid(reference, parameterGroups.Select(item => item.Group));
        var orderedGroups = parameterGroups
            .OrderByDescending(item => primaryGroupGuid.HasValue && item.Group?.Guid == primaryGroupGuid.Value)
            .ThenBy(item => item.Name, StringComparer.CurrentCultureIgnoreCase)
            .ToList();

        var parameterKeys = new Dictionary<Guid, string>();
        var groupKeys = new Dictionary<Guid, string>();
        var usedKeys = new HashSet<string>(StringComparer.Ordinal);

        foreach (var group in orderedGroups)
        {
            dlg.ДобавитьГруппу(group.Name);
            string groupKey = MakeUniqueDialogKey($"[Группа] {group.Name}", usedKeys);
            groupKeys[group.Group?.Guid ?? Guid.Empty] = groupKey;
            dlg.ДобавитьФлаг(groupKey, false);

            foreach (var parameter in group.Parameters)
            {
                string key = parameter.Name ?? "(параметр без имени)";
                string comment = GetObjectComment(parameter);
                if (!string.IsNullOrWhiteSpace(comment)) key += $" [{comment}]";
                key = MakeUniqueDialogKey(key, usedKeys);
                parameterKeys[parameter.Guid] = key;
                dlg.ДобавитьФлаг(key, false);
            }
        }

        var selected = new List<ParameterInfo>();
        if (dlg.Показать())
        {
            bool selectAll = GetDialogFlag(dlg, selectAllKey);
            foreach (var group in orderedGroups)
            {
                bool selectGroup = GetDialogFlag(dlg, groupKeys[group.Group?.Guid ?? Guid.Empty]);
                foreach (var parameter in group.Parameters)
                {
                    string key = parameterKeys[parameter.Guid];
                    if (selectAll || selectGroup || GetDialogFlag(dlg, key))
                        selected.Add(parameter);
                }
            }
        }
        return selected;
    }

    /// <summary>Определяет главную группу, чтобы показать её первой в списке параметров.</summary>
    /// <param name="reference">Справочник-владелец параметров.</param>
    /// <param name="groups">Фактически присутствующие в диалоге группы.</param>
    /// <returns>GUID основной группы либо null, если список групп пуст.</returns>
    private static Guid? GetPrimaryGroupGuid(ReferenceInfo reference, IEnumerable<ParameterGroup> groups)
    {
        var availableGroups = groups.Where(group => group != null).ToList();
        if (availableGroups.Count == 0) return null;

        object mainGroup = GetPropertyValue(reference?.Description, "MainGroup");
        object mainGroupGuidValue = GetPropertyValue(mainGroup, "Guid");
        Guid? mainGroupGuid = mainGroupGuidValue is Guid guid ? guid : (Guid?)null;
        ParameterGroup primary = availableGroups.FirstOrDefault(group =>
            (mainGroupGuid.HasValue && group.Guid == mainGroupGuid.Value) ||
            string.Equals(group.Name, reference?.Name, StringComparison.CurrentCultureIgnoreCase) ||
            IsTrue(GetPropertyValue(group, "IsMain")));

        return primary?.Guid ?? availableGroups[0].Guid;
    }

    /// <summary>Преобразует значение API в bool без распространения ошибок преобразования.</summary>
    /// <param name="value">Значение флага группы.</param>
    /// <returns>true, если значение явно истинно.</returns>
    private static bool IsTrue(object value)
    {
        try { return value != null && Convert.ToBoolean(value); }
        catch { return false; }
    }

    /// <summary>Разрешает совпадение подписей полей, сохраняя уникальные ключи InputDialog.</summary>
    /// <param name="label">Видимая подпись поля.</param>
    /// <param name="usedKeys">Набор ключей, уже добавленных в диалог.</param>
    /// <returns>Уникальная подпись/ключ для добавления поля.</returns>
    private static string MakeUniqueDialogKey(string label, HashSet<string> usedKeys)
    {
        string key = label;
        int duplicateIndex = 2;
        while (!usedKeys.Add(key))
            key = $"{label} ({duplicateIndex++})";
        return key;
    }

    /// <summary>Получает комментарий параметра или связи, учитывая версии API с Description.</summary>
    /// <param name="source">Параметр или группа связи DOCs.</param>
    /// <returns>Комментарий либо пустая строка.</returns>
    private static string GetObjectComment(object source)
    {
        return Convert.ToString(GetPropertyValue(source, "Comment") ?? GetPropertyValue(source, "Description"));
    }

    /// <summary>Считывает checkbox по имени поля, учитывая отмену или недоступность значения.</summary>
    /// <param name="dialog">InputDialog, показанный пользователю.</param>
    /// <param name="key">Уникальное имя поля-флажка.</param>
    /// <returns>Состояние флажка; при ошибке возвращается false.</returns>
    private static bool GetDialogFlag(dynamic dialog, string key)
    {
        try { return Convert.ToBoolean(dialog[key] ?? false); }
        catch { return false; }
    }

    /// <summary>Находит все направленные связи между выбранными каталогами.</summary>
    /// <param name="catalogs">Каталоги и типы, выбранные пользователем для экспорта.</param>
    /// <returns>Выбранные связи с зафиксированными master/slave-контекстами.</returns>
    private List<SelectedRelation> SelectRelations(List<SelectedCatalogContext> catalogs)
    {
        var selectedReferenceGuids = catalogs.Select(item => item.Reference.Guid).ToHashSet();
        var itemsByCatalog = new List<CatalogRelationItem>();

        // Опрос каждого справочника отдельно нужен для получения локального link.Name.
        foreach (var currentCatalog in catalogs)
        {
            var owners = new[] { currentCatalog.Reference.Description, currentCatalog.ConnectionGroup }
                .Where(owner => owner != null)
                .Distinct()
                .ToList();

            foreach (var owner in owners)
            {
                bool isConnection = currentCatalog.ConnectionGroup != null && ReferenceEquals(owner, currentCatalog.ConnectionGroup);
                foreach (var link in GetLinksFromOwner(owner))
                {
                    var targetRef = GetSlaveReference(link);
                    if (targetRef == null || !selectedReferenceGuids.Contains(targetRef.Guid) || targetRef.Guid == currentCatalog.Reference.Guid)
                        continue;

                    var targetCatalog = catalogs.FirstOrDefault(item => item.Reference.Guid == targetRef.Guid);
                    if (targetCatalog == null) continue;

                    itemsByCatalog.Add(new CatalogRelationItem
                    {
                        Link = link,
                        SourceCatalog = currentCatalog,
                        TargetCatalog = targetCatalog,
                        IsConnectionRelation = isConnection
                    });
                }
            }
        }

        if (itemsByCatalog.Count == 0)
        {
            Сообщение("Информация", "Связи между выбранными справочниками не найдены.");
            return new List<SelectedRelation>();
        }

        var dlg = СоздатьДиалогВвода("Шаг 4: Выберите связи между справочниками");
        dlg.ОтобразитьПолосыПрокрутки(true, false);
        var usedKeys = new HashSet<string>(StringComparer.Ordinal);
        var itemKeys = new Dictionary<CatalogRelationItem, string>();

        foreach (var group in itemsByCatalog.GroupBy(item => item.SourceCatalog.Reference.Name)
                     .OrderBy(group => group.Key, StringComparer.CurrentCultureIgnoreCase))
        {
            dlg.ДобавитьГруппу(group.Key);

            // В одном справочнике один GUID должен давать только один флажок.
            var uniqueInGroup = group
                .GroupBy(item => item.Link.Guid)
                .Select(linkGroup => linkGroup.First());

            foreach (var item in uniqueInGroup.OrderBy(item => item.Link.Name, StringComparer.CurrentCultureIgnoreCase))
            {
                string sourceName = item.IsConnectionRelation
                    ? $"{item.SourceCatalog.Reference.Name} (Подключение)"
                    : item.SourceCatalog.Reference.Name;

                // DOCs возвращает настроенное имя связи со стороны опрашиваемого справочника.
                string label = $"{item.Link.Name} [{sourceName} -> {item.TargetCatalog.Reference.Name}]";
                string comment = GetObjectComment(item.Link);
                if (!string.IsNullOrWhiteSpace(comment))
                    label += $" [{comment}]";
                label += $" {{{item.Link.Guid}}}";

                string key = MakeUniqueDialogKey(label, usedKeys);
                item.DialogKey = key;
                itemKeys[item] = key;
                dlg.ДобавитьФлаг(key, false);
            }
        }

        var chosen = new List<SelectedRelation>();
        if (dlg.Показать())
        {
            var selectedItems = itemsByCatalog
                .Where(item => itemKeys.TryGetValue(item, out var key) && GetDialogFlag(dlg, key))
                .ToList();
            var handledGuids = new HashSet<Guid>();

            foreach (var item in selectedItems)
            {
                if (!handledGuids.Add(item.Link.Guid)) continue;

                chosen.Add(new SelectedRelation
                {
                    Link = item.Link,
                    Master = item.SourceCatalog,
                    Slave = item.TargetCatalog,
                    IsConnectionRelation = item.IsConnectionRelation
                });
            }
        }
        return chosen;
    }

    /// <summary>Преобразует тип параметра DOCs в стандартный тип UML и тип атрибута EA.</summary>
    /// <param name="pType">Тип значения параметра в модели DOCs.</param>
    /// <returns>Пара ссылок для UML type и Enterprise Architect attribute type.</returns>
    private (string typeRef, string eaType) MapType(ParameterType pType)
    {
        if (pType == null) return ("EAC__string", "string");
        if (pType.IsBoolean) return ("EAC__boolean", "boolean");
        if (pType.IsDateTime) return ("EAC__date", "date");
        if (pType.IsFloat || pType.IsMoney || pType.IsNumber) return ("EAC__double", "double");
        if (pType.IsInt) return ("EAC__int", "int");
        return ("EAC__string", "string");
    }

    /// <summary>Строит XMI для всех выбранных каталогов, типов, параметров и направленных связей.</summary>
    /// <param name="selectedCatalogs">Подготовленные контексты всех справочников, выбранных пользователем.</param>
    /// <param name="links">Выбранные связи с master/slave-парами каталогов.</param>
    private void GenerateXmiFile(List<SelectedCatalogContext> selectedCatalogs, List<SelectedRelation> links)
    {
        var saveDlg = СоздатьДиалогСохраненияФайла();
        saveDlg.Заголовок = "Сохранение файла XMI";
        saveDlg.Фильтр = "Файлы XML (*.xml)|*.xml";
        saveDlg.РасширениеПоУмолчанию = "xml";
        if (!saveDlg.Показать()) return;

        var catalogByReferenceGuid = selectedCatalogs.ToDictionary(context => context.Reference.Guid, context => BuildCatalog(context.Reference));
        var catalogs = selectedCatalogs.Select(context => catalogByReferenceGuid[context.Reference.Guid]).ToList();
        var classesByReferenceGuid = selectedCatalogs.ToDictionary(
            context => context.Reference.Guid,
            context => BuildClasses(context, catalogByReferenceGuid[context.Reference.Guid].Guid));
        var allClasses = classesByReferenceGuid.Values.SelectMany(classes => classes).ToList();
        var catalogTypeAssocs = allClasses.Select(cls =>
        {
            var catalog = catalogs.First(item => item.Guid == cls.CatalogGuid);
            return new CatalogTypeAssocModel
            {
                Guid = FormatEaId(CreateDeterministicGuid($"Contains_{catalog.Guid}_{cls.Guid}")),
                CatalogGuid = catalog.Guid,
                CatalogName = catalog.Name,
                TypeGuid = cls.Guid,
                TypeName = cls.Name
            };
        }).ToList();
        foreach (var catalog in catalogs)
            catalog.Events.AddRange(BuildEvents(catalog, allClasses));
        var allAssocs = new List<AssocModel>();

        foreach (var link in links)
        {
            if (link?.Link == null || link.Master?.Reference == null || link.Slave?.Reference == null)
                continue;

            // Источник должен содержать связь в собственных группах параметров.
            var masterClasses = classesByReferenceGuid[link.Master.Reference.Guid]
                .Where(classModel => link.IsConnectionRelation
                    ? classModel.IsConnection
                    : !classModel.IsConnection && IsLinkAttachedToMasterClass(classModel, link.Link))
                .ToList();

            // Получатель проверяется по ограничениям самой связи, а не по SlaveGroup.
            var slaveClasses = classesByReferenceGuid[link.Slave.Reference.Guid]
                .Where(classModel => !classModel.IsConnection && IsLinkAllowedForSlaveClass(classModel, link.Link))
                .ToList();

            foreach (var masterClass in masterClasses)
            {
                foreach (var slaveClass in slaveClasses)
                {
                    var multiplicity = GetAssociationMultiplicity(link.Link);
                    string sourceRole = link.Link.Name;
                    string targetRole = GetRelationNameForCatalog(link.Slave, link.Link.Guid);
                    Guid assocRawGuid = CreateDeterministicGuid(
                        $"Assoc_{link.Link.Guid}_{masterClass.Guid}_{slaveClass.Guid}");
                    var assoc = new AssocModel
                    {
                        AssocGuid = FormatEaId(assocRawGuid),
                        SrcPropGuid = FormatEaId(assocRawGuid, "EAID_src_"),
                        DstPropGuid = FormatEaId(assocRawGuid, "EAID_dst_"),
                        // Если обе роли заданы, подписи концов полностью описывают связь.
                        Name = !string.IsNullOrWhiteSpace(sourceRole) && !string.IsNullOrWhiteSpace(targetRole)
                            ? null
                            : link.Link.Name,
                        SourceClassGuid = masterClass.Guid, SourceClassName = masterClass.Name,
                        TargetClassGuid = slaveClass.Guid, TargetClassName = slaveClass.Name,
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

        AddComplexHierarchyAssociations(selectedCatalogs, classesByReferenceGuid, allAssocs);
        AddIntraCatalogHierarchyAssociations(selectedCatalogs, classesByReferenceGuid, allAssocs);

        string combinedKeys = string.Join("_", selectedCatalogs
            .OrderBy(context => context.Reference.Guid)
            .Select(context => context.Reference.Guid));
        string packageGuid = FormatEaId(
            CreateDeterministicGuid("ModelPackage_" + combinedKeys),
            prefix: "EAPK_");
        string exportTimestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        var pkg = new XElement("packagedElement",
            new XAttribute(xmi + "type", "uml:Package"),
            new XAttribute(xmi + "id", packageGuid),
            new XAttribute("name", $"Справочники T-FLEX DOCs {exportTimestamp}"),
            new XAttribute("visibility", "public")
        );

        foreach (var catalog in catalogs)
        {
            var catalogElem = new XElement("packagedElement",
                new XAttribute(xmi + "type", "uml:Component"),
                new XAttribute(xmi + "id", catalog.Guid),
                new XAttribute("name", catalog.Name),
                new XAttribute("visibility", "public")
            );
            AddOwnedComment(catalogElem, catalog.Comment);

            foreach (var cls in allClasses.Where(c => c.CatalogGuid == catalog.Guid))
            {
                var clsElem = new XElement("packagedElement",
                    new XAttribute(xmi + "type", "uml:Class"),
                    new XAttribute(xmi + "id", cls.Guid),
                    new XAttribute("name", cls.Name),
                    new XAttribute("visibility", "public")
                );
                AddOwnedComment(clsElem, cls.Comment);

                foreach (var p in cls.Parameters)
                    clsElem.Add(CreateProperty(p.Guid, p.Name, p.TypeRef));

                catalogElem.Add(clsElem);
            }

            foreach (var eventModel in catalog.Events)
            {
                var signal = CreateSignal(eventModel);
                AddOwnedComment(signal, eventModel.Comment);
                catalogElem.Add(signal);
            }

            pkg.Add(catalogElem);
        }

        foreach (var a in allAssocs)
        {
            pkg.Add(new XElement("packagedElement",
                new XAttribute(xmi + "type", "uml:Association"),
                new XAttribute(xmi + "id", a.AssocGuid),
                string.IsNullOrEmpty(a.Name) ? null : new XAttribute("name", a.Name),
                new XAttribute("visibility", "public"),
                new XElement("memberEnd", new XAttribute(xmi + "idref", a.DstPropGuid)),
                new XElement("memberEnd", new XAttribute(xmi + "idref", a.SrcPropGuid)),
                CreateOwnedEnd(a.DstPropGuid, a.AssocGuid, a.TargetClassGuid, a.TargetRole, a.TargetLower, a.TargetUpper),
                CreateOwnedEnd(a.SrcPropGuid, a.AssocGuid, a.SourceClassGuid, a.SourceRole, a.SourceLower, a.SourceUpper)
            ));
        }

        foreach (var association in catalogTypeAssocs)
            pkg.Add(CreateCatalogTypeAssociation(association));

        var ext = BuildEaExtension(packageGuid, catalogs, allClasses, allAssocs, catalogTypeAssocs);
        var stereotypeApplications = BuildStereotypeApplications(catalogs, allClasses);

        var doc = new XDocument(
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
                    pkg,
                    stereotypeApplications
                ),
                ext
            )
        );

        var writerSettings = new XmlWriterSettings { Encoding = new UTF8Encoding(false), Indent = true };
        using (var writer = XmlWriter.Create(saveDlg.ИмяФайла, writerSettings))
            doc.Save(writer);
        Сообщение("Завершение", $"Схема успешно сохранена:\n{saveDlg.ИмяФайла}");
    }

    /// <summary>Создаёт XMI UML Property для атрибута класса и задаёт его тип.</summary>
    /// <param name="id">Глобальный идентификатор свойства.</param>
    /// <param name="name">Имя параметра.</param>
    /// <param name="typeRef">XMI idref стандартного типа.</param>
    /// <returns>XML-элемент ownedAttribute.</returns>
    private XElement CreateProperty(string id, string name, string typeRef)
    {
        return new XElement("ownedAttribute",
            new XAttribute(xmi + "type", "uml:Property"), new XAttribute(xmi + "id", id),
            new XAttribute("name", name), new XAttribute("visibility", "public"),
            new XElement("lowerValue", new XAttribute(xmi + "type", "uml:LiteralInteger"), new XAttribute("value", "1")),
            new XElement("upperValue", new XAttribute(xmi + "type", "uml:LiteralInteger"), new XAttribute("value", "1")),
            new XElement("type", new XAttribute(xmi + "idref", typeRef))
        );
    }

    /// <summary>Создаёт UML Association между компонентом справочника и его типом.</summary>
    /// <param name="association">Идентификаторы и концы связи справочник—тип.</param>
    /// <returns>Пакетированный UML-элемент ассоциации.</returns>
    private XElement CreateCatalogTypeAssociation(CatalogTypeAssocModel association)
    {
        Guid containsGuid = CreateDeterministicGuid($"Contains_{association.CatalogGuid}_{association.TypeGuid}");
        association.Guid = FormatEaId(containsGuid);
        string catalogEndId = FormatEaId(containsGuid, "EAID_src_");
        string typeEndId = FormatEaId(containsGuid, "EAID_dst_");
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

    /// <summary>Извлекает документацию и настройки справочника для UML и EA extension.</summary>
    /// <param name="reference">Справочник T-FLEX DOCs.</param>
    /// <returns>Промежуточная модель справочника.</returns>
    private CatalogModel BuildCatalog(ReferenceInfo reference)
    {
        var catalog = new CatalogModel
        {
            Guid = FormatEaId(reference.Guid != Guid.Empty
                ? reference.Guid
                : CreateDeterministicGuid($"Catalog_{reference.Name}")),
            Name = reference.Name,
            Comment = reference.Description?.Comment
        };

        AddStructureSettings(catalog.Settings, reference.Description);
        return catalog;
    }

    /// <summary>Читает набор пользовательских настроек структуры в пары подпись—значение.</summary>
    /// <param name="settings">Коллекция настроек, предназначенная для tagged values.</param>
    /// <param name="group">Главная группа справочника DOCs с настройками структуры.</param>
    private static void AddStructureSettings(List<KeyValuePair<string, string>> settings, object group)
    {
        var structureSettings = new[]
        {
            // === Вкладка «Основные» ===
            Tuple.Create("Имя таблицы в БД", "TableName"), // Исправлено с: "Имя таблицы"
            Tuple.Create("Видимость справочника в интерфейсе", "Visibility"), // Исправлено с: "Видимость"

            // === Вкладка «Дополнительные» (Левый столбец чекбоксов) ===
            Tuple.Create("Поддержка истории изменений", "SupportsDataChangeLog"), // Исправлено с: "Журнал изменений данных"
            Tuple.Create("Поддержка работы с прототипами", "SupportsPrototypes"), // Исправлено с: "Прототипы"
            Tuple.Create("Поддержка механизма подписей", "SupportsSignature"), // Исправлено с: "Подписи"
            Tuple.Create("Поддержка параметра \"Владелец\"", "SupportsOwner"), // Исправлено с: "Владелец"
            Tuple.Create("Допускается изменение типа объектов", "CanChangeClass"), // Исправлено с: "Можно менять тип"
            Tuple.Create("Поддержка экземпляров изделий", "SupportsObjectsInstances"), // Исправлено с: "Экземпляры объектов"
            Tuple.Create("Поддержка дат действия", "SupportsActivityDates"), // Исправлено с: "Даты действия"
            Tuple.Create("Поддержка структур изделия", "SupportsStructureTypes"), // Исправлено с: "Типы структур"
            Tuple.Create("Поддержка механизма контекстных замен", "SupportsSubstitutesInContext"), // Исправлено с: "Замены в контексте"

            // === Вкладка «Дополнительные» (Правый столбец чекбоксов) ===
            Tuple.Create("Поддержка корзины", "SupportsRecycleBin"), // Исправлено с: "Корзина"
            Tuple.Create("Поддержка сортировки", "SupportsOrder"), // Исправлено с: "Порядок объектов"
            // Tuple.Create("Поддержка журнала", "..."), // Примечание: в API отсутствует отдельное свойство под чекбокс «Поддержка журнала»
            Tuple.Create("Поддержка ревизий", "SupportsRevisions"), // Исправлено с: "Ревизии"
            Tuple.Create("Поддержка дополнительных параметров", "SupportsExtendedParameters"), // Исправлено с: "Расширенные параметры"
            Tuple.Create("Поддержка применимости", "SupportsApplicability"), // Исправлено с: "Применимость"
            Tuple.Create("Поддержка контекстов проектирования", "SupportsDesignContexts"), // Исправлено с: "Контексты проектирования"
            Tuple.Create("Поддержка конфигурирования", "SupportsConfigurationSettings"), // Исправлено с: "Конфигурирование"
            // Tuple.Create("Поддержка альтернативных представлений", "..."), // Примечание: чекбокс в UI есть, но в исходном списке API отсутствовал

            // === Вкладка «Дополнительные» (Выпадающие списки и группы) ===
            Tuple.Create("Иерархия объектов", "HierarchyType"), // Исправлено с: "Тип иерархии"
            Tuple.Create("Поддержка управления доступом на объекты", "SupportsMandatoryAccess"), // Исправлено с: "Обязательный доступ"
            Tuple.Create("Поддержка стадий", "SupportsStages"), // Исправлено с: "Этапы"
            Tuple.Create("Конфигуратор", "ConfiguratorGuid"), // Исправлено с: "GUID конфигуратора"

            // === Вкладка «Типы подписей» ===
            Tuple.Create("Использовать все типы подписей", "UseAllSignatureTypes"), // Исправлено с: "Все типы подписей"

            // =========================================================================
            // Свойства, которые отсутствуют в пользовательском интерфейсе (закомментированы):
            // =========================================================================
            // Tuple.Create("Тип структуры", "Type"), // В UI отсутствует
            // Tuple.Create("Статус активности", "ActivityStatus"), // В UI отсутствует
            // Tuple.Create("Рабочий стол", "SupportsDesktop"), // В UI отсутствует
            // Tuple.Create("Номенклатура", "SupportsNomenclature"), // В UI отсутствует
            // Tuple.Create("Иерархия", "HasHierarchy"), // Дублирует "Иерархия объектов" (HierarchyType)
            // Tuple.Create("Классы", "SupportsClasses"), // В UI отсутствует
            // Tuple.Create("Системные объекты", "SupportsSystemObjects"), // В UI отсутствует
            // Tuple.Create("Приватные папки", "SupportsPrivateFolders"), // В UI отсутствует
            // Tuple.Create("Шифрование", "SupportsEncryption"), // В UI отсутствует
            // Tuple.Create("Множественные вложения", "SupportMultiAttachment"), // В UI отсутствует
            // Tuple.Create("Можно редактировать", "CanEdit"), // В UI отсутствует
            // Tuple.Create("Можно удалять", "CanDelete"), // В UI отсутствует
            // Tuple.Create("Имя таблицы сгенерировано", "TableNameGenerated") // Внутреннее свойство, в UI не выводится
        };
        
        foreach (var setting in structureSettings)
        {
            object value = group == null ? "недоступно" : GetStructureSettingValue(group, setting.Item2);
            settings.Add(new KeyValuePair<string, string>(setting.Item1, Convert.ToString(value)));
        }
    }

    /// <summary>Безопасно читает одно свойство структуры через reflection для совместимости API.</summary>
    /// <param name="source">Объект DOCs, содержащий настройку.</param>
    /// <param name="propertyName">Имя свойства API.</param>
    /// <returns>Значение свойства, либо текст состояния ошибки чтения.</returns>
    private static object GetStructureSettingValue(object source, string propertyName)
    {
        if (source == null) return "недоступно";
        try
        {
            var property = source.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);
            return property?.GetValue(source, null) ?? "недоступно";
        }
        catch (System.Reflection.TargetInvocationException)
        {
            return "ошибка чтения";
        }
        catch (InvalidOperationException)
        {
            return "ошибка чтения";
        }
    }

    /// <summary>Собирает события справочника, его выбранных типов и их групп параметров.</summary>
    /// <param name="catalog">Экспортируемый справочник.</param>
    /// <param name="classes">Классы типов, выбранных для этого справочника.</param>
    /// <returns>События без повторов по GUID или имени.</returns>
    private List<EventModel> BuildEvents(CatalogModel catalog, List<ClassModel> classes)
    {
        var events = new List<EventModel>();
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var reference = Context.Connection.ReferenceCatalog.Find(catalog.Name);
        AddEventsFromOwner(events, seen, catalog, reference?.Description);

        foreach (var sourceClass in classes.Where(cls => cls.CatalogGuid == catalog.Guid).Select(cls => cls.SourceClass).Where(item => item != null))
        {
            AddEventsFromOwner(events, seen, catalog, sourceClass);
            var groups = GetEnumerableProperty(sourceClass, "ParameterGroups");
            foreach (var group in groups)
                AddEventsFromOwner(events, seen, catalog, group);
        }

        return events;
    }

    /// <summary>Добавляет события и пользовательские команды одного объекта DOCs.</summary>
    /// <param name="events">Накопитель событий экспорта.</param>
    /// <param name="seen">Ключи уже обработанных событий для устранения повторов.</param>
    /// <param name="catalog">Справочник, содержащий событие.</param>
    /// <param name="owner">Группа или тип DOCs с коллекцией событий.</param>
    private static void AddEventsFromOwner(List<EventModel> events, HashSet<string> seen, CatalogModel catalog, object owner)
    {
        if (owner == null) return;
        foreach (string collectionName in new[] { "Events", "UserEvents" })
        {
            foreach (var eventObject in GetEnumerableProperty(owner, collectionName))
            {
                string name = Convert.ToString(GetPropertyValue(eventObject, "Name"));
                object button = GetPropertyValue(eventObject, "Button");
                if (string.IsNullOrWhiteSpace(name))
                    name = Convert.ToString(GetPropertyValue(button, "MenuCaption")) ?? Convert.ToString(GetPropertyValue(button, "Text"));
                if (string.IsNullOrWhiteSpace(name)) continue;

                string identity = Convert.ToString(GetPropertyValue(eventObject, "Guid"));
                if (string.IsNullOrWhiteSpace(identity)) identity = name;
                if (!seen.Add(identity)) continue;

                string comment = Convert.ToString(GetPropertyValue(eventObject, "Comment"))
                    ?? Convert.ToString(GetPropertyValue(eventObject, "Description"))
                    ?? Convert.ToString(GetPropertyValue(button, "Hint"));
                Guid eventGuid = ReadGuid(eventObject, "Guid", "GUID", "EventGuid");
                events.Add(new EventModel
                {
                    Guid = FormatEaId(eventGuid != Guid.Empty
                        ? eventGuid
                        : CreateDeterministicGuid($"Event_{catalog.Guid}_{name}")),
                    Name = name,
                    Comment = comment,
                    CatalogGuid = catalog.Guid
                });
            }
        }
    }

    /// <summary>Получает коллекцию свойства API через reflection, не завершая экспорт при ошибке API.</summary>
    /// <param name="source">Объект, у которого читается коллекция.</param>
    /// <param name="propertyName">Имя свойства-коллекции.</param>
    /// <returns>Непустые элементы коллекции либо пустая последовательность.</returns>
    private static IEnumerable<object> GetEnumerableProperty(object source, string propertyName)
    {
        var value = GetPropertyValue(source, propertyName) as System.Collections.IEnumerable;
        if (value == null) yield break;
        foreach (var item in value)
            if (item != null) yield return item;
    }

    /// <summary>Получает link groups владельца через свойство Links или метод GetLinks без обязательной сигнатуры.</summary>
    /// <param name="owner">Группа или описание справочника DOCs.</param>
    /// <returns>Найденные link groups; при недоступности API возвращается пустая последовательность.</returns>
    private static IEnumerable<ParameterGroup> GetLinksFromOwner(object owner)
    {
        if (owner == null) yield break;

        var seenGuids = new HashSet<Guid>();
        foreach (var link in GetEnumerableProperty(owner, "Links").OfType<ParameterGroup>())
            if (seenGuids.Add(link.Guid))
                yield return link;

        object methodResult = null;
        bool methodFailed = false;
        try
        {
            var method = owner.GetType().GetMethod("GetLinks", BindingFlags.Instance | BindingFlags.Public, null, Type.EmptyTypes, null);
            methodResult = method?.Invoke(owner, null);
        }
        catch
        {
            methodFailed = true;
        }
        if (methodFailed) yield break;

        if (methodResult is System.Collections.IEnumerable links)
        {
            foreach (var link in links)
                if (link is ParameterGroup parameterGroup && seenGuids.Add(parameterGroup.Guid))
                    yield return parameterGroup;
        }
    }

    /// <summary>Безопасно получает справочник Slave через необязательную группу конца связи.</summary>
    /// <param name="link">Группа связи, у которой может отсутствовать SlaveGroup.</param>
    /// <returns>Справочник Slave или null, если группа/свойство/справочник недоступны.</returns>
    private static ReferenceInfo GetSlaveReference(ParameterGroup link)
    {
        object slaveGroup = GetPropertyValue(link, "SlaveGroup");
        return GetPropertyValue(slaveGroup, "ReferenceInfo") as ReferenceInfo;
    }

    /// <summary>Получает имя той же связи в контексте второго справочника.</summary>
    /// <param name="catalog">Справочник, у которого запрашивается имя роли.</param>
    /// <param name="linkGuid">GUID связи, найденной со стороны первого справочника.</param>
    /// <returns>Контекстное имя из link.Name либо пустая строка, если связь не найдена.</returns>
    private static string GetRelationNameForCatalog(SelectedCatalogContext catalog, Guid linkGuid)
    {
        if (catalog?.Reference == null) return string.Empty;

        var owners = new[] { catalog.Reference.Description, catalog.ConnectionGroup }
            .Where(owner => owner != null)
            .Distinct();
        foreach (var owner in owners)
        {
            var matchingLink = GetLinksFromOwner(owner).FirstOrDefault(link => link.Guid == linkGuid);
            if (matchingLink != null) return matchingLink.Name ?? string.Empty;
        }

        return string.Empty;
    }

    /// <summary>Безопасно читает публичное свойство API через reflection.</summary>
    /// <param name="source">Исходный объект API.</param>
    /// <param name="propertyName">Имя свойства.</param>
    /// <returns>Значение свойства или null, если свойство отсутствует либо чтение не удалось.</returns>
    private static object GetPropertyValue(object source, string propertyName)
    {
        if (source == null) return null;
        try { return source.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public)?.GetValue(source, null); }
        catch { return null; }
    }

    /// <summary>Создаёт packagedElement UML Signal для события DOCs.</summary>
    /// <param name="eventModel">Модель события с именем и GUID.</param>
    /// <returns>Элемент сигнала XMI 2.1.</returns>
    private XElement CreateSignal(EventModel eventModel)
    {
        return new XElement("packagedElement",
            new XAttribute(xmi + "type", "uml:Signal"),
            new XAttribute(xmi + "id", eventModel.Guid),
            new XAttribute("name", eventModel.Name),
            new XAttribute("visibility", "public"));
    }

    /// <summary>Создаёт UML ownedEnd для одного конца ассоциации с его ролью и кратностью.</summary>
    /// <param name="id">Идентификатор конца ассоциации.</param>
    /// <param name="assocId">Идентификатор ассоциации-владельца.</param>
    /// <param name="classGuid">GUID класса, являющегося типом конца.</param>
    /// <param name="role">Имя роли на этом конце.</param>
    /// <param name="lower">Нижняя граница кратности.</param>
    /// <param name="upper">Верхняя граница кратности или -1/* для неограниченной.</param>
    /// <returns>XML-элемент ownedEnd.</returns>
    private XElement CreateOwnedEnd(string id, string assocId, string classGuid, string role, string lower, string upper)
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
                new XAttribute("value", unlimited ? "-1" : upper))
        );
    }

    /// <summary>Форматирует нижнюю и верхнюю границы кратности для EA-коннектора.</summary>
    /// <param name="lower">Нижняя граница кратности.</param>
    /// <param name="upper">Верхняя граница или -1/* для неограниченной кратности.</param>
    /// <returns>Значение кратности в формате EA.</returns>
    private static string FormatMultiplicity(string lower, string upper)
    {
        string normalizedUpper = upper == "-1" ? "*" : upper;
        if (lower == normalizedUpper) return lower;
        return $"{lower}..{normalizedUpper}";
    }

    /// <summary>Переводит тип связи DOCs в кратности концов UML-ассоциации.</summary>
    /// <param name="link">Группа связи DOCs между master и slave каталогами.</param>
    /// <returns>Нижние и верхние границы для master- и slave-концов.</returns>
    private static (string sourceLower, string sourceUpper, string targetLower, string targetUpper) GetAssociationMultiplicity(ParameterGroup link)
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

    /// <summary>Формирует EA extension с каталогами, типами, атрибутами и коннекторами.</summary>
    /// <param name="packageGuid">XMI ID корневого UML-пакета.</param>
    /// <param name="catalogs">Экспортируемые справочники.</param>
    /// <param name="classes">Экспортируемые типы и их параметры.</param>
    /// <param name="assocs">Направленные связи между классами выбранных справочников.</param>
    /// <param name="catalogTypeAssocs">Связи типов с их справочниками.</param>
    /// <returns>Extension-узел Enterprise Architect для файла XMI.</returns>
    private XElement BuildEaExtension(string packageGuid, List<CatalogModel> catalogs, List<ClassModel> classes,
                                      List<AssocModel> assocs, List<CatalogTypeAssocModel> catalogTypeAssocs)
    {
        var ext = new XElement(xmi + "Extension", new XAttribute("extender", "Enterprise Architect"), new XAttribute("extenderID", "6.5"));
        var elems = new XElement("elements");
        string created = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        int localId = 0;

        foreach (var catalog in catalogs)
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
            if (!string.IsNullOrWhiteSpace(catalog.Comment))
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
            element.Add(new XElement("style", new XAttribute("appearance", "BackColor=-1;BorderColor=-1;BorderWidth=-1;FontColor=-1;VSwimLanes=1;HSwimLanes=1;BorderStyle=0;")));
            AddTags(element, catalog.Guid, catalog.Settings);
            element.Add(new XElement("xrefs"));
            element.Add(new XElement("extendedProperties", new XAttribute("tagged", "0"), new XAttribute("package_name", catalog.Name)));
            var catalogLinks = catalogTypeAssocs.Where(item => item.CatalogGuid == catalog.Guid)
                .Select(item => new XElement("Association",
                    new XAttribute(xmi + "id", item.Guid),
                    new XAttribute("start", item.CatalogGuid),
                    new XAttribute("end", item.TypeGuid)));
            element.Add(new XElement("links", catalogLinks));
            elems.Add(element);
        }

        foreach (var cls in classes)
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
            if (!string.IsNullOrWhiteSpace(cls.Comment))
                properties.Add(new XAttribute("documentation", cls.Comment));

            var el = new XElement("element",
                new XAttribute(xmi + "idref", cls.Guid),
                new XAttribute(xmi + "type", "uml:Class"),
                new XAttribute("name", cls.Name),
                properties);
            el.Add(new XElement("model",
                new XAttribute("package", packageGuid),
                new XAttribute("owner", cls.CatalogGuid),
                new XAttribute("tpos", localId),
                new XAttribute("ea_localid", localId++),
                new XAttribute("ea_eleType", "element")));
            el.Add(CreateEaProjectMetadata(created));
            el.Add(new XElement("code", new XAttribute("gentype", "Java")));
            el.Add(new XElement("style", new XAttribute("appearance", "BackColor=-1;BorderColor=-1;BorderWidth=-1;FontColor=-1;VSwimLanes=1;HSwimLanes=1;BorderStyle=0;")));
            if (cls.Parameters.Count > 0)
            {
                var attrs = new XElement("attributes");
                foreach (var p in cls.Parameters)
                    attrs.Add(new XElement("attribute", new XAttribute(xmi + "idref", p.Guid), new XAttribute("name", p.Name), new XElement("properties", new XAttribute("type", p.EaType))));
                el.Add(attrs);
            }
            el.Add(new XElement("extendedProperties", new XAttribute("tagged", "0"), new XAttribute("package_name", catalogs.First(item => item.Guid == cls.CatalogGuid).Name)));
            var allClassLinks = catalogTypeAssocs
                .Where(item => item.TypeGuid == cls.Guid)
                .Select(item => new XElement("Association",
                    new XAttribute(xmi + "id", item.Guid),
                    new XAttribute("start", item.CatalogGuid),
                    new XAttribute("end", item.TypeGuid)))
                .Concat(assocs
                    .Where(association => association.SourceClassGuid == cls.Guid ||
                                          association.TargetClassGuid == cls.Guid)
                    .Select(association => new XElement("Association",
                        new XAttribute(xmi + "id", association.AssocGuid),
                        new XAttribute("start", association.SourceClassGuid),
                        new XAttribute("end", association.TargetClassGuid))));
            el.Add(new XElement("links", allClassLinks));
            el.Add(new XElement("xrefs"));
            elems.Add(el);
        }

        foreach (var catalog in catalogs)
        {
            foreach (var eventModel in catalog.Events)
            {
                var properties = new XElement("properties",
                    new XAttribute("isSpecification", "false"),
                    new XAttribute("sType", "Signal"),
                    new XAttribute("nType", "0"),
                    new XAttribute("scope", "public"),
                    new XAttribute("stereotype", "event"));
                if (!string.IsNullOrWhiteSpace(eventModel.Comment))
                    properties.Add(new XAttribute("documentation", eventModel.Comment));
                var element = new XElement("element",
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
                elems.Add(element);
            }
        }
        ext.Add(elems);

        var connectors = new XElement("connectors");
        foreach (var a in assocs)
        {
            string srcMult = FormatMultiplicity(a.SourceLower, a.SourceUpper);
            string dstMult = FormatMultiplicity(a.TargetLower, a.TargetUpper);

            var connector = new XElement("connector",
                new XAttribute(xmi + "idref", a.AssocGuid),
                string.IsNullOrEmpty(a.Name) ? null : new XAttribute("name", a.Name),
                new XElement("source",
                    new XAttribute(xmi + "idref", a.SourceClassGuid),
                    new XElement("model", new XAttribute("type", "Class"), new XAttribute("name", a.SourceClassName)),
                    string.IsNullOrEmpty(a.SourceRole) ? null : new XElement("role",
                        new XAttribute("name", a.SourceRole), new XAttribute("visibility", "Public")),
                    new XElement("type", new XAttribute("multiplicity", srcMult), new XAttribute("aggregation", "none")),
                    new XElement("modifiers", new XAttribute("isOrdered", "false"), new XAttribute("isNavigable", "true")),
                    new XElement("style", new XAttribute("value", "Owned=0;Navigable=Unspecified;"))),
                new XElement("target",
                    new XAttribute(xmi + "idref", a.TargetClassGuid),
                    new XElement("model", new XAttribute("type", "Class"), new XAttribute("name", a.TargetClassName)),
                    string.IsNullOrEmpty(a.TargetRole) ? null : new XElement("role",
                        new XAttribute("name", a.TargetRole), new XAttribute("visibility", "Public")),
                    new XElement("type", new XAttribute("multiplicity", dstMult), new XAttribute("aggregation", "none")),
                    new XElement("modifiers", new XAttribute("isOrdered", "false"), new XAttribute("isNavigable", "false")),
                    new XElement("style", new XAttribute("value", "Owned=0;Navigable=Unspecified;"))),
                new XElement("properties",
                    new XAttribute("ea_type", "Association"),
                    new XAttribute("direction", a.Direction ?? "Unspecified")),
                new XElement("labels",
                    new XAttribute("lb", srcMult),
                    string.IsNullOrEmpty(a.SourceRole) ? null : new XAttribute("lt", $"+{a.SourceRole}"),
                    new XAttribute("rb", dstMult),
                    string.IsNullOrEmpty(a.TargetRole) ? null : new XAttribute("rt", $"+{a.TargetRole}")));
            connectors.Add(connector);
        }
        foreach (var association in catalogTypeAssocs)
        {
            connectors.Add(new XElement("connector",
                new XAttribute(xmi + "idref", association.Guid),
                new XAttribute("name", "Содержит"),
                new XElement("source", new XAttribute(xmi + "idref", association.CatalogGuid)),
                new XElement("target", new XAttribute(xmi + "idref", association.TypeGuid)),
                new XElement("properties", new XAttribute("ea_type", "Association"))));
        }
        ext.Add(connectors);

        ext.Add(new XElement("profiles", CreateCustomProfile()));
        return ext;
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

    /// <summary>Добавляет UML Comment к элементу только при непустом описании.</summary>
    /// <param name="umlElement">Элемент UML-модели, к которому прикрепляется комментарий.</param>
    /// <param name="comment">Описание объекта из DOCs.</param>
    private static void AddOwnedComment(XElement umlElement, string comment)
    {
        if (!string.IsNullOrWhiteSpace(comment))
        {
            string ownerGuid = Convert.ToString(umlElement.Attribute(xmi + "id"));
            umlElement.Add(new XElement("ownedComment",
                new XAttribute(xmi + "type", "uml:Comment"),
                new XAttribute(xmi + "id", FormatEaId(
                    CreateDeterministicGuid($"Comment_{ownerGuid}"))),
                new XElement("body", comment)));
        }
    }

    /// <summary>Добавляет настройки справочника в EA-раздел tagged values.</summary>
    /// <param name="element">Extension element справочника.</param>
    /// <param name="modelElement">XMI ID компонента-справочника.</param>
    /// <param name="settings">Пары русской подписи настройки и её значения.</param>
    private static void AddTags(XElement element, string modelElement, IEnumerable<KeyValuePair<string, string>> settings)
    {
        var tags = new XElement("tags");
        int index = 0;
        foreach (var setting in settings)
        {
            tags.Add(new XElement("tag",
                new XAttribute(xmi + "id", FormatEaId(
                    CreateDeterministicGuid($"Tag_{modelElement}_{setting.Key}"))),
                new XAttribute("name", setting.Key),
                new XAttribute("value", setting.Value ?? string.Empty),
                new XAttribute("modelElement", modelElement)));
            index++;
        }
        if (index > 0) element.Add(tags);
    }

    /// <summary>Создаёт детерминированный GUID на основе стабильного строкового ключа DOCs.</summary>
    /// <param name="sourceKey">Стабильный ключ исходной сущности или производного элемента.</param>
    /// <returns>GUID, вычисленный через MD5; для пустого ключа возвращается Guid.Empty.</returns>
    private static Guid CreateDeterministicGuid(string sourceKey)
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
    private static string FormatEaId(Guid guid, string prefix = "EAID_")
    {
        return prefix + guid.ToString("N").ToUpperInvariant();
    }

    /// <summary>Создаёт применения пользовательских стереотипов и тегов профиля.</summary>
    /// <param name="catalogs">Справочники для стереотипа catalog и tagged values.</param>
    /// <param name="classes">Типы DOCs для стереотипа type.</param>
    /// <returns>Узлы профиля, вкладываемые в UML Model.</returns>
    private static IEnumerable<XElement> BuildStereotypeApplications(IEnumerable<CatalogModel> catalogs, IEnumerable<ClassModel> classes)
    {
        return catalogs.Select(catalog => new XElement(profile + "catalog", new XAttribute("base_Component", catalog.Guid)))
            .Concat(classes.Select(cls => new XElement(profile + "type", new XAttribute("base_Class", cls.Guid))))
            .Concat(catalogs.SelectMany(catalog => catalog.Events.Select(eventModel =>
                new XElement(profile + "event", new XAttribute("base_Signal", eventModel.Guid)))))
            .Concat(catalogs.SelectMany(catalog => catalog.Settings.Select(setting =>
                CreateProfileTag("Component", catalog.Guid, setting.Key, setting.Value))));
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

    /// <summary>Создаёт модели выбранных типов и, для сложной иерархии, псевдотип подключения.</summary>
    /// <param name="context">Выбранный каталог, его типы и выбранные параметры.</param>
    /// <param name="catalogGuid">XMI ID UML-компонента справочника.</param>
    /// <returns>Типы с параметрами, подтверждённо принадлежащими каждому типу.</returns>
    private List<ClassModel> BuildClasses(SelectedCatalogContext context, string catalogGuid)
    {
        var list = new List<ClassModel>();
        var reference = context?.Reference;
        if (reference == null) return list;

        var sourceClasses = reference.Classes?.AllClasses?.Cast<object>().ToList() ?? new List<object>();
        foreach (var type in context.Types ?? new ТипОбъекта[0])
        {
            var sourceClass = sourceClasses.FirstOrDefault(item =>
                string.Equals(Convert.ToString(GetPropertyValue(item, "Name")), type.Имя, StringComparison.Ordinal));
            var cls = new ClassModel
            {
                Guid = FormatEaId(GetClassGuid(type, sourceClass, reference.Guid)),
                Name = type.Имя,
                CatalogGuid = catalogGuid,
                SourceClass = sourceClass,
                SourceType = type,
                Comment = Convert.ToString(GetPropertyValue(sourceClass, "Comment"))
            };
            var attachedParams = context.Parameters
                .Where(parameter => IsParameterAttachedToType(parameter, type, sourceClass, reference))
                .GroupBy(parameter => parameter.Guid)
                .Select(group => group.First());
            foreach (var parameter in attachedParams)
            {
                var (typeRef, eaType) = MapType(parameter.Type);
                string attrId = FormatEaId(CreateDeterministicGuid(
                    $"Property_{cls.Guid}_{parameter.Guid}"));
                cls.Parameters.Add(new ParamModel
                {
                    Guid = attrId,
                    Name = parameter.Name,
                    TypeRef = typeRef,
                    EaType = eaType
                });
            }
            list.Add(cls);
        }

        if (IsComplexHierarchy(GetPropertyValue(reference.Description, "HierarchyType")))
        {
            var connectionGroup = context.ConnectionGroup ?? FindConnectionGroup(reference);
            var connection = new ClassModel
            {
                Guid = FormatEaId(GetConnectionGroupGuid(connectionGroup, reference.Guid)),
                Name = "Подключение",
                CatalogGuid = catalogGuid,
                SourceClass = connectionGroup,
                SourceType = connectionGroup,
                IsConnection = true,
                Comment = GetObjectComment(connectionGroup)
            };

            foreach (var parameter in context.ConnectionParameters ?? new List<ParameterInfo>())
            {
                var (typeRef, eaType) = MapType(parameter.Type);
                string connectionAttrId = FormatEaId(CreateDeterministicGuid(
                    $"ConnectionProperty_{connection.Guid}_{parameter.Guid}"));
                connection.Parameters.Add(new ParamModel
                {
                    Guid = connectionAttrId,
                    Name = parameter.Name,
                    TypeRef = typeRef,
                    EaType = eaType
                });
            }
            list.Add(connection);
        }

        return list;
    }

    /// <summary>Получает GUID типа DOCs или создаёт стабильный резервный GUID.</summary>
    /// <param name="type">Тип-обёртка, выбранный в диалоге DOCs.</param>
    /// <param name="sourceClass">Модельный ClassObject, если он найден.</param>
    /// <param name="referenceGuid">GUID справочника-владельца.</param>
    /// <returns>Исходный GUID типа либо детерминированный GUID по справочнику и имени.</returns>
    private static Guid GetClassGuid(ТипОбъекта type, object sourceClass, Guid referenceGuid)
    {
        Guid typeGuid = ReadGuid(type, "Guid", "GUID", "ClassGuid");
        if (typeGuid != Guid.Empty) return typeGuid;

        Guid classGuid = ReadGuid(sourceClass, "Guid", "GUID", "ClassGuid");
        if (classGuid != Guid.Empty) return classGuid;

        return CreateDeterministicGuid($"Class_{referenceGuid}_{type?.Имя}");
    }

    /// <summary>Получает GUID группы подключения либо стабильный GUID от справочника.</summary>
    /// <param name="connectionGroup">Группа подключения T-FLEX DOCs.</param>
    /// <param name="referenceGuid">GUID справочника-владельца.</param>
    /// <returns>GUID группы или детерминированный GUID подключения.</returns>
    private static Guid GetConnectionGroupGuid(object connectionGroup, Guid referenceGuid)
    {
        Guid connectionGuid = ReadGuid(connectionGroup, "Guid", "GUID", "GroupGuid");
        return connectionGuid != Guid.Empty
            ? connectionGuid
            : CreateDeterministicGuid($"Connection_{referenceGuid}");
    }

    /// <summary>Проверяет принадлежность параметра типу с учётом общей группы и наследования.</summary>
    /// <param name="parameter">Параметр, выбранный пользователем.</param>
    /// <param name="sourceType">Wrapper-тип DOCs или группа подключения.</param>
    /// <param name="sourceClass">Модельный ClassObject типа.</param>
    /// <param name="reference">Справочник-владелец типов и главной группы параметров.</param>
    /// <returns>true, если параметр принадлежит типу напрямую, через базовый тип или как общий параметр.</returns>
    private static bool IsParameterAttachedToType(
        ParameterInfo parameter,
        object sourceType,
        object sourceClass,
        ReferenceInfo reference)
    {
        if (parameter == null) return false;

        // Главная группа справочника содержит общие параметры всех его типов.
        var mainGroup = GetPropertyValue(reference?.Description, "MainGroup");
        Guid mainGroupGuid = ReadGuid(mainGroup, "Guid", "GUID");
        Guid parameterGroupGuid = parameter.Group?.Guid ?? Guid.Empty;
        if (parameterGroupGuid != Guid.Empty &&
            (parameterGroupGuid == mainGroupGuid ||
             string.Equals(parameter.Group?.Name, "Общие параметры", StringComparison.CurrentCultureIgnoreCase) ||
             string.Equals(parameter.Group?.Name, reference?.Name, StringComparison.CurrentCultureIgnoreCase)))
        {
            return true;
        }

        // ClassTree DOCs знает о наследуемых группах и возвращает все подходящие классы.
        if (reference?.Classes != null && parameter.Group != null && sourceClass is ClassObject targetClass)
        {
            try
            {
                var classesWithGroup = reference.Classes.GetParameterGroupClasses(
                    parameter.Group,
                    includeInherit: true);
                if (classesWithGroup != null &&
                    classesWithGroup.Any(classObject => classObject != null && classObject.Guid == targetClass.Guid))
                    return true;
            }
            catch { }
        }

        // Явно обходим BaseClass на случай, если ClassTree не смог вернуть наследников.
        if (sourceClass is ClassObject classObject)
        {
            var current = classObject;
            var visited = new HashSet<Guid>();
            while (current != null && visited.Add(current.Guid))
            {
                try
                {
                    if (GetEnumerableProperty(current, "Parameters")
                        .Any(item => ReadGuid(item, "Guid", "ParameterGuid", "GUID") == parameter.Guid))
                        return true;
                }
                catch { }

                try
                {
                    if (parameterGroupGuid != Guid.Empty && current.ParameterGroups != null &&
                        current.ParameterGroups.Any(group => group != null && group.Guid == parameterGroupGuid))
                        return true;
                }
                catch { }

                current = GetPropertyValue(current, "BaseClass") as ClassObject;
            }
        }

        // Wrapper-тип служит последним совместимым способом прочитать связи параметра с типом.
        if (sourceType != null)
        {
            foreach (string propertyName in new[]
                     {
                         "Parameters", "Параметры", "ParameterGroups", "ГруппыПараметров"
                     })
            {
                if (GetEnumerableProperty(sourceType, propertyName).Any(item =>
                    (parameter.Guid != Guid.Empty &&
                     ReadGuid(item, "Guid", "ParameterGuid", "GroupGuid", "GUID") == parameter.Guid) ||
                        (parameterGroupGuid != Guid.Empty &&
                         ReadGuid(item, "Guid", "GroupGuid", "GUID") == parameterGroupGuid)))
                    return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Проверяет, подключена ли связь к конкретному типу-источнику (Master).
    /// Связь принадлежит типу, только если явно присутствует в его группах параметров.
    /// </summary>
    /// <param name="classModel">Экспортируемый класс типа DOCs.</param>
    /// <param name="linkGroup">Связь, принадлежность которой проверяется.</param>
    /// <returns>true, если связь явно привязана к типу или является структурной связью подключения.</returns>
    private static bool IsLinkAttachedToMasterClass(ClassModel classModel, ParameterGroup linkGroup)
    {
        if (classModel == null || linkGroup == null) return false;
        if (classModel.IsConnection) return true;

        if (classModel.SourceClass is ClassObject classObject)
        {
            try
            {
                if (classObject.ParameterGroups != null &&
                    classObject.ParameterGroups.Any(group => group != null && group.Guid == linkGroup.Guid))
                    return true;
            }
            catch { }

            try
            {
                if (classObject.SwappedToSelfParameterGroups != null &&
                    classObject.SwappedToSelfParameterGroups.Any(group => group != null && group.Guid == linkGroup.Guid))
                    return true;
            }
            catch { }

            return false;
        }

        return false;
    }

    /// <summary>Проверяет допустимость типа-приёмника через разрешённые классы самой связи.</summary>
    /// <param name="slaveClassModel">Экспортируемый класс типа-приёмника.</param>
    /// <param name="linkGroup">Связь, настройки которой задают допустимые типы-приёмники.</param>
    /// <returns>true, если тип разрешён связью либо у связи не задано ограничение по типам.</returns>
    private static bool IsLinkAllowedForSlaveClass(ClassModel slaveClassModel, ParameterGroup linkGroup)
    {
        if (slaveClassModel == null || linkGroup == null) return false;

        try
        {
            var allowedClasses = linkGroup.GetAllowedClassesToLink();
            if (allowedClasses != null && allowedClasses.Count > 0)
            {
                if (slaveClassModel.SourceClass is ClassObject slaveClassObject)
                    return allowedClasses.Any(allowedClass =>
                        allowedClass != null &&
                        (allowedClass.Guid == slaveClassObject.Guid || allowedClass.IsBaseClassFor(slaveClassObject)));

                return allowedClasses.Any(allowedClass => IsSameClassModel(slaveClassModel, allowedClass));
            }
        }
        catch { }

        return true;
    }

    /// <summary>Сопоставляет модель типа с классом DOCs по GUID или имени.</summary>
    /// <param name="model">Класс экспортной модели.</param>
    /// <param name="classObj">Объект класса, возвращённый API T-FLEX DOCs.</param>
    /// <returns>true, если объекты представляют один тип.</returns>
    private static bool IsSameClassModel(ClassModel model, object classObj)
    {
        if (model == null || classObj == null) return false;

        Guid objectGuid = ReadGuid(classObj, "Guid", "GUID", "ClassGuid");
        if (objectGuid != Guid.Empty)
        {
            return ReadGuid(model.SourceClass, "Guid", "GUID", "ClassGuid") == objectGuid ||
                   ReadGuid(model.SourceType, "Guid", "GUID", "ClassGuid") == objectGuid;
        }

        string name = Convert.ToString(GetPropertyValue(classObj, "Name") ?? GetPropertyValue(classObj, "Имя"));
        return !string.IsNullOrWhiteSpace(name) &&
               string.Equals(model.Name, name, StringComparison.Ordinal);
    }

    /// <summary>Создаёт физические связи сложной иерархии через псевдокласс «Подключение».</summary>
    /// <param name="selectedCatalogs">Выбранные справочники и их типы объектов.</param>
    /// <param name="classesByReferenceGuid">UML-классы, сгруппированные по GUID справочника.</param>
    /// <param name="allAssocs">Общий список ассоциаций для UML и EA Extension.</param>
    private static void AddComplexHierarchyAssociations(
        IEnumerable<SelectedCatalogContext> selectedCatalogs,
        IDictionary<Guid, List<ClassModel>> classesByReferenceGuid,
        List<AssocModel> allAssocs)
    {
        if (selectedCatalogs == null || classesByReferenceGuid == null || allAssocs == null) return;

        foreach (var context in selectedCatalogs.Where(item => item?.Reference != null))
        {
            if (!classesByReferenceGuid.TryGetValue(context.Reference.Guid, out var catalogClasses) || catalogClasses == null)
                continue;

            var connectionClass = catalogClasses.FirstOrDefault(classModel => classModel?.IsConnection == true);
            bool isComplexHierarchy = IsComplexHierarchy(GetPropertyValue(context.Reference.Description, "HierarchyType"));
            if (connectionClass == null || (!isComplexHierarchy && !catalogClasses.Any(classModel => classModel?.IsConnection == true)))
                continue;

            foreach (var objectClass in catalogClasses.Where(classModel => classModel != null && !classModel.IsConnection))
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
        Guid associationGuid = CreateDeterministicGuid(
            $"{associationSeed}_{connectionClass.Guid}_{objectClass.Guid}");
        var association = new AssocModel
        {
            AssocGuid = FormatEaId(associationGuid),
            SrcPropGuid = FormatEaId(associationGuid, "EAID_src_"),
            DstPropGuid = FormatEaId(associationGuid, "EAID_dst_"),
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

    /// <summary>Добавляет ассоциации «Состоит из» по допустимым дочерним классам дерева.</summary>
    /// <param name="selectedCatalogs">Выбранные справочники и их типы объектов.</param>
    /// <param name="classesByReferenceGuid">UML-классы, сгруппированные по GUID справочника.</param>
    /// <param name="allAssocs">Общий список ассоциаций для UML и EA Extension.</param>
    private static void AddIntraCatalogHierarchyAssociations(
        IEnumerable<SelectedCatalogContext> selectedCatalogs,
        IDictionary<Guid, List<ClassModel>> classesByReferenceGuid,
        List<AssocModel> allAssocs)
    {
        if (selectedCatalogs == null || classesByReferenceGuid == null || allAssocs == null) return;

        foreach (var context in selectedCatalogs.Where(item => item?.Reference != null && IsHierarchicalCatalog(item.Reference)))
        {
            if (!classesByReferenceGuid.TryGetValue(context.Reference.Guid, out var catalogClasses)) continue;
            if (catalogClasses == null) continue;

            foreach (var parentClass in catalogClasses.Where(classModel => classModel != null))
            {
                foreach (var childObjectClass in GetChildClasses(parentClass))
                {
                    var childClass = ResolveSelectedChildClass(childObjectClass, catalogClasses);
                    if (childClass == null || childClass.Guid == parentClass.Guid) continue;
                    if (parentClass.Associations.Any(association =>
                            association != null && association.Name == "Состоит из" && association.TargetClassGuid == childClass.Guid)) continue;

                    Guid associationGuid = CreateDeterministicGuid(
                        $"ConsistsOf_{parentClass.Guid}_{childClass.Guid}");
                    var association = new AssocModel
                    {
                        AssocGuid = FormatEaId(associationGuid),
                        SrcPropGuid = FormatEaId(associationGuid, "EAID_src_"),
                        DstPropGuid = FormatEaId(associationGuid, "EAID_dst_"),
                        Name = "Состоит из",
                        SourceClassGuid = parentClass.Guid,
                        SourceClassName = parentClass.Name,
                        TargetClassGuid = childClass.Guid,
                        TargetClassName = childClass.Name,
                        SourceLower = "0",
                        SourceUpper = "1",
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

    /// <summary>Получает дочерние типы через свойство ChildObjectClasses исходного класса или wrapper-типа.</summary>
    /// <param name="parentClass">Класс-родитель, дочерние типы которого требуется прочитать.</param>
    /// <returns>Объекты дочерних классов; при недоступности API возвращается пустой список.</returns>
    private static IEnumerable<object> GetChildClasses(ClassModel parentClass)
    {
        if (parentClass == null) return Enumerable.Empty<object>();

        var sourceClass = parentClass.SourceClass;
        var sourceType = parentClass.SourceType;
        if (TryGetChildClassesFromOwner(sourceClass, out var childClasses))
            return childClasses;
        if (!ReferenceEquals(sourceType, sourceClass) && TryGetChildClassesFromOwner(sourceType, out childClasses))
            return childClasses;
        return Enumerable.Empty<object>();
    }

    /// <summary>Безопасно читает коллекцию ChildObjectClasses через публичное свойство экземпляра.</summary>
    /// <param name="owner">Объект ClassObject или wrapper-класса.</param>
    /// <param name="childClasses">Полученные элементы коллекции.</param>
    /// <returns>true, если свойство найдено и его значение удалось перечислить.</returns>
    private static bool TryGetChildClassesFromOwner(object owner, out IEnumerable<object> childClasses)
    {
        childClasses = Enumerable.Empty<object>();
        if (owner == null) return false;

        try
        {
            var property = owner.GetType().GetProperty("ChildObjectClasses", BindingFlags.Public | BindingFlags.Instance);
            if (property == null) return false;

            var value = property.GetValue(owner, null) as System.Collections.IEnumerable;
            if (value == null) return false;

            var result = new List<object>();
            foreach (var childClass in value)
                if (childClass != null) result.Add(childClass);
            childClasses = result;
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>Находит выбранный UML-класс по GUID или имени объекта ChildObjectClasses.</summary>
    /// <param name="childObjectClass">Дочерний ClassObject из модели DOCs.</param>
    /// <param name="catalogClasses">Типы и псевдоклассы текущего справочника.</param>
    /// <returns>Совпавший выбранный класс либо null.</returns>
    private static ClassModel ResolveSelectedChildClass(object childObjectClass, IEnumerable<ClassModel> catalogClasses)
    {
        if (childObjectClass == null || catalogClasses == null) return null;

        Guid childGuid = ReadGuid(childObjectClass, "Guid", "GUID", "ClassGuid");
        if (childGuid != Guid.Empty)
        {
            var classByGuid = catalogClasses.FirstOrDefault(classModel =>
                classModel != null &&
                (ReadGuid(classModel.SourceClass, "Guid", "GUID", "ClassGuid") == childGuid ||
                 ReadGuid(classModel.SourceType, "Guid", "GUID", "ClassGuid") == childGuid));
            if (classByGuid != null) return classByGuid;
        }

        string childName = Convert.ToString(GetPropertyValue(childObjectClass, "Name") ??
                                            GetPropertyValue(childObjectClass, "Имя"));
        if (string.IsNullOrWhiteSpace(childName)) return null;

        return catalogClasses.FirstOrDefault(classModel =>
            classModel != null &&
            (string.Equals(Convert.ToString(GetPropertyValue(classModel.SourceClass, "Name")), childName, StringComparison.Ordinal) ||
             string.Equals(Convert.ToString(GetPropertyValue(classModel.SourceClass, "Имя")), childName, StringComparison.Ordinal) ||
             string.Equals(Convert.ToString(GetPropertyValue(classModel.SourceType, "Name")), childName, StringComparison.Ordinal) ||
             string.Equals(Convert.ToString(GetPropertyValue(classModel.SourceType, "Имя")), childName, StringComparison.Ordinal) ||
             string.Equals(classModel.Name, childName, StringComparison.Ordinal)));
    }

    /// <summary>Читает GUID из одного из известных имен свойств DOCs, возвращая Empty при неизвестном формате.</summary>
    /// <param name="source">Объект API или wrapper с идентификатором.</param>
    /// <param name="propertyNames">Допустимые названия идентификатора в версиях API.</param>
    /// <returns>Найденный GUID либо Guid.Empty.</returns>
    private static Guid ReadGuid(object source, params string[] propertyNames)
    {
        foreach (string propertyName in propertyNames)
        {
            object value = GetPropertyValue(source, propertyName);
            if (value is Guid guid) return guid;
            if (Guid.TryParse(Convert.ToString(value), out guid)) return guid;
        }
        return Guid.Empty;
    }
}