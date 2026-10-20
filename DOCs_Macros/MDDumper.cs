// Макрос оставлен в глобальном пространстве имен: загрузчик T-FLEX DOCs находит
// MacroProvider по типу сборки, а не по имени пространства имен.
//
// Автономный макрос выгрузки модели данных одного справочника в Markdown.
// Вся работа ведётся только через прямые свойства Open API TFlex.DOCs.Model:
//   ReferenceInfo               — сам справочник (Guid, Id, HierarchyType, Classes, Description);
//   ParameterGroupCollection    — структурные таблицы справочника (GetAllGroups);
//   ParameterGroup              — группа параметров либо группа связи (IsLinkGroup, LinkType);
//   ParameterInfo / ParameterType — параметры групп и их типы данных;
//   ClassObject / ClassObjectCollection — дерево типов, входимость и привязка групп.
// Для подчинённых Списков объектов (1:N, Type == ParameterGroupType.TableOneToMany)
// дополнительно используются их собственные члены Open API:
//   ParameterGroup.Classes / ClassTree.AllClasses   — дерево типов строк списка;
//   ParameterGroup.Parameters                       — параметры строк списка;
//   ParameterGroup.GetLinks()                       — связи строк списка с другими справочниками;
//   ParameterGroup.TableName                        — физическая таблица строк в БД.
// Рефлексия и недокументированные члены API не используются.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TFlex.DOCs.Model;
using TFlex.DOCs.Model.Classes;
using TFlex.DOCs.Model.Macros;
using TFlex.DOCs.Model.Macros.ObjectModel;
using TFlex.DOCs.Model.Structure;

/// <summary>
/// Автономный макрос T-FLEX DOCs для полной выгрузки модели данных выбранного справочника
/// в структурированном Markdown-документе с указанием GUID всех сущностей.
/// Предназначен для передачи в LLM/ИИ-агенты при генерации специализированных макросов.
/// </summary>
public class ReferenceModelExtractorMacro : MacroProvider
{
    /// <summary>
    /// Провайдер макроса, через который вызываются штатные диалоги и сообщения DOCs.
    /// Совпадает с самим макросом, но позволяет единообразно обращаться к API из любого метода.
    /// </summary>
    private readonly MacroProvider provider;

    /// <summary>Создаёт экземпляр макроса с контекстом, переданным загрузчиком T-FLEX DOCs.</summary>
    /// <param name="context">Контекст выполнения макроса и активное подключение к DOCs.</param>
    public ReferenceModelExtractorMacro(MacroContext context) : base(context)
    {
        provider = this;
    }

    /// <summary>Точка входа макроса: запускает диалог выбора справочника и генерацию отчёта.</summary>
    public override void Run()
    {
        try
        {
            ReferenceInfo reference = SelectReference();
            if (reference == null) return;

            string report = BuildMarkdownReport(reference);
            SaveReport(report, reference.Name);
        }
        catch (Exception ex)
        {
            provider.Сообщение("Ошибка", $"При выгрузке модели данных произошла ошибка:\n{ex.Message}");
        }
    }

    /// <summary>
    /// Отображает диалог выбора справочника через штатный механизм DOCs.
    /// </summary>
    /// <returns>Выбранный <see cref="ReferenceInfo"/> либо null при отмене ввода.</returns>
    private ReferenceInfo SelectReference()
    {
        ДиалогВвода dialog = provider.СоздатьДиалогВвода("Выбор справочника для выгрузки модели данных");
        dialog.Ширина = 560;
        dialog.Высота = 320;
        const string key = "Справочник";

        // Штатный контрол выбора справочника; isRequired = true запрещает подтверждение пустого выбора.
        dialog.ДобавитьВыборСправочника(key, null, true);

        if (!dialog.Показать()) return null;

        object value = dialog.Значение(key);
        if (value is ReferenceInfo info) return info;

        // Контрол может вернуть только имя справочника — тогда ищем его в каталоге платформы.
        string selectedName = Convert.ToString(value);
        if (string.IsNullOrWhiteSpace(selectedName)) return null;

        return Context.Connection.ReferenceCatalog.Find(selectedName);
    }

