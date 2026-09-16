using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TFlex.DOCs.Model;
using TFlex.DOCs.Model.Classes;
using TFlex.DOCs.Model.References;

public class SchemaFetcher
{
    // Запуск из корня решения: dotnet run --project .\DOCs_Macros\DOCs_Macros.csproj -- schema
    public static void Fetch()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("=== ПОДКЛЮЧЕНИЕ К T-FLEX DOCs ДЛЯ ВЫГРУЗКИ СХЕМЫ ===");

        // Читаем путь к серверу из локального файла настроек моста
        string appDataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TFlexDocs_VsCodeBridge");
        string serverConfigFile = Path.Combine(appDataDir, "server.txt");

        string serverAddress = File.Exists(serverConfigFile) ? File.ReadAllText(serverConfigFile).Trim() : "";

        if (string.IsNullOrEmpty(serverAddress))
        {
            Console.Write("Введите адрес сервера T-FLEX DOCs (например, http://docs-server:8080): ");
            serverAddress = Console.ReadLine();
            Directory.CreateDirectory(appDataDir);
            File.WriteAllText(serverConfigFile, serverAddress);
        }

        var connectionParametersType = typeof(TFlex.DOCs.Model.ServerConnection).Assembly
            .GetType("TFlex.DOCs.Model.ConnectionParameters", throwOnError: false);

        if (connectionParametersType == null)
        {
            Console.WriteLine("Не удалось найти тип ConnectionParameters в сборке T-FLEX DOCs.");
            return;
        }

        var connParams = Activator.CreateInstance(
            connectionParametersType,
            System.Reflection.BindingFlags.Instance |
            System.Reflection.BindingFlags.Public |
            System.Reflection.BindingFlags.NonPublic,
            binder: null,
            args: null,
            culture: null);
        connectionParametersType.GetProperty("Server")?.SetValue(connParams, serverAddress);

        Console.Write("Логин T-FLEX DOCs: ");
        string userName = Console.ReadLine();
        Console.Write("Пароль T-FLEX DOCs: ");
        string password = ReadPassword();
        Console.WriteLine();

        connectionParametersType.GetProperty("UserName")?.SetValue(connParams, userName);
        connectionParametersType.GetProperty("WindowsAuthentication")?.SetValue(connParams, false);

        var passwordProperty = connectionParametersType.GetProperty("Password");
        object passwordValue = null;
        if (passwordProperty != null)
        {
            passwordValue = Activator.CreateInstance(passwordProperty.PropertyType, password);
        }

        var openMethod = typeof(TFlex.DOCs.Model.ServerConnection)
            .GetMethods()
            .FirstOrDefault(method =>
                method.Name == "Open" &&
                method.GetParameters().Length == 5 &&
                method.GetParameters()[0].ParameterType == typeof(string) &&
                method.GetParameters()[1].ParameterType == passwordProperty.PropertyType &&
                method.GetParameters()[2].ParameterType == typeof(string));
        if (openMethod == null)
        {
            Console.WriteLine("Не найден метод ServerConnection.Open для серверной авторизации.");
            return;
        }

        object connection;
        try
        {
            connection = openMethod.Invoke(null, new object[] { userName, passwordValue, serverAddress, null, null });
        }
        catch (System.Reflection.TargetInvocationException ex)
        {
            Console.WriteLine($"Не удалось подключиться к T-FLEX DOCs: {ex.InnerException?.GetBaseException().Message ?? ex.Message}");
            return;
        }

        if (connection == null)
        {
            Console.WriteLine("Не удалось открыть соединение с сервером T-FLEX DOCs.");
            return;
        }

