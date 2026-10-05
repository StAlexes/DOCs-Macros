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

public class XmiSchemaExporterMacro : MacroProvider
{
    public XmiSchemaExporterMacro(MacroContext context) : base(context) { }

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

    private class CatalogModel
    {
        public string Guid { get; set; } = "EAID_" + System.Guid.NewGuid().ToString("N").ToUpper();
        public string Name { get; set; }
        public string Comment { get; set; }
        public List<KeyValuePair<string, string>> Settings { get; set; } = new List<KeyValuePair<string, string>>();
        public List<EventModel> Events { get; set; } = new List<EventModel>();
    }

    private class EventModel
    {
        public string Guid { get; set; } = "EAID_" + System.Guid.NewGuid().ToString("N").ToUpper();
        public string Name { get; set; }
        public string Comment { get; set; }
        public string CatalogGuid { get; set; }
    }

    private class ParamModel
    {
        public string Guid { get; set; } = "EAID_" + System.Guid.NewGuid().ToString("N").ToUpper();
        public string Name { get; set; }
        public string TypeRef { get; set; }
        public string EaType { get; set; }
    }

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

    public override void Run()
    {
        // 1-3. Выбор справочника А, его типов и параметров
        string refA = SelectReference("Шаг 1: Выберите справочник А");
        if (string.IsNullOrEmpty(refA)) return;
        var typesA = SelectObjectTypes(refA, "Шаг 2: Выберите типы объектов справочника А");
        if (typesA.Length == 0) return;
        var paramsA = SelectParameters(refA, "Шаг 3: Выберите параметры справочника А");

        // 4-6. Выбор справочника Б, его типов и параметров
        string refB = SelectReference("Шаг 4: Выберите справочник Б");
        if (string.IsNullOrEmpty(refB)) return;
        var typesB = SelectObjectTypes(refB, "Шаг 5: Выберите типы объектов справочника Б");
        if (typesB.Length == 0) return;
        var paramsB = SelectParameters(refB, "Шаг 6: Выберите параметры справочника Б");

        // 7. Выбор связей
        var selectedLinks = SelectRelations(refA, refB);

        // 8. Сохранение XMI
        GenerateXmiFile(refA, typesA, paramsA, refB, typesB, paramsB, selectedLinks);
    }

    private string SelectReference(string title)
    {
        var refs = Context.Connection.ReferenceCatalog.GetReferences().Select(r => r.Name).OrderBy(n => n).ToArray();
        var dlg = СоздатьДиалогВвода(title);
        dlg.ДобавитьВыборИзСписка("Справочник", refs);
        return dlg.Показать() ? dlg["Справочник"]?.ToString() : null;
    }

    private ТипОбъекта[] SelectObjectTypes(string refName, string title)
    {
        var dlg = СоздатьДиалогВыбораТипов(refName);
        dlg.Заголовок = title;
        dlg.ВыборАбстрактныхТипов = true;
        dlg.ВыборФлажками = true;
        dlg.АвтоВыборФлажками = false;
        return dlg.Показать() ? dlg.ВыбранныеТипы : new ТипОбъекта[0];
    }

    private List<ParameterInfo> SelectParameters(string refName, string title)
    {
        var refInfo = Context.Connection.ReferenceCatalog.Find(refName);
        if (refInfo == null) return new List<ParameterInfo>();

        var parameters = refInfo.Description.GetAllGroups()
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

        var groups = parameters.GroupBy(p => p.Group?.Name ?? "Общие параметры").OrderBy(g => g.Key);
        var duplicateNames = parameters
            .GroupBy(p => p.Name)
            .Where(group => group.Count() > 1)
            .Select(group => group.Key)
            .ToHashSet(StringComparer.Ordinal);
        var parameterKeys = new Dictionary<Guid, string>();

        foreach (var g in groups)
        {
            dlg.ДобавитьГруппу(g.Key);
            foreach (var p in g.OrderBy(p => p.Name))
            {
                string key = duplicateNames.Contains(p.Name) ? $"{g.Key} / {p.Name}" : p.Name;
                parameterKeys[p.Guid] = key;
                dlg.ДобавитьФлаг(key, false);
            }
        }

        var selected = new List<ParameterInfo>();
        if (dlg.Показать())
        {
            foreach (var p in parameters)
            {
                string key = parameterKeys[p.Guid];
                if (dlg[key] != null && (bool)dlg[key]) selected.Add(p);
            }
        }
        return selected;
    }