    /// <summary>
    /// Формирует полный Markdown-отчёт по модели данных справочника.
    /// Отчёт содержит 7 разделов: общие сведения и настройки, группы параметров и связи,
    /// детальный состав параметров, дерево типов, правила входимости, матрицу подключения
    /// групп к типам и подчинённые Списки объектов (1:N) с их опциями, типами строк,
    /// параметрами строк и связями строк.
    /// </summary>
    /// <param name="reference">Выбранный справочник T-FLEX DOCs.</param>
    /// <returns>Сформированный Markdown-текст.</returns>
    private string BuildMarkdownReport(ReferenceInfo reference)
    {
        var sb = new StringBuilder();

        // === Заголовок ===
        sb.AppendLine($"# МОДЕЛЬ ДАННЫХ СПРАВОЧНИКА: {reference.Name}");
        sb.AppendLine();

        // === Раздел 1: Общие сведения и настройки справочника ===
        sb.AppendLine("## 1. Общие сведения и настройки справочника");
        sb.AppendLine($"- **Наименование:** {reference.Name}");
        sb.AppendLine($"- **GUID справочника:** `{reference.Guid}`");
        sb.AppendLine($"- **ID справочника:** {reference.Id}");
        sb.AppendLine($"- **Базовая иерархия:** {reference.HierarchyType}");
        sb.AppendLine();

        // 22 настройки структуры через AddStructureSettings-паттерн
        sb.AppendLine("### Опции структуры (22 ключевые настройки):");
        sb.AppendLine("| Настройка | Значение |");
        sb.AppendLine("| :--- | :--- |");
        var settings = new List<KeyValuePair<string, string>>();
        AddStructureSettings(settings, reference.Description);
        foreach (var setting in settings)
        {
            sb.AppendLine($"| {setting.Key} | {setting.Value} |");
        }
        sb.AppendLine();

        // === Раздел 2: Группы параметров и связи ===
        // GetAllGroups() возвращает все вложенные структурные таблицы справочника, включая
        // служебные связи, созданные внутри подчинённых списков объектов (TableOneToMany).
        // В таблице Раздела 2 показываются только элементы, относящиеся к шапке справочника:
        //   - корневая группа описания и группы параметров 1:1 (TableOneToOne);
        //   - сами подчинённые списки объектов (TableOneToMany) — как отдельная строка обзора;
        //   - таблица подключения сложной иерархии (HierarchyTable);
        //   - связи, принадлежащие непосредственно шапке справочника (Type = Link).
        // Связи строк подчинённых списков отсекаются: их владельцем (MasterGroup) является
        // список объектов, поэтому такие связи выгружаются исключительно в Разделе 7
        // в контексте своего списка строк.
        sb.AppendLine("## 2. Группы параметров и связи");
        sb.AppendLine("| Имя группы | GUID группы | Тип группы | Целевой справочник | GUID цели | Кратность (LinkType) |");
        sb.AppendLine("| :--- | :--- | :--- | :--- | :--- | :--- |");

        ParameterGroupCollection allGroups = reference.Description?.GetAllGroups();
        if (allGroups != null)
        {
            foreach (ParameterGroup group in allGroups)
            {
                if (group == null) continue;

                // Исключаем связи, принадлежащие подчинённым спискам объектов (1:N):
                // их владельцем (MasterGroup) является список объектов TableOneToMany.
                // Если MasterGroup == null, связь считается относящейся к шапке справочника
                // и остаётся в таблице. IsTableOneToMany эквивалентен проверке
                // Type == ParameterGroupType.TableOneToMany.
                if (group.IsLinkGroup && group.MasterGroup != null &&
                    (group.MasterGroup.Type == ParameterGroupType.TableOneToMany || group.MasterGroup.IsTableOneToMany))
                {
                    continue;
                }

                string groupType = group.Type != null ? Convert.ToString(group.Type.Name) : "";
                string targetRefName = "";
                string targetRefGuid = "";
                string multiplicity = "";

                if (group.IsLinkGroup)
                {
                    // Ведомая группа связи держит ссылку на целевой справочник (ReferenceInfo).
                    ReferenceInfo linkedRef = group.SlaveGroup?.ReferenceInfo;
                    targetRefName = linkedRef != null ? linkedRef.Name : "";
                    targetRefGuid = linkedRef != null ? linkedRef.Guid.ToString() : "";
                    multiplicity = DescribeMultiplicity(group);
                }
                else if (group.IsHierarchyTable)
                {
                    groupType = "HierarchyTable (группа подключения)";
                }

                sb.AppendLine($"| {Md(group.Name)} | `{group.Guid}` | {Md(groupType)} | {Md(targetRefName)} | {Code(targetRefGuid)} | {Md(multiplicity)} |");
            }
        }
        sb.AppendLine();

        // === Раздел 3: Детальный состав параметров по группам ===
        sb.AppendLine("## 3. Детальный состав параметров по группам");

        if (allGroups != null)
        {
            foreach (ParameterGroup group in allGroups)
            {
                // В Разделе 3 выгружаются только параметры шапки справочника: корневая группа
                // (ParameterGroupType.Reference), группы параметров 1:1 (TableOneToOne) и таблица
                // подключения сложной иерархии (IsHierarchyTable).
                // Связи (Link) не имеют собственных параметров: состав связи полностью определяется
                // целевым справочником, поэтому блок параметров для них не формируется.
                // Подчинённые списки объектов (TableOneToMany) отсекаются здесь, так как их
                // собственные параметры строк выводятся вместе с типами строк и связями строк
                // в специализированном Разделе 7 — иначе возникает дублирование выгрузки.
                // IsTableOneToMany эквивалентен Type == ParameterGroupType.TableOneToMany
                // (проверено по IL: get_IsTableOneToMany вызывает ParameterGroupType.op_Equality),
                // поэтому набор групп здесь совпадает с набором в Разделе 7 и ни одна группа
                // не теряется при выгрузке.
                if (group == null || group.IsLinkGroup || group.Type == ParameterGroupType.TableOneToMany || group.IsTableOneToMany)
                    continue;

                // Таблица подключения сложной иерархии — служебная группа, помечается явно.
                string groupHeader = group.IsHierarchyTable
                    ? $"{group.Name} (группа подключения)"
                    : group.Name;
                sb.AppendLine($"### Группа: {Md(groupHeader)} (`{group.Guid}`)");
                sb.AppendLine("| Имя параметра | GUID параметра | Тип данных (C#) | Обязательный | Видимый |");
                sb.AppendLine("| :--- | :--- | :--- | :--- | :--- |");

                if (group.Parameters == null)
                {
                    sb.AppendLine("| _(параметры отсутствуют)_ | — | — | — | — |");
                    sb.AppendLine();
                    continue;
                }

                foreach (ParameterInfo parameter in group.Parameters)
                {
                    if (parameter == null) continue;

                    // В одной ячейке даётся и C#-тип (для генерации кода макроса), и исходное
                    // имя типа в модели DOCs (для сверки с интерфейсом настройки справочника).
                    string docsType = parameter.Type != null ? Convert.ToString(parameter.Type.Name) : "неизвестный";
                    string typeCell = $"{MapType(parameter.Type)} (DOCs: {Md(docsType)})";
                    sb.AppendLine($"| {Md(parameter.Name)} | `{parameter.Guid}` | {typeCell} | {BoolText(parameter.IsRequired)} | {BoolText(parameter.IsVisible)} |");
                }
                sb.AppendLine();
            }
        }

        // === Раздел 4: Типы объектов справочника (Дерево наследования) ===
        sb.AppendLine("## 4. Типы объектов справочника (Дерево наследования)");
        sb.AppendLine("| Имя типа | GUID типа | Абстрактный | Базовый тип (BaseClass) | GUID базового типа |");
        sb.AppendLine("| :--- | :--- | :--- | :--- | :--- |");

        ClassObjectCollection allClasses = reference.Classes?.AllClasses;
        if (allClasses != null)
        {
            foreach (ClassObject cls in allClasses)
            {
                if (cls == null) continue;

                string baseName = cls.Base?.Name ?? "(корневой)";
                string baseGuid = cls.Base?.Guid.ToString() ?? "—";
                sb.AppendLine($"| {Md(cls.Name)} | `{cls.Guid}` | {BoolText(cls.IsAbstract)} | {Md(baseName)} | {baseGuid} |");
            }
        }
        sb.AppendLine();

        // === Раздел 5: Правила входимости типов ===
        sb.AppendLine("## 5. Правила входимости типов (Иерархия объектов)");

        if (allClasses != null)
        {
            foreach (ClassObject cls in allClasses)
            {
                if (cls == null) continue;

                sb.AppendLine($"- **Тип {Md(cls.Name)} (`{cls.Guid}`):**");

                // Родительские типы: GetParentObjectClasses() возвращает типы, которые могут
                // выступать родителями для данного типа в иерархии объектов справочника.
                ClassObjectCollection parentClasses = cls.GetParentObjectClasses();
                if (parentClasses != null && parentClasses.Count > 0)
                {
                    string parents = string.Join(", ", parentClasses
                        .Where(p => p != null)
                        .Select(p => $"{Md(p.Name)} (`{p.Guid}`)"));
                    sb.AppendLine($"  - *Разрешённые родительские типы:* {parents}");
                }
                else
                {
                    sb.AppendLine("  - *Разрешённые родительские типы:* (нет ограничений)");
                }

                // Дочерние типы: ChildObjectClasses — типы, которые могут создаваться
                // непосредственно под объектами данного типа.
                ClassObjectCollection childClasses = cls.ChildObjectClasses;
                if (childClasses != null && childClasses.Count > 0)
                {
                    string children = string.Join(", ", childClasses
                        .Where(c => c != null)
                        .Select(c => $"{Md(c.Name)} (`{c.Guid}`)"));
                    sb.AppendLine($"  - *Разрешённые дочерние типы:* {children}");
                }
                else
                {
                    sb.AppendLine("  - *Разрешённые дочерние типы:* (нет)");
                }
            }
        }
        sb.AppendLine();

        // === Раздел 6: Матрица подключения групп параметров и связей к типам ===
        sb.AppendLine("## 6. Матрица подключения групп параметров и связей к типам");

        // Карта «группа -> типы, которым она доступна с учётом наследования» строится один раз,
        // чтобы не вызывать reference.Classes.GetParameterGroupClasses для каждого типа отдельно.
        // Карта переиспользуется и в Разделе 7 для определения типов шапки, к которым подключён
        // подчинённый список объектов (1:N).
        Dictionary<Guid, HashSet<Guid>> appliedClasses = BuildGroupApplicationMap(reference, allGroups);

        if (allClasses != null)
        {
            foreach (ClassObject cls in allClasses)
            {
                if (cls == null) continue;

                sb.AppendLine($"- **Тип {Md(cls.Name)} (`{cls.Guid}`):**");

                // cls.ParameterGroups — группы, явно подключённые к типу (без учёта наследования).
                ParameterGroupCollection classGroups = cls.ParameterGroups;
                var ownGuids = new HashSet<Guid>();
                if (classGroups != null)
                {
                    foreach (ParameterGroup ownGroup in classGroups)
                    {
                        if (ownGroup != null) ownGuids.Add(ownGroup.Guid);
                    }
                }

                if (ownGuids.Count > 0)
                {
                    string groups = string.Join(", ", classGroups
                        .Where(g => g != null)
                        .Select(g => $"{Md(g.Name)} (`{g.Guid}`)"));
                    sb.AppendLine($"  - *Подключенные группы/связи:* {groups}");
                }
                else
                {
                    sb.AppendLine("  - *Подключенные группы/связи:* (нет)");
                }

                // Унаследованные группы: подключены к базовым типам и доступны данному типу
                // через механизм наследования (проверено GetParameterGroupClasses(group, true)).
                string inherited = DescribeInheritedGroups(appliedClasses, ownGuids, cls.Guid, allGroups);
                if (!string.IsNullOrEmpty(inherited))
                {
                    sb.AppendLine($"  - *Унаследованные группы:* {inherited}");
                }
            }
        }
        sb.AppendLine();

        // === Раздел 7: Подчинённые Списки объектов (1:N) ===
        AppendObjectListsSection(sb, allGroups, allClasses, appliedClasses);

        return sb.ToString();
    }