        using (var disposable = (System.IDisposable)connection)
        {
            Console.WriteLine("\nЗагрузка списка справочников...");
            var catalog = connection.GetType().GetProperty("ReferenceCatalog")?.GetValue(connection);
            if (catalog == null)
            {
                Console.WriteLine("Не удалось получить каталог справочников.");
                return;
            }

            var listMethod = catalog.GetType().GetMethod("GetAllReferences", Type.EmptyTypes);
            var refsObject = listMethod != null ? listMethod.Invoke(catalog, null) : null;
            if (refsObject == null)
            {
                var getReferencesMethod = catalog.GetType().GetMethod("GetReferences", Type.EmptyTypes);
                refsObject = getReferencesMethod?.Invoke(catalog, null);
            }

            if (refsObject == null)
            {
                Console.WriteLine("Не удалось получить список справочников из каталога.");
                return;
            }

            var allReferences = new List<ReferenceInfo>();
            foreach (var item in (System.Collections.IEnumerable)refsObject)
            {
                allReferences.Add((ReferenceInfo)item);
            }

            allReferences = allReferences.OrderBy(r => r.Name).ToList();

            for (int i = 0; i < allReferences.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {allReferences[i].Name}");
            }

            Console.Write("\nВведите номер справочника для выгрузки: ");
            if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > allReferences.Count)
            {
                Console.WriteLine("Неверный номер.");
                return;
            }

