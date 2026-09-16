using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

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

        var sb = new StringBuilder();
        sb.AppendLine("# Справочник API T-FLEX DOCs (с пространствами имён)");
        sb.AppendLine("> Используй указанные `Namespace` для добавления в `using`. Пометки [RU alias] — русскоязычные аналоги, [has Async] — наличие асинхронной версии.\n");

        int totalTypes = 0;

        foreach (var dllName in targetAssemblies)
        {
            string fullPath = Path.Combine(tflexFolder, dllName);
            if (!File.Exists(fullPath)) continue;

            try
            {
                var asm = Assembly.LoadFrom(fullPath);
                sb.AppendLine($"## Сборка: {dllName}\n");

                var types = asm.GetExportedTypes()
                    .Where(t => t.IsPublic && (t.IsClass || t.IsInterface))
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

                    var propLines = new List<string>();
                    foreach (var p in engProps)
                    {
                        var ruMatch = ruProps.FirstOrDefault(r => r.PropertyType == p.PropertyType);
                        string ruNote = ruMatch != null ? $" [RU: {ruMatch.Name}]" : "";
                        propLines.Add($"{p.Name}: {p.PropertyType.Name}{ruNote}");
                    }
                    foreach (var r in ruProps.Where(r => !engProps.Any(e => e.PropertyType == r.PropertyType)))
                    {
                        propLines.Add($"{r.Name}: {r.PropertyType.Name} [RU only]");
                    }

                    // 2. Методы
                    var allMethods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
                        .Where(m => !m.IsSpecialName && !ignoredMethods.Contains(m.Name))
                        .ToList();

                    var engMethods = allMethods.Where(m => !Regex.IsMatch(m.Name, @"\p{IsCyrillic}") && !m.Name.EndsWith("Async"))
                        .GroupBy(m => m.Name)
                        .ToList();

                    var methodLines = new List<string>();

                    foreach (var group in engMethods)
                    {
                        string methodName = group.Key;
                        bool hasAsync = allMethods.Any(m => m.Name == methodName + "Async");
                        string asyncNote = hasAsync ? " [has Async]" : "";

                        var ruMatch = allMethods.FirstOrDefault(m => Regex.IsMatch(m.Name, @"\p{IsCyrillic}") && 
                                                                     m.GetParameters().Length == group.First().GetParameters().Length);
                        string ruNote = ruMatch != null ? $" [RU: {ruMatch.Name}]" : "";

                        var best = group.OrderByDescending(m => m.GetParameters().Length).First();
                        var overloadsNote = group.Count() > 1 ? $" (+{group.Count() - 1})" : "";

                        methodLines.Add($"- `{best.ReturnType.Name} {best.Name}({string.Join(", ", best.GetParameters().Select(p => p.ParameterType.Name + " " + p.Name))}){overloadsNote}`{asyncNote}{ruNote}");
                    }

                    if (!propLines.Any() && !methodLines.Any()) continue;

                    // ВЫВОД КЛАССА С ЕГО ПРОСТРАНСТВОМ ИМЁН (Namespace)
                    string ns = string.IsNullOrEmpty(type.Namespace) ? "Global" : type.Namespace;
                    sb.AppendLine($"### `{type.Name}` (Namespace: `{ns}`)");
                    
                    if (propLines.Any()) sb.AppendLine($"**Свойства:** {string.Join(", ", propLines)}");
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
}