    /// <summary>
    /// Формирует Раздел 7 отчёта — подробное описание подчинённых Списков объектов (1:N).
    /// Список объектов — это структурная таблица DOCs (<c>ParameterGroupType.TableOneToMany</c>),
    /// существующая только в контексте объекта-шапки. В отличие от группы параметров 1:1, список
    /// имеет собственное дерево типов строк (<c>group.Classes</c>), собственные параметры строк
    /// (<c>group.Parameters</c>) и собственные связи строк с другими справочниками
    /// (<c>group.GetLinks()</c>).
    /// </summary>
    /// <param name="sb">Построитель текста отчёта.</param>
    /// <param name="allGroups">Все структурные группы справочника (источник списков объектов).</param>
    /// <param name="allClasses">Все типы объектов справочника (типы шапки).</param>
    /// <param name="appliedClasses">Карта «GUID группы — GUID типов шапки, к которым группа применима».</param>
    private static void AppendObjectListsSection(
        StringBuilder sb,
        ParameterGroupCollection allGroups,
        ClassObjectCollection allClasses,
        Dictionary<Guid, HashSet<Guid>> appliedClasses)
    {
        sb.AppendLine("## 7. Подчинённые Списки объектов (1:N)");

        var objectLists = allGroups?
            .Where(group => group != null && !group.IsLinkGroup && group.Type == ParameterGroupType.TableOneToMany)
            .ToList() ?? new List<ParameterGroup>();

        if (objectLists.Count == 0)
        {
            sb.AppendLine("_(в справочнике нет подчинённых списков объектов 1:N)_");
            return;
        }

        int listIndex = 0;
        foreach (ParameterGroup listGroup in objectLists)
        {
            listIndex++;
            sb.AppendLine();
            sb.AppendLine($"### 7.{listIndex}. Список объектов: {Md(listGroup.Name)} (`{listGroup.Guid}`)");
            sb.AppendLine($"- **Имя таблицы в БД:** `{Md(listGroup.TableName)}`");

            // Типы шапки определяются по уже построенной карте привязок групп к типам.
            sb.AppendLine($"- **Подключен к типам шапки:** {DescribeHostClasses(listGroup, allClasses, appliedClasses)}");
            sb.AppendLine();

            // Опции структуры списка объектов читаются тем же набором из 22 настроек,
            // что и настройки справочника: у структурной таблицы они собственные.
            sb.AppendLine("#### Опции структуры списка:");
            sb.AppendLine("| Настройка | Значение |");
            sb.AppendLine("| :--- | :--- |");
            var listSettings = new List<KeyValuePair<string, string>>();
            AddStructureSettings(listSettings, listGroup);
            foreach (var setting in listSettings)
            {
                sb.AppendLine($"| {setting.Key} | {setting.Value} |");
            }
            sb.AppendLine();

            AppendObjectListRowClasses(sb, listGroup);
            sb.AppendLine();

            AppendObjectListRowParameters(sb, listGroup);
            sb.AppendLine();

            AppendObjectListRowLinks(sb, listGroup);
        }
    }