            var refInfo = allReferences[choice - 1];
            ExportReferenceSchema(refInfo);
        }
    }

    private static string ReadPassword()
    {
        if (Console.IsInputRedirected)
        {
            return Console.ReadLine() ?? string.Empty;
        }

        var password = new StringBuilder();
        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(intercept: true);

            if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password.Length--;
                Console.Write("\b \b");
            }
            else if (!char.IsControl(key.KeyChar))
            {
                password.Append(key.KeyChar);
                Console.Write('*');
            }
        }
        while (key.Key != ConsoleKey.Enter);

        return password.ToString();
    }

    private static void ExportReferenceSchema(ReferenceInfo refInfo)
    {
        string projectDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", ".."));
        string outDir = Path.Combine(projectDir, "Schemas");
        Directory.CreateDirectory(outDir);

        string cleanName = string.Join("_", refInfo.Name.Split(Path.GetInvalidFileNameChars()));
        string referenceIdentifier = ToIdentifier(refInfo.Name);
        string mdPath = Path.Combine(outDir, $"{cleanName}.schema.md");
        string csPath = Path.Combine(outDir, $"{cleanName}.guids.cs");

        var paramGroup = refInfo.Description; // Главная группа параметров
        var classes = refInfo.Classes;

        var sbMd = new StringBuilder();
        var sbCs = new StringBuilder();

        // --- ЗАГОЛОВОК MARKDOWN ---
        sbMd.AppendLine($"# Справочник: {refInfo.Name}");
        sbMd.AppendLine($"- **GUID справочника:** `{refInfo.Guid}`");
        sbMd.AppendLine($"- **Системный ID:** `{refInfo.Id}`\n");

        sbMd.AppendLine("## Свойства справочника:");
        sbMd.AppendLine($"- **Тип иерархии:** `{refInfo.HierarchyType}`");
        sbMd.AppendLine($"- **Видимость:** `{refInfo.Visibility}`");
        sbMd.AppendLine($"- **Статус активности:** `{refInfo.ActivityStatus}`");
        sbMd.AppendLine($"- **Статический:** `{refInfo.IsStatic}`");
        sbMd.AppendLine($"- **Есть иерархия:** `{refInfo.HasHierarchy}`");
        sbMd.AppendLine($"- **Поддерживает рабочий стол:** `{refInfo.SupportsDesktop}`");
        sbMd.AppendLine($"- **Поддерживает корзину:** `{refInfo.SupportsRecycleBin}`");
        object supportsNomenclature = GetPropertyValue(refInfo, "SupportsNomenclature");
        sbMd.AppendLine($"- **Поддерживает номенклатуру:** `{supportsNomenclature}`");
        sbMd.AppendLine($"- **Поддерживает экземпляры объектов:** `{refInfo.SupportsObjectsInstances}`");
        sbMd.AppendLine($"- **Поддерживает типы структур:** `{refInfo.SupportsStructureTypes}`");
        sbMd.AppendLine($"- **ID группы иерархии:** `{refInfo.HierarchyGroupId}`");
        sbMd.AppendLine($"- **Описание загружено:** `{refInfo.DescriptionLoaded}`");
        sbMd.AppendLine();

        sbMd.AppendLine("## Возможности и настройки структуры:");
        AppendGroupProperty(sbMd, paramGroup, "Тип структуры", "Type");
        AppendGroupProperty(sbMd, paramGroup, "Тип иерархии", "HierarchyType");
        AppendGroupProperty(sbMd, paramGroup, "Видимость", "Visibility");
        AppendGroupProperty(sbMd, paramGroup, "Статус активности", "ActivityStatus");
        AppendGroupProperty(sbMd, paramGroup, "Журнал изменений данных", "SupportsDataChangeLog");
        AppendGroupProperty(sbMd, paramGroup, "Ревизии", "SupportsRevisions");
        AppendGroupProperty(sbMd, paramGroup, "Прототипы", "SupportsPrototypes");
        AppendGroupProperty(sbMd, paramGroup, "Владелец", "SupportsOwner");
        AppendGroupProperty(sbMd, paramGroup, "Подписи", "SupportsSignature");
        AppendGroupProperty(sbMd, paramGroup, "Все типы подписей", "UseAllSignatureTypes");
        AppendGroupProperty(sbMd, paramGroup, "Конфигурирование", "SupportsConfigurationSettings");
        AppendGroupProperty(sbMd, paramGroup, "Даты действия", "SupportsActivityDates");
        AppendGroupProperty(sbMd, paramGroup, "Применимость", "SupportsApplicability");
        AppendGroupProperty(sbMd, paramGroup, "Контексты проектирования", "SupportsDesignContexts");
        AppendGroupProperty(sbMd, paramGroup, "Замены в контексте", "SupportsSubstitutesInContext");
        AppendGroupProperty(sbMd, paramGroup, "Корзина", "SupportsRecycleBin");
        AppendGroupProperty(sbMd, paramGroup, "Рабочий стол", "SupportsDesktop");
        AppendGroupProperty(sbMd, paramGroup, "Номенклатура", "SupportsNomenclature");
        AppendGroupProperty(sbMd, paramGroup, "Экземпляры объектов", "SupportsObjectsInstances");
        AppendGroupProperty(sbMd, paramGroup, "Типы структур", "SupportsStructureTypes");
        AppendGroupProperty(sbMd, paramGroup, "Иерархия", "HasHierarchy");
        AppendGroupProperty(sbMd, paramGroup, "Классы", "SupportsClasses");
        AppendGroupProperty(sbMd, paramGroup, "Системные объекты", "SupportsSystemObjects");
        AppendGroupProperty(sbMd, paramGroup, "Порядок объектов", "SupportsOrder");
        AppendGroupProperty(sbMd, paramGroup, "Приватные папки", "SupportsPrivateFolders");
        AppendGroupProperty(sbMd, paramGroup, "Расширенные параметры", "SupportsExtendedParameters");
        AppendGroupProperty(sbMd, paramGroup, "Обязательный доступ", "SupportsMandatoryAccess");
        AppendGroupProperty(sbMd, paramGroup, "Шифрование", "SupportsEncryption");
        AppendGroupProperty(sbMd, paramGroup, "Этапы", "SupportsStages");
        AppendGroupProperty(sbMd, paramGroup, "Множественные вложения", "SupportMultiAttachment");
        AppendGroupProperty(sbMd, paramGroup, "Можно менять тип", "CanChangeClass");
        AppendGroupProperty(sbMd, paramGroup, "Можно редактировать", "CanEdit");
        AppendGroupProperty(sbMd, paramGroup, "Можно удалять", "CanDelete");
        AppendGroupProperty(sbMd, paramGroup, "Имя таблицы", "TableName");
        AppendGroupProperty(sbMd, paramGroup, "Имя таблицы сгенерировано", "TableNameGenerated");
        AppendGroupProperty(sbMd, paramGroup, "GUID конфигуратора", "ConfiguratorGuid");
        sbMd.AppendLine();

        // --- ЗАГОЛОВОК GUIDS C# ---
        sbCs.AppendLine("using System;");
        sbCs.AppendLine();
        sbCs.AppendLine($"public static class Guids_{referenceIdentifier}");
        sbCs.AppendLine("{");
        sbCs.AppendLine($"    /// <summary>Справочник \"{refInfo.Name}\"</summary>");
        sbCs.AppendLine($"    public static readonly Guid Справочник_{referenceIdentifier} = new Guid(\"{refInfo.Guid}\");\n");

        // --- ТИПЫ ОБЪЕКТОВ ---
        sbMd.AppendLine("## Типы объектов:");
        var exportedParameters = new HashSet<Guid>();
        var exportedLinks = new HashSet<Guid>();
        var typeIdentifiers = new HashSet<string>(StringComparer.Ordinal);
        var parameterIdentifiers = new HashSet<string>(StringComparer.Ordinal);
        var linkIdentifiers = new HashSet<string>(StringComparer.Ordinal);
        var objectListIdentifiers = new HashSet<string>(StringComparer.Ordinal);

        AppendObjectLists(sbMd, sbCs, refInfo, objectListIdentifiers);

        foreach (var cls in classes.AllClasses)
        {
            string parentInfo = cls.Base != null ? $" (Наследует от: `{cls.Base.Name}`)" : "";
            sbMd.AppendLine($"### Тип: `{cls.Name}`{parentInfo}");
            sbMd.AppendLine($"- **GUID типа:** `{cls.Guid}`");

            sbCs.AppendLine($"    /// <summary>Тип объекта: {cls.Name}</summary>");
            string typeIdentifier = GetUniqueIdentifier($"Тип_{ToIdentifier(cls.Name)}", cls.Guid, typeIdentifiers);
            sbCs.AppendLine($"    public static readonly Guid {typeIdentifier} = new Guid(\"{cls.Guid}\");");
            sbMd.AppendLine();
        }

        sbMd.AppendLine("## Параметры:");
        foreach (var parameter in classes.AllClasses
            .SelectMany(cls => cls.ParameterGroups.SelectMany(group => group.Parameters))
            .Where(parameter => exportedParameters.Add(parameter.Guid)))
        {
            sbMd.AppendLine($"- `{parameter.Name}` (Тип: {parameter.Type.Name}, GUID: `{parameter.Guid}`)");
            string parameterName = ToIdentifier(parameter.Name);
            sbCs.AppendLine($"    /// <summary>Параметр: {parameter.Name} (Справочник: {refInfo.Name})</summary>");
            string parameterIdentifier = GetUniqueIdentifier($"p{ToIdentifier(refInfo.Name)}_{parameterName}", parameter.Guid, parameterIdentifiers);
            sbCs.AppendLine($"    public static readonly Guid {parameterIdentifier} = new Guid(\"{parameter.Guid}\");");
        }

        sbMd.AppendLine();
        sbMd.AppendLine("## Связи:");
        foreach (var link in classes.AllClasses
            .SelectMany(cls => cls.ParameterGroups.Where(group => group.IsLinkGroup))
            .Where(link => exportedLinks.Add(link.Guid)))
        {
            sbMd.AppendLine($"- `{link.Name}` (Тип связи: {link.LinkType}, Направление: {link.LinkVisibility}, GUID: `{link.Guid}`)");
            sbCs.AppendLine($"    /// <summary>Связь: {link.Name} (Справочник: {refInfo.Name})</summary>");
            string linkIdentifier = GetUniqueIdentifier($"l{ToIdentifier(refInfo.Name)}_{ToIdentifier(link.Name)}", link.Guid, linkIdentifiers);
            sbCs.AppendLine($"    public static readonly Guid {linkIdentifier} = new Guid(\"{link.Guid}\");");
        }

        sbCs.AppendLine("}");

        File.WriteAllText(mdPath, sbMd.ToString(), Encoding.UTF8);
        File.WriteAllText(csPath, sbCs.ToString(), Encoding.UTF8);

        Console.WriteLine($"\nСхема успешно сохранена:\n - {mdPath}\n - {csPath}");
    }

    private static string ToIdentifier(string value)
    {
        var identifier = new StringBuilder();
        foreach (char character in value)
        {
            identifier.Append(char.IsLetterOrDigit(character) || character == '_' ? character : '_');
        }

        if (identifier.Length == 0 || char.IsDigit(identifier[0]))
        {
            identifier.Insert(0, '_');
        }

        return identifier.ToString();
    }

    private static string GetUniqueIdentifier(string baseIdentifier, Guid guid, HashSet<string> usedIdentifiers)
    {
        if (usedIdentifiers.Add(baseIdentifier))
        {
            return baseIdentifier;
        }

        string uniqueIdentifier = $"{baseIdentifier}_{guid.ToString("N").Substring(0, 8)}";
        usedIdentifiers.Add(uniqueIdentifier);
        return uniqueIdentifier;
    }

    private static object GetPropertyValue(object instance, string propertyName)
    {
        return instance.GetType().GetProperty(propertyName)?.GetValue(instance) ?? "недоступно";
    }

    private static void AppendGroupProperty(StringBuilder markdown, object group, string label, string propertyName)
    {
        if (group == null)
        {
            markdown.AppendLine($"- **{label}:** `недоступно`");
            return;
        }

        var property = group.GetType().GetProperty(propertyName);
        object value;
        try
        {
            value = property?.GetValue(group) ?? "недоступно";
        }
        catch (Exception ex) when (ex is System.Reflection.TargetInvocationException || ex is InvalidOperationException)
        {
            value = "ошибка чтения";
        }

        markdown.AppendLine($"- **{label}:** `{value}`");
    }

    private static void AppendObjectLists(StringBuilder markdown, StringBuilder guids, ReferenceInfo reference, HashSet<string> objectListIdentifiers)
    {
        markdown.AppendLine("## Списки объектов:");

        try
        {
            var settingsType = typeof(TFlex.DOCs.Model.ServerConnection).Assembly
                .GetType("TFlex.DOCs.Model.DataExchange.Settings.ExportReferenceSettings", false);
            var createMethod = settingsType?.GetMethod(
                "Create",
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static,
                binder: null,
                types: new[] { typeof(ReferenceInfo) },
                modifiers: null);
            var settings = createMethod?.Invoke(null, new object[] { reference });
            var objectLists = settings?.GetType().GetProperty("ObjectLists")?.GetValue(settings) as System.Collections.IEnumerable;

            if (objectLists == null)
            {
                markdown.AppendLine("- Не удалось получить список объектных списков через ExportReferenceSettings.");
                markdown.AppendLine();
                return;
            }

            var visited = new HashSet<Guid>();
            bool hasObjectLists = false;
            foreach (var objectList in objectLists)
            {
                hasObjectLists = true;
                AppendObjectList(markdown, guids, reference, objectList, 0, visited, objectListIdentifiers);
            }

            bool hasLinkBasedObjectLists = AppendLinkBasedObjectLists(markdown, guids, reference, visited, objectListIdentifiers);
            if (!hasObjectLists && !hasLinkBasedObjectLists)
            {
                markdown.AppendLine("- Нет списков объектов.");
            }
        }
        catch (Exception ex) when (ex is System.Reflection.TargetInvocationException || ex is InvalidOperationException)
        {
            markdown.AppendLine($"- Ошибка чтения списков объектов: `{ex.Message}`");
        }

        markdown.AppendLine();
    }

    private static bool AppendLinkBasedObjectLists(StringBuilder markdown, StringBuilder guids, ReferenceInfo reference, HashSet<Guid> visited, HashSet<string> objectListIdentifiers)
    {
        var classes = reference.Classes?.AllClasses as System.Collections.IEnumerable;
        if (classes == null)
        {
            return false;
        }

        bool hasObjectLists = false;
        foreach (var classObject in classes)
        {
            string className = Convert.ToString(classObject.GetType().GetProperty("Name")?.GetValue(classObject));
            var groups = classObject.GetType().GetProperty("ParameterGroups")?.GetValue(classObject) as System.Collections.IEnumerable;
            if (groups == null)
            {
                continue;
            }

            foreach (var group in groups)
            {
                bool isTableOneToMany = Convert.ToBoolean(GetPropertyValue(group, "IsTableOneToMany"));
                bool isLinkGroup = Convert.ToBoolean(GetPropertyValue(group, "IsLinkGroup"));
                bool isAnyReferenceLink = Convert.ToBoolean(GetPropertyValue(group, "IsAnyReferenceLink"));
                bool isSearchQueryLink = Convert.ToBoolean(GetPropertyValue(group, "IsSearchQueryLink"));
                if (!isTableOneToMany || isLinkGroup || isAnyReferenceLink || isSearchQueryLink)
                {
                    continue;
                }

                hasObjectLists = true;

                Guid groupGuid = (Guid)(group.GetType().GetProperty("Guid")?.GetValue(group) ?? Guid.Empty);
                if (!visited.Add(groupGuid))
                {
                    continue;
                }

                string name = Convert.ToString(group.GetType().GetProperty("Name")?.GetValue(group));
                markdown.AppendLine($"- **Список объектов:** `{name}`");
                markdown.AppendLine($"  - **GUID:** `{groupGuid}`");
                markdown.AppendLine($"  - **Владелец (тип):** `{className}`");
                markdown.AppendLine($"  - **Справочник:** `{reference.Name}`");
                AppendIndentedGroupProperty(markdown, group, "", "Тип связи", "LinkType");
                AppendIndentedGroupProperty(markdown, group, "", "Направление", "LinkVisibility");
                AppendIndentedGroupProperty(markdown, group, "", "Таблица один-ко-многим", "IsTableOneToMany");
                AppendObjectListGuid(guids, reference, name, groupGuid, objectListIdentifiers);
            }
        }

        return hasObjectLists;
    }

    private static void AppendObjectList(StringBuilder markdown, StringBuilder guids, ReferenceInfo reference, object settings, int depth, HashSet<Guid> visited, HashSet<string> objectListIdentifiers)
    {
        if (settings == null || depth > 32)
        {
            return;
        }

        var masterGroup = settings.GetType().GetProperty("MasterGroup")?.GetValue(settings);
        if (masterGroup == null)
        {
            markdown.AppendLine("- Список объектов без описания группы.");
            return;
        }

        Guid groupGuid = (Guid)(masterGroup.GetType().GetProperty("Guid")?.GetValue(masterGroup) ?? Guid.Empty);
        if (!visited.Add(groupGuid))
        {
            return;
        }

        string indent = new string(' ', depth * 2);
        string name = Convert.ToString(masterGroup.GetType().GetProperty("Name")?.GetValue(masterGroup));
        markdown.AppendLine($"{indent}- **Список:** `{name}`");
        markdown.AppendLine($"{indent}  - **GUID:** `{groupGuid}`");
        AppendObjectListGuid(guids, reference, name, groupGuid, objectListIdentifiers);
        AppendIndentedGroupProperty(markdown, masterGroup, indent, "Тип", "Type");
        AppendIndentedGroupProperty(markdown, masterGroup, indent, "Иерархия", "HierarchyType");
        AppendIndentedGroupProperty(markdown, masterGroup, indent, "Поддерживает классы", "SupportsClasses");
        AppendIndentedGroupProperty(markdown, masterGroup, indent, "Поддерживает прототипы", "SupportsPrototypes");

        var classes = masterGroup.GetType().GetProperty("Classes")?.GetValue(masterGroup);
        var allClasses = classes?.GetType().GetProperty("AllClasses")?.GetValue(classes) as System.Collections.IEnumerable;
        if (allClasses != null)
        {
            foreach (var classObject in allClasses)
            {
                string className = Convert.ToString(classObject.GetType().GetProperty("Name")?.GetValue(classObject));
                object classGuid = classObject.GetType().GetProperty("Guid")?.GetValue(classObject) ?? Guid.Empty;
                markdown.AppendLine($"{indent}  - **Тип списка:** `{className}` (GUID: `{classGuid}`)");
            }
        }

        var nestedLists = settings.GetType().GetProperty("ObjectLists")?.GetValue(settings) as System.Collections.IEnumerable;
        if (nestedLists != null)
        {
            foreach (var nestedList in nestedLists)
            {
                AppendObjectList(markdown, guids, reference, nestedList, depth + 1, visited, objectListIdentifiers);
            }
        }
    }

    private static void AppendObjectListGuid(StringBuilder guids, ReferenceInfo reference, string name, Guid guid, HashSet<string> usedIdentifiers)
    {
        string identifier = GetUniqueIdentifier($"ol{ToIdentifier(reference.Name)}_{ToIdentifier(name)}", guid, usedIdentifiers);
        guids.AppendLine($"    /// <summary>Список объектов: {name} (Справочник: {reference.Name})</summary>");
        guids.AppendLine($"    public static readonly Guid {identifier} = new Guid(\"{guid}\");");
    }

    private static void AppendIndentedGroupProperty(StringBuilder markdown, object group, string indent, string label, string propertyName)
    {
        object value = GetPropertyValue(group, propertyName);
        markdown.AppendLine($"{indent}  - **{label}:** `{value}`");
    }
}