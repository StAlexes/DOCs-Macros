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
        /// <summary>Уникальный ключ флажка связи в диалоге.</summary>
        public string DialogKey { get; set; }
    }

    /// <summary>Промежуточная UML-модель типа DOCs и его атрибутов/ассоциаций.</summary>
    private class ClassModel
    {
        public string Guid { get; set; } = "EAID_" + System.Guid.NewGuid().ToString("N").ToUpper();
        public string Name { get; set; }
        public string Comment { get; set; }
        public string CatalogGuid { get; set; }
        public object SourceClass { get; set; }
        public List<ParamModel> Parameters { get; set; } = new List<ParamModel>();
        public List<AssocModel> Associations { get; set; } = new List<AssocModel>();
    }

    /// <summary>Промежуточная UML-модель справочника, его настроек и событий.</summary>
    private class CatalogModel
    {
        public string Guid { get; set; } = "EAID_" + System.Guid.NewGuid().ToString("N").ToUpper();
        public string Name { get; set; }
        public string Comment { get; set; }
        public List<KeyValuePair<string, string>> Settings { get; set; } = new List<KeyValuePair<string, string>>();
        public List<EventModel> Events { get; set; } = new List<EventModel>();
    }

    /// <summary>Промежуточное представление пользовательского или системного события DOCs.</summary>
    private class EventModel
    {
        public string Guid { get; set; } = "EAID_" + System.Guid.NewGuid().ToString("N").ToUpper();
        public string Name { get; set; }
        public string Comment { get; set; }
        public string CatalogGuid { get; set; }
    }

    /// <summary>Промежуточное представление параметра DOCs как UML Property.</summary>
    private class ParamModel
    {
        public string Guid { get; set; } = "EAID_" + System.Guid.NewGuid().ToString("N").ToUpper();
        public string Name { get; set; }
        public string TypeRef { get; set; }
        public string EaType { get; set; }
    }

    /// <summary>Промежуточное представление направленной связи между классами.</summary>
    private class AssocModel
    {
        public string AssocGuid { get; set; } = "EAID_" + System.Guid.NewGuid().ToString("N").ToUpper();
        public string SrcPropGuid { get; set; } = "EAID_src_" + System.Guid.NewGuid().ToString("N").ToUpper();
        public string DstPropGuid { get; set; } = "EAID_dst_" + System.Guid.NewGuid().ToString("N").ToUpper();
        public string Name { get; set; }
        public string SourceClassGuid { get; set; }
        public string SourceClassName { get; set; }
        public string TargetClassGuid { get; set; }
        public string TargetClassName { get; set; }
        public string SourceLower { get; set; } = "1";
        public string SourceUpper { get; set; } = "1";
        public string TargetLower { get; set; } = "1";
        public string TargetUpper { get; set; } = "1";
    }

    /// <summary>Ассоциация EA, связывающая компонент справочника с UML-классом его типа.</summary>
    private class CatalogTypeAssocModel
    {
        public string Guid { get; set; } = CreateEaId();
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
        while (true)
        {
            string title = $"Шаг 1: Выберите справочник ({selectedCatalogs.Count + 1})";
            string referenceName = SelectReference(title, selectedCatalogs.Select(item => item.Reference.Guid).ToHashSet());
            if (string.IsNullOrWhiteSpace(referenceName))
                break;

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
                selectedCatalogs.Add(new SelectedCatalogContext
                {
                    Reference = reference,
                    Types = types,
                    Parameters = parameters
                });
            }

            if (types.Length == 0 && selectedCatalogs.Count == 0)
            {
                bool? tryAgain = QuestionWithCancel("Не выбраны типы объектов. Выбрать другой справочник?");
                if (tryAgain != true) return;
                continue;
            }

            bool? addMore = QuestionWithCancel("Добавить еще типы из справочника?");
            if (addMore != true)
                break;
        }

        if (selectedCatalogs.Count == 0)
        {
            Сообщение("Информация", "Не выбран ни один справочник с типами объектов для экспорта.");
            return;
        }

        var selectedRelations = SelectRelations(selectedCatalogs);
        GenerateXmiFile(selectedCatalogs, selectedRelations);
    }

    /// <summary>Показывает выбор справочника и исключает каталоги, уже выбранные в текущем запуске.</summary>
    /// <param name="title">Заголовок диалога выбора справочника.</param>
    /// <param name="excludedReferenceGuids">GUID ранее выбранных справочников.</param>
    /// <returns>Имя выбранного каталога или null при отмене/отсутствии вариантов.</returns>
    private string SelectReference(string title, HashSet<Guid> excludedReferenceGuids)
    {
        var refs = Context.Connection.ReferenceCatalog.GetReferences()
            .Where(reference => reference != null && !excludedReferenceGuids.Contains(reference.Guid))
            .Select(reference => reference.Name)
            .OrderBy(name => name, StringComparer.CurrentCultureIgnoreCase)
            .ToArray();
        if (refs.Length == 0) return null;

        var dlg = СоздатьДиалогВвода(title);
        dlg.ДобавитьВыборИзСписка("Справочник", refs);
        return dlg.Показать() ? dlg["Справочник"]?.ToString() : null;
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

        var parameters = reference.Description.GetAllGroups()
            .Where(group => group != null)
            .SelectMany(group => group.Parameters ?? Enumerable.Empty<ParameterInfo>())
            .Where(p => p != null && p.IsVisible)
            .GroupBy(p => p.Guid)
            .Select(group => group.First())
            .ToList();
        if (parameters.Count == 0) return new List<ParameterInfo>();

        var dlg = СоздатьДиалогВвода(title);
        dlg.Высота = 700;
        dlg.Ширина = 620;
        dlg.ОтобразитьПолосыПрокрутки(true, false);

        const string selectAllKey = "[v] Выбрать все параметры";
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
        Guid? primaryGroupGuid = GetPrimaryGroupGuid(reference, parameterGroups.Select(item => item.Group));
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
        var candidates = new List<SelectedRelation>();

        foreach (var master in catalogs)
        {
            var links = master.Reference.Description?.GetLinks();
            if (links == null) continue;

            foreach (var link in links)
            {
                var slaveReference = link?.SlaveGroup?.ReferenceInfo;
                if (slaveReference == null || !selectedReferenceGuids.Contains(slaveReference.Guid) || slaveReference.Guid == master.Reference.Guid)
                    continue;

                var slave = catalogs.FirstOrDefault(item => item.Reference.Guid == slaveReference.Guid);
                if (slave == null) continue;

                candidates.Add(new SelectedRelation { Link = link, Master = master, Slave = slave });
            }
        }

        if (candidates.Count == 0)
        {
            Сообщение("Информация", "Связи между выбранными справочниками не найдены.");
            return candidates;
        }

        var dlg = СоздатьДиалогВвода("Шаг 4: Выберите связи между справочниками");
        var usedKeys = new HashSet<string>(StringComparer.Ordinal);
        var linkKeys = new Dictionary<SelectedRelation, string>();
        foreach (var group in candidates.GroupBy(item => item.Master.Reference.Name)
                     .OrderBy(group => group.Key, StringComparer.CurrentCultureIgnoreCase))
        {
            dlg.ДобавитьГруппу(group.Key);
            foreach (var relation in group.OrderBy(item => item.Link.Name, StringComparer.CurrentCultureIgnoreCase))
            {
                string label = $"{relation.Link.Name} [{relation.Master.Reference.Name} -> {relation.Slave.Reference.Name}]";
                string comment = GetObjectComment(relation.Link);
                if (!string.IsNullOrWhiteSpace(comment)) label += $" [{comment}]";
                string key = MakeUniqueDialogKey(label, usedKeys);
                relation.DialogKey = key;
                linkKeys[relation] = key;
                dlg.ДобавитьФлаг(key, false);
            }
        }

        var chosen = new List<SelectedRelation>();
        if (dlg.Показать())
        {
            foreach (var relation in candidates)
                if (GetDialogFlag(dlg, linkKeys[relation])) chosen.Add(relation);
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
            context => BuildClasses(context.Types, context.Parameters, context.Reference, catalogByReferenceGuid[context.Reference.Guid].Guid));
        var allClasses = classesByReferenceGuid.Values.SelectMany(classes => classes).ToList();
        var catalogTypeAssocs = allClasses.Select(cls =>
        {
            var catalog = catalogs.First(item => item.Guid == cls.CatalogGuid);
            return new CatalogTypeAssocModel
            {
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
            var masterClasses = classesByReferenceGuid[link.Master.Reference.Guid];
            var slaveClasses = classesByReferenceGuid[link.Slave.Reference.Guid];
            foreach (var masterClass in masterClasses)
            {
                foreach (var slaveClass in slaveClasses)
                {
                    var multiplicity = GetAssociationMultiplicity(link.Link);
                    var assoc = new AssocModel
                    {
                        Name = link.Link.Name,
                        SourceClassGuid = masterClass.Guid, SourceClassName = masterClass.Name,
                        TargetClassGuid = slaveClass.Guid, TargetClassName = slaveClass.Name,
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

        string packageGuid = "EAPK_" + System.Guid.NewGuid().ToString("N").ToUpper();
        var pkg = new XElement("packagedElement",
            new XAttribute(xmi + "type", "uml:Package"),
            new XAttribute(xmi + "id", packageGuid),
            new XAttribute("name", $"Модель: {string.Join(", ", catalogs.Select(catalog => catalog.Name))}"),
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

                foreach (var a in cls.Associations)
                    clsElem.Add(CreateAssocProperty(a.DstPropGuid, a.AssocGuid, a.TargetClassGuid, a.TargetLower, a.TargetUpper));

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
                new XAttribute("name", a.Name),
                new XAttribute("visibility", "public"),
                new XElement("memberEnd", new XAttribute(xmi + "idref", a.DstPropGuid)),
                new XElement("memberEnd", new XAttribute(xmi + "idref", a.SrcPropGuid)),
                CreateOwnedEnd(a.SrcPropGuid, a.AssocGuid, a.SourceClassGuid, a.SourceLower, a.SourceUpper)
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
        string catalogEndId = CreateEaId();
        string typeEndId = CreateEaId();
        return new XElement("packagedElement",
            new XAttribute(xmi + "type", "uml:Association"),
            new XAttribute(xmi + "id", association.Guid),
            new XAttribute("name", ""),
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
                events.Add(new EventModel { Name = name, Comment = comment, CatalogGuid = catalog.Guid });
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

    /// <summary>Создаёт навигационное свойство класса для выбранной связи DOCs.</summary>
    /// <param name="id">Идентификатор конца ассоциации.</param>
    /// <param name="assocId">Идентификатор UML Association.</param>
    /// <param name="targetGuid">Идентификатор целевого класса.</param>
    /// <param name="lower">Нижняя граница кратности.</param>
    /// <param name="upper">Верхняя граница кратности или символ бесконечности.</param>
    /// <returns>Элемент ownedAttribute, принадлежащий классу-источнику.</returns>
    private XElement CreateAssocProperty(string id, string assocId, string targetGuid, string lower, string upper)
    {
        return new XElement("ownedAttribute",
            new XAttribute(xmi + "type", "uml:Property"), new XAttribute(xmi + "id", id),
            new XAttribute("visibility", "public"), new XAttribute("association", assocId),
            new XElement("type", new XAttribute(xmi + "idref", targetGuid)),
            new XElement("lowerValue", new XAttribute(xmi + "type", "uml:LiteralInteger"), new XAttribute("value", lower)),
            new XElement("upperValue", new XAttribute(xmi + "type", upper == "*" ? "uml:LiteralUnlimitedNatural" : "uml:LiteralInteger"), new XAttribute("value", upper == "*" ? "-1" : upper))
        );
    }

    /// <summary>Создаёт противоположный конец UML Association с типом и кратностью.</summary>
    /// <param name="id">Идентификатор конца связи.</param>
    /// <param name="assocId">Идентификатор ассоциации-владельца.</param>
    /// <param name="sourceGuid">GUID класса, являющегося типом конца.</param>
    /// <param name="lower">Минимальная кратность.</param>
    /// <param name="upper">Максимальная кратность или символ бесконечности.</param>
    /// <returns>XML-элемент ownedEnd.</returns>
    private XElement CreateOwnedEnd(string id, string assocId, string sourceGuid, string lower, string upper)
    {
        return new XElement("ownedEnd",
            new XAttribute(xmi + "type", "uml:Property"), new XAttribute(xmi + "id", id),
            new XAttribute("visibility", "public"), new XAttribute("association", assocId),
            new XElement("type", new XAttribute(xmi + "idref", sourceGuid)),
            new XElement("lowerValue", new XAttribute(xmi + "type", "uml:LiteralInteger"), new XAttribute("value", lower)),
            new XElement("upperValue", new XAttribute(xmi + "type", upper == "*" ? "uml:LiteralUnlimitedNatural" : "uml:LiteralInteger"), new XAttribute("value", upper == "*" ? "-1" : upper))
        );
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
    /// <param name="assocs">Связи между классами разных каталогов.</param>
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
            var typeLinks = catalogTypeAssocs.Where(item => item.TypeGuid == cls.Guid)
                .Select(item => new XElement("Association",
                    new XAttribute(xmi + "id", item.Guid),
                    new XAttribute("start", item.CatalogGuid),
                    new XAttribute("end", item.TypeGuid)));
            el.Add(new XElement("links", typeLinks));
            el.Add(new XElement("xrefs"));
            elems.Add(el);
        }

        foreach (var eventModel in catalogs.SelectMany(catalog => catalog.Events))
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
                properties);
            elems.Add(element);
        }
        ext.Add(elems);

        var connectors = new XElement("connectors");
        foreach (var a in assocs)
        {
            connectors.Add(new XElement("connector", new XAttribute(xmi + "idref", a.AssocGuid),
                new XElement("source", new XAttribute(xmi + "idref", a.SourceClassGuid)),
                new XElement("target", new XAttribute(xmi + "idref", a.TargetClassGuid)),
                new XElement("properties", new XAttribute("ea_type", "Association"))
            ));
        }
        foreach (var association in catalogTypeAssocs)
        {
            connectors.Add(new XElement("connector",
                new XAttribute(xmi + "idref", association.Guid),
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
            umlElement.Add(new XElement("ownedComment",
                new XAttribute(xmi + "type", "uml:Comment"),
                new XAttribute(xmi + "id", CreateEaId()),
                new XElement("body", comment)));
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
                new XAttribute(xmi + "id", CreateEaId()),
                new XAttribute("name", setting.Key),
                new XAttribute("value", setting.Value ?? string.Empty),
                new XAttribute("modelElement", modelElement)));
            index++;
        }
        if (index > 0) element.Add(tags);
    }

    /// <summary>Генерирует уникальный EAID в формате GUID с разделителями.</summary>
    /// <returns>Новый XMI ID.</returns>
    private static string CreateEaId()
    {
        string guid = System.Guid.NewGuid().ToString("N").ToUpperInvariant();
        return $"EAID_{guid.Substring(0, 8)}_{guid.Substring(8, 4)}_{guid.Substring(12, 4)}_{guid.Substring(16, 4)}_{guid.Substring(20, 12)}";
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

    /// <summary>Строит UML-классы из типов DOCs и выбранных параметров каталога.</summary>
    /// <param name="types">Выбранные типы объектов.</param>
    /// <param name="parameters">Выбранные параметры, включаемые в каждый тип.</param>
    /// <param name="reference">Источник имен, комментариев и классов DOCs.</param>
    /// <param name="catalogGuid">ID UML Component-владельца типов.</param>
    /// <returns>Промежуточные модели классов для сериализации.</returns>
    private List<ClassModel> BuildClasses(ТипОбъекта[] types, List<ParameterInfo> parameters, ReferenceInfo reference, string catalogGuid)
    {
        var list = new List<ClassModel>();
        var sourceClasses = reference.Classes?.AllClasses?.Cast<object>().ToList() ?? new List<object>();
        foreach (var t in types)
        {
            var sourceClass = sourceClasses.FirstOrDefault(item => string.Equals(Convert.ToString(GetPropertyValue(item, "Name")), t.Имя, StringComparison.Ordinal));
            var cls = new ClassModel
            {
                Name = t.Имя,
                CatalogGuid = catalogGuid,
                SourceClass = sourceClass,
                Comment = Convert.ToString(GetPropertyValue(sourceClass, "Comment"))
            };
            foreach (var p in parameters)
            {
                var (typeRef, eaType) = MapType(p.Type);
                cls.Parameters.Add(new ParamModel { Name = p.Name, TypeRef = typeRef, EaType = eaType });
            }
            list.Add(cls);
        }
        return list;
    }
}