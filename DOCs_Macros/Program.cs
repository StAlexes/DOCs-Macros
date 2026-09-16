using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

public class Program
{
    public static readonly string TFlexFolder = @"C:\Program Files (x86)\T-FLEX DOCs 18 (18ReleaseOAK)\Program";

    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        // 1. Инициализируем окружение: переносим директорию и настраиваем автопоиск всех DLL
        if (Directory.Exists(TFlexFolder))
        {
            Directory.SetCurrentDirectory(TFlexFolder);
        }

        AppDomain.CurrentDomain.AssemblyResolve += (sender, resolveArgs) =>
        {
            try
            {
                string simpleName = new AssemblyName(resolveArgs.Name).Name;
                string targetPath = Path.Combine(TFlexFolder, simpleName + ".dll");

                if (File.Exists(targetPath))
                {
                    return Assembly.LoadFrom(targetPath);
                }
            }
            catch { }
            return null;
        };

        // 2. Запуск диспетчера
        RunDispatcher(args);
    }

    private static void RunDispatcher(string[] args)
    {
        // Если передан аргумент командной строки (например, dotnet run -- schema)
        if (args != null && args.Length > 0)
        {
            string cmd = args[0].ToLowerInvariant().TrimStart('-');
            switch (cmd)
            {
                case "schema":
                case "s":
                    SchemaFetcher.Fetch();
                    return;
                case "api":
                case "a":
                    ApiDumper.ExportTFlexApiToMarkdown();
                    return;
            }
        }

        // Интерактивное текстовое меню, если запустили без аргументов
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=================================================");
            Console.WriteLine("    T-FLEX DOCs 18 — ИНСТРУМЕНТЫ ВАЙБКОДИНГА    ");
            Console.WriteLine("=================================================");
            Console.WriteLine("[1] Выгрузить схему справочника из базы (SchemaFetcher)");
            Console.WriteLine("[2] Сгенерировать справочник API из DLL (ApiDumper)");
            Console.WriteLine("[0] Выход");
            Console.WriteLine("-------------------------------------------------");
            Console.Write("Выберите действие: ");

            string choice = Console.ReadLine()?.Trim();

            Console.WriteLine();
            switch (choice)
            {
                case "1":
                    SchemaFetcher.Fetch();
                    Pause();
                    break;
                case "2":
                    ApiDumper.ExportTFlexApiToMarkdown();
                    Pause();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Неверный ввод. Нажмите любую клавишу...");
                    Console.ReadKey(true);
                    break;
            }
        }
    }

    private static void Pause()
    {
        Console.WriteLine("\nНажмите любую клавишу для возврата в главное меню...");
        Console.ReadKey(true);
    }
}