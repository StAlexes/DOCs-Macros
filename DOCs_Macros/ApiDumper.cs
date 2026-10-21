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

        string[] ignoredMethods = { 
            "Equals", "GetHashCode", "GetType", "ToString", "MemberwiseClone", 
            "ReferenceEquals", "InitializeLifetimeService", "Dispose", "Clone" 
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
        sb.AppendLine("> Используй указанные `Namespace` для добавления в `using`. Пометка [RU: …] — русскоязычный алиас метода или свойства, найденный зеркалированием по идентичному тексту `<summary>` в XML-документации; [has Async] — наличие асинхронной версии; [RU only] — член, существующий только в русскоязычном алиасе.\n");

        int totalTypes = 0;

        foreach (var dllName in targetAssemblies)
        {
            string fullPath = Path.Combine(tflexFolder, dllName);
            if (!File.Exists(fullPath)) continue;

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

                        string propSummary = xmlDoc.GetPropertySummary(p);
                        propLines.Add(new PropEntry
                        {
                            TypeName = FormatTypeName(p.PropertyType),
                            Name = p.Name,
                            RuNote = note,
                            DocNote = string.IsNullOrEmpty(propSummary) ? "" : $" — {propSummary}"
                        });
                    }

                    // 2. Методы
                    var allMethods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
                        .Where(m => !m.IsSpecialName && !ignoredMethods.Contains(m.Name))
                        .ToList();

                    var ruMethods = allMethods.Where(m => Regex.IsMatch(m.Name, @"\p{IsCyrillic}")).ToList();
                    var engMethods = allMethods.Where(m => !Regex.IsMatch(m.Name, @"\p{IsCyrillic}") && !m.Name.EndsWith("Async"))
                        .GroupBy(m => m.Name)
                        .ToList();

                    var methodLines = new List<string>();

                    // Кириллические методы, уже использованные как алиасы латинских методов.
                    // Нужно, чтобы во втором проходе вывести только «осиротевшие» кириллические
                    // методы и не продублировать те, что уже показаны в строке латинского метода.
                    var matchedRuMethods = new HashSet<MethodInfo>();

                    foreach (var group in engMethods)
                    {
                        string methodName = group.Key;
                        bool hasAsync = allMethods.Any(m => m.Name == methodName + "Async");
                        string asyncNote = hasAsync ? " [has Async]" : "";

                        var best = group.OrderByDescending(m => m.GetParameters().Length).First();
                        var overloadsNote = group.Count() > 1 ? $" (+{group.Count() - 1})" : "";

                        // Описание метода из парного XML-файла документации (с fallback по перегрузкам)
                        string methodSummary = xmlDoc.GetMethodSummary(best);

                        // Русскоязычный алиас метода ищется строго по идентичному тексту <summary>
                        // и совпадающей арности — случайная длина параметров больше не используется.
                        var ruMatch = FindMirroredMethod(ruMethods, best, methodSummary, xmlDoc);
                        string ruNote = "";
                        if (ruMatch != null)
                        {
                            ruNote = $" [RU: {ruMatch.Name}]";

                            // Алиас закреплён за латинским методом: во втором проходе он
                            // уже не должен выводиться как самостоятельный [RU only].
                            matchedRuMethods.Add(ruMatch);
                        }

                        // Сигнатура строится через FormatTypeName: устраняются артефакты
                        // рефлексии вида List`1, Nullable`1 и корректно печатается void.
                        string paramsList = string.Join(", ", best.GetParameters().Select(p => $"{FormatTypeName(p.ParameterType)} {p.Name}"));
                        string methodSig = $"{FormatTypeName(best.ReturnType)} {best.Name}({paramsList}){overloadsNote}";

                        string docNote = string.IsNullOrEmpty(methodSummary) ? "" : $" — {methodSummary}";

                        methodLines.Add($"- `{methodSig}`{asyncNote}{ruNote}{docNote}");
                    }

                    // ВТОРОЙ ПРОХОД: самостоятельные кириллические методы.
                    // Прежняя версия обходила только коллекцию engMethods, поэтому у чисто
                    // кириллических классов макроязыка (ДиалогВвода, Объект, Подключение,
                    // ДиалогВыбораОбъектов и т.п.) коллекция engMethods пуста и секция
                    // «**Методы:**» не генерировалась вовсе — методы терялись безвозвратно.
                    // Сюда же попадают методы двуязычных классов, объявленные исключительно
                    // на русском языке и не нашедшие латинской пары по тексту <summary>.
                    // Все они выводятся с маркером [RU only], чтобы русскоязычный API
                    // макроязыка сохранялся в справочнике полностью.
                    var remainingRuMethods = ruMethods
                        .Where(m => !matchedRuMethods.Contains(m) && !m.Name.EndsWith("Async"))
                        .GroupBy(m => m.Name)
                        .OrderBy(g => g.Key)
                        .ToList();

                    foreach (var group in remainingRuMethods)
                    {
                        string methodName = group.Key;
                        bool hasAsync = allMethods.Any(m => m.Name == methodName + "Async");
                        string asyncNote = hasAsync ? " [has Async]" : "";

                        var best = group.OrderByDescending(m => m.GetParameters().Length).First();
                        var overloadsNote = group.Count() > 1 ? $" (+{group.Count() - 1})" : "";

                        string methodSummary = xmlDoc.GetMethodSummary(best);
                        string docNote = string.IsNullOrEmpty(methodSummary) ? "" : $" — {methodSummary}";

                        string paramsList = string.Join(", ", best.GetParameters().Select(p => $"{FormatTypeName(p.ParameterType)} {p.Name}"));
                        string methodSig = $"{FormatTypeName(best.ReturnType)} {best.Name}({paramsList}){overloadsNote}";

                        methodLines.Add($"- `{methodSig}`{asyncNote} [RU only]{docNote}");
                    }

                    // Описание типа (класса/интерфейса) из парного XML-файла документации
                    string typeSummary = xmlDoc.GetTypeSummary(type);

                    if (!propLines.Any() && !methodLines.Any()) continue;

                    // САМОДОСТОЯТЕЛЬНАЯ ШАПКА БЛОКА: полное имя типа, пространство имён и сборка
                    // дублируются в каждом блоке, чтобы сплиттеры RAG не теряли контекст фрагмента.
                    string ns = string.IsNullOrEmpty(type.Namespace) ? "Global" : type.Namespace;
                    sb.AppendLine($"### [Type] {type.FullName}");
                    sb.AppendLine($"- **Namespace:** `{ns}`");
                    sb.AppendLine($"- **Assembly:** `{dllName}`");
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

                    if (methodLines.Any())
                    {
                        sb.AppendLine("**Методы:**");
                        foreach (var m in methodLines) sb.AppendLine(m);
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

        File.WriteAllText(outPath, sb.ToString(), Encoding.UTF8);
        Console.WriteLine($"\n Готово! Всего классов: {totalTypes}");
        Console.WriteLine($" Файл сохранён: {outPath}");
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

            string ruSummary = xmlDoc.GetMethodSummary(ruMethod);
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