    /// <summary>
    /// Определяет типы шапки, к которым подключён подчинённый список объектов.
    /// Привязка берётся из карты <paramref name="appliedClasses"/>, построенной через
    /// <c>reference.Classes.GetParameterGroupClasses(group, true)</c>.
    /// </summary>
    /// <param name="listGroup">Группа — подчинённый список объектов (1:N).</param>
    /// <param name="allClasses">Все типы объектов справочника (типы шапки).</param>
    /// <param name="appliedClasses">Карта «GUID группы — GUID типов, к которым группа применима».</param>
    /// <returns>Список «Имя (`GUID`)» либо пояснение об отсутствии явных привязок.</returns>
    private static string DescribeHostClasses(
        ParameterGroup listGroup,
        ClassObjectCollection allClasses,
        Dictionary<Guid, HashSet<Guid>> appliedClasses)
    {
        HashSet<Guid> applied;
        if (appliedClasses == null || allClasses == null ||
            !appliedClasses.TryGetValue(listGroup.Guid, out applied) || applied.Count == 0)
        {
            return "(не подключен / доступен всем)";
        }

        var hostNames = allClasses
            .Where(cls => cls != null && applied.Contains(cls.Guid))
            .Select(cls => $"{Md(cls.Name)} (`{cls.Guid}`)")
            .ToList();

        return hostNames.Count == 0 ? "(не подключен / доступен всем)" : string.Join(", ", hostNames);
    }

