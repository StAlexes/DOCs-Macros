using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
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
        sb.AppendLine("> Используй указанные `Namespace` для добавления в `using`. Строка `**Уровень API:**` отделяет прикладной Macro DSL (доступно внутри MacroProvider) от низкоуровневого Core Open API. Пометка [RU: …] — русскоязычный алиас метода или свойства, найденный зеркалированием по идентичному тексту `<summary>` в XML-документации; [RU only] — член, существующий только в русскоязычном алиасе; [унаследовано] — член, полученный от базового класса коллекции макромодели. Методы описываются отдельными секциями `#### [Method]` со строками `**Алиасы:**`, `**RU only**` и `**Async**`.\n");

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
                                !t.Name.EndsWith("Collection") &&
                                !t.Name.EndsWith("Description") &&
                                !t.Name.EndsWith("Dto") &&
                                !t.Name.EndsWith("Activity") &&
                                !t.Name.Contains("Xml") &&
                                !t.Name.Contains("Log") &&
                                !t.Name.Contains("Enumerator") &&
                                !t.Name.Contains("Internal") &&
                                !t.Name.StartsWith("<"))
                    .OrderBy(t => t.Namespace)
                    .ThenBy(t => t.Name)
                    .ToList();

                int addedForDll = 0;

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
                            false));
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
                            true));
                    }

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

                    if (!propLines.Any() && !methodEntries.Any()) continue;

                    // САМОДОСТОЯТЕЛЬНАЯ ШАПКА БЛОКА: полное имя типа, пространство имён и сборка
                    // дублируются в каждом блоке, чтобы сплиттеры RAG не теряли контекст фрагмента.
                    string ns = string.IsNullOrEmpty(type.Namespace) ? "Global" : type.Namespace;
                    sb.AppendLine($"### [Type] {type.FullName}");
                    sb.AppendLine($"- **Namespace:** `{ns}`");
                    sb.AppendLine($"- **Assembly:** `{dllName}`");

                    // Явно отделяем прикладной Macro DSL от низкоуровневого Core Open API.
                    string apiLevel = IsMacroDslType(type)
                        ? "Macro DSL (доступно внутри MacroProvider)"
                        : "Core Open API";
                    sb.AppendLine($"- **Уровень API:** {apiLevel}");

                    if (!string.IsNullOrEmpty(typeSummary))
                        sb.AppendLine($"- **Описание:** {typeSummary}");
                    sb.AppendLine();

                    if (propLines.Any())
                    {
                        sb.AppendLine("**Свойства:**");
                        foreach (var p in propLines)
                            sb.AppendLine($"- `{p.TypeName} {p.Name}`{p.RuNote}{p.DocNote}");
                        sb.AppendLine();
                    }

                    if (methodEntries.Any())
                    {
                        foreach (var m in methodEntries)
                        {
                            // H4 важен: parse() закрывает секцию на заголовке,
                            // поэтому каждый метод станет отдельным чанком.
                            sb.AppendLine($"#### [Method] `{m.Signature}`");
                            sb.AppendLine($"- **Полное имя:** `{m.FullName}`");

                            if (!string.IsNullOrEmpty(m.Summary))
                                sb.AppendLine($"- **Описание:** {m.Summary}");

                            if (m.Parameters.Any())
                            {
                                sb.AppendLine("- **Параметры:**");
                                foreach (var p in m.Parameters)
                                    sb.AppendLine($"  - `{p}`");
                            }

                            if (!string.IsNullOrEmpty(m.Alias))
                                sb.AppendLine($"- **Алиасы:** `{m.Alias}`");

                            if (m.RuOnly)
                                sb.AppendLine("- **RU only**");

                            if (m.IsAsync)
                                sb.AppendLine("- **Async**");

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
            ReturnType = FormatTypeName(method.ReturnType)
        };
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

                XElement summaryElement = member.Element("summary");
                if (summaryElement == null)
                    continue;

                string summaryText = CleanText(summaryElement.Value);
                if (string.IsNullOrEmpty(summaryText))
                    continue;

                // Первое вхождение имеет приоритет: повторные ключи (например, у перегрузок с одинаковым членом)
                // не должны перезаписывать уже найденное описание.
                if (!_docs.ContainsKey(nameAttr.Value))
                    _docs.Add(nameAttr.Value, summaryText);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($" [XML Docs] Не удалось прочитать {Path.GetFileName(xmlPath)}: {ex.Message}");
            _docs.Clear();
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
    /// Формирует список типов параметров метода в нотации идентификаторов XML-документации:
    /// полное имя типа, для массивов — суффикс "[]", для ref/out — суффикс "@",
    /// для generic-параметров — обратная кавычка с индексом ("`0").
    /// </summary>
    private static string BuildParameterList(MethodInfo method)
    {
        ParameterInfo[] parameters;
        try
        {
            parameters = method.GetParameters();
        }
        catch (Exception)
        {
            return string.Empty;
        }

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
        string value;
        return _docs.TryGetValue(key, out value) ? value : null;
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

        foreach (var pair in _docs)
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