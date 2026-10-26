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
    /// Дополнительные шумовые пространства имён, отсекаемые только при CleanRagNoise == true.
    /// Это внутренние настройки конфигуратора, сериализаторы пакетов обмена и слой
    /// совместимости: прикладного API, пригодного для генерации кода макроязыка, в них нет.
    /// </summary>
    private static readonly string[] AdditionalNoiseNamespaces = {
        "TFlex.DOCs.Model.Configuration.ControlSettings",
        "TFlex.DOCs.Model.DataExchange.Settings.Serialization",
        "TFlex.DOCs.Model.Configuration.Compatibility"
    };

    /// <summary>
    /// Курируемая карта сценариев генерации модели данных — «точек входа» API.
    ///
    /// Причина появления секции: методы и типы, собирающие модель данных
    /// (CreateReferenceBuilder, ClassObjectBuilder и т.п.), описываются в XML-документации
    /// метафорами «Создает экземпляр класса для создания типа» и не содержат ни слова
    /// «справочник». Векторный поиск по живому запросу «Создать справочник» такие записи
    /// не находит: их приходилось искать точной строкой сигнатуры.
    /// Секция перечисляет сценарий и точную пару «тип + метод», что даёт RAG
    /// лексический и семантический якорь на естественных формулировках задачи.
    /// </summary>
    private static readonly string[] EntryPointScenarios = {
        "## Сценарии генерации модели данных (точки входа)",
        "",
        "Типовые задачи генерации и изменения модели данных и соответствующие им точки входа в API. Если запрос звучит как «создать справочник», «создать тип объектов», «добавить параметр» — начинай именно с этих членов.",
        "",
        "- **Создать справочник:** `TFlex.DOCs.Model.ReferenceCatalog.CreateReferenceBuilder(ReferenceCatalogFolder)` → настроить `ReferenceBuilder` → `ReferenceBuilder.Save(bool)`. Ключевые слова: создать справочник, новый справочник, генерация справочника, create reference.",
        "- **Создать папку справочников:** `TFlex.DOCs.Model.ReferenceCatalog.CreateFolder(string, ReferenceCatalogFolder)`, `ReferenceCatalog.MoveFolder(...)`.",
        "- **Найти существующий справочник:** `ReferenceCatalog.Find(string)`, `ReferenceCatalog.FindReference(string)`, `ReferenceCatalog.FindFolderByName(string)`.",
        "- **Создать тип объектов:** `new TFlex.DOCs.Model.Structure.Builders.ClassObjectBuilder(ClassTree)` → `ClassObjectBuilder.Save()`. Ключевые слова: создать тип объектов, новый тип объектов, класс объектов, create class object, type builder.",
        "- **Создать производный тип объектов:** `new ClassObjectBuilder(ClassTree, ClassObject baseType)` — тип, порождённый от указанного базового типа.",
        "- **Изменить существующий тип объектов:** `new ClassObjectBuilder(ClassTree, ClassObject existingType)` → изменить свойства → `Save()`.",
        "- **Создать группу параметров:** `new TFlex.DOCs.Model.Structure.Builders.ParameterGroupBuilder(ClassObjectBuilder)`. Ключевые слова: создать группу параметров, группа параметров, create parameter group.",
        "- **Создать параметр справочника:** `TFlex.DOCs.Model.Structure.Builders.ParameterInfoBuildersFactory.CreateParameterInfoBuilder(...)`, `ParameterInfoBuilder`.",
        "- **Задать условие уникальности:** `new TFlex.DOCs.Model.Structure.Builders.UniqueIndexBuilder(ClassObjectBuilder)`. Ключевые слова: условие уникальности, уникальный индекс, unique index.",
        "- **Создать связь (ссылочное свойство) типа объектов:** `new TFlex.DOCs.Model.Structure.Builders.LinkGroupBuilder(ParameterGroup)`.",
        "- **Ключевые слова для поиска сценариев:** создать, создание, добавить, новый, конструктор, сформировать, настроить, изменить, справочник, тип объектов, параметр, builder, factory, create, add, new, build, generate.",
        ""
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
        sb.AppendLine("> Используй указанные `Namespace` для добавления в `using`. Строка `**Уровень API:**` отделяет прикладной Macro DSL (доступно внутри MacroProvider) от низкоуровневого Core Open API; пометка `Core Open API (генерация модели данных)` — это типы-конструкторы (Builder/Factory), через которые собирается модель данных. Пометка [RU: …] — русскоязычный алиас метода или свойства, найденный зеркалированием по идентичному тексту `<summary>` в XML-документации; [RU only] — член, существующий только в русскоязычном алиасе; [унаследовано] — член, полученный от базового класса коллекции макромодели. Каждый метод выводится отдельной секцией `#### [Method]`, каждый конструктор — секцией `#### [Constructor]`; в чанке доступны строки `**Алиасы:**`, `**RU only**`, `**Async**`, `**Возвращает:**` (выводится только когда текст `<returns>` описывает результат, а не пересказывает summary), `**Назначение:**`, `**Ключевые слова:**`, `**Как получить:**`, `**Как получить (из кода):**`, `**Прямое создание:**` и `**Унаследовано от:**`.\n");

        // Курируемые сценарии генерации модели данных — раньше говорящий запрос
        // «Создать справочник» / «Создать тип объекта» не имел ни одной лексической точки
        // опоры в справочнике: описания builder-типов сформулированы через «создает экземпляр
        // класса», и векторный поиск не выводил их в топ.
        foreach (var scenarioLine in EntryPointScenarios)
            sb.AppendLine(scenarioLine);

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

                foreach (var type in types)
                {
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
                    string apiLevel = IsMacroDslType(type)
                        ? "Macro DSL (доступно внутри MacroProvider)"
                        : IsBuilderRelatedType(type)
                            ? "Core Open API (генерация модели данных)"
                            : "Core Open API";
                    sb.AppendLine($"- **Уровень API:** {apiLevel}");

                    if (!string.IsNullOrEmpty(typeSummary))
                        sb.AppendLine($"- **Описание:** {typeSummary}");

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
                                sb.AppendLine($"- **Ключевые слова:** {keywordLine}");

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

                Console.WriteLine($"[Успешно] {dllName}: классов: {addedForDll}");
                totalTypes += addedForDll;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Ошибка] {dllName}: {ex.Message}");
            }
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
            case "ParameterGroupBuilder": return "группа параметров справочника";
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
            case "ParameterGroupBuilder": return "группа параметров справочника";
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