    /// <summary>
    /// Безопасно читает коллекцию типов строк подчинённого списка объектов.
    /// Дерево типов строк (<c>ParameterGroup.Classes</c> → <c>ClassTree</c>) хранится отдельно
    /// от дерева типов шапки и в редких случаях (повреждённая структура, отключённая таблица)
    /// может быть недоступно. Ошибка чтения не должна прерывать формирование всего отчёта,
    /// поэтому исключение подавляется и возвращается <c>null</c>.
    /// </summary>
    /// <param name="listGroup">Группа — подчинённый список объектов (1:N).</param>
    /// <returns>Коллекция типов строк либо <c>null</c>, если она недоступна.</returns>
    private static ClassObjectCollection TryGetRowClasses(ParameterGroup listGroup)
    {
        try
        {
            return listGroup.Classes?.AllClasses;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Выводит таблицу типов строк подчинённого списка объектов (собственное дерево
    /// наследования строк, независимое от дерева типов шапки).
    /// </summary>
    /// <param name="sb">Построитель текста отчёта.</param>
    /// <param name="listGroup">Группа — подчинённый список объектов (1:N).</param>
    private static void AppendObjectListRowClasses(StringBuilder sb, ParameterGroup listGroup)
    {
        sb.AppendLine("#### Типы строк списка:");
        sb.AppendLine("| Имя типа строки | GUID типа строки | Абстрактный | Базовый тип | GUID базового типа |");
        sb.AppendLine("| :--- | :--- | :--- | :--- | :--- |");

        ClassObjectCollection rowClasses = TryGetRowClasses(listGroup);
        if (rowClasses == null || rowClasses.Count == 0)
        {
            sb.AppendLine("| _(типы строк отсутствуют)_ | — | — | — | — |");
            return;
        }

        foreach (ClassObject rowClass in rowClasses)
        {
            if (rowClass == null) continue;

            string baseName = rowClass.Base?.Name ?? "(корневой)";
            string baseGuid = rowClass.Base?.Guid.ToString() ?? "—";
            sb.AppendLine($"| {Md(rowClass.Name)} | `{rowClass.Guid}` | {BoolText(rowClass.IsAbstract)} | {Md(baseName)} | {baseGuid} |");
        }
    }

    /// <summary>
    /// Выводит таблицу параметров строк подчинённого списка объектов.
    /// В отличие от связей, строки списка имеют собственный состав параметров.
    /// </summary>
    /// <param name="sb">Построитель текста отчёта.</param>
    /// <param name="listGroup">Группа — подчинённый список объектов (1:N).</param>
    private static void AppendObjectListRowParameters(StringBuilder sb, ParameterGroup listGroup)
    {
        sb.AppendLine("#### Параметры строк списка:");
        sb.AppendLine("| Имя параметра | GUID параметра | Тип данных (C#) | Обязательный | Видимый |");
        sb.AppendLine("| :--- | :--- | :--- | :--- | :--- |");

        ParameterInfoCollection rowParameters = listGroup.Parameters;
        if (rowParameters == null || rowParameters.Count == 0)
        {
            sb.AppendLine("| _(параметры отсутствуют)_ | — | — | — | — |");
            return;
        }

        foreach (ParameterInfo parameter in rowParameters)
        {
            if (parameter == null) continue;

            // В одной ячейке даётся и C#-тип (для генерации кода макроса), и исходное
            // имя типа в модели DOCs (для сверки с интерфейсом настройки списка).
            string docsType = parameter.Type != null ? Convert.ToString(parameter.Type.Name) : "неизвестный";
            string typeCell = $"{MapType(parameter.Type)} (DOCs: {Md(docsType)})";
            sb.AppendLine($"| {Md(parameter.Name)} | `{parameter.Guid}` | {typeCell} | {BoolText(parameter.IsRequired)} | {BoolText(parameter.IsVisible)} |");
        }
    }

    /// <summary>
    /// Выводит таблицу связей строк подчинённого списка объектов с другими справочниками.
    /// Собственные параметры связей не выгружаются: состав связи задаётся целевым справочником.
    /// </summary>
    /// <param name="sb">Построитель текста отчёта.</param>
    /// <param name="listGroup">Группа — подчинённый список объектов (1:N).</param>
    private static void AppendObjectListRowLinks(StringBuilder sb, ParameterGroup listGroup)
    {
        sb.AppendLine("#### Связи строк списка:");

        ParameterGroupCollection listLinks = listGroup.GetLinks();
        if (listLinks == null || listLinks.Count == 0)
        {
            sb.AppendLine("_(связи строк отсутствуют)_");
            return;
        }

        sb.AppendLine("| Имя связи | GUID связи | Целевой справочник | GUID цели | Кратность (LinkType) | Подключена к типам строк |");
        sb.AppendLine("| :--- | :--- | :--- | :--- | :--- | :--- |");

        foreach (ParameterGroup link in listLinks)
        {
            if (link == null) continue;

            // Ведомая группа связи держит ссылку на целевой справочник (ReferenceInfo).
            ReferenceInfo linkedRef = link.SlaveGroup?.ReferenceInfo ?? link.ReferenceInfo;
            string targetName = linkedRef != null ? linkedRef.Name : "";
            string targetGuid = linkedRef != null ? linkedRef.Guid.ToString() : "";

            sb.AppendLine(
                $"| {Md(link.Name)} | `{link.Guid}` | {Md(targetName)} | {Code(targetGuid)} | " +
                $"{Md(DescribeMultiplicity(link))} | {DescribeLinkRowClasses(link, listGroup)} |");
        }
    }

    /// <summary>
    /// Определяет типы строк списка объектов, к которым явно подключена связь.
    /// Тип строки считается подключённым, если <c>ClassObject.ParameterGroups</c> содержит
    /// группу связи (<c>link.Guid</c>).
    /// </summary>
    /// <param name="link">Группа связи строк списка объектов.</param>
    /// <param name="listGroup">Группа — подчинённый список объектов (1:N).</param>
    /// <returns>Список «Имя (`GUID`)» либо «(все типы строк)», если явных привязок нет.</returns>
    private static string DescribeLinkRowClasses(ParameterGroup link, ParameterGroup listGroup)
    {
        ClassObjectCollection rowClasses = TryGetRowClasses(listGroup);
        if (rowClasses == null || rowClasses.Count == 0) return "(все типы строк)";

        var linkedNames = new List<string>();
        foreach (ClassObject rowClass in rowClasses)
        {
            if (rowClass == null) continue;

            ParameterGroupCollection rowClassGroups = rowClass.ParameterGroups;
            if (rowClassGroups == null) continue;

            foreach (ParameterGroup rowClassGroup in rowClassGroups)
            {
                if (rowClassGroup != null && rowClassGroup.Guid == link.Guid)
                {
                    linkedNames.Add($"{Md(rowClass.Name)} (`{rowClass.Guid}`)");
                    break;
                }
            }
        }

        return linkedNames.Count == 0 ? "(все типы строк)" : string.Join(", ", linkedNames);
    }

    /// <summary>
    /// Строит карту «группа параметров -> множество GUID типов, которым группа доступна».
    /// Доступность вычисляется с учётом наследования через
    /// <see cref="ClassTree.GetParameterGroupClasses(ParameterGroup, bool)"/> с параметром true.
    /// Карта строится однократно, чтобы не выполнять запрос по каждому типу отдельно.
    /// </summary>
    /// <param name="reference">Справочник, владеющий деревом типов и группами.</param>
    /// <param name="allGroups">Все структурные группы справочника.</param>
    /// <returns>Словарь «GUID группы — множество GUID типов, где группа применима».</returns>
    private static Dictionary<Guid, HashSet<Guid>> BuildGroupApplicationMap(ReferenceInfo reference, ParameterGroupCollection allGroups)
    {
        var map = new Dictionary<Guid, HashSet<Guid>>();
        if (reference?.Classes == null || allGroups == null) return map;

        foreach (ParameterGroup group in allGroups)
        {
            if (group == null) continue;

            var classGuids = new HashSet<Guid>();
            try
            {
                ClassObjectCollection applied = reference.Classes.GetParameterGroupClasses(group, true);
                if (applied != null)
                {
                    foreach (ClassObject appliedClass in applied)
                    {
                        if (appliedClass != null) classGuids.Add(appliedClass.Guid);
                    }
                }
            }
            catch
            {
                // Группа может быть недоступна для запроса привязок (например, таблица подключения
                // сложной иерархии) — такая группа просто не попадёт в карту наследования.
            }

            map[group.Guid] = classGuids;
        }

        return map;
    }

    /// <summary>
    /// Формирует список групп, унаследованных типом от базовых типов.
    /// Группа считается унаследованной, если она применима к типу с учётом наследования,
    /// но отсутствует среди групп, явно подключённых к самому типу.
    /// </summary>
    /// <param name="appliedClasses">Карта «GUID группы — типы, где группа применима».</param>
    /// <param name="ownGuids">GUID групп, явно подключённых к типу.</param>
    /// <param name="classGuid">GUID рассматриваемого типа.</param>
    /// <param name="allGroups">Все структурные группы справочника (источник имён и GUID).</param>
    /// <returns>Список унаследованных групп в формате Markdown либо пустая строка.</returns>
    private static string DescribeInheritedGroups(Dictionary<Guid, HashSet<Guid>> appliedClasses, HashSet<Guid> ownGuids, Guid classGuid, ParameterGroupCollection allGroups)
    {
        if (allGroups == null) return "";

        var inherited = new List<string>();
        foreach (ParameterGroup group in allGroups)
        {
            if (group == null || ownGuids.Contains(group.Guid)) continue;

            HashSet<Guid> applied;
            if (appliedClasses.TryGetValue(group.Guid, out applied) && applied.Contains(classGuid))
            {
                inherited.Add($"{Md(group.Name)} (`{group.Guid}`)");
            }
        }

        return string.Join(", ", inherited);
    }

    /// <summary>
    /// Преобразует тип параметра DOCs в тип данных C# для отображения в отчёте.
    /// Использует тот же набор предикатов <see cref="ParameterType"/>, что и XmiSupport.MapType
    /// из XMIgenerator.cs, но возвращает привычное имя типа .NET для генерации кода макроса.
    /// </summary>
    /// <param name="parameterType">Тип значения параметра в модели DOCs.</param>
    /// <returns>Имя типа данных C#: bool, DateTime, int, decimal, double или string.</returns>
    private static string MapType(ParameterType parameterType)
    {
        if (parameterType == null) return "string";
        if (parameterType.IsBoolean) return "bool";
        if (parameterType.IsDateTime) return "DateTime";
        if (parameterType.IsInt) return "int";
        if (parameterType.IsMoney) return "decimal";
        if (parameterType.IsFloat || parameterType.IsNumber) return "double";
        return "string";
    }

    /// <summary>
    /// Вычисляет кратности концов связи по типу связи DOCs (<c>ParameterGroup.LinkType</c>).
    /// Логика перенесена из XmiSupport.GetAssociationMultiplicity файла XMIgenerator.cs,
    /// чтобы макрос оставался полностью автономным и не зависел от других файлов проекта.
    /// </summary>
    /// <param name="group">Группа связи, для которой нужны кратности.</param>
    /// <returns>Строка вида «1..1 → 0..* (LinkType: OneToMany)» либо пустая строка при сбое.</returns>
    private static string DescribeMultiplicity(ParameterGroup group)
    {
        try
        {
            // Первое значение относится к текущему (master) справочнику, второе — к целевому (slave).
            string sourceLower = "1", sourceUpper = "1";
            string targetLower = "1", targetUpper = "1";

            switch (group.LinkType)
            {
                case LinkType.OneToMany:
                    sourceLower = "1"; sourceUpper = "1";
                    targetLower = "0"; targetUpper = "*";
                    break;
                case LinkType.ManyToOne:
                    sourceLower = "0"; sourceUpper = "*";
                    targetLower = "1"; targetUpper = "1";
                    break;
                case LinkType.ManyToMany:
                    sourceLower = "0"; sourceUpper = "*";
                    targetLower = "0"; targetUpper = "*";
                    break;
                case LinkType.OneToOne:
                    sourceLower = "1"; sourceUpper = "1";
                    targetLower = "1"; targetUpper = "1";
                    break;
                default:
                    // Для LinkType.AnyReference/SearchQuery/Undefined кратность не определена.
                    return $"(LinkType: {group.LinkType})";
            }

            string source = FormatMultiplicity(sourceLower, sourceUpper);
            string target = FormatMultiplicity(targetLower, targetUpper);
            return $"{source} → {target} (LinkType: {group.LinkType})";
        }
        catch
        {
            // Кратность не критична для модели данных — при сбое выгрузка продолжается.
            return "";
        }
    }

    /// <summary>Форматирует кратность конца связи в нотации UML: «1», «0..1», «0..*».</summary>
    /// <param name="lower">Нижняя граница кратности.</param>
    /// <param name="upper">Верхняя граница кратности или «*» для неограниченной.</param>
    /// <returns>Строка кратности.</returns>
    private static string FormatMultiplicity(string lower, string upper)
    {
        return lower == upper ? lower : $"{lower}..{upper}";
    }

    /// <summary>
    /// Оборачивает непустой GUID в обратные кавычки, чтобы Markdown не «съедал» значение.
    /// </summary>
    /// <param name="guid">Строковое представление GUID либо пустая строка.</param>
    /// <returns>Строка с GUID в код-разметке либо прочерк.</returns>
    private static string Code(string guid)
    {
        return string.IsNullOrWhiteSpace(guid) ? "—" : $"`{guid}`";
    }

    /// <summary>
    /// Экранирует текст для вставки внутрь ячейки Markdown-таблицы или маркированного списка:
    /// вертикальная черта ломает столбцы таблицы, а перевод строки — саму разметку.
    /// </summary>
    /// <param name="text">Исходное значение из модели DOCs.</param>
    /// <returns>Безопасная для Markdown строка.</returns>
    private static string Md(string text)
    {
        if (string.IsNullOrEmpty(text)) return "";
        return text.Replace("|", "\\|").Replace("\r", " ").Replace("\n", " ").Trim();
    }

    /// <summary>
    /// Читает 22 ключевые настройки структуры справочника из корневой группы параметров.
    /// Паттерн заимствован из XMIgenerator.cs (метод AddStructureSettings).
    /// </summary>
    /// <param name="settings">Список пар «настройка — значение» для заполнения.</param>
    /// <param name="group">Корневая группа параметров справочника (reference.Description).</param>
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

    /// <summary>
    /// Добавляет пару «ключ — значение» в список настроек, преобразуя null в пустую строку.
    /// </summary>
    /// <param name="settings">Список настроек для добавления.</param>
    /// <param name="key">Ключ настройки (человекочитаемое имя).</param>
    /// <param name="value">Значение настройки.</param>
    private static void AddSetting(List<KeyValuePair<string, string>> settings, string key, string value)
    {
        settings.Add(new KeyValuePair<string, string>(key, value ?? ""));
    }

    /// <summary>
    /// Преобразует булево значение в строку "Да" / "Нет" для отображения в отчёте.
    /// </summary>
    /// <param name="value">Булево значение.</param>
    /// <returns>"Да" если true, иначе "Нет".</returns>
    private static string BoolText(bool value)
    {
        return value ? "Да" : "Нет";
    }

    /// <summary>
    /// Сохраняет сформированный отчёт в файл через диалог сохранения DOCs,
    /// копирует текст в буфер обмена и показывает информационное сообщение.
    /// </summary>
    /// <param name="report">Сформированный Markdown-текст отчёта.</param>
    /// <param name="referenceName">Наименование справочника (для имени файла).</param>
    private void SaveReport(string report, string referenceName)
    {
        ДиалогСохраненияФайла saveDialog = provider.СоздатьДиалогСохраненияФайла("Сохранение модели данных справочника");
        saveDialog.Заголовок = "Сохранение модели данных справочника";
        saveDialog.Фильтр = "Markdown (*.md)|*.md|Все файлы (*.*)|*.*";
        saveDialog.РасширениеПоУмолчанию = "md";
        saveDialog.ИмяФайла = MakeDefaultFileName(referenceName);

        if (!saveDialog.Показать()) return;

        string filePath = saveDialog.ИмяФайла;
        File.WriteAllText(filePath, report, new UTF8Encoding(false));

        string clipboardNote = TryCopyToClipboard(report)
            ? "Текст скопирован в буфер обмена для вставки в чат с ИИ."
            : "Скопируйте текст вручную из сохранённого файла.";

        provider.Сообщение("Завершение",
            $"Модель данных справочника \"{referenceName}\" успешно выгружена:\n" +
            $"Файл: {filePath}\n" +
            clipboardNote);
    }

    /// <summary>
    /// Формирует имя файла по умолчанию из наименования справочника, заменяя недопустимые символы.
    /// </summary>
    /// <param name="referenceName">Наименование справочника.</param>
    /// <returns>Имя файла вида «Модель_данных_&lt;справочник&gt;.md».</returns>
    private static string MakeDefaultFileName(string referenceName)
    {
        string safeName = string.IsNullOrWhiteSpace(referenceName) ? "Reference" : referenceName.Trim();
        foreach (char invalid in Path.GetInvalidFileNameChars())
        {
            safeName = safeName.Replace(invalid, '_');
        }

        return $"Модель_данных_{safeName}.md";
    }

    /// <summary>
    /// Копирует сформированный отчёт в буфер обмена Windows.
    /// Вызов обёрнут в try/catch: в серверных сценариях буфер обмена может быть недоступен,
    /// и это не должно прерывать успешно завершённую выгрузку.
    /// </summary>
    /// <param name="report">Markdown-текст отчёта.</param>
    /// <returns>true, если текст попал в буфер обмена.</returns>
    private static bool TryCopyToClipboard(string report)
    {
        try
        {
            System.Windows.Forms.Clipboard.SetText(report);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