    private List<ParameterGroup> SelectRelations(string refNameA, string refNameB)
    {
        var refA = Context.Connection.ReferenceCatalog.Find(refNameA);
        var refB = Context.Connection.ReferenceCatalog.Find(refNameB);
        var links = new List<ParameterGroup>();

        if (refA != null && refB != null)
        {
            foreach (var link in refA.Description.GetLinks())
            {
                if (link.SlaveGroup != null && link.SlaveGroup.ReferenceInfo?.Guid == refB.Guid)
                    links.Add(link);
            }
        }

        if (links.Count == 0)
        {
            Сообщение("Информация", $"Связи между '{refNameA}' и '{refNameB}' не найдены.");
            return links;
        }

        var dlg = СоздатьДиалогВвода("Шаг 7: Выберите связи");
        foreach (var rel in links.OrderBy(r => r.Name))
            dlg.ДобавитьФлаг(rel.Name, false);

        var chosen = new List<ParameterGroup>();
        if (dlg.Показать())
        {
            foreach (var rel in links)
            {
                string flag = rel.Name;
                if (dlg[flag] != null && (bool)dlg[flag]) chosen.Add(rel);
            }
        }
        return chosen;
    }

    private (string typeRef, string eaType) MapType(ParameterType pType)
    {
        if (pType == null) return ("EAC__string", "string");
        if (pType.IsBoolean) return ("EAC__boolean", "boolean");
        if (pType.IsDateTime) return ("EAC__date", "date");
        if (pType.IsFloat || pType.IsMoney || pType.IsNumber) return ("EAC__double", "double");
        if (pType.IsInt) return ("EAC__int", "int");
        return ("EAC__string", "string");
    }

    private void GenerateXmiFile(string refA, ТипОбъекта[] typesA, List<ParameterInfo> paramsA,
                                 string refB, ТипОбъекта[] typesB, List<ParameterInfo> paramsB,
                                 List<ParameterGroup> links)
    {
        var saveDlg = СоздатьДиалогСохраненияФайла();
        saveDlg.Заголовок = "Сохранение файла XMI";
        saveDlg.Фильтр = "Файлы XML (*.xml)|*.xml";
        saveDlg.РасширениеПоУмолчанию = "xml";
        if (!saveDlg.Показать()) return;

        var referenceA = Context.Connection.ReferenceCatalog.Find(refA);
        var referenceB = Context.Connection.ReferenceCatalog.Find(refB);
        var catalogA = BuildCatalog(referenceA);
        var catalogB = BuildCatalog(referenceB);
        var catalogs = new List<CatalogModel> { catalogA, catalogB };
        var classesA = BuildClasses(typesA, paramsA, referenceA, catalogA.Guid);
        var classesB = BuildClasses(typesB, paramsB, referenceB, catalogB.Guid);
        var allClasses = classesA.Concat(classesB).ToList();
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
            foreach (var ca in classesA)
            {
                foreach (var cb in classesB)
                {
                    var multiplicity = GetAssociationMultiplicity(link);
                    var assoc = new AssocModel
                    {
                        Name = link.Name,
                        SourceClassGuid = ca.Guid, SourceClassName = ca.Name,
                        TargetClassGuid = cb.Guid, TargetClassName = cb.Name,
                        SourceLower = multiplicity.sourceLower,
                        SourceUpper = multiplicity.sourceUpper,
                        TargetLower = multiplicity.targetLower,
                        TargetUpper = multiplicity.targetUpper
                    };
                    ca.Associations.Add(assoc);
                    allAssocs.Add(assoc);
                }
            }
        }

