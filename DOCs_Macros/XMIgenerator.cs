using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
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
        public List<ParamModel> Parameters { get; set; } = new List<ParamModel>();
        public List<AssocModel> Associations { get; set; } = new List<AssocModel>();
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

    private static readonly XNamespace xmi = "http://schema.omg.org/spec/XMI/2.1";
    private static readonly XNamespace uml = "http://schema.omg.org/spec/UML/2.1";

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
        dlg.АвтоВыборФлажками = true;
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

        var classesA = BuildClasses(typesA, paramsA);
        var classesB = BuildClasses(typesB, paramsB);
        var allClasses = classesA.Concat(classesB).ToList();
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

        var pkg = new XElement(uml + "Package",
            new XAttribute(xmi + "type", "uml:Package"),
            new XAttribute(xmi + "id", "EAPK_" + System.Guid.NewGuid().ToString("N").ToUpper()),
            new XAttribute("name", $"Классы {refA} и {refB}"),
            new XAttribute("visibility", "public")
        );

        foreach (var cls in allClasses)
        {
            var clsElem = new XElement(uml + "Class",
                new XAttribute(xmi + "type", "uml:Class"),
                new XAttribute(xmi + "id", cls.Guid),
                new XAttribute("name", cls.Name),
                new XAttribute("visibility", "public")
            );

            foreach (var p in cls.Parameters)
                clsElem.Add(CreateProperty(p.Guid, p.Name, p.TypeRef));

            foreach (var a in cls.Associations)
                clsElem.Add(CreateAssocProperty(a.DstPropGuid, a.AssocGuid, a.TargetClassGuid, a.TargetLower, a.TargetUpper));

            pkg.Add(clsElem);
        }

        foreach (var a in allAssocs)
        {
            pkg.Add(new XElement(uml + "Association",
                new XAttribute(xmi + "type", "uml:Association"),
                new XAttribute(xmi + "id", a.AssocGuid),
                new XAttribute("name", a.Name),
                new XAttribute("visibility", "public"),
                new XElement("memberEnd", new XAttribute(xmi + "idref", a.DstPropGuid)),
                new XElement("memberEnd", new XAttribute(xmi + "idref", a.SrcPropGuid)),
                CreateOwnedEnd(a.SrcPropGuid, a.AssocGuid, a.SourceClassGuid, a.SourceLower, a.SourceUpper)
            ));
        }

        var ext = BuildEaExtension(allClasses, allAssocs);

        var doc = new XDocument(
            new XDeclaration("1.0", "windows-1252", "yes"),
            new XElement(xmi + "XMI",
                new XAttribute(XNamespace.Xmlns + "xmi", xmi.NamespaceName),
                new XAttribute(XNamespace.Xmlns + "uml", uml.NamespaceName),
                new XAttribute(xmi + "version", "2.1"),
                new XElement(uml + "Model",
                    new XAttribute(xmi + "type", "uml:Model"),
                    new XAttribute("name", "EA_Model"),
                    new XAttribute("visibility", "public"),
                    pkg
                ),
                ext
            )
        );

        doc.Save(saveDlg.ИмяФайла);
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

    private XElement CreateAssocProperty(string id, string assocId, string targetGuid, string lower, string upper)
    {
        return new XElement("ownedAttribute",
            new XAttribute(xmi + "type", "uml:Property"), new XAttribute(xmi + "id", id),
            new XAttribute("visibility", "public"), new XAttribute("association", assocId),
            new XElement("type", new XAttribute(xmi + "idref", targetGuid)),
            new XElement("lowerValue", new XAttribute(xmi + "type", "uml:LiteralInteger"), new XAttribute("value", lower)),
            new XElement("upperValue", new XAttribute(xmi + "type", upper == "*" ? "uml:LiteralUnlimitedNatural" : "uml:LiteralInteger"), new XAttribute("value", upper))
        );
    }

    private XElement CreateOwnedEnd(string id, string assocId, string sourceGuid, string lower, string upper)
    {
        return new XElement("ownedEnd",
            new XAttribute(xmi + "type", "uml:Property"), new XAttribute(xmi + "id", id),
            new XAttribute("visibility", "public"), new XAttribute("association", assocId),
            new XElement("type", new XAttribute(xmi + "idref", sourceGuid)),
            new XElement("lowerValue", new XAttribute(xmi + "type", "uml:LiteralInteger"), new XAttribute("value", lower)),
            new XElement("upperValue", new XAttribute(xmi + "type", upper == "*" ? "uml:LiteralUnlimitedNatural" : "uml:LiteralInteger"), new XAttribute("value", upper))
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

    private XElement BuildEaExtension(List<ClassModel> classes, List<AssocModel> assocs)
    {
        var ext = new XElement(xmi + "Extension", new XAttribute("extender", "Enterprise Architect"), new XAttribute("extenderID", "6.5"));
        var elems = new XElement("elements");

        foreach (var cls in classes)
        {
            var el = new XElement("element", new XAttribute(xmi + "idref", cls.Guid), new XAttribute(xmi + "type", "uml:Class"), new XAttribute("name", cls.Name));
            if (cls.Parameters.Count > 0)
            {
                var attrs = new XElement("attributes");
                foreach (var p in cls.Parameters)
                    attrs.Add(new XElement("attribute", new XAttribute(xmi + "idref", p.Guid), new XAttribute("name", p.Name), new XElement("properties", new XAttribute("type", p.EaType))));
                el.Add(attrs);
            }
            elems.Add(el);
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
        ext.Add(connectors);
        return ext;
    }

    private List<ClassModel> BuildClasses(ТипОбъекта[] types, List<ParameterInfo> parameters)
    {
        var list = new List<ClassModel>();
        foreach (var t in types)
        {
            var cls = new ClassModel { Name = t.Имя };
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