        string packageGuid = "EAPK_" + System.Guid.NewGuid().ToString("N").ToUpper();
        var pkg = new XElement("packagedElement",
            new XAttribute(xmi + "type", "uml:Package"),
            new XAttribute(xmi + "id", packageGuid),
            new XAttribute("name", $"Классы {refA} и {refB}"),
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

    private static void AddStructureSettings(List<KeyValuePair<string, string>> settings, object group)
    {
        var structureSettings = new[]
        {
            Tuple.Create("Тип структуры", "Type"),
            Tuple.Create("Тип иерархии", "HierarchyType"),
            Tuple.Create("Видимость", "Visibility"),
            Tuple.Create("Статус активности", "ActivityStatus"),
            Tuple.Create("Журнал изменений данных", "SupportsDataChangeLog"),
            Tuple.Create("Ревизии", "SupportsRevisions"),
            Tuple.Create("Прототипы", "SupportsPrototypes"),
            Tuple.Create("Владелец", "SupportsOwner"),
            Tuple.Create("Подписи", "SupportsSignature"),
            Tuple.Create("Все типы подписей", "UseAllSignatureTypes"),
            Tuple.Create("Конфигурирование", "SupportsConfigurationSettings"),
            Tuple.Create("Даты действия", "SupportsActivityDates"),
            Tuple.Create("Применимость", "SupportsApplicability"),
            Tuple.Create("Контексты проектирования", "SupportsDesignContexts"),
            Tuple.Create("Замены в контексте", "SupportsSubstitutesInContext"),
            Tuple.Create("Корзина", "SupportsRecycleBin"),
            Tuple.Create("Рабочий стол", "SupportsDesktop"),
            Tuple.Create("Номенклатура", "SupportsNomenclature"),
            Tuple.Create("Экземпляры объектов", "SupportsObjectsInstances"),
            Tuple.Create("Типы структур", "SupportsStructureTypes"),
            Tuple.Create("Иерархия", "HasHierarchy"),
            Tuple.Create("Классы", "SupportsClasses"),
            Tuple.Create("Системные объекты", "SupportsSystemObjects"),
            Tuple.Create("Порядок объектов", "SupportsOrder"),
            Tuple.Create("Приватные папки", "SupportsPrivateFolders"),
            Tuple.Create("Расширенные параметры", "SupportsExtendedParameters"),
            Tuple.Create("Обязательный доступ", "SupportsMandatoryAccess"),
            Tuple.Create("Шифрование", "SupportsEncryption"),
            Tuple.Create("Этапы", "SupportsStages"),
            Tuple.Create("Множественные вложения", "SupportMultiAttachment"),
            Tuple.Create("Можно менять тип", "CanChangeClass"),
            Tuple.Create("Можно редактировать", "CanEdit"),
            Tuple.Create("Можно удалять", "CanDelete"),
            Tuple.Create("Имя таблицы", "TableName"),
            Tuple.Create("Имя таблицы сгенерировано", "TableNameGenerated"),
            Tuple.Create("GUID конфигуратора", "ConfiguratorGuid")
        };

        foreach (var setting in structureSettings)
        {
            object value = group == null ? "недоступно" : GetStructureSettingValue(group, setting.Item2);
            settings.Add(new KeyValuePair<string, string>(setting.Item1, Convert.ToString(value)));
        }
    }

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

    private static IEnumerable<object> GetEnumerableProperty(object source, string propertyName)
    {
        var value = GetPropertyValue(source, propertyName) as System.Collections.IEnumerable;
        if (value == null) yield break;
        foreach (var item in value)
            if (item != null) yield return item;
    }

    private static object GetPropertyValue(object source, string propertyName)
    {
        if (source == null) return null;
        try { return source.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public)?.GetValue(source, null); }
        catch { return null; }
    }

    private XElement CreateSignal(EventModel eventModel)
    {
        return new XElement("packagedElement",
            new XAttribute(xmi + "type", "uml:Signal"),
            new XAttribute(xmi + "id", eventModel.Guid),
            new XAttribute("name", eventModel.Name),
            new XAttribute("visibility", "public"));
    }

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

    private static void AddOwnedComment(XElement umlElement, string comment)
    {
        if (!string.IsNullOrWhiteSpace(comment))
            umlElement.Add(new XElement("ownedComment",
                new XAttribute(xmi + "type", "uml:Comment"),
                new XAttribute(xmi + "id", CreateEaId()),
                new XElement("body", comment)));
    }

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

    private static string CreateEaId()
    {
        string guid = System.Guid.NewGuid().ToString("N").ToUpperInvariant();
        return $"EAID_{guid.Substring(0, 8)}_{guid.Substring(8, 4)}_{guid.Substring(12, 4)}_{guid.Substring(16, 4)}_{guid.Substring(20, 12)}";
    }

    private static IEnumerable<XElement> BuildStereotypeApplications(IEnumerable<CatalogModel> catalogs, IEnumerable<ClassModel> classes)
    {
        return catalogs.Select(catalog => new XElement(profile + "catalog", new XAttribute("base_Component", catalog.Guid)))
            .Concat(classes.Select(cls => new XElement(profile + "type", new XAttribute("base_Class", cls.Guid))))
            .Concat(catalogs.SelectMany(catalog => catalog.Events.Select(eventModel =>
                new XElement(profile + "event", new XAttribute("base_Signal", eventModel.Guid)))))
            .Concat(catalogs.SelectMany(catalog => catalog.Settings.Select(setting =>
                CreateProfileTag("Component", catalog.Guid, setting.Key, setting.Value))));
    }

    private static XElement CreateProfileTag(string baseType, string elementId, string tagName, string tagValue)
    {
        string profileName = System.Xml.XmlConvert.EncodeLocalName(tagName.Replace(' ', '_'));
        return new XElement(profile + profileName,
            new XAttribute("base_" + baseType, elementId),
            new XAttribute("__EAStereoName", tagName),
            new XAttribute(profileName, tagValue ?? string.Empty));
    }

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