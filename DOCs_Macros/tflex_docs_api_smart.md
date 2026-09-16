# Справочник API T-FLEX DOCs (с пространствами имён)
> Используй указанные `Namespace` для добавления в `using`. Пометки [RU alias] — русскоязычные аналоги, [has Async] — наличие асинхронной версии.

## Сборка: TFlex.DOCs.Model.dll

### `AuthToken` (Namespace: `TFlex.DOCs.Model`)
**Свойства:** AccessToken: String, RefreshToken: String, RefreshTokenLifetime: Int64

### `AuthTokenExtensions` (Namespace: `TFlex.DOCs.Model`)
**Методы:**
- `TimeSpan GetExpirationTime(AuthToken authToken)`

### `ClientView` (Namespace: `TFlex.DOCs.Model`)
**Свойства:** Connection: ServerConnection, Id: Int32, Current: ClientView, IsAdministrator: Boolean, IsSystem: Boolean, Name: String, HostName: String, HostId: Int32, UserId: Int32, UserName: String, WorkingFolder: String, WorkingFolderPendingValue: String, WorkingFolderPendingStatus: WorkingFolderPendingStatus
**Методы:**
- `List`1 GetAllClientViews(ServerConnection connection) (+1)` [has Async]
- `List`1 GetHosts(ServerConnection connection) (+1)`
- `User GetUser()` [has Async]

### `ConnectionParameters` (Namespace: `TFlex.DOCs.Model`)
**Свойства:** UserName: String, Password: MD5HashString, WindowsAuthentication: Boolean, AccessToken: String, OidcToken: String, OidcProvider: Guid, Server: String, Proxy: IWebProxy, ConfigurationGuid: Nullable`1, UseSessionLog: Boolean, Communication: CommunicationMode, Compression: CompressionAlgorithm, DataSerializer: DataSerializerAlgorithm, ServerVersion: Int32, Version: String
**Методы:**
- `String GetServerAddress()`
- `String GetServerInstance()`
- `String GetServerNameWithInstance()`
- `String Serialize()`
- `ConnectionParameters Deserialize(String data)`

### `DomainObject` (Namespace: `TFlex.DOCs.Model`)
**Свойства:** Id: Int32

### `DomainObjectCollection`1` (Namespace: `TFlex.DOCs.Model`)
**Свойства:** Item: T, Count: Int32
**Методы:**
- `T Find(Int32 id)`
- `Int32 IndexOf(Int32 id) (+1)`
- `Boolean Contains(Int32 id) (+1)`
- `Void CopyTo(T[] array, Int32 arrayIndex)`
- `IEnumerator`1 GetEnumerator()`

### `DomainObjectComparer` (Namespace: `TFlex.DOCs.Model`)
**Методы:**
- `Int32 Compare(DomainObject x, DomainObject y)`

### `GuidDomainObject` (Namespace: `TFlex.DOCs.Model`)
**Свойства:** Guid: Guid

### `GuidDomainObjectCollection`1` (Namespace: `TFlex.DOCs.Model`)
**Свойства:** Connection: ServerConnection
**Методы:**
- `T Find(Guid guid)`
- `Int32 IndexOf(Guid guid)`
- `Boolean Contains(Guid guid)`

### `GuidDomainObjectComparer` (Namespace: `TFlex.DOCs.Model`)
**Методы:**
- `Int32 Compare(GuidDomainObject x, GuidDomainObject y)`

### `IconImage` (Namespace: `TFlex.DOCs.Model`)
**Свойства:** IsSvgImage: Boolean, SvgSource: Byte[], IconSource: Byte[], InternalIcon: Icon, SmallImage: Image, MediumImage: Image, LargeImage: Image, ImageSource: ImageSource, SmallIconSize: Size, MediumIconSize: Size, LargeIconSize: Size, IsLargeFont: Boolean, Key: String
**Методы:**
- `Void Save(Stream outputStream)`
- `Byte[] Serialize()`
- `IconImage Get(String name, ResourceManager iconOwner)`
- `Single GetScaleMetric()`

### `IModelCallback` (Namespace: `TFlex.DOCs.Model`)
**Методы:**
- `Boolean OnDesktopOperation(DesktopOperationInfo operation, Object context)` [has Async]
- `Boolean CanOverwriteFile(String filePath)`
- `CallbackDialogResult ShowMessage(String message, MessageDialogType type)`
- `Boolean IsWorkerThread()`

### `IProgress` (Namespace: `TFlex.DOCs.Model`)
**Методы:**
- `Void Report(String description)`

### `LoadingCallback` (Namespace: `TFlex.DOCs.Model`)
**Методы:**
- `Task Invoke(ServerConnection connection, IProgress waiting, CancellationToken cancellation)`
- `IAsyncResult BeginInvoke(ServerConnection connection, IProgress waiting, CancellationToken cancellation, AsyncCallback callback, Object object)`
- `Task EndInvoke(IAsyncResult result)`

### `OpenIdProvider` (Namespace: `TFlex.DOCs.Model`)
**Свойства:** Id: Guid, Name: String, Authority: String, ClientId: String, Scope: String, RedirectPath: String

### `ParameterGroupManager` (Namespace: `TFlex.DOCs.Model`)
**Свойства:** Guid: Guid, Id: Int32, OneToOneParameters: ParameterInfoCollection, RequiredParameters: ReadOnlyCollection`1, Events: EventCollection, OneToOneTables: ParameterGroupCollection, OneToOneLinks: ParameterGroupCollection, OneToOneLinksToComplexHierarchy: ParameterGroupCollection, OneToManyTables: ParameterGroupCollection, OneToManyLinks: ParameterGroupCollection, OneToManyLinksToComplexHierarchy: ParameterGroupCollection, AnyReferenceLinks: ParameterGroupCollection, SearchQueryLinks: ParameterGroupCollection
**Методы:**
- `List`1 GetEventHandlers(ParameterGroupEvent event)`
- `SigningParametersInfo GetSigningParametersInfo()` [has Async]
- `Void SetSigningParameters(List`1 signingParameters)` [has Async]
- `ParameterGroupCollection GetRelations()`
- `ParameterGroup FindRelation(Int32 groupId) (+1)`
- `ParameterGroupCollection GetLinks()` [has Async]
- `ParameterGroupCollection GetCorruptedLinks()`
- `List`1 GetRequiredParameters()`
- `ParameterGroupCollection GetOneToOneRelations()` [has Async]
- `ParameterGroupCollection GetOneToManyRelations(Boolean includeAnyReferenceLinks) (+1)` [has Async]
- `ParameterGroupCollection GetAllGroups()`
- `ParameterGroupCollection GetSwappedLinks()`
- `ParameterGroup FindSwappedLink(Int32 groupId) (+1)`
- `ParameterGroupCollection GetSwappedToOneLinks()`
- `ParameterGroupCollection GetSwappedToManyLinks()`
- `ParameterGroup FindOneToManyTable(Int32 id) (+2)`

### `ReferenceCatalog` (Namespace: `TFlex.DOCs.Model`)
**Свойства:** Connection: ServerConnection, RootFolder: ReferenceCatalogFolder, IsPDMInstalled: Boolean, IsDataExchangeInstalled: Boolean, IsLibraryInstalled: Boolean, IsNSIClassifierInstalled: Boolean, Root: ReferenceCatalogFolder
**Методы:**
- `Void Load()` [has Async]
- `List`1 GetReferences()`
- `List`1 GetFolders()`
- `ReferenceInfo Find(Int32 referenceId) (+3)`
- `ReferenceCatalogFolder FindFolderByName(String name)`
- `ReferenceCatalogFolder FindFolder(String fullName) (+1)`
- `Void RegisterSpecialReferenceObject(Guid parameterGroupGuid, Guid classObjectGuid, Type specialReferenceObjectType)`
- `Void RegisterSpecialReference(Guid parameterGroupGuid, SpecialReferenceFactory referenceFactory)`
- `List`1 GetSystemEventHandlers(Guid parameterGroupGuid)`
- `Boolean RegisterSystemEventHandler(ISystemEventHandlerProvider handler)`
- `Void Reload()`
- `List`1 GetAllReferences()`
- `ReferenceInfo FindReference(Int32 referenceId) (+3)`
- `Boolean IsLocked(Int32 referenceId) (+2)`
- `ReferenceBuilder CreateReferenceBuilder(ReferenceCatalogFolder folder)`
- `ReferenceCatalogFolder CreateFolder(String name, ReferenceCatalogFolder parent)`
- `Void RenameFolder(ReferenceCatalogFolder folder, String name)`
- `Void DeleteFolder(ReferenceCatalogFolder folder)`
- `Void MoveFolder(ReferenceCatalogFolder folder, ReferenceCatalogFolder newParent)`

### `ReferenceCatalogFolder` (Namespace: `TFlex.DOCs.Model`)
**Свойства:** Catalog: ReferenceCatalog, Id: Int32, Guid: Guid, Parent: ReferenceCatalogFolder, Folders: ReadOnlyCollection`1, FullName: String, References: ReadOnlyCollection`1, Name: String
**Методы:**
- `ReferenceCatalogFolder CreateFolder(String name)` [has Async]
- `Void Rename(String name)` [has Async]
- `Void Delete()` [has Async]
- `Void MoveTo(ReferenceCatalogFolder newParent)` [has Async]
- `Boolean Contains(ReferenceCatalogFolder folder)`
- `Boolean ContainsReferences()`
- `Int32 CompareTo(ReferenceCatalogFolder other)`
- `Boolean IsEmpty()`

### `ReferenceInfo` (Namespace: `TFlex.DOCs.Model`)
**Свойства:** Connection: ServerConnection, Catalog: ReferenceCatalog, Parent: ReferenceCatalogFolder, Description: ParameterGroup, DescriptionLoaded: Boolean, Classes: ClassTree, IsStatic: Boolean, Id: Int32, HierarchyGroupId: Int32, Guid: Guid, Name: String, Icon: IconImage, IconLoaded: Boolean, Visibility: ReferenceVisibility, HierarchyType: ReferenceHierarchyType, ActivityStatus: GroupActivityStatus, SupportsDesktop: Boolean, SupportsRecycleBin: Boolean, SupportsNomenclature: Boolean, HasHierarchy: Boolean, SupportsObjectsInstances: Boolean, IsObjectsInstancesImpl: Boolean, SupportsStructureTypes: Boolean
**Методы:**
- `ParameterGroup RefreshDescription()` [has Async]
- `Reference CreateReference(Boolean prototypeMode) (+1)` [has Async]
- `Void RefreshStaticReference()`
- `Int32 CompareTo(ReferenceInfo other)`
- `LicenseFunction GetLicense(ClassObject classObject)`
- `Boolean HasTypesLicense()`
- `LicenseFunction GetStructureEditLicense()`

### `ServerConnection` (Namespace: `TFlex.DOCs.Model`)
**Свойства:** IsWebServer: Boolean, ConnectionParameters: ConnectionParameters, ClientView: ClientView, CachingFileServer: CachingFileServerObject, WorkingFolder: String, IsConnected: Boolean, IsDemo: Boolean, LdapUrl: String, IsAdministrator: Boolean, IsSystem: Boolean, Version: Version, EvaluationTime: Nullable`1, UpdateVersion: Version, UpdateRequestRequired: Boolean, Callback: IModelCallback, ServerName: String, InstanceName: String, CurrentConfiguration: BaseConfiguration, DefaultConfiguration: ClientConfiguration, FullTextSearchEnabled: Boolean, SupportCertificates: Boolean, FilePreviewers: FilePreviewers, ViewerService: ViewerService, ToolsManager: CommonToolsManager, Mail: MailService, ServerTaskManager: ServerTaskManager, BytesSent: Int64, BytesReceived: Int64, UnzippedBytesSent: Int64, UnzippedBytesReceived: Int64, SentPercentsZipped: Double, ReceivedPercentsZipped: Double, ReferenceCatalog: ReferenceCatalog, IsReferenceCatalogLoaded: Boolean, References: SystemReferences, EventWatcher: EventWatcher, SaveSettingsOnClose: Boolean, Stages: List`1, Schemes: List`1, SignatureTypes: List`1, AccessGroups: List`1, Logging: LogManager, SettingsManager: SettingsManager, ClientViews: List`1, Licenses: LicenseManager, ReferencesStorageCollection: ReferencesStorageCollection, IsCertificateItemsEmpty: Boolean, ExtendedParameters: ExtendedParametersManager, ConfigurationSettings: ConfigurationSettings, OmitHasChildrenCheck: Boolean
**Методы:**
- `Boolean HasLicense(LicenseFunction function)`
- `IList`1 GetCertificatesInfo()`
- `Void CloseCertificate(Certificate certificate) (+1)` [has Async]
- `Void CloseCertificates()` [has Async]
- `Void SendCertificates()` [has Async]
- `Void RefreshStages()`
- `Void RefreshSchemes()`
- `Void RefreshSignatureTypes()`
- `Void RefreshAccessGroups()`
- `Void RefreshClientViews()`
- `CertificateSessionItem GetOpenedCertificate(Guid certificateGuid)`
- `Boolean IsCertificateOpened(Guid certificateGuid, Boolean checkExpiration)`
- `String GetCertificateKeyName(Guid certificateGuid)`
- `Void ClearCertificateItems()`
- `Void AddCertificateItem(CertificateSessionItem item)`
- `Void SetRequestLogger(ILogger logger)`
- `Void SetWebServerMode(String actualFileFolder, String workingFileFolder)`
- `Void SubscribeAccessChanges()` [has Async]
- `ServerConnection Open(String userName, MD5HashString password, String server, Nullable`1 configurationGuid, CommunicationMode communication, DataSerializerAlgorithm dataSerializer, CompressionAlgorithm compression, IWebProxy proxy) (+6)` [has Async]
- `Boolean Prepare(Boolean reregister, Nullable`1 configuration, LoadingCallback loadingCallback)`
- `ServerConnection OpenWithToken(String server, String accessToken, Nullable`1 configurationGuid, CommunicationMode communication, DataSerializerAlgorithm dataSerializer, CompressionAlgorithm compression, IWebProxy proxy) (+1)` [has Async]
- `ServerConnection OpenWithOidcToken(String server, String oidcToken, Guid oidcProvider, Nullable`1 configurationGuid, CommunicationMode communication, DataSerializerAlgorithm dataSerializer, CompressionAlgorithm compression, IWebProxy proxy) (+1)` [has Async]
- `AuthToken GenerateToken(String userName, MD5HashString password, String server, String clientId, Nullable`1 configurationGuid, CommunicationMode communication, DataSerializerAlgorithm dataSerializer, CompressionAlgorithm compression, IWebProxy proxy) (+3)` [has Async]
- `AuthToken RefreshToken(String server, String refreshToken, String clientId, CommunicationMode communication, DataSerializerAlgorithm dataSerializer, CompressionAlgorithm compression, IWebProxy proxy) (+2)` [has Async]
- `List`1 GetConfigurations(String userName, MD5HashString password, String server, CommunicationMode communication, DataSerializerAlgorithm dataSerializer, CompressionAlgorithm compression, ConfigurationType configurationType) (+4)` [has Async]
- `List`1 GetOpenIdProviders(String server, CommunicationMode communication, DataSerializerAlgorithm dataSerializer, CompressionAlgorithm compression) (+1)` [has Async]
- `Void Close()` [has Async]
- `Boolean SetConnectionParameters()`
- `Boolean RunUpdate(ConcurrencyException exception, Object ownerWindow)`
- `String GetLocalStorageFolderPath(String folderName)`
- `String GetWorkingFolder(DesignContextObject designContextObject)` [has Async]
- `String GetDefaultSystemWorkingFolderPath()`
- `Void SetWorkingFolder(String path)` [has Async]
- `List`1 GetActiveConnections()` [has Async]
- `Void CloseUserConnection(IEnumerable`1 connections)` [has Async]
- `List`1 GetAvailableLicenses()` [has Async]
- `LicenseKeyInfo GetLicenseKeyInfo()` [has Async]
- `List`1 GetClientConfigurations()` [has Async]
- `List`1 GetWebConfigurations()` [has Async]
- `Boolean ExportConfigurations(Stream stream, IEnumerable`1 configurations)` [has Async]
- `List`1 ImportConfigurations(Stream stream)` [has Async]

### `ServerConnectionExtensions` (Namespace: `TFlex.DOCs.Model`)
**Методы:**
- `Reference CreateReference(ServerConnection connection, Int32 referenceId, Boolean prototypeMode) (+5)` [has Async]
- `Void ClearWorkingFolder(ServerConnection connection)` [has Async]

### `ServerDiscovery` (Namespace: `TFlex.DOCs.Model`)
**Методы:**
- `ServiceSpecification Find(String serverAddress, Boolean udpDiscovery, TimeSpan searchTime)` [has Async]
- `Int32 GetServerVersion(ServiceSpecification service)`

### `ServerGateway` (Namespace: `TFlex.DOCs.Model`)
**Свойства:** Connection: ServerConnection, ConnectionParameters: ConnectionParameters, IsConnected: Boolean, IsDemo: Boolean, Version: Version, EvaluationTime: Nullable`1, Callback: IModelCallback, ServerName: String, CurrentConfiguration: BaseConfiguration, Mail: MailService, BytesSent: Int64, BytesReceived: Int64, UnzippedBytesSent: Int64, UnzippedBytesReceived: Int64, SentPercentsZipped: Double, ReceivedPercentsZipped: Double
**Методы:**
- `Void Connect(String userName, MD5HashString password, String server, Nullable`1 configurationGuid, CommunicationMode communication, DataSerializerAlgorithm dataSerializer, CompressionAlgorithm compression, IWebProxy proxy) (+7)`
- `Void Disconnect()`
- `List`1 GetActiveConnections()`
- `List`1 GetAvailableLicenses()`
- `LicenseKeyInfo GetLicenseKeyInfo()`
- `List`1 GetClientConfigurations()`
- `List`1 GetWebConfigurations()`
- `List`1 GetConfigurations()`
- `Boolean ExportConfigurations(Stream stream, IEnumerable`1 configurations) (+1)`
- `List`1 ImportConfigurations(Stream stream)`

### `SpecialReferenceFactory` (Namespace: `TFlex.DOCs.Model`)
**Свойства:** IsStatic: Boolean, ReferenceType: Type, ClassTreeType: Type
**Методы:**
- `Void LoadRequiredParameters(LoadSettings settings, Boolean oneToOneLink)`
- `Filter GetSpecialFilter(SpecialFilterArgs args)`

### `SpecialReferenceFactory`2` (Namespace: `TFlex.DOCs.Model`)
**Свойства:** ReferenceType: Type, ClassTreeType: Type
**Методы:**
- `TReference CreateReference(ParameterGroup masterGroup)`
- `TClassTree CreateClassTree(ParameterGroup masterGroup)`

### `StateCollection`1` (Namespace: `TFlex.DOCs.Model`)
**Свойства:** IsModified: Boolean, Item: T, Count: Int32, IsReadOnly: Boolean
**Методы:**
- `Void Rollback()`
- `IEnumerable`1 GetAddedItems()`
- `IEnumerable`1 GetDeletedItems()`
- `Int32 IndexOf(T item)`
- `Void Insert(Int32 index, T item)`
- `Void RemoveAt(Int32 index)`
- `Void Add(T item)`
- `Void Clear()`
- `Boolean Contains(T item)`
- `Void CopyTo(T[] array, Int32 arrayIndex)`
- `Boolean Remove(T item)`
- `IEnumerator`1 GetEnumerator()`

### `StateGuidDomainObject` (Namespace: `TFlex.DOCs.Model`)
**Свойства:** IsAdded: Boolean, IsModified: Boolean, IsDeleted: Boolean, IsChanged: Boolean

### `SystemReferences` (Namespace: `TFlex.DOCs.Model`)
**Свойства:** Connection: ServerConnection, Certificates: CertificateReference, Credentials: CredentialsReference, Users: UserReference, GlobalParameters: GlobalParameterReference, Macros: MacroReference, Reports: ReportReference, Units: UnitReference, WorkingAreas: WorkingAreaReference, NavigationPanel: NavigationPanelReference, AssignmentFolders: AssignmentFolderReference, FileServers: FileServerReference, CachingFileServers: CachingFileServerReference, StructureTypes: StructureTypesReference, DesignContexts: DesignContextsReference, ProductsClassifier: ProductsClassifierReference, ProductOptions: ProductOptionsReference, ProductOptionValues: ProductOptionValuesReference, RevisionNamingRules: RevisionNamingRulesReference, ProductsApplicability: ProductsApplicabilityReference, TypicalConfigurationSettings: TypicalConfigurationSettingsReference, MasterDataServers: Reference, SearchQueries: SearchQueryReference, Configurators: ConfiguratorsReference, UserDialogs: UserDialogsReference, ProductsInstances: ProductsInstancesReference, ProductDesignNumbers: ProductsDesignNumbersReference, ProductsMilestones: ProductsMilestonesReference, ProductCategories: CategoriesReference, DataModels: DataModelsReference, NomParametersSynchro: NomParametersSynchroReference, StructureVariants: StructureVariantsReference, OptionsTableSet: OptionsTableSetReference

### `AccessCommand` (Namespace: `TFlex.DOCs.Model.Access`)
**Свойства:** Id: Int32, Name: String, Type: AccessType

### `AccessGroup` (Namespace: `TFlex.DOCs.Model.Access`)
**Свойства:** Connection: ServerConnection, Id: Int32, Guid: Guid, Name: String, Type: AccessType, IsTemporary: Boolean, IsModified: Boolean, CanEdit: Boolean, CanDelete: Boolean, Changing: Boolean
**Методы:**
- `Void SetIsModifiedOn()`
- `Boolean SetCommandState(AccessCommand command, AccessCommandState state)`
- `Boolean IsAllowed(AccessCommand command)`
- `Boolean IsForbidden(AccessCommand command)`
- `AccessCommandState GetCommandState(AccessCommand command)`
- `Void BeginChanges()`
- `Boolean EndChanges()` [has Async]
- `Void CancelChanges()`
- `Void Delete()`
- `IEnumerator`1 GetEnumerator()`
- `List`1 GetGroups(ServerConnection connection)` [has Async]
- `AccessGroup Find(ServerConnection connection, Int32 id) (+2)`

### `AccessGroupCommand` (Namespace: `TFlex.DOCs.Model.Access`)
**Свойства:** Command: AccessCommand, State: AccessCommandState

### `AccessInfo` (Namespace: `TFlex.DOCs.Model.Access`)
**Свойства:** Editable: Boolean, Owner: UserReferenceObject, Access: AccessGroup, Link: ParameterGroup, StageID: Int32, IsAdded: Boolean, IsModified: Boolean, InheritedFrom: AccessInheritedFrom, AccessDirection: AccessDirection, StartDate: Nullable`1, EndDate: Nullable`1, xAccessObjectID: Int32, xReferenceID: Int32, CommandType: AccessCommandType, AccessTypeID: AccessTypeID, AutoGeneratePK: Boolean, PrimaryKey: Int32
**Методы:**
- `Void Clear()`
- `Void Assign(Object source)`
- `Void ResetAccessGroupKey()`
- `Boolean CanBeEditable(AccessTypeID editorTypeID, Boolean isInherited, Boolean isStageEditor)`
- `Boolean MustResetPrimaryKey(AccessTypeID editorTypeID, Boolean inherited, Boolean isStageEditor)`

### `AccessManager` (Namespace: `TFlex.DOCs.Model.Access`)
**Свойства:** Object: ReferenceObject, ParentAccessObject: ReferenceObject, ObjectId: Int32, HasParentAccessObject: Boolean, InheritAccess: AccessManager
**Методы:**
- `AccessInfo MakeInheritedFrom(AccessInfo access, AccessTypeID editorTypeID, AccessInheritedFrom from, Boolean inherited)`
- `Void SetInherit(Boolean inherit, Boolean copyInheritAccess)`
- `AccessManager GetSystemAccess(ServerConnection connection)` [has Async]
- `AccessManager GetReferenceAccess(ReferenceInfo reference)` [has Async]
- `AccessManager GetReferenceObjectAccess(ReferenceObject referenceObject, AccessRightsLoadOptions options) (+1)` [has Async]
- `AccessManager GetAllReferenceAccesses(ReferenceInfo reference)` [has Async]
- `AccessManager GetStageAccess(Stage stage)` [has Async]

### `AccessManagerBase` (Namespace: `TFlex.DOCs.Model.Access`)
**Свойства:** AccessTypeID: AccessTypeID, CommandType: AccessCommandType, AllowDuplicates: Boolean, Connection: ServerConnection, Reference: ReferenceInfo, Stage: Stage, ReferenceId: Int32, StageId: Int32, IsModified: Boolean, IsInherit: Boolean
**Методы:**
- `Void ReplacePrimaryKey(Int32 oldKey, Int32 newKey, Boolean replaceInList)`
- `Void SetInherit(Boolean inherit, Boolean copyInheritAccess)`
- `Boolean ExistsAccessInfo(List`1 accesses, Nullable`1 primaryKey, UserReferenceObject owner, AccessGroup group, ParameterGroup link, Nullable`1 commandType, Nullable`1 accessTypeID, Nullable`1 accessDirection, Nullable`1 stageID)`
- `AccessInfo FindAccessInfo(List`1 accesses, Nullable`1 primaryKey, UserReferenceObject owner, AccessGroup group, ParameterGroup link, Nullable`1 commandType, Nullable`1 accessTypeID, Nullable`1 accessDirection, Nullable`1 stageID, Int32 currentObjectId)`
- `List`1 FindAllAccessInfo(List`1 accesses, UserReferenceObject owner, AccessGroup group, ParameterGroup link, Nullable`1 commandType, Nullable`1 accessTypeID, Nullable`1 accessDirection, Nullable`1 stageID)`
- `Void SetAccess(Int32 primaryKey, UserReferenceObject owner, AccessGroup group, AccessGroup oldGroup, ParameterGroup link, AccessCommandType commandType, AccessTypeID accessTypeID, AccessDirection accessDirection, Nullable`1 oldAccessDirection, Nullable`1 startDate, Nullable`1 endDate) (+1)`
- `Void RemoveAccess(Nullable`1 primaryKey, UserReferenceObject owner, AccessGroup group, AccessType type, ParameterGroup link, Nullable`1 accessDirection) (+1)`
- `Boolean Save()` [has Async]
- `IEnumerator`1 GetEnumerator()`

### `AccessRights` (Namespace: `TFlex.DOCs.Model.Access`)
**Свойства:** ReferenceId: Int32, ObjectId: Int32, Type: AccessType, IsInherit: Boolean
**Методы:**
- `Int32 GetAccessRightPk(Int32 accessGroupId)`
- `Boolean IsAllowed(AccessCommand command, ParameterGroup link)`
- `AccessCommandState GetCommandState(AccessCommand command, ParameterGroup link)`
- `IEnumerable`1 GetAccessGroups()`
- `Boolean IsSpecialInstanceCommand(ParameterGroup group, AccessCommand command)`
- `AccessRights GetSystemAccess(ServerConnection connection)`
- `AccessRights GetReferenceAccess(ReferenceInfo reference)`
- `AccessRights GetReferenceObjectAccess(ReferenceInfo reference) (+2)`
- `IReadOnlyList`1 GetReferencesHasExplicitAccessesForUser(UserReferenceObject userObject)`
- `IReadOnlyList`1 GetReferencesStructureAccess(UserReferenceObject userObject, IEnumerable`1 references)`
- `IReadOnlyList`1 GetReferencesObjectsAccess(UserReferenceObject userObject, IEnumerable`1 references)`
- `IReadOnlyList`1 GetReferencesObjectsOwnerAccess(ServerConnection connection, IEnumerable`1 references)`
- `IReadOnlyList`1 FindExplicitlyAccessedObjects(UserReferenceObject userObject, ReferenceInfo reference)`
- `IReadOnlyList`1 FindObjectsWithOwnerAccess(UserReferenceObject userObject, ReferenceInfo reference)`
- `IReadOnlyList`1 LoadReferenceObjectsAccesses(UserReferenceObject userObject, ReferenceInfo reference, IEnumerable`1 objectsIds)`
- `Boolean IsCommandAllowed(AccessCommand command, ReferenceObject referenceObject, ParameterGroup link) (+7)` [has Async]
- `Boolean IsReferenceCommandsAllowed(ReferenceInfo referenceInfo, ServerConnection connection, AccessCommand[] commands)`
- `Boolean HasAdminAccess(ServerConnection connection, User user)` [has Async]

### `AccessType` (Namespace: `TFlex.DOCs.Model.Access`)
**Свойства:** AccessTypeID: AccessTypeID, Type: AccessCommandType, Id: Int32, IsSystem: Boolean, IsReference: Boolean, IsObject: Boolean, IsLink: Boolean, IsStage: Boolean, Name: String, Commands: ReadOnlyCollection`1, Item: AccessCommand, Reference: ReferenceAccessType, Object: ObjectAccessType, Stage: StageAccessType, Link: LinkAccessType, System: SystemAccessType
**Методы:**
- `List`1 GetTypes()`
- `List`1 GetGroups(ServerConnection connection)` [has Async]

### `LinkAccessType` (Namespace: `TFlex.DOCs.Model.Access`)
**Свойства:** AccessTypeID: AccessTypeID, Type: AccessCommandType, Name: String, IsLink: Boolean, Read: AccessCommand, Add: AccessCommand, Remove: AccessCommand

### `ObjectAccessType` (Namespace: `TFlex.DOCs.Model.Access`)
**Свойства:** AccessTypeID: AccessTypeID, Type: AccessCommandType, Name: String, IsObject: Boolean, Read: AccessCommand, Delete: AccessCommand, Change: AccessCommand, CreateChildren: AccessCommand, Print: AccessCommand, ChangeAccess: AccessCommand, Copy: AccessCommand, ClearHistory: AccessCommand, ReadHistory: AccessCommand, ChangeStage: AccessCommand, ChangeSignature: AccessCommand, EditSaveSignature: AccessCommand, ChangeClass: AccessCommand, Moving: AccessCommand, CreateDuplicate: AccessCommand, UnlockReferenceObject: AccessCommand, ChangeOwner: AccessCommand, ObjectEditingRemarks: AccessCommand, ObjectVersionDelete: AccessCommand, ObjectChangeOrderIndex: AccessCommand, ObjectCreateHierarchyLink: AccessCommand, ObjectDeleteHierarchyLink: AccessCommand, ObjectChangeInstances: AccessCommand

### `ReferenceAccessFilters` (Namespace: `TFlex.DOCs.Model.Access`)
**Свойства:** Reference: ReferenceInfo, IsModified: Boolean
**Методы:**
- `Filter GetFilter(UserReferenceObject userObject)`
- `Void SetFilter(UserReferenceObject userObject, Filter filter)`
- `Boolean RemoveFilter(UserReferenceObject userObject)`
- `Boolean Save()`
- `Void Clear()`
- `IEnumerator`1 GetEnumerator()`

### `ReferenceAccessType` (Namespace: `TFlex.DOCs.Model.Access`)
**Свойства:** AccessTypeID: AccessTypeID, Type: AccessCommandType, Name: String, IsReference: Boolean, ChangeStructure: AccessCommand, ChangeExtendedParameters: AccessCommand, ChangeAccess: AccessCommand, Delete: AccessCommand, Export: AccessCommand, TablePaste: AccessCommand, Display: AccessCommand, ReferenceShowInCatalog: AccessCommand, ChangeWindowSettings: AccessCommand, EditCommonViews: AccessCommand, EditPersonalViews: AccessCommand, EditCommonCatalogs: AccessCommand, EditPersonalCatalogs: AccessCommand

### `ReferenceObjectSetAccessManager` (Namespace: `TFlex.DOCs.Model.Access`)
**Свойства:** Objects: IReadOnlyCollection`1
**Методы:**
- `Void SetInherit(Boolean inherit, Boolean copyInheritAccess)`
- `ReferenceObjectSetAccessManager GetReferenceObjectsAccess(IEnumerable`1 referenceObjects, AccessRightsLoadOptions options)` [has Async]

### `StageAccessType` (Namespace: `TFlex.DOCs.Model.Access`)
**Свойства:** Name: String, AccessTypeID: AccessTypeID, IsStage: Boolean

### `SystemAccessType` (Namespace: `TFlex.DOCs.Model.Access`)
**Свойства:** AccessTypeID: AccessTypeID, Type: AccessCommandType, Name: String, IsSystem: Boolean, CreateReference: AccessCommand, EditCommonWorkingPages: AccessCommand, EditPersonalWorkingPages: AccessCommand, EditCommonSearchConditions: AccessCommand, EditPersonalSearchConditions: AccessCommand, EditCommonFilters: AccessCommand, EditPersonalFilters: AccessCommand, EditCommonViews: AccessCommand, EditPersonalViews: AccessCommand, EditCommonCatalogs: AccessCommand, EditPersonalCatalogs: AccessCommand, EditConfigurations: AccessCommand, EditEmailAccounts: AccessCommand, DevelopmentCommonPrototypeLinearBusinessProcesses: AccessCommand, EditCommonEventHandlers: AccessCommand, EditPersonalEventHandlers: AccessCommand, ManageServerTasks: AccessCommand, SubsystemAdministrator: AccessCommand, ObjectChangeMasterServer: AccessCommand

### `ClassGroupSettings` (Namespace: `TFlex.DOCs.Model.Classes`)
**Свойства:** IsInherit: Boolean, Connection: ServerConnection
**Методы:**
- `String Serialize()`
- `Void Deserialize(String data)`

### `ClassObject` (Namespace: `TFlex.DOCs.Model.Classes`)
**Свойства:** Classes: ClassTree, Id: Int32, Guid: Guid, Base: ClassObject, ChildClasses: ClassObjectCollection, CanContainChildren: Boolean, Icon: IconImage, IconLoaded: Boolean, Name: String, Comment: String, SupportsSaveAndCreate: Boolean, ShowChangeCommandInObjectProperties: Nullable`1, IsAbstract: Boolean, IsSealed: Boolean, UseBaseClassIcon: Boolean, CanCreateInRoot: Boolean, InheritRevisionNamingRule: Boolean, RevisionNamingRule: RevisionNamingRuleObject, PropertiesDisplayType: PropertiesDisplayType, CreateFromPrototype: Boolean, Hidden: Boolean, CanCreateObjects: Boolean, CanEdit: Boolean, CanDelete: Boolean, IsStandaloneProductByDefault: Boolean, ChildObjectClasses: ClassObjectCollection, InheritMasterObjectClasses: Boolean, MasterObjectClasses: ClassObjectCollection, ParameterGroups: ParameterGroupCollection, SwappedToSelfParameterGroups: ParameterGroupCollection, Attributes: ClassObjectAttributes, SigningParameters: String, IsUniqueIndexInherit: Boolean, UniqueIndex: UniqueIndex, Dialog: Dialog, WebDialog: Dialog, HierarchyLinkDialog: Dialog, HierarchyLinkWebDialog: Dialog, IsSupportMultiAttachmentInherit: Boolean, SupportMultiAttachment: Boolean, IsSchemeInherit: Boolean, IsDefaultStageInherit: Boolean, Scheme: Scheme, DefaultStage: SchemeStage, HasLinkedNomenclatureType: Boolean, LinkedNomenclatureType: NomenclatureType, InheritCanChange: Boolean, CanChange: Boolean, XmlId: String, ObjectFormat: ObjectFormat
**Методы:**
- `Boolean CanCreateChildObject(ClassObject classObject)`
- `Boolean GetShowChangeCommandWithInheritance()`
- `ParameterGroupCollection GetUnattachedParameterGroups()`
- `T GetGroupSettings(Guid settingsId, ParameterGroup group)` [has Async]
- `Boolean SetGroupSettings(Guid settingsId, ParameterGroup group, T settings)` [has Async]
- `Boolean ClearGroupSettings(Guid settingsId, ParameterGroup group)` [has Async]
- `ClassObjectCollection GetParentObjectClasses()`
- `Boolean IsInherit(ParameterGroup group) (+3)`
- `ClassObject GetBaseClassOfGroup(ParameterGroup group)`
- `Boolean IsBaseClassFor(ClassObject classObject)`
- `Boolean CanChangeTo(ClassObject other)`
- `ClassObjectCollection GetAllChildClasses()`
- `Void SetEventHandlersInheritance(ParameterGroupEvent event, Nullable`1 inheritEventHandlers)` [has Async]
- `Nullable`1 GetEventHandlersInheritance(ParameterGroupEvent event)`
- `List`1 GetEventHandlers(ParameterGroupEvent event)`
- `Int32 CompareTo(ClassObject other) (+1)`
- `Boolean TryParseXmlId(String xmlId, Int32& id, Boolean& deleted)`
- `List`1 GetSigningParametersGuids()`
- `SigningParametersInfo GetSigningParametersInfo()` [has Async]
- `Void SetSigningParameters(List`1 signingParameters)` [has Async]

### `ClassObjectAttribute` (Namespace: `TFlex.DOCs.Model.Classes`)
**Свойства:** Attributes: ClassObjectAttributes, Name: String, IsSystem: Boolean, CanSerialize: Boolean, CanChangeCaption: Boolean, Caption: String, Value: Object, IsReadOnly: Boolean, CanRemove: Boolean
**Методы:**
- `Void SetModified()`

### `ClassObjectAttribute`1` (Namespace: `TFlex.DOCs.Model.Classes`)
**Свойства:** CanChangeCaption: Boolean, IsSystem: Boolean, Caption: String, Value: Object, CanRemove: Boolean

### `ClassObjectAttributes` (Namespace: `TFlex.DOCs.Model.Classes`)
**Свойства:** Class: ClassObject, BaseClass: ClassObject, Item: ClassObjectAttribute, IsReadOnly: Boolean, IsModified: Boolean, Count: Int32
**Методы:**
- `Void AddAttribute(String name, ClassObjectAttribute attr)`
- `Boolean RemoveAttribute(String name)`
- `Boolean IsValid(String name)`
- `Void Validate(String name)`
- `T GetValue(String name)`
- `Boolean SetValue(String name, Object value)`
- `Boolean Contains(String name)`
- `Boolean TryGetAttribute(String name, ClassObjectAttribute& attr)`
- `IEnumerator`1 GetEnumerator()`

### `ClassTree` (Namespace: `TFlex.DOCs.Model.Classes`)
**Свойства:** AllClasses: ClassObjectCollection, Owner: ParameterGroup, BaseClasses: ClassObjectCollection, ContainsFolderClasses: Boolean
**Методы:**
- `ClassObject Find(Int32 classId) (+2)`
- `ClassObjectCollection GetRootClasses()`
- `ClassObjectCollection GetRootBaseClasses()`
- `ClassObjectCollection GetNotAbstractClasses()`
- `ClassObjectCollection GetFolderClasses()`
- `ClassObjectCollection GetParameterGroupClasses(ParameterGroup parameterGroup, Boolean includeInherit)`
- `IEnumerable`1 GetIndexClasses(UniqueIndex index)`

### `PropertiesDisplayTypeExtensions` (Namespace: `TFlex.DOCs.Model.Classes`)
**Методы:**
- `String GetName(PropertiesDisplayType type)`

### `SpecialClassTree`1` (Namespace: `TFlex.DOCs.Model.Classes`)
**Методы:**
- `TClass Find(Int32 classId) (+2)`
- `IEnumerator`1 GetEnumerator()`

### `BaseConfiguration` (Namespace: `TFlex.DOCs.Model.Configuration`)
**Свойства:** Connection: ServerConnection, Guid: Guid, Name: String, Comment: String, Title: String, ConfiguratorGuid: Guid, Icon: IconImage, AccessUsersUseType: ItemListUseType, ReferencesUseType: ItemListUseType, CanEdit: Boolean, CanEditProperties: Boolean, CanDelete: Boolean, AsClientConfiguration: ClientConfiguration, AsWebConfiguration: WebConfiguration
**Методы:**
- `Image GetImage()`
- `Byte[] GetImageBytes()`
- `Void SetImage(Image image)`
- `List`1 GetAccessUsers()`
- `Void SetAccessUsers(IEnumerable`1 users)`
- `Boolean IsAccessibleFor(UserReferenceObject user)`
- `List`1 GetReferences()` [has Async]
- `Void SetReferences(IEnumerable`1 references)`
- `Boolean SupportsReference(ReferenceInfo reference) (+1)`
- `List`1 GetAvailableLicenses()`
- `Dictionary`2 GetLicenses()`
- `Void SetLicenses(Dictionary`2 data)`
- `Void Save()`
- `ClientConfiguration ToServerData()`
- `Boolean Delete()`

### `ClientConfiguration` (Namespace: `TFlex.DOCs.Model.Configuration`)
**Свойства:** IsDefault: Boolean, RegistryName: String, RegistryKey: String, CurrentRegistryKey: String, CommandLineArguments: String, MainWindowIcon: IconImage, SkinName: String, MainWindowTitle: String, LoginWindowTitle: String, Default: ClientConfiguration, AsClientConfiguration: ClientConfiguration
**Методы:**
- `Image GetLoginWindowImage()`
- `Byte[] GetLoginWindowImageBytes()`
- `Void SetLoginWindowImage(Image image)`
- `ClientConfiguration ToServerData()`

### `CommandDisplayModeExtensions` (Namespace: `TFlex.DOCs.Model.Configuration`)
**Методы:**
- `String GetName(CommandDisplayMode mode)`

### `DialogConfigurationUseTypeExtensions` (Namespace: `TFlex.DOCs.Model.Configuration`)
**Методы:**
- `String GetConfigurationUseTypeName(ConfigurationUseType type)`

### `ISettingsContainer` (Namespace: `TFlex.DOCs.Model.Configuration`)
**Свойства:** Application: String, Context: String, Exists: Boolean, Interface: String, IsNew: Boolean, IsCommon: Boolean, IsLoaded: Boolean, ObjectId: Int32, ParameterGroupId: Int32, FolderGuid: Guid, SharingType: SettingsSharingType, SupportsViews: Boolean, Data: Object, IsLocked: Boolean, CurrentView: ISettingsView, DefaultView: ISettingsView, Views: ReadOnlyCollection`1
**Методы:**
- `Void Lock()`
- `Void Unlock()`
- `Void Clear()`
- `Void Reset()`
- `Void Save()` [has Async]
- `Boolean Load()` [has Async]
- `Boolean Remove()`
- `Boolean Reload(Boolean reloadViews)` [has Async]
- `Void ReloadViews()`
- `Void ApplyViewOnLoading(ISettingsView view)`
- `Void ApplyView(ISettingsView view)`
- `ISettingsView CreateView(String name, SettingsViewType type, Boolean inSettingsContext, Boolean copySettings, ConfigurationUseType configurationUseType, List`1 configurations, SettingsViewAccessType accessType, List`1 users, Boolean showOpenAsCommand, String comment) (+1)`
- `Boolean DeleteView(ISettingsView view)`
- `Void UpdateView(ISettingsView view, String name, SettingsViewType type, Boolean inSettingsContext, Boolean copySettings, ConfigurationUseType configurationUseType, List`1 configurations, SettingsViewAccessType accessType, List`1 users, Boolean showOpenAsCommand, String comment) (+1)`

### `ISettingsView` (Namespace: `TFlex.DOCs.Model.Configuration`)
**Свойства:** Id: Guid, IsDefault: Boolean, IsInSettingsContext: Boolean, IsShared: Boolean, IsDefaultForAllConfigurations: Boolean, ConfigurationUseType: ConfigurationUseType, Configurations: ReadOnlyCollection`1, AccessType: SettingsViewAccessType, Users: ReadOnlyCollection`1, Name: String, Comment: String, Type: SettingsViewType, ShowOpenAsCommand: Boolean, Owner: ISettingsContainer
**Методы:**
- `Boolean CanUseInCurrentConfiguration()`
- `String GetData()`

### `ItemListUseTypeExtensions` (Namespace: `TFlex.DOCs.Model.Configuration`)
**Методы:**
- `String GetName(ItemListUseType type)`

### `MdiTypeExtension` (Namespace: `TFlex.DOCs.Model.Configuration`)
**Методы:**
- `String GetText(MdiType mdiType)`
- `IconImage GetIcon(MdiType mdiType)`
- `AccessCommand GetAccessCommand(MdiType mdiType)`
- `Boolean IsAdministrationMdiType(MdiType mdiType)`

### `RawSettingsView` (Namespace: `TFlex.DOCs.Model.Configuration`)
**Свойства:** Id: Guid, Name: String, ConfigurationUseType: ConfigurationUseType, Configurations: ReadOnlyCollection`1, Type: SettingsViewType, InSettingsContext: Boolean

### `SettingsContainer`1` (Namespace: `TFlex.DOCs.Model.Configuration`)
**Свойства:** ExplicitLoading: Boolean, Connection: ServerConnection, Application: String, Interface: String, Context: String, ParameterGroupId: Int32, ObjectId: Int32, FolderGuid: Guid, SupportsViews: Boolean, SharingType: SettingsSharingType, IsCommon: Boolean, IsLoaded: Boolean, IsNew: Boolean, Exists: Boolean, Data: TSettingsData, CurrentApplicationName: String, IsLocked: Boolean, Views: ReadOnlyCollection`1, CurrentView: SettingsView`1, DefaultView: SettingsView`1
**Методы:**
- `Void Lock()`
- `Void Unlock()`
- `Boolean Load()` [has Async]
- `Void Clear()`
- `Void ReloadViews()`
- `Void Reset()`
- `Boolean Reload(Boolean reloadViews)` [has Async]
- `Void Save()` [has Async]
- `Boolean Remove()`
- `SettingsView`1 CreateView(String name, SettingsViewType type, Boolean inSettingsContext, Boolean copySettings, ConfigurationUseType configurationUseType, List`1 configurations, SettingsViewAccessType accessType, List`1 users, Boolean showOpenAsCommand, String comment)`
- `Void UpdateView(SettingsView`1 view, String name, SettingsViewType type, Boolean inSettingsContext, Boolean copySettings, ConfigurationUseType configurationUseType, List`1 configurations, SettingsViewAccessType accessType, List`1 users, Boolean showOpenAsCommand, String comment)`
- `Boolean DeleteView(SettingsView`1 view)`
- `Void ApplyView(SettingsView`1 view) (+1)`
- `ICollection`1 GetSharedViews(ServerConnection connection, Int32 parameterGroupId, String interface, String application, String contextName)` [has Async]
- `ICollection`1 GetShowOpenAsCommandViews(ServerConnection connection, Int32 parameterGroupId, String interface, String application, String contextName, CancellationToken token)` [has Async]
- `Void RegisterSettingsType()`

### `SettingsManager` (Namespace: `TFlex.DOCs.Model.Configuration`)
**Методы:**
- `String GetSettingsData(SettingsSharingType settingsSharingType, Int32 groupId, String interfaceName, String appInstance, Guid folderGuid, Int32 objectId, String context, Guid viewGuid)` [has Async]
- `IReadOnlyCollection`1 GetViews(SettingsSharingType settingsSharingType, Int32 groupId, String interfaceName, String appInstance, Guid folderGuid, Int32 objectId, String context)` [has Async]
- `IReadOnlyCollection`1 GetAllViews(ParameterGroup referenceGroup)` [has Async]
- `RawSettingsView CreateView(String viewName, SettingsViewType type, ConfigurationUseType configurationUseType, List`1 configurations, Boolean inSettingsContext, SettingsSharingType sharingType, String data, Int32 groupId, String interfaceName, String appInstance, Guid folderGuid, Int32 objectId, String context)` [has Async]
- `Guid UpdateView(Guid viewId, String data, String viewName, SettingsViewType type, ConfigurationUseType configurationUseType, List`1 configurations, Boolean inSettingsContext, SettingsSharingType sharingType, Int32 groupId, String interfaceName, String appInstance, Guid folderGuid, Int32 objectId, String context) (+1)` [has Async]
- `Boolean DeleteView(Guid id)` [has Async]

### `SettingsView`1` (Namespace: `TFlex.DOCs.Model.Configuration`)
**Свойства:** Owner: SettingsContainer`1, Id: Guid, Name: String, Comment: String, Type: SettingsViewType, IsDefault: Boolean, IsShared: Boolean, IsInSettingsContext: Boolean, ConfigurationUseType: ConfigurationUseType, Configurations: ReadOnlyCollection`1, DefaultConfigurations: ReadOnlyCollection`1, IsDefaultForAllConfigurations: Boolean, ShowOpenAsCommand: Boolean, AccessType: SettingsViewAccessType, Users: ReadOnlyCollection`1
**Методы:**
- `Boolean CanUseInCurrentConfiguration()`
- `Boolean CanUseInConfiguration(BaseConfiguration configuration)`
- `String GetData()`
- `Int32 CompareTo(SettingsView`1 other) (+1)`

### `SettingsViewAccessTypeExtensions` (Namespace: `TFlex.DOCs.Model.Configuration`)
**Методы:**
- `String GetName(SettingsViewAccessType type)`

### `SettingsViewTypeExtensions` (Namespace: `TFlex.DOCs.Model.Configuration`)
**Методы:**
- `String GetName(SettingsViewType type)`

### `WebConfiguration` (Namespace: `TFlex.DOCs.Model.Configuration`)
**Свойства:** RootMenuLink: MenuLinkGroup, IsVisibleControlPanel: Boolean, AsWebConfiguration: WebConfiguration
**Методы:**
- `ClientConfiguration ToServerData()`
- `Void LoadLinks()`

### `WindowsInCompatibilityMode` (Namespace: `TFlex.DOCs.Model.Configuration.Compatibility`)
**Свойства:** Connection: ServerConnection
**Методы:**
- `WindowsInCompatibilityMode Get(ServerConnection connection)` [has Async]
- `Boolean OpenInCompatibilityMode(Guid windowId)`
- `Boolean OpenReferenceInCompatibilityMode(Int32 referenceId)`
- `Boolean OpenWorkingPageInCompatibilityMode(Int32 workingPageId)`
- `List`1 GetWindows()`
- `Void Reload()` [has Async]
- `Void Save(ICollection`1 windowsInCompatibilityMode)`
- `Void Clear()`

### `LiteConfigurationExt` (Namespace: `TFlex.DOCs.Model.Configuration.Configurations.Extensions`)
**Методы:**
- `Boolean IsLite(BaseConfiguration configuration)`

### `HyperLink` (Namespace: `TFlex.DOCs.Model.Configuration.Configurations.WebConfigurationStructure`)
**Свойства:** Text: String, Resource: String, CanContainChildren: Boolean

### `MenuLink` (Namespace: `TFlex.DOCs.Model.Configuration.Configurations.WebConfigurationStructure`)
**Свойства:** OpenType: LinkOpenType

### `MenuLinkGroup` (Namespace: `TFlex.DOCs.Model.Configuration.Configurations.WebConfigurationStructure`)
**Свойства:** Configuration: WebConfiguration, IsRoot: Boolean, SourceText: String, Text: String, HideNavigationMenu: Boolean, ShowHeader: Boolean, ShowFooter: Boolean, Address: String, Children: LinkGroupCollection, Parent: MenuLinkGroup, Icon: IconImage, HasChildren: Boolean, CanContainChildren: Boolean, Theme: String
**Методы:**
- `String GetFullAddress(String source)`
- `Void AddChild(MenuLinkGroup group)`

### `ReferenceCatalogFolderGroup` (Namespace: `TFlex.DOCs.Model.Configuration.Configurations.WebConfigurationStructure`)
**Свойства:** ChildLinksLoaded: Boolean, Text: String, FolderFullName: String, ReferenceCatalogFolder: ReferenceCatalogFolder

### `ReferenceLink` (Namespace: `TFlex.DOCs.Model.Configuration.Configurations.WebConfigurationStructure`)
**Свойства:** Text: String, ReferenceGuid: Guid, Reference: ReferenceInfo, Filter: Filter, ViewType: ReferenceViewType, CanContainChildren: Boolean, HideReferenceTextAndIcon: Boolean

### `SystemWindowLink` (Namespace: `TFlex.DOCs.Model.Configuration.Configurations.WebConfigurationStructure`)
**Свойства:** Text: String, WindowType: MdiType, CanContainChildren: Boolean

### `TransliteExtensions` (Namespace: `TFlex.DOCs.Model.Configuration.Configurations.WebConfigurationStructure`)
**Методы:**
- `String ToTranslit(Char c) (+1)`

### `WorkingPageLink` (Namespace: `TFlex.DOCs.Model.Configuration.Configurations.WebConfigurationStructure`)
**Свойства:** Text: String, WorkingPageGuid: Guid, WorkingPage: WorkingPage, CanContainChildren: Boolean

### `AnyValueListComboBoxEditData` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings`)
**Свойства:** ParameterPath: String
**Методы:**
- `Boolean Validate(Boolean throwOnError)`

### `ConformityInfo` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings`)
**Свойства:** MachingReferenceParameterGuid: String, DestinationParameterGuid: String

### `FilterEditControlData` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings`)
**Свойства:** ReferenceGuid: Guid, ObjectListPath: String, ParameterPath: String
**Методы:**
- `Boolean Validate(Boolean throwOnError)`

### `FloatDefaultRepositoryItemXMLData` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings`)
**Свойства:** MaxValue: Nullable`1, MinValue: Nullable`1, UseMinValue: Boolean, UseMaxValue: Boolean

### `FormulaRepositoryItemXMLData` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings`)
**Свойства:** FormulaCreatorType: String, ExtensionType: String

### `IntDefaultRepositoryItemXMLData` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings`)
**Свойства:** MaxValue: Nullable`1, MinValue: Nullable`1, UseMinValue: Boolean, UseMaxValue: Boolean

### `MacroEntryPointSelectorData` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings`)
**Свойства:** LinkGuid: Guid

### `OneToManyMultiSelectorControlData` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings`)
**Свойства:** ParameterPath: String, PopupViewId: Guid, ShowContextMenu: Boolean, ShowSelectObjectButton: Boolean

### `ParameterMatchingControlXMLData` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings`)
**Свойства:** MatchingReferenceGuid: String, MatchingParameterGuid: String, ClearLinkAction: Int32, LinkToFillGuid: String, CanUseNotExistValue: Boolean, ShowPopupOnMaching: Boolean, AllowClear: Boolean, AllowSelectFromDialog: Boolean, DisableFilterInDialog: Boolean, DisableFilterWhenLinkFilled: Boolean, Filter: Filter, MaxLoadCount: Int32, UseRelevance: Boolean, ContextFormulaSource: String, UseContainsFilter: Boolean, ViewId: String, ViewName: String, СonformityParameters: List`1 [RU only]
**Методы:**
- `Boolean Validate(Boolean throwOnError)`

### `ProjectDateTimeEditRepositoryItemXMLData` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings`)
**Свойства:** ShiftToWorkTimeAtValueChange: Boolean

### `ReferenceInfoRepositoryItemXMLData` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings`)
**Свойства:** ShowHiddenReferences: Boolean, ShowInnactiveReferences: Boolean, ShowHierarchyGroups: Boolean, ShowObjectInstancesGroups: Boolean

### `RepositoryBaseEditItemXMLData` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings`)
**Свойства:** ReferenceGuid: Guid, Filter: Filter

### `RepositoryBooleanListItemXMLData` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings`)
**Свойства:** Yes: String, No: String

### `RepositoryExtendedHtmlEditXMLData` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings`)
**Свойства:** UseInteractiveLinks: Boolean

### `RepositoryExtendedRtfEditXMLData` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings`)
**Свойства:** UseInteractiveLinks: Boolean

### `RepositoryGuidEditItemXMLData` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings`)
**Свойства:** PathToParameterContainingReferenceId: String, PrototypeMode: Boolean, PathToParameterContainingClassId: String

### `RepositoryItemExpressionEditXMLData` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings`)
**Свойства:** LinkGuid: String, ParameterNameGuid: String, ParameterDescriptionGuid: String

### `RepositoryItemListFromLinkEditXMLData` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings`)
**Свойства:** LinkGuid: String, LinkObjectsGuid: String, ParameterNameGuid: String, ParameterValueGuid: String, IsEditable: Boolean

### `RepositoryItemTimeSpanEditXMLData` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings`)
**Свойства:** ShowDays: Boolean, ShowHours: Boolean, ShowMinutes: Boolean, ShowSeconds: Boolean, AllowNegative: Boolean

### `RepositoryItemUniversalPathEditXMLData` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings`)
**Свойства:** AllowUserChangeInputType: Boolean, InputType: String

### `RepositoryRichTextEditXMLData` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings`)
**Свойства:** LineSpacingInterval: Nullable`1

### `RepositoryRtfEditXMLData` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings`)
**Свойства:** LineSpacingInterval: Nullable`1

### `RepositoryTextWithHintsEditXMLData` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings`)
**Свойства:** HintsMode: Boolean

### `SelectCatalogFolderGuidControlXMLData` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings`)
**Свойства:** ReferenceGuid: String, CatalogGuid: String
**Методы:**
- `Boolean Validate(Boolean throwOnError)`

### `TflexParameterValueGeneratorData` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings`)
**Свойства:** ObjectGuid: Guid

### `TflexRepositoryAnyReferenceObjectLinkEditXMLData` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings`)
**Свойства:** DataSource: Int32, ParameterGuid: String, ReferencesWithFilters: List`1

### `TflexRepositoryItemXMLData` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings`)
**Свойства:** UserControlName: String, ParameterName: String, Parameter: ParameterInfo, ParameterGroup: ParameterGroup
**Методы:**
- `String ConvertToString(ServerConnection connection, TflexRepositoryItemXMLData data)`
- `TflexRepositoryItemXMLData ConvertFromString(ServerConnection connection, String data, String& controlName)`
- `String GetUserControlName(String data)`
- `Boolean Validate(Boolean throwOnError)`
- `ParameterGroup GetParameterGroup()`
- `Guid GetGuid(String guid)`

### `TflexRepositoryLinkEditComboBoxXMLData` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings`)
**Свойства:** ReferenceGuid: String, ReferenceFilterGuid: String, ParameterGuid: String, Editable: Boolean, SavePath: Boolean, LevelPath: Int32, SeparatorPath: String, IsLinkControl: Boolean, AutoComplete: Boolean, PrototypeMode: Boolean, PopupContentViewName: String, AllowClear: Boolean, AllowSelectFromDialog: Boolean, Parameter: ParameterInfo
**Методы:**
- `Boolean Validate(Boolean throwOnError)`

### `TflexRepositoryMultiTextEditXMLData` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings`)
**Свойства:** FormatParameterGuid: Guid

### `TflexRepositorySelectFromDialogEditData` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings`)
**Свойства:** AsseblyName: String, DialogClassName: String, AllowEditControlValue: Boolean
**Методы:**
- `Boolean Validate(Boolean throwOnError)`

### `TflexRepositorySelectReferenceObjectStageData` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings`)
**Свойства:** ReferenceGuid: Guid, ShowNullStage: Boolean, PathToParameterContainingReferenceId: String, EditValueType: String
**Методы:**
- `Boolean Validate(Boolean throwOnError)`

### `TflexRepositoryUnitValueEditXMLData` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings`)
**Свойства:** DataSource: Int32, ParameterPath: String, StoreInBaseUnit: Boolean, ShowChooseButton: Boolean, ShowClearButton: Boolean

### `TflexSelectReferenceParameterGuidControlData` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings`)
**Свойства:** ReferenceGuid: Guid, PathToParameterContainingReferenceId: String, LinkParameterGuid: Guid, ShowAll: Boolean, ShowParameters: Boolean, ShowOnlyUserParameters: Boolean, ShowLinks: Boolean, ShowOnlyToOneLinks: Boolean, ShowOnlyToManyLinks: Boolean, ShowTablesToMany: Boolean, ParameterTypeId: Int32, ShowSignatures: Boolean
**Методы:**
- `Boolean Validate(Boolean throwOnError)`

### `TflexSelectReferenceTypeGuidControlData` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings`)
**Свойства:** ReferenceGuid: Guid, ObjectListPath: String, LinkParameterGuid: Guid, ParameterPath: String, ForbidSelectAbstractClass: Boolean, ConsiderSpecificClasses: Boolean, SpecificClasses: List`1, UseOnlySpecificClasses: Boolean
**Методы:**
- `ReferencePath GetOldValue(ParameterGroup parameterGroup)`
- `Boolean Validate(Boolean throwOnError)`

### `TimeSpanRepositoryItemXMLData` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings`)
**Свойства:** ShowYears: Boolean, ShowMonths: Boolean, ShowDays: Boolean, ShowHours: Boolean, ShowMinutes: Boolean, ShowSeconds: Boolean

### `WeekEditRepositoryItemXMLData` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings`)
**Свойства:** UseStartOfWeek: Boolean

### `AgregationParameterDataSettings` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings.Diagram`)
**Свойства:** SourceParameter: SeriesParameter, OutputParameter: SeriesParameter, SummaryAggregationFunctionType: SummaryAggregationFunctionType, SummaryAggregationFormula: String

### `DiagramSettings` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings.Diagram`)
**Свойства:** DiagramType: DiagramType, Series: ObservableCollection`1, Panes: ObservableCollection`1, Titles: ObservableCollection`1, IsRotated: Boolean, LoadDataOnlyWithCurrentObject: Boolean, VariablesDataString: String, ElementsFormula: String, SeriesFormula: String, SeriesPointFormula: String, ThemeName: String, PaletteType: PaletteType, IsXNavigationEnabled: Boolean, IsYNavigationEnabled: Boolean, IsHorizontalScrollBarVisible: Boolean, IsVerticalScrollBarVisible: Boolean, IsAxisXSynchronize: Boolean, Legends: ObservableCollection`1, IsLegendVisible: Boolean, LegendHorizontalPosition: HorizontalPositionType, LegendVerticalPosition: VerticalPositionType, LegendOrientation: Orientation

### `LegendSettings` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings.Diagram`)
**Свойства:** IsLegendVisible: Boolean, Name: String, HorizontalPosition: HorizontalPositionType, VerticalPosition: VerticalPositionType, Orientation: LegendOrientation, LegendMarkerMode: LegendMarkerMode

### `PaneSettings` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings.Diagram`)
**Свойства:** Name: String, Id: Guid, IsDefault: Boolean, Height: Double, MirrorHeight: Double, BackgroundImagePath: String, IsAxisXVisible: Boolean, AxisXTitleCaption: String, AxisXTitleAlignment: TitleAlignmentType, AxisXFormatString: String, IsAxisXLogarithmic: Boolean, AxisXLogarithmicBase: Double, AxisXDateTimeGridAlignment: DateTimeMeasurementUnitType, AxisXDateTimeMeasureUnit: DateTimeMeasurementUnitType, IsAxisYVisible: Boolean, AxisYTitleCaption: String, AxisYTitleAlignment: TitleAlignmentType, AxisYFormatString: String, AxisYDateTimeGridAlignment: DateTimeMeasurementUnitType, AxisYDateTimeMeasureUnit: DateTimeMeasurementUnitType, IsAxisYLogarithmic: Boolean, AxisYLogarithmicBase: Double, AxisXGridLinesVisible: Boolean, AxisXGridLinesMinorVisible: Boolean, AxisXInterlaced: Boolean, AxisXMinScrollValue: String, AxisXMaxScrollValue: String, AxisYGridLinesVisible: Boolean, AxisYGridLinesMinorVisible: Boolean, AxisYInterlaced: Boolean, AxisYMinScrollValue: String, AxisYMaxScrollValue: String

### `ParameterDataSettings` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings.Diagram`)
**Свойства:** Type: SeriesParameter, CalculationType: CalculationValueType, UniversalPathString: String, DataType: ParameterDataType, ParameterGuid: Guid, CalculateString: String
**Методы:**
- `Void CopyPropertiesTo(ParameterDataSettings parameter)`

### `SeriesSettings` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings.Diagram`)
**Свойства:** Guid: Guid, Name: String, Reference: Guid, FilterString: String, IsActive: Boolean, SeriesType: String, GroupName: String, LineThickness: Int32, ComputationType: SeriesComputationType, ComputationFormula: String, UniversalPathString: String, Color: Color, ModelType: String, MarkerModelType: Marker2DModelType, MarkerModelSize: Int32, MarkerModelVisibility: Boolean, MarkerModel2Type: Marker2DModelType, MarkerModel2Size: Int32, MarkerModel2Visibility: Boolean, IsLabelVisible: Boolean, IsLabelConnectorVisible: Boolean, LabelPointView: LabelViewMode, LabelViewPatternString: String, LabelPosition: LabelPositionMode, LabelKind: LabelKindMode, LabelAngle: Double, PaneId: Guid, ShowInLegend: Boolean, LegendName: String, LegendTextPattern: String, UseSummaryGrouping: Boolean, SummaryGroupingStep: Double, SummaryGroupingOffset: Double, AlignSummaryGrouping: Boolean, WindowSummaryGroupingByCount: Boolean, IsSummaryAgregationFormulaReturnPoints: Boolean, AgregationParameters: ObservableCollection`1, SummaryGroupingMode: SummaryAggregationGroupingMode, SummaryGroupingAlignMode: SummaryAggregationGroupingAlignMode, SummaryGroupingFormula: String, IntegrationEnabled: Boolean, PointIntegrationType: PointIntegrationType, Parameters: ObservableCollection`1, HoleRadiusPercent: Double, NestedDonutWeight: Double, NestedDonutInnerIndent: Double, TotalLabelPattern: String, ShowTitle: Boolean

### `TitleSettings` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings.Diagram`)
**Свойства:** Name: String, Dock: TitleDock, Alignment: TitleAlignmentType, TitleLinkType: TitleLinkType, Group: String

### `BoundsInterval` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings.Diagram.Types.Formulas`)
**Свойства:** Start: PointParameterValueEx, End: PointParameterValueEx, Type: ParameterDataType

### `Point` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings.Diagram.Types.Formulas`)
**Свойства:** Id: Guid, AdditionalData: Object, SourceReferenceObject: ReferenceObject, SourcePoints: ReadOnlyCollection`1, Argument: PointParameterValueEx, ArgumentType: ParameterDataType, StringArgument: String, DateTimeArgument: DateTime, NumericalArgument: Double, DateTimeValue: DateTime, NumericalValue: Double, DateTimeValue2: DateTime, NumericalValue2: Double, Value: PointParameterValue, Value2: PointParameterValue, Weight: Double, CloseValue: Double, HighValue: Double, LowValue: Double, OpenValue: Double, GroupBounds: BoundsInterval, ArgumentBounds: BoundsInterval, Group: PointParameterValueEx

### `PointGroup` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings.Diagram.Types.Formulas`)
**Свойства:** Points: List`1, Bounds: BoundsInterval, AdditionalData: Object

### `PointParameterValue` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings.Diagram.Types.Formulas`)
**Свойства:** ValueType: ParameterDataType, DateTimeValue: DateTime, NumericalValue: Double
**Методы:**
- `Int32 CompareTo(PointParameterValue other) (+5)`

### `PointParameterValueEx` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings.Diagram.Types.Formulas`)
**Свойства:** ValueType: ParameterDataType, DateTimeValue: DateTime, NumericalValue: Double, StringValue: String
**Методы:**
- `Int32 CompareTo(PointParameterValueEx other) (+5)`

### `PointsGroup` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings.Diagram.Types.Formulas`)
**Свойства:** Id: Guid, Points: List`1, GroupBounds: BoundsInterval, ArgumentBounds: BoundsInterval, AdditionalData: Object

### `PointViewParameters` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings.Diagram.Types.Formulas`)
**Свойства:** Id: Guid, LabelText: String, Color: Nullable`1

### `SeriesViewParameters` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings.Diagram.Types.Formulas`)
**Свойства:** Id: Guid, LegendText: String, Color: Nullable`1

### `ObjectSearchAreaSettings` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings.ObjectSearch.FormulaTypes`)
**Свойства:** Name: String, Paths: ReferencePath[], Filters: Filter[]

### `ObjectSearchSettings` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings.ObjectSearch.FormulaTypes`)
**Свойства:** SearchAreas: ObjectSearchAreaSettings[]

### `PathColumnInfo` (Namespace: `TFlex.DOCs.Model.Configuration.ControlSettings.ObjectSearch.FormulaTypes`)
**Свойства:** SystemColumn: ObjectSearchSystemColumnType, Path: ReferencePath, IsVisibleDefault: Boolean, IsVisibleForce: Nullable`1

### `Dialog` (Namespace: `TFlex.DOCs.Model.Configuration.Dialogs`)
**Свойства:** Id: Int32, Guid: Guid, ParameterGroup: ParameterGroup, Class: ClassObject, IsDockable: Boolean, HideToolbarInSeparateWindow: Boolean, EditObjectListInPropertyPanel: Boolean, Groups: ReadOnlyCollection`1, Changing: Boolean
**Методы:**
- `List`1 GetLinkDialogs()`
- `Void AddLinkDialog(ParameterGroup link)`
- `Boolean ContainsLinkDialog(Guid linkGuid)`
- `Boolean RemoveLinkDialog(Guid linkGuid)`
- `Void SwapGroups(DialogGroup group1, DialogGroup group2)`
- `DialogGroup CreateGroup()`
- `Void BeginChanges()`
- `Boolean ValidateLicense(Boolean throwOnError)`
- `Void CancelChanges()`
- `Void EndChanges()` [has Async]
- `Void Delete()` [has Async]

### `DialogGroup` (Namespace: `TFlex.DOCs.Model.Configuration.Dialogs`)
**Свойства:** Id: Int32, Guid: Guid, Dialog: Dialog, Name: String, Comment: String, Icon: IconImage, GeneratedForGroup: ParameterGroup, Pages: ReadOnlyCollection`1, Changing: Boolean
**Методы:**
- `Void SwapPages(DialogPage page1, DialogPage page2)`
- `Void AddPage(DialogPage page)` [has Async]
- `Boolean RemovePage(DialogPage page)` [has Async]
- `Void BeginChanges()`
- `Void CancelChanges()`
- `Void EndChanges()` [has Async]
- `Void Delete()` [has Async]
- `List`1 GetObjectPages(ReferenceObject referenceObject)`

### `DialogManager` (Namespace: `TFlex.DOCs.Model.Configuration.Dialogs`)
**Свойства:** ParameterGroup: ParameterGroup, Pages: ReadOnlyCollection`1, AllDialogs: List`1
**Методы:**
- `Void Refresh()` [has Async]
- `Dialog GetGroupDialog()`
- `Dialog GetClassDialog(ClassObject classObject)`
- `Dialog CreateGroupDialog()`
- `Dialog CreateClassDialog(ClassObject classObject)`
- `DialogPage CreateGroupPage()`
- `DialogPage CreateClassPage(ClassObject classObject)`
- `Dialog GetObjectDialog(ReferenceObject referenceObject)`
- `Dialog GetObjectWebDialog(ReferenceObject referenceObject)`
- `Dialog GetEmptyWebDialog()`
- `Dialog GetLinkDialog(ComplexHierarchyLink link) (+1)`
- `Boolean IsUsed(DialogPage page)`

### `DialogPage` (Namespace: `TFlex.DOCs.Model.Configuration.Dialogs`)
**Свойства:** HasNativeData: Boolean, Id: Int32, Guid: Guid, ParameterGroup: ParameterGroup, Class: ClassObject, Name: String, Comment: String, Type: DialogPageType, DialogType: DialogType, Icon: IconImage, Data: Byte[], Assembly: String, ClassName: String, UsedInPanel: Boolean, UsedInDialog: Boolean, Hidden: Boolean, UsedInClient: DialogPageClient, GeneratedForGroup: ParameterGroup, AccessType: DialogPageAccessType, ConfigurationUseType: ConfigurationUseType, Users: UserCollection, Configurations: ConfigurationCollection, IsModified: Boolean, Changing: Boolean
**Методы:**
- `Boolean CanUseInCurrentConfiguration()`
- `Boolean CanUseInConfiguration(BaseConfiguration configuration)`
- `Filter GetFilter()`
- `Void SetFilter(Filter filter)`
- `Boolean ValidateLicense(Boolean throwOnError)`
- `Void BeginChanges()`
- `Void CancelChanges()`
- `Void EndChanges()` [has Async]
- `Void Delete()` [has Async]

### `DialogPageAccessTypeExtensions` (Namespace: `TFlex.DOCs.Model.Configuration.Dialogs`)
**Методы:**
- `String GetAccessTypeName(DialogPageAccessType type)`

### `DialogPageClientExtensions` (Namespace: `TFlex.DOCs.Model.Configuration.Dialogs`)
**Методы:**
- `String GetClientName(DialogPageClient client)`

### `DialogPageTypeExtensions` (Namespace: `TFlex.DOCs.Model.Configuration.Dialogs`)
**Методы:**
- `String GetPageName(DialogPageType type)`

### `DialogTypeExtensions` (Namespace: `TFlex.DOCs.Model.Configuration.Dialogs`)
**Методы:**
- `String GetName(DialogType type)`

### `AllowedClassesSettings` (Namespace: `TFlex.DOCs.Model.Configuration.Dialogs.Layout`)
**Свойства:** Mode: AllowedClassesMode, Classes: List`1
**Методы:**
- `Void WriteXElement(XElement element)`
- `Void ReadXElement(XElement element)`

### `Extensions` (Namespace: `TFlex.DOCs.Model.Configuration.Dialogs.Layout`)
**Методы:**
- `String ConvertToString64(Image image)`
- `Image ConvertFromString64(String value)`

### `Font` (Namespace: `TFlex.DOCs.Model.Configuration.Dialogs.Layout`)
**Свойства:** FamilyName: String, Size: Int32, Style: FontStyles, Bold: Boolean, Underline: Boolean, Strikeout: Boolean, Italic: Boolean
**Методы:**
- `Boolean TryParse(String value, Font& font)`

### `LayoutDescriptionParser` (Namespace: `TFlex.DOCs.Model.Configuration.Dialogs.Layout`)
**Методы:**
- `PageDescription GetDescription(String xml, Func`2 itemDescriptionCreator) (+1)`

### `Padding` (Namespace: `TFlex.DOCs.Model.Configuration.Dialogs.Layout`)
**Свойства:** Empty: Padding, All: Int32, Left: Int32, Right: Int32, Top: Int32, Bottom: Int32
**Методы:**
- `Boolean TryParse(String value, Padding& padding, String separator)`

### `WorkspaceManager` (Namespace: `TFlex.DOCs.Model.Configuration.Dialogs.Layout.Workspace`)
**Методы:**
- `Guid Save(ServerConnection connection, Guid id, String workspaceInterface, String data)`
- `String Load(ServerConnection connection, Guid id)`
- `Boolean Delete(ServerConnection connection, Guid id)`

### `IReferenceVariable` (Namespace: `TFlex.DOCs.Model.Configuration.Variables`)
**Свойства:** ReferenceGuid: Guid

### `ReferenceClassArrayVariable` (Namespace: `TFlex.DOCs.Model.Configuration.Variables`)
**Свойства:** Type: Type, IsArray: Boolean, AllowNullValue: Boolean

### `ReferenceClassVariable` (Namespace: `TFlex.DOCs.Model.Configuration.Variables`)
**Свойства:** Type: Type, AllowNullValue: Boolean
**Методы:**
- `ClassObject FindClassObject(Guid referenceGuid, Guid classGuid, ServerConnection connection)`

### `ReferenceObjectArrayVariable` (Namespace: `TFlex.DOCs.Model.Configuration.Variables`)
**Свойства:** Type: Type, IsArray: Boolean

### `ReferenceObjectVariable` (Namespace: `TFlex.DOCs.Model.Configuration.Variables`)
**Свойства:** Type: Type

### `ReferenceStageArrayVariable` (Namespace: `TFlex.DOCs.Model.Configuration.Variables`)
**Свойства:** Type: Type, IsArray: Boolean

### `ReferenceStageVariable` (Namespace: `TFlex.DOCs.Model.Configuration.Variables`)
**Свойства:** Type: Type

### `ReferenceUserArrayVariable` (Namespace: `TFlex.DOCs.Model.Configuration.Variables`)
**Свойства:** ReferenceGuid: Guid, Type: Type

### `ReferenceUserVariable` (Namespace: `TFlex.DOCs.Model.Configuration.Variables`)
**Свойства:** ReferenceGuid: Guid, Type: Type

### `ReferenceVariable` (Namespace: `TFlex.DOCs.Model.Configuration.Variables`)
**Свойства:** AllowNullValue: Boolean, IsNull: Boolean, ReferenceGuid: Guid
**Методы:**
- `Void SetDefaultValue()`

### `ReferenceVariableManager` (Namespace: `TFlex.DOCs.Model.Configuration.Variables`)
**Свойства:** ReferenceVariableTypeCollection: IReadOnlyList`1, SupportedTypes: HashSet`1
**Методы:**
- `Type GetVariableType(Type parameterType, Boolean isArray)`

### `Variable` (Namespace: `TFlex.DOCs.Model.Configuration.Variables`)
**Свойства:** Connection: ServerConnection, Variables: VariableCollection, Name: String, Comment: String, IsArray: Boolean, Type: Type, AllowNullValue: Boolean, IsNull: Boolean, Value: Object
**Методы:**
- `Void SetDefaultValue()`

### `Variable`1` (Namespace: `TFlex.DOCs.Model.Configuration.Variables`)
**Свойства:** Type: Type, IsNull: Boolean, Value: T
**Методы:**
- `Void SetDefaultValue()`

### `VariableArray`1` (Namespace: `TFlex.DOCs.Model.Configuration.Variables`)
**Свойства:** Value: T[], IsArray: Boolean, Type: Type, IsNull: Boolean
**Методы:**
- `T[] GetConvertedArray(Object value)`
- `Void SetDefaultValue()`

### `VariableValueChangedDelegate` (Namespace: `TFlex.DOCs.Model.Configuration.Variables`)
**Методы:**
- `Void Invoke(Variable variable, Object oldValue)`
- `IAsyncResult BeginInvoke(Variable variable, Object oldValue, AsyncCallback callback, Object object)`
- `Void EndInvoke(IAsyncResult result)`

### `WorkingPage` (Namespace: `TFlex.DOCs.Model.Configuration.WorkingPages`)
**Свойства:** Connection: ServerConnection, Id: Int32, Guid: Guid, IsAdded: Boolean, IsModified: Boolean, Name: String, Comment: String, Type: WorkingPageType, AlwaysVisible: Boolean, AccessType: WorkingPageAccessType, AccessUsers: UserCollection, ConfigurationUseType: ConfigurationUseType, Configurations: ConfigurationCollection, Data: Byte[], Icon: IconImage, IsPrivate: Boolean, StartPageUsers: StartPageUserCollection, LastEditor: User, LastEditDate: Nullable`1
**Методы:**
- `Boolean CanUseInCurrentConfiguration()`
- `Boolean CanUseInConfiguration(BaseConfiguration configuration)`
- `Boolean Save()`
- `String GetHyperlink(String serverAddress)` [has Async]
- `Boolean Delete()`
- `Boolean MoveUp()`
- `Boolean MoveDown()`
- `Boolean Move(Int32 range)`

### `WorkingPageManager` (Namespace: `TFlex.DOCs.Model.Configuration.WorkingPages`)
**Методы:**
- `List`1 GetPages(ServerConnection connection) (+1)`
- `List`1 GetWindowsPages(ServerConnection connection)` [has Async]
- `List`1 GetWebPages(ServerConnection connection)`
- `WorkingPage GetDefaultStartPage(ServerConnection connection) (+1)` [has Async]
- `WorkingPage Find(ServerConnection connection, Int32 pageId) (+3)`
- `Boolean Export(ServerConnection connection, Stream stream, IEnumerable`1 pages) (+2)`
- `List`1 Import(ServerConnection connection, Stream stream) (+1)`

### `IconsLoader` (Namespace: `TFlex.DOCs.Model.Connection`)
**Методы:**
- `Dictionary`2 LoadParameterGroupIcons(ServerConnection connection, ICollection`1 objectIds) (+1)` [has Async]
- `Dictionary`2 LoadClassObjectIcons(ServerConnection connection, ICollection`1 objectIds) (+1)` [has Async]
- `Dictionary`2 LoadCatalogFolderIcons(ServerConnection connection, ICollection`1 objectIds) (+1)` [has Async]
- `Dictionary`2 LoadDialogGroupIcons(ServerConnection connection, ICollection`1 objectIds) (+1)` [has Async]
- `Dictionary`2 LoadDialogPageIcons(ServerConnection connection, ICollection`1 objectIds) (+1)` [has Async]
- `Dictionary`2 LoadWorkingPageIcons(ServerConnection connection, ICollection`1 objectIds) (+1)` [has Async]

### `DataExchangeAccess` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** AccessId: Int32, UserId: Int32, ObjectId: Int32

### `DataExchangeAccessFilter` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** UserId: Int32, FilterXml: String

### `DataExchangeAccessGroup` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** TypeId: Int32, SystemType: SystemObjectType, AllowedCommands: ReadOnlyCollection`1, ForbiddenCommands: ReadOnlyCollection`1, References: ReadOnlyCollection`1, Objects: ReadOnlyCollection`1, Stages: ReadOnlyCollection`1, Links: ReadOnlyCollection`1

### `DataExchangeAliasedParameterInfo` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** Alias: DataExchangeExtendedParameterReferenceLink, MasterAttributeId: Int32, MasterAttributeGuid: Guid, ClassId: Int32

### `DataExchangeApplication` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** Description: String, EventServiceApplication: Boolean, FileObjectId: Guid, Configurations: ReadOnlyCollection`1, ConfigurationUseType: ConfigurationUseType

### `DataExchangeCatalog` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** Folders: ReadOnlyCollection`1, Users: ReadOnlyCollection`1, UsersUseType: ItemListUseType

### `DataExchangeCatalogFolder` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** ParentId: Int32, Type: String, Icon: Byte[], CustomIcon: Boolean, Properties: String, Objects: ReadOnlyCollection`1, Filter: String, ParameterSearchFolder: String

### `DataExchangeClasses` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** Classes: ReadOnlyCollection`1, GroupSettings: ReadOnlyCollection`1, ExternalParameterGroups: ReadOnlyDictionary`2, ExternalParameters: ReadOnlyDictionary`2, ExternalClasses: ReadOnlyDictionary`2, ClassesKeys: ReadOnlyDictionary`2, SlaveGroupsClasses: ReadOnlyCollection`1
**Методы:**
- `Guid GetClass(Int32 classId) (+1)`

### `DataExchangeClassGroupSettings` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** Guid: Guid, ClassId: Int32, GroupId: Int32, Data: String

### `DataExchangeClassObject` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** Description: String, BaseClassId: Int32, Icon: Byte[], GroupId: Int32, SystemType: SystemObjectType, PropertiesDisplayType: PropertiesDisplayType, UniqueIndexId: Int32, SchemeId: Int32, DefaultStageId: Int32, LinkedClassId: Int32, CanChange: String, ParameterGroups: ReadOnlyCollection`1, ChildObjectClasses: ReadOnlyCollection`1, MasterObjectClasses: ReadOnlyCollection`1, SigningParametersData: String, ObjectFormat: String, ShowChangeCommandInObjectProperties: Nullable`1, SwappedToSelfParameterGroups: ReadOnlyCollection`1, CanCreateInRoot: Boolean, UseBaseClassIcon: Boolean, Sealed: Boolean, Abstract: Boolean, CreateFromPrototype: Boolean, SupportsSaveAndCreate: Boolean, Attributes: String, SupportMultiAttachment: Boolean, InheritMasterObjectClasses: Boolean, IsStandaloneProductByDefault: Boolean, Hidden: Boolean

### `DataExchangeDesktopObject` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** Parameters: ReadOnlyCollection`1, Links: ReadOnlyDictionary`2, AnyReferenceLinks: ReadOnlyDictionary`2, HasLinks: Boolean

### `DataExchangeDialog` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** Comment: String, GroupId: Int32, ClassId: Int32, Dockable: Boolean, EditObjectListInPropertyPanel: Boolean, Groups: ReadOnlyCollection`1, LinkDialogs: ReadOnlyCollection`1, IsWeb: Boolean

### `DataExchangeDialogData` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** Dialogs: ReadOnlyCollection`1, Pages: ReadOnlyCollection`1

### `DataExchangeDialogGroup` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** Comment: String, Icon: Byte[], DialogId: Int32, DialogPages: ReadOnlyCollection`1, GeneratedForGroupId: Int32

### `DataExchangeDialogPage` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** Comment: String, GroupId: Int32, ClassId: Int32, Type: DialogPageType, Icon: Byte[], Assembly: String, ClassName: String, UsedInPanel: Boolean, UsedInDialog: Boolean, Hidden: Boolean, UsedInClient: DialogPageClient, GeneratedForGroupId: Int32, DialogType: String, IsWeb: Boolean, Users: ReadOnlyCollection`1, AccessType: DialogPageAccessType, Configurations: ReadOnlyCollection`1, ConfigurationUseType: ConfigurationUseType, FilterData: String, Data: Byte[], XmlData: String

### `DataExchangeExtendedParameterInfo` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** ExtendedOptions: ExtendedParameterInfoOptions, Aliases: ReadOnlyCollection`1, ValuesStorageId: Int32

### `DataExchangeExtendedParameterReferenceLink` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** ParameterId: Int32, ParameterGuid: Guid, GroupId: Int32, GroupGuid: Guid, ClassId: Int32, ClassGuid: Guid, Alias: String, Comment: String

### `DataExchangeExternalLinkedObject` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** Id: Int32, Guid: Guid, Path: String

### `DataExchangeExternalLinkedObjects` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** Objects: ReadOnlyCollection`1

### `DataExchangeExternalObjects` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** ExternalLinkedGroups: ReadOnlyDictionary`2, ExternalLinkedParameters: ReadOnlyDictionary`2, ExternalLinkedClasses: ReadOnlyDictionary`2

### `DataExchangeFile` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** Product: String, ProductVersion: Version, SchemaVersion: Int32, Mode: ExportMode, StructureIdentify: Boolean, Schemes: ReadOnlyCollection`1, Stages: ReadOnlyCollection`1, StageAccesses: ReadOnlyCollection`1, References: ReadOnlyCollection`1, Classes: ReadOnlyCollection`1, Objects: ReadOnlyCollection`1, ExternalLinkedObjects: ReadOnlyCollection`1, ReferenceDialogs: ReadOnlyCollection`1, ReferenceSettings: ReadOnlyCollection`1, ReferenceCatalogs: ReadOnlyCollection`1, AccessGroups: ReadOnlyDictionary`2, UserAccessGroups: ReadOnlyCollection`1, UserGroups: ReadOnlyDictionary`2, ReferencesAccesses: ReadOnlyCollection`1, ReferenceGroupSettings: ReadOnlyCollection`1, Applications: ReadOnlyCollection`1
**Методы:**
- `Guid GetParameterGroup(Int32 parameterGroupId)`
- `Stream GetFileStream(DataExchangeObject dataExchangeObject)`

### `DataExchangeGateway` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Методы:**
- `Void Export(ExportReferenceSettings referenceSettings, ExportObjectsSettings objectSettings, ExportOptions options, IReadOnlyList`1 additionalSettings) (+13)` [has Async]
- `Void Import(ServerConnection connection, ImportReferenceSettings settings, ImportOptions options) (+3)` [has Async]
- `DataExchangeFile Read(Stream stream, CancellationToken token)`
- `Stream ReadData(Stream stream, CancellationToken token)`

### `DataExchangeHierarchyLink` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** ParentId: Int32, ChildId: Int32, StructureTypes: ReadOnlyCollection`1

### `DataExchangeIndex` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** GroupId: Int32, UniqueKey: Boolean, Parameters: ReadOnlyCollection`1

### `DataExchangeIndexParameter` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** Id: Int32, CheckDefaultValue: Boolean

### `DataExchangeInstancesGroupInfo` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** MasterReferenceId: Int32, InstancesReferenceId: Int32, LinkToObjectId: Int32, LinkToHierarchyId: Int32, InstanceMainClassId: Int32

### `DataExchangeKeyObject` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** Id: Int32, Guid: Guid

### `DataExchangeNameObject` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** Name: String

### `DataExchangeNomenclatureClassObject` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** ReferenceId: Int32, ReferenceClassId: Int32, LinkedInheritClasses: Boolean, Rules: List`1

### `DataExchangeNomenclatureRule` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** ReferenceParameterId: Int32, NomenclatureParameterId: Int32

### `DataExchangeObject` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** GroupId: Int32, LinkedObjects: ReadOnlyDictionary`2, Signatures: ReadOnlyCollection`1, IsPrototype: Boolean, SystemType: SystemObjectType, ApplicabilityConditions: String, InstanceObjectHierarchyLinksPath: ReadOnlyCollection`1

### `DataExchangeObjectExtensions` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Методы:**
- `IconImage GetIconImage(IDataExchangeObjectWithIcon dataExchangeObject)`

### `DataExchangeObjects` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** Objects: ReadOnlyCollection`1, HierarchyLinks: ReadOnlyCollection`1
**Методы:**
- `DataExchangeObject GetObject(Int32 objectId)`

### `DataExchangeParameter` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** Id: Int32, Type: Type
**Методы:**
- `Object GetValue()`

### `DataExchangeParameter`1` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** Type: Type, Value: T
**Методы:**
- `Object GetValue()`

### `DataExchangeParameterGroup` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** Parameters: ReadOnlyCollection`1, Indexes: ReadOnlyCollection`1, TableName: String, LogData: Boolean, SupportsRevisions: Boolean, SupportsExtendedParameters: Boolean, Caption: String, MasterGroupId: Int32, SlaveGroupId: Int32, GroupType: ParameterGroupType, Visibility: ParameterGroupVisibility, Icon: Byte[], Description: String, DefaultParameterId: Int32, HierarchyType: ReferenceHierarchyType, SupportsClasses: Boolean, SupportsDesktop: Boolean, SupportsRecycleBin: Boolean, SupportsSystemObjects: Boolean, CheckAccess: Int32, SupportsEncryption: Boolean, SupportsPrototype: Boolean, LinkType: LinkType, LinkVisibility: LinkVisibility, SupportsOrder: Boolean, SupportsMandatoryAccess: Boolean, SupportsOwner: Boolean, SystemType: SystemObjectType, SupportsSignature: Boolean, UseAllSignatureTypes: Boolean, SignatureTypes: ReadOnlyCollection`1, SigningParametersData: String, PrivateFolderPrototypeId: Int32, SupportsStages: Boolean, UniqueIndexId: Int32, SchemeId: Int32, DefaultStageId: Int32, EventHandlers: ReadOnlyCollection`1, UserEvents: ReadOnlyCollection`1, DoubleDirectionLink: Boolean, UserControl: String, AuthorAccessGroup: String, SelectionPath: String, CanChangeClass: Boolean, SupportsConfigurationSettings: Boolean, SupportsDesignContexts: Boolean, SupportsActivityDates: Boolean, SupportsApplicability: Boolean, SupportsSubstitutesInContext: Boolean, SearchQueryLinkFilter: String, SearchQueryLinkPathToFilter: String, SupportsObjectsInstances: Boolean, ObjectsInstances: DataExchangeInstancesGroupInfo, IsObjectsInstancesImpl: Boolean, Configurator: Guid, TableNameGenerated: Boolean, DefaultAccessLevel: Int32, ObjectFormat: String, RevisionNamingRule: String, LinkRequired: LinkRequired, IsAsymmetricLink: Boolean

### `DataExchangeParameterGroupEvent` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** GroupId: Int32, ButtonData: String

### `DataExchangeParameterInfo` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** GroupId: Int32, FieldName: String, ParameterType: ParameterType, Caption: String, Comment: String, IsRequired: Boolean, DefaultValue: String, IsVisible: Boolean, Nullable: Boolean, EditType: ParameterEditType, Length: Int32, Format: String, IsIndexed: Boolean, IsFullTextSearchEnabled: Boolean, ActivityStatus: ParameterActivityStatus, UnitId: Int32, UnitGuid: Guid, ListType: String, ListValues: ReadOnlyCollection`1, NomenclatureParameterID: Int32, SystemType: SystemObjectType, CalculationType: String, UserControl: String, RangeInfo: String, State: String, CertificateGuid: Guid, NormalizedFieldName: String

### `DataExchangeParameterListValue` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** Guid: Guid, Name: String, Value: String, Icon: Byte[]

### `DataExchangeReference` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** CatalogFolders: ReadOnlyCollection`1, ParameterGroups: ReadOnlyCollection`1, ParameterGroupKeys: ReadOnlyDictionary`2, ParameterKeys: ReadOnlyDictionary`2, SignatureTypeKeys: ReadOnlyDictionary`2, StageKeys: ReadOnlyDictionary`2
**Методы:**
- `Guid GetParameter(Int32 parameterId) (+1)`
- `Guid GetParameterGroup(Int32 parameterGroupId) (+1)`

### `DataExchangeReferenceAccesses` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** Accesses: ReadOnlyCollection`1, AccessFilters: ReadOnlyCollection`1

### `DataExchangeReferenceCatalogs` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** Catalogs: ReadOnlyCollection`1

### `DataExchangeReferenceDialogs` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** DialogData: ReadOnlyCollection`1

### `DataExchangeReferenceGroupSettings` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** GroupSettings: ReadOnlyCollection`1

### `DataExchangeReferenceHeader` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** File: DataExchangeFile, Guid: Guid, Caption: String, Description: String

### `DataExchangeReferenceSettings` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** Settings: ReadOnlyCollection`1

### `DataExchangeSchemeStages` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** Comment: String, StageIds: ReadOnlyCollection`1, Transitions: ReadOnlyCollection`1

### `DataExchangeSchemeTransition` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** FromStageId: Int32, ToStageId: Int32, Automatic: Boolean, Manual: Boolean

### `DataExchangeSettings` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** Guid: Guid, Name: String, IsView: Boolean, GroupId: Int32, ObjectId: Int32, Application: String, Interface: String, Context: String, Configurations: ReadOnlyCollection`1, ConfigurationUseType: ConfigurationUseType, DefaultConfigurations: ReadOnlyCollection`1, FolderGuid: Guid, Data: String, AccessType: SettingsViewAccessType, Users: ReadOnlyCollection`1

### `DataExchangeSignature` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** Key: Int32, TypeId: Int32, Date: DateTime, UserId: Int32, State: SignatureState, Resolution: String, Actual: Boolean, DigitalSignature: Byte[], SignedParameters: String, OnBehalfOf: Int32, CredentialId: Int32

### `DataExchangeSignatureType` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** Description: String

### `DataExchangeStage` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** Comment: String

### `DataExchangeStageAccessGroup` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** StageId: Int32, AccessGroupGuid: Guid, UserGuid: Guid

### `ExportCallback` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Методы:**
- `Boolean Invoke(String[] messages, Int32 counter, Int32 totalCount)`
- `IAsyncResult BeginInvoke(String[] messages, Int32 counter, Int32 totalCount, AsyncCallback callback, Object object)`
- `Boolean EndInvoke(IAsyncResult result)`

### `ExportDialogsModeExtensions` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Методы:**
- `String GetName(ExportDialogsMode mode)`

### `ExportErrorObjectsCallback` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Методы:**
- `Boolean Invoke(List`1 descriptions)`
- `IAsyncResult BeginInvoke(List`1 descriptions, AsyncCallback callback, Object object)`
- `Boolean EndInvoke(IAsyncResult result)`

### `ExportLinkedObjectsModeExtensions` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Методы:**
- `String GetName(ExportLinkedObjectsMode mode)`

### `ExportObjectsModeExtensions` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Методы:**
- `String GetName(ExportObjectsMode mode)`

### `ExportOptions` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** ExportMode: ExportMode, EncryptionMode: ExportEncryptionMode, ObjectsMode: ExportObjectsMode, DialogsMode: ExportDialogsMode, LinkedObjectsMode: ExportLinkedObjectsMode, IncludeStructure: Boolean, IncludeSigningParameters: Boolean, IncludePrototypes: Boolean, IncludeViews: Boolean, IncludeDialogs: Boolean, IncludeMacros: Boolean, IncludeAccesses: Boolean, IncludeRevisionNamingRules: Boolean, IncludeConfigurators: Boolean, IncludeProductsApplicability: Boolean, IncludeObjectStages: Boolean, IncludeCoordinates: Boolean, IncludeInstances: Boolean, ExportSpecificInstances: Boolean, FileName: String, DataFormat: TextFormats, Indent: Boolean, UsePackage: Boolean, BinaryFormat: Boolean, PlainFormat: Boolean, PlainCompression: CompressionAlgorithm, DataStream: Stream, FileIds: List`1, ApplicationIds: List`1, Callback: ExportCallback, ErrorObjectsCallback: ExportErrorObjectsCallback, SynchronizeInvoke: ISynchronizeInvoke

### `IDataExchangeObjectWithIcon` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** Icon: Byte[]

### `ImportCallback` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Методы:**
- `Boolean Invoke(String[] messages, Int64 counter, Int64 totalCount)`
- `IAsyncResult BeginInvoke(String[] messages, Int64 counter, Int64 totalCount, AsyncCallback callback, Object object)`
- `Boolean EndInvoke(IAsyncResult result)`

### `ImportErrorObjectsCallback` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Методы:**
- `Void Invoke(List`1 errors)`
- `IAsyncResult BeginInvoke(List`1 errors, AsyncCallback callback, Object object)`
- `Void EndInvoke(IAsyncResult result)`

### `ImportModeExtensions` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Методы:**
- `String GetName(ImportMode mode)`

### `ImportObjectsAsyncCallback` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Методы:**
- `ValueTask Invoke(Dictionary`2 importedObjects, CancellationToken token)`
- `IAsyncResult BeginInvoke(Dictionary`2 importedObjects, CancellationToken token, AsyncCallback callback, Object object)`
- `ValueTask EndInvoke(IAsyncResult result)`

### `ImportObjectsCallback` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Методы:**
- `Void Invoke(Dictionary`2 importedObjects)`
- `IAsyncResult BeginInvoke(Dictionary`2 importedObjects, AsyncCallback callback, Object object)`
- `Void EndInvoke(IAsyncResult result)`

### `ImportOptions` (Namespace: `TFlex.DOCs.Model.DataExchange`)
**Свойства:** FileName: String, DataStream: Stream, SearchFile: Func`2, Mode: ImportMode, IncludeStructure: Boolean, IncludeObjects: Boolean, IncludeViews: Boolean, IncludeDialogs: Boolean, IncludeAccesses: Boolean, IncludeApplications: Boolean, IncludeSigningParameters: Boolean, DisableNesting: Boolean, SynchronizeObjects: Boolean, Callback: ImportCallback, ObjectsCallback: ImportObjectsCallback, ObjectsAsyncCallback: ImportObjectsAsyncCallback, ErrorObjectsCallback: ImportErrorObjectsCallback, SynchronizeInvoke: ISynchronizeInvoke

### `DialogPageExchangeService` (Namespace: `TFlex.DOCs.Model.DataExchange.Dialogs`)
**Методы:**
- `Boolean Export(ParameterGroup reference, ClassObject classObject, Dictionary`2 dialogPages, String fileName)` [has Async]
- `List`1 Import(String fileName, DialogGroup dialogGroup, ParameterGroup parameterGroup, ClassObject classObject)` [has Async]

### `ExportSettingsExtensions` (Namespace: `TFlex.DOCs.Model.DataExchange.Extensions`)
**Методы:**
- `String GetVisualization(ExportObjectsSettings settings)`

### `ExportCatalogSettings` (Namespace: `TFlex.DOCs.Model.DataExchange.Settings`)
**Свойства:** Catalog: Catalog, IncludeAllCatalogFolders: Boolean, CatalogFolders: IReadOnlyCollection`1
**Методы:**
- `Boolean AddCatalogFolder(Int32 id) (+2)`
- `Void AddCatalogFolders(Int32[] ids) (+5)`
- `Boolean ContainsCatalogFolder(Int32 id) (+2)`
- `Boolean RemoveCatalogFolder(Int32 id) (+2)`

### `ExportClassSettings` (Namespace: `TFlex.DOCs.Model.DataExchange.Settings`)
**Свойства:** ClassObject: ClassObject, IncludeAllEventHandlers: Boolean, EventHandlers: IReadOnlyCollection`1, HasEventHandlers: Boolean, IncludeAllProperties: Boolean, Properties: IReadOnlyCollection`1, RevisionNamingRule: ExportRevisionNamingRuleSettings, HasRevisionNamingRule: Boolean, Scheme: ExportSchemeSettings, HasScheme: Boolean
**Методы:**
- `Boolean AddEventHandler(Int32 id)`
- `Void AddEventHandlers(Int32[] ids) (+1)`
- `Boolean Add(ParameterGroupEventHandler eventHandler) (+2)`
- `Boolean ContainsEventHandler(Int32 id)`
- `Boolean Contains(ParameterGroupEventHandler eventHandler)`
- `Boolean RemoveEventHandler(Int32 id)`
- `Boolean Remove(ParameterGroupEventHandler eventHandler)`
- `Boolean AddProperty(ClassProperties property)`
- `Void AddProperties(ClassProperties[] properties) (+1)`
- `Boolean ContainsProperty(ClassProperties property)`
- `Boolean RemoveProperty(ClassProperties property)`
- `ExportRevisionNamingRuleSettings AddRevisionNamingRule()`
- `Void RemoveRevisionNamingRule()`
- `ExportSchemeSettings AddScheme()`
- `Void RemoveScheme()`

### `ExportDialogGroupSettings` (Namespace: `TFlex.DOCs.Model.DataExchange.Settings`)
**Свойства:** DialogGroup: DialogGroup, IncludeAllDialogPages: Boolean, DialogPages: IReadOnlyCollection`1, HasDialogPages: Boolean
**Методы:**
- `Boolean AddDialogPage(Int32 id) (+1)`
- `Void AddDialogPages(Int32[] ids) (+3)`
- `Boolean Add(DialogPage dialogPage) (+2)`
- `Boolean ContainsDialogPage(Int32 id) (+1)`
- `Boolean Contains(DialogPage dialogPage)`
- `Boolean RemoveDialogPage(Int32 id) (+1)`
- `Boolean Remove(DialogPage dialogPage)`

### `ExportDialogsModeExtensions` (Namespace: `TFlex.DOCs.Model.DataExchange.Settings`)
**Методы:**
- `String GetName(ExportSettingsLoadDirection loadDirection)`

### `ExportLinkSettings` (Namespace: `TFlex.DOCs.Model.DataExchange.Settings`)
**Свойства:** LinkGroup: ParameterGroup, IncludeAllProperties: Boolean, Properties: IReadOnlyCollection`1
**Методы:**
- `Boolean AddProperty(ParameterGroupProperties property)`
- `Void AddProperties(ParameterGroupProperties[] properties) (+1)`
- `Boolean ContainsProperty(ParameterGroupProperties property)`
- `Boolean RemoveProperty(ParameterGroupProperties property)`

### `ExportObjectsSettings` (Namespace: `TFlex.DOCs.Model.DataExchange.Settings`)
**Свойства:** ObjectIds: IReadOnlyCollection`1, HierarchyLinkIds: IReadOnlyCollection`1
**Методы:**
- `Boolean ContainsObjectId(Int32 objectId)`
- `Void AddObject(Int32 objectId, Int32 instanceId) (+2)`
- `Void AddObjects(IEnumerable`1 objectIds, Dictionary`2 instanceIds) (+2)`
- `ExportObjectsSettings Create(ReferenceInfo referenceInfo) (+1)` [has Async]
- `Boolean ContainsHierarchyLinkId(Int32 hierarchyLinkId)`
- `Void AddHierarchyLink(ComplexHierarchyLink hierarchyLink) (+1)`
- `Void AddHierarchyLinks(IEnumerable`1 hierarchyLinks) (+1)`

### `ExportParameterGroupSettings` (Namespace: `TFlex.DOCs.Model.DataExchange.Settings`)
**Свойства:** ParameterGroup: ParameterGroup, IncludeAllProperties: Boolean, Properties: IReadOnlyCollection`1
**Методы:**
- `Boolean AddProperty(ParameterGroupProperties property)`
- `Void AddProperties(ParameterGroupProperties[] properties) (+1)`
- `Boolean ContainsProperty(ParameterGroupProperties property)`
- `Boolean RemoveProperty(ParameterGroupProperties property)`

### `ExportParameterSettings` (Namespace: `TFlex.DOCs.Model.DataExchange.Settings`)
**Свойства:** Parameter: ParameterInfo, IncludeAllProperties: Boolean, Properties: IReadOnlyCollection`1
**Методы:**
- `Boolean AddProperty(ParameterProperties property)`
- `Void AddProperties(ParameterProperties[] properties) (+1)`
- `Boolean ContainsProperty(ParameterProperties property)`
- `Boolean RemoveProperty(ParameterProperties property)`

### `ExportReferencePathExtensions` (Namespace: `TFlex.DOCs.Model.DataExchange.Settings`)
**Методы:**
- `ExportSettings AddToExportSettings(ReferencePath path, ExportSettings settings)`

### `ExportReferenceSettings` (Namespace: `TFlex.DOCs.Model.DataExchange.Settings`)
**Свойства:** IncludeAllAccesses: Boolean, Accesses: IReadOnlyCollection`1, HasAccesses: Boolean, IncludeAllCatalogs: Boolean, Catalogs: IReadOnlyCollection`1, HasCatalogs: Boolean, IncludeAllClasses: Boolean, Classes: IReadOnlyCollection`1, HasClasses: Boolean, MasterGroup: ParameterGroup, IncludeAllDialogGroups: Boolean, DialogGroups: IReadOnlyCollection`1, HasDialogGroups: Boolean, IncludeAllEventHandlers: Boolean, EventHandlers: IReadOnlyCollection`1, HasEventHandlers: Boolean, IncludeAllEvents: Boolean, Events: IReadOnlyCollection`1, HasEvents: Boolean, IncludeAllLinks: Boolean, Links: IReadOnlyCollection`1, HasLinks: Boolean, IncludeAllObjectLists: Boolean, ObjectLists: IReadOnlyCollection`1, HasObjectLists: Boolean, IncludeAllParameterGroups: Boolean, ParameterGroups: IReadOnlyCollection`1, HasParameterGroups: Boolean, IncludeAllParameters: Boolean, Parameters: IReadOnlyCollection`1, HasParameters: Boolean, IncludeAllProperties: Boolean, Properties: IReadOnlyCollection`1, IncludeAllPrototypes: Boolean, Prototypes: IReadOnlyCollection`1, HasPrototypes: Boolean, RevisionNamingRule: ExportRevisionNamingRuleSettings, HasRevisionNamingRule: Boolean, Scheme: ExportSchemeSettings, HasScheme: Boolean, IncludeAllSignatureTypes: Boolean, SignatureTypes: IReadOnlyCollection`1, HasSignatureTypes: Boolean, IncludeAllViews: Boolean, Views: IReadOnlyCollection`1, HasViews: Boolean
**Методы:**
- `Boolean ContainsParameter(Guid guid) (+1)`
- `Boolean Contains(ParameterInfo parameter) (+5)`
- `Boolean RemoveParameter(Int32 id) (+1)`
- `Boolean Remove(ParameterInfo parameter) (+5)`
- `Boolean AddProperty(ParameterGroupProperties property)`
- `Void AddProperties(ParameterGroupProperties[] properties) (+1)`
- `Boolean ContainsProperty(ParameterGroupProperties property)`
- `Boolean RemoveProperty(ParameterGroupProperties property)`
- `Boolean AddPrototype(Int32 id) (+2)`
- `Void AddPrototypes(Int32[] ids) (+5)`
- `Boolean ContainsPrototype(Int32 id) (+2)`
- `Boolean RemovePrototype(Int32 id) (+2)`
- `ExportRevisionNamingRuleSettings AddRevisionNamingRule()`
- `Void RemoveRevisionNamingRule()`
- `ExportSchemeSettings AddScheme()`
- `Void RemoveScheme()`
- `Boolean AddSignatureType(Int32 id) (+2)`
- `Void AddSignatureTypes(Int32[] ids) (+5)`
- `Boolean ContainsSignatureType(Int32 id) (+2)`
- `Boolean RemoveSignatureType(Int32 id) (+2)`
- `Boolean AddView(Guid viewGuid) (+1)`
- `Void AddViews(Guid[] viewGuids) (+3)`
- `Boolean ContainsView(Guid viewGuid) (+1)`
- `Boolean RemoveView(Guid viewGuid) (+1)`
- `IReadOnlyCollection`1 GetAllViews()` [has Async]
- `Void AddLinks(Guid[] guids) (+3)`
- `ExportLinkSettings Add(ParameterGroup linkGroup) (+17)`
- `Boolean TryGetLink(Int32 id, ExportLinkSettings& settings) (+1)`
- `Boolean TryGet(ParameterGroup linkGroup, ExportLinkSettings& settings) (+3)`
- `Boolean ContainsLink(Int32 id) (+1)`
- `Boolean RemoveLink(Int32 id) (+1)`
- `ExportReferenceSettings AddObjectList(Int32 id) (+2)`
- `Boolean ContainsObjectList(Int32 id) (+2)`
- `Boolean RemoveObjectList(Int32 id) (+2)`
- `ExportParameterGroupSettings AddParameterGroup(Int32 id) (+2)`
- `Void AddParameterGroups(Int32[] ids) (+5)`
- `Boolean TryGetParameterGroup(Int32 id, ExportParameterGroupSettings& settings) (+1)`
- `Boolean ContainsParameterGroup(Int32 id) (+2)`
- `Boolean RemoveParameterGroup(Int32 id) (+2)`
- `ExportParameterSettings AddParameter(Int32 id) (+1)`
- `Void AddParameters(Int32[] ids) (+3)`
- `Boolean TryGetParameter(Int32 id, ExportParameterSettings& settings) (+1)`
- `Boolean AddAccess(Int32 id) (+2)`
- `Void AddAccesses(Int32[] ids) (+5)`
- `Boolean ContainsAccess(Int32 id) (+2)`
- `Boolean RemoveAccess(Int32 id) (+2)`
- `ExportCatalogSettings AddCatalog(Int32 id) (+2)`
- `Void AddCatalogs(Int32[] ids) (+5)`
- `Boolean ContainsCatalog(Int32 id) (+2)`
- `Boolean RemoveCatalog(Int32 id) (+2)`
- `ExportClassSettings AddClass(Int32 id) (+1)`
- `Void AddClasses(Int32[] ids) (+3)`
- `Boolean TryGetClass(Int32 id, ExportClassSettings& settings) (+1)`
- `Boolean ContainsClass(Int32 id) (+1)`
- `Boolean RemoveClass(Int32 id) (+1)`
- `ExportReferenceSettings Create(ReferenceInfo referenceInfo) (+1)` [has Async]
- `ExportDialogGroupSettings AddDialogGroup(Int32 id) (+1)`
- `Void AddDialogGroups(Int32[] ids) (+3)`
- `Boolean ContainsDialogGroup(Int32 id) (+1)`
- `Boolean RemoveDialogGroup(Int32 id) (+1)`
- `Boolean AddEventHandler(Int32 id)`
- `Void AddEventHandlers(Int32[] ids) (+1)`
- `Boolean ContainsEventHandler(Int32 id)`
- `Boolean RemoveEventHandler(Int32 id)`
- `Boolean AddEvent(Int32 id) (+1)`
- `Void AddEvents(Int32[] ids) (+3)`
- `Boolean ContainsEvent(Int32 id) (+1)`
- `Boolean RemoveEvent(Int32 id) (+1)`
- `ExportLinkSettings AddLink(Int32 id) (+1)`

### `ExportRevisionNamingRuleSettings` (Namespace: `TFlex.DOCs.Model.DataExchange.Settings`)
**Свойства:** RevisionNamingRule: RevisionNamingRuleObject, IncludeAllRevisionLevels: Boolean, RevisionLevels: IReadOnlyCollection`1, HasRevisionLevels: Boolean
**Методы:**
- `Boolean AddRevisionLevel(Int32 id) (+2)`
- `Void AddRevisionLevels(Int32[] ids) (+5)`
- `Boolean ContainsRevisionLevel(Int32 id) (+2)`
- `Boolean RemoveRevisionLevel(Int32 id) (+2)`

### `ExportSchemeSettings` (Namespace: `TFlex.DOCs.Model.DataExchange.Settings`)
**Свойства:** Scheme: Scheme, IncludeAllStages: Boolean, Stages: IReadOnlyCollection`1, HasStages: Boolean, IncludeAllTransitions: Boolean, Transitions: IReadOnlyCollection`1, HasTransitions: Boolean
**Методы:**
- `Boolean AddStage(Int32 id) (+4)`
- `Void AddStages(Int32[] ids) (+3)`
- `Boolean ContainsStage(Int32 id) (+2)`
- `Boolean RemoveStage(Int32 id) (+2)`
- `Boolean AddTransition(ValueTuple`2 transitionGuid) (+1)`
- `Void AddTransitions(ValueTuple`2[] transitionGuids) (+3)`
- `Boolean ContainsTransition(SchemeStageTransition transition)`
- `Boolean RemoveTransition(SchemeStageTransition transition)`

### `ExportSettings` (Namespace: `TFlex.DOCs.Model.DataExchange.Settings`)
**Свойства:** Connection: ServerConnection, MasterGroup: ParameterGroup, IncludeSignatures: Boolean, DontChangeObjects: Boolean, LoadDirection: ExportSettingsLoadDirection, IncludeAllParameters: Boolean, Parameters: ParameterInfoCollection, HasParameters: Boolean, IncludeAllLinks: Boolean, Links: IReadOnlyCollection`1, HasLinks: Boolean
**Методы:**
- `Void AddLoadDirection(ExportSettingsLoadDirection loadDirection)`
- `Void RemoveLoadDirection(ExportSettingsLoadDirection loadDirection)`
- `Boolean AddParameter(Int32 parameterId) (+1)`
- `Void AddParameters(Int32[] parameterIds) (+3)`
- `Boolean Add(ParameterInfo parameter) (+2)`
- `Boolean ContainsParameter(Int32 parameterId) (+1)`
- `Boolean Contains(ParameterInfo parameter)`
- `Boolean RemoveParameter(Int32 parameterId) (+1)`
- `Boolean Remove(ParameterInfo parameter)`
- `LinkExportSettings GetLink(Int32 linkId) (+2)`
- `Boolean ContainsLink(ParameterGroup link) (+2)`
- `LinkExportSettings AddLink(Int32 linkId) (+2)`
- `Void CopyLinks(ExportSettings sourceSettings)`
- `StructureTypeExportSettings GetStructureType()`
- `StructureTypeExportSettings AddStructureType()`
- `ProductsApplicabilityExportSettings GetProductsApplicability()`
- `ProductsApplicabilityExportSettings AddProductsApplicability()`
- `StartProductExportSettings GetStartProduct()`
- `StartProductExportSettings AddStartProduct()`
- `EndProductExportSettings GetEndProduct()`
- `EndProductExportSettings AddEndProduct()`
- `RemarksExportSettings GetRemarks()`
- `RemarksExportSettings AddRemarks()`
- `StructureTypesExportSettings GetStructureTypes()`
- `StructureTypesExportSettings AddStructureTypes()`

### `ExportSettingsManager` (Namespace: `TFlex.DOCs.Model.DataExchange.Settings`)
**Методы:**
- `String Serialize(ExportOptions options, IReadOnlyList`1 referenceSettingsList, IReadOnlyList`1 objectsSettingsList)`
- `ValueTuple`3 Deserialize(ServerConnection connection, String settings)`

### `ImportCatalogSettings` (Namespace: `TFlex.DOCs.Model.DataExchange.Settings`)
**Свойства:** Guid: Guid, CatalogFolders: Guid[]

### `ImportClassSettings` (Namespace: `TFlex.DOCs.Model.DataExchange.Settings`)
**Свойства:** Guid: Guid

### `ImportDialogGroupSettings` (Namespace: `TFlex.DOCs.Model.DataExchange.Settings`)
**Свойства:** Guid: Guid, DialogPages: IReadOnlyCollection`1, HasDialogPages: Boolean
**Методы:**
- `Boolean AddDialogPage(Guid guid)`
- `Void AddDialogPages(Guid[] guids) (+1)`
- `Boolean ContainsDialogPage(Guid guid)`
- `Boolean RemoveDialogPage(Guid guid)`

### `ImportLinkSettings` (Namespace: `TFlex.DOCs.Model.DataExchange.Settings`)
**Свойства:** Guid: Guid, IncludeAllProperties: Boolean, Properties: IReadOnlyCollection`1
**Методы:**
- `Boolean AddProperty(ParameterProperties property)`
- `Void AddProperties(ParameterProperties[] properties) (+1)`
- `Boolean ContainsProperty(ParameterProperties property)`
- `Boolean RemoveProperty(ParameterProperties property)`

### `ImportParameterGroupSettings` (Namespace: `TFlex.DOCs.Model.DataExchange.Settings`)
**Свойства:** Guid: Guid

### `ImportParameterSettings` (Namespace: `TFlex.DOCs.Model.DataExchange.Settings`)
**Свойства:** Guid: Guid

### `ImportReferenceSettings` (Namespace: `TFlex.DOCs.Model.DataExchange.Settings`)
**Свойства:** IncludeAllAccesses: Boolean, Accesses: IReadOnlyCollection`1, HasAccesses: Boolean, Catalogs: IReadOnlyCollection`1, HasCatalogs: Boolean, IncludeAllClasses: Boolean, Classes: IReadOnlyCollection`1, HasClasses: Boolean, MasterGroup: DataExchangeParameterGroup, MasterGroupGuid: Guid, DialogGroups: IReadOnlyCollection`1, HasDialogGroups: Boolean, IncludeAllEventHandlers: Boolean, HandlerEvents: IReadOnlyCollection`1, HasEventHandlers: Boolean, IncludeAllEvents: Boolean, Events: IReadOnlyCollection`1, HasEvents: Boolean, IncludeAllLinks: Boolean, Links: IReadOnlyCollection`1, HasLinks: Boolean, IncludeAllObjectLists: Boolean, ObjectLists: IReadOnlyCollection`1, HasObjectLists: Boolean, IncludeAllParameterGroups: Boolean, ParameterGroups: IReadOnlyCollection`1, HasParameterGroups: Boolean, IncludeAllParameters: Boolean, Parameters: IReadOnlyCollection`1, HasParameters: Boolean, Stages: HashSet`1, Transitions: HashSet`1, HasStages: Boolean, HasTransitions: Boolean, IsSchemeExists: Boolean, IncludeAllSignatureTypes: Boolean, SignatureTypes: IReadOnlyCollection`1, HasSignatureTypes: Boolean, IncludeAllViews: Boolean, Views: IReadOnlyCollection`1, HasViews: Boolean
**Методы:**
- `Boolean ContainsView(Guid viewGuid)`
- `Boolean RemoveView(Guid viewGuid)`
- `Boolean AddAccess(Guid accessGuid)`
- `Void AddAccesses(Guid[] accessGuids) (+1)`
- `Boolean ContainsAccess(Guid accessGuid)`
- `Boolean RemoveAccess(Guid accessGuid)`
- `ImportCatalogSettings AddCatalog(Guid guid)`
- `Boolean AddFolders(Guid catalogGuid, Guid[] folderGuids)`
- `Boolean ContainsCatalog(Guid catalogGuid)`
- `Boolean RemoveCatalog(Guid catalogGuid)`
- `Boolean ContainsFolder(Guid folderGuid)`
- `Boolean RemoveFolder(Guid folderGuid)`
- `ImportClassSettings AddClass(Guid guid)`
- `Void AddClasses(Guid[] guids) (+1)`
- `Boolean TryGetClass(Guid guid, ImportClassSettings& settings)`
- `Boolean ContainsClass(Guid guid)`
- `Boolean RemoveClass(Guid guid)`
- `ImportReferenceSettings Create(DataExchangeParameterGroup referenceInfo) (+1)`
- `ImportDialogGroupSettings AddDialogGroup(Guid guid)`
- `Void AddDialogGroups(Guid[] guids) (+1)`
- `Boolean TryGetDialogGroup(Guid guid, ImportDialogGroupSettings& settings)`
- `Boolean ContainsDialogGroup(Guid guid)`
- `Boolean RemoveDialogGroup(Guid guid)`
- `Boolean AddEventHandler(Guid guid)`
- `Void AddEventHandlers(Guid[] guids) (+1)`
- `Boolean ContainsEventHandler(Guid guid)`
- `Boolean RemoveEventHandler(Guid guid)`
- `Boolean AddEvent(Guid guid)`
- `Void AddEvents(Guid[] guids) (+1)`
- `Boolean ContainsEvent(Guid guid)`
- `Boolean RemoveEvent(Guid guid)`
- `ImportLinkSettings AddLink(Guid guid)`
- `Void AddLinks(Guid[] guids) (+1)`
- `Boolean TryGetLink(Guid guid, ImportLinkSettings& settings)`
- `Boolean ContainsLink(Guid guid)`
- `Boolean RemoveLink(Guid guid)`
- `ImportReferenceSettings AddObjectList(Guid guid)`
- `Boolean ContainsObjectList(Guid guid)`
- `Boolean RemoveObjectList(Guid guid)`
- `ImportParameterGroupSettings AddParameterGroup(Guid guid)`
- `Void AddParameterGroups(Guid[] guids) (+1)`
- `Boolean TryGetParameterGroup(Guid guid, ImportParameterGroupSettings& settings)`
- `Boolean ContainsParameterGroup(Guid guid)`
- `Boolean RemoveParameterGroup(Guid guid)`
- `ImportParameterSettings AddParameter(Guid guid)`
- `Void AddParameters(Guid[] guids) (+1)`
- `Boolean TryGetParameter(Guid guid, ImportParameterSettings& settings)`
- `Boolean ContainsParameter(Guid guid)`
- `Boolean RemoveParameter(Guid guid)`
- `ImportSchemeSettings AddScheme(Guid guid)`
- `Boolean AddStage(Int32 stageId)`
- `Void AddStages(IEnumerable`1 stageIds)`
- `Boolean AddTransition(Int32 fromStageId, Int32 toStageId)`
- `Void AddTransitions(IEnumerable`1 transitions)`
- `Boolean ContainsStage(Int32 stageId)`
- `Boolean RemoveStage(Int32 stageId)`
- `Boolean ContainsTransition(Int32 fromStageId, Int32 toStageId)`
- `Boolean RemoveTransition(Int32 fromStageId, Int32 toStageId)`
- `Boolean AddSignatureType(Guid signatureTypeGuid)`
- `Void AddSignatureTypes(Guid[] signatureTypeGuids) (+1)`
- `Boolean ContainsSignatureType(Guid signatureTypeGuid)`
- `Boolean RemoveSignatureType(Guid signatureTypeGuid)`
- `Boolean AddView(Guid viewGuid)`
- `Void AddViews(Guid[] viewGuids) (+1)`

### `ImportSchemeSettings` (Namespace: `TFlex.DOCs.Model.DataExchange.Settings`)
**Свойства:** Guid: Guid, Stages: HashSet`1, Transitions: HashSet`1

### `LinkExportSettings` (Namespace: `TFlex.DOCs.Model.DataExchange.Settings`)
**Свойства:** Connection: ServerConnection, Owner: ExportSettings, LinkGroup: ParameterGroup, IncludeLinkObjectsById: Boolean

### `ExportObjectsPreviewLinkSettingsData` (Namespace: `TFlex.DOCs.Model.DataExchange.Settings.Serialization`)
**Свойства:** LinkGroup: Guid, IncludeLinkObjectsById: Boolean

### `ExportObjectsPreviewReferenceSettingsData` (Namespace: `TFlex.DOCs.Model.DataExchange.Settings.Serialization`)
**Свойства:** Objects: List`1

### `ExportObjectsPreviewSettingsData` (Namespace: `TFlex.DOCs.Model.DataExchange.Settings.Serialization`)
**Свойства:** MasterGroup: Guid, IncludeSignatures: Boolean, DontChangeObjects: Boolean, LoadDirection: ExportSettingsLoadDirection, IncludeAllParameters: Boolean, Parameters: List`1, IncludeAllLinks: Boolean, Links: List`1

### `ExportPreviewSettingsData` (Namespace: `TFlex.DOCs.Model.DataExchange.Settings.Serialization`)
**Свойства:** References: List`1, Objects: List`1

### `ExportReferencePreviewCatalogSettingsData` (Namespace: `TFlex.DOCs.Model.DataExchange.Settings.Serialization`)
**Свойства:** Catalog: Guid, CatalogFolders: List`1

### `ExportReferencePreviewClassSettingsData` (Namespace: `TFlex.DOCs.Model.DataExchange.Settings.Serialization`)
**Свойства:** ClassObject: Guid, IncludeAllProperties: Boolean, Properties: List`1, IncludeAllEventHandlers: Boolean, HandlerEvents: List`1

### `ExportReferencePreviewDialogGroupSettingsData` (Namespace: `TFlex.DOCs.Model.DataExchange.Settings.Serialization`)
**Свойства:** DialogGroup: Guid, IncludeAllDialogPages: Boolean, DialogPages: List`1

### `ExportReferencePreviewEventHandlerSettingsData` (Namespace: `TFlex.DOCs.Model.DataExchange.Settings.Serialization`)
**Свойства:** Event: Guid, Handler: Guid, EntryPoint: String, Data: String

### `ExportReferencePreviewLinkSettingsData` (Namespace: `TFlex.DOCs.Model.DataExchange.Settings.Serialization`)
**Свойства:** LinkGroup: Guid, IncludeAllProperties: Boolean, Properties: List`1

### `ExportReferencePreviewParameterGroupSettingsData` (Namespace: `TFlex.DOCs.Model.DataExchange.Settings.Serialization`)
**Свойства:** ParameterGroup: Guid, IncludeAllProperties: Boolean, Properties: List`1

### `ExportReferencePreviewParameterSettingsData` (Namespace: `TFlex.DOCs.Model.DataExchange.Settings.Serialization`)
**Свойства:** Parameter: Guid, IncludeAllProperties: Boolean, Properties: List`1

### `ExportReferencePreviewRevisionNamingRuleSettingsData` (Namespace: `TFlex.DOCs.Model.DataExchange.Settings.Serialization`)
**Свойства:** RevisionNamingRule: Guid, IncludeAllRevisionLevels: Boolean, RevisionLevels: List`1

### `ExportReferencePreviewSchemeSettingsData` (Namespace: `TFlex.DOCs.Model.DataExchange.Settings.Serialization`)
**Свойства:** Scheme: Guid, Stages: List`1, Transitions: List`1

### `ExportReferencePreviewSettingsData` (Namespace: `TFlex.DOCs.Model.DataExchange.Settings.Serialization`)
**Свойства:** MasterGroup: Guid, IncludeAllProperties: Boolean, Properties: List`1, IncludeAllClasses: Boolean, Classes: List`1, IncludeAllParameters: Boolean, Parameters: List`1, IncludeAllParameterGroups: Boolean, ParameterGroups: List`1, IncludeAllLinks: Boolean, Links: List`1, IncludeAllObjectLists: Boolean, ObjectLists: List`1, IncludeAllEvents: Boolean, Events: List`1, IncludeAllEventHandlers: Boolean, HandlerEvents: List`1, IncludeAllDialogGroups: Boolean, DialogGroups: List`1, IncludeAllAccesses: Boolean, Accesses: List`1, IncludeAllCatalogs: Boolean, Catalogs: List`1, IncludeAllPrototypes: Boolean, Prototypes: List`1, RevisionNamingRule: ExportReferencePreviewRevisionNamingRuleSettingsData, Scheme: ExportReferencePreviewSchemeSettingsData, IncludeAllSignatureTypes: Boolean, SignatureTypes: List`1, IncludeAllViews: Boolean, Views: List`1

### `ExportSettingsData` (Namespace: `TFlex.DOCs.Model.DataExchange.Settings.Serialization`)
**Свойства:** Mode: ExportObjectsMode, Structure: Boolean, SigningParameters: Boolean, Prototypes: Boolean, Views: Boolean, Macros: Boolean, DialogsMode: ExportDialogsMode, Accesses: Boolean, RevisionNamingRules: Boolean, Configurators: Boolean, Stages: Boolean, Coordinates: Boolean, Instances: Boolean, LinkedMode: ExportLinkedObjectsMode, Preview: ExportPreviewSettingsData

### `Changelist` (Namespace: `TFlex.DOCs.Model.Desktop`)
**Свойства:** Connection: ServerConnection, Number: Int32, User: User, UserName: String, HostName: String, Date: DateTime, Comment: String, Label: String
**Методы:**
- `ChangelistObject FindObjectInChangelist(ReferenceObject referenceObject)`
- `IEnumerator`1 GetEnumerator()`

### `ChangelistObject` (Namespace: `TFlex.DOCs.Model.Desktop`)
**Свойства:** Changelist: Changelist, Id: Int32, Guid: Guid, Reference: ReferenceInfo, ReferenceObject: ReferenceObject, CurrentReferenceObject: ReferenceObject, Name: String, Version: Int32, Class: ClassObject, Icon: IconImage, ChangeTypeIcon: IconImage, IsAdded: Boolean, IsDeleted: Boolean, IsUpdated: Boolean, IsRestored: Boolean, IsLabeled: Boolean, SourceVersion: Int32, IsPrototype: Boolean, IsVersionDeleted: Boolean, ChangeTypeDescription: String

### `Desktop` (Namespace: `TFlex.DOCs.Model.Desktop`)
**Методы:**
- `IEnumerable`1 CheckOut(IEnumerable`1 objects, Boolean delete, Object context, Boolean canSetSignaturesNotActual, Int32 packetSize) (+5)` [has Async]
- `IEnumerable`1 CheckIn(IEnumerable`1 objects, String comment, Boolean executeCallBack, Object context, Int32 packetSize, Boolean keepCheckedOut) (+9)` [has Async]
- `IEnumerable`1 UndoCheckOut(IEnumerable`1 objects, Object context, Int32 packetSize) (+4)` [has Async]
- `Void AddLabel(IEnumerable`1 objects, String label, String comment)`
- `List`1 GetCheckedOutObjects(ServerConnection connection) (+2)` [has Async]
- `Boolean HasCheckedOutObjects(ClientView clientView, ReferenceInfo referenceInfo) (+2)` [has Async]
- `Int32 GetCheckedOutObjectsCount(IEnumerable`1 clientViews)`
- `List`1 GetAllCheckedOutObjects(ServerConnection connection) (+1)` [has Async]
- `List`1 GetCurrentUserHistory(ServerConnection connection, Boolean withLabel) (+2)`
- `List`1 GetUserHistory(User user, HistoryFilters filters, Boolean withLabel) (+1)`
- `List`1 GetAllUserHistory(ServerConnection connection, Boolean withLabel) (+2)`
- `List`1 GetChangelists(ReferenceObject referenceObject, Boolean withLabel)`
- `List`1 GetObjectChangelists(ReferenceObject referenceObject, Boolean withLabel)`
- `IEnumerable`1 Restore(IEnumerable`1 objects, Boolean restoreChildren, String comment, Boolean skipLinks, Int32 packetSize, Boolean executeCallback) (+2)` [has Async]
- `ReferenceObject CheckOutVersion(ReferenceObject referenceObject, Int32 version, Boolean canSetSignaturesNotActual) (+1)`
- `Boolean ClearRecycleBin(IEnumerable`1 desktopObjects, Int32 packetSize) (+1)` [has Async]

### `DesktopOperationInfo` (Namespace: `TFlex.DOCs.Model.Desktop`)
**Свойства:** Connection: ServerConnection, Type: DesktopOperationType, Count: Int32, Items: ReadOnlyCollection`1, Comment: String, KeepCheckedOut: Boolean
**Методы:**
- `Void AddNewLinkedObjects()` [has Async]
- `List`1 GetObjects()`
- `DesktopOperationItem Find(DesktopObject desktopObject)`
- `Boolean Contains(DesktopOperationItem item)`
- `Boolean Remove(DesktopOperationItem item)`
- `DesktopOperationItem Add(DesktopObject desktopObject)`
- `Void AddContext(DesktopOperationInfoContext context)`
- `DesktopOperationInfoContext FindContext(DesktopOperationInfoContextType type)`

### `DesktopOperationInfoContext` (Namespace: `TFlex.DOCs.Model.Desktop`)
**Свойства:** Type: DesktopOperationInfoContextType

### `DesktopOperationItem` (Namespace: `TFlex.DOCs.Model.Desktop`)
**Свойства:** Item: DesktopObject, ReferenceId: Int32, ObjectId: Int32, DependentItems: ReadOnlyCollection`1
**Методы:**
- `HashSet`1 GetAllDependentItems()`
- `Void MergeAllDependentItems(HashSet`1 items)`

### `HistoryFilters` (Namespace: `TFlex.DOCs.Model.Desktop`)
**Свойства:** Connection: ServerConnection, FromDateTime: Nullable`1, ToDateTime: Nullable`1, FromChangelistNumber: Int32, ToChangelistNumber: Int32, HostName: String

### `MoveFileReferenceObjectToStorageContext` (Namespace: `TFlex.DOCs.Model.Desktop`)
**Свойства:** CheckedOutObjectsWithChangedStorage: HashSet`1, Type: DesktopOperationInfoContextType

### `ConnectionInfo` (Namespace: `TFlex.DOCs.Model.Diagnostics`)
**Свойства:** ConnectionCount: Int32
**Методы:**
- `List`1 GetLockedLicenses()`
- `Boolean IsLocked(ModuleLicense license)`

### `ReferencePathExtensions` (Namespace: `TFlex.DOCs.Model.Exceptions`)
**Методы:**
- `String GetGuidStringToPathItem(GroupPathItem linkPathItem) (+1)`

### `ISupportPreviewParameters` (Namespace: `TFlex.DOCs.Model.FilePreview`)
**Методы:**
- `IEnumerable`1 GetPreviewParameters()`

### `IVariable` (Namespace: `TFlex.DOCs.Model.FilePreview`)
**Свойства:** Name: String, TextValue: String, RealValue: Double, IsText: Boolean, Comment: String

### `PreviewParameterInfo` (Namespace: `TFlex.DOCs.Model.FilePreview`)
**Свойства:** Type: Type, Value: Object, Name: String

### `CombineFilesProvider` (Namespace: `TFlex.DOCs.Model.FilePreview.CADExchange`)
**Свойства:** OutputPath: String, PageTypes: Int32[], FilesToCombine: String[], ShortDenotations: String[], IsEmbedded: Boolean
**Методы:**
- `Byte[] Serialize()`
- `CombineFilesProvider Deserialize(Byte[] data)`
- `Void Execute(ServerConnection connection)`
- `String GetLinkPath(String file)`

### `CADBaseValue` (Namespace: `TFlex.DOCs.Model.FilePreview.CADInteraction`)
**Свойства:** ParameterInfo: CADParameterInfo

### `CADDim` (Namespace: `TFlex.DOCs.Model.FilePreview.CADInteraction`)
**Методы:**
- `Void Accept(ICadObjectVisitor visitor)`

### `CADElementTypeValue` (Namespace: `TFlex.DOCs.Model.FilePreview.CADInteraction`)
**Свойства:** DefaultEnumValue: CADElementTypeValue, DefaultValueAsObject: Object, DefaultValueAsString: String

### `CADElementValue` (Namespace: `TFlex.DOCs.Model.FilePreview.CADInteraction`)
**Свойства:** EnumValue: CADElementTypeValue, ValueAsObject: Object, ValueAsString: String, AttachedVariable: CADVar

### `CADFragment` (Namespace: `TFlex.DOCs.Model.FilePreview.CADInteraction`)
**Методы:**
- `Void Accept(ICadObjectVisitor visitor)`

### `CADLinkDescriptorInfo` (Namespace: `TFlex.DOCs.Model.FilePreview.CADInteraction`)
**Свойства:** LinkBackName: String, LinkName: String, LinkSystemName: String, UID: Guid

### `CADLinkedObjectInfo` (Namespace: `TFlex.DOCs.Model.FilePreview.CADInteraction`)
**Свойства:** LinkDescriptor: CADLinkDescriptorInfo, LinkedObjectSearchString: String, LinkedObjectGroupType: CADObjectTypes, LinkedObjectTypeSystemName: String

### `CadMeasure` (Namespace: `TFlex.DOCs.Model.FilePreview.CADInteraction`)
**Свойства:** Unit: String, ShowFullText: Boolean

### `CADMethodInfo` (Namespace: `TFlex.DOCs.Model.FilePreview.CADInteraction`)
**Свойства:** DisplayName: String, ID: Guid, IsAuxiliary: Boolean, IsStatic: Boolean, SystemName: String

### `CADObject` (Namespace: `TFlex.DOCs.Model.FilePreview.CADInteraction`)
**Свойства:** ID: UInt32, Name: String, DisplayName: String, CADType: CADObjectTypes
**Методы:**
- `String GetSearchIdentifier()`
- `Void Accept(ICadObjectVisitor visitor)`
- `Void SetAdditionalProperties(Object sourceObject)`

### `CADObjectInfo` (Namespace: `TFlex.DOCs.Model.FilePreview.CADInteraction`)
**Свойства:** ObjectId: UInt64, DocumentFileName: String, DisplayName: String, ObjectName: String, SearchString: String, SubType: CADObjectTypes, HasTable: Boolean, Visible: Boolean, Properties: CADObjectPropertyInfo[]

### `CADObjectPropertyInfo` (Namespace: `TFlex.DOCs.Model.FilePreview.CADInteraction`)
**Свойства:** Name: String, Description: String, DataType: CADObjectPropertiesTypes, MeasureUnit: String, Value: Object

### `CADParameterInfo` (Namespace: `TFlex.DOCs.Model.FilePreview.CADInteraction`)
**Свойства:** DisplayName: String, ID: Guid, IsAuxiliary: Boolean, IsStatic: Boolean, ReferenceTypeID: Guid, SystemName: String

### `CADStructureElementInfo` (Namespace: `TFlex.DOCs.Model.FilePreview.CADInteraction`)
**Свойства:** StructureElementType: CADStructureElementTypeInfo, UID: Guid, ValueCollection: CADElementValueCollection, LinkedObjectInfoCollection: CADLinkedObjectInfoCollection

### `CADStructureElementTypeInfo` (Namespace: `TFlex.DOCs.Model.FilePreview.CADInteraction`)
**Свойства:** IsEnum: Boolean, Name: String, Description: String, SystemName: String, SystemParentName: String, ParentElementType: CADStructureElementTypeInfo, UID: Guid, Icon: String, IsAbstract: Boolean, TypeValueCollection: CADElementTypeValueCollection, MethodInfoCollection: CADMethodInfoCollection
**Методы:**
- `List`1 GetHierarchyTypes()`

### `CADVar` (Namespace: `TFlex.DOCs.Model.FilePreview.CADInteraction`)
**Свойства:** Name: String, TextValue: String, RealValue: Double, IsText: Boolean, Comment: String

### `ICADObject` (Namespace: `TFlex.DOCs.Model.FilePreview.CADInteraction`)
**Свойства:** ID: UInt32, Name: String, DisplayName: String, CADType: CADObjectTypes
**Методы:**
- `String GetSearchIdentifier()`
- `Void Accept(ICadObjectVisitor visitor)`

### `ICadObjectVisitor` (Namespace: `TFlex.DOCs.Model.FilePreview.CADInteraction`)
**Методы:**
- `Void Visit(CADArea cadObject) (+4)`

### `IInteractiveCADControl` (Namespace: `TFlex.DOCs.Model.FilePreview.CADInteraction`)
**Свойства:** Handle: IntPtr, SelectedObject: CADObjectInfo, SelectedObjects: IEnumerable`1, Document: SharedDocument, IsInternalControlDisposed: Boolean
**Методы:**
- `Void SelectCadObjects(IReadOnlyCollection`1 items)`
- `Void ChangeTree(String configuration)`
- `InteractiveCADVersionInfo GetVersion()`
- `Void BeginSelectionSession(CADObjectTypes[] selectableTypes)`
- `Void EndSelectionSession()`
- `Void ShowStepText(String str)`
- `IEnumerable`1 GetSelectionAllObjects()`
- `Void SetSelectionFragments(Guid[] guids)`
- `Void SetCADSelection(Boolean enable)`

### `IInteractiveCadControlProvider` (Namespace: `TFlex.DOCs.Model.FilePreview.CADInteraction`)
**Свойства:** SynchronizationContext: SynchronizationContext
**Методы:**
- `IInteractiveCADControl FindInteractiveControl(IntPtr ptr)`
- `Boolean LoadApi()`

### `InteractiveCADVersionChecker` (Namespace: `TFlex.DOCs.Model.FilePreview.CADInteraction`)
**Методы:**
- `Boolean Check(InteractiveCADVersionInfo versionInfo)`
- `Boolean SupportStructureElements(InteractiveCADVersionInfo versionInfo)`

### `InteractiveCADVersionInfo` (Namespace: `TFlex.DOCs.Model.FilePreview.CADInteraction`)
**Свойства:** Product: InteractiveCADs, Version: Version

### `CadDocument` (Namespace: `TFlex.DOCs.Model.FilePreview.CADService`)
**Свойства:** Path: String, ReadOnly: Boolean, Context: Object, Id: Int32, IsActive: Boolean, Provider: CadDocumentProvider
**Методы:**
- `Void Open(CadDocumentProvider provider)`
- `Void Close(Boolean save)`
- `VariableCollection GetVariables()`
- `FragmentCollection GetFragments2D()`
- `FragmentCollection GetFragments3D()`
- `LCSCollection GetLCSs()`
- `ConnectorCollection GetConnectors2D()`
- `ConnectorCollection GetConnectors3D()`
- `Fragment3D InsertFragment3D(LCS documentLCS, LCS fragmentLCS, String fragmentPath, Boolean embedded) (+1)`
- `Void SaveInNomenclature(ICollection`1 productStructures, Boolean autoCheckIn) (+1)`
- `ICollection`1 GetProductStructures()`
- `CadDocument OpenPart(Fragment fragment)`
- `CadDocument OpenLink(Fragment fragment)`
- `Void Regenerate(RegenerateOption regenerateOption) (+1)`
- `QualityAnalysisResult[] Analyze3DModelQuality(String reportFilePath, String analysisScript)`
- `String Export(ExportContext exportContext)`
- `PageInfo[] GetPagesInfo(Int32[] types)`

### `CadDocumentProvider` (Namespace: `TFlex.DOCs.Model.FilePreview.CADService`)
**Свойства:** IsActive: Boolean, Context: Object, Connection: ServerConnection, FilePreview: FilePreviewType
**Методы:**
- `CadDocumentProvider Connect(ServerConnection connection, String extension, String integrationRule, Object context) (+1)`
- `CadDocument OpenDocument(String path, Boolean readOnly, Object context)`
- `CadDocument OpenVirtualDocument(ReferenceObject referenceObject, Boolean readOnly, Object context)`
- `String RegisterLibrary(String path)`
- `RestResponse CallPluginRestService(String pluginName, RestRequest restRequest)`

### `Connector` (Namespace: `TFlex.DOCs.Model.FilePreview.CADService`)
**Свойства:** IsActive: Boolean
**Методы:**
- `VariableCollection GetVariables()`

### `Connector2D` (Namespace: `TFlex.DOCs.Model.FilePreview.CADService`)
**Свойства:** StartNode: Point2D, EndNode: Point2D
**Методы:**
- `Void Initialize(Point2D startNode, Point2D endNode)`
- `Double GetAngle()`

### `Connector3D` (Namespace: `TFlex.DOCs.Model.FilePreview.CADService`)
**Свойства:** LCS: LCS

### `ExportContext` (Namespace: `TFlex.DOCs.Model.FilePreview.CADService`)
**Свойства:** Path: String, Page: Int32, Pages: List`1, Parameters: Dictionary`2, IsRepresentation: Boolean, Item: Object
**Методы:**
- `Boolean ContainsParameter(String key)`
- `IEnumerable`1 GetParametersEnumerator()`

### `Fragment` (Namespace: `TFlex.DOCs.Model.FilePreview.CADService`)
**Свойства:** Document: CadDocument, IsActive: Boolean
**Методы:**
- `VariableCollection GetVariables()`
- `Nullable`1 GetRealProperty(String propertyName)`
- `String GetTextProperty(String propertyName)`
- `PropertyCollection GetProperties()`
- `CadDocument OpenPart()`
- `CadDocument OpenLink()`

### `Fragment3D` (Namespace: `TFlex.DOCs.Model.FilePreview.CADService`)
**Методы:**
- `LCSCollection GetLCSs()`
- `Nullable`1 CompareTo(String compareToDocument, LCS sourceLCS, String compareToDocumentLCS)`
- `Boolean IsCorrect()`

### `LCS` (Namespace: `TFlex.DOCs.Model.FilePreview.CADService`)
**Свойства:** Origin: Point3D, PointX: Point3D, PointY: Point3D, PointZ: Point3D
**Методы:**
- `Void Initialize(Point3D origin, Point3D pointX, Point3D pointY, Point3D pointZ)`

### `ModelObject` (Namespace: `TFlex.DOCs.Model.FilePreview.CADService`)
**Свойства:** Name: String, UniqueId: String

### `ModelObjectCollection`1` (Namespace: `TFlex.DOCs.Model.FilePreview.CADService`)
**Свойства:** Count: Int32, IsReadOnly: Boolean
**Методы:**
- `Boolean Add(T modelObject)`
- `T Find(String name)`
- `Void ForEach(Action`1 action)`
- `IEnumerator`1 GetEnumerator()`
- `Boolean Contains(T item)`
- `Void CopyTo(T[] array, Int32 arrayIndex)`

### `PageInfo` (Namespace: `TFlex.DOCs.Model.FilePreview.CADService`)
**Свойства:** Name: String, Visible: Boolean, Type: Int32, Index: Int32, Properties: PageProperties

### `PageProperties` (Namespace: `TFlex.DOCs.Model.FilePreview.CADService`)
**Свойства:** Paper: PaperInfo

### `PaperInfo` (Namespace: `TFlex.DOCs.Model.FilePreview.CADService`)
**Свойства:** Format: String, Orientation: PaperOrientation

### `Point2D` (Namespace: `TFlex.DOCs.Model.FilePreview.CADService`)
**Свойства:** X: Double, Y: Double

### `Point3D` (Namespace: `TFlex.DOCs.Model.FilePreview.CADService`)
**Свойства:** Z: Double

### `ProductStructure` (Namespace: `TFlex.DOCs.Model.FilePreview.CADService`)
**Свойства:** ObjectId: String, Name: String

### `Property` (Namespace: `TFlex.DOCs.Model.FilePreview.CADService`)
**Свойства:** Name: String, Value: Object, DataValue: DynamicData

### `Property`1` (Namespace: `TFlex.DOCs.Model.FilePreview.CADService`)
**Свойства:** Value: T

### `RestRequest` (Namespace: `TFlex.DOCs.Model.FilePreview.CADService`)
**Свойства:** Id: String, Data: String, Format: String

### `RestResponse` (Namespace: `TFlex.DOCs.Model.FilePreview.CADService`)
**Свойства:** Data: String, Format: String, ResponseStatus: RestResponseStatus

### `Unit` (Namespace: `TFlex.DOCs.Model.FilePreview.CADService`)
**Свойства:** FullName: String, ShortName: String, TypeName: String

### `Variable` (Namespace: `TFlex.DOCs.Model.FilePreview.CADService`)
**Свойства:** Name: String, Value: Object, Expression: String, Description: String, IsExternal: Boolean, GroupName: String, Hidden: Boolean, Unit: Unit, IsModify: Boolean, Owner: VariableCollection
**Методы:**
- `Void SetModify()`
- `Void CopyFrom(Variable other)`

### `Variable`1` (Namespace: `TFlex.DOCs.Model.FilePreview.CADService`)
**Свойства:** Value: T

### `CadDocumentExtensions` (Namespace: `TFlex.DOCs.Model.FilePreview.CADService.TFlexCadDocument`)
**Методы:**
- `TFlexPageInfo[] GetTFlexPagesInfo(CadDocument document, TFlexPageType[] pageTypes)`

### `TFlexPageInfo` (Namespace: `TFlex.DOCs.Model.FilePreview.CADService.TFlexCadDocument`)
**Свойства:** Name: String, PageType: TFlexPageType, Index: Int32, Visible: Boolean, Properties: PageProperties

### `CADClient` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract`)
**Свойства:** Info: ServiceInfo
**Методы:**
- `CadDocumentRequestResult Open(String path, Boolean readOnly, Byte[] byteContext, ClientCallContext context)` [has Async]
- `CadDocumentRequestResult OpenVirtual(Guid referenceObjectGuid, Boolean readOnly, Byte[] byteContext, ClientCallContext context)` [has Async]
- `RequestResult Close(CadDocumentData document, Boolean save, ClientCallContext context)` [has Async]
- `StringRequestResult Export(CadDocumentData document, ExportContext exportContext, ClientCallContext context)` [has Async]
- `RequestResult Regenerate(CadDocumentData document, ClientCallContext context)` [has Async]
- `RequestResult RegenerateWithOption(CadDocumentData document, RegenerateOption regenerateOption, ClientCallContext context)` [has Async]
- `QualityAnalysisCollectionRequestResult Analyze3DModelQuality(CadDocumentData document, String reportFilePath, String analysisScript, ClientCallContext context)` [has Async]
- `VariableCollectionRequestResult GetVariables(CadDocumentData document, String ownerId, ClientCallContext context)` [has Async]
- `VariableCollectionRequestResult SaveVariables(CadDocumentData document, String ownerId, List`1 variables, ClientCallContext context)` [has Async]
- `FragmentCollectionRequestResult GetFragments2D(CadDocumentData document, ClientCallContext context)` [has Async]
- `FragmentCollectionRequestResult GetFragments3D(CadDocumentData document, ClientCallContext context)` [has Async]
- `ConnectorCollectionRequestResult GetConnectors2D(CadDocumentData document, String ownerId, ClientCallContext context)` [has Async]
- `ConnectorCollectionRequestResult GetConnectors3D(CadDocumentData document, String ownerId, ClientCallContext context)` [has Async]
- `LCSCollectionRequestResult GetLCSs(CadDocumentData document, String ownerId, ClientCallContext context)` [has Async]
- `Fragment3DRequestResult InsertFragment3D(CadDocumentData targetDocument, String targetLCSName, String fragmentPath, String fragmentLCSName, Boolean byConnector, Boolean embedded, ClientCallContext context)` [has Async]
- `RequestResult SaveInNomenclature(CadDocumentData document, Boolean recursive, Boolean autoCheckIn, ICollection`1 productStructures, ClientCallContext context)` [has Async]
- `ProductStructureCollectionRequestResult GetProductStructures(CadDocumentData documentData, ClientCallContext context)` [has Async]
- `CadDocumentRequestResult OpenPart(CadDocumentData document, String ownerId, ClientCallContext context)` [has Async]
- `CadDocumentRequestResult OpenLink(CadDocumentData document, String ownerId, ClientCallContext context)` [has Async]
- `NullableDoubleRequestResult GetRealProperty(CadDocumentData document, String ownerId, String propertyName, ClientCallContext context)` [has Async]
- `StringRequestResult GetTextProperty(CadDocumentData document, String ownerId, String propertyName, ClientCallContext context)` [has Async]
- `PropertyCollectionRequestResult GetProperties(CadDocumentData document, String ownerId, ClientCallContext context)` [has Async]
- `DoubleRequestResult Compare(CadDocumentData document, String ownerId, String ownerLcs, String filePath, String fileLcs, ClientCallContext context)` [has Async]
- `StringRequestResult OpenLibrary(String fullPath, ClientCallContext context)` [has Async]
- `StringRequestResult CloseLibrary(String fullPath, ClientCallContext context)` [has Async]
- `RequestResult SetIntegrationRule(String integrationRule, ClientCallContext context)` [has Async]
- `BoolRequestResult IsCorrect(CadDocumentData document, String ownerId, ClientCallContext context)` [has Async]
- `PageInfoArrayRequestResult GetPagesInfo(CadDocumentData document, Int32[] types, ClientCallContext context)` [has Async]
- `RestResponseRequestResult CallPluginRestService(String pluginName, RestRequestData restRequest, ClientCallContext context)` [has Async]

### `CadDocumentData` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract`)
**Свойства:** Path: String, ReadOnly: Boolean, ByteContext: Byte[], Id: Int32, VirtualAssembly: Guid, Context: Object

### `CADServer` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract`)
**Свойства:** Info: ServiceInfo
**Методы:**
- `ValueTask`1 Open(String path, Boolean readOnly, Byte[] byteContext, ServerCallContext context)`
- `ValueTask`1 OpenVirtual(Guid referenceObjectGuid, Boolean readOnly, Byte[] byteContext, ServerCallContext context)`
- `ValueTask`1 Close(CadDocumentData document, Boolean save, ServerCallContext context)`
- `ValueTask`1 Export(CadDocumentData document, ExportContext exportContext, ServerCallContext context)`
- `ValueTask`1 Regenerate(CadDocumentData document, ServerCallContext context)`
- `ValueTask`1 RegenerateWithOption(CadDocumentData document, RegenerateOption regenerateOption, ServerCallContext context)`
- `ValueTask`1 Analyze3DModelQuality(CadDocumentData document, String reportFilePath, String analysisScript, ServerCallContext context)`
- `ValueTask`1 GetVariables(CadDocumentData document, String ownerId, ServerCallContext context)`
- `ValueTask`1 SaveVariables(CadDocumentData document, String ownerId, List`1 variables, ServerCallContext context)`
- `ValueTask`1 GetFragments2D(CadDocumentData document, ServerCallContext context)`
- `ValueTask`1 GetFragments3D(CadDocumentData document, ServerCallContext context)`
- `ValueTask`1 GetConnectors2D(CadDocumentData document, String ownerId, ServerCallContext context)`
- `ValueTask`1 GetConnectors3D(CadDocumentData document, String ownerId, ServerCallContext context)`
- `ValueTask`1 GetLCSs(CadDocumentData document, String ownerId, ServerCallContext context)`
- `ValueTask`1 InsertFragment3D(CadDocumentData targetDocument, String targetLCSName, String fragmentPath, String fragmentLCSName, Boolean byConnector, Boolean embedded, ServerCallContext context)`
- `ValueTask`1 SaveInNomenclature(CadDocumentData document, Boolean recursive, Boolean autoCheckIn, ICollection`1 productStructures, ServerCallContext context)`
- `ValueTask`1 GetProductStructures(CadDocumentData documentData, ServerCallContext context)`
- `ValueTask`1 OpenPart(CadDocumentData document, String ownerId, ServerCallContext context)`
- `ValueTask`1 OpenLink(CadDocumentData document, String ownerId, ServerCallContext context)`
- `ValueTask`1 GetRealProperty(CadDocumentData document, String ownerId, String propertyName, ServerCallContext context)`
- `ValueTask`1 GetTextProperty(CadDocumentData document, String ownerId, String propertyName, ServerCallContext context)`
- `ValueTask`1 GetProperties(CadDocumentData document, String ownerId, ServerCallContext context)`
- `ValueTask`1 Compare(CadDocumentData document, String ownerId, String ownerLcs, String filePath, String fileLcs, ServerCallContext context)`
- `ValueTask`1 OpenLibrary(String fullPath, ServerCallContext context)`
- `ValueTask`1 CloseLibrary(String fullPath, ServerCallContext context)`
- `ValueTask`1 SetIntegrationRule(String integrationRule, ServerCallContext context)`
- `ValueTask`1 IsCorrect(CadDocumentData document, String ownerId, ServerCallContext context)`
- `ValueTask`1 GetPagesInfo(CadDocumentData document, Int32[] types, ServerCallContext context)`
- `ValueTask`1 CallPluginRestService(String pluginName, RestRequestData restRequest, ServerCallContext context)`

### `ExceptionHandler` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract`)
**Методы:**
- `Void Invoke(Exception e)`
- `IAsyncResult BeginInvoke(Exception e, AsyncCallback callback, Object object)`
- `Void EndInvoke(IAsyncResult result)`

### `ExchangePluginData` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract`)
**Свойства:** PluginModuleName: String, PluginClassName: String, Data: Byte[]

### `FilePreviewClient` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract`)
**Свойства:** Info: ServiceInfo
**Методы:**
- `IntPtr ShowFile(Int32 id, ShowFileContext fileContext, ClientCallContext context)` [has Async]
- `Void SetControlSize(Int32 id, Int32 width, Int32 height, ClientCallContext context)` [has Async]
- `Byte[] ExchangePluginData(String pluginModuleName, String pluginClassName, Int32 controlId, Byte[] data, ClientCallContext context)` [has Async]
- `Byte[] GenerateReport(String moduleName, String className, Byte[] data, ClientCallContext context)` [has Async]
- `Int32 GetImagePageCount(Int32 id, String filePath, ClientCallContext context)` [has Async]
- `Byte[] GetPreviewImage(Int32 id, String filePath, Int32 pageIndex, ClientCallContext context)` [has Async]
- `Boolean IsInstalledPreviewProgram(Int32 id, String extension, Boolean isAnyCPUMode, ClientCallContext context)` [has Async]
- `String GetCadServiceAddress(Int32 communication, Int32 dataSerializer, ClientCallContext context)` [has Async]
- `String GetTechnologyCadExchangeServiceAddress(Int32 communication, Int32 dataSerializer, String assemblyPath, String assemblyName, ClientCallContext context)` [has Async]
- `Boolean IsSupportSaving(Int32 id, ClientCallContext context)` [has Async]
- `Boolean SaveAs(Int32 id, String filePath, ClientCallContext context)` [has Async]
- `Void CloseFilePreviews(String file, ClientCallContext context)` [has Async]
- `Boolean IsDocumentChanged(Int32 id, ClientCallContext context)` [has Async]
- `Void SaveChanges(Int32 id, ClientCallContext context)` [has Async]
- `String GetFilePreviewInformation(Int32 id, ClientCallContext context)` [has Async]
- `Void Print(Int32 id, ClientCallContext context)` [has Async]
- `Tuple`2 ExecuteDocumentRequest(Int32 id, FilePreviewRequest request, ClientCallContext context)` [has Async]

### `FilePreviewerDescriptionAttribute` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract`)
**Свойства:** Name: String, ErrorMessage: String, Extensions: String, Platform: CPUDigitCapacity, SupportExecutionInCurrentProcess: Boolean, IsDefault: Boolean, Key: String, IgnoreInListPreviewModule: Boolean

### `FilePreviewLoaderManager` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract`)
**Методы:**
- `IFilePreviewManager CreatePreviewByType(String type)`
- `IFilePreviewManager CreatePreviewFromAssembly(String assemblyPath, String assemblyName)`
- `T CreateHandler(String assemblyPath, String assemblyName, Type baseType, String previewKey)`
- `Type GetTypeByServiceBase(String assemblyPath, String assemblyName, String baseServiceName)`
- `String GetImageTempPath()`
- `Byte[] LoadImageFromFile(String filePath)`

### `FilePreviewManager` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract`)
**Методы:**
- `Int32 GetImagePageCount(String filePath)`
- `Byte[] GetPreviewImage(String filePath, Int32 pageIndex)`
- `Boolean IsInstalledPreviewProgram(Boolean isAnyCPUMode)`

### `FilePreviewServer` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract`)
**Свойства:** Info: ServiceInfo, BackwardExchangePluginData: FeedbackWriter`2
**Методы:**
- `ValueTask`1 ShowFile(Int32 id, ShowFileContext fileContext, ServerCallContext context)`
- `ValueTask SetControlSize(Int32 id, Int32 width, Int32 height, ServerCallContext context)`
- `ValueTask`1 ExchangePluginData(String pluginModuleName, String pluginClassName, Int32 controlId, Byte[] data, ServerCallContext context)`
- `ValueTask`1 GenerateReport(String moduleName, String className, Byte[] data, ServerCallContext context)`
- `ValueTask`1 GetImagePageCount(Int32 id, String filePath, ServerCallContext context)`
- `ValueTask`1 GetPreviewImage(Int32 id, String filePath, Int32 pageIndex, ServerCallContext context)`
- `ValueTask`1 IsInstalledPreviewProgram(Int32 id, String extension, Boolean isAnyCPUMode, ServerCallContext context)`
- `ValueTask`1 GetCadServiceAddress(Int32 communication, Int32 dataSerializer, ServerCallContext context)`
- `ValueTask`1 GetTechnologyCadExchangeServiceAddress(Int32 communication, Int32 dataSerializer, String assemblyPath, String assemblyName, ServerCallContext context)`
- `ValueTask`1 IsSupportSaving(Int32 id, ServerCallContext context)`
- `ValueTask`1 SaveAs(Int32 id, String filePath, ServerCallContext context)`
- `ValueTask CloseFilePreviews(String file, ServerCallContext context)`
- `ValueTask`1 IsDocumentChanged(Int32 id, ServerCallContext context)`
- `ValueTask SaveChanges(Int32 id, ServerCallContext context)`
- `ValueTask`1 GetFilePreviewInformation(Int32 id, ServerCallContext context)`
- `ValueTask Print(Int32 id, ServerCallContext context)`
- `Void SetupBackwardExchangePluginData(FeedbackWriter`2 notify)`
- `ValueTask`1 ExecuteDocumentRequest(Int32 id, FilePreviewRequest request, ServerCallContext context)`

### `FileShownHandler` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract`)
**Методы:**
- `Void Invoke(IFilePreviewContext context)`
- `IAsyncResult BeginInvoke(IFilePreviewContext context, AsyncCallback callback, Object object)`
- `Void EndInvoke(IAsyncResult result)`

### `IFileObjectEditingState` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract`)
**Свойства:** CanEditDocument: Boolean

### `IFilePreviewContext` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract`)
**Свойства:** LastModificationTime: DateTime, FilePath: String, FileExtension: String, Parameters: String, CustomFilePreviewerGuid: Guid, DefaultFilePreviewerGuid: Guid, ShowToolsButtons: Boolean, LinkedObjectId: Int32, LinkedReferenceId: Int32, LinkedObjectGuid: Guid, HierarchyLink: Guid, EnablePrint: Boolean, AsyncModeSupported: Boolean, ConfigurationSettings: String, ObjectContext: String
**Методы:**
- `Void GenerateFile()`
- `Boolean IsChanged(IFilePreviewContext filePreviewContext)`

### `IFilePreviewContextLinkedInstance` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract`)
**Свойства:** LinkedObjectInstanceGuid: Guid

### `IFilePreviewControl` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract`)
**Свойства:** FilePreviewInformation: String, SupportsSaving: Boolean
**Методы:**
- `Void ShowFile(String fileName, Object context) (+1)`
- `Void ShowFileWithParameters(String fileName, String parameters)`
- `Boolean SaveAs(String filePath)`

### `IFilePreviewManager` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract`)
**Методы:**
- `Int32 GetImagePageCount(String filePath)`
- `Byte[] GetPreviewImage(String filePath, Int32 pageIndex)`
- `Boolean IsInstalledPreviewProgram(Boolean isAnyCPUMode)`

### `IFilePreviewPlugin` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract`)
**Методы:**
- `Byte[] ExchangeData(Object control, Byte[] data, IFilePreviewPluginCallback callback)`

### `IFilePreviewPluginCallback` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract`)
**Методы:**
- `Task`1 BackwardExchangeData(String pluginModuleName, String pluginClassName, Byte[] data)`

### `IInteractiveFilePreviewControl` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract`)
**Методы:**
- `Boolean IsDocumentChanged()`
- `Void SaveChanges()`
- `Void ShowVirtualAssembly(ShowFileContext showFileContext)`

### `ISupportDynamicPreviewContext` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract`)
**Свойства:** LoadByDataModel: Guid, UseDynamicPreview: Boolean

### `ISupportPrint` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract`)
**Свойства:** CanPrint: Boolean
**Методы:**
- `Void Print()`

### `ISupportServerConnection` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract`)
**Методы:**
- `Void SetServerConnection(ServerConnection serverConnection, Boolean isAnnotationEnabled)`

### `ITFlexCadFilePreview` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract`)
**Методы:**
- `Void ShowFile(String fileName, Object context, TFlexCadWindowType windowType, Boolean readOnly, Nullable`1 viewAreaSize)`

### `ProductStructureData` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract`)
**Свойства:** ObjectId: String, Name: String

### `RequestResult` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract`)
**Свойства:** Error: InternalErrorData
**Методы:**
- `Void SetError(Exception exception)`
- `Boolean IsNullOrFaulted(RequestResult result)`

### `RequestResult`1` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract`)
**Свойства:** Value: T

### `ShowFileContext` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract`)
**Свойства:** FilePath: String, OpeningDocumentId: Int32, OpeningObjectGuid: Guid, Parameters: String, CanPrint: Boolean, AdditionalUniqueKey: String, ReadOnly: Boolean, TFlexCadWindowType: Int32, ViewAreaWidth: Int32, ViewAreaHeight: Int32, LinkedObjectInstanceGuid: Guid, ViewAreaSize: Size

### `Connector2DData` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract.Models`)
**Свойства:** StartNode: Point2DData, EndNode: Point2DData

### `Connector3DData` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract.Models`)
**Свойства:** Lcs: LcsData

### `DoubleVariableData` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract.Models`)
**Свойства:** Value: Double

### `IntPropertyData` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract.Models`)
**Свойства:** Value: Int32

### `LcsData` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract.Models`)
**Свойства:** Origin: Point3DData, PointX: Point3DData, PointY: Point3DData, PointZ: Point3DData

### `ModelObjectData` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract.Models`)
**Свойства:** Name: String, UniqueId: String

### `Point2DData` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract.Models`)
**Свойства:** X: Double, Y: Double

### `Point3DData` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract.Models`)
**Свойства:** Z: Double

### `PropertyData` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract.Models`)
**Свойства:** Name: String

### `QualityAnalysisResult` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract.Models`)
**Свойства:** State: QualityAnalysisState, FullMessage: String, ShortMessage: String, IsFixable: Boolean

### `RealPropertyData` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract.Models`)
**Свойства:** Value: Double

### `RestRequestData` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract.Models`)
**Свойства:** Id: String, Data: String, Format: String

### `RestResponseData` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract.Models`)
**Свойства:** Data: String, Format: String, ResponseStatus: RestResponseStatus

### `StringVariableData` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract.Models`)
**Свойства:** Value: String

### `TextPropertyData` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract.Models`)
**Свойства:** Value: String

### `UnitData` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract.Models`)
**Свойства:** FullName: String, ShortName: String, TypeName: String

### `VariableData` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract.Models`)
**Свойства:** Name: String, Expression: String, Description: String, IsExternal: Boolean, GroupName: String, Hidden: Boolean, Unit: UnitData

### `FilePreviewCommand` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract.QueryFilePreview`)
**Методы:**
- `Void Accept(ICommandVisitor visitor)`

### `FilePreviewRequest` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract.QueryFilePreview`)
**Свойства:** ObjectCommands: List`1, SelectCommands: List`1

### `ICommandVisitor` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract.QueryFilePreview`)
**Методы:**
- `Void Visit(Fragment3DCommand command) (+3)`

### `IQueryFilePreviewControl` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract.QueryFilePreview`)
**Методы:**
- `Boolean ExecuteDocumentRequest(FilePreviewRequest request, String& requestId)`

### `Fragment2DCommand` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract.QueryFilePreview.Fragments`)
**Свойства:** Priority: Int32, Angle: Int32
**Методы:**
- `Void Accept(ICommandVisitor visitor)`

### `Fragment3DCommand` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract.QueryFilePreview.Fragments`)
**Свойства:** Z: Int32, Transformation: Double[]
**Методы:**
- `Void Accept(ICommandVisitor visitor)`

### `FragmentCommandBase` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract.QueryFilePreview.Fragments`)
**Свойства:** Id: Guid, Name: String, File: String, Layer: String, X: Int32, Y: Int32, Variable: Variable
**Методы:**
- `Void Accept(ICommandVisitor visitor)`

### `Variable` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract.QueryFilePreview.Fragments`)
**Свойства:** Name: String, Value: String

### `DeselectObjectCommand` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract.QueryFilePreview.SelectObjects`)
**Свойства:** Objects: List`1, DelelectAll: Boolean
**Методы:**
- `Void Accept(ICommandVisitor visitor)`

### `SelectObjectCommand` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract.QueryFilePreview.SelectObjects`)
**Свойства:** Objects: List`1
**Методы:**
- `Void Accept(ICommandVisitor visitor)`

### `TechnologyCadExchangeServiceClient` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract.TechnologyCadExchangeService`)
**Свойства:** Info: ServiceInfo
**Методы:**
- `List`1 GetCadObjects(String documentFileName, String[] searchStrings, ClientCallContext context)` [has Async]
- `CadDimensionDto GetCadDimension(String documentFileName, String searchString, ClientCallContext context)` [has Async]
- `CadMeasureDto GetMeasures(String documentFileName, String[] searchStrings, String measuredParameter, ClientCallContext context)` [has Async]
- `CadObjectPropertyDto GetCadObjectProperty(CadObjectDto objectInfo, CadObjectPropertyDto propertyInfo, ClientCallContext context)` [has Async]
- `List`1 GetCadObjectProperties(CadObjectDto objectInfo, ClientCallContext context)` [has Async]
- `CadRoughnessDto GetCadRoughness(String draftFileName, String searchString, ClientCallContext context)` [has Async]
- `CadStructureDto GetCadStructureElements(String draftFileName, String[] structureElementTypes, ClientCallContext context)` [has Async]
- `CadObjectDto FindCadObject(String documentFileName, String searchString, ClientCallContext context)` [has Async]
- `CadObjectDto GetCadObject(String documentFileName, String searchString, ClientCallContext context)` [has Async]
- `List`1 GetVariables(String draftFileName, ClientCallContext context)` [has Async]
- `Void GetTextTable(CadObjectDto cadObjectInfo, ClientCallContext context)` [has Async]
- `Void SelectObject(String draftFileName, String searchString, ClientCallContext context)` [has Async]
- `Void SelectObjects(IntPtr controlHandle, AssemblyItemDto[] items, ClientCallContext context)` [has Async]
- `Void SubscribeSelectObjects(IntPtr controlHandle, ClientCallContext context)` [has Async]
- `Void UnsubscribeSelectObjects(IntPtr controlHandle, ClientCallContext context)` [has Async]
- `Void SubscribeGettingMeasures(IntPtr controlHandle, ClientCallContext context)` [has Async]
- `Void UnsubscribeGettingMeasures(IntPtr controlHandle, ClientCallContext context)` [has Async]
- `Void ChangeTree(IntPtr controlHandle, String configuration, ClientCallContext context)` [has Async]
- `Void ShowStepText(IntPtr controlHandle, String text, ClientCallContext context)` [has Async]
- `List`1 GetSelectionAllObjects(IntPtr controlHandle, ClientCallContext context)` [has Async]
- `Void SetSelectionFragments(IntPtr controlHandle, Guid[] guids, ClientCallContext context)` [has Async]
- `Void SubscribeGettingCADSelection(IntPtr controlHandle, ClientCallContext context)` [has Async]
- `Void UnsubscribeGettingCADSelection(IntPtr controlHandle, ClientCallContext context)` [has Async]

### `TechnologyCadExchangeServiceServer` (Namespace: `TFlex.DOCs.Model.FilePreview.Contract.TechnologyCadExchangeService`)
**Свойства:** Info: ServiceInfo, SelectionCompleted: NotifyWriter`1, MeasuresCompleted: NotifyWriter`1, CADSelectionChanged: NotifyWriter`1
**Методы:**
- `ValueTask`1 GetCadObjects(String documentFileName, String[] searchStrings, ServerCallContext context)`
- `ValueTask`1 GetCadDimension(String documentFileName, String searchString, ServerCallContext context)`
- `ValueTask`1 GetMeasures(String documentFileName, String[] searchStrings, String measuredParameter, ServerCallContext context)`
- `ValueTask`1 GetCadObjectProperty(CadObjectDto objectInfo, CadObjectPropertyDto propertyInfo, ServerCallContext context)`
- `ValueTask`1 GetCadObjectProperties(CadObjectDto objectInfo, ServerCallContext context)`
- `ValueTask`1 GetCadRoughness(String draftFileName, String searchString, ServerCallContext context)`
- `ValueTask`1 GetCadStructureElements(String draftFileName, String[] structureElementTypes, ServerCallContext context)`
- `ValueTask`1 FindCadObject(String documentFileName, String searchString, ServerCallContext context)`
- `ValueTask`1 GetCadObject(String documentFileName, String searchString, ServerCallContext context)`
- `ValueTask`1 GetVariables(String draftFileName, ServerCallContext context)`
- `ValueTask GetTextTable(CadObjectDto cadObjectInfo, ServerCallContext context)`
- `ValueTask SelectObject(String draftFileName, String searchString, ServerCallContext context)`
- `ValueTask SelectObjects(IntPtr controlHandle, AssemblyItemDto[] items, ServerCallContext context)`
- `ValueTask SubscribeSelectObjects(IntPtr controlHandle, ServerCallContext context)`
- `ValueTask UnsubscribeSelectObjects(IntPtr controlHandle, ServerCallContext context)`
- `ValueTask SubscribeGettingMeasures(IntPtr controlHandle, ServerCallContext context)`
- `ValueTask UnsubscribeGettingMeasures(IntPtr controlHandle, ServerCallContext context)`
- `ValueTask ChangeTree(IntPtr controlHandle, String configuration, ServerCallContext context)`
- `ValueTask ShowStepText(IntPtr controlHandle, String text, ServerCallContext context)`
- `ValueTask`1 GetSelectionAllObjects(IntPtr controlHandle, ServerCallContext context)`
- `ValueTask SetSelectionFragments(IntPtr controlHandle, Guid[] guids, ServerCallContext context)`
- `ValueTask SubscribeGettingCADSelection(IntPtr controlHandle, ServerCallContext context)`
- `ValueTask UnsubscribeGettingCADSelection(IntPtr controlHandle, ServerCallContext context)`
- `Void SetupSelectionCompleted(NotifyWriter`1 notify)`
- `Void SetupMeasuresCompleted(NotifyWriter`1 notify)`
- `Void SetupCADSelectionChanged(NotifyWriter`1 notify)`

### `FileContext` (Namespace: `TFlex.DOCs.Model.FilePreview.DocumentSharing`)
**Свойства:** PathName: String, VirtualAssembly: Guid, Context: Object, ReadOnly: Boolean, LastModificationTime: DateTime, LinkedInstanceGuid: Guid

### `SharedDocument` (Namespace: `TFlex.DOCs.Model.FilePreview.DocumentSharing`)
**Свойства:** OpenedDocuments: IList`1, FileName: String, VirtualAssembly: Guid, ReadOnly: Boolean
**Методы:**
- `String GetFilePreviewControlAssemblyNameByExtension(String extension)`
- `SharedDocument OpenDocument(String fileName, Object context, Boolean readOnly) (+2)`
- `T OpenVirtualDocument(FileContext fileContext)`
- `Void CloseDocument(SharedDocument sharedDocument)`
- `SharedDocument FindDocumentByContext(FileContext context)`
- `CADObjectInfo FindCADObject(String searchString)`
- `Object GetCADObjectPropertyValue(CADObjectInfo cadObjectInfo, CADObjectPropertyInfo cadObjectPropertyInfo)`
- `CADObjectPropertyInfo GetCADObjectPropertyValueWithUnit(CADObjectInfo cadObjectInfo, CADObjectPropertyInfo cadObjectPropertyInfo)`
- `CADVar[] GetVariables()`
- `CADStructureElementInfo[] GetStructureElements(String[] structureElementTypes)`
- `CADStructureElementTypeInfo[] GetStructureElementTypes()`
- `CADDim GetDimension(String searchString)`
- `CADRoughness GetRoughness(String searchString)`
- `CADObject GetCADObject(String searchString)`
- `InteractiveCADVersionInfo GetVersion()`
- `DataTable GetTable(CADObjectInfo cadObjectInfo)`
- `CadMeasure GetMeasure(String[] searchString, String measuredParameter)`

### `SharedDocumentImplementationAttribute` (Namespace: `TFlex.DOCs.Model.FilePreview.DocumentSharing`)
**Свойства:** Extensions: String[]

### `CADServiceProxy` (Namespace: `TFlex.DOCs.Model.FilePreview.Service`)
**Методы:**
- `CadDocumentData Open(String path, Boolean readOnly, Object context)`
- `CadDocumentData OpenVirtual(Guid referenceObjectGuid, Boolean readOnly, Object context)`
- `Void Close(CadDocumentData document, Boolean save)`
- `String Export(CadDocumentData document, ExportContext context)`
- `PageInfo[] GetPagesInfo(CadDocumentData document, Int32[] types)`
- `Void Regenerate(CadDocumentData document, RegenerateOption regenerateOption) (+1)`
- `QualityAnalysisResult[] Analyze3DModelQuality(CadDocumentData document, String reportFilePath, String analysisScript)`
- `RestResponse CallPluginRestService(String pluginName, RestRequest restRequest)`
- `VariableCollection GetVariables(CadDocumentData document, String ownerId)`
- `VariableCollection SaveVariables(CadDocumentData document, String ownerId, VariableCollection variables)`
- `FragmentCollection GetFragments2D(CadDocumentData document)`
- `FragmentCollection GetFragments3D(CadDocumentData document)`
- `ConnectorCollection GetConnectors2D(CadDocumentData document, String ownerId)`
- `ConnectorCollection GetConnectors3D(CadDocumentData document, String ownerId)`
- `LCSCollection GetLCSs(CadDocumentData document, String ownerId)`
- `Fragment3D InsertFragment3D(CadDocumentData targetDocument, String targetLCSName, String fragmentPath, String fragmentLCSName, Boolean byConnector, Boolean embedded)`
- `Void SaveInNomenclature(CadDocumentData document, Boolean recursive, Boolean autoCheckIn, ICollection`1 productStructures) (+1)`
- `ICollection`1 GetProductStructures(CadDocumentData document)`
- `CadDocumentData OpenPart(CadDocumentData document, String ownerId)`
- `CadDocumentData OpenLink(CadDocumentData document, String ownerId)`
- `Nullable`1 GetRealProperty(CadDocumentData document, String ownerId, String propertyName)`
- `String GetTextProperty(CadDocumentData document, String ownerId, String propertyName)`
- `PropertyCollection GetProperties(CadDocumentData document, String ownerId)`
- `Double Compare(CadDocumentData document, String ownerId, String ownerLcs, String filePath, String fileLcs)`
- `String OpenLibrary(String fullPath)`
- `String CloseLibrary(String fullPath)`
- `Void SetIntegrationRule(String integrationRule)`
- `Boolean IsCorrect(CadDocumentData document, String ownerId)`

### `EmptyViewerServiceOwner` (Namespace: `TFlex.DOCs.Model.FilePreview.Service`)
**Свойства:** AsyncModeSupported: Boolean, Control: Object
**Методы:**
- `Void DoShowPreview(Boolean async)`

### `FilePreviewImageManager` (Namespace: `TFlex.DOCs.Model.FilePreview.Service`)
**Методы:**
- `Byte[] GetImageData(String fileName, Int32 pageIndex, ServerConnection connection) (+1)`
- `Int32 GetImagePageCount(String fileName, ServerConnection connection)`

### `FilePreviewType` (Namespace: `TFlex.DOCs.Model.FilePreview.Service`)
**Свойства:** Extension: String, PreviewType: String, PreviewKey: String, AssemblyPath: String, AssemblyName: String, StandAlone: String, ErrorMessage: String, IsAnyCPU: Boolean, IsValidStandAlone: Boolean, NeedChangeStandAlone: Boolean, IsStandalone: Boolean
**Методы:**
- `String GetUsedStandAlone()`
- `Void ChangeUsedStandAlone()`

### `IViewerServiceOwner` (Namespace: `TFlex.DOCs.Model.FilePreview.Service`)
**Свойства:** AsyncModeSupported: Boolean, Control: Object
**Методы:**
- `Void DoShowPreview(Boolean executeAsynchronously)`

### `ServiceCallbackOwnerInfo` (Namespace: `TFlex.DOCs.Model.FilePreview.Service`)
**Свойства:** OwnerId: Int32, Type: FilePreviewType, Owner: IViewerServiceOwner, ConnectionParameters: String, Async: Boolean, IsInstalledProgram: Boolean, Context: String

### `ViewerService` (Namespace: `TFlex.DOCs.Model.FilePreview.Service`)
**Свойства:** OperationTimeout: TimeSpan
**Методы:**
- `Int32 GetNewId()`
- `ViewProviderProxy GetCallback(ServiceCallbackOwnerInfo ownerInfo, Process& process) (+1)`
- `Void CloseProcess(Process process)`
- `CADServiceProxy GetCADService(ServiceCallbackOwnerInfo ownerInfo)`
- `TechnologyCadExchangeServiceClient GetTechnologyCadExchangeService(ServiceCallbackOwnerInfo ownerInfo, String assemblyPath, String assemblyName)`

### `ViewProviderProxy` (Namespace: `TFlex.DOCs.Model.FilePreview.Service`)
**Свойства:** IsAvailable: Boolean
**Методы:**
- `IntPtr ShowFile(Int32 id, ShowFileContext context)`
- `Void CloseFilePreviews(String file)`
- `Void SetControlSize(Int32 id, Int32 width, Int32 height)`
- `Byte[] GenerateReport(String moduleName, String className, Byte[] context)`
- `Byte[] ExchangePluginData(String pluginModuleName, String pluginClassName, Int32 controlId, Byte[] data)`
- `Int32 GetImagePageCount(Int32 id, String filePath)`
- `Byte[] GetPreviewImage(Int32 id, String filePath, Int32 pageIndex)`
- `Boolean IsInstalledPreviewProgram(Int32 id, String extension, Boolean isAnyCPUMode)`
- `String GetCadServiceAddress()`
- `Boolean IsSupportSaving(Int32 id)`
- `Boolean SaveAs(Int32 id, String filePath)`
- `Boolean IsDocumentChandged(Int32 id)`
- `Void SaveChanges(Int32 id)`
- `String GetFilePreviewInformation(Int32 id)`
- `Void Print(Int32 id)`
- `Tuple`2 ExecuteDocumentRequest(Int32 id, FilePreviewRequest request)`
- `String GetMeasureServiceAddress(String assemblyPath, String assemlbyName)`

### `ExtensionUtility` (Namespace: `TFlex.DOCs.Model.FilePreview.Settings`)
**Методы:**
- `Boolean Verify(String ext)`
- `String[] ExtractExtensionsFromString(String exts, Boolean& allCorrect) (+1)`
- `String ConcatExtensionsToString(String[] exts, Boolean& allCorrect) (+1)`
- `Boolean Compare(String ext1, String ext2)`

### `FilePreviewer` (Namespace: `TFlex.DOCs.Model.FilePreview.Settings`)
**Свойства:** Guid: Guid, Name: String, AssemblyPath: String, AssemblyName: String, ErrorMessage: String, Platform: CPUDigitCapacity, Extensions: List`1, RunInCurrentProcess: Boolean
**Методы:**
- `Boolean TryLoadFilePreviewersFromFile(String filePath, FilePreviewer& filePreviewer) (+1)`
- `FilePreviewType GetFilePreviewType()`
- `FilePreviewType GetFilePreviewTypeByAssembly()`

### `FilePreviewers` (Namespace: `TFlex.DOCs.Model.FilePreview.Settings`)
**Свойства:** Previewers: FilePreviewersCollection, Connection: ServerConnection, Interface: String, SharingType: SettingsSharingType, ParameterGroupId: Int32, SupportsViews: Boolean
**Методы:**
- `FilePreviewer GetFilePreviewer(Guid guid) (+1)`
- `FilePreviewType GetFilePreviewType(Guid customFilePreviewerGuid, Guid defaultFilePreviewerGuid, String extension)`

### `WebConnectionSettings` (Namespace: `TFlex.DOCs.Model.FilePreview.Settings`)
**Свойства:** ApplicationDirectory: String, SystemUserName: String, SystemPassword: String
**Методы:**
- `String CreateConnectionData()`
- `ServerConnection CreateConnection(ConnectionParameters connectionParameters)`
- `String Serialize()`
- `WebConnectionSettings Deserialize(String data)`

### `RequestErrorHandler` (Namespace: `TFlex.DOCs.Model.Internal.Connection`)
**Методы:**
- `Boolean Invoke(IRequestResultError error)`
- `IAsyncResult BeginInvoke(IRequestResultError error, AsyncCallback callback, Object object)`
- `Boolean EndInvoke(IAsyncResult result)`

### `DateTimeOperator` (Namespace: `TFlex.DOCs.Model.Internal.Search`)
**Методы:**
- `Nullable`1 GetDateTimeMaskMode(ComparisonOperator comparisonOperator)`
- `Boolean IsSpecialDateTimeOperator(ComparisonOperator comparisonOperator)`

### `FilterParser` (Namespace: `TFlex.DOCs.Model.Internal.Search`)
**Свойства:** Filter: Filter
**Методы:**
- `Filter Parse(String str, Boolean throwOnError)`

### `ISupportSecondOperatorCustomValue` (Namespace: `TFlex.DOCs.Model.Internal.Search`)
**Методы:**
- `String SerializeTermValue(Object value, Nullable`1 mode)`
- `Object ParseTermValue(String value)`
- `Boolean IsValidTermValue(Object value, Term term, Boolean throwOnError)`

### `AvailableProductLicense` (Namespace: `TFlex.DOCs.Model.Licensing`)
**Свойства:** Module: ModuleLicense, TotalCount: Int32, AccessibleCount: Int32

### `LicenseKeyInfo` (Namespace: `TFlex.DOCs.Model.Licensing`)
**Свойства:** HaspID: UInt64, ServerAddress: String, SupportEndDate: DateTime, OperationTestingEndDate: DateTime

### `LicenseManager` (Namespace: `TFlex.DOCs.Model.Licensing`)
**Методы:**
- `LicenseQuota CreateQuota(ModuleLicense license)`

### `LicenseQuota` (Namespace: `TFlex.DOCs.Model.Licensing`)
**Свойства:** IsAdded: Boolean, IsModified: Boolean, IsDeleted: Boolean, License: ModuleLicense, AvailableCount: Int32, User: UserReferenceObject, UserId: Int32, TotalCount: Int32
**Методы:**
- `Void BeginChanges()`
- `Void CancelChanges()`

### `ModuleLicense` (Namespace: `TFlex.DOCs.Model.Licensing`)
**Свойства:** Id: Int32, IsModuleSet: Boolean, Name: String, Functions: IList`1
**Методы:**
- `List`1 GetLicenses()`

### `ModuleSetLicense` (Namespace: `TFlex.DOCs.Model.Licensing`)
**Свойства:** Modules: IList`1, IsModuleSet: Boolean

### `ReloadLicenseKeyResult` (Namespace: `TFlex.DOCs.Model.Licensing`)
**Свойства:** Success: Boolean, Message: String

### `GuidKeyElement` (Namespace: `TFlex.DOCs.Model.Logging`)
**Свойства:** Id: Int32, Guid: Guid

### `GuidNameElement` (Namespace: `TFlex.DOCs.Model.Logging`)
**Свойства:** Name: String, Guid: Guid

### `CodeInfo` (Namespace: `TFlex.DOCs.Model.Macros`)
**Свойства:** Type: CodeType, Language: ProgrammingLanguage, Version: Int32
**Методы:**
- `Boolean TryParse(String value, CodeInfo& info)`

### `CompilationResult` (Namespace: `TFlex.DOCs.Model.Macros`)
**Свойства:** HasErrors: Boolean, HasWarnings: Boolean, IsClean: Boolean, Count: Int32, IsReadOnly: Boolean, Item: CompilationResultElement
**Методы:**
- `Void Add(CompilationResultElement item)`
- `Void Clear()`
- `Boolean Contains(CompilationResultElement item)`
- `Void CopyTo(CompilationResultElement[] array, Int32 arrayIndex)`
- `IEnumerator`1 GetEnumerator()`
- `Boolean Remove(CompilationResultElement item)`
- `Int32 IndexOf(CompilationResultElement item)`
- `Void Insert(Int32 index, CompilationResultElement item)`
- `Void RemoveAt(Int32 index)`

### `CompilationResultElement` (Namespace: `TFlex.DOCs.Model.Macros`)
**Свойства:** Severity: CompilationResultElementSeverity, Line: Int32, Column: Int32, Number: String, CodeSource: String, IsMainSourceCode: Boolean, Text: String, IsWarning: Boolean, IsError: Boolean

### `DynamicMacro` (Namespace: `TFlex.DOCs.Model.Macros`)
**Свойства:** Guid: Guid, Name: String, SourceCodes: List`1, References: List`1, IsCompiled: Boolean, CompilationResult: CompilationResult, UseCurrentDomain: Boolean, UseConvertResult: Boolean, IsDefineServer: Boolean, ExecutionPlace: ExecutionPlace, DebugMode: Boolean, AdditionalFiles: List`1
**Методы:**
- `Void AddReference(String reference)`
- `Void Clear()`
- `CompilationResult Compile()`
- `Object Run(MacroContext context, String entryPoint, Object[] args) (+1)` [has Async]
- `Type GetMacroProviderType()`
- `IEnumerable`1 GetEntryPoints()`

### `ExecutionPlaceExtensions` (Namespace: `TFlex.DOCs.Model.Macros`)
**Методы:**
- `Boolean IsOnServerSide(ExecutionPlace executionPlace)`
- `Boolean IsOnClientSide(ExecutionPlace executionPlace)`
- `String GetDefineOption(ExecutionPlace executionPlace)`

### `FormulaMacro` (Namespace: `TFlex.DOCs.Model.Macros`)
**Свойства:** Formula: String, IsReturnValue: Boolean, IsText: Boolean, IsSimpleText: Boolean, IsFlowchart: Boolean, ReturnType: Type, CodeOffset: Int32
**Методы:**
- `Void Clear()`
- `MacroValidationResults Validate()`
- `Object Calculate(MacroContext context)` [has Async]
- `String GetMacroCode()`
- `Void ReadXml(XmlReader reader)`
- `Void WriteXml(XmlWriter writer)`

### `FormulaMacroEx` (Namespace: `TFlex.DOCs.Model.Macros`)
**Свойства:** CodeOffset: Int32, Parameters: IReadOnlyCollection`1, IsReturnValue: Boolean, ReturnType: Type
**Методы:**
- `T Calculate(MacroContext context, Object[] args)` [has Async]
- `Void Clear()`

### `FormulaMacroExtensions` (Namespace: `TFlex.DOCs.Model.Macros`)
**Методы:**
- `String GetShortCode(String formula, String text, Int32 maxNameLength)`

### `ICodeEditBaseExtension` (Namespace: `TFlex.DOCs.Model.Macros`)
**Свойства:** Name: String

### `ICodeEditExtension` (Namespace: `TFlex.DOCs.Model.Macros`)
**Свойства:** TextCodeEditorExtension: ITextCodeEditExtension, FlowchartCodeEditorExtension: IFlowchartCodeEditExtension, FormulaCreator: IFormulaMacroCreator

### `ICodeEditProvider` (Namespace: `TFlex.DOCs.Model.Macros`)
**Свойства:** FormulaCreator: IFormulaMacroCreator, MacroContextCreator: IMacroContextCreator

### `ICodeEditWithExtensionProvider` (Namespace: `TFlex.DOCs.Model.Macros`)
**Свойства:** Extension: ICodeEditExtension, ContextObject: ReferenceObject

### `IFormulaMacroCreator` (Namespace: `TFlex.DOCs.Model.Macros`)
**Методы:**
- `FormulaMacro CreateFormula(String formula)`

### `IMacroContextCreator` (Namespace: `TFlex.DOCs.Model.Macros`)
**Методы:**
- `MacroContext CreateContext(Object owner)`

### `IMacroContextUserInteraction` (Namespace: `TFlex.DOCs.Model.Macros`)
**Методы:**
- `Void Initialize(MacroContext context)`
- `Void ShowMessage(String caption, String text, Object[] args)`
- `Boolean ShowQuestion(String text)`
- `Nullable`1 ShowQuestionWithCancel(String text)`
- `Boolean ShowObjectPropertyDialog(ObjectAccessor object, String caption, Boolean inNewWindow, Boolean showConfirmationOnCancel) (+2)`
- `ClassObject ShowClassObjectSelectionDialog(ParameterGroup parameterGroup, String caption)`
- `Void OpenReferenceWindowCore(Reference reference, Filter filter, ReferenceObject rootObject, String viewName, CatalogFolder catalogFolder)`
- `Void OpenProjectEditor(IEnumerable`1 projects, String view, String style)`
- `Void OpenWorksUsage(ReferenceObject project, String view, String style)`
- `Void OpenResourcesUsage(ReferenceObject projectElement, ReferenceObject resource, String view, String style)`
- `IInputDialog CreateInputDialog()`
- `IOpenFileDialog CreateOpenFileDialog()`
- `IOpenFolderDialog CreateOpenFolderDialog()`
- `ISaveFileDialog CreateSaveFileDialog()`
- `ISelectObjectDialog CreateSelectObjectDialog(ReferenceInfo referenceInfo) (+1)`
- `ISelectObjectsFromReferencesDialog CreateSelectObjectsFromReferencesDialog()`
- `ISelectListObjectsDialog CreateSelectListObjectsDialog(List`1 listObjects)`
- `ISelectClassObjectsDialog CreateSelectClassObjectsDialog(ParameterGroup parameterGroup)`
- `IWaitingDialog CreateWaitingDialogCore()`
- `ReferenceObject[] GetSelectedObjects(ILayoutItem layoutItem) (+1)`
- `ComplexHierarchyLink[] GetSelectedHierarchyLinks(ILayoutItem layoutItem) (+1)`
- `Void RefreshReferenceWindow()`
- `Void RefreshReferenceObjects(ICollection`1 objects)`
- `IWindow GetCurrentWindow()`
- `IProgressIndicator GetProgressIndicator()`
- `Object CreateRunBusinessProcessContext(ReferenceObject procedure, IEnumerable`1 objects, IEnumerable`1 appendantObjects, ReferenceObject beginState)`
- `Boolean ShowLinearBusinessProcessDialog(ReferenceObject procedure, IEnumerable`1 objects)`
- `Boolean ShowDataExchangeDialog(ReferenceObject masterDataBindingObject, Object dataSettings)`
- `Void ShowDataExchangeWaitDialog(ReferenceObject masterDataBindingObject, Object dataSettings)`
- `Void RunOnUIThread(Action action)`
- `Void OpenFilePreview(IReadOnlyCollection`1 files)`
- `Void OpenWorkingPage(WorkingPage workingPage)`

### `ITextCodeEditExtension` (Namespace: `TFlex.DOCs.Model.Macros`)
**Методы:**
- `List`1 GetToolGroups(Language language)`

### `MacroContext` (Namespace: `TFlex.DOCs.Model.Macros`)
**Свойства:** Connection: ServerConnection, Group: ParameterGroup, Reference: Reference, ConfigurationSettings: ConfigurationSettings, ReferenceObject: ReferenceObject, ReferenceObjectInstance: ReferenceObjectInstance, CancellationToken: CancellationToken, HierarchyLink: ComplexHierarchyLink, ModelChangedArgs: ModelEventArgs, ObjectChangedArgs: ObjectChangedEventArgs, ChangedParameter: Parameter, ChangedLink: LinkInfo, ChangedParameterName: String, ChangedLinkName: String, IsAdministrator: Boolean, RefreshCollectionControls: Boolean, CancellingChanges: Boolean, Filter: Filter, Item: Object
**Методы:**
- `Void RaiseEvent(String eventName)`
- `Object RunMacro(String macroName, String entryPoint, Object[] parameters)`
- `Void GenerateError(String text, Object[] args)`
- `Void WriteTextToLogFile(String text)`
- `Void WriteMessageToLogFile(String message, TypeLogMessageAccessor typeLogMessage)`
- `ReportGenerationContext GenerateReport(Report report, ReportGenerationContext reportContext, OpenReportType openReportType)`
- `Void ShowMessage(String caption, String text, Object[] args)`
- `Boolean ShowQuestion(String text)`
- `Nullable`1 ShowQuestionWithCancel(String text)`
- `IInputDialog CreateInputDialog()`
- `ISelectObjectDialog CreateSelectObjectDialog(String referenceName) (+2)`
- `ISelectObjectsFromReferencesDialog CreateSelectObjectsDialogFromReferences()`
- `ISelectListObjectsDialog CreateSelectListObjectsDialog(List`1 listObjects)`
- `ISelectClassObjectsDialog CreateSelectClassObjectsDialog(String referenceName) (+1)`
- `IWaitingDialog GetWaitingDialog()`
- `IWaitingDialog CreateWaitingDialog()`
- `IOpenFileDialog CreateOpenFileDialog()`
- `IOpenFolderDialog CreateOpenFolderDialog()`
- `ISaveFileDialog CreateSaveFileDialog()`
- `T GetUserDialog(String userDialogTypeName, CreateDialogObjectInstanceDelegate`1 dialogObjectCreator, Boolean emptyDialog) (+1)`
- `Boolean ShowObjectPropertyDialog(ObjectAccessor refObj, String caption, Boolean inNewWindow, Boolean showConfirmationOnCancel) (+3)`
- `ClassObject ShowClassObjectSelectionDialog(String referenceName, String caption)`
- `Void OpenProjectEditor(IEnumerable`1 projects, String view, String style)`
- `Void OpenWorksUsage(ReferenceObject project, String view, String style)`
- `Void OpenResourcesUsage(ReferenceObject projectElement, ReferenceObject resource, String view, String style)`
- `Void OpenReferenceWindow(String referenceName, String filterString, ObjectAccessor rootObject, String viewName, String catalogFolder) (+1)`
- `Void RunOnUIThread(Action action)`
- `T CreateInstance()`
- `ReferenceObject[] GetSelectedObjects(ILayoutItem layoutItem) (+1)`
- `ComplexHierarchyLink[] GetSelectedHierarchyLinks(ILayoutItem layoutItem) (+1)`
- `Void RefreshReferenceWindow()`
- `Void RefreshWorkingPageControl(String[] controlNames)`
- `Void RefreshPropertiesDialogContent()`
- `Void RefreshControls(String[] controlNames) (+1)`
- `Void RefreshReferenceObjects(ICollection`1 referenceObjects)`
- `IWindow GetCurrentWindow()`
- `IProgressIndicator GetProgressIndicator()`
- `Void OpenFilePreview(TList objects)`
- `Void OpenWorkingPage(String workingPage)`
- `T GetObjectFromContext(ReferenceObject referenceObject, ComplexHierarchyLink hierarchyLink) (+1)`
- `ClassObject[] GetClassObjects(String referenceName, String[] classObjects)`
- `Void Register(MarshalByRefObject obj)`
- `Void CloseDialog(Boolean saveChanges, Boolean showConfirmationOnCancel)`

### `MacroDependencyFinder` (Namespace: `TFlex.DOCs.Model.Macros`)
**Методы:**
- `String ReplaceDevExpressVersion(String reference)`

### `MacroProvider` (Namespace: `TFlex.DOCs.Model.Macros`)
**Свойства:** Context: MacroContext, CurrentConfiguration: ConfSettings, CurrentObject: RefObj, CurrentObjectInstance: RefObjInstance, SelectedObjects: RefObjList, CurrentHierarchyLink: HierarchyLink, SelectedHierarchyLinks: HierarchyLinkList, Parameter: ParameterAccessor [RU: Параметр], Class: ClassObjectAccessor [RU: Тип], Global: GlobalParameterAccessor [RU: ГлобальныйПараметр], Nomenclature: NomenclatureReferenceAccessor [RU: Номенклатура], ProgressIndicator: ProgressIndicatorAccessor [RU: ИндикаторВыполнения], BusinessProcesses: BusinessProcessesReferenceAccessor [RU: БизнесПроцессы], DataExchange: DataExchangeAccessor [RU: ОбменДанными], ProjectManagement: ProjectManagementReferenceAccessor [RU: УправлениеПроектами], Chancellery: ChancelleryReferenceAccessor [RU: Канцелярия], Reports: ReportReferenceAccessor [RU: Отчёты], WaitingDialog: WaitingDialogAccessor [RU: ДиалогОжидания], ChangedParameter: String [RU: ИзмененныйПараметр], ChangedLink: String [RU: ИзмененныйПараметр], CurrentUser: UserRefObj, FilterVariable: VariableAccessor [RU: ПеременнаяФильтра], CancelWhenObjectEndEditProperties: Boolean [RU: ОтменаПриЗавершенииРедактированияСвойств], CurrentWindow: WindowObj, ТекущаяКонфигурация: Конфигурация [RU only], ТекущийОбъект: Объект [RU only], ТекущийЭкземплярОбъекта: ЭкземплярОбъекта [RU only], ВыбранныеОбъекты: Объекты [RU only], ТекущееПодключение: Подключение [RU only], ВыбранныеПодключения: Подключения [RU only], ТекущийПользователь: Пользователь [RU only], ТекущееОкно: Окно [RU only]
**Методы:**
- `Void Run()` [RU: Прервать]
- `RefObj FindObject(String referenceName, String parameter, Object value) (+3)` [RU: НайтиОбъекты]
- `Void ChangeStageObjects(RefObjList objects, String stageName, String comment, Boolean ignoreSchemeStage) (+1)` [RU: НайтиОбъекты]
- `RefObj FindLoadedObject(String referenceName, String parameter, Object value) (+3)` [RU: НайтиОбъекты]
- `RefObjList FindObjects(String referenceName, String parameter, Object value) (+3)` [RU: НайтиОбъекты]
- `RefObj FindPrototype(String referenceName, String filter)` [RU: НайтиЗагруженныйОбъект]
- `RefObj CreateObject(String referenceName, String className, ObjectAccessor parentObject) (+4)` [RU: СоздатьОбъект]
- `CopiedObjects CopyObject(ObjectAccessor prototype, ObjectAccessor parentObject, Boolean copyChildren, Boolean copyLinkedPrototypes)` [RU: ИзменитьСтадиюОбъектов]
- `HierarchyLink CreateHierarchyLink(RefObj parentObject, RefObj childObject)` [RU: НайтиЗагруженныйОбъект]
- `SearchTerm SearchTerm(String parameter, String operator, Object value)` [RU: НайтиОбъекты]
- `Void Error(String text, Object[] args)` [RU: НайтиЗагруженныйОбъект]
- `Void Message(String caption, String text, Object[] args) (+1)` [RU: НайтиОбъекты]
- `Boolean Question(String text)` [RU: СоздатьОбъект]
- `Nullable`1 QuestionWithCancel(String text)` [RU: СоздатьОбъект]
- `Void Break()` [RU: Прервать]
- `Void Cancel(String text, Object[] args) (+2)` [RU: Прервать]
- `Void CloseDialog(Boolean saveChanges, Boolean showConfirmationOnCancel)` [RU: НайтиЗагруженныйОбъект]
- `Void RaiseEvent(String eventName)` [RU: СоздатьОбъект]
- `Object RunMacro(String macro, String entryPoint, Object[] parameters)` [RU: НайтиОбъекты]
- `UserDialogObjectAccessor GetUserDialog(String userDialogTypeName, Boolean emptyDialog) (+1)` [RU: СоздатьОбъект]
- `InputDialog CreateInputDialog(String caption)` [RU: СоздатьОбъект]
- `OpenFileDialog CreateOpenFileDialog(String caption)` [RU: СоздатьОбъект]
- `SaveFileDialog CreateSaveFileDialog(String caption)` [RU: СоздатьОбъект]
- `OpenFolderDialog CreateOpenFolderDialog(String caption)` [RU: СоздатьОбъект]
- `SelectObjectsDialog CreateSelectObjectsDialog(String referenceName)` [RU: СоздатьОбъект]
- `SelectObjectsFromReferencesDialog CreateSelectObjectsFromReferencesDialog()` [RU: Прервать]
- `SelectListObjectsDialog CreateSelectListObjectsDialog(RefObjList refObjList)` [RU: СоздатьОбъект]
- `SelectClassObjectsDialog CreateSelectClassObjectsDialog(String referenceName)` [RU: СоздатьОбъект]
- `Void OpenReferenceWindow(String referenceName, String filter, RefObj rootObject, String viewName, String catalogFolder)` [RU: ОткрытьОкноСправочника]
- `Boolean ShowPropertyDialog(RefObj refObj, Boolean showInNewWindow, Boolean showConfirmationOnCancel) (+3)` [RU: НайтиЗагруженныйОбъект]
- `ClassRefObj ShowClassObjectSelectionDialog(String referenceName, String caption)` [RU: НайтиЗагруженныйОбъект]
- `KeyValuePair`2 IconWithText(String icon, String text)` [RU: НайтиЗагруженныйОбъект]
- `KeyValuePair`2 UniversalIconWithText(String icon, String text)` [RU: НайтиЗагруженныйОбъект]
- `IconObj GetIcon(String icon)` [RU: СоздатьОбъект]
- `ValueList GetValueList(String parameterName, String referenceName, String objectList)` [RU: НайтиОбъекты]
- `Void SaveAll(Boolean checkIn, String comment, Boolean executeCallback)` [RU: НайтиОбъекты]
- `Void CancelAll(Boolean undoCheckOut)` [RU: СоздатьОбъект]
- `Void CheckInObjects(RefObjList objects, String comment, Boolean showDialog)` [RU: НайтиОбъекты]
- `Void UndoCheckOutObjects(RefObjList objects)` [RU: СоздатьОбъект]
- `Void RefreshReferenceWindow()` [RU: Прервать]
- `Void RefreshReferenceObjects(RefObjList objects)` [RU: СоздатьОбъект]
- `Void RefreshWorkingPageControl(String[] controls)` [RU: СоздатьОбъект]
- `Void RefreshControls(String[] controls) (+1)` [RU: СоздатьОбъект]
- `Void RefreshPropertiesDialogContent()` [RU: Прервать]
- `Void OpenFilePreview(RefObj file) (+1)` [RU: СоздатьОбъект]
- `Void OpenWorkingPage(String workingPage)` [RU: СоздатьОбъект]
- `MailTaskObj CreateMailTask()` [RU: Прервать]
- `MailMessageObj CreateMailMessage()` [RU: Прервать]
- `Void LogMessage(String message, TypeLogMessageObj typeLogMessage)` [RU: НайтиЗагруженныйОбъект]
- `Void LogText(String text)` [RU: СоздатьОбъект]
- `ClassRefObj[] GetClassObjects(String reference, String[] classObjects)` [RU: НайтиЗагруженныйОбъект]

### `MacroTemplate` (Namespace: `TFlex.DOCs.Model.Macros`)
**Методы:**
- `String Run(String text, MacroContext context, IFormulaMacroCreator formulaCreator) (+1)`

### `MacroValidationResults` (Namespace: `TFlex.DOCs.Model.Macros`)
**Свойства:** HasErrors: Boolean, HasWarnings: Boolean, IsTextValidation: Boolean, CompilationResult: CompilationResult, IsFlowchartValidation: Boolean, FlowchartResults: ValidationResults, ResultException: Exception
**Методы:**
- `Void ThrowIfError()`

### `SourceCode` (Namespace: `TFlex.DOCs.Model.Macros`)
**Свойства:** Code: String, Source: String

### `FormulaAnalyzer` (Namespace: `TFlex.DOCs.Model.Macros.Analyzers`)
**Свойства:** ParameterGroup: ParameterGroup
**Методы:**
- `FormulaAnalyzer Create(ReferenceInfo referenceInfo) (+2)`

### `FormulaExtensions` (Namespace: `TFlex.DOCs.Model.Macros.Analyzers`)
**Методы:**
- `Void AddToLoadSettings(FormulaMacro formulaMacro, LoadSettings loadSettings)`

### `IFlowchartCodeEditExtension` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart`)
**Свойства:** DefaultSupportedTypes: Type[], AccessorTypes: AccessorDefaultType[], ToolboxController: IToolboxController, ToolIcons: Dictionary`2, Namespaces: NamespaceInfo[], ReplaceArguments: Dictionary`2, ContextVariables: ContextVariableInfo[], ObsoleteActivityTypes: Type[], ObsoleteContextVariables: ContextVariableInfo[]

### `ISignatureType` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart`)
**Свойства:** SignatureType: InArgument`1

### `IToolboxActivityInfo` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart`)
**Свойства:** Name: String, ToolboxInterfaceName: String, IsSystem: Boolean, Categories: List`1, Item: IToolboxCategoryActivityInfo
**Методы:**
- `List`1 GetItems()`

### `IToolboxCategoryActivityInfo` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart`)
**Свойства:** Name: String, Items: List`1, IsExpandedByDefault: Boolean, AllowSort: Boolean
**Методы:**
- `IToolboxCategoryActivityInfo Copy()`

### `IToolboxController` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart`)
**Свойства:** Toolboxes: List`1, ActivityTypes: List`1, BaseActivityTypes: List`1, ActivityItems: List`1, Item: IToolboxActivityInfo, IsRegisterIcons: Boolean
**Методы:**
- `Void RegisterMetadata(Object builder)`
- `Void RegisterIcons()`

### `IToolboxItemActivityInfo` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart`)
**Свойства:** Name: String, Category: String, BitmapName: String, IsStandardIcon: Boolean, Description: String, TypeDescription: String, ActivityType: Type, RecalcPropertyNames: String[], ReturnType: Type

### `AddRangeToCollectionActivity`1` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Activities`)
**Свойства:** Values: InArgument`1, Items: InArgument`1, IsInsert: InArgument`1, Index: InArgument`1

### `AddToCollectionActivity`1` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Activities`)
**Свойства:** Values: InArgument`1, Item: InArgument`1, IsInsert: InArgument`1, Index: InArgument`1

### `ClearCollectionActivity`1` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Activities`)
**Свойства:** Values: InArgument`1

### `CountOfItemsInCollectionActivity`1` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Activities`)
**Свойства:** Values: InArgument`1

### `ExistsInCollectionActivity`1` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Activities`)
**Свойства:** Values: InArgument`1, Item: InArgument`1

### `FindInCollectionActivity`1` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Activities`)
**Свойства:** Values: InArgument`1, IsOne: InArgument`1, Body: ActivityFunc`2

### `ForActivity`1` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Activities`)
**Свойства:** From: InArgument`1, To: InArgument`1, Step: InArgument`1, Body: ActivityAction`1

### `ForEachActivity`1` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Activities`)
**Свойства:** Values: InArgument`1, Body: ActivityAction`1

### `HandlerActivity`2` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Activities`)
**Свойства:** Body: ActivityAction`2

### `RemoveFromCollectionActivity`1` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Activities`)
**Свойства:** Values: InArgument`1, Item: InArgument`1, IsIndex: InArgument`1, Index: InArgument`1

### `SendMailItemActivityBase` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Activities`)
**Свойства:** To: Collection`1, Subject: Activity`1, Text: Activity`1, Template: InArgument`1, Attachments: Collection`1, CopyToExternalEMail: InArgument`1

### `AssignObjectPropertyActivityBase` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Activities.Base`)
**Свойства:** Object: InArgument`1, Action: InArgument`1, Value: Activity`1

### `ChangeLinkActivityBase` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Activities.Base`)
**Свойства:** Object: InArgument`1, LinkedObject: InArgument`1, Reference: InArgument`1, Link: InArgument`1

### `CommentCodeActivity`1` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Activities.Base`)
**Свойства:** Comment: String

### `CommentNativeActivity`1` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Activities.Base`)
**Свойства:** Comment: String

### `ObjectPropertyActivityBase` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Activities.Base`)
**Свойства:** Object: InArgument`1, Action: InArgument`1, Link: InArgument`1

### `SequenceActivityBase` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Activities.Base`)
**Свойства:** Variables: Collection`1, Activities: Collection`1

### `SequenceResultActivityBase` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Activities.Base`)
**Свойства:** Activities: Collection`1

### `SequenceResultActivityReturnBase` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Activities.Base`)
**Свойства:** Activities: Collection`1

### `WhileActivityBase` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Activities.Base`)
**Свойства:** Condition: Activity`1, Body: Activity

### `EnumManager` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Activities.Elements`)
**Методы:**
- `Void CacheMetadata(CodeActivityMetadata& metadata, InArgument`1 argument, String errorText)`

### `ObjectPropertyManager` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Activities.Elements`)
**Методы:**
- `Boolean GetPropertyValue(String actionString, ObjectAccessor referenceObject, Object& result, String linkName)`
- `Boolean SetPropertyValue(String actionString, IEnumerable`1 objects, Object value)`

### `ButtonFieldInputDialog` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Activities.InputDialog`)
**Свойства:** TypeName: String, Code: InArgument`1, Width: InArgument`1

### `CaptionFieldInputDialog` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Activities.InputDialog`)
**Свойства:** TypeName: String, Caption: InArgument`1

### `ChangePictureFieldInputDialog` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Activities.InputDialog`)
**Свойства:** TypeName: String, Index: InArgument`1, Object: InArgument`1, ImagePath: InArgument`1

### `CommentFieldInputDialog` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Activities.InputDialog`)
**Свойства:** TypeName: String, Comment: InArgument`1

### `DateFieldInputDialog` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Activities.InputDialog`)
**Свойства:** TypeName: String

### `DateTimeBaseFieldInputDialog` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Activities.InputDialog`)
**Свойства:** DefaultValue: InArgument`1

### `DateTimeFieldInputDialog` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Activities.InputDialog`)
**Свойства:** TypeName: String, Mask: InArgument`1

### `DialogSizeFieldInputDialog` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Activities.InputDialog`)
**Свойства:** TypeName: String, Width: InArgument`1, Height: InArgument`1

### `DoubleFieldInputDialog` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Activities.InputDialog`)
**Свойства:** TypeName: String, DefaultValue: InArgument`1, DecimalPlaces: InArgument`1

### `ElementEnabledFieldInputDialog` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Activities.InputDialog`)
**Свойства:** TypeName: String, Value: InArgument`1

### `ElementVisibilityFieldInputDialog` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Activities.InputDialog`)
**Свойства:** TypeName: String, Value: InArgument`1

### `FieldInputDialog` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Activities.InputDialog`)
**Свойства:** TypeName: String, InputDialog: InputDialog

### `FlagFieldInputDialog` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Activities.InputDialog`)
**Свойства:** TypeName: String, DefaultValue: InArgument`1

### `GroupFieldInputDialog` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Activities.InputDialog`)
**Свойства:** TypeName: String

### `IntegerFieldInputDialog` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Activities.InputDialog`)
**Свойства:** TypeName: String, DefaultValue: InArgument`1

### `PictureFieldInputDialog` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Activities.InputDialog`)
**Свойства:** TypeName: String, Object: InArgument`1, ImagePath: InArgument`1, LineCount: InArgument`1

### `SelectFromListFieldInputDialog` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Activities.InputDialog`)
**Свойства:** TypeName: String, DefaultValue: InArgument`1, Values: InArgument`1

### `SelectFromReferenceFieldInputDialog` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Activities.InputDialog`)
**Свойства:** TypeName: String, Reference: InArgument`1, Parameter: InArgument`1, DefaultValue: InArgument`1, Filter: InArgument`1, Code: InArgument`1

### `StringFieldInputDialog` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Activities.InputDialog`)
**Свойства:** TypeName: String, DefaultValue: InArgument`1, IsMultiline: Boolean, Mask: InArgument`1

### `TextFieldInputDialog` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Activities.InputDialog`)
**Свойства:** TypeName: String, Text: InArgument`1, LineCount: InArgument`1

### `TimeFieldInputDialog` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Activities.InputDialog`)
**Свойства:** TypeName: String

### `ValueChangedFieldInputDialog` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Activities.InputDialog`)
**Свойства:** TypeName: String, Code: InArgument`1

### `ValueFieldInputDialog` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Activities.InputDialog`)
**Свойства:** FieldName: InArgument`1, IsRequired: Boolean

### `AndAlsoOperator` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Activities.Search`)
**Свойства:** LogicalType: LogicalActivityOperatorType
**Методы:**
- `Nullable`1 CompareFirst(Boolean firstOperand)`
- `Boolean CompareSecond(Boolean secondOperand)`

### `OrElseOperator` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Activities.Search`)
**Свойства:** LogicalType: LogicalActivityOperatorType
**Методы:**
- `Nullable`1 CompareFirst(Boolean firstOperand)`
- `Boolean CompareSecond(Boolean secondOperand)`

### `ContextVariableInfo` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Classes`)
**Свойства:** ActivityType: Type
**Методы:**
- `Object GetValue(ActivityContext context) (+1)`
- `Object GetAccessorValue(ActivityContext context) (+1)`

### `ContextVariables` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Classes`)
**Свойства:** Variables: ContextVariableInfo[]

### `NamespaceInfo` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Classes`)
**Свойства:** Namespace: String, Assembly: String

### `ParameterGroupInfo` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Classes`)
**Свойства:** Reference: ReferenceInfo, Class: ClassObject, IsReferenceInfo: Boolean, IsClass: Boolean, IsHierarchy: Boolean, Parameters: ParameterInfoCollection
**Методы:**
- `ParameterGroupInfo LoadReferenceInfo(ReferenceInfo referenceInfo)`
- `ParameterGroupInfo LoadClass(ClassObject classObject)`
- `ParameterGroupInfo LoadHierarchy(ReferenceInfo referenceInfo)`
- `ParameterInfo GetParameter(Guid parameterGuid)`

### `VariableInfo` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Classes`)
**Свойства:** DisplayName: String, VariableName: String, Type: Type, Value: String, DefaultType: DefaultSupportedType, Description: String, IsEmbed: Boolean, IsContext: Boolean, ContextVariableInfo: ContextVariableInfo, IsPath: Boolean, Path: StructurePath, Image: Object, ImageResource: String, IsImageLoad: Boolean, GroupName: String, Reference: Guid, Classes: Guid[], IsReferenceObject: Boolean, IsHierarchyLink: Boolean, IsMultiple: Boolean, VariableDefaultType: DefaultSupportedType
**Методы:**
- `VariableInfo Create(String variableName, Type variableType) (+7)`
- `Boolean IsEmbedVariable(String variableName)`
- `Boolean IsContextVariable(String variableName)`
- `Boolean IsPathVariable(String variableName)`
- `Object GetValue(ActivityContext context) (+1)`
- `Object GetAccessorValue(ActivityContext context) (+1)`

### `ActivityContextExtensions` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Extensions`)
**Методы:**
- `MacroContext GetMacroContext(ActivityContext context)`
- `MacroProvider GetMacroProvider(ActivityContext context)`
- `FormulaMacro GetFormulaMacro(ActivityContext context)`
- `IFormulaMacroCreator GetFormulaCreator(ActivityContext context)`
- `ServerConnection GetConnection(ActivityContext context)`
- `Void Return(NativeActivityContext context, Object result)`
- `Object GetVariableValue(ActivityContext context, String variableName)`
- `Boolean TryVariableValue(ActivityContext context, String variableName, Object& value)`
- `Void SetVariableValue(ActivityContext context, String variableName, Object value)`
- `FlowchartMacroContext GetFlowchartMacroContext(ActivityContext context)`
- `T GetProvider(ActivityContext context, Func`1 createProviderAction)`

### `ActivityMetadataExtensions` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Extensions`)
**Методы:**
- `Void AddValidationWarning(ActivityMetadata metadata, String message) (+2)`
- `Void AddValidationArgumentWarning(ActivityMetadata metadata, String argument) (+2)`
- `Void AddValidationArgumentError(ActivityMetadata metadata, String argument) (+2)`
- `Void VerifyArgument(ActivityMetadata metadata, InArgument`1 argument, String name, Boolean isWarning) (+2)`
- `Void VerifyActivity(ActivityMetadata metadata, Activity activity, String name, Boolean isWarning) (+2)`
- `Void AddValidationMetadataArgumentError(ActivityMetadata metadata, String argument) (+2)`
- `Void AddValidationFunctionError(ActivityMetadata metadata) (+2)`
- `Void VerifyFunction(ActivityMetadata metadata, Activity activity) (+2)`
- `VariableInfo VerifyVariable(ActivityMetadata metadata, InArgument`1 argument, String variableCaption, Func`2 verifyTypeFunc, Boolean supportMultiple) (+2)`
- `VariableInfo VerifyAccessorVariable(ActivityMetadata metadata, InArgument`1 argument, DefaultSupportedType defaultType, String variableCaption, Boolean supportMultiple) (+5)`
- `Void SetReturnChildrenCollection(NativeActivityMetadata metadata, Collection`1 children)`

### `ArgumentExtensions` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Extensions`)
**Методы:**
- `T ToValue(InArgument`1 argument)`

### `ConstraintExtensions` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Extensions`)
**Методы:**
- `Void AddConstraint(Collection`1 constraints, DelegateInArgument`1 activityDelegate, T activity, Boolean isWarning, String messageText, Expression`1 expression, Boolean argumentMessage) (+2)`
- `Void AddParameter(Collection`1 constraints, T activity)`
- `Void AddAction(Collection`1 constraints, T activity)`
- `Void AddReference(Collection`1 constraints, T activity)`
- `Void AddReferenceObject(Collection`1 constraints, T activity)`
- `Void AddPrototype(Collection`1 constraints, T activity)`
- `Void AddClassObject(Collection`1 constraints, T activity)`
- `Void AddLink(Collection`1 constraints, T activity)`
- `Void AddObjectsList(Collection`1 constraints, T activity)`
- `Void AddFilter(Collection`1 constraints, T activity)`
- `Void AddSignatureType(Collection`1 constraints, T activity)`
- `Void AddOperator(Collection`1 constraints, T activity)`
- `Boolean AssertExpression(InArgument`1 argument)`

### `LocationReferenceEnvironmentExtensions` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Extensions`)
**Методы:**
- `IEnumerable`1 GetAvailableLocationReferences(LocationReferenceEnvironment environment)`

### `ParameterInfoExtensions` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Extensions`)
**Методы:**
- `String Serialize(ParameterInfo parameterInfo, ParameterGroupInfo groupInfo)`
- `String DeSerilizeParameterGuid(String text)`
- `String DeSerilizeClassGuid(String text)`
- `ParameterInfo DeSerialize(ServerConnection connection, String text, ParameterGroupInfo& groupInfo) (+1)`

### `ActivityDataHelper` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Helpers`)
**Методы:**
- `ParameterObjectValue GetParameterValue(ActivityContext context, InArgument`1 variable, InArgument`1 parameter, Boolean autoChanging, Boolean throwOnError)`
- `ParameterObjectValue[] GetParameterValues(ActivityContext context, InArgument`1 variable, InArgument`1 parameter, Boolean autoChanging, Boolean throwOnError)`
- `Object GetVariableValue(ActivityContext context, InArgument`1 variable, Boolean throwOnError)`
- `ObjectAccessor GetObjectAccessorValue(ActivityContext context, InArgument`1 variable, InArgument`1 reference, Boolean throwOnError) (+1)`
- `ObjectAccessor[] GetObjectAccessorValues(ActivityContext context, InArgument`1 variable, InArgument`1 reference, Boolean throwOnError) (+1)`
- `ReferenceObject GetReferenceObjectValue(ActivityContext context, InArgument`1 variable, InArgument`1 reference, Boolean throwOnError) (+1)`
- `ReferenceObject[] GetReferenceObjectValues(ActivityContext context, InArgument`1 variable, InArgument`1 reference, Boolean throwOnError) (+1)`
- `HierarchyLinkAccessor GetHierarchyLinkAccessorValue(ActivityContext context, InArgument`1 variable, Boolean throwOnError)`
- `ComplexHierarchyLink GetComplexHierarchyLinkValue(ActivityContext context, InArgument`1 variable, Boolean throwOnError)`
- `ComparisonOperator GetOperator(String value)`

### `ClassObjectHelper` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Helpers`)
**Методы:**
- `ClassObject FindClass(ServerConnection connection, String referenceName, String className) (+1)`

### `ExpressionHelper` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Helpers`)
**Методы:**
- `ReferenceInfo FindReference(ActivityContext context, InArgument`1 reference)`
- `Boolean IsReferenceExpression(ServerConnection connection, String value, ReferenceInfo& referenceInfo) (+1)`
- `T GetEnumValue(ActivityContext context, InArgument`1 argument, T defaultValue)`
- `Boolean TryParseLink(ServerConnection connection, String referenceName, String linkText, ParameterGroup& linkGroup) (+1)`
- `Boolean IsObjectsListExpression(ServerConnection connection, String value, String referenceName)`
- `ClassObject FindClass(ActivityContext context, InArgument`1 classObject, ReferenceInfo referenceInfo) (+1)`
- `Boolean TryParseClassObject(ServerConnection connection, String referenceName, String classObjectText, ClassObject& classObject) (+1)`
- `Boolean IsClassObjectExpression(ServerConnection connection, String value, String referenceName, ReferenceInfo& referenceInfo, ClassObject& classObject) (+2)`
- `Boolean IsParameterExpression(String value, ClassObject classObject, ParameterInfo& parameterInfo) (+1)`
- `ParameterInfo FindParameter(ActivityContext context, InArgument`1 parameter, ClassObject classObject)`
- `Boolean TryParsePrototype(String value, ReferenceInfo referenceInfo, ReferenceObject& prototypeObject)`
- `ReferenceObject ParsePrototype(String value, ReferenceInfo referenceInfo)`
- `Boolean TryParseClassObjectWithPrototype(ServerConnection connection, String value, String referenceName, ClassObject& classObject, ReferenceObject& prototypeObject) (+1)`
- `Boolean TryParseReferenceObject(ServerConnection connection, String referenceName, String referenceObjectName, ReferenceObject& referenceObject) (+1)`
- `Boolean TryParseSpecialReferenceObject(Reference reference, String referenceObjectName, T& referenceObject)`
- `ReferenceObject FindReferenceObject(ActivityContext context, ReferenceInfo referenceInfo, InArgument`1 referenceObject, Boolean prototypeMode)`
- `Boolean IsReferenceObjectExpression(ServerConnection connection, String value, String reference, ReferenceObject& referenceObject, Boolean prototypeMode) (+1)`
- `Boolean IsSpecialReferenceObjectExpression(ServerConnection connection, String value, String reference, T& referenceObject, Boolean prototypeMode)`
- `SignatureType FindSignatureType(ActivityContext context, InArgument`1 signatureType)`
- `AccessGroup FindAccessGroup(ActivityContext context, InArgument`1 accessGroup)`
- `Filter ParseFilter(String text, ReferenceInfo referenceInfo, MacroContext macroContext, Boolean validate)`
- `Filter GetFilter(ActivityContext context, ReferenceInfo referenceInfo, InArgument`1 filter, Boolean validate) (+1)`
- `Boolean IsExpression(String value)`
- `Boolean IsExpressionOrNull(String value)`
- `Boolean IsGuidValue(String value)`
- `Boolean TryParseInArgumentValue(InArgument`1 argument, T& value)`
- `String GetExpressionValue(String expression)`
- `String MakeExpressionValue(String value)`

### `ParameterGroupHelper` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Helpers`)
**Методы:**
- `ReferenceInfo GetReference(ParameterGroup group)`
- `Boolean TryParse(String path, ServerConnection connection, ParameterGroup& group)`
- `String CombineListObjectPath(String reference, String listObject)`
- `ParameterGroup FindRelation(ServerConnection connection, String referenceName, String relationName) (+2)`
- `String CombineRelativePath(ParameterGroup group)`

### `ParameterHelper` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Helpers`)
**Методы:**
- `ParameterInfo FindParameter(ClassObject classObject, String parameterName)`

### `ReferenceCatalogHelper` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Helpers`)
**Методы:**
- `String Serialize(Object value)`
- `Object DeSerialize(ServerConnection connection, String value)`
- `String SerClassObject(String reference, String classObject)`
- `String SerGroup(String reference, String group)`
- `String GetSerReference(String value)`
- `String GetSerObject(String value)`

### `ReferenceHelper` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Helpers`)
**Методы:**
- `ReferenceInfo Find(ServerConnection connection, String referenceName) (+2)`
- `Reference Create(ServerConnection connection, String referenceName, Boolean prototypeMode, Boolean throwOnError) (+3)`

### `ActivityManager` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Managers`)
**Методы:**
- `T Load(String code, Boolean ignoreDynamicRootActivity, Boolean tryConvertCode) (+1)`
- `String Serialize(Activity activity)`

### `ActivitySettingsManager` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Managers`)
**Методы:**
- `Boolean AddNamespace(Activity activity, NamespaceInfo namespaceInfo)`
- `Void AddNamespaces(Activity activity, IEnumerable`1 namespaces)`

### `CodeVersionConverter` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Managers`)
**Методы:**
- `Boolean IsNeedConvert(String code)`
- `String TryConvert(String code)`
- `String Convert(String code)`

### `EmbedVariableManager` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Managers`)
**Свойства:** Instance: EmbedVariableManager, Variables: ReadOnlyCollection`1
**Методы:**
- `Boolean AddVariable(ContextVariableInfo variable)`
- `Void AddVariables(IEnumerable`1 variables)`
- `Boolean RemoveVariable(ContextVariableInfo variable)`
- `Void RemoveVariables(IEnumerable`1 variables)`
- `ContextVariableInfo GetVariableByName(String variableName)`
- `ContextVariableInfo GetVariable(String variableValue)`
- `List`1 GetVariables(Type type, Boolean canInherit) (+1)`
- `Void Clear()`

### `ExpressionManager` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Managers`)
**Методы:**
- `Boolean TryNeedUpdateArgumentType(Argument value, Type& argumentType)`
- `Argument CreateArgument(Type expressionType, String expressionText, ArgumentDirection direction) (+1)`
- `ActivityWithResult CreateExpression(Type expressionType, String expressionText, ArgumentDirection direction)`
- `String GetExpressionText(ActivityWithResult activity) (+1)`
- `Type GetValueType(Argument argument)`

### `VariableManager` (Namespace: `TFlex.DOCs.Model.Macros.Flowchart.Managers`)
**Методы:**
- `Void SetFilterVariables(Filter filter, ActivityContext context)`
- `Boolean IsSupportVariableType(Type variableType)`
- `Boolean IsEqualTypes(Type filterType, Type variableType)`
- `List`1 GetAvailableVariables(ActivityContext context)`

### `M` (Namespace: `TFlex.DOCs.Model.Macros.MathExtensions`)
**Методы:**
- `Double Sin(Double degrees)`
- `Double Asin(Double d)`
- `Double Cos(Double degrees)`
- `Double Acos(Double d)`
- `Double Tg(Double degrees)`
- `Double Atg(Double d)`
- `Double Ctg(Double degrees)`
- `Double Actg(Double d)`

### `MathExtensions` (Namespace: `TFlex.DOCs.Model.Macros.MathExtensions`)
**Методы:**
- `Double DegreesToRadians(Double degrees) (+1)`
- `Double RadiansToDegrees(Double radians)`

### `AccessAccessor` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** IsInherit: Boolean [RU: Унаследован]
**Методы:**
- `Void SetForAllUsers(String accessName, AccessDirectionObj accessDirection)` [RU: НазначитьВсемПользователям]
- `Void Set(RefObj user, String accessName, AccessDirectionObj accessDirection) (+1)` [RU: Назначить]
- `Void Delete(RefObj user, String accessName) (+1)` [RU: НазначитьВсемПользователям]
- `Void DeleteAll()` [RU: УдалитьВсе]

### `AccessDirectionAccessor` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** Default: AccessDirectionObj, Children: AccessDirectionObj, Entity: AccessDirectionObj, ПоУмолчанию: НаправлениеДоступа [RU only], ДочерниеОбъекты: НаправлениеДоступа [RU only], Объект: НаправлениеДоступа [RU only]
**Методы:**
- `Object GetRealValue()`

### `BusinessProcessesReferenceAccessor` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Методы:**
- `Boolean Run(String name, RefObj refObj, Dictionary`2 variables, Boolean showDialog, String variablesTemplate, String begin, RefObjList appendantObjects) (+2)` [RU: Запустить]
- `Boolean Edit(String name, Dictionary`2 variables)` [RU: Изменить]
- `Boolean RunLinearProcess(String prototype, RefObj refObj, Boolean showDialog) (+5)` [RU: ЗапуститьЛинейныйПроцесс]
- `Void Complete(String process, String state, String solution, RefObjList objects, String comment)` [RU: Завершить]

### `ChancelleryReferenceAccessor` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Методы:**
- `ResolutionTaskObj CreateResolutionTask()` [RU: СоздатьЗадание]

### `ClassObjectAccessor` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** Name: String [RU: Имя], Guid: Guid [RU: УникальныйИдентификатор], Parameters: List`1 [RU: Параметры]
**Методы:**
- `Boolean IsInherit(String className) (+1)` [RU: ПорожденОт]
- `Boolean ContainsLink(String link)` [RU: ПорожденОт]
- `Int32 CompareTo(Object obj)` [RU: ПорожденОт]

### `ConfigurationSettingsAccessor` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** ApplyProduct: Boolean [RU: ПрименитьИзделие], Product: RefObj, ApplyDesignContext: Boolean [RU: ПрименитьИзделие], DesignContext: RefObj, ApplyDate: Boolean [RU: ПрименитьИзделие], Date: Nullable`1 [RU: Дата], ActiveStructure: RefObj, VisibleStructures: RefObjList, EditableStructures: RefObjList, ApplyCategories: Boolean [RU: ПрименитьИзделие], ShowAllCategories: Boolean [RU: ПрименитьИзделие], ShowEmptyCategories: Boolean [RU: ПрименитьИзделие], Categories: RefObjList, ApplySelectRevisionsTerms: Boolean [RU: ПрименитьИзделие], SelectRevisionsTermsObject: RefObj, SelectRevisionsTerms: String [RU: УсловияВыбораРевизий], Изделие: Объект [RU only], КонтекстПроектирования: Объект [RU only], АктивныйТипСтруктуры: Объект [RU only], ОтображаемыеСтруктуры: Объекты [RU only], РедактируемыеСтруктуры: Объекты [RU only], Категории: Объекты [RU only], ОбъектУсловийВыбораРевизий: Объект [RU only]
**Методы:**
- `Void AddStructure(RefObj structure, Boolean editable)` [RU: ДобавитьСтруктуру]
- `Void RemoveStructure(RefObj structure)` [RU: УдалитьСтруктуру]
- `Void AddCategory(RefObj category)` [RU: УдалитьСтруктуру]
- `Void RemoveCategory(RefObj category)` [RU: УдалитьСтруктуру]

### `CopySetAccessor` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** Count: Int32, Changing: Boolean [RU: Редактируется], Owner: RefObj, Владелец: Объект [RU only]
**Методы:**
- `Void CopyTo(ObjectAccessor[] array, Int32 arrayIndex)`
- `IEnumerator`1 GetEnumerator()` [RU: Сохранить]
- `RefObj GetCopy(ObjectAccessor prototype)` [RU: ПолучитьКопию]
- `Boolean Save()` [RU: Сохранить]
- `Void CancelChanges()` [RU: Сохранить]
- `Boolean Contains(ObjectAccessor refObj)` [RU: ПолучитьКопию]

### `CurrentStateChangeEventTypeAccessor` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** Add: CurrentStateChangeEventTypeObj, Move: CurrentStateChangeEventTypeObj, Delete: CurrentStateChangeEventTypeObj, Перенос: ТипИзмененияТекущегоСостояния [RU only], Удаление: ТипИзмененияТекущегоСостояния [RU only], Добавление: ТипИзмененияТекущегоСостояния [RU only]
**Методы:**
- `Object GetRealValue()`

### `DataExchangeAccessor` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Методы:**
- `DataExchangeResultsAccessor Import(String ruleName, String reference, String filter, RefObj rootObject, Boolean onlyChildrenObjects, String filePath, Boolean showDialog, Dictionary`2 extendedProperties, Boolean advancedLogging, RefObjList attachToObjects, Nullable`1 readPacketSize, Nullable`1 writePacketSize, String referenceRule, Int32 count, Int32 offset, IReadOnlyCollection`1 sortFields, Boolean deleting, Boolean usePackage, Boolean allowProcessingList, Boolean showWaitingDialog, String connectionString, String intermediateConnectionString, Boolean clearTransformTables)` [RU: Импортировать]
- `DataExchangeResultsAccessor ImportFromCatalog(String ruleName, String catalogFolder, String reference, String filePath, Boolean showDialog, Dictionary`2 extendedProperties, Boolean advancedLogging, RefObjList attachToObjects, Nullable`1 readPacketSize, Nullable`1 writePacketSize, String referenceRule, Int32 count, Int32 offset, IReadOnlyCollection`1 sortFields, Boolean deleting, Boolean usePackage, Boolean allowProcessingList, Boolean showWaitingDialog, String connectionString)` [RU: ИмпортироватьИзКаталога]
- `DataExchangeResultsAccessor ImportObject(String ruleName, Object refObject, Boolean showDialog, Dictionary`2 extendedProperties, Boolean advancedLogging, RefObjList attachToObjects, Nullable`1 readPacketSize, Nullable`1 writePacketSize, String referenceRule, Boolean deleting, Boolean allowProcessingList, Boolean reloadObjects, Boolean showWaitingDialog, String connectionString) (+2)` [RU: ИмпортироватьОбъект]
- `DataExchangeResultsAccessor ImportObjects(String ruleName, IEnumerable`1 refObjects, Boolean showDialog, Dictionary`2 extendedProperties, Boolean advancedLogging, RefObjList attachToObjects, Nullable`1 readPacketSize, Nullable`1 writePacketSize, String referenceRule, Boolean deleting, Boolean allowProcessingList, Boolean reloadObjects, Boolean showWaitingDialog, String connectionString) (+2)` [RU: ИмпортироватьОбъект]
- `DataExchangeResultsAccessor Export(String ruleName, String reference, String filter, RefObj rootObject, Boolean onlyChildrenObjects, String filePath, Boolean showDialog, Dictionary`2 extendedProperties, Boolean advancedLogging, RefObjList attachToObjects, Nullable`1 readPacketSize, Nullable`1 writePacketSize, String referenceRule, Int32 count, Int32 offset, IReadOnlyCollection`1 sortFields, Boolean deleting, String server, Boolean usePackage, Boolean allowProcessingList, Boolean showWaitingDialog, Dictionary`2 transferData, String connectionString, Boolean onlyStructure)` [RU: Экспортировать]
- `DataExchangeResultsAccessor ExportFromCatalog(String ruleName, String catalogFolder, String reference, String filePath, Boolean showDialog, Dictionary`2 extendedProperties, Boolean advancedLogging, RefObjList attachToObjects, Nullable`1 readPacketSize, Nullable`1 writePacketSize, String referenceRule, Int32 count, Int32 offset, IReadOnlyCollection`1 sortFields, Boolean deleting, String server, Boolean usePackage, Boolean allowProcessingList, Boolean showWaitingDialog, Dictionary`2 transferData, String connectionString, Boolean onlyStructure)` [RU: ЭкспортироватьИзКаталога]
- `DataExchangeResultsAccessor ExportObject(String ruleName, Object refObject, Boolean showDialog, Dictionary`2 extendedProperties, Boolean advancedLogging, String filePath, Nullable`1 readPacketSize, Nullable`1 writePacketSize, String referenceRule, Boolean deleting, String server, Boolean usePackage, Boolean allowProcessingList, Boolean reloadObjects, Boolean showWaitingDialog, Dictionary`2 transferData, String connectionString, Boolean onlyStructure) (+2)` [RU: ЭкспортироватьОбъект]
- `DataExchangeResultsAccessor ExportObjects(String ruleName, IEnumerable`1 refObjects, Boolean showDialog, Dictionary`2 extendedProperties, Boolean advancedLogging, String filePath, Nullable`1 readPacketSize, Nullable`1 writePacketSize, String referenceRule, Boolean deleting, String server, Boolean usePackage, Boolean allowProcessingList, Boolean reloadObjects, Boolean showWaitingDialog, Dictionary`2 transferData, String connectionString, Boolean onlyStructure) (+2)` [RU: ЭкспортироватьОбъект]
- `DataTransferResultsAccessor RunDataTransfer(RefObj dataTransferObject, Dictionary`2 extendedProperties, Boolean allowProcessingList) (+1)` [RU: ЗапуститьПередачуДанных]
- `MasterServerMacroProvider ConnectToServer(String server)` [RU: ПодключитьсяКСерверу]

### `DataExchangeResultsAccessor` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** Cancelled: Boolean [RU: Отменён], HasErrors: Boolean [RU: Отменён], HasWarnings: Boolean [RU: Отменён], ErrorMessage: String [RU: ТекстОшибки], StartTime: DateTime [RU: ВремяНачала], EndTime: DateTime [RU: ВремяНачала], Duration: TimeSpan [RU: Длительность], FilePath: String [RU: ТекстОшибки], File: RefObj, Stream: Stream [RU: ПотокДанных], Data: Object [RU: Данные], DataType: Type [RU: ТипДанных], Objects: RefObjList, DataTransferObjects: RefObjList, Файл: Объект [RU only], Объекты: Объекты [RU only], ПередаваемыеДанныеСервера: Объекты [RU only]
**Методы:**
- `TResult GetData()` [RU: ПолучитьДанные]
- `Void ThrowIfCancelled()` [RU: ПолучитьДанные]
- `String GetFullErrorMessage()` [RU: ПолучитьДанные]
- `Void Clear()` [RU: ПолучитьДанные]

### `DataTransferResultsAccessor` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** HasErrors: Boolean [RU: ИмеютсяОшибки], Objects: RefObjList, Объекты: Объекты [RU only]
**Методы:**
- `String GetFullErrorMessage()` [RU: ПолучитьПолныйТекстОшибки]

### `DynamicType` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Методы:**
- `TypeCode GetTypeCode()`
- `Boolean ToBoolean(IFormatProvider provider)`
- `Byte ToByte(IFormatProvider provider)`
- `Char ToChar(IFormatProvider provider)`
- `DateTime ToDateTime(IFormatProvider provider)`
- `Decimal ToDecimal(IFormatProvider provider)`
- `Double ToDouble(IFormatProvider provider)`
- `Int16 ToInt16(IFormatProvider provider)`
- `Int32 ToInt32(IFormatProvider provider)`
- `Int64 ToInt64(IFormatProvider provider)`
- `SByte ToSByte(IFormatProvider provider)`
- `Single ToSingle(IFormatProvider provider)`
- `Guid ToGuid(IFormatProvider provider)`
- `Byte[] ToByteArray(IFormatProvider provider)`
- `Object ToType(Type conversionType, IFormatProvider provider) (+1)`
- `UInt16 ToUInt16(IFormatProvider provider)`
- `UInt32 ToUInt32(IFormatProvider provider)`
- `UInt64 ToUInt64(IFormatProvider provider)`

### `GlobalParameterAccessor` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** Item: DynamicType

### `HierarchyLink` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Методы:**
- `HierarchyLink CreateInstance(ComplexHierarchyLink hierarchyLink, MacroContext context)`

### `HierarchyLinkAccessor` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** Item: DynamicType, Changing: Boolean [RU: Редактируется], IsDeleted: Boolean [RU: Редактируется], Parameter: ParameterAccessor [RU: Параметр], ParentObject: RefObj, ChildObject: RefObj, LinkedObject: LinkedObjectAccessor`1, LinkedObjects: LinkedObjectsAccessor`2, Reference: ReferenceAccessor [RU: Справочник], РодительскийОбъект: Объект [RU only], ДочернийОбъект: Объект [RU only], СвязанныйОбъект: LinkedObjectAccessor`1 [RU only], СвязанныеОбъекты: LinkedObjectsAccessor`2 [RU only]
**Методы:**
- `Void AddLink(String linkName, ObjectAccessor refObj)` [RU: Подключить]
- `Void RemoveLink(String linkName, ObjectAccessor refObj) (+1)` [RU: Подключить]
- `Void Save()` [RU: Сохранить]
- `Void BeginChanges()` [RU: Сохранить]
- `Void CancelChanges()` [RU: Сохранить]
- `Boolean Delete()` [RU: Сохранить]
- `HierarchyLink Copy(Boolean copyApplicability, String[] skipParameters)` [RU: Подключить]
- `HierarchyLink FullCopy(ObjectAccessor newParent, ObjectAccessor newChild, String[] copyLinks) (+1)` [RU: Отключить]

### `HierarchyLinkAccessorList`1` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** Item: T, Count: Int32, IsReadOnly: Boolean
**Методы:**
- `Int32 IndexOf(T item)`
- `Void Insert(Int32 index, T item)`
- `Void RemoveAt(Int32 index)`
- `Void Add(T item)`
- `Void Clear()`
- `Boolean Contains(T item)`
- `Void CopyTo(T[] array, Int32 arrayIndex)`
- `Boolean Remove(T item)`
- `IEnumerator`1 GetEnumerator()`

### `HierarchyLinkList` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Методы:**
- `HierarchyLinkList CreateInstance(IEnumerable`1 links, MacroContext context)`

### `IBusinessProcessesAccessor` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Методы:**
- `Boolean Run(MacroContext context, String name, IEnumerable`1 objects, IEnumerable`1 appendantObjects, Dictionary`2 variables, Boolean showDialog, String variablesTemplate, String begin, Func`5 createRunBusinessProcessContext)`
- `Boolean RunLinear(MacroContext context, String prototypeName, IEnumerable`1 objects, Boolean showDialog, Func`3 showLinearBusinessProcessDialog)`
- `Boolean Edit(MacroContext context, String name, Dictionary`2 variables)`
- `Void Complete(MacroContext context, String process, String state, String solution, IEnumerable`1 objects, String comment)`

### `ICommonDialog` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** Caption: String, InitialDirectory: String
**Методы:**
- `Boolean Show()`

### `ICommonFileDialog` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** FilterIndex: Int32, Filter: String, FileNames: String[], FileName: String, AddExtension: Boolean, DefaultExt: String
**Методы:**
- `Stream OpenFile()`

### `IconAccessor` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** IsSvgIcon: Boolean [RU: ВекторныйФормат], SvgIcon: Byte[] [RU: Векторная], Icon: Image [RU: Растровая]

### `IInputDialog` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** Caption: String, Height: Double, Width: Double, FieldValueChangedFlowchartHandler: ActivityEventHandler
**Методы:**
- `Void AddIntegerField(String name, Int32 value, Boolean required)`
- `Void AddDoubleField(String name, Double value, Int32 decimalPlaces, Boolean required)`
- `Void AddStringField(String name, String mask, MaskType maskType, String value, Boolean multiline, Boolean required, Boolean useAllWidth, Int32 lineCount, Boolean useSpellChecker) (+2)`
- `Void AddDateField(String name, DateTime value, Boolean required, Int32 mode, String mask)`
- `Void AddFlagField(String name, Boolean value, Boolean required, Boolean useAllWidth)`
- `Void AddSelectValueField(String name, Object value, Boolean required, Object[] values)`
- `Void AddMultiselectFromList(String name, Object[] values, Boolean required)`
- `Void AddSelectReferenceField(String name, Object value, Boolean required)`
- `Void AddSelectFromReferenceField(String name, String reference, String parameter, Object value, Boolean required, String filter, Guid rootObjectGuid) (+3)`
- `Void AddMatchReferenceObjectField(String name, String reference, String parameter, String relevanceContext, Object value, Boolean required, String filter, String viewName, Boolean useContainsFilter) (+3)`
- `Void AddButton(String name, Action`1 handler, Nullable`1 width) (+2)`
- `Void AddPanel(String name, String header, Double height, Boolean verticalScrollBar, Boolean autoVerticalScrollBar)`
- `Void AddFieldsToPanel(String panelName, String[] fieldName)`
- `Boolean Show(MacroContext context)`
- `Object GetValue(String name)`
- `Void SetValue(String name, Object value)`
- `Void SetIcon(IconImage icon)`
- `Void AddComment(String name, String comment)`
- `Void AddGroup(String text)`
- `Int32 AddPicture(MacroContext context, String parameterName, Int32 lineCount) (+1)`
- `Void ChangePicture(Int32 position, ObjectAccessor refObject, String parameterName)`
- `Int32 AddIcon(ObjectAccessor refObject, String parameterName, Int32 lineCount)`
- `Void ChangeIcon(Int32 position, ObjectAccessor refObject, String parameterName)`
- `Void AddText(String text, Int32 lineCount) (+1)`
- `Void SetSize(Int32 width, Int32 height)`
- `Void SetElementVisibility(String elementName, Boolean visible) (+1)`
- `Void SetElementEnabled(String elementName, Boolean enabled) (+1)`
- `Void SetElementRequired(String elementName, Boolean required) (+1)`
- `Void SetElementFilter(String elementName, String filter) (+1)`
- `Void SetScrollBarsVisibility(Boolean verticalScrollBar, Boolean autoVerticalScrollBar)`

### `InputDialog` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** Caption: String, Height: Double, Width: Double, Item: Object
**Методы:**
- `Void AddInteger(String name, Int32 value, Boolean required)`
- `Void AddDouble(String name, Double value, Int32 decimalPlaces, Boolean required)`
- `Void AddString(String name, String value, Boolean multiline, Boolean required, Boolean useAllWidth, Int32 lineCount)`
- `Void AddMask(String name, String mask, TypeOfMask maskType, String value, Boolean required, Boolean useAllWidth, Int32 lineCount) (+1)`
- `Void AddDate(String name, DateTime value, Boolean required)`
- `Void AddTime(String name, DateTime value, Boolean required)`
- `Void AddPanel(String name, String header, Double height, Boolean verticalScrollBar, Boolean autoVerticalScrollBar)`
- `Void AddFieldsToPanel(String panelName, String[] fieldNames)`
- `Void AddDateTime(String name, DateTime value, Boolean required, String mask)`
- `Void AddFlag(String name, Boolean value, Boolean required, Boolean useAllWidth)`
- `Void AddSelectFromList(String name, Object value, Boolean required, Object[] values) (+1)`
- `Void AddMultiselectFromList(String name, Object[] values, Boolean required) (+1)`
- `Void AddSelectReference(String name, Object value, Boolean required)`
- `Void AddSelectFromReference(String name, String reference, String parameter, Object value, Boolean required, String filter, Guid rootObjectGuid) (+2)`
- `Void AddMatchReferenceObject(String name, String reference, String parameter, String relevanceContext, Object value, Boolean required, String filter, String viewName, Boolean useContainsFilter) (+2)`
- `Void AddButton(String name, Action`1 handler, Nullable`1 width) (+1)`
- `Boolean Show()`
- `Object GetValue(String name)`
- `Void SetValue(String name, Object value)`
- `Void AddComment(String name, String comment)`
- `Void AddGroup(String text)`
- `Void AddText(String text, Int32 lineCount) (+1)`
- `Int32 AddPicture(ObjectAccessor refObj, String imagePath, Int32 lineCount) (+2)`
- `Void ChangePicture(Int32 position, ObjectAccessor refObj, String imagePath)`
- `Int32 AddIcon(ObjectAccessor refObj, String iconPath, Int32 lineCount) (+1)`
- `Void ChangeIcon(Int32 position, ObjectAccessor refObj, String iconPath)`
- `Void SetIcon(Guid guid) (+1)`
- `Void SetSize(Int32 width, Int32 height)`
- `Void SetScrollBarsVisibility(Boolean verticalScrollBar, Boolean autoVerticalScrollBar)`
- `Void SetElementVisibility(String elementName, Boolean visible) (+1)`
- `Void SetElementEnabled(String elementName, Boolean enabled) (+1)`
- `Void SetElementRequired(String elementName, Boolean required) (+1)`
- `Void SetElementFilter(String elementName, String filter)`

### `IOpenFileDialog` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** MultipleSelect: Boolean, SupportMultiDottedExtensions: Boolean

### `IOpenFolderDialog` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** DirectoryName: String

### `IProxyValue` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Методы:**
- `Object GetRealValue()`

### `ISaveFileDialog` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** ValidateNames: Boolean, CreatePrompt: Boolean, OverwritePrompt: Boolean

### `ISelectClassObjectsDialog` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** SelectedClassObjects: ClassObject[], AllowedClassObjects: ClassObject[], Caption: String, SelectAbstractClasses: Boolean, CheckboxSelection: Boolean, CheckboxesAutoSelection: Boolean
**Методы:**
- `Boolean Show()`

### `ISelectListObjectsDialog` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** AllowOrderResult: Boolean, ShowSearchPanel: Boolean, HideDefaultColumns: Boolean, MirrorDialog: Boolean, ListObjects: List`1
**Методы:**
- `Void AddColumn(String name, Func`2 calculateValue)`

### `ISelectObjectDialog` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** ReferenceInfo: ReferenceInfo, Reference: Reference, MultipleSelect: Boolean, ShowToolbar: Boolean, Filter: Filter, FocusedObject: ReferenceObject, CheckboxSelection: Boolean, CheckboxesAutoSelection: Boolean, RootObject: ReferenceObject, View: String, Catalog: String, CatalogFolder: String, PrototypeMode: Boolean, IsReadOnly: Boolean

### `ISelectObjectsFromReferencesDialog` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** SelectedObjects: ReferenceObject[], SelectedHierarchyLinks: ComplexHierarchyLink[], Caption: String
**Методы:**
- `Boolean Show()`

### `IWaitingDialog` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Методы:**
- `Void Show(String caption, Boolean canCancel)`
- `Void Hide()`
- `Boolean NextStep(String description, Nullable`1 progress) (+1)`

### `LinkedHiearchyLinkAccessor`1` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** Item: T

### `LinkedHierarchyLinksAccessor`2` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** Item: TList

### `LinkedObjectAccessor`1` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** Item: T

### `LinkedObjectsAccessor`2` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** Item: TList

### `MailBodyTypeAccessor` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** Text: MailBodyTypeObj, Rtf: MailBodyTypeObj, Html: MailBodyTypeObj, ФорматТекст: ФорматТекста [RU only], ФорматRtf: ФорматТекста [RU only], ФорматHtml: ФорматТекста [RU only]
**Методы:**
- `Object GetRealValue()`

### `MailMessageAccessor` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** Owner: UserRefObj, Recipients: List`1, CopyRecipients: List`1, Subject: String [RU: Заголовок], Body: String [RU: Заголовок], BodyType: MailBodyTypeObj, SendDate: Nullable`1 [RU: ВремяОтправки], ReceivedDate: Nullable`1 [RU: ВремяОтправки], ReadDate: Nullable`1 [RU: ВремяОтправки], FileAttachments: List`1 [RU: ФайлыВложения], RefObjAttachments: RefObjList, Folder: String [RU: Заголовок], Владелец: Объект [RU only], Получатели: List`1 [RU only], ПолучателиКопии: List`1 [RU only], ФорматТекста: ФорматТекста [RU only], ОбъектыВложения: Объекты [RU only]
**Методы:**
- `Boolean AddRecipient(UserRefObj user)` [RU: ДобавитьПолучателя]
- `Boolean DeleteRecipient(UserRefObj user)` [RU: ДобавитьПолучателя]
- `Boolean AddCopyRecipient(UserRefObj user)` [RU: ДобавитьПолучателя]
- `Boolean DeleteCopyRecipient(UserRefObj user)` [RU: ДобавитьПолучателя]
- `Void Save()` [RU: Сохранить]
- `Void Send()` [RU: Сохранить]
- `Boolean Delete()` [RU: Сохранить]
- `Boolean AddFileAttachments(String filePath)` [RU: ДобавитьПолучателя]
- `Boolean DeleteFileAttachment(String filePath)` [RU: ДобавитьПолучателя]
- `Boolean AddRefObjAttachment(RefObj refObj)` [RU: ДобавитьПолучателя]
- `Boolean DeleteRefObjAttachment(RefObj refObj)` [RU: ДобавитьПолучателя]
- `Object GetRealValue()` [RU: Сохранить]

### `MailMessageObj` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Методы:**
- `MailMessageObj CreateInstance(MailMessage mailMessage, MacroContext context)`

### `MailTaskAccessor` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** Subject: String [RU: Тема], Body: String [RU: Тема], StartDate: Nullable`1 [RU: ДатаНачала], EndDate: Nullable`1 [RU: ДатаНачала], CheckDate: Nullable`1 [RU: ДатаНачала], Executors: RefObjList, FileAttachments: List`1 [RU: ФайлыВложения], RefObjAttachments: RefObjList, PropertyHyperlink: String [RU: Тема], Hyperlink: String [RU: Тема], Исполнители: Объекты [RU only], ОбъектыВложения: Объекты [RU only]
**Методы:**
- `Void Save()` [RU: Сохранить]
- `Void Send()` [RU: Сохранить]
- `Boolean AddExecutor(String fullName) (+1)` [RU: ДобавитьИсполнителя]
- `Boolean DeleteExecutor(String fullName) (+1)` [RU: ДобавитьИсполнителя]
- `Boolean AddFileAttachment(String filePath)` [RU: ДобавитьИсполнителя]
- `Boolean DeleteFileAttachment(String filePath)` [RU: ДобавитьИсполнителя]
- `Boolean AddRefObjAttachment(RefObj refObj)` [RU: ДобавитьИсполнителя]
- `Boolean DeleteRefObjAttachment(RefObj refObj)` [RU: ДобавитьИсполнителя]
- `Object GetRealValue()` [RU: Сохранить]

### `MailTaskObj` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Методы:**
- `MailTaskObj CreateInstance(MailTask mailTask, MacroContext context)`

### `MaskTypeAccessor` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** Simple: TypeOfMask, Numeric: TypeOfMask, TimeSpan: TypeOfMask, DateTime: TypeOfMask, RegEx: TypeOfMask, Простая: ТипМаски [RU only], Числовая: ТипМаски [RU only], ПромежутокВремени: ТипМаски [RU only], ДатаИВремя: ТипМаски [RU only], РегулярноеВыражение: ТипМаски [RU only]
**Методы:**
- `Object GetRealValue()`

### `NomenclatureReferenceAccessor` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Методы:**
- `RefObj LinkObject(RefObj refObj, RefObj parentObject) (+1)` [RU: Подключить]
- `RefObj CreateObject(String className, RefObj parentObject) (+1)` [RU: Подключить]

### `ObjectAccessor` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** Item: DynamicType, Parameter: ParameterAccessor [RU: Параметр], Id: Int32 [RU: Идентификатор], Guid: Guid [RU: ГлобальныйИдентификатор], Author: UserRefObj, Owner: RefObj, ParentObject: RefObj, NewParentObject: RefObj, ParentObjects: RefObjList, ChildObjects: RefObjList, AllChildObjects: RefObjList, AllParentObjects: RefObjList, MasterObject: RefObj, RootObject: RefObj, LinkedObject: LinkedObjectAccessor`1, LinkedObjects: LinkedObjectsAccessor`2, LinkedHierarchyLink: LinkedHiearchyLinkAccessor`1, LinkedHierarchyLinks: LinkedHierarchyLinksAccessor`2, ChildHierarchyLinks: HierarchyLinkList, AllChildHierarchyLinks: HierarchyLinkList, ParentHierarchyLinks: HierarchyLinkList, Reference: ReferenceAccessor [RU: Справочник], Class: ClassRefObj, Changing: Boolean [RU: Редактируется], IsAdded: Boolean [RU: Редактируется], IsDeleted: Boolean [RU: Редактируется], Signatures: SignatureObjList, IsCheckedOut: Boolean [RU: Редактируется], IsCheckedOutByCurrentUser: Boolean [RU: Редактируется], Access: AccessAccessor [RU: Доступ], PropertyHyperlink: String [RU: СсылкаНаСвойства], Hyperlink: String [RU: СсылкаНаСвойства], Автор: Пользователь [RU only], ПользовательВладелец: Объект [RU only], РодительскийОбъект: Объект [RU only], НовыйРодительскийОбъект: Объект [RU only], РодительскиеОбъекты: Объекты [RU only], ДочерниеОбъекты: Объекты [RU only], ВсеДочерниеОбъекты: Объекты [RU only], ВсеРодительскиеОбъекты: Объекты [RU only], Владелец: Объект [RU only], КорневойОбъект: Объект [RU only], СвязанныйОбъект: LinkedObjectAccessor`1 [RU only], СвязанныеОбъекты: LinkedObjectsAccessor`2 [RU only], СвязанноеПодключение: LinkedHiearchyLinkAccessor`1 [RU only], СвязанныеПодключения: LinkedHierarchyLinksAccessor`2 [RU only], ДочерниеПодключения: Подключения [RU only], ВсеДочерниеПодключения: Подключения [RU only], РодительскиеПодключения: Подключения [RU only], Тип: ТипОбъекта [RU only], Подписи: Подписи [RU only]
**Методы:**
- `Void SetAccess(RefObj user, String accessName, AccessDirectionObj accessDirection) (+1)` [RU: ПрименитьИзменения]
- `Void DeleteAccess(RefObj user, String accessName) (+1)` [RU: Подключить]
- `Void DeleteAllAccesses()` [RU: Изменить]
- `Void OnCreated()` [RU: Изменить]
- `T CastTo(Func`2 objectValidate, Func`3 objectCreator)` [RU: Подключить]
- `T To()` [RU: Изменить]
- `HierarchyLink GetChildLink(RefObj childObj)` [RU: ВернутьРодительскоеПодключение]
- `HierarchyLink GetParentLink(RefObj parentObj)` [RU: ВернутьРодительскоеПодключение]
- `Void AddLink(String linkName, ObjectAccessor refObj)` [RU: Подключить]
- `Void RemoveLink(String linkName, ObjectAccessor refObj) (+1)` [RU: Подключить]
- `Void BeginChanges()` [RU: Изменить]
- `Void BeginChangesKeepingSignatures()` [RU: Изменить]
- `Void BeginChangesClass(String className)` [RU: ВернутьРодительскоеПодключение]
- `Void CheckIn(String comment, Boolean showDialog, Boolean keepCheckedOut)` [RU: ПрименитьИзменения]
- `Void SetOwnerUser(RefObj user)` [RU: ВернутьРодительскоеПодключение]
- `Void Save()` [RU: Изменить]
- `Void CancelChanges(Boolean undoCheckOut)` [RU: ВернутьРодительскоеПодключение]
- `RefObj CreateListObject(String objectListName, String className) (+1)` [RU: Подключить]
- `Boolean Delete()` [RU: Изменить]
- `RefObj Copy(String className, RefObj parentObject, String[] skipParameters) (+1)` [RU: Изменить]
- `RefObj FullCopy(String[] copyLinks)` [RU: ВернутьРодительскоеПодключение]
- `Boolean AddSignature(String signatureType, RefObj user)` [RU: Подключить]
- `Boolean SetSignature(String signatureType, RefObj user, String resolution) (+1)` [RU: Подключить]
- `Boolean ChangeStage(String stageName, String comment, Boolean ignoreSchemeStage) (+1)` [RU: Подключить]
- `Void RaiseEvent(String eventName)` [RU: ВернутьРодительскоеПодключение]

### `ObjectAccessorList`1` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** Item: T, Count: Int32
**Методы:**
- `List`1 To()`
- `Int32 IndexOf(T item)`
- `Void Insert(Int32 index, T item)`
- `Void RemoveAt(Int32 index)`
- `Void Add(T item)`
- `Void Clear()`
- `Boolean Contains(T item)`
- `Void CopyTo(T[] array, Int32 arrayIndex)`
- `Boolean Remove(T item)`
- `IEnumerator`1 GetEnumerator()`

### `ObjectInstanceAccessor` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** SourceObject: RefObj, SourceHierarchyLink: HierarchyLink, ИсходныйОбъект: Объект [RU only], ИсходноеПодключение: Подключение [RU only]

### `OpenFileDialog` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** AddExtension: Boolean, Caption: String, DefaultExt: String, FileName: String, FileNames: String[], Filter: String, FilterIndex: Int32, InitialDirectory: String, MultipleSelect: Boolean, SupportMultiDottedExtensions: Boolean
**Методы:**
- `Boolean Show()`
- `Stream OpenFile()`

### `OpenFolderDialog` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** Caption: String, DirectoryName: String, InitialDirectory: String
**Методы:**
- `Boolean Show()`

### `ParameterAccessor` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** Item: DynamicType
**Методы:**
- `String Value(String parameterName)` [RU: Значение]

### `ProgressIndicatorAccessor` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** Text: String [RU: Текст]
**Методы:**
- `Void Hide()` [RU: Скрыть]
- `Void Show()` [RU: Скрыть]

### `ProjectManagementReferenceAccessor` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Методы:**
- `Void OpenProjectEditor(RefObjList projects, String view, String style)` [RU: ОткрытьРедакторПроектов]
- `Void OpenWorksUsage(RefObj project, String view, String style)` [RU: ОткрытьРедакторПроектов]
- `Void OpenResourcesUsage(RefObj project, RefObj resource, String view, String style)` [RU: ОткрытьИспользованиеРесурсов]

### `ReferenceAccessor` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** Name: String [RU: Имя], Guid: Guid [RU: УникальныйИдентификатор], Id: Int32 [RU: Идентификатор], SignatureTypes: SignatureTypeObjList, ТипыПодписей: ТипыПодписей [RU only]

### `RefObj` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Методы:**
- `RefObj CreateInstance(ReferenceObject object, MacroContext context, ComplexHierarchyLink hierarchyLink) (+1)`

### `RefObjInstance` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Методы:**
- `RefObjInstance CreateInstance(ReferenceObjectInstance object, MacroContext context)`

### `RefObjList` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Методы:**
- `RefObjList CreateInstance(IEnumerable`1 objects, MacroContext context)`
- `RefObjList Match(String filter)`
- `RefObjList SelectTopLevelObjectsFromList()`

### `ReportAccessor` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** OpenFile: Boolean [RU: ОткрытьФайл], OpenFileInTab: Boolean [RU: ОткрытьФайл], ReportFolderPath: String [RU: ПутьКПапке], ReportFileName: String [RU: ПутьКПапке], UserDialog: UserDialogObj, ПользовательскийДиалог: ПользовательскийДиалог [RU only]
**Методы:**
- `ReportResult Generate(RefObj refObj) (+2)` [RU: Сформировать]
- `Object GetRealValue()` [RU: Сформировать]

### `ReportReferenceAccessor` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Методы:**
- `ReportRef Find(String name, String referenceName)` [RU: Найти]
- `Object GetRealValue()`

### `ReportResultAccessor` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** ReportFilePath: String [RU: ПутьКФайлу], ReportFile: RefObj, HasErrors: Boolean [RU: СодержитОшибки], ErrorsDescription: String [RU: ПутьКФайлу], ФайлОтчёта: Объект [RU only]
**Методы:**
- `Object GetRealValue()`

### `ResolutionTaskAccessor` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** Responsible: UserRefObj, Comment: String [RU: Комментарий], Ответственный: Пользователь [RU only]

### `ResolutionTaskObj` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Методы:**
- `ResolutionTaskObj CreateInstance(MailResolution mailTask, MacroContext context)`

### `SaveFileDialog` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** Caption: String, InitialDirectory: String, FilterIndex: Int32, Filter: String, FileNames: String[], FileName: String, ValidateNames: Boolean, AddExtension: Boolean, DefaultExt: String, CreatePrompt: Boolean, OverwritePrompt: Boolean
**Методы:**
- `Stream OpenFile()`
- `Boolean Show()`

### `SearchTerm` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** Parameter: String, Operator: String, Value: String
**Методы:**
- `Filter ToFilter(ReferenceInfo reference)`
- `IEnumerable`1 Parse(String value)`

### `SearchTermList`1` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** Item: T, Count: Int32, IsReadOnly: Boolean
**Методы:**
- `Void Add(T item)`
- `Void Clear()`
- `Boolean Contains(T item)`
- `Void CopyTo(T[] array, Int32 arrayIndex)`
- `IEnumerator`1 GetEnumerator()`
- `Int32 IndexOf(T item)`
- `Void Insert(Int32 index, T item)`
- `Boolean Remove(T item)`
- `Void RemoveAt(Int32 index)`
- `Filter ToFilter(ReferenceInfo reference)`

### `SearchTerms` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Методы:**
- `SearchTerms CreateInstance(IEnumerable`1 terms)`

### `SelectClassObjectsDialogAccessor`1` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** Caption: String, SelectedClasses: T[], AllowedClasses: T[], SelectAbstractClasses: Boolean, CheckboxesAutoSelection: Boolean, CheckboxSelection: Boolean
**Методы:**
- `Boolean Show()`

### `SelectListObjectsDialog` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Методы:**
- `Void AddColumn(String name, Func`2 calculateValue)`

### `SelectListObjectsDialogAccessor`2` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** Caption: String, AllowOrderResult: Boolean, ShowSearchPanel: Boolean, HideDefaultColumns: Boolean, MirrorDialog: Boolean, ListObjects: TList, SelectedObjects: TList
**Методы:**
- `Boolean Show()`
- `Void AddColumn(String name, Func`2 calculateValue)`

### `SelectObjectDialogAccessor`4` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** MultipleSelect: Boolean, Filter: String, CheckboxSelection: Boolean, CheckboxesAutoSelection: Boolean, SelectedHierarchyLinks: HList, FocusedObject: T, SelectedObjects: TList, FocusedHierarchyLink: H, RootObject: T, Caption: String, View: String, Catalog: String, CatalogFolder: String, ShowToolbar: Boolean, PrototypeMode: Boolean, IsReadOnly: Boolean
**Методы:**
- `Boolean Show()`

### `SelectObjectsDialogAccessor`2` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** SelectedObjects: TList, Caption: String
**Методы:**
- `Boolean Show()`

### `SignatureAccessor` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** SignatureType: String [RU: ТипПодписи], User: RefObj, SignatureDate: Nullable`1 [RU: ДатаПодписи], IsActual: Boolean [RU: Актуальная], IsSet: Boolean [RU: Актуальная], Resolution: String [RU: ТипПодписи], HasDigitalSignature: Boolean [RU: Актуальная], Пользователь: Объект [RU only]
**Методы:**
- `Boolean Delete()` [RU: Удалить]

### `SignatureAccessorList`1` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** Item: T, Item: T, Count: Int32
**Методы:**
- `Int32 IndexOf(T item)`
- `Void Insert(Int32 index, T item)`
- `Void RemoveAt(Int32 index)`
- `Void Add(T item)`
- `Void Clear()`
- `Boolean Contains(T item)`
- `Void CopyTo(T[] array, Int32 arrayIndex)`
- `Boolean Remove(T item)`
- `IEnumerator`1 GetEnumerator()`

### `SignatureObj` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Методы:**
- `SignatureObj CreateInstance(Signature signature, MacroContext context)`

### `SignatureTypeAccessor` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** Id: Int32 [RU: Идентификатор], Guid: Guid [RU: УникальныйИдентификатор], Name: String [RU: Наименование], Description: String [RU: Наименование]

### `SignatureTypeAccessorList`1` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** Item: T, Item: T, Count: Int32
**Методы:**
- `Int32 IndexOf(T item)`
- `Void Insert(Int32 index, T item)`
- `Void RemoveAt(Int32 index)`
- `Void Add(T item)`
- `Void Clear()`
- `Boolean Contains(T item)`
- `Void CopyTo(T[] array, Int32 arrayIndex)`
- `Boolean Remove(T item)`
- `IEnumerator`1 GetEnumerator()`

### `SignatureTypeObj` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Методы:**
- `SignatureTypeObj CreateInstance(SignatureType signatureType, MacroContext context)`

### `UserDialogObj` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Методы:**
- `UserDialogObj CreateInstance(UserDialogObject object, MacroContext context)`

### `UserDialogObjectAccessor` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** Caption: String [RU: Заголовок], ShowConfirmationOnCancel: Boolean [RU: ПоказыватьПодтверждениеПриОтмене]
**Методы:**
- `UserDialogObjectAccessor CreateInstance(UserDialogObject object, MacroContext context)`
- `Boolean Show()` [RU: ПоказатьДиалог]

### `UserObjectAccessor` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** Name: String [RU: Наименование], Description: String [RU: Наименование], FirstName: String [RU: Наименование], LastName: String [RU: Наименование], Patronymic: String [RU: Наименование], ShortName: String [RU: Наименование], Login: String [RU: Наименование]
**Методы:**
- `UserObjectAccessor CreateInstance(User object, MacroContext context)`

### `UserRefObj` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Методы:**
- `UserRefObj CreateInstance(User object, MacroContext context)`

### `ValueList` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Методы:**
- `ValueList CreateInstance(ParameterValueList valueList)`

### `ValueListItem` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Методы:**
- `ValueListItem CreateInstance(ListValue listValue)`

### `ValueListItemAccessor` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** Name: String [RU: Наименование], Value: Object [RU: Значение], Icon: IconObj, Иконка: Иконка [RU only]

### `VariableAccessor` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** Item: Object

### `WaitingDialogAccessor` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Методы:**
- `Void Show(String caption, Boolean canCancel)` [RU: Показать]
- `Void Hide()` [RU: Скрыть]
- `Boolean NextStep(String description, Nullable`1 progress) (+1)` [RU: Скрыть]

### `ДиалогВвода` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** Заголовок: String [RU only], Высота: Double [RU only], Ширина: Double [RU only]

### `ДиалогВыбораОбъектов` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** МножественныйВыбор: Boolean [RU only], Фильтр: String [RU only], ВыборФлажками: Boolean [RU only], АвтоВыборФлажками: Boolean [RU only], ВыбранныеОбъекты: Объекты [RU only], ВыбранныеПодключения: Подключения [RU only], ФокусированныйОбъект: Объект [RU only], ФокусированноеПодключение: Подключение [RU only], КорневойОбъект: Объект [RU only], Заголовок: String [RU only], Вид: String [RU only], Каталог: String [RU only], ПапкаКаталога: String [RU only], ПоказатьПанельКнопок: Boolean [RU only], РежимПрототипов: Boolean [RU only], ТолькоЧтение: Boolean [RU only]

### `ДиалогВыбораОбъектовИзНабора` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** Заголовок: String [RU only], СортироватьВручную: Boolean [RU only], ОтображатьСтрокуПоиска: Boolean [RU only], СкрытьСистемныеКолонки: Boolean [RU only], ОтзеркалитьДиалог: Boolean [RU only], НаборОбъектов: Объекты [RU only], ВыбранныеОбъекты: Объекты [RU only]

### `ДиалогВыбораОбъектовИзСправочников` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** ВыбранныеОбъекты: Объекты [RU only], Заголовок: String [RU only]

### `ДиалогВыбораПапки` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** Заголовок: String [RU only], ИмяПапки: String [RU only], НачальнаяПапка: String [RU only]

### `ДиалогВыбораТипов` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** Заголовок: String [RU only], ВыбранныеТипы: ТипОбъекта[] [RU only], РазрешенныеТипы: ТипОбъекта[] [RU only], ВыборАбстрактныхТипов: Boolean [RU only], ВыборФлажками: Boolean [RU only], АвтоВыборФлажками: Boolean [RU only]

### `ДиалогВыбораФайла` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** ДобавитьРасширение: Boolean [RU only], Заголовок: String [RU only], РасширениеПоУмолчанию: String [RU only], ИмяФайла: String [RU only], ИменаФайлов: String[] [RU only], Фильтр: String [RU only], ИндексФильтра: Int32 [RU only], НачальнаяПапка: String [RU only], МножественныйВыбор: Boolean [RU only], ПоддержкаСоставныхРасширений: Boolean [RU only]

### `ДиалогСохраненияФайла` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Свойства:** Заголовок: String [RU only], НачальнаяПапка: String [RU only], ИндексФильтра: Int32 [RU only], Фильтр: String [RU only], ИменаФайлов: String[] [RU only], ИмяФайла: String [RU only], ПроверкаНаименований: Boolean [RU only], ДобавитьРасширение: Boolean [RU only], РасширениеПоУмолчанию: String [RU only], ЗапросРазрешенияНаСозданиеФайла: Boolean [RU only], ЗапросРазрешенияНаПерезаписьФайла: Boolean [RU only]

### `Задание` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Методы:**
- `Задание CreateInstance(MailTask mailTask, MacroContext context)`

### `ЗаданиеКанцелярии` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Методы:**
- `ЗаданиеКанцелярии CreateInstance(MailResolution mailTask, MacroContext context)`

### `ЗначениеСписка` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Методы:**
- `ЗначениеСписка CreateInstance(ListValue listValue)`

### `Объект` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Методы:**
- `Объект CreateInstance(ReferenceObject object, MacroContext context, ComplexHierarchyLink hierarchyLink) (+1)`

### `Объекты` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Методы:**
- `Объекты CreateInstance(IEnumerable`1 objects, MacroContext context)`

### `Подключение` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Методы:**
- `Подключение CreateInstance(ComplexHierarchyLink hierarchyLink, MacroContext context)`

### `Подключения` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Методы:**
- `Подключения CreateInstance(IEnumerable`1 links, MacroContext context)`

### `Подпись` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Методы:**
- `Подпись CreateInstance(Signature signature, MacroContext context)`

### `Пользователь` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Методы:**
- `Пользователь CreateInstance(User object, MacroContext context)`

### `ПользовательскийДиалог` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Методы:**
- `ПользовательскийДиалог CreateInstance(UserDialogObject object, MacroContext context)`

### `Сообщение` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Методы:**
- `Сообщение CreateInstance(MailMessage mailMessage, MacroContext context)`

### `СписокЗначений` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Методы:**
- `СписокЗначений CreateInstance(ParameterValueList valueList)`

### `ТипПодписи` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Методы:**
- `ТипПодписи CreateInstance(SignatureType signatureType, MacroContext context)`

### `ЭкземплярОбъекта` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel`)
**Методы:**
- `ЭкземплярОбъекта CreateInstance(ReferenceObjectInstance object, MacroContext context)`

### `DataExchangeRunSettings` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel.DataExchange`)
**Свойства:** Context: MacroContext, MacroProviderType: Type, Name: String, ReferenceRuleName: String, IsImport: Boolean, ReadPacketSize: Nullable`1, WritePacketSize: Nullable`1, ShowDialog: Boolean, AdvancedLogging: Boolean, FilePath: String, UsePackage: Boolean, ReferenceName: String, FilterString: String, RootObject: ReferenceObject, OnlyChildrenObjects: Boolean, Count: Int32, Offset: Int32, SortFields: IReadOnlyCollection`1, Objects: IReadOnlyCollection`1, CatalogFolder: String, ExtendedProperties: Dictionary`2, AttachToObjects: IReadOnlyCollection`1, ShowSettingsDialog: Func`3, GetReference: Func`2, Deleting: Boolean, Server: String, AllowProcessingList: Boolean, ReloadObjects: Boolean, ShowWaitingDialog: Boolean, TransferData: Dictionary`2, ConnectionString: String, OnlyStructure: Boolean, IntermediateConnectionString: String, ClearTransformTables: Boolean

### `DataTransferRunSettings` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel.DataExchange`)
**Свойства:** Context: MacroContext, DataTransferObject: ReferenceObject, ExtendedProperties: Dictionary`2, AllowProcessingList: Boolean

### `IDataExchangeAccessor` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel.DataExchange`)
**Методы:**
- `DataExchangeResultsAccessor Run(DataExchangeRunSettings runSettings) (+1)`
- `DataTransferResultsAccessor From(MacroContext context, List`1 results)`
- `MasterServerMacroProvider Connect(MacroContext context, String server)`

### `MasterServerMacroProvider` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel.DataExchange`)
**Методы:**
- `Void Disconnect()` [RU: Отключиться]

### `ILayoutItem` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel.Layout`)
**Свойства:** Name: String
**Методы:**
- `Void Reload()` [has Async]

### `ILayoutItemWithFilter` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel.Layout`)
**Методы:**
- `Void SetFilter(Filter filter) (+1)`
- `Void SetFilterIsActive(Boolean value)`

### `ILayoutItemWithReferenceGroup` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel.Layout`)
**Методы:**
- `ParameterGroup GetReferenceGroup()`

### `ILayoutItemWithView` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel.Layout`)
**Методы:**
- `Void SetView(ISettingsView view)` [has Async]
- `ReadOnlyCollection`1 GetViews()`
- `ISettingsView GetCurrentView()`

### `IProgressIndicator` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel.Layout`)
**Свойства:** Text: String
**Методы:**
- `Void Hide()`
- `Void Show()`

### `IWindow` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel.Layout`)
**Свойства:** Name: String
**Методы:**
- `Void ReloadItem(String name)`
- `ILayoutItem FindItem(String name)`
- `Void Reload()` [has Async]
- `Void ReloadItems(Predicate`1 layoutItemPredicate)` [has Async]

### `LayoutItemAccessor` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel.Layout`)
**Свойства:** Name: String [RU: Наименование], SelectedObjects: RefObjList, SelectedHierarchyLinks: HierarchyLinkList, IsSupportFilter: Boolean [RU: ПоддерживаетФильтрацию], IsSupportView: Boolean [RU: ПоддерживаетФильтрацию], ВыбранныеОбъекты: Объекты [RU only], ВыбранныеПодключения: Подключения [RU only]
**Методы:**
- `Void ApplyFilter(String parameter, Object value) (+5)` [RU: УстановитьТекущийФильтр]
- `Void EnableFilter()` [RU: ВключитьФильтр]
- `Void DisableFilter()` [RU: ВключитьФильтр]
- `Void ApplyView(ValueTuple`2 view) (+2)` [RU: УстановитьТекущийФильтр]
- `ValueTuple`2[] GetViews()` [RU: ВключитьФильтр]
- `Nullable`1 GetCurrentView()` [RU: ВключитьФильтр]
- `Void Reload()` [RU: ВключитьФильтр]

### `LayoutItemObj` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel.Layout`)
**Методы:**
- `LayoutItemObj CreateInstance(ILayoutItem item, MacroContext context)`

### `WindowAccessor` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel.Layout`)
**Свойства:** Name: String [RU: Наименование]
**Методы:**
- `Void Reload()` [RU: Обновить]
- `Void ReloadItem(String name)` [RU: ОбновитьЭлементУправления]
- `LayoutItemObj FindItem(String name)` [RU: ОбновитьЭлементУправления]

### `WindowObj` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel.Layout`)
**Методы:**
- `WindowObj CreateInstance(IWindow window, MacroContext context)`

### `Окно` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel.Layout`)
**Методы:**
- `Окно CreateInstance(IWindow window, MacroContext context)`

### `ЭлементУправления` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel.Layout`)
**Методы:**
- `ЭлементУправления CreateInstance(ILayoutItem item, MacroContext context)`

### `AccessorDefaultType` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel.Types`)
**Свойства:** Name: String, TypeClass: Type, SubClass: Type, RealClass: Type, DefaultType: DefaultSupportedType, EnumerableDefaultType: DefaultSupportedType, IconName: String, Image: Object, Reference: Guid, Classes: Guid[], CastFunc: Func`3, CastObjectFunc: Func`2
**Методы:**
- `List`1 GetTypeClasses(IEnumerable`1 types)`
- `String GetDefaultTypeName(IEnumerable`1 types, DefaultSupportedType defaultType)`
- `Type GetDefaultTypeClass(IEnumerable`1 types, DefaultSupportedType defaultType)`
- `Type GetDefaultSubClass(IEnumerable`1 types, DefaultSupportedType defaultType, Boolean strongType)`
- `DefaultSupportedType GetDefaultSupportedType(IEnumerable`1 types, Type type)`
- `Type GetRealType(IEnumerable`1 types, DefaultSupportedType defaultType) (+1)`
- `Type GetTypeByRealType(IEnumerable`1 types, Type realType)`
- `AccessorDefaultType GetAccessorType(IEnumerable`1 types, DefaultSupportedType defaultType) (+1)`
- `AccessorDefaultType GetAccessorTypeByRealType(IEnumerable`1 types, Type realType)`
- `AccessorDefaultType GetEnumerableAccessorType(IEnumerable`1 types, Type type)`
- `Object Convert(Object value)`

### `IInfoAttribute` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel.Types`)
**Свойства:** Language: Language, Name: String, Key: String

### `InfoAttribute` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel.Types`)
**Свойства:** Language: Language, Name: String, Key: String, Code: String, EndsWithSemicolon: Boolean, AdditionalName: String
**Методы:**
- `String GetKey(MemberInfo member)`

### `InfoAttributeExtensions` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel.Types`)
**Методы:**
- `T GetCurrentLanguageAttribute(MemberInfo member, Language language) (+1)`
- `Boolean HasObsoleteAttribute(MemberInfo member)`

### `TypeInfoAttribute` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel.Types`)
**Свойства:** Language: Language, Name: String, Key: String, NoIndexBaseClasses: Boolean

### `AccessManagerExtensions` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel.Types.Extensions`)
**Методы:**
- `Void Set(AccessManager manager, UserReferenceObject user, AccessGroup accessGroup, AccessDirection accessDirection)`
- `Void Clear(AccessManager manager, AccessType type)`

### `ClassObjectExtensions` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel.Types.Extensions`)
**Методы:**
- `ReferenceInfo GetReferenceInfo(ClassObject classObject)`
- `Icon CloneIcon(ClassObject classObject)`

### `EntryPointExtensions` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel.Types.Extensions`)
**Методы:**
- `String GetFullName(IEntryPoint entryPoint, ProgrammingLanguage language)`
- `Boolean IsExpression(String serEntryPoint)`
- `Int32 GetParametersCount(String serEntryPoint)`
- `Boolean IsEqual(IEntryPoint entryPoint, String serEntryPoint, ProgrammingLanguage language)`

### `FrameworkExtensions` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel.Types.Extensions`)
**Методы:**
- `String GetDisplayNamespace(Type type)`
- `String GetDisplayName(Type type, ProgrammingLanguage language) (+1)`
- `String GetDisplayFullName(Type type, ProgrammingLanguage language)`
- `String GetFormattedName(Type type)`

### `HierarchyLinkAccessorExtensions` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel.Types.Extensions`)
**Методы:**
- `ObjectAccessor GetParentObject(HierarchyLinkAccessor hierarchyLink)`
- `ObjectAccessor GetChildObject(HierarchyLinkAccessor hierarchyLink)`

### `MacroContextExtensions` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel.Types.Extensions`)
**Методы:**
- `ObjectAccessor CreateObject(MacroContext context, ReferenceObject referenceObject, Language language) (+1)`
- `IEnumerable`1 CreateObjects(MacroContext context, IEnumerable`1 referenceObjects, Language language) (+1)`
- `HierarchyLinkAccessor CreateHierarchyLink(MacroContext context, ComplexHierarchyLink hierarchyLink, Language language) (+1)`
- `IEnumerable`1 CreateHierarchyLinks(MacroContext context, IEnumerable`1 hierarchyLinks, Language language) (+1)`
- `SignatureAccessor CreateSignature(MacroContext context, Signature signature)`
- `InputDialog CreateInputDialog(MacroContext context, String caption)`
- `IEnumerable`1 CreateParameterValueList(MacroContext context, IEnumerable`1 values)`
- `Object GetAccessorValue(MacroContext context, Object obj)`

### `MacroProviderAccessorExtensions` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel.Types.Extensions`)
**Методы:**
- `ObjectAccessor GetCurrentObject(MacroProvider macroProvider)`
- `HierarchyLinkAccessor GetCurrentHierarchyLink(MacroProvider macroProvider)`
- `ObjectAccessor GetCurrentUser(MacroProvider macroProvider)`

### `ParameterGroupExtensions` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel.Types.Extensions`)
**Методы:**
- `Icon CloneIcon(ParameterGroup parameterGroup) (+1)`
- `Boolean IsInherit(ParameterGroup group, Guid groupGuid)`

### `AccessorCreator` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel.Types.Helpers`)
**Методы:**
- `ObjectAccessor CreateObject(ReferenceObject referenceObject, MacroContext context)`

### `AccessorManager` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel.Types.Helpers`)
**Свойства:** Culture: Language, AccessorTypes: ReadOnlyCollection`1, AvailableAccessorTypes: ReadOnlyCollection`1, DefaultSupportedMacroTypes: List`1
**Методы:**
- `Tuple`2 CreateTypeDescription(Func`2 func) (+1)`
- `Void RegisterTypes(IEnumerable`1 accessorTypes)`
- `String GetDefaultTypeName(DefaultSupportedType defaultType)`
- `Type GetDefaultTypeClass(DefaultSupportedType defaultType, Language macrolanguage) (+1)`
- `Type GetDefaultSubClass(DefaultSupportedType defaultType, Boolean strongType)`
- `DefaultSupportedType GetDefaultSupportedType(Type type)`
- `Type GetRealType(DefaultSupportedType defaultType) (+1)`
- `Type GetTypeByRealType(Type realType)`
- `AccessorDefaultType GetEnumerableAccessorType(Type type)`
- `AccessorDefaultType GetAvailableAccessorType(Type type)`
- `AccessorDefaultType GetAccessorType(Type type) (+1)`
- `Type GetValueListType()`
- `List`1 GetObjects(Object desktopObject)`
- `List`1 GetHierarhyLinks(Object desktopObject)`
- `Boolean IsAccessorType(Type type)`
- `Object TryCast(Object value, Type toType)`
- `Object GetRealValue(Object value)`

### `TypeHelper` (Namespace: `TFlex.DOCs.Model.Macros.ObjectModel.Types.Helpers`)
**Методы:**
- `Boolean CanConvert(Type sourceType, Type destinationType)`
- `Object Convert(Object value, Type type, Boolean throwOnError, Boolean useBase64) (+1)`
- `Boolean TryConvert(Object value, Type type, Object& result, Boolean useBase64)`
- `Object GetDefaultValue(Type type)`
- `Object GetDOCsDefaultValue(Type type)`
- `Boolean IsDefaultValue(Object value)`
- `Object ParseExpression(String value)`

### `LinkedObjectPathElement` (Namespace: `TFlex.DOCs.Model.Macros.Path`)
**Свойства:** DefaultType: DefaultSupportedType
**Методы:**
- `ObjectValue GetValue(StructurePathContext context)`
- `Boolean IsEqual(PathElement pathElement)`

### `LinkGroupPathElement` (Namespace: `TFlex.DOCs.Model.Macros.Path`)
**Свойства:** LinkGuid: Guid, IsToMany: Boolean, IsLink: Boolean, DefaultType: DefaultSupportedType
**Методы:**
- `ObjectValue GetValue(StructurePathContext context)`
- `Boolean IsEqual(PathElement pathElement)`

### `NomenclatureObjectPathElement` (Namespace: `TFlex.DOCs.Model.Macros.Path`)
**Свойства:** DefaultType: DefaultSupportedType
**Методы:**
- `ObjectValue GetValue(StructurePathContext context)`
- `Boolean IsEqual(PathElement pathElement)`

### `ObjectPathElement` (Namespace: `TFlex.DOCs.Model.Macros.Path`)
**Свойства:** ReferenceObjectGuid: Guid, HierarchyLinkGuid: Guid, DefaultType: DefaultSupportedType
**Методы:**
- `ObjectValue GetValue(StructurePathContext context)`
- `Boolean NeedReplaceParentOnAdd(PathElement parent)`
- `Boolean IsEqual(PathElement pathElement)`

### `PathElement` (Namespace: `TFlex.DOCs.Model.Macros.Path`)
**Свойства:** Path: String, DisplayName: String, DefaultType: DefaultSupportedType, Type: Type, Parent: PathElement
**Методы:**
- `String GetDisplayNameFromParts(String[] parts)`
- `ObjectValue GetValue(StructurePathContext context)`
- `Boolean NeedReplaceParentOnAdd(PathElement parent)`
- `Boolean IsEqual(PathElement pathElement1, PathElement pathElement2) (+1)`

### `ReferencePathElement` (Namespace: `TFlex.DOCs.Model.Macros.Path`)
**Свойства:** ReferenceGuid: Guid, PrototypeMode: Boolean
**Методы:**
- `ObjectValue GetValue(StructurePathContext context)`
- `Boolean IsEqual(PathElement pathElement)`

### `RegisterPathType` (Namespace: `TFlex.DOCs.Model.Macros.Path`)
**Свойства:** Key: String, ParseFunc: Func`2

### `StructurePath` (Namespace: `TFlex.DOCs.Model.Macros.Path`)
**Свойства:** CurrentElement: PathElement, ResultDefaultType: DefaultSupportedType, ResultType: Type, IsContextVariable: Boolean, IsVariable: Boolean, IsReference: Boolean, IsCorrect: Boolean, IsEmpty: Boolean, Item: PathElement, Count: Int32
**Методы:**
- `Boolean Validate()`
- `ObjectValue GetObjectValue(ServerConnection connection) (+2)`
- `Object GetValue(ServerConnection connection) (+2)`
- `Object GetAccessorValue(ServerConnection connection) (+2)`
- `String Serialize()`
- `String ToVariableString()`
- `Boolean TryParse(String value, StructurePath& path)`
- `StructurePath Parse(String value)`
- `Void RegisterType(RegisterPathType pathType)`
- `Int32 IndexOf(PathElement item)`
- `Void Clear()`
- `Boolean Contains(PathElement item)`
- `Void CopyTo(PathElement[] array, Int32 arrayIndex)`
- `IEnumerator`1 GetEnumerator()`

### `StructurePathContext` (Namespace: `TFlex.DOCs.Model.Macros.Path`)
**Свойства:** Path: StructurePath, MacroContext: MacroContext, ParentValue: ObjectValue

### `StructurePathExtensions` (Namespace: `TFlex.DOCs.Model.Macros.Path`)
**Методы:**
- `TStructurePath Add(TStructurePath path, PathElement pathElement)`
- `TStructurePath AddReference(TStructurePath path, ReferenceInfo referenceInfo, Boolean prototypeMode) (+2)`
- `TStructurePath AddObject(TStructurePath path, ReferenceObject referenceObject) (+1)`
- `TStructurePath AddLinkGroup(TStructurePath path, ParameterGroup parameterGroup)`
- `TStructurePath AddVariable(TStructurePath path, VariableInfo variableInfo, ReferenceInfo referenceInfo)`

### `VariablePathElement` (Namespace: `TFlex.DOCs.Model.Macros.Path`)
**Свойства:** VariableInfoString: String, ReferenceGuid: Guid, IsContext: Boolean, VariableInfo: VariableInfo, IsContextReference: Boolean, DefaultType: DefaultSupportedType, Type: Type
**Методы:**
- `ObjectValue GetValue(StructurePathContext context)`
- `Boolean IsEqual(PathElement pathElement)`

### `ClassObjectLinksGroup` (Namespace: `TFlex.DOCs.Model.Macros.Tools`)
**Свойства:** ClassObject: ClassObject, LinksParameterGroups: ParameterGroupCollection, ReferencePath: ReferencePath, IsFolder: Boolean, HasChildren: Boolean

### `ClassObjectParametersGroup` (Namespace: `TFlex.DOCs.Model.Macros.Tools`)
**Свойства:** ClassObject: ClassObject, IsFolder: Boolean, HasChildren: Boolean

### `ClassObjectTool` (Namespace: `TFlex.DOCs.Model.Macros.Tools`)
**Свойства:** ClassObject: ClassObject

### `CommonToolsManager` (Namespace: `TFlex.DOCs.Model.Macros.Tools`)
**Свойства:** Connection: ServerConnection, Favorites: ToolGroup, Language: Language, Macro: ToolGroup, Math: ToolGroup, Surrounds: ToolGroup, References: ToolGroup, Groups: Dictionary`2, Interface: String, SupportsViews: Boolean
**Методы:**
- `Void UpdateTools()`
- `List`1 LoadGroups(Assembly assembly, String fileName)`
- `ToolGroup LoadGroup(Assembly assembly, String fileName)`
- `Void Save()` [has Async]
- `Void CreateReferencesToolGroup()`

### `GuidTool` (Namespace: `TFlex.DOCs.Model.Macros.Tools`)
**Свойства:** Guid: Guid, IsGroupedMethod: Boolean

### `IToolGroup` (Namespace: `TFlex.DOCs.Model.Macros.Tools`)
**Свойства:** Name: String, Icon: IconImage

### `ObjectsListsGroup` (Namespace: `TFlex.DOCs.Model.Macros.Tools`)
**Свойства:** ClassObject: ClassObject, IsFolder: Boolean, HasChildren: Boolean

### `PathItemGroup` (Namespace: `TFlex.DOCs.Model.Macros.Tools`)
**Свойства:** PathItem: PathItem, HasChildren: Boolean, IsGroupedMethod: Boolean

### `ReferenceGroup` (Namespace: `TFlex.DOCs.Model.Macros.Tools`)
**Свойства:** ParameterGroup: ParameterGroup, HasChildren: Boolean

### `Tool` (Namespace: `TFlex.DOCs.Model.Macros.Tools`)
**Свойства:** Code: String, CursorPosition: Int32, ElementName: String, IsFolder: Boolean, IsGroupedMethod: Boolean

### `ToolGroup` (Namespace: `TFlex.DOCs.Model.Macros.Tools`)
**Свойства:** IsFolder: Boolean, IsGroupedMethod: Boolean, HasChildren: Boolean, Icon: IconImage, HasOverloads: Boolean, Name: String, Children: Collection`1
**Методы:**
- `ToolGroup FindByName(String name)`

### `ToolGroupHeader` (Namespace: `TFlex.DOCs.Model.Macros.Tools`)
**Свойства:** Name: String, Icon: IconImage, GroupType: Type, IsStatic: Boolean, IsNeutral: Boolean, SubGroups: ToolGroupHeader[]

### `ToolGroupStaticHeader` (Namespace: `TFlex.DOCs.Model.Macros.Tools`)
**Свойства:** IsStatic: Boolean

### `ToolsLoader` (Namespace: `TFlex.DOCs.Model.Macros.Tools`)
**Методы:**
- `ToolGroup Load(ToolGroupHeader header, Language language)`
- `Boolean TryFillHeaderName(ToolGroupHeader header, Type groupType, TypeInfoAttribute currentLanguageAttribute, Type linkedType, Language language, String& headerName)`
- `Tool CreateToolItem(InfoAttribute attribute, MemberInfo member, Boolean isStatic)`

### `ToolsManagerData` (Namespace: `TFlex.DOCs.Model.Macros.Tools`)
**Свойства:** Favorites: ToolGroup, Language: String, MacroLanguage: Language, References: List`1

### `Account` (Namespace: `TFlex.DOCs.Model.Mail`)
**Свойства:** IsLoggedIn: Boolean, IsClientViewAccount: Boolean, OwnerId: Int32, Owner: User, Connection: ServerConnection, Guid: Guid, Folders: MailFolderCollection, Inbox: MailFolder, SentItems: MailFolder, Drafts: MailFolder, DeletedItems: MailFolder, Rules: IEnumerable`1, Name: String, MessagesAccess: MailMessagesAccess, CanSaveMessagesOnServer: Boolean
**Методы:**
- `Void ReloadFolders()` [has Async]
- `Void ReloadRules()`
- `Boolean IsSystemFolder(MailItemFolder folder)`
- `Void SendMessage(MailMessage message)`
- `Boolean SetMessageUnread(MailMessage message)`
- `Boolean SetMessageRead(MailMessage message)`
- `Boolean SetMessagesUnread(IEnumerable`1 messages)`
- `Boolean SetMessagesRead(IEnumerable`1 messages)`
- `Boolean SetFolderRead(MailItemFolder mailItemFolder)`
- `Boolean SetFolderUnread(MailItemFolder mailItemFolder)`
- `List`1 GetRootFolders()` [has Async]
- `Void AddRule(MailRule rule)`
- `Void RemoveRule(MailRule rule)`
- `Void MoveRuleUp(MailRule rule)`
- `Void MoveRuleDown(MailRule rule)`
- `Void SaveRules()`
- `Boolean SaveFolder(MailItemFolder folder)`
- `Boolean MoveMailFolderTo(MailFolder folder, MailFolder newParentFolder)`
- `Boolean DeleteFolder(MailItemFolder folder, Boolean useAccountSettings)`
- `Boolean MoveMessagesTo(MailFolder toFolder, IEnumerable`1 messages)`
- `Boolean DeleteMessages(IEnumerable`1 messages, Boolean useAccountSettings)`
- `MailMessage FindMessage(Int32 globalId, Int32 folderId)`

### `Attachment` (Namespace: `TFlex.DOCs.Model.Mail`)
**Свойства:** Name: String, IsFile: Boolean, IsObject: Boolean, IsMessage: Boolean, IsTask: Boolean

### `CategoryMailField` (Namespace: `TFlex.DOCs.Model.Mail`)
**Методы:**
- `List`1 GetComparisonOperators()`
- `String ConvertToString(Object value)`
- `Object Parse(ServerConnection connection, String str, IFormatProvider provider, ComparisonOperator operator)`

### `ContainsMailCategoryOperator` (Namespace: `TFlex.DOCs.Model.Mail`)
**Свойства:** RequireValueList: Boolean
**Методы:**
- `Boolean Compare(Object firstOperand, Object secondOperand)`

### `ControllerMailField` (Namespace: `TFlex.DOCs.Model.Mail`)
**Методы:**
- `List`1 GetComparisonOperators()`
- `Object Parse(ServerConnection connection, String str, IFormatProvider provider, ComparisonOperator operator)`

### `DOCsAccount` (Namespace: `TFlex.DOCs.Model.Mail`)
**Свойства:** Instance: DOCsAccount, TaskAccess: MailTasksAccess, Guid: Guid, Name: String, Outbox: MailFolder, TaskFolders: TaskFolderCollection, CanSaveMessagesOnServer: Boolean
**Методы:**
- `Int32 GetUnreadTaskCount()` [has Async]
- `List`1 GetTasks(Int32 count, Int32 startIndex, Filter filter) (+1)` [has Async]
- `List`1 GetUnreadTasks()` [has Async]
- `Void CancelTasks(Filter filter)` [has Async]
- `MailMessage FindMessage(Int32 globalId, Int32 folderId)`
- `MailTask FindTask(Int32 globalId)` [has Async]
- `List`1 GetObjectMessages(ReferenceObject object)` [has Async]
- `List`1 GetObjectTasks(ReferenceObject object)` [has Async]
- `List`1 GetObjectItems(ReferenceObject object)` [has Async]
- `Boolean IsSystemFolder(MailItemFolder folder)`
- `SmtpServerSettings GetSmtpServerSettings(User user)`
- `Void SendMessage(MailMessage message)`
- `Boolean SetMessagesRead(IEnumerable`1 messages)`
- `Boolean SetMessagesUnread(IEnumerable`1 messages)`
- `Boolean SetFolderRead(MailItemFolder mailItemFolder)`
- `Boolean SetFolderUnread(MailItemFolder mailItemFolder)`

### `EMailAddress` (Namespace: `TFlex.DOCs.Model.Mail`)
**Свойства:** Name: String, Email: String, Address: String, NetMailAddress: MailAddress

### `EmailServerSettings` (Namespace: `TFlex.DOCs.Model.Mail`)
**Методы:**
- `XmlSchema GetSchema()`
- `Void ReadXml(XmlReader reader)`
- `Void WriteXml(XmlWriter writer)`

### `EqualMailCategoryOperator` (Namespace: `TFlex.DOCs.Model.Mail`)
**Свойства:** Type: ComparisonOperatorType, RequireValueList: Boolean
**Методы:**
- `Boolean Compare(Object firstOperand, Object secondOperand)`

### `FileAttachment` (Namespace: `TFlex.DOCs.Model.Mail`)
**Свойства:** IsFile: Boolean, Name: String, Size: Int64, FilePath: String, SourceFilePath: String
**Методы:**
- `Boolean DownloadFile()`
- `Void DeleteFile()`

### `FolderMovedHandler` (Namespace: `TFlex.DOCs.Model.Mail`)
**Методы:**
- `Void Invoke(MailFolder folder)`
- `IAsyncResult BeginInvoke(MailFolder folder, AsyncCallback callback, Object object)`
- `Void EndInvoke(IAsyncResult result)`

### `FromMailField` (Namespace: `TFlex.DOCs.Model.Mail`)
**Методы:**
- `List`1 GetComparisonOperators()`
- `Object Parse(ServerConnection connection, String str, IFormatProvider provider, ComparisonOperator operator)`

### `GetMailFieldStringValueDelegate` (Namespace: `TFlex.DOCs.Model.Mail`)
**Методы:**
- `Boolean Invoke(MailItem item, String& value)`
- `IAsyncResult BeginInvoke(MailItem item, String& value, AsyncCallback callback, Object object)`
- `Boolean EndInvoke(String& value, IAsyncResult result)`

### `GetMailFieldValueDelegate` (Namespace: `TFlex.DOCs.Model.Mail`)
**Методы:**
- `Boolean Invoke(MailItem item, Object& value)`
- `IAsyncResult BeginInvoke(MailItem item, Object& value, AsyncCallback callback, Object object)`
- `Boolean EndInvoke(Object& value, IAsyncResult result)`

### `IMap` (Namespace: `TFlex.DOCs.Model.Mail`)
**Свойства:** ServerName: String, Login: String, SessionPassword: String, Password: String, SSLMode: SSLMode, MoveToDeleted: Boolean, Port: Int32, IsEmpty: Boolean, AskPassword: Boolean, IsModified: Boolean
**Методы:**
- `IMapSettings ToServerSettings()`

### `ImapAccount` (Namespace: `TFlex.DOCs.Model.Mail`)
**Свойства:** ImapPasswordAction: SessionPasswordDelegate, SmtpPasswordAction: SessionPasswordDelegate, IsLoggedIn: Boolean, IsImapSessionPasswordEntered: Boolean, IsSmtpSessionPasswordEntered: Boolean, Guid: Guid, CanSaveMessagesOnServer: Boolean, SaveOnServer: Boolean, ParentId: Int32, IsModified: Boolean, Inbox: MailFolder, SentItems: MailFolder, DeletedItems: MailFolder, Smtp: Smtp, Imap: IMap, Email: String, UserName: String, InboxFolderName: String, SentFolderName: String, DeletedFolderName: String, DraftsFolderName: String, Name: String
**Методы:**
- `Boolean CheckImapConnection(String serverName, Int32 port, SSLMode sslMode, String login, String password)`
- `Boolean IsSystemFolder(MailItemFolder folder)`
- `EmailErrorType Relogin(String password)`
- `EmailError CheckConnection()`
- `Void ChangeImapPassword(String password, Boolean save)`
- `Void ChangeSmtpSessionPassword(String password, Boolean save)`
- `Void ChangeImapSessionPassword(String password, Boolean save)`
- `String[] GetServerFolders()`
- `Char GetFolderSeparator()`
- `Void Reconnect()`
- `Boolean SaveFolder(MailItemFolder folder)`
- `Boolean MoveMailFolderTo(MailFolder folder, MailFolder newParentFolder)`
- `Boolean DeleteFolder(MailItemFolder folder, Boolean useAccountSettings)`
- `Boolean MoveMessagesTo(MailFolder toFolder, IEnumerable`1 messages)`
- `Boolean DeleteMessages(IEnumerable`1 messages, Boolean useAccountSettings)`
- `String GetLogFilePath()`
- `MailFolder CreateRootFolder()`
- `Void CheckServerFolders()` [has Async]
- `Boolean IsCheckFolderMessages(MailFolder folder)`
- `Int32 GetCheckingFolderMaxMessagesCount(MailFolder folder)`
- `Boolean IsCheckAnyFolderMessages()`
- `Void CheckFoldersMessages()` [has Async]
- `Void CheckFolderMessages(MailFolder folder, CancellationToken token)` [has Async]
- `List`1 GetRootFolders()` [has Async]
- `MailMessage FindMessage(Int32 globalId, Int32 folderId)`
- `Void SetName(String name)`
- `Boolean CanDelete()`
- `Boolean Delete()`
- `String GetFolderPath(MailFolder folder, String folderName)`
- `EmailError SendTestMessage()`
- `Void SendMessage(MailMessage message)` [has Async]
- `Void SendMessages()` [has Async]
- `Void Save()` [has Async]
- `Boolean SetMessagesRead(IEnumerable`1 messages)`
- `Boolean SetMessagesUnread(IEnumerable`1 messages)`
- `Boolean SetFolderRead(MailItemFolder mailItemFolder)`
- `Boolean SetFolderUnread(MailItemFolder mailItemFolder)`
- `Void RegisterWatcher(ImapAccountWatcher watcher)`
- `Void UnregisterWatcher(ImapAccountWatcher watcher)`

### `ImapAccountTemplate` (Namespace: `TFlex.DOCs.Model.Mail`)
**Свойства:** Connection: ServerConnection, CopyAccountUserName: Boolean, CopyAccountEmail: Boolean, CopyAccountLogin: Boolean, Smtp: Smtp, Imap: IMap, Email: String, UserName: String, Id: Int32, Name: String, IsModified: Boolean, Accounts: ReadOnlyCollection`1
**Методы:**
- `Void CopyToAccounts()`
- `ImapAccount AddUser(User user)` [has Async]
- `Boolean RemoveUserAccount(ImapAccount account)`
- `Void Save()` [has Async]
- `Boolean Delete()`

### `IsNotOneOfMailCategoryOperator` (Namespace: `TFlex.DOCs.Model.Mail`)
**Методы:**
- `Boolean Compare(Object firstOperand, Object secondOperand)`

### `IsOneOfMailCategoryOperator` (Namespace: `TFlex.DOCs.Model.Mail`)
**Свойства:** RequireValueList: Boolean
**Методы:**
- `Boolean Compare(Object firstOperand, Object secondOperand)`

### `IsOwnerOperator` (Namespace: `TFlex.DOCs.Model.Mail`)
**Свойства:** SupportsSecondOperand: Boolean
**Методы:**
- `Boolean Compare(Object firstOperand, Object secondOperand)`

### `MailAccessInfo` (Namespace: `TFlex.DOCs.Model.Mail`)
**Свойства:** AccountId: Nullable`1, OwnerId: Nullable`1, UserObjectId: Int32, UserObject: UserReferenceObject, AccessType: MailAccessType, IsModified: Boolean
**Методы:**
- `MailAccessInfo ToServer()`

### `MailAccessManager` (Namespace: `TFlex.DOCs.Model.Mail`)
**Свойства:** Account: Account, Owner: User, AccessType: MailAccessType, Connection: ServerConnection
**Методы:**
- `MailAccessManager GetManager(User user, MailAccessType accessType) (+1)`
- `Void SetMessagesAccess(UserReferenceObject userObject, MailMessagesAccess access)`
- `Void SetTasksAccess(UserReferenceObject userObject, MailTasksAccess access)`
- `Void RemoveAccess(UserReferenceObject userObject)`
- `Void Save()` [has Async]
- `IEnumerator`1 GetEnumerator()`

### `MailAddress` (Namespace: `TFlex.DOCs.Model.Mail`)
**Свойства:** Name: String, Email: String

### `MailCategory` (Namespace: `TFlex.DOCs.Model.Mail`)
**Свойства:** Connection: ServerConnection, Icon: IconImage, Id: Int32, Guid: Guid, IsPublic: Boolean, Color: Nullable`1, Name: String, IsModified: Boolean
**Методы:**
- `MailCategory ToServer()`

### `MailCategoryManager` (Namespace: `TFlex.DOCs.Model.Mail`)
**Свойства:** Connection: ServerConnection, Categories: ReadOnlyCollection`1
**Методы:**
- `MailCategory Find(Int32 categoryId) (+2)`
- `MailCategory CreateCategory()`
- `MailCategory AddCategory(String name, Nullable`1 color) (+2)`
- `Void RemoveCategories(IEnumerable`1 categories)`
- `Boolean Save()`
- `Void RemoveCategory(MailCategory category, List`1 items)`
- `Void ClearCategories(List`1 items)`
- `Void Reload()`

### `MailCheckStatusField` (Namespace: `TFlex.DOCs.Model.Mail`)
**Методы:**
- `List`1 GetComparisonOperators()`

### `MailExtensions` (Namespace: `TFlex.DOCs.Model.Mail`)
**Методы:**
- `RuleType ToModel(MailRuleType type)`
- `MailRuleType ToServer(RuleType type)`

### `MailField` (Namespace: `TFlex.DOCs.Model.Mail`)
**Свойства:** IsSystem: Boolean, FromExtendedData: Boolean, VisibleInEditor: Boolean, IsXmlDataType: Boolean, XQuery_FieldType: String, XQuery_ElementPath: String, XQuery_AttributeName: String, SupportsTasks: Boolean, SupportsMessages: Boolean, Name: String, Title: String, Nullable: Boolean, Type: ParameterType
**Методы:**
- `Boolean TryGetValue(MailItem item, Object& value)`
- `Boolean TryGetStringValue(MailItem item, String& value)`
- `List`1 GetComparisonOperators()`
- `String ConvertToString(Object value)`
- `String ConvertToServerString(Object value)`
- `Object Parse(ServerConnection connection, String str, IFormatProvider provider, ComparisonOperator operator)`
- `Object ParseServer(ServerConnection connection, String str, IFormatProvider provider, ComparisonOperator operator)`

### `MailFolder` (Namespace: `TFlex.DOCs.Model.Mail`)
**Свойства:** MessageCount: Int32, UnreadMessageCount: Int32, Type: MailFolderType
**Методы:**
- `MailFolder GetParent()`
- `List`1 GetSubfolders()` [has Async]
- `Boolean HasSubfolders()`
- `List`1 GetMessages(Int32 count, Int32 startIndex) (+1)`
- `List`1 GetUnreadMessages()`
- `List`1 GetReadMessages()`

### `MailFolderAddedHandler` (Namespace: `TFlex.DOCs.Model.Mail`)
**Методы:**
- `Void Invoke(MailFolder folder)`
- `IAsyncResult BeginInvoke(MailFolder folder, AsyncCallback callback, Object object)`
- `Void EndInvoke(IAsyncResult result)`

### `MailItem` (Namespace: `TFlex.DOCs.Model.Mail`)
**Свойства:** RealSender: MailUser, Account: Account, IsMessage: Boolean, IsTask: Boolean, Id: Int32, GlobalId: Int32, Guid: Guid, From: MailAddress, To: IEnumerable`1, SentDate: Nullable`1, ReceivedDate: Nullable`1, ReadDate: Nullable`1, Subject: String, BodyType: MailBodyType, Body: String, Categories: MailCategoryCollection, Attachments: AttachmentCollection, AttachmentCount: Int32, VisibleAttachmentCount: Int32, IsModified: Boolean, IsSent: Boolean, IsRead: Boolean, IsDraft: Boolean, IsDeleted: Boolean
**Методы:**
- `Void SetBody(String body, MailBodyType type)`
- `Void ResetAttachments()`
- `MailMessage Reply(Boolean copyBody)`
- `MailMessage ReplyAll(Boolean copyBody)`
- `MailMessage Forward(Boolean copyBody)`
- `Void BuildBody(String text, String comments)`
- `Boolean TryGetValue(MailField field, Object& value)`
- `Boolean TryGetStringValue(MailField field, String& value)`

### `MailItemAttachment` (Namespace: `TFlex.DOCs.Model.Mail`)
**Свойства:** Item: MailItem, Name: String, IsMessage: Boolean, IsTask: Boolean

### `MailItemFolder` (Namespace: `TFlex.DOCs.Model.Mail`)
**Свойства:** Account: Account, IsModified: Boolean, Id: Int32, Guid: Guid, Name: String, Comment: String, Icon: IconImage, IsSystem: Boolean
**Методы:**
- `Boolean CanDelete()`
- `Boolean SetItemsRead()`
- `Boolean SetItemsUnread()`

### `MailLoadSettings` (Namespace: `TFlex.DOCs.Model.Mail`)
**Свойства:** Count: Int32, Startindex: Int32, LoadBody: Boolean, LoadToUsers: Boolean, LoadFromUser: Boolean, ObjectKey: ObjectKey, Filter: Filter

### `MailMessage` (Namespace: `TFlex.DOCs.Model.Mail`)
**Свойства:** Account: Account, IsMessage: Boolean, To: MailAddressCollection, Copy: MailAddressCollection, Folder: MailFolder, Uid: Int32, AccountId: Int32
**Методы:**
- `Void Save()`
- `Void Send()`
- `Boolean MoveTo(MailFolder folder)`
- `Boolean SetRead(IEnumerable`1 messages) (+1)`
- `Boolean SetUnread(IEnumerable`1 messages) (+1)`
- `Boolean Delete()`

### `MailMessageField` (Namespace: `TFlex.DOCs.Model.Mail`)
**Свойства:** SupportsTasks: Boolean

### `MailMessagesAccessInfo` (Namespace: `TFlex.DOCs.Model.Mail`)
**Свойства:** AccessType: MailAccessType, Access: MailMessagesAccess
**Методы:**
- `MailAccessInfo ToServer()`

### `MailRule` (Namespace: `TFlex.DOCs.Model.Mail`)
**Свойства:** Account: Account, Id: Int32, Guid: Guid, PriorityIndex: Int32, IsActive: Boolean, Name: String, IsModified: Boolean, RuleType: RuleType, UseFromTerm: Boolean, UseSubjectTerm: Boolean, FromAddress: MailAddress, Subject: String, Actions: ReadOnlyCollection`1
**Методы:**
- `Void AddAction(MailRuleAction action)`
- `Void RemoveAction(MailRuleAction action)`

### `MailRuleAction` (Namespace: `TFlex.DOCs.Model.Mail`)
**Свойства:** Rule: MailRule, Name: String, IsModified: Boolean
**Методы:**
- `MailRuleAction ToServer()`

### `MailService` (Namespace: `TFlex.DOCs.Model.Mail`)
**Свойства:** Connection: ServerConnection, CategoryManager: MailCategoryManager, Accounts: ReadOnlyCollection`1, AccountTemplates: ReadOnlyCollection`1, DOCsAccount: DOCsAccount, ReminderManager: ReminderManager
**Методы:**
- `Void ReloadAccountTemplates()`
- `Boolean RegisterMailField(MailField field)`
- `MailField GetMailField(String name)`
- `ICollection`1 GetMailFields(Boolean isInEditor)`

### `MailTask` (Namespace: `TFlex.DOCs.Model.Mail`)
**Свойства:** Account: Account, IsTask: Boolean, From: MailUser, OnBehalf: MailUser, Controller: MailUser, CanChangeController: Boolean, Executors: MailTaskExecutorCollection, To: MailTaskToCollection, Emails: List`1, StartDate: Nullable`1, EndDate: Nullable`1, CanChangeEndDate: Boolean, CheckDate: Nullable`1, CanChangeCheckDate: Boolean, PercentComplete: Int32, Priority: MailTaskPriority, Status: MailTaskStatus, ParentTask: MailTask, IsAttachment: Boolean, AcceptType: MailTaskAcceptType, CanAccept: Boolean, CanReject: Boolean, CanComplete: Boolean, CanCancel: Boolean, CanSuspend: Boolean, CanRestore: Boolean, CanUpdatePercentComplete: Boolean, CanDelete: Boolean, CanDelegate: Boolean
**Методы:**
- `Void Save()`
- `Void Send()`
- `Boolean CanAddExecutor(MailTaskExecutor item)`
- `Boolean Accept()`
- `Boolean Reject(String comment)`
- `Boolean Complete(String comment, Object state)`
- `Boolean Cancel(String comment)`
- `Boolean Suspend()`
- `Boolean Restore()`
- `Void ReloadExecutors()`
- `Boolean UpdatePercentComplete(Int32 percentComplete)`
- `Boolean SetRead(IEnumerable`1 tasks) (+1)`
- `Boolean SetUnread(IEnumerable`1 tasks) (+1)`
- `Boolean Delete(IEnumerable`1 tasks) (+1)`
- `MailTask Delegate()`
- `MailTask Replan()`

### `MailTaskExecutor` (Namespace: `TFlex.DOCs.Model.Mail`)
**Свойства:** Name: String, User: User, UserId: Int32, ReceivedDate: Nullable`1, ReadDate: Nullable`1, AcceptDate: Nullable`1, CompleteDate: Nullable`1, PercentComplete: Int32, Status: MailTaskStatus, Comment: String

### `MailTaskExtensions` (Namespace: `TFlex.DOCs.Model.Mail`)
**Методы:**
- `String GetCaption(MailTaskPriority priority) (+1)`
- `String GetPropertyHyperlink(MailTask mailTask, String serverAddress)` [has Async]
- `String GetHyperlink(MailTask mailTask, String serverAddress)` [has Async]

### `MailTaskField` (Namespace: `TFlex.DOCs.Model.Mail`)
**Свойства:** SupportsMessages: Boolean
**Методы:**
- `Object Parse(ServerConnection connection, String str, IFormatProvider provider, ComparisonOperator operator)`

### `MailTaskPriorityConverter` (Namespace: `TFlex.DOCs.Model.Mail`)
**Методы:**
- `Object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, Object value, Type destinationType)`

### `MailTasksAccessInfo` (Namespace: `TFlex.DOCs.Model.Mail`)
**Свойства:** AccessType: MailAccessType, Access: MailTasksAccess
**Методы:**
- `MailAccessInfo ToServer()`

### `MailTaskStatusConverter` (Namespace: `TFlex.DOCs.Model.Mail`)
**Методы:**
- `Object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, Object value, Type destinationType)`

### `MailTerm` (Namespace: `TFlex.DOCs.Model.Mail`)
**Свойства:** Field: MailField, ParameterName: String

### `MailUpdateProvider` (Namespace: `TFlex.DOCs.Model.Mail`)
**Методы:**
- `MailMessage[] Update(IEnumerable`1 messages)`

### `MailUser` (Namespace: `TFlex.DOCs.Model.Mail`)
**Свойства:** Connection: ServerConnection, Name: String, Email: String, User: User, UserId: Int32

### `MessageCountChangedHandler` (Namespace: `TFlex.DOCs.Model.Mail`)
**Методы:**
- `Void Invoke(MailFolder folder)`
- `IAsyncResult BeginInvoke(MailFolder folder, AsyncCallback callback, Object object)`
- `Void EndInvoke(IAsyncResult result)`

### `MessageLoadSettings` (Namespace: `TFlex.DOCs.Model.Mail`)
**Свойства:** LoadCopyUsers: Boolean

### `NotContainsMailCategoryOperator` (Namespace: `TFlex.DOCs.Model.Mail`)
**Методы:**
- `Boolean Compare(Object firstOperand, Object secondOperand)`

### `NotEqualMailCategoryOperator` (Namespace: `TFlex.DOCs.Model.Mail`)
**Свойства:** Type: ComparisonOperatorType
**Методы:**
- `Boolean Compare(Object firstOperand, Object secondOperand)`

### `ObjectAttachment` (Namespace: `TFlex.DOCs.Model.Mail`)
**Свойства:** Connection: ServerConnection, Reference: ReferenceInfo, Class: ClassObject, Object: ReferenceObject, Name: String, ReferenceName: String, ReferenceId: Int32, ObjectId: Int32, ClassId: Int32, IsObject: Boolean

### `OnBehalfTaskField` (Namespace: `TFlex.DOCs.Model.Mail`)
**Методы:**
- `List`1 GetComparisonOperators()`
- `Object Parse(ServerConnection connection, String str, IFormatProvider provider, ComparisonOperator operator)`

### `OwnerMailField` (Namespace: `TFlex.DOCs.Model.Mail`)
**Методы:**
- `List`1 GetComparisonOperators()`
- `Object Parse(ServerConnection connection, String str, IFormatProvider provider, ComparisonOperator operator)`

### `PercentMailField` (Namespace: `TFlex.DOCs.Model.Mail`)
**Методы:**
- `List`1 GetComparisonOperators()`

### `RedirectRuleAction` (Namespace: `TFlex.DOCs.Model.Mail`)
**Свойства:** To: List`1, TextTemplate: String, SubjectTemplate: String
**Методы:**
- `MailRuleAction ToServer()`

### `ResponsibleMailResolutionField` (Namespace: `TFlex.DOCs.Model.Mail`)
**Методы:**
- `List`1 GetComparisonOperators()`

### `Smtp` (Namespace: `TFlex.DOCs.Model.Mail`)
**Свойства:** SessionPassword: String, AskPassword: Boolean, ServerName: String, Login: String, Password: String, UseSSL: Boolean, UseAuthentication: Boolean, CopyToSent: Boolean, AuthenticationType: String, AuthenticationAsIncomingMail: Boolean, Port: Int32, IsEmpty: Boolean, IsModified: Boolean
**Методы:**
- `SmtpSettings ToServerSettings()`

### `SortRuleAction` (Namespace: `TFlex.DOCs.Model.Mail`)
**Свойства:** FolderId: Int32, FolderPath: String
**Методы:**
- `MailRuleAction ToServer()`

### `SSLModeExtensions` (Namespace: `TFlex.DOCs.Model.Mail`)
**Методы:**
- `SSLMode ToServer(SSLMode apiValue)`
- `SSLMode ToModel(SSLMode serverValue)`

### `TaskFolder` (Namespace: `TFlex.DOCs.Model.Mail`)
**Свойства:** Account: DOCsAccount, IsPrivate: Boolean, LoadSharedTasks: Boolean
**Методы:**
- `Filter GetFilter()`
- `Void SetFilter(Filter filter)`
- `List`1 GetTasks(TaskLoadSettings settings) (+1)`

### `TaskLoadSettings` (Namespace: `TFlex.DOCs.Model.Mail`)
**Свойства:** LoadControllerUser: Boolean, LoadOnBehalfUser: Boolean, Storage: ReferencesStorage

### `ToMailField` (Namespace: `TFlex.DOCs.Model.Mail`)
**Методы:**
- `List`1 GetComparisonOperators()`
- `Object Parse(ServerConnection connection, String str, IFormatProvider provider, ComparisonOperator operator)`

### `AddressBookSettingReferenceObject` (Namespace: `TFlex.DOCs.Model.Mail.AddressBook`)
**Свойства:** Class: AddressBookSettingType, Name: StringParameter, ReferenceParameter: GuidParameter, AddressParameter: StringParameter, NameParameter: StringParameter

### `AddressBookSettingsReference` (Namespace: `TFlex.DOCs.Model.Mail.AddressBook`)
**Свойства:** Classes: AddressBookSettingsTypes

### `AddressBookSettingsTypes` (Namespace: `TFlex.DOCs.Model.Mail.AddressBook`)
**Свойства:** AddressBookSettingReferenceObject: AddressBookSettingType

### `AddressBookSettingType` (Namespace: `TFlex.DOCs.Model.Mail.AddressBook`)
**Свойства:** Classes: AddressBookSettingsTypes, IsAddressBookSettingReferenceObject: Boolean

### `Converter` (Namespace: `TFlex.DOCs.Model.Mail.Converters`)
**Методы:**
- `ConvertResponse Convert(ConvertRequest request, Object context) (+2)`
- `String GetEmpty()`

### `ConvertRequest` (Namespace: `TFlex.DOCs.Model.Mail.Converters`)
**Свойства:** Body: String, BodyType: MailBodyType, ToMailBodyType: MailBodyType
**Методы:**
- `Converter GetConverter()`
- `ConvertResponse Execute(Object context) (+1)`

### `ConvertResponse` (Namespace: `TFlex.DOCs.Model.Mail.Converters`)
**Свойства:** Failed: ConvertResponse, IsSuccessed: Boolean, Body: String, BodyType: MailBodyType, ConvertException: Exception

### `MailBodyTypeConverter` (Namespace: `TFlex.DOCs.Model.Mail.Converters`)
**Методы:**
- `ConvertResponse Convert(MailItem item, MailBodyType to, Object context) (+1)`
- `Converter Conveter(MailBodyType bodyType)`

### `MailItemReminder` (Namespace: `TFlex.DOCs.Model.Mail.Reminders`)
**Свойства:** Object: Object

### `ReferenceObjectReminder` (Namespace: `TFlex.DOCs.Model.Mail.Reminders`)
**Свойства:** Object: Object

### `Reminder` (Namespace: `TFlex.DOCs.Model.Mail.Reminders`)
**Свойства:** Connection: ServerConnection, Id: Int32, IsNew: Boolean, Date: DateTime, Text: String, Object: Object
**Методы:**
- `Void Save()`
- `Boolean Delete()` [has Async]

### `ReminderManager` (Namespace: `TFlex.DOCs.Model.Mail.Reminders`)
**Свойства:** Reminders: List`1, RemindersToShow: List`1
**Методы:**
- `ReferenceObjectReminder GetReminder(ReferenceObject refObj) (+1)`
- `Void WaitRemindThread(Object state)`

### `AdminAccessChangedCallback` (Namespace: `TFlex.DOCs.Model.Notification`)
**Методы:**
- `Void Invoke()`
- `IAsyncResult BeginInvoke(AsyncCallback callback, Object object)`
- `Void EndInvoke(IAsyncResult result)`

### `EventWatcher` (Namespace: `TFlex.DOCs.Model.Notification`)
**Свойства:** Connection: ServerConnection
**Методы:**
- `EventWatcherLock WatchCreatedObjects(ReferenceInfo referenceInfo, ISynchronizeInvoke synchronizeInvoke, ObjectCreatedCallback callback) (+7)` [has Async]
- `EventWatcherLock WatchChangedObjects(ReferenceInfo referenceInfo, ISynchronizeInvoke synchronizeInvoke, ObjectCreatedCallback callback) (+7)` [has Async]
- `EventWatcherLock WatchDeletedObjects(ReferenceInfo referenceInfo, ISynchronizeInvoke synchronizeInvoke, ObjectCreatedCallback callback) (+7)` [has Async]
- `EventWatcherLock WatchReferenceChanged(ReferenceInfo referenceInfo, ISynchronizeInvoke synchronizeInvoke, ObjectCreatedCallback callback) (+1)` [has Async]
- `EventWatcherLock WatchReferencesChanged(IEnumerable`1 references, ISynchronizeInvoke synchronizeInvoke, ObjectCreatedCallback callback)` [has Async]
- `EventWatcherLock WatchReferenceCatalogChanged(ISynchronizeInvoke synchronizeInvoke, ReferenceChangedCallback callback)` [has Async]
- `EventWatcherLock WatchReferenceCatalogChangedDebug(ISynchronizeInvoke synchronizeInvoke, ReferenceChangedCallbackDebug callback)` [has Async]
- `EventWatcherLock WatchAccessChangedChanged(ISynchronizeInvoke synchronizeInvoke, AdminAccessChangedCallback callback)` [has Async]
- `EventWatcherLock WatchForCreatedObjects(ReferenceInfo referenceInfo, ISynchronizeInvoke synchronizeInvoke, ObjectCreatedCallback callback) (+3)`
- `EventWatcherLock WatchForChangedObjects(ReferenceInfo referenceInfo, ISynchronizeInvoke synchronizeInvoke, ObjectCreatedCallback callback) (+3)`
- `EventWatcherLock WatchForDeletedObjects(ReferenceInfo referenceInfo, ISynchronizeInvoke synchronizeInvoke, ObjectCreatedCallback callback) (+3)`
- `EventWatcherLock WatchForReferenceChanged(ReferenceInfo referenceInfo, ISynchronizeInvoke synchronizeInvoke, ObjectCreatedCallback callback) (+1)`
- `EventWatcherLock WatchForReferencesChanged(IEnumerable`1 references, ISynchronizeInvoke synchronizeInvoke, ObjectCreatedCallback callback)`

### `EventWatcherLock` (Namespace: `TFlex.DOCs.Model.Notification`)
**Свойства:** IsAlive: Boolean
**Методы:**
- `ValueTask StopWatching(CancellationToken token)`

### `ObjectCreatedCallback` (Namespace: `TFlex.DOCs.Model.Notification`)
**Методы:**
- `Void Invoke(Int32 referenceId, Int32 objectId, Int32 clientView)`
- `IAsyncResult BeginInvoke(Int32 referenceId, Int32 objectId, Int32 clientView, AsyncCallback callback, Object object)`
- `Void EndInvoke(IAsyncResult result)`

### `ObjectWithGuidCreatedallback` (Namespace: `TFlex.DOCs.Model.Notification`)
**Методы:**
- `Void Invoke(Guid referenceGuid, Guid objectGuid, Int32 clientView)`
- `IAsyncResult BeginInvoke(Guid referenceGuid, Guid objectGuid, Int32 clientView, AsyncCallback callback, Object object)`
- `Void EndInvoke(IAsyncResult result)`

### `ReferenceChangedCallback` (Namespace: `TFlex.DOCs.Model.Notification`)
**Методы:**
- `Void Invoke(Int32 referenceId, ChangeType changeType, Int32 clientView)`
- `IAsyncResult BeginInvoke(Int32 referenceId, ChangeType changeType, Int32 clientView, AsyncCallback callback, Object object)`
- `Void EndInvoke(IAsyncResult result)`

### `ReferenceChangedCallbackDebug` (Namespace: `TFlex.DOCs.Model.Notification`)
**Методы:**
- `Void Invoke(Int32 referenceId, ChangeType changeType, Int32 clientView, String callStack)`
- `IAsyncResult BeginInvoke(Int32 referenceId, ChangeType changeType, Int32 clientView, String callStack, AsyncCallback callback, Object object)`
- `Void EndInvoke(IAsyncResult result)`

### `EventSourceExtension` (Namespace: `TFlex.DOCs.Model.Notification.ServerEventHandlers`)
**Методы:**
- `String GetText(EventSource type)`

### `EventTypeExtension` (Namespace: `TFlex.DOCs.Model.Notification.ServerEventHandlers`)
**Методы:**
- `String GetText(EventType type)`

### `ServerEventHandlerCondition` (Namespace: `TFlex.DOCs.Model.Notification.ServerEventHandlers`)
**Свойства:** Parameter: ParameterInfo, LinkGroup: ParameterGroup, Condition: Int32, Value: Object

### `MessageRecipient` (Namespace: `TFlex.DOCs.Model.Notification.ServerTasks`)
**Свойства:** RecipientType: MessageRecipientType, Value: String

### `RegularTimeTrigger` (Namespace: `TFlex.DOCs.Model.Notification.ServerTasks`)
**Свойства:** Interval: TimeSpan, Ticks: Int64, Name: String
**Методы:**
- `DateTime GetExecuteTime(DateTime lastExecuteTime)`

### `SendMessageTaskAction` (Namespace: `TFlex.DOCs.Model.Notification.ServerTasks`)
**Свойства:** MessageRecipients: List`1, Administrators: List`1, MessageTemplateId: Guid, AttachObject: Boolean, Name: String
**Методы:**
- `TaskActionResult Execute(MacroContext context)`

### `ServerTask` (Namespace: `TFlex.DOCs.Model.Notification.ServerTasks`)
**Свойства:** Connection: ServerConnection, Id: Int32, Guid: Guid, Name: String, Comment: String, Enabled: Boolean, User: User, Trigger: Trigger, Action: TaskAction, LastSuccessExecuteTime: DateTime, LastExecuteTime: DateTime, NextExecuteTime: DateTime, LastExecuteResult: String, ExecuteCount: Int32, IsAdded: Boolean, IsModified: Boolean, IsDeleted: Boolean, IsChanged: Boolean, Changing: Boolean
**Методы:**
- `Void BeginChanges()`
- `Void CancelChanges()`
- `Boolean Save()` [has Async]
- `Boolean Delete()` [has Async]

### `ServerTaskManager` (Namespace: `TFlex.DOCs.Model.Notification.ServerTasks`)
**Свойства:** Connection: ServerConnection
**Методы:**
- `ServerTask GetTask(Int32 id) (+1)` [has Async]
- `List`1 GetTasks()` [has Async]
- `ServerTask CreateTask()`
- `Boolean DeleteTasks(ICollection`1 tasks)` [has Async]
- `ServerEventHandler GetEventHandler(Guid handlerId)` [has Async]
- `List`1 GetEventHandlers(Boolean reload)` [has Async]
- `ServerEventHandler CreateEventHandler()`
- `Boolean SaveEventHandler(ServerEventHandler eventHandler)` [has Async]
- `Boolean DeleteEventHandler(ServerEventHandler eventHandler)` [has Async]
- `Void ImportEventHandlers(Stream stream)` [has Async]
- `Void ExportEventHandlers(Stream stream, IEnumerable`1 handlers)` [has Async]
- `List`1 GetRaisedServerEvents(DateTime dateTime)` [has Async]
- `List`1 GetNewOnsetDateRaisedEvents()` [has Async]

### `ServerTaskServiceData` (Namespace: `TFlex.DOCs.Model.Notification.ServerTasks`)
**Свойства:** LoadedTasksFunc: Func`1, FindTaskByIdFunc: Func`2, FindTaskByGuidFunc: Func`2, GetIsSoppedFunc: Func`1, UpdateTaskFunc: Action`2, LoggingProcessTasks: Boolean
**Методы:**
- `Void RaiseTaskChanged(ServerTask task)`
- `Void RaiseTaskDeleted(Int32[] idCollection, Guid[] guidCollection)`
- `Void RaiseStopping()`

### `TaskAction` (Namespace: `TFlex.DOCs.Model.Notification.ServerTasks`)
**Свойства:** Name: String
**Методы:**
- `TaskActionResult Execute(MacroContext context)`

### `TaskActionContext` (Namespace: `TFlex.DOCs.Model.Notification.ServerTasks`)
**Свойства:** Task: ServerTask, ExecuteTime: DateTime, MacroTaskActionData: String

### `TaskActionResult` (Namespace: `TFlex.DOCs.Model.Notification.ServerTasks`)
**Свойства:** Result: String, Success: Boolean

### `TaskActionTypeExtension` (Namespace: `TFlex.DOCs.Model.Notification.ServerTasks`)
**Методы:**
- `String GetText(TaskActionType type)`

### `Trigger` (Namespace: `TFlex.DOCs.Model.Notification.ServerTasks`)
**Свойства:** Name: String
**Методы:**
- `DateTime GetExecuteTime(DateTime lastExecuteTime)`

### `ISystemTaskActionExecuter` (Namespace: `TFlex.DOCs.Model.Notification.ServerTasks.Handlers`)
**Методы:**
- `String Execute(MacroContext context)`

### `MacroTaskAction` (Namespace: `TFlex.DOCs.Model.Notification.ServerTasks.Handlers`)
**Свойства:** Connection: ServerConnection, MacroGuid: Guid, Macro: Macro, EntryPoint: String, Data: String, Name: String
**Методы:**
- `TaskActionResult Execute(MacroContext context)`

### `SystemTaskAction` (Namespace: `TFlex.DOCs.Model.Notification.ServerTasks.Handlers`)
**Свойства:** TypeName: String, ActionExecuterType: Type, Name: String
**Методы:**
- `TaskActionResult Execute(MacroContext context)`

### `DayTrigger` (Namespace: `TFlex.DOCs.Model.Notification.ServerTasks.Triggers`)
**Свойства:** DayInterval: Int32, Name: String
**Методы:**
- `DateTime GetExecuteTime(DateTime lastExecuteTime)`

### `ImmediatelyTrigger` (Namespace: `TFlex.DOCs.Model.Notification.ServerTasks.Triggers`)
**Свойства:** Name: String
**Методы:**
- `DateTime GetExecuteTime(DateTime lastExecuteTime)`

### `MonthTrigger` (Namespace: `TFlex.DOCs.Model.Notification.ServerTasks.Triggers`)
**Свойства:** Months: Boolean[], MonthDays: Boolean[], LastDay: Boolean, Name: String
**Методы:**
- `DateTime GetExecuteTime(DateTime lastExecuteTime)`

### `ReferenceTrigger` (Namespace: `TFlex.DOCs.Model.Notification.ServerTasks.Triggers`)
**Свойства:** Name: String
**Методы:**
- `DateTime GetExecuteTime(DateTime lastExecuteTime)`

### `TimeTrigger` (Namespace: `TFlex.DOCs.Model.Notification.ServerTasks.Triggers`)
**Свойства:** StartTime: DateTime, Name: String
**Методы:**
- `DateTime GetExecuteTime(DateTime lastExecuteTime)`

### `WeekTrigger` (Namespace: `TFlex.DOCs.Model.Notification.ServerTasks.Triggers`)
**Свойства:** Item: Boolean, Days: Boolean[], WeekInterval: Int32, Name: String
**Методы:**
- `DateTime GetExecuteTime(DateTime lastExecuteTime)`

### `IObjectNode` (Namespace: `TFlex.DOCs.Model.ObjectsComparison`)
**Свойства:** DesktopObject: DesktopObject, Version: Int32
**Методы:**
- `Void AddParameterGroup(IParameterGroupNode node)`
- `ObjectValue GetParameterValue(String parameterPath)`
- `IEnumerable`1 GetRelationObjects(Guid ruleRelationGuid) (+1)`
- `IEnumerable`1 GetParameterGroups()`
- `IEnumerable`1 GetRelations()`
- `Void AddRelation(IRelationNode relation)`
- `Void AddRelationObject(IRelationNode relationNode, IObjectNode node)`
- `Boolean HasParameterGroup(String parameterGroupUniqueId)`
- `Boolean HasRelation(String relationUniqueId)`

### `IObjectsComparisonNode` (Namespace: `TFlex.DOCs.Model.ObjectsComparison`)
**Свойства:** UniqueId: String, Name: String, State: State

### `IParameterGroupNode` (Namespace: `TFlex.DOCs.Model.ObjectsComparison`)
**Методы:**
- `Void AddParameter(IParameterNode parameterNode)`
- `IEnumerable`1 GetParameters()`

### `IParameterNode` (Namespace: `TFlex.DOCs.Model.ObjectsComparison`)
**Свойства:** ParameterGroup: IParameterGroupNode, Value: Object, Diff: String

### `IRelationNode` (Namespace: `TFlex.DOCs.Model.ObjectsComparison`)
**Методы:**
- `IEnumerable`1 GetObjects()`
- `Void AddRelationObject(IObjectNode node)`

### `ObjectsComparisonExt` (Namespace: `TFlex.DOCs.Model.ObjectsComparison`)
**Методы:**
- `String GetComparisonKey(ReferenceObject referenceObject) (+2)`

### `ObjectsComparisonSettings` (Namespace: `TFlex.DOCs.Model.ObjectsComparison`)
**Свойства:** ComparisonRules: ComparisonRuleBase
**Методы:**
- `Void Add(ComparisonRuleBase rule)`
- `Void Delete(ComparisonRuleBase rule)`
- `ObjectsComparisonSettings CreateFullCopy()`

### `ObjectsForComparisonStorage` (Namespace: `TFlex.DOCs.Model.ObjectsComparison`)
**Свойства:** ReferenceGroup: ParameterGroup, ReferenceGuid: Guid, ObjectsForComparison: IReadOnlyList`1, NeedToCompare: Boolean
**Методы:**
- `Void Add(DesktopObject referenceObject)`
- `Void InsertAt(Int32 index, DesktopObject referenceObject)`
- `Void Remove(DesktopObject referenceObject)`
- `Void RemoveAt(Int32 index)`
- `Void Swap(Int32 firstIndex, Int32 secondIndex)`
- `Void SetSettings(ObjectsComparisonSettings settings)`
- `ObjectsComparisonSettings GetSettings()`
- `IList`1 GetDiff()` [has Async]
- `Void UpdatePositions(ReferenceObject[] referenceObjects, Int32[] positions)`

### `ComparisonRuleBase` (Namespace: `TFlex.DOCs.Model.ObjectsComparison.ComparisonRules`)
**Свойства:** Icon: IconImage, Key: String, Name: String, ReferencePath: String, Parent: ComparisonRuleBase, ChildrenRules: List`1
**Методы:**
- `Void Add(ComparisonRuleBase comparisonRule)`
- `Void Remove(ComparisonRuleBase comparisonRule)`
- `ComparisonRuleBase CreateShallowCopy()`
- `IObjectsComparisonNode Run(IObjectNode left, IObjectNode right)`

### `ComparisonRuleFactory` (Namespace: `TFlex.DOCs.Model.ObjectsComparison.ComparisonRules`)
**Методы:**
- `ComparisonRuleBase Create(ParameterInfo parameterInfo) (+1)`

### `ComparisonRulesRoot` (Namespace: `TFlex.DOCs.Model.ObjectsComparison.ComparisonRules`)
**Методы:**
- `IObjectsComparisonNode Run(IObjectNode left, IObjectNode right)`

### `ParameterGroupRule` (Namespace: `TFlex.DOCs.Model.ObjectsComparison.ComparisonRules`)
**Свойства:** Guid: Guid
**Методы:**
- `IObjectsComparisonNode Run(IObjectNode left, IObjectNode right)`

### `ParameterRule` (Namespace: `TFlex.DOCs.Model.ObjectsComparison.ComparisonRules`)
**Свойства:** Guid: Guid, ParameterInfo: ParameterInfo
**Методы:**
- `IObjectsComparisonNode Run(IObjectNode left, IObjectNode right)`

### `RelationRule` (Namespace: `TFlex.DOCs.Model.ObjectsComparison.ComparisonRules`)
**Свойства:** Guid: Guid
**Методы:**
- `Void Add(ComparisonRuleBase comparisonRule)`
- `Void Remove(ComparisonRuleBase comparisonRule)`
- `IObjectsComparisonNode Run(IObjectNode left, IObjectNode right)`
- `ParameterGroup FindParameterGroup(ParameterGroup mainGroup)`

### `SwappedRelationRule` (Namespace: `TFlex.DOCs.Model.ObjectsComparison.ComparisonRules`)
**Методы:**
- `ParameterGroup FindParameterGroup(ParameterGroup mainGroup)`

### `ApplicabilityColumnData` (Namespace: `TFlex.DOCs.Model.ObjectStructure`)
**Свойства:** IsEmpty: Boolean, Type: ColumnDataType

### `BaseColumnData` (Namespace: `TFlex.DOCs.Model.ObjectStructure`)
**Свойства:** Owner: StructureGroupSettings, Type: ColumnDataType, IsEmpty: Boolean
**Методы:**
- `BaseColumnData CreateEmpty(ColumnDataType type)`
- `Void SetInternalOwner(StructureGroupSettings owner)`

### `BasePathColumnData` (Namespace: `TFlex.DOCs.Model.ObjectStructure`)
**Методы:**
- `Void SetPathFuncs(Func`3 deserialize, Func`2 convert, Func`2 serialize, Func`2 masterGroupGetterFunc, Func`2 outputGroupGetter, Func`3 filterAdder)`

### `ChildrenObjectsStructure` (Namespace: `TFlex.DOCs.Model.ObjectStructure`)
**Свойства:** ShowAddCommandLock: Nullable`1, ShowRemoveCommandLock: Nullable`1, IsLinkOrParameterGroup: Boolean, Type: StructureGroupTypes
**Методы:**
- `String AppendToPath(String path, HierarchyDirections hierarchyDirection, String pathItem) (+1)`

### `ClassObjectStructure` (Namespace: `TFlex.DOCs.Model.ObjectStructure`)
**Свойства:** ShowEmptyFolderLock: Nullable`1, ShowFolderLock: Nullable`1, ShowCreateCommandLock: Nullable`1, ShowAddCommandLock: Nullable`1, ShowDeleteCommandLock: Nullable`1, ShowRemoveCommandLock: Nullable`1, CanHidedLock: Nullable`1, Type: StructureGroupTypes, IsLinkOrParameterGroup: Boolean
**Методы:**
- `String AppendToPath(String path, StructureGroup structureGroup)`

### `ComplexHierarchyLinkStructure` (Namespace: `TFlex.DOCs.Model.ObjectStructure`)
**Свойства:** IsLinkOrParameterGroup: Boolean, Type: StructureGroupTypes
**Методы:**
- `String AppendToPath(String path, HierarchyDirections hierarchyDirection, String pathItem) (+1)`

### `DefaultVisibleParameterColumnData` (Namespace: `TFlex.DOCs.Model.ObjectStructure`)
**Свойства:** DisplayMask: String, Type: ColumnDataType, IsEmpty: Boolean
**Методы:**
- `ReferencePath GetParameterPath(ParameterGroup parameterGroup)`

### `DynamicColumnData` (Namespace: `TFlex.DOCs.Model.ObjectStructure`)
**Свойства:** Source: String, Filter: String, Header: String, Value: String, ValueToString: String, DisplayMask: String, ValueSplitter: String, CreateObjectsOnEdit: Boolean, SummaryType: Int32, Type: ColumnDataType, IsEmpty: Boolean, SourcePath: Object, ValuePath: Object, ValueToStringPath: Object, HeaderPath: Object

### `GroupCache` (Namespace: `TFlex.DOCs.Model.ObjectStructure`)
**Методы:**
- `Void Clear()`
- `Boolean IsUnidirectionalLink(ParameterGroup group, ParameterGroup prefix)`
- `Boolean ContainsInClass(ClassObject classObject, Guid linkId, Boolean isToAny)`

### `MacroColumnData` (Namespace: `TFlex.DOCs.Model.ObjectStructure`)
**Свойства:** Formula: String, Type: ColumnDataType, IsEmpty: Boolean

### `NomenclatureLinkedObjectStructure` (Namespace: `TFlex.DOCs.Model.ObjectStructure`)
**Свойства:** Type: StructureGroupTypes, IsLinkOrParameterGroup: Boolean, ShowCreateCommandLock: Nullable`1, ShowAddCommandLock: Nullable`1, ShowDeleteCommandLock: Nullable`1, ShowRemoveCommandLock: Nullable`1, CanContainMultipleReferencesDetalization: Boolean
**Методы:**
- `String AppendToPath(String path, StructureGroup structureGroup)`

### `ObjectColumnData` (Namespace: `TFlex.DOCs.Model.ObjectStructure`)
**Свойства:** IsEmpty: Boolean, Type: ColumnDataType

### `ObjectStructureDataLoader` (Namespace: `TFlex.DOCs.Model.ObjectStructure`)
**Методы:**
- `List`1 LoadReference(Reference reference, StructureGroup parentGroup, Boolean loadDeleted, MacroContext filterContext, Filter filter)`
- `Boolean StructureGroupHasChildren(StructureGroup structureGroup, ReferenceObject parentObject, Boolean loadDeleted)`
- `Boolean ReferenceObjectHasChildren(ReferenceObject referenceObject, StructureGroup parentGroup, Boolean loadDeleted)`
- `List`1 LoadStructureGroup(StructureGroup structureGroup, ReferenceObject parentObject, Boolean loadDeleted)`

### `ParameterColumnData` (Namespace: `TFlex.DOCs.Model.ObjectStructure`)
**Свойства:** IsManualSetter: Boolean, CanSetGetter: String, ValueSetter: String, LinkAggregator: String, Parameter: String, DisplayMask: String, Type: ColumnDataType, IsEmpty: Boolean, ValueSetterPath: Object, CanSetGetterPath: Object, LinkAggregationPath: Object, ParameterPath: Object, SystemParameter: SystemParameterType
**Методы:**
- `T GetParameterPath(ParameterGroup group)`

### `ParameterGroupStructure` (Namespace: `TFlex.DOCs.Model.ObjectStructure`)
**Свойства:** Type: StructureGroupTypes, IsLinkOrParameterGroup: Boolean
**Методы:**
- `String AppendToPath(String path, StructureGroup structureGroup)`

### `PlanningCategoryColumnData` (Namespace: `TFlex.DOCs.Model.ObjectStructure`)
**Свойства:** IsEmpty: Boolean, PlanningCategory: Guid, Type: ColumnDataType

### `ReferenceColumnData` (Namespace: `TFlex.DOCs.Model.ObjectStructure`)
**Свойства:** IsEmpty: Boolean, Type: ColumnDataType

### `StructureColumn` (Namespace: `TFlex.DOCs.Model.ObjectStructure`)
**Свойства:** Id: Guid, Name: String, IsHidden: Boolean, Index: Int32, IsSystem: Boolean, IsDynamic: Boolean, AllowEdit: Boolean
**Методы:**
- `Void CopyPropertiesFrom(StructureColumn column)`
- `XmlSchema GetSchema()`
- `Void ReadXml(XmlReader reader)`
- `Void WriteXml(XmlWriter writer)`

### `StructureConfigurationData` (Namespace: `TFlex.DOCs.Model.ObjectStructure`)
**Свойства:** Connection: ServerConnection, Item: StructureGroup, Roots: IEnumerable`1, Groups: ReadOnlyCollection`1, Columns: ReadOnlyCollection`1, Variables: VariableCollection
**Методы:**
- `Void CreateDefaultStructure(ParameterGroup parameterGroup) (+1)`
- `StructureGroup CreateGroup(ParameterGroup parameterGroup, StructureGroupType type, StructureGroup parent, HierarchyDirections hierarchyDirection, String userPath) (+1)`
- `Void RemoveGroup(StructureGroup group)`
- `Void AddGroup(StructureGroup group, StructureGroup parentGroup)`
- `StructureColumn CreateColumn(Guid id, String columnName, Boolean isDynamic) (+1)`
- `Void RemoveColumn(Guid id) (+1)`
- `Boolean HasVisibleGroupSupportsDesignContexts()`
- `Void UpdateColumnIndexes()`
- `Void UpdateDynamicColumnData(StructureColumn column)`
- `String Serialize()`
- `StructureConfigurationData Deserialize(String xmlData, ServerConnection connection)`
- `XmlSchema GetSchema()`
- `Void ReadXml(XmlReader reader)`
- `Void WriteXml(XmlWriter writer)`

### `StructureGroup` (Namespace: `TFlex.DOCs.Model.ObjectStructure`)
**Свойства:** Owner: StructureConfigurationData, GroupCacheInfo: Object, GroupSettings: StructureGroupSettings, CanSupportOrder: Boolean, Filter: StructureGroupFilter, ExternalFilter: Filter, MergedFilter: Filter, UseParentColumns: Boolean, HasColumnsSettings: Boolean, IsChildrenWithParent: Boolean, IsEmpty: Boolean, Item: BaseColumnData, GroupId: Guid, ParameterGroup: ParameterGroup, Type: StructureGroupType, ReferencePath: String, ShowInStructure: Boolean, ClassId: Guid, Path: String, Parent: StructureGroup, AllParents: IEnumerable`1, Master: StructureGroup, Classes: IEnumerable`1, Children: IEnumerable`1, ContainActiveMultipleReferenceDetalization: Boolean, IsMultipleReferenceDetalization: Boolean, IsHidden: Boolean
**Методы:**
- `Boolean IsUnidirectionalLink(GroupCache groupCache)`
- `Void AddExtendedParameter(String key, Object value)`
- `Boolean RemoveExtendedParameter(String key)`
- `Boolean HasExtendedParameter(String key)`
- `Boolean TryGetExtendedParameter(String key, Object& value)`
- `Void RegisterExtendedParameterSerializer(String key, XmlSerializer serializer)`
- `List`1 GetDisplayChildGroups(DesktopObject referenceObject, GroupCache groupCache)`
- `StructureGroup CreateChild(ParameterGroup parameterGroup, StructureGroupType type, HierarchyDirections hierarchyDirection, String userPath) (+1)`
- `Void RemoveGroup(StructureGroup child)`
- `StructureGroup CreateClassStructure(Guid classId)`
- `Void Delete()`
- `Void DeleteWithChildren()`
- `StructureGroup GetClassStructure(ClassObject classObject)`
- `Void CopyPropertiesFrom(StructureGroup structureGroup)`
- `XmlSchema GetSchema()`
- `Void ReadXml(XmlReader reader)`
- `Void WriteXml(XmlWriter writer)`
- `Boolean CanContainStructureGroup(DesktopObject desktopObject, StructureGroup group, GroupCache groupCache)`

### `StructureGroupFilter` (Namespace: `TFlex.DOCs.Model.ObjectStructure`)
**Свойства:** Owner: StructureConfigurationData, Variables: VariableCollection

### `StructureGroupSettings` (Namespace: `TFlex.DOCs.Model.ObjectStructure`)
**Свойства:** Type: StructureGroupType, Owner: StructureConfigurationData, ParameterGroup: ParameterGroup, HierarchyDirection: HierarchyDirections, DisableConfigurationFilter: Boolean, Hidden: Boolean, ShowFolder: Boolean, ShowEmptyFolder: Boolean, ShowCreateCommand: Boolean, ShowDeleteCommand: Boolean, ShowAddCommand: Boolean, ShowRemoveCommand: Boolean, ShowReferenceCommands: Boolean, ShowDialogOnCreating: Nullable`1, SupportsOrder: Boolean, ShowReferenceFolders: Boolean, DefaultGroupIndex: Nullable`1, GroupIndex: Nullable`1, GlobalOrdering: Boolean, ShowCommandsFromRoot: Boolean, ObjectOrderPath: String, GroupFolderIconPath: String, VisibleToVarName: String, SelectedToVarName: String, ObjectOrderDirection: Nullable`1, UniversalPathOutputGroupGuid: Guid, RecursiveGroup: String, UserPath: String, GroupingPath: String, SystemFolderId: String, LinkGroupingKeyPath: String, UserFolderName: String, CustomIconGuid: Guid, Columns: Dictionary`2, Item: BaseColumnData
**Методы:**
- `Void RemoveColumn(Guid id)`
- `IEnumerator`1 GetEnumerator()`

### `StructureGroupType` (Namespace: `TFlex.DOCs.Model.ObjectStructure`)
**Свойства:** ShowFolderLock: Nullable`1, ShowEmptyFolderLock: Nullable`1, ShowCreateCommandLock: Nullable`1, ShowAddCommandLock: Nullable`1, ShowDeleteCommandLock: Nullable`1, ShowRemoveCommandLock: Nullable`1, CanHidedLock: Nullable`1, DisableConfigurationFilterLock: Nullable`1, IsLinkOrParameterGroup: Boolean, CanContainMultipleReferencesDetalization: Boolean, Type: StructureGroupTypes
**Методы:**
- `String AppendToPath(String path, HierarchyDirections hierarchyDirection, String pathItem) (+2)`

### `SwappedToManyLinkStructure` (Namespace: `TFlex.DOCs.Model.ObjectStructure`)
**Свойства:** Type: StructureGroupTypes, IsLinkOrParameterGroup: Boolean
**Методы:**
- `String AppendToPath(String path, HierarchyDirections hierarchyDirection, String pathItem) (+1)`

### `SwappedToOneLinkStructure` (Namespace: `TFlex.DOCs.Model.ObjectStructure`)
**Свойства:** Type: StructureGroupTypes, IsLinkOrParameterGroup: Boolean
**Методы:**
- `String AppendToPath(String path, HierarchyDirections hierarchyDirection, String pathItem) (+1)`

### `ToAnyStructure` (Namespace: `TFlex.DOCs.Model.ObjectStructure`)
**Свойства:** Type: StructureGroupTypes, IsLinkOrParameterGroup: Boolean, CanContainMultipleReferencesDetalization: Boolean
**Методы:**
- `String AppendToPath(String path, StructureGroup structureGroup)`

### `ToOneStructure` (Namespace: `TFlex.DOCs.Model.ObjectStructure`)
**Свойства:** Type: StructureGroupTypes, IsLinkOrParameterGroup: Boolean
**Методы:**
- `String AppendToPath(String path, StructureGroup structureGroup)`

### `UserFolderStructure` (Namespace: `TFlex.DOCs.Model.ObjectStructure`)
**Свойства:** ShowCreateCommandLock: Nullable`1, ShowDeleteCommandLock: Nullable`1, Type: StructureGroupTypes, IsLinkOrParameterGroup: Boolean
**Методы:**
- `String AppendToPath(String path, StructureGroup structureGroup)`

### `ToCharacteristicsLinkStructure` (Namespace: `TFlex.DOCs.Model.ObjectStructure.GroupTypes`)
**Свойства:** Type: StructureGroupTypes, ShowAddCommandLock: Nullable`1, ShowRemoveCommandLock: Nullable`1

### `BooleanParameter` (Namespace: `TFlex.DOCs.Model.Parameters`)
**Свойства:** IsEmpty: Boolean
**Методы:**
- `Boolean GetBoolean()`
- `TypeCode GetTypeCode()`

### `ByteArrayParameter` (Namespace: `TFlex.DOCs.Model.Parameters`)
**Методы:**
- `Byte[] GetByteArray()`
- `TypeCode GetTypeCode()`

### `ByteParameter` (Namespace: `TFlex.DOCs.Model.Parameters`)
**Методы:**
- `Byte GetByte()`
- `TypeCode GetTypeCode()`

### `ComplexHierarchyLinkParameters` (Namespace: `TFlex.DOCs.Model.Parameters`)
**Свойства:** Link: ComplexHierarchyLink

### `ComplexHierarchyLinkSystemFields` (Namespace: `TFlex.DOCs.Model.Parameters`)
**Свойства:** IsModified: Boolean, Id: Int32, Guid: Guid, IsPrimary: Boolean, AuthorName: String, AuthorId: Int32, Author: User, CreationDate: DateTime, EditorName: String, EditorId: Int32, Editor: User, EditDate: DateTime, StartDate: Nullable`1, EndDate: Nullable`1, DesignContextId: Int32, OriginalId: Int32, DeletedInDesignContext: Boolean, ConflictWithOriginal: ConflictWithOriginal, AutoSelectRevision: Boolean, StructureTypeId: Int32, StructureType: StructureTypesReferenceObject

### `DateTimeParameter` (Namespace: `TFlex.DOCs.Model.Parameters`)
**Свойства:** IsEmpty: Boolean
**Методы:**
- `DateTime GetDateTime()`
- `TypeCode GetTypeCode()`

### `DateTimeParameterOffsetExtensions` (Namespace: `TFlex.DOCs.Model.Parameters`)
**Методы:**
- `String GetDisplayText(DateTimeParameterOffset value, String nullText)`
- `String GetDateUnitString(Double unitValue, String singular, String plural, String genitive)`

### `DecimalParameter` (Namespace: `TFlex.DOCs.Model.Parameters`)
**Методы:**
- `Decimal GetDecimal()`
- `TypeCode GetTypeCode()`

### `DoubleParameter` (Namespace: `TFlex.DOCs.Model.Parameters`)
**Методы:**
- `Double GetDouble()`
- `TypeCode GetTypeCode()`

### `GuidParameter` (Namespace: `TFlex.DOCs.Model.Parameters`)
**Свойства:** IsEmpty: Boolean
**Методы:**
- `Guid GetGuid()`
- `TypeCode GetTypeCode()`
- `Boolean TryParse(String s, Guid& result)`

### `IconParameter` (Namespace: `TFlex.DOCs.Model.Parameters`)
**Свойства:** Value: IconImage, IsNull: Boolean
**Методы:**
- `IconImage GetIcon()`
- `Byte[] GetByteArray()`
- `TypeCode GetTypeCode()`

### `ImageParameter` (Namespace: `TFlex.DOCs.Model.Parameters`)
**Методы:**
- `Image GetImage()`
- `TypeCode GetTypeCode()`

### `Int16Parameter` (Namespace: `TFlex.DOCs.Model.Parameters`)
**Методы:**
- `Int16 GetInt16()`
- `TypeCode GetTypeCode()`

### `Int32Parameter` (Namespace: `TFlex.DOCs.Model.Parameters`)
**Методы:**
- `Int32 GetInt32()`
- `TypeCode GetTypeCode()`

### `Int64Parameter` (Namespace: `TFlex.DOCs.Model.Parameters`)
**Методы:**
- `Int64 GetInt64()`
- `TypeCode GetTypeCode()`

### `Parameter` (Namespace: `TFlex.DOCs.Model.Parameters`)
**Свойства:** Value: Object, EmptyValue: Object, IsEmpty: Boolean, IsNull: Boolean, IsModified: Boolean, IsReadOnly: Boolean, ParameterInfo: ParameterInfo, Owner: ParameterCollection, HasNullValueFromValueList: Boolean, HasValueOutsideUneditableValueList: Boolean
**Методы:**
- `Void SetNull()`
- `Void SetModified()`
- `Boolean GetBoolean()`
- `Byte GetByte()`
- `Int16 GetInt16()`
- `Int32 GetInt32()`
- `Int64 GetInt64()`
- `String GetString()`
- `Single GetSingle()`
- `Double GetDouble()`
- `Decimal GetDecimal()`
- `Guid GetGuid()`
- `Byte[] GetByteArray()`
- `DateTime GetDateTime()`
- `IconImage GetIcon()`
- `Image GetImage()`
- `Object GetValue(Type type) (+1)`
- `Boolean ToBoolean(IFormatProvider provider)`
- `Byte ToByte(IFormatProvider provider)`
- `Char ToChar(IFormatProvider provider)`
- `DateTime ToDateTime(IFormatProvider provider)`
- `Decimal ToDecimal(IFormatProvider provider)`
- `Double ToDouble(IFormatProvider provider)`
- `Int16 ToInt16(IFormatProvider provider)`
- `Int32 ToInt32(IFormatProvider provider)`
- `Int64 ToInt64(IFormatProvider provider)`
- `SByte ToSByte(IFormatProvider provider)`
- `Single ToSingle(IFormatProvider provider)`
- `Object ToType(Type conversionType, IFormatProvider provider)`
- `UInt16 ToUInt16(IFormatProvider provider)`
- `UInt32 ToUInt32(IFormatProvider provider)`
- `UInt64 ToUInt64(IFormatProvider provider)`
- `TypeCode GetTypeCode()`

### `Parameter`1` (Namespace: `TFlex.DOCs.Model.Parameters`)
**Свойства:** Value: T, EmptyValue: T, IsEmpty: Boolean, IsNull: Boolean

### `ReferenceObjectParameters` (Namespace: `TFlex.DOCs.Model.Parameters`)
**Свойства:** Object: ReferenceObject

### `ReferenceObjectSystemFields` (Namespace: `TFlex.DOCs.Model.Parameters`)
**Свойства:** Id: Int32, Guid: Guid, AuthorName: String, AuthorId: Int32, Author: User, OwnerId: Int32, Owner: UserReferenceObject, OnBehalfOfId: Int32, OnBehalfOf: UserReferenceObject, CredentialId: Int32, Credential: CredentialsReferenceObject, CreationDate: DateTime, EditorName: String, EditorId: Int32, Editor: User, EditDate: DateTime, StageEditDate: DateTime, Version: Int32, Deleted: Boolean, ClientViewId: Int32, ClientView: ClientView, Order: Nullable`1, StageId: Int32, Stage: SchemeStage, IsLinkedToNomenclature: Boolean, LogicalObjectGuid: Guid, RevisionName: String, LastRevisionName: String, SourceRevisionName: String, IsActualRevision: Boolean, StartDate: Nullable`1, EndDate: Nullable`1, DesignContextId: Int32, OriginalId: Int32, DeletedInDesignContext: Boolean, ConflictWithOriginal: ConflictWithOriginal, MasterServerId: Int32, MasterServer: ReferenceObject, IsRevisionsContainer: Boolean, StructureTypeId: Int32, StructureType: StructureTypesReferenceObject, InstanceGroupGuid: Guid, InstanceMappingType: InstancesMappingType, InstanceSequenceNumbers: String, IsStandaloneProduct: Boolean

### `SingleParameter` (Namespace: `TFlex.DOCs.Model.Parameters`)
**Методы:**
- `Single GetSingle()`
- `TypeCode GetTypeCode()`

### `SpecialComplexHierarchyLinkParameters`1` (Namespace: `TFlex.DOCs.Model.Parameters`)
**Свойства:** Link: TComplexHierarchyLink

### `StringParameter` (Namespace: `TFlex.DOCs.Model.Parameters`)
**Свойства:** IsEmpty: Boolean
**Методы:**
- `String GetString()`
- `Guid GetGuid()`
- `TypeCode GetTypeCode()`

### `SystemParameter` (Namespace: `TFlex.DOCs.Model.Parameters`)
**Свойства:** IsReadOnly: Boolean, IsNull: Boolean
**Методы:**
- `TypeCode GetTypeCode()`

### `AssemblyLoader` (Namespace: `TFlex.DOCs.Model.Plugins`)
**Свойства:** Plugins: ReadOnlyCollection`1
**Методы:**
- `T GetClassObject()`

### `Context` (Namespace: `TFlex.DOCs.Model.Plugins`)
**Свойства:** Default: Boolean, ContextUniqueString: String
**Методы:**
- `Boolean BaseEquals(Context context)`

### `HtmlHelper` (Namespace: `TFlex.DOCs.Model.Plugins`)
**Методы:**
- `String InsertFirstString(String htmlString, String str) (+1)`
- `String InsertLastString(String htmlString, String str)`
- `String GetHtmlBodyText(String htmlString)`
- `String ReplaceNewLineSymbols(String str)`
- `String InsertIntoEmptyHtml(String str)`

### `ImageExtensions` (Namespace: `TFlex.DOCs.Model.Plugins`)
**Методы:**
- `Icon RenderToIcon(Image image) (+1)`
- `Image GetResizedImage(Image source, Int32 width, Int32 height)`

### `IPluginLibrary` (Namespace: `TFlex.DOCs.Model.Plugins`)
**Методы:**
- `Void RegisterPlugin()`
- `Void OnCreatingVisualRepresentation(IModelVisualRepresentation VisualRepresentation)`
- `Void OnCreatingReferenceVisualRepresentation(Reference reference, IModelVisualRepresentation VisualRepresentation)`

### `ObjectCreator` (Namespace: `TFlex.DOCs.Model.Plugins`)
**Методы:**
- `Boolean IsExistObjectForContext(Context context, Boolean findInAllAssemblies) (+1)`
- `Object CreateObject(Type createType, Context context) (+4)`
- `Boolean TryCreateObject(Context context, T& createdObject)`
- `Object CreateObjectByFoundContext(Type createType, Guid reference, ClassObject classObject) (+3)`

### `PluginInterfacesMap` (Namespace: `TFlex.DOCs.Model.Plugins`)
**Свойства:** Map: PluginInterfacesMap
**Методы:**
- `Void RegisterPluginType(Type interfaceType, Context context, Type classType) (+1)`

### `ReferenceContext` (Namespace: `TFlex.DOCs.Model.Plugins`)
**Свойства:** ClassObject: Guid
**Методы:**
- `ReferenceContext GetOrCreate(Guid referenceGuid)`

### `ReferenceInfoHelper` (Namespace: `TFlex.DOCs.Model.Plugins`)
**Методы:**
- `Boolean CheckReferenceIsDeactivating(ReferenceInfo referenceInfo) (+1)`
- `Boolean CheckDeactivationWasCancelled(ReferenceInfo referenceInfo) (+1)`
- `Void CancelDeactivation(ReferenceInfo referenceInfo) (+1)`
- `Void AddActiveTask(ReferenceInfo referenceInfo, Task task) (+1)`

### `Application` (Namespace: `TFlex.DOCs.Model.Plugins.Applications`)
**Свойства:** AssemblyPath: String, Type: ApplicationType, ReadOnly: Boolean, Guid: Guid, Name: String, Description: String

### `ApplicationsManager` (Namespace: `TFlex.DOCs.Model.Plugins.Applications`)
**Свойства:** Connection: ServerConnection, Applications: ICollection`1
**Методы:**
- `Application CreateNewApplication(ApplicationType type)`
- `Void DeleteApplication(Application application)`
- `Void SaveApplication(Application application)`
- `Void LoadApplications(Action`1 exceptionCallback, IEnumerable`1 applicationGuids) (+1)`
- `IEnumerable`1 GetStoredApplications()`
- `IEnumerable`1 GetLocalApplications()`
- `IEnumerable`1 GetSystemApplications()`
- `IEnumerable`1 GetSystemApplicationsModels()`
- `IEnumerable`1 GetSystemApplicationsUIs()`
- `IEnumerable`1 GetSystemApplicationsUIClient()`
- `IEnumerable`1 GetSystemApplicationsUniClient()`

### `CommonApplication` (Namespace: `TFlex.DOCs.Model.Plugins.Applications`)
**Свойства:** Connection: ServerConnection, EventServiceApplication: Boolean, FileObjectId: Guid, Configurations: List`1, ConfigurationUseType: ConfigurationUseType, Guid: Guid, Name: String, Description: String, AssemblyPath: String, ReadOnly: Boolean, Type: ApplicationType

### `LocalApplication` (Namespace: `TFlex.DOCs.Model.Plugins.Applications`)
**Свойства:** ReadOnly: Boolean, Type: ApplicationType

### `IAssignmentReferenceHelper` (Namespace: `TFlex.DOCs.Model.Plugins.Processes`)
**Свойства:** DisableUserCreateObject: Boolean

### `IProjectsHelper` (Namespace: `TFlex.DOCs.Model.Plugins.Projects`)
**Методы:**
- `Void OpenWorkWindow(ReferenceObject work, IntPtr handle)`

### `TechnologyPlugin` (Namespace: `TFlex.DOCs.Model.Plugins.Technology`)
**Методы:**
- `List`1 UpdateOperationNumbers(ReferenceObject techProcess, Int32 firstNumber, Int32 numerationStep, Int32 numberLength) (+1)`
- `List`1 UpdateStepNumbers(ReferenceObject techOperation, Int32 firstNumber, Int32 numerationStep, Int32 numberLength) (+1)`
- `List`1 GetToleranceLetters(ServerConnection connection, Int32 group) (+2)`
- `List`1 GetToleranceDigits(ServerConnection connection, Int32 group) (+2)`
- `List`1 GetToleranceDigitsInUse(ServerConnection connection, Double dimension, String letter, Int32 group) (+2)`
- `List`1 GetTolerance(ServerConnection connection, Double dimension, String letter, Int32 digit, Int32 group) (+2)`

### `ToleranceModel` (Namespace: `TFlex.DOCs.Model.Plugins.Technology`)
**Свойства:** BasicSize: Double, EngineeringFit: String, LowerDeviation: Double, UpperDeviation: Double, Quality: Int32
**Методы:**
- `List`1 GetToleranceLetters(ServerConnection connection, Int32 group) (+1)`
- `List`1 GetToleranceDigits(ServerConnection connection, Int32 group) (+1)`
- `List`1 GetToleranceDigitsInUse(ServerConnection connection, Double dimension, String letter, Int32 group) (+1)`
- `List`1 GetTolerance(ServerConnection connection, Double dimension, String letter, Int32 digit, Int32 group) (+1)`

### `ReferenceRestrictiveListClassGroupSettings` (Namespace: `TFlex.DOCs.Model.ReferenceRestrictiveLists`)
**Свойства:** ReferencesUseType: ItemListUseType, Except: List`1
**Методы:**
- `Void Deserialize(String data)`
- `String Serialize()`

### `AnyReferenceLoadSettings` (Namespace: `TFlex.DOCs.Model.References`)
**Методы:**
- `Boolean Add(ParameterInfo parameter)`

### `ChunkObjectsArgs` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** Empty: ChunkObjectsArgs, Part: Int32, Objects: List`1, HierarchyLinks: List`1

### `ComplexHierarchyLink` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** ParameterValues: ParameterCollection, SystemFields: ComplexHierarchyLinkSystemFields, IsModified: Boolean, Reference: Reference, Id: Int32, Guid: Guid, ParentObject: ReferenceObject, ChildObject: ReferenceObject, ParentObjectId: Int32, ChildObjectId: Int32, RevisionsGroupId: Int32, IsAdded: Boolean, IsNew: Boolean, IsDeleted: Boolean, Prototype: ComplexHierarchyLink, SaveSet: ComplexHierarchyLinkSaveSet, CanEdit: Boolean, HasEditableStructure: Boolean, CanDelete: Boolean, SaveWith: ReferenceObject, Changing: Boolean, IsCheckedOut: Boolean, IsCheckedOutByCurrentUser: Boolean, CanCheckOut: Boolean, CanCheckIn: Boolean, CanUndoCheckOut: Boolean, LockState: ReferenceObjectLockState, IsInRecycleBin: Boolean, IsPrimary: Boolean
**Методы:**
- `Boolean SetPrimary()`
- `Boolean SetRevisionsGroupId(Int32 revisionsGroupId)`
- `Void BeginChanges()` [has Async]
- `Boolean EndChanges(ReferenceObjectInstance sourceStructureInstance, ReferenceObjectInstance parentObjectInstance, ReferenceObjectInstance baseInstance) (+1)` [has Async]
- `Void CancelChanges()` [has Async]
- `ObjectValue GetObjectValue(ReferencePath path, PathCalculationSettings settings, Boolean throwOnError)`
- `Void UpdateRelevantParameters(ComplexHierarchyLink sourceHierarchyLink, CopyReferenceObjectsContext copyContext) (+1)`
- `Void UpdateFromLink(ComplexHierarchyLink sourceHierarchyLink, Boolean copyParameters, CopyReferenceObjectsContext copyContext, Boolean copyApplicability) (+1)`
- `ParameterGroup FindRelation(Guid groupGuid)`
- `Boolean ContainsRelation(Int32 groupId)`
- `ComplexHierarchyLink CreateCopy(ReferenceObject newParent, ReferenceObject newChild)`
- `Boolean BelongsToConfigurationSettings()`
- `String GetGroupingKey()`

### `ComplexHierarchyLinkExtensions` (Namespace: `TFlex.DOCs.Model.References`)
**Методы:**
- `Void ModifyLink(TObject hierarchyLink, Action`1 action, Boolean cancelOnError)`
- `ComplexHierarchyLink CopyAllTo(ComplexHierarchyLink hierarchyLink, Reference target)`

### `ComplexHierarchyLinkInstanceData` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** HierarchyLink: ComplexHierarchyLink, SourceStructureObjectInstance: ReferenceObjectInstance, ParentObjectInstance: ReferenceObjectInstance, BaseInstance: ReferenceObjectInstance
**Методы:**
- `Boolean EndChanges()` [has Async]

### `ComplexHierarchyLinkInstancesSaveSet` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** Count: Int32
**Методы:**
- `Void AddRange(IEnumerable`1 hierarchyLinks) (+1)`
- `Void Clear()`
- `Boolean Remove(ComplexHierarchyLink item)`
- `Void CancelChanges()` [has Async]
- `Boolean EndChanges()` [has Async]
- `Void Add(ComplexHierarchyLink hierarchyLink) (+1)` [has Async]
- `Boolean Contains(ComplexHierarchyLink hierarchyLink)`
- `Void CopyTo(ComplexHierarchyLink[] array, Int32 arrayIndex)`
- `IEnumerator`1 GetEnumerator()`

### `ComplexHierarchyLinkSaveSet` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** Changing: Boolean, Count: Int32, IsReadOnly: Boolean
**Методы:**
- `Void AddRange(IEnumerable`1 hierarchyLinks)`
- `Void Clear()`
- `Boolean Remove(ComplexHierarchyLink item)`
- `Void CancelChanges()` [has Async]
- `Boolean EndChanges()` [has Async]
- `Void Add(ComplexHierarchyLink hierarchyLink)` [has Async]
- `Boolean Contains(ComplexHierarchyLink hierarchyLink)`
- `Void CopyTo(ComplexHierarchyLink[] array, Int32 arrayIndex)`
- `IEnumerator`1 GetEnumerator()`

### `ComplexHierarchyLinkValue` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** IsHierarchyLink: Boolean, Link: ComplexHierarchyLink

### `ConfigurationSettings` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** Connection: ServerConnection, TypicalConfiguration: TypicalConfigurationSettingsObject, ApplyProductFilter: Boolean, Product: ProductsClassifierReferenceObject, ApplyProductDesignNumber: Boolean, ApplyProductMilestoneNumber: Boolean, ProductDesignNumber: Nullable`1, ProductMilestoneNumber: Nullable`1, SerialNumber: ProductsInstancesReferenceObject, ApplyOptionValues: Boolean, OptionValues: List`1, ApplyDesignContext: Boolean, DesignContext: DesignContextObject, ActiveDesignContext: DesignContextObject, ShowDeletedInDesignContextLinks: Boolean, ApplyDate: Boolean, Date: Object, ApplyStructureType: Boolean, ActiveStructure: StructureTypesReferenceObject, ApplyStructureTypes: Boolean, ActiveStructures: StructureTypesData, VisibleStructures: IEnumerable`1, EditableStructures: IEnumerable`1, CustomCriteriaValues: List`1, SelectRevisionsFilterCriteriaValue: CustomCriteriaValue, FilterDate: Nullable`1, ApplyStructureVariants: Boolean, StructureVariants: StructureVariantFilterData, ApplyCategoriesFilter: Boolean, Categories: CategoriesFilterData, ApplyTerms: Boolean, SelectRevisionsTermsObject: ReferenceFiltersGroupObject, SelectRevisionsTerms: FilterDataCollection
**Методы:**
- `Boolean ValidateDesignContextEdit(Boolean throwOnError)`
- `Boolean ValidateDateEdit(Boolean throwOnError)`
- `Void AppendToFilter(Filter filter, ParameterGroup linkGroup)` [has Async]
- `Void AppendToInstanceFilter(Filter filter, ParameterGroup linkGroup, CancellationToken token)` [has Async]
- `List`1 GetExcludedByApplicabilityObjectIds(ParameterGroup referenceGroup, ApplicabilityGroupType applicabilityGroupType)` [has Async]

### `ConfigurationSettingsContainer` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** ConfigurationSettings: ConfigurationSettings, Connection: ServerConnection, SupportsViews: Boolean, Interface: String, Application: String

### `ConfigurationSettingsData` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** TypicalConfiguration: Guid, Product: Guid, DesignContext: Guid, ApplyStructureType: Boolean, ActiveStructureTypeGuid: Guid, ActiveStructureTypes: List`1, DisplayStructureTypes: List`1, ShowBaseStructure: Boolean, StatusesDate: String, ShowEmptyCategories: Boolean, ShowAllCategories: Boolean, ApplyProductFilter: Boolean, ApplyCategoriesFilter: Boolean, ApplyTerms: Boolean, ApplyDate: Boolean, ApplyDesignContext: Boolean, ApplyOptionValues: Boolean, ShowDeletedInDesignContextLinks: Boolean, ApplyProductDesignNumber: Boolean, ApplyProductMilestoneNumber: Boolean, ApplyStructureVariants: Boolean, ProductCategories: List`1, SelectRevisionsTermsObject: Guid, SelectRevisionsTerms: FilterDataCollection, ProductMilestoneNumber: Int32, ProductDesignNumber: Int32, SerialNumberGuid: Guid, OptionValues: List`1, ConfiguratorGuid: Guid, StructureVariants: StructureVariantFilterData, CustomCriteriaValueData: CustomCriteriaValueData

### `ConfigurationSettingsDataExtension` (Namespace: `TFlex.DOCs.Model.References`)
**Методы:**
- `ConfigurationSettings GetConfigurationSettings(ConfigurationSettingsData data, ServerConnection connection)` [has Async]
- `ConfigurationSettingsData GetConfigurationSettingsData(ConfigurationSettings configurationSettings, Configurator configurator)`

### `ConfigurationSettingsExtensions` (Namespace: `TFlex.DOCs.Model.References`)
**Методы:**
- `Void SetToday(ConfigurationSettings settings)`

### `ConfigurationSettingsSerialization` (Namespace: `TFlex.DOCs.Model.References`)
**Методы:**
- `String ToXml(ConfigurationSettings configurationSettings, Configurator configurator)`
- `ConfigurationSettings FromXml(ServerConnection connection, String data)` [has Async]
- `String ToJson(ConfigurationSettings configurationSettings, Configurator configurator)`
- `ConfigurationSettings FromJson(ServerConnection connection, String data)` [has Async]
- `Byte[] ToBson(ConfigurationSettings configurationSettings, Configurator configurator)`
- `ConfigurationSettings FromBson(ServerConnection connection, Byte[] data)` [has Async]

### `CopyReferenceObjectsContext` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** ParentObject: ReferenceObject, CopyChildren: Boolean, ReloadSourceChildren: Boolean, CopyLinkedPrototypes: Boolean, LinkPermanentCharacteristicsOnCopy: Boolean, NomenclatureHierarchyLink: NomenclatureHierarchyLink, Parameters: Dictionary`2
**Методы:**
- `Boolean SkipCopy(ParameterInfo info) (+1)`
- `Void RaiseProcess(String message, Object[] args)`

### `DesktopObject` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** Id: Int32, Guid: Guid, CanEdit: Boolean, CanDelete: Boolean, IsCheckedOut: Boolean, IsCheckedOutByCurrentUser: Boolean, CanCheckOut: Boolean, CanCheckIn: Boolean, CanUndoCheckOut: Boolean, LockState: ReferenceObjectLockState, Links: ReferenceObjectLinks, Reference: Reference, IsAdded: Boolean, IsNew: Boolean, IsDeleted: Boolean, IsModified: Boolean, IsChanged: Boolean, Changing: Boolean, LockStateDescription: String, LockStateIcon: IconImage
**Методы:**
- `IEnumerable`1 CheckOut(Boolean delete, Object context) (+2)` [has Async]
- `IEnumerable`1 CheckIn(String comment, Object context, Boolean executeCallBack, Boolean keepCheckedOut) (+4)` [has Async]
- `IEnumerable`1 UndoCheckOut(Object context) (+1)` [has Async]
- `Boolean CanChangeLink(LinkInfo link, ReferenceObject addObject, ReferenceObject removeObject)`
- `ClassObjectCollection GetAllowedClassesToLink(ParameterGroup linkGroup)`
- `List`1 GetObjects(Guid linkGuid) (+3)` [has Async]
- `Boolean TryGetObjects(Guid linkGuid, List`1& objects) (+1)` [has Async]
- `ReferenceObject GetObject(Guid linkGroup) (+3)` [has Async]
- `Boolean TryGetObject(Guid linkGroup, ReferenceObject& linkedObject) (+1)` [has Async]
- `ComplexHierarchyLink GetLinkedComplexLink(Guid linkGroup) (+1)`
- `Boolean TryLinkedComplexLink(Guid linkGroup, ComplexHierarchyLink& link) (+1)`
- `ReferenceObject CreateListObject(Guid objectList, Guid listObjectClass) (+2)`
- `Void ClearObjectList(Guid objectList) (+1)`
- `Void SetLinkedObject(Guid linkGroup, ReferenceObject newLinkedObject) (+1)`
- `ReferenceObject AddLinkedObject(Guid linkGuid, ReferenceObject newLinkedObject) (+1)`
- `Void SetLinkedComplexLink(Guid linkGroup, ComplexHierarchyLink link) (+1)`
- `Void AddLinkedComplexLink(Guid linkGroup, ComplexHierarchyLink link) (+1)`
- `Boolean RemoveLinkedObject(Guid linkGuid, ReferenceObject linkedObject) (+1)`
- `Boolean RemoveLinkedComplexLink(Guid linkGuid, ComplexHierarchyLink link) (+1)`
- `Void ClearLinks(Guid linkGuid) (+2)`
- `ParameterGroup FindRelation(Guid groupGuid)`
- `Boolean ContainsRelation(Int32 groupId)`
- `Void BeginChanges()` [has Async]
- `Boolean EndChanges()` [has Async]
- `Void CancelChanges()` [has Async]
- `ObjectValue GetObjectValue(ReferencePath path, PathCalculationSettings settings, Boolean throwOnError) (+1)`
- `Boolean BelongsToConfigurationSettings()`

### `DesktopObjectPacketSet`1` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** Count: Int32, IsReadOnly: Boolean
**Методы:**
- `Object GetContextProperty(String propertyName)`
- `Void SetContextProperty(String propertyName, Object value)`
- `Void Add(TDesktopObject desktopObject)` [has Async]
- `Boolean Contains(TDesktopObject desktopObject)`
- `Void CopyTo(TDesktopObject[] array, Int32 arrayIndex)`
- `IEnumerator`1 GetEnumerator()`

### `EditSession` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** Object: ReferenceObject
**Методы:**
- `Boolean EndChanges()` [has Async]
- `Void CancelChanges()` [has Async]

### `EntranceObjectRelation` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** ComplexLink: ComplexHierarchyLink, Child: ReferenceObjectEntrance, Amount: Double

### `FilePreviewImageObjectValue` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** File: FileObject, PageIndex: Int32

### `ICachedToDeleteReferenceObjects` (Namespace: `TFlex.DOCs.Model.References`)
**Методы:**
- `Void CacheToDelete(ICollection`1 referenceObjects, DesktopObjectPacketSet`1 packetSet)`

### `IPacketSetReferenceObject` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** PacketSet: DesktopObjectPacketSet`1

### `IProgressive` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** Percent: Double, AutoCalculation: Boolean
**Методы:**
- `Boolean CanChangePercent()`
- `Boolean CanChangeAutoCalculation()`
- `Void ChangePercent(Double percent)`
- `Void ChangeAutoCalculation(Boolean autoCalculation)`
- `Void RecalculateProgress()`

### `IServerEventHandlerProvider` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** ServerEventHandler: ServerEventHandler

### `LoadSettings` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** MasterGroup: ParameterGroup, LoadDeleted: Boolean, LoadBinaryParameters: Boolean, LoadAllHierarchyParameters: Boolean, OmitHasChildrenCheck: Nullable`1, UseConfigurationSettings: Boolean, UseInstanceMode: Boolean, ParentInstance: ReferenceObjectInstance, RelationsForCheck: ReadOnlyCollection`1, RelationsForCheckCount: Int32, Parameters: ParameterInfoCollection, LoadStructureTypes: Boolean, CheckOnlyHasSubfolders: Boolean, Relations: ICollection`1, SortFields: ReadOnlyCollection`1, HasSortFields: Boolean, SelectionContext: String
**Методы:**
- `Boolean Add(ParameterGroup group) (+5)`
- `Void LoadOneToOneTables()`
- `Void AddRange(IEnumerable`1 parameters)`
- `LoadSettings GetLinkLoadSettings(ParameterGroup linkGroup)`
- `Void ClearLinkLoadSettings(ParameterGroup linkGroup)`
- `Void Clear(Boolean useMaster) (+1)`
- `Boolean AddGroup(ParameterGroup group)`
- `Void AddParameters(Guid[] guids) (+1)`
- `Void AddMasterGroupParameters()`
- `Void AddAllParameters()`
- `Boolean Contains(ParameterInfo item)`
- `Boolean Remove(ParameterInfo parameter) (+2)`
- `RelationLoadSettings AddRelation(ParameterGroup relation, Func`2 staticReferenceFunc) (+5)` [has Async]
- `RelationLoadSettings GetRelation(ParameterGroup relation) (+1)`
- `LinkedObjectLoadSettings AddLinkedObjectRelation()`
- `StructureTypeRelationLoadSettings AddStructureTypeRelation()`
- `ApplicabilityRelationLoadSettings AddApplicabilityRelation()`
- `StartProductRelationLoadSettings AddStartProductRelation()`
- `EndProductRelationLoadSettings AddEndProductRelation()`
- `ObjectRemarksRelationLoadSettings AddObjectRemarksRelation()`
- `AuthorRelationLoadSettings AddAuthorRelation()`
- `EditorRelationLoadSettings AddEditorRelation()`
- `OwnerRelationLoadSettings AddOwnerRelation()`
- `OnBehalfOfRelationLoadSettings AddOnBehalfOfRelation()`
- `CredentialRelationLoadSettings AddCredentialRelation()`
- `MasterServerRelationLoadSettings AddMasterServerRelation()`
- `BaseRepresentationLoadSettings AddBaseRepresentationRelation()`
- `Void CheckLink(Int32 relationId, Boolean swapped) (+2)`
- `Boolean ContainsRelationForCheck(Int32 id)`
- `Boolean RemoveRelation(ParameterGroup relation) (+1)`
- `Void Append(LoadSettings settings)`
- `SortField AddSortField(ParameterGroup linkGroup, ParameterInfo parameter, SortOrder order) (+1)` [has Async]
- `Boolean RemoveSortField(SortField field)`
- `Void AddSortFields(SortField[] sortFields) (+1)`
- `Void ClearSortFields()`

### `NomParametersMatchingReference` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** Classes: NomParametersMatchingTypes

### `NomParametersMatchingReferenceObject` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** Class: NomParametersMatchingType, LinkedReferenceParameter: GuidParameter, NomenclatureParameter: GuidParameter

### `NomParametersMatchingType` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** Classes: NomParametersMatchingTypes, IsNomParametersMatchingReferenceObject: Boolean

### `NomParametersMatchingTypes` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** NomParametersMatchingReferenceObject: NomParametersMatchingType

### `NomParametersSynchroReference` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** Classes: NomParametersSynchroTypes
**Методы:**
- `Guid GetLinkGuidToLinkedReference(Guid classGuid, Guid referenceGuid)` [has Async]
- `NomParametersSynchroReferenceObject GetParametersSynchronizationObject(Guid classGuid, Guid referenceGuid, Boolean includeLinkedInheritClasses)` [has Async]
- `Boolean SupportsNomenclature(Guid referenceGuid)`

### `NomParametersSynchroReferenceObject` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** Class: NomParametersSynchroType, NomenclatureClass: GuidParameter, LinkedReference: GuidParameter, LinkedReferenceClass: GuidParameter, LinkGuidToLinkedReference: GuidParameter, UseLinkedInheritClasses: BooleanParameter, UseBaseClassSettings: Boolean, FolderIsMacro: Boolean, Macro: String, ForbidSelectionFromOtherFolders: Boolean, ShowFolderDialog: Boolean, CreateUserSubfolder: Boolean, DefaultFolderGuid: Guid, NomParametersMatching: ReferenceObjectCollection`1
**Методы:**
- `ReferenceObject FindDefaultParent(Reference reference)`

### `NomParametersSynchroType` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** Classes: NomParametersSynchroTypes, IsNomParametersSynchroReferenceObject: Boolean

### `NomParametersSynchroTypes` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** NomParametersSynchroReferenceObject: NomParametersSynchroType

### `ObjectFormat` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** MasterGroup: ParameterGroup, Type: ObjectFormatType, Format: String
**Методы:**
- `Void FillSettings(LoadSettings loadSettings, Boolean loadLinks)` [has Async]

### `ObjectIterator` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** Count: Int32, Offset: Int32, LastId: Int32, PartNumber: Int32, State: LoadState

### `ObjectLinkChangedEventArgsBase` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** Link: LinkInfo, AddedObject: ReferenceObject, RemovedObject: ReferenceObject, Type: ObjectChangeType, Sender: Object

### `ObjectParameterChangedEventArgsBase` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** Parameter: Parameter, NewValue: Object, OldValue: Object, Type: ObjectChangeType, Sender: Object

### `ObjectParameterFormat` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** Link: ParameterGroup, Parameter: ParameterInfo, Path: ReferencePath, Format: String

### `ObjectStageChangedEventArgsBase` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** NewStage: Stage, OldStage: Stage, Type: ObjectChangeType, Sender: Object

### `ObjectValue` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** Value: Object, IsCollection: Boolean, IsObject: Boolean, IsHierarchyLink: Boolean, IsParameter: Boolean
**Методы:**
- `T GetValue()`
- `Boolean ToBoolean(IFormatProvider provider)`
- `Byte ToByte(IFormatProvider provider)`
- `Char ToChar(IFormatProvider provider)`
- `DateTime ToDateTime(IFormatProvider provider)`
- `Decimal ToDecimal(IFormatProvider provider)`
- `Double ToDouble(IFormatProvider provider)`
- `Int16 ToInt16(IFormatProvider provider)`
- `Int32 ToInt32(IFormatProvider provider)`
- `Int64 ToInt64(IFormatProvider provider)`
- `SByte ToSByte(IFormatProvider provider)`
- `Single ToSingle(IFormatProvider provider)`
- `Object ToType(Type conversionType, IFormatProvider provider)`
- `UInt16 ToUInt16(IFormatProvider provider)`
- `UInt32 ToUInt32(IFormatProvider provider)`
- `UInt64 ToUInt64(IFormatProvider provider)`

### `ObjectValue`1` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** Value: T

### `ParameterContainer` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** ParameterValues: ParameterCollection, Item: Parameter, Item: Parameter, Item: Parameter
**Методы:**
- `IEnumerator`1 GetEnumerator()`

### `ParameterObjectValue` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** Parameter: Parameter, IsParameter: Boolean
**Методы:**
- `Boolean ToBoolean(IFormatProvider provider)`
- `Byte ToByte(IFormatProvider provider)`
- `Char ToChar(IFormatProvider provider)`
- `DateTime ToDateTime(IFormatProvider provider)`
- `Decimal ToDecimal(IFormatProvider provider)`
- `Double ToDouble(IFormatProvider provider)`
- `Int16 ToInt16(IFormatProvider provider)`
- `Int32 ToInt32(IFormatProvider provider)`
- `Int64 ToInt64(IFormatProvider provider)`
- `SByte ToSByte(IFormatProvider provider)`
- `Single ToSingle(IFormatProvider provider)`
- `Object ToType(Type conversionType, IFormatProvider provider)`
- `UInt16 ToUInt16(IFormatProvider provider)`
- `UInt32 ToUInt32(IFormatProvider provider)`
- `UInt64 ToUInt64(IFormatProvider provider)`

### `RecursiveLoadDirectionExtensions` (Namespace: `TFlex.DOCs.Model.References`)
**Методы:**
- `Boolean HasChildren(RecursiveLoadDirection loadDirection)`
- `Boolean HasParents(RecursiveLoadDirection loadDirection)`
- `Boolean HasChildrenOneLevel(RecursiveLoadDirection loadDirection)`
- `Boolean HasParentsOneLevel(RecursiveLoadDirection loadDirection)`

### `Reference` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** Connection: ServerConnection, ConfigurationSettings: ConfigurationSettings, ConfigurationSettingsLinkGroup: ParameterGroup, SpecialConfigurationSettings: ConfigurationSettings, LinkInfo: LinkInfo, Storage: ReferencesStorage, LoadSettings: LoadSettings, PrototypeMode: Boolean, Prototypes: Reference, IsSlave: Boolean, Name: String, Icon: IconImage, ParameterGroup: ParameterGroup, Objects: ReferenceObjectCollection, Classes: ClassTree, Id: Int32, SearchQueries: SearchQueryReference, UndoManager: UndoManager
**Методы:**
- `List`1 IsUniqueObjects(ICollection`1 objects, Boolean reload)` [has Async]
- `Void LoadSignatures(IEnumerable`1 objects)` [has Async]
- `Void ReloadSignatures(IEnumerable`1 objects)` [has Async]
- `Void EndChanges(IEnumerable`1 objects) (+2)` [has Async]
- `List`1 CheckLooping(IEnumerable`1 hierarchyLinks)` [has Async]
- `Void Delete(IEnumerable`1 objects) (+1)` [has Async]
- `List`1 DeleteDesktopObjects(IEnumerable`1 objects)` [has Async]
- `IReadOnlyCollection`1 CreateComplexHierarchyLinks(IEnumerable`1 parents, IEnumerable`1 children, Boolean linkFromParents) (+1)` [has Async]
- `IReadOnlyCollection`1 CreateParentComplexHierarchyLinksWithInstanceData(IEnumerable`1 parents, IEnumerable`1 children)` [has Async]
- `IReadOnlyCollection`1 CreateChildComplexHierarchyLinksWithInstanceData(IEnumerable`1 parents, IEnumerable`1 children)` [has Async]
- `Void ImportFromParentStructure(IEnumerable`1 objects, StructureTypesReferenceObject activeStructure, StructureTypesReferenceObject parentStructure, DesignContextObject designContext)` [has Async]
- `Void ChangeMasterServer(IReadOnlyCollection`1 objects, ReferenceObject masterServer, String comment)` [has Async]
- `Void Unlock(IEnumerable`1 objects)` [has Async]
- `Void UpdateLastRevisionNames(ReferenceObject object) (+1)` [has Async]
- `ReferenceObject GetPrivateFolder()` [has Async]
- `Boolean CanCreateHierarchyLink(ReferenceObject parentObject, ClassObject parentClass, ReferenceObject childObject, ClassObject childClass) (+1)`
- `Boolean CanDeleteHierarchyLink(ComplexHierarchyLink link)`
- `Boolean ContainsEventRaising(EventHandler`1 eventHandler)`
- `Boolean ContainsEventRaised(EventHandler`1 eventHandler)`
- `Boolean CheckLicense(ClassObject classObject, Boolean throwOnError)`
- `List`1 SetObjectsOrder(ReferenceObject object, Int32 order) (+1)`
- `Boolean CanCreateUniqueObject(ClassObject classObject, IDictionary`2 values, ReferenceObject& existingObject) (+2)` [has Async]
- `Boolean CanCreateUniqueObjects(ICollection`1 parameters)` [has Async]
- `List`1 IsUnique(ICollection`1 objects, Boolean reload)` [has Async]
- `Filter GetAccessFilter()`
- `Void SetObjectsOwner(IEnumerable`1 objects, UserReferenceObject owner)` [has Async]
- `Void UseAsRevisions(Dictionary`2 objects, RevisionLevelObject revisionLevel, String sourceRevisionName)`
- `List`1 GetExistingRevisionNames(Guid logicalObjectGuid) (+1)`
- `ComplexHierarchyLink CreateEmptyHierarchLink(ReferenceObject childObject)`
- `Dictionary`2 LoadSimpleReferences(ServerConnection connection, Int32[] referenceIDs)`
- `Dictionary`2 LoadSimpleObjects(ServerConnection connection, Int32 referenceID, List`1 objectIDs)`
- `List`1 Find(Filter filter, Int32 maxCount, ReferenceObject parent, MacroContext formulaContext, LoadSettings loadSettings) (+16)` [has Async]
- `List`1 FindWithMaxCount(ParameterInfo parameter, ComparisonOperator op, Object value, Int32 maxCount, LoadSettings loadSettings) (+1)` [has Async]
- `ReferenceObject FindOne(ParameterInfo parameter, ComparisonOperator op, Object value) (+3)` [has Async]
- `IReferenceObjectCollection Load(ObjectIterator iterator, Filter filter, ReferenceObject rootObject, MacroContext formulaContext, RecursiveLoadDirection loadDirection)` [has Async]
- `IReferenceObjectCollection LoadWithInstances(ObjectIterator iterator, Filter filter, ReferenceObjectInstance rootObjectInstance, ReferenceObject rootObject, MacroContext formulaContext, RecursiveLoadDirection loadDirection)` [has Async]
- `Void Reload(IEnumerable`1 objects, IReadOnlyCollection`1 additionalSettings, Boolean forceLoadDeleted) (+2)` [has Async]
- `Void TryReload(IEnumerable`1 objects, IReadOnlyCollection`1 additionalSettings, Boolean forceLoadDeleted) (+2)` [has Async]
- `ReferenceObjectCollection CreateLoader(Filter filter, ReferenceObject parent, IEnumerable`1 sourceObjects, MacroContext formulaContext, Boolean ignoreLink) (+1)` [has Async]
- `ReferenceObjectCollection CreateParentsLoader(Filter filter, ReferenceObject referenceObject, MacroContext formulaContext)` [has Async]
- `PartialRecursiveCollection CreateRecursiveLoader(Filter filter, ReferenceObject parent, IEnumerable`1 sourceObjects, MacroContext formulaContext, RecursiveLoadDirection loadDirection, Boolean hierarchyLinksOnly) (+1)` [has Async]
- `PartialRecursiveCollection CreateRecursiveLoaderWithInstances(Filter filter, ReferenceObjectInstance parentInstance, IEnumerable`1 sourceObjects, MacroContext formulaContext, RecursiveLoadDirection loadDirection, Boolean hierarchyLinksOnly, ReferenceObject parent) (+1)` [has Async]
- `Void Refresh(Boolean objectsOnly) (+1)` [has Async]
- `Void ClearLoadedObjects()`
- `Void LoadLinks(List`1 rootObjects, LoadSettings settings)` [has Async]
- `List`1 GetDeletedObjects(Filter filter, Int32 count, Int32 offset, MacroContext formulaContext) (+1)` [has Async]
- `Int32 GetDeletedObjectsCount(Filter filter) (+1)` [has Async]
- `List`1 RecursiveLoad(IEnumerable`1 objects, RecursiveLoadDirection loadDirection, LoadSettings loadSettings)` [has Async]
- `ReferenceObject CreateReferenceObject(ReferenceObject parentObject, ClassObject classObject) (+3)`
- `ReferenceObject CreateRevisionsContainer(ReferenceObject parentObject, ClassObject classObject, Guid revisionsContainerGuid) (+1)`
- `ReferenceObjectCopySet CopyReferenceObject(ReferenceObject prototype, ReferenceObject parentObject, ClassObject classObject, Boolean copyChildren, IEnumerable`1 skip) (+6)`
- `ReferenceObjectCopySet CopyReferenceObjects(IEnumerable`1 sourceObjects, ReferenceObject parentObject, Boolean copyChildren, Boolean copyLinkedObjects) (+1)`
- `Boolean CanCreateReferenceObject(ReferenceObject prototype, ReferenceObject parentObject, ClassObject classObject)`
- `Void ValidateNewReferenceObject(ReferenceObject prototype, ReferenceObject parentObject, ClassObject classObject)`
- `Boolean UserHasAccessToCurrentStructureType(Boolean throwOnError)`
- `Boolean UserHasAccessToEditObjectsInCurrentStructureType(Boolean throwOnError)`
- `List`1 GetReports(ClassObject classObject) (+1)`
- `Boolean IsReportAssociatedClass(ClassObject classObject)`
- `List`1 GetReportGeneratorCommands(ClassObject classObject)`
- `Int32 CompareTo(Reference other)`

### `ReferenceComparer` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** Instance: ReferenceComparer
**Методы:**
- `Int32 Compare(Reference x, Reference y)`

### `ReferenceExtensions` (Namespace: `TFlex.DOCs.Model.References`)
**Методы:**
- `Reference GetReference(ServerConnection connection, Int32 referenceId, ReferencesStorage storage, Boolean throwOnError) (+1)`
- `NomenclatureReference GetNomenclatureReference(Reference reference, Boolean throwOnError) (+1)`
- `Reference CreateReference(ServerConnection connection, String referenceName) (+3)`
- `IDisposable ClearAndHoldUseConfigurationSettings(Reference reference)`
- `IDisposable ChangeAndHoldConfigurationSettings(Reference reference, ConfigurationSettings configurationSettings, ParameterGroup linkGroup) (+2)`
- `ReferenceObjectCollection GetObjects(Reference reference, CatalogFolder folder)`
- `List`1 GetAllLoadedObjects(Reference reference)`
- `ReferenceObject FindLoadedReferenceObject(Reference reference, Guid guid)`
- `IDisposable DisableCheckByActiveDate(Reference reference)`

### `ReferenceLinkAdditionalSettings` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** Data: ReferenceLinkAdditionalSettingsData, IsValid: Boolean, IsLoaded: Boolean, LinkGroup: ParameterGroup, LinkClass: ClassObject
**Методы:**
- `ReferenceLinkAdditionalSettings Create(ParameterGroup linkGroup, ClassObject linkClass)`
- `Void Reload()`
- `Boolean Save()`
- `Boolean Remove()`

### `ReferenceLinkAdditionalSettingsData` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** DefaultParentObjectGuid: Guid, DefaultParentObjectName: String, ShowSelectObjectDialog: Boolean, PathIsMacro: Boolean, Macros: String, ForbidSelectionFromOtherFolders: Boolean
**Методы:**
- `String Serialize()`
- `Void Deserialize(String data)`

### `ReferenceObject` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** BaseObject: StateGuidDomainObject, Reference: Reference, Id: Int32, Guid: Guid, ParameterValues: ParameterCollection, SystemFields: ReferenceObjectSystemFields, HasChildren: Boolean, IsHasChildrenLoaded: Boolean, HasSubfolders: Boolean, Parent: ReferenceObject, IsParentLoaded: Boolean, NewParent: ReferenceObject, Parents: ReferenceObjectCollection, IsParentsLoaded: Boolean, Children: ReferenceObjectCollection, IsChildrenLoaded: Boolean, Class: ClassObject, IsPrivateFolder: Boolean, IsInPrivateFolder: Boolean, CanEdit: Boolean, CanDelete: Boolean, IsActualVersion: Boolean, AttachToMasterObject: Boolean, IsAdded: Boolean, IsNew: Boolean, IsPartial: Boolean, IsDeleted: Boolean, IsModified: Boolean, IsChanged: Boolean, IsPrototype: Boolean, PacketSet: DesktopObjectPacketSet`1, SaveSet: ReferenceObjectSaveSet, CanUnlock: Boolean, Changing: Boolean, StandAloneChanging: Boolean, ChangingObject: ReferenceObject, StandAloneCopies: ReadOnlyCollection`1, IsCopy: Boolean, EditableObject: ReferenceObject, Prototype: ReferenceObject, ChangesCounter: Int32, IsNotActual: Boolean, Master: DesktopObject, MasterObject: ReferenceObject, MasterHierarchyLink: ComplexHierarchyLink, IsMaster: Boolean, Versions: ReferenceObjectCollection, IsCheckedOut: Boolean, IsCheckedOutByCurrentUser: Boolean, CanCheckOut: Boolean, CanCheckIn: Boolean, CanUndoCheckOut: Boolean, IsInRecycleBin: Boolean, LockState: ReferenceObjectLockState, CanContainChildren: Boolean, SupportMultiAttachment: Boolean, Signatures: SignatureCollection, ObjectStages: ObjectStageInfo
**Методы:**
- `Boolean MoveUp()`
- `Boolean MoveDown()`
- `Boolean Swap(ReferenceObject objectToSwap)`
- `Void IncreaseSelectionRank(String context)`
- `List`1 GetAllLinkedFiles()`
- `Void Load(IReadOnlyCollection`1 parameters, IReadOnlyCollection`1 toOneLinks) (+2)` [has Async]
- `Boolean CanChangeLink(LinkInfo link, ReferenceObject addObject, ReferenceObject removeObject)`
- `IReadOnlyCollection`1 GetLinkedComplexLinks(Guid linkGuid) (+1)` [has Async]
- `Boolean TryLinkedComplexLinks(Guid linkGuid, IReadOnlyCollection`1& links) (+1)` [has Async]
- `ReferenceObject CreateRevision(RevisionLevelObject revisionLevel, ReferenceObject parent, Boolean recursive, Boolean copyApplicability)`
- `Boolean UseAsRevision(Guid logicalObjectGuid, Boolean throwOnError)`
- `Boolean UseAsNewLogicalObject(Boolean throwOnError)`
- `List`1 GetExistingRevisionNames()`
- `List`1 GetRevisions()`
- `ReadOnlyCollection`1 GetSelectedRevisions()` [has Async]
- `Boolean IsSelectRevisionFailed()`
- `Filter GetRevisionContainerFilter()` [has Async]
- `Boolean CopySignaturesFromRevision(ReferenceObject sourceObject, Boolean throwOnError)`
- `IReadOnlyCollection`1 GetRemarks()`
- `ReferenceObject CreateCopy(ClassObject newClass, OneToManyTable destLink, IEnumerable`1 skip, CopyReferenceObjectsContext context, Boolean loadLinks, Boolean forRevision, Boolean copyLinks) (+4)`
- `ReferenceObject CreateFullCopy(IEnumerable`1 linkIds, ClassObject classObject, ReferenceObject parentObject)`
- `ReferenceObject CopyAllTo(Reference reference)`
- `Void Refresh(ReferenceObject source)`
- `Boolean CanCopy(ParameterInfo parameter) (+1)`
- `Boolean BelongsToConfigurationSettings()`
- `Boolean HasLinkedObjects(ICollection`1 relations, Boolean loadDeleted) (+2)`
- `ParameterGroup FindRelation(Guid groupGuid)`
- `Boolean ContainsRelation(Int32 groupId)`
- `ObjectValue GetObjectValue(ReferencePath path, PathCalculationSettings settings, ComplexHierarchyLink hierarchyLink, ReferenceObjectInstance objectInstance, Boolean throwOnError) (+8)`
- `Boolean IsLoaded(ReferencePath path, ComplexHierarchyLink hierarchyLink) (+1)`
- `Boolean Match(Filter filter)`
- `Stack`1 GetPath(ReferenceObject rootObject) (+1)` [has Async]
- `Void Reload(LoadSettings loadSettings) (+2)` [has Async]
- `Boolean TryReload()` [has Async]
- `Boolean IsAvailable()`
- `ComplexHierarchyLink GetParentLink(ReferenceObject parentObject)` [has Async]
- `ICollection`1 GetParentLinks(ReferenceObject parentObject)`
- `ComplexHierarchyLink GetChildLink(ReferenceObject childObject)` [has Async]
- `ICollection`1 GetChildLinks(ReferenceObject childObject)`
- `ComplexHierarchyLink CreateParentLink(ReferenceObject parentObject)`
- `ComplexHierarchyLink CreateChildLink(ReferenceObject childObject)`
- `ComplexHierarchyLinkInstanceData CreateChildLinkWithInstancesData(ReferenceObject childObject, ReferenceObjectInstance sourceStructureObjectInstance, ReferenceObjectInstance parentObjectInstance)`
- `ComplexHierarchyLinkInstanceData CreateChildLinkWithBaseInstancesData(ReferenceObject childObject, ReferenceObjectInstance baseInstance, ReferenceObjectInstance parentObjectInstance)`
- `Boolean DeleteLink(ComplexHierarchyLink link)`
- `Boolean CanCreateChildObject(ClassObject childClass)`
- `Boolean CanCreateChildLink(ReferenceObject childReferenceObject)`
- `Boolean CanCreateParentLink(ReferenceObject parentReferenceObject)`
- `Boolean SetSignature(User user, SignatureType signatureType, String resolution, Boolean throwOnError, X509Certificate2 certificate) (+1)`
- `List`1 GetSignatures(Int32 objectVersion)`
- `Byte[] GetSigningReferenceObjectData()`
- `DigitalSignatureContent GetDigitalSignatureContent()`
- `Void OnParameterChanged(Parameter p)`
- `Boolean CanChangeParameter(Parameter p, Object newValue)`
- `Boolean IsUnique(ReferenceObject& existingObject)`
- `Boolean SetParent(ReferenceObject parentObject)`
- `Boolean CanSetParent(ReferenceObject parentObject)`
- `Boolean ValidateParentObject(ReferenceObject parentObject, ClassObject classObject, Boolean throwOnError)`
- `Void SetOwnerUser(UserReferenceObject owner)` [has Async]
- `Boolean CheckIsObjectVersionActual()`
- `Void BeginChanges(Boolean reload) (+2)` [has Async]
- `Boolean TryBeginChanges(ReferenceObject& actualObject)`
- `ReferenceObject BeginStandAloneChanges(ComplexHierarchyLink hierarchyLink) (+1)`
- `Boolean TryBeginStandAloneChanges(ComplexHierarchyLink hierarchyLink, ReferenceObject& referenceObject)`
- `Boolean ApplyChanges()` [has Async]
- `Boolean EndChanges()` [has Async]
- `Void CreateSaveSet()`
- `Void CancelChanges()` [has Async]
- `Void Unlock()` [has Async]
- `Void Delete()` [has Async]

### `ReferenceObjectCollection`1` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** AsList: IList`1, Item: T
**Методы:**
- `Int32 IndexOf(T item)`
- `Boolean Contains(T item)`
- `Void CopyTo(T[] array, Int32 arrayIndex)`
- `Boolean Remove(T item)`
- `IEnumerator`1 GetEnumerator()`

### `ReferenceObjectComparer` (Namespace: `TFlex.DOCs.Model.References`)
**Методы:**
- `Int32 Compare(ReferenceObject x, ReferenceObject y)`

### `ReferenceObjectCopySet` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** Context: CopyReferenceObjectsContext
**Методы:**
- `ReferenceObject GetNewObject(ReferenceObject source)`
- `Boolean EndChanges()` [has Async]

### `ReferenceObjectEntrance` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** Owner: ReferenceObjectEntrancesTree, Object: ReferenceObject, DirectEntrancesCount: Nullable`1, EntrancesCount: Double, Children: IEnumerable`1
**Методы:**
- `IEnumerable`1 GetDirectEntrances()`

### `ReferenceObjectEntrancesTree` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** MainObject: ReferenceObject, AllObjects: IEnumerable`1, RootObjects: IEnumerable`1
**Методы:**
- `Void Load()`
- `Void Reload()`

### `ReferenceObjectExtensions` (Namespace: `TFlex.DOCs.Model.References`)
**Методы:**
- `Boolean IsLinkedToNomenclature(ReferenceObject referenceObject)`
- `NomenclatureObject GetLinkedNomenclatureObject(ReferenceObject referenceObject)` [has Async]
- `Dictionary`2 GetLinkedNomenclatureObjects(IEnumerable`1 referenceObjects)` [has Async]
- `Void Modify(TObject referenceObject, Action`1 action, ReferenceObjectSaveSet saveSet, Boolean cancelOnError, Boolean reload, Boolean applyChanges) (+2)` [has Async]
- `Boolean TryActualBeginChanges(TReferenceObject& referenceObject, String& message, ClassObject classObject, Boolean unlock) (+1)` [has Async]
- `Void ModifyActual(TReferenceObject& referenceObject, Action`1 action, Boolean endChanges)`
- `Void TryEndChanges(TReferenceObject referenceObject, Action`1 action)`
- `Boolean TryGetActual(TReferenceObject& referenceObject)`
- `Void RemoveNotActualParentLinks(ReferenceObject referenceObject)`
- `List`1 GetChangedHierarchyLinks(IEnumerable`1 objects)`
- `ObjectValue GetObjectValue(DesktopObject obj, ReferencePath path, ComplexHierarchyLink parentLink, Boolean throwOnError)`
- `String GetPropertyHyperlink(ReferenceObject obj, String serverAddress)` [has Async]
- `String GetHyperlink(ReferenceObject obj, String serverAddress)` [has Async]
- `String GetFileHyperlink(FileObject file, String serverAddress)` [has Async]

### `ReferenceObjectLockStateExtensions` (Namespace: `TFlex.DOCs.Model.References`)
**Методы:**
- `String GetName(ReferenceObjectLockState state)`

### `ReferenceObjectSaveSet` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** Changing: Boolean, Owner: ReferenceObject, ObjectsToDelete: ReadOnlyCollection`1, Count: Int32, IsReadOnly: Boolean
**Методы:**
- `Void AddRange(IEnumerable`1 collection)` [has Async]
- `Boolean Contains(ReferenceObject item)`
- `ReferenceObject Find(Int32 referenceId, Int32 objectId)`
- `Boolean EndChanges()` [has Async]
- `Void CancelChanges(ReferenceObject referenceObject) (+2)` [has Async]
- `Void AddObjectToDelete(ReferenceObject referenceObject)`
- `Void AddObjectsToDelete(IEnumerable`1 referenceObjects)`
- `Void Add(ReferenceObject item)` [has Async]
- `Void CopyTo(ReferenceObject[] array, Int32 arrayIndex)`
- `IEnumerator`1 GetEnumerator()`

### `ReferenceObjectSaveSetExtensions` (Namespace: `TFlex.DOCs.Model.References`)
**Методы:**
- `T RunWithSaveSet(Func`2 operation, CancellationToken token) (+3)` [has Async]

### `ReferenceObjectValue` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** IsObject: Boolean, Object: ReferenceObject, Link: ComplexHierarchyLink

### `ReferenceObjectWithInstance` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** ReferenceObject: ReferenceObject, ObjectInstance: ReferenceObjectInstance

### `ReferenceObjectWithLink` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** Link: ComplexHierarchyLink

### `ReferencesStorage` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** Guid: Guid, Connection: ServerConnection, Settings: ReferencesStorageSettings, IgnoreRefreshReferences: HashSet`1, IsDisposed: Boolean, Item: Object
**Методы:**
- `Reference Get(Int32 referenceId, Boolean throwOnError) (+4)` [has Async]
- `TReferenceObject Append(TReferenceObject referenceObject, Boolean throwOnError, Boolean skipChangingObjectCheck) (+1)`
- `IReadOnlyCollection`1 AppendReadOnly(IReadOnlyCollection`1 referenceObjects, Boolean throwOnError)`
- `Reference Find(Int32 referenceId) (+2)`
- `Void Refresh()`
- `Void ClearLoadedObjects()`
- `Void Clear(Int32 referenceId) (+3)`
- `Boolean ChangeSettings(ReferencesStorageSettings settings)`

### `ReferencesStorageExtension` (Namespace: `TFlex.DOCs.Model.References`)
**Методы:**
- `ReferencesStorage GetOrCreateStorage(Reference reference)`

### `ReferencesStorageSettings` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** Bind: Boolean, CanRefreshReference: Boolean, CanRefreshStaticReference: Boolean, CanRefreshReferenceCatalog: Boolean, CheckHierarchyWhenChanging: Boolean, OnReferenceAppending: Action`1, InitAllParametersWhenCreate: Boolean, CanCreateNewInstanceForStaticReference: Boolean

### `RelationLoadSettings` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** Owner: LoadSettings, Relation: ParameterGroup, Load: Boolean, LoadHierarchy: Boolean, ConfigurationSettings: ConfigurationSettings, LoadHasChildren: Boolean, StaticReference: Reference, StaticReferenceSettings: StaticReferenceLoadSettings, Filter: Filter, RecursiveLoadHierarchy: RecursiveLoadDirection
**Методы:**
- `Void TakeStaticReference()`

### `SaveSetEditSession` (Namespace: `TFlex.DOCs.Model.References`)
**Методы:**
- `Void Add(ReferenceObject referenceObject)`
- `Boolean EndChanges()`
- `Void CancelChanges()`

### `SignatureObjectValue` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** Signature: Signature, SignatureParameter: SignatureParameter

### `SpecialComplexHierarchyLink`2` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** Reference: TReference, ParentObject: TReferenceObject, ChildObject: TReferenceObject

### `SpecialFilterArgs` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** ParameterGroup: ParameterGroup, PrototypeMode: Boolean, IsSlave: Boolean, IsSystemCatalog: Boolean, UseCache: Boolean

### `SpecialReference`1` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** Objects: ReferenceObjectCollection`1
**Методы:**
- `TObject CreateReferenceObject(ReferenceObject parentObject, ClassObject classObject) (+3)`
- `TObject CreateRevisionsContainer(ReferenceObject parentObject, ClassObject classObject, Guid revisionsContainerGuid) (+1)`
- `IComparer`1 GetAfterLoadSortComparer()`
- `TObject Find(Int32 objectId, Boolean ignoreLinks) (+3)` [has Async]
- `TObject FindOne(ParameterInfo parameter, ComparisonOperator op, Object value) (+3)` [has Async]

### `SpecialReferenceObject`1` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** Reference: TReference

### `UniqueIndexCheckResult` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** Index: UniqueIndex, ObjectId: Int32, IsObjectDeleted: Boolean, AddedClientView: ClientView
**Методы:**
- `ReferenceObject GetObject()`

### `UniqueObjectCheckParameters` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** Class: ClassObject, Values: IDictionary`2, CheckResult: UniqueIndexCheckResult, ExistingObject: ReferenceObject

### `UniqueObjectCheckResult` (Namespace: `TFlex.DOCs.Model.References`)
**Свойства:** Object: ReferenceObject, ExistingObject: ReferenceObject, IsExistingInBatch: Boolean, IsObjectDeleted: Boolean, AddedClientView: ClientView, Index: UniqueIndex
**Методы:**
- `String GetErrorDetails()`

### `AssignmentChangeManager`1` (Namespace: `TFlex.DOCs.Model.References.Assignments`)
**Методы:**
- `Boolean CanChangeParameter(TAssignment assignment, Parameter parameter, Object newValue)`
- `Boolean CanChangeLink(TAssignment assignment, LinkInfo link, ReferenceObject addObject, ReferenceObject removeObject)`

### `AssignmentFolderReference` (Namespace: `TFlex.DOCs.Model.References.Assignments`)
**Свойства:** Classes: AssignmentFolderTypes
**Методы:**
- `ManualAssignmentFolderReferenceObject[] GetAllManualFolders()`
- `ManualAssignmentFolderReferenceObject CreateManualFolder(AssignmentFolderReferenceObject parent)`
- `SearchAssignmentFolderReferenceObject CreateSearchFolder(AssignmentFolderReferenceObject parent)`

### `AssignmentFolderReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Assignments`)
**Свойства:** Class: AssignmentFolderType, Name: StringParameter, ViewName: StringParameter, IsPublic: BooleanParameter
**Методы:**
- `Boolean CanCreateManualFolder()`
- `Boolean CanCreateSearchFolder()`
- `Boolean CanDeleteFolder()`

### `AssignmentFolderType` (Namespace: `TFlex.DOCs.Model.References.Assignments`)
**Свойства:** Classes: AssignmentFolderTypes, IsAssignmentFolderReferenceObject: Boolean, IsSearchAssignmentFolderReferenceObject: Boolean, IsManualAssignmentFolderReferenceObject: Boolean

### `AssignmentFolderTypes` (Namespace: `TFlex.DOCs.Model.References.Assignments`)
**Свойства:** AssignmentFolderReferenceObject: AssignmentFolderType, SearchAssignmentFolderReferenceObject: AssignmentFolderType, ManualAssignmentFolderReferenceObject: AssignmentFolderType

### `AssignmentFormulaMacro` (Namespace: `TFlex.DOCs.Model.References.Assignments`)
**Свойства:** CodeOffset: Int32

### `AssignmentMacroProvider` (Namespace: `TFlex.DOCs.Model.References.Assignments`)
**Свойства:** Context: AssignmentMacroContext
**Методы:**
- `List`1 GetSubordinateIds()` [RU: ПолучитьИдентификаторыПодчинённых]

### `AssignmentReference` (Namespace: `TFlex.DOCs.Model.References.Assignments`)
**Свойства:** ProcessHelper: IAssignmentReferenceHelper, UsersAssistance: AssignmentUsersAssistance, Classes: AssignmentTypes
**Методы:**
- `List`1 GetAssociatedAssignments(Guid referenceObjectGuid, Boolean reload) (+1)`
- `Boolean OpenAssignmentsInMail(ServerConnection serverConnection)` [has Async]

### `AssignmentReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Assignments`)
**Свойства:** StatusType: AssignmentStatus, DescriptionFormatType: DescriptionFormatType, AcceptType: AssignmentAcceptType, Percent: Double, AutoCalculation: Boolean, IsDraft: Boolean, IsCancelled: Boolean, InProgress: Boolean, Class: AssignmentType, Name: StringParameter, StartDate: DateTimeParameter, EndDate: DateTimeParameter, CheckDate: DateTimeParameter, Description: StringParameter, DescriptionTextFormat: Int32Parameter, Status: Int32Parameter, LaboriousnessPlan: SingleParameter, LaboriousnessFact: SingleParameter, AuxiliaryTime: SingleParameter, PercentComplete: PercentParameter, Importance: Int32Parameter, Number: StringParameter, Basic: BooleanParameter, Priority: ByteParameter, ErrorLog: StringParameter, ExtendedData: StringParameter, Comments: IEnumerable`1, StatusChangeComments: IEnumerable`1, Executor: User, StorageExecutor: User, Task: ReferenceObject, LinkedMaterials: AnyReferenceLink, StorageLinkedMaterials: ICollection`1, MailingList: ReferenceObjectCollection`1, Controller: User, OnBehalf: User, StorageOnBehalf: User, Files: ReferenceObjectCollection`1, Categories: ReferenceObjectCollection, DependentAssignments: ReferenceObjectCollection`1
**Методы:**
- `CommentReferenceObject AddComment(Guid listObjectClass) (+1)`
- `ReferenceObject AddLinkedMaterial(ReferenceObject newLinkedObject)`
- `ReferenceObject AddStorageLinkedMaterial(ReferenceObject newLinkedObject)`
- `Boolean RemoveLinkedMaterial(ReferenceObject linkedObject)`
- `Boolean RemoveStorageLinkedMaterial(ReferenceObject linkedObject)`
- `ReferenceObject Subscribe(User user) (+1)`
- `Boolean Unsubscribe(User user) (+1)`
- `ReferenceObject AddFile(ReferenceObject newLinkedObject)`
- `Boolean RemoveFile(ReferenceObject linkedObject)`
- `ReferenceObject AddCategory(ReferenceObject newLinkedObject)`
- `Boolean RemoveCategory(ReferenceObject linkedObject)`
- `ReferenceObject AddDependentAssignment(AssignmentReferenceObject newLinkedObject)`
- `Boolean RemoveDependentAssignment(ReferenceObject linkedObject)`
- `Boolean CheckCurrentUserIsAssignmentManager()`
- `Void ChangeAutoCalculation(Boolean autoCalculation)`
- `Boolean CanChangeAutoCalculation()`
- `Boolean CanChangePercent()`
- `Void ChangePercent(Double percent)`
- `Void RecalculateProgress()`
- `Boolean CanChangeParameter(Parameter p, Object newValue)`
- `Boolean CanChangeLink(LinkInfo link, ReferenceObject addObject, ReferenceObject removeObject)`
- `Boolean CanRemove()`
- `CommentReferenceObject CreateComment(String text, Boolean endChanges, String name, CommentType type) (+2)`
- `Boolean CanCreateComment()`
- `AssignmentReferenceObject GetParentAssignment()`
- `GroupAssignmentReferenceObject GetGroupAssignment()`
- `Void BuildBody(String text, String comments)`
- `Void Accept(AssignmentReferenceObject[] assignments) (+1)`
- `Boolean CanAccept()`
- `Boolean Complete(String comment, String description) (+1)`
- `Boolean CanComplete(Boolean statusReload) (+1)`
- `Void Reject(AssignmentReferenceObject[] assignments) (+1)`
- `Boolean CanReject()`
- `Void Close(AssignmentReferenceObject[] assignments) (+1)`
- `Boolean CanClose(Boolean statusReload) (+1)`
- `Void Suspend(AssignmentReferenceObject[] assignments) (+1)`
- `Boolean CanSuspend(Boolean statusReload) (+1)`
- `Void Resume(AssignmentReferenceObject[] assignments) (+1)`
- `Boolean CanResume(Boolean statusReload) (+1)`
- `Void Cancel(AssignmentReferenceObject[] assignments, CancellationActionOnDependentAssignments actionOnDependentAssignments, String comment) (+1)`
- `Boolean CanCancel(Boolean statusReload) (+1)`
- `Void Elaborate(AssignmentReferenceObject[] assignments, String comment)`
- `Boolean CanElaborate(Boolean statusReload) (+1)`
- `Boolean CanCreateLinkedAssignment()`
- `Void SetImportance(AssignmentReferenceObject[] assignments, AssignmentImportance importance)`
- `Boolean CanClone()`
- `AssignmentReferenceObject CreateClone(AssignmentType type, Boolean shouldSaved) (+1)`

### `AssignmentType` (Namespace: `TFlex.DOCs.Model.References.Assignments`)
**Свойства:** CanCreateObjects: Boolean, Classes: AssignmentTypes, IsAssignmentReferenceObject: Boolean, IsMemoReferenceObject: Boolean, IsAssignmentWithExecutorsList: Boolean, IsGroupAssignment: Boolean, IsProcessAssignment: Boolean, IsAgreementProcessAssignment: Boolean, IsWorkProcessAssignment: Boolean, IsExceptionProcessAssignment: Boolean, IsDurationExceptionProcessAssignment: Boolean, IsNoExecutorExceptionProcessAssignment: Boolean, IsNoSolutionExceptionProcessAssignment: Boolean, IsNativeExceptionProcessAssignment: Boolean, IsMacroProcessAssignment: Boolean, IsRunSubprocessProcessAssignment: Boolean, IsProjectAssignment: Boolean

### `AssignmentTypes` (Namespace: `TFlex.DOCs.Model.References.Assignments`)
**Свойства:** AssignmentReferenceObject: AssignmentType, MemoReferenceObject: AssignmentType, AssignmentWithExecutorsList: AssignmentType, GroupAssignment: AssignmentType, ProcessAssignment: AssignmentType, AgreementProcessAssignment: AssignmentType, WorkProcessAssignment: AssignmentType, ExceptionProcessAssignment: AssignmentType, DurationExceptionProcessAssignment: AssignmentType, NoExecutorExceptionProcessAssignment: AssignmentType, NoSolutionExceptionProcessAssignment: AssignmentType, NativeExceptionProcessAssignment: AssignmentType, MacroProcessAssignment: AssignmentType, RunSubprocessProcessAssignment: AssignmentType, ProjectAssignment: AssignmentType

### `AssignmentUsersAssistance` (Namespace: `TFlex.DOCs.Model.References.Assignments`)
**Свойства:** SubordinateIds: ReadOnlyCollection`1, UserIdsWhoseRightsInheritedByCredentials: ReadOnlyCollection`1
**Методы:**
- `Void Clear()`

### `AssignmentWithExecutorsListObject` (Namespace: `TFlex.DOCs.Model.References.Assignments`)
**Свойства:** PossibleExecutors: ReferenceObjectCollection
**Методы:**
- `ReferenceObject AddPossibleExecutor(ReferenceObject newLinkedObject)`
- `Boolean RemovePossibleExecutor(ReferenceObject linkedObject)`

### `CategoryReference` (Namespace: `TFlex.DOCs.Model.References.Assignments`)
**Свойства:** Classes: CategoryTypes

### `CategoryReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Assignments`)
**Свойства:** Class: CategoryType, Name: StringParameter, Icon: IconParameter

### `CategoryType` (Namespace: `TFlex.DOCs.Model.References.Assignments`)
**Свойства:** Classes: CategoryTypes, IsCategory: Boolean

### `CategoryTypes` (Namespace: `TFlex.DOCs.Model.References.Assignments`)
**Свойства:** Category: CategoryType

### `CommentLinkReference` (Namespace: `TFlex.DOCs.Model.References.Assignments`)
**Свойства:** Classes: CommentTypes

### `CommentReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Assignments`)
**Свойства:** Class: CommentType, Name: StringParameter, Text: StringParameter, DetailedDescription: StringParameter, DescriptionFormatType: DescriptionFormatType, DescriptionTextFormat: Int32Parameter, OnBehalf: User, LinkedMaterials: AnyReferenceLink
**Методы:**
- `ReferenceObject AddLinkedMaterial(ReferenceObject newLinkedObject)`
- `Boolean RemoveLinkedMaterial(ReferenceObject linkedObject)`

### `CommentType` (Namespace: `TFlex.DOCs.Model.References.Assignments`)
**Свойства:** Classes: CommentTypes, IsComment: Boolean, IsStatusChangeComment: Boolean

### `CommentTypes` (Namespace: `TFlex.DOCs.Model.References.Assignments`)
**Свойства:** Comment: CommentType, StatusChangeComment: CommentType

### `ExecutorCandidate` (Namespace: `TFlex.DOCs.Model.References.Assignments`)
**Свойства:** Status: ExecutorCandidateStatus, Executor: User, Assignment: AssignmentReferenceObject

### `ExecutorCandidatesManager` (Namespace: `TFlex.DOCs.Model.References.Assignments`)
**Свойства:** GroupAssignment: GroupAssignmentReferenceObject, IsModified: Boolean
**Методы:**
- `ExecutorCandidate Add(User user)`
- `Void Remove(UserReferenceObject user)`
- `Void Detaching(UserReferenceObject user)`
- `List`1 GetChanges()`
- `Void CancelChanges()`
- `IEnumerator`1 GetEnumerator()`

### `GroupAssignmentReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Assignments`)
**Свойства:** AutomaticCalculation: BooleanParameter, AutoCalculation: Boolean, IsModified: Boolean, IsChanged: Boolean
**Методы:**
- `ExecutorCandidatesManager GetExecutorsManager()`
- `Void RecalculateProgress()`
- `GroupAssignmentReferenceObject GetGroupAssignment()`
- `Void ChangeAutoCalculation(Boolean autoCalculation)`

### `IAssignmentChangeManager`1` (Namespace: `TFlex.DOCs.Model.References.Assignments`)
**Методы:**
- `Boolean CanChangeParameter(TAssignment assignment, Parameter parameter, Object newValue)`
- `Boolean CanChangeLink(TAssignment assignment, LinkInfo link, ReferenceObject addObject, ReferenceObject removeObject)`

### `ManualAssignmentFolderReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Assignments`)
**Свойства:** LinkedAssignments: IEnumerable`1
**Методы:**
- `Void AddAssignments(IEnumerable`1 assignments)`
- `Void RemoveAssignments(IEnumerable`1 assignments)`

### `SearchAssignmentFolderReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Assignments`)
**Свойства:** Objects: ReferenceObjectCollection, Filter: StringParameter, ParamSearchFolder: StringParameter, IsCreatedByParameter: BooleanParameter, IsAutoCreated: Boolean
**Методы:**
- `Filter GetSearchFilter()`

### `StatusChangeCommentObject` (Namespace: `TFlex.DOCs.Model.References.Assignments`)
**Свойства:** OldStatusType: AssignmentStatus, NewStatusType: AssignmentStatus, OldStatus: Int32Parameter, NewStatus: Int32Parameter

### `AssignmentDescriptionFormatTypeConverter` (Namespace: `TFlex.DOCs.Model.References.Assignments.Converters`)
**Методы:**
- `ConvertResponse Convert(AssignmentReferenceObject assignment, DescriptionFormatType to, Object context) (+1)`

### `BomSectionsReference` (Namespace: `TFlex.DOCs.Model.References.BomSections`)
**Свойства:** Classes: BomSectionsTypes

### `BomSectionsReferenceObject` (Namespace: `TFlex.DOCs.Model.References.BomSections`)
**Свойства:** Name: String, Code: Int32

### `BomSectionsType` (Namespace: `TFlex.DOCs.Model.References.BomSections`)
**Свойства:** Classes: BomSectionsTypes

### `BomSectionsTypes` (Namespace: `TFlex.DOCs.Model.References.BomSections`)
**Свойства:** BomSection: BomSectionsType

### `BorrowLinkedObject` (Namespace: `TFlex.DOCs.Model.References.Borrowing`)
**Свойства:** Action: BorrowLinkedObjectAction
**Методы:**
- `Boolean SetAction(BorrowLinkedObjectAction action)`

### `BorrowLinkInfo` (Namespace: `TFlex.DOCs.Model.References.Borrowing`)
**Свойства:** Key: BorrowLinkKey, Action: BorrowLinkedObjectAction

### `BorrowLinkKey` (Namespace: `TFlex.DOCs.Model.References.Borrowing`)
**Свойства:** LinkId: Guid, IsSwapped: Boolean

### `BorrowLinkSetting` (Namespace: `TFlex.DOCs.Model.References.Borrowing`)
**Свойства:** LinkGuid: Guid, Visible: Boolean, Status: BorrowObjectStatus

### `BorrowMacroContext` (Namespace: `TFlex.DOCs.Model.References.Borrowing`)
**Свойства:** SourceObjects: List`1, DestinationObject: ReferenceObject, Result: List`1

### `BorrowObject` (Namespace: `TFlex.DOCs.Model.References.Borrowing`)
**Свойства:** Reference: Reference, HasChildren: Boolean, ReferenceObject: ReferenceObject, ComplexLink: ComplexHierarchyLink, Ignored: Boolean, RevisionTypeGuid: Guid, CopyApplicability: Boolean, ExistEqualsParameters: Boolean, ExistEqualsOnlyLinkParameters: Boolean, IsChildrenLoaded: Boolean, Children: ReadOnlyCollection`1, BorrowParameters: ReadOnlyCollection`1, Parent: BorrowObject, ExistingUniqueObjectByEqualsParameters: ReferenceObject
**Методы:**
- `Boolean HasAnyLinkedObject(Guid linkId, Boolean isSwapped) (+1)`
- `IEnumerable`1 GetLinkedObjects(BorrowLinkKey linkKey) (+1)`
- `Void LoadChildren(HashSet`1 linksToLoad)`
- `Void FillBorrowParametersMap(IDictionary`2 map)`
- `IDictionary`2 GetBorrowParametersMap()`
- `BorrowObjectStatus GetObjectStatus(Boolean recalc)`
- `Dictionary`2 GetParameterValuesForCheckUnique(Boolean isNewParentObject, Boolean& existUniqueBorrowParameter)`
- `Void AddObjectParameter(ParameterInfo parameter, Object fromValue, Object toValue, Boolean onlySetValue)`
- `Object GetValue(ParameterInfo parameter, Object currentValue) (+2)`
- `Boolean FindChanged(ParameterInfo parameter, Object fromValue, BorrowParameterObject& resParameter)`

### `BorrowObjectManager` (Namespace: `TFlex.DOCs.Model.References.Borrowing`)
**Свойства:** Struct: BorrowObjectStruct, LinkSettings: IReadOnlyCollection`1
**Методы:**
- `BorrowObjectManager CreateInstance(Guid parameterGroup)`
- `Void RegisterCreator(Guid parameterGroup, Func`1 factory)`
- `Void Initialize(ICollection`1 objects) (+3)`
- `Void SetLinkSettings(ICollection`1 linkSettings)`
- `Task`1 Execute(IProgress progress, Func`4 borrowStructureInitializer, CancellationToken cancellationToken)`

### `BorrowObjectStruct` (Namespace: `TFlex.DOCs.Model.References.Borrowing`)
**Свойства:** DestinationObject: ReferenceObject, Formula: String, ClearHiddenLinks: Boolean, LinkActions: HashSet`1, SourceObjects: IReadOnlyCollection`1, ResultObject: ReferenceObject, ResultObjects: List`1, SupportCopyFiles: Boolean, PrototypeCopyContext: CopyReferenceObjectsContext, BorrowReference: Reference, Reference: Reference, HideNotUseObjectsInUI: Boolean, BorrowObjects: ReadOnlyCollection`1, AdditionalName: String
**Методы:**
- `Void AddUniqueParameter(ParameterInfo parameter, Object toValue)`
- `String GenerateUniqueValue(String parameterValue, ParameterInfo parameter, List`1 bufferValues)`
- `Void AddGlobalParameter(ParameterInfo parameter, Object fromValue, Object toValue)`
- `BorrowObject AddObject(BorrowObject borrowObject)`
- `BorrowObject FindObject(ReferenceObject referenceObject, ComplexHierarchyLink complexKey)`
- `String GetObjectStatusName(BorrowObject borrowObject)`
- `BorrowObject Create(ReferenceObject obj, ComplexHierarchyLink link, BorrowObject parent)`
- `BorrowLinkedObject CreateLinked(ReferenceObject obj, BorrowObject parent) (+1)`
- `Void ChangeLinkActions(HashSet`1 newActions)`

### `BorrowParameterObject` (Namespace: `TFlex.DOCs.Model.References.Borrowing`)
**Свойства:** ParameterId: Int32, Parameter: ParameterInfo, FromValue: Object, ToValue: Object, OriginalValue: Object
**Методы:**
- `Object GetNewValue(Object currentValue)`

### `BorrowStatusChangedHandler` (Namespace: `TFlex.DOCs.Model.References.Borrowing`)
**Методы:**
- `Void Invoke(Object sender, BorrowObjectStatus prevStatus, BorrowObjectStatus newStatus)`
- `IAsyncResult BeginInvoke(Object sender, BorrowObjectStatus prevStatus, BorrowObjectStatus newStatus, AsyncCallback callback, Object object)`
- `Void EndInvoke(IAsyncResult result)`

### `IAdditionalSettings` (Namespace: `TFlex.DOCs.Model.References.Borrowing`)
**Свойства:** Parameters: Dictionary`2

### `CalendarReference` (Namespace: `TFlex.DOCs.Model.References.Calendar`)
**Свойства:** Classes: CalendarTypes

### `CalendarReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Calendar`)
**Свойства:** Class: CalendarType, WorkingTimesLink: OneToManyLink, WorkingTimes: ICollection`1, WorkTimeManager: WorkTimeManager, Name: StringParameter
**Методы:**
- `Void UpdateWorkTimeElements()`
- `Boolean HasAnyWorkTimeInterval()`
- `Void LoadWorkingTimes()`

### `CalendarType` (Namespace: `TFlex.DOCs.Model.References.Calendar`)
**Свойства:** Classes: CalendarTypes, WorkTimePriorityComparer: Func`2

### `Catalog` (Namespace: `TFlex.DOCs.Model.References.Catalogs`)
**Свойства:** Manager: CatalogManager, RootFolders: ReadOnlyCollection`1, Id: Int32, Guid: Guid, Name: String, IsPrivate: Boolean, IsAdded: Boolean, IsModified: Boolean, IsSystem: Boolean, AccessUsersUseType: ItemListUseType, Item: CatalogFolder, Count: Int32, IsReadOnly: Boolean
**Методы:**
- `Void Reload()` [has Async]
- `List`1 FindCatalogFolders(String name)`
- `CatalogFolder FindCatalogFolder(Int32 id) (+1)`
- `Boolean Save()`
- `Boolean Delete()`
- `ReferenceObjectCollection CreateLoader(IEnumerable`1 folders, Filter filter, MacroContext formulaContext)`
- `List`1 GetAccessUsers()`
- `Void SetAccessUsers(IEnumerable`1 users)`
- `Boolean ChangingFolderObjectsAllowed()`
- `Int32 IndexOf(CatalogFolder item)`
- `Boolean Contains(CatalogFolder item)`
- `Void CopyTo(CatalogFolder[] array, Int32 arrayIndex)`
- `IEnumerator`1 GetEnumerator()`
- `Int32 CompareTo(Catalog other)`

### `CatalogFolder` (Namespace: `TFlex.DOCs.Model.References.Catalogs`)
**Свойства:** Catalog: Catalog, Id: Int32, Guid: Guid, Name: String, Icon: IconImage, HasIcon: Boolean, IsIconLoaded: Boolean, ShowObjects: Boolean, Parent: CatalogFolder, Subfolders: ReadOnlyCollection`1, IsAdded: Boolean, IsModified: Boolean, IsSystem: Boolean, Objects: ReferenceObjectCollection, AsUserFolder: UserFolder, AsSearchFolder: SearchFolder, AsFolderGroup: FolderGroup
**Методы:**
- `List`1 GetAllSubfolders()`
- `Boolean Save()` [has Async]
- `Boolean Delete()` [has Async]
- `List`1 FindSubFolders(String name)`
- `CatalogFolder FindSubFolder(Guid guid)`
- `Int32 CompareTo(CatalogFolder other)`

### `CatalogManager` (Namespace: `TFlex.DOCs.Model.References.Catalogs`)
**Свойства:** ReferenceInfo: ReferenceInfo, Reference: Reference, SearchQueries: SearchQueryReference, Item: Catalog, Count: Int32, IsReadOnly: Boolean
**Методы:**
- `List`1 Find(String name) (+2)`
- `List`1 FindCatalogFolder(String name) (+1)`
- `Void Reload()` [has Async]
- `Int32 IndexOf(Catalog item)`
- `Boolean Contains(Catalog item)`
- `Void CopyTo(Catalog[] array, Int32 arrayIndex)`
- `IEnumerator`1 GetEnumerator()`

### `ClassCatalogFolder` (Namespace: `TFlex.DOCs.Model.References.Catalogs`)
**Свойства:** Class: ClassObject, HasIcon: Boolean

### `FolderGroup` (Namespace: `TFlex.DOCs.Model.References.Catalogs`)
**Свойства:** AsFolderGroup: FolderGroup

### `SearchFolder` (Namespace: `TFlex.DOCs.Model.References.Catalogs`)
**Свойства:** AsSearchFolder: SearchFolder, ParameterSearchFolder: String, IsCreatedByParameter: Boolean
**Методы:**
- `Filter GetFilter()`
- `Void SetFilter(Filter filter)`
- `Filter GetSearchFilter()`

### `UserFolder` (Namespace: `TFlex.DOCs.Model.References.Catalogs`)
**Свойства:** AsUserFolder: UserFolder
**Методы:**
- `Boolean Add(ReferenceObject referenceObject) (+1)`
- `Boolean Remove(ReferenceObject referenceObject) (+1)`

### `Certificate` (Namespace: `TFlex.DOCs.Model.References.Certificate`)
**Свойства:** Class: CertificateType, Name: StringParameter, IsExpired: Boolean, ExpirationDate: DateTimeParameter, Algorithm: StringParameter, Hash: PasswordParameter, Note: StringParameter, BeginningOfExpiration: DateTimeParameter, UseTrustedUsersList: BooleanParameter, EncryptedText: StringParameter, UserCertificates: ReferenceObjectCollection
**Методы:**
- `Boolean OpenCloseSymmetricKey(String phKeyPassword)`
- `Tuple`2 CreateKeysPair(String sourcePasswordHash)`
- `Boolean IsCertificateAvailable()`
- `Filter CreateFilter(ServerConnection connection, Boolean showExpired)`
- `ReferenceObject AddUserCertificates(ReferenceObject newLinkedObject)`
- `Boolean RemoveUserCertificates(ReferenceObject linkedObject)`

### `CertificateReference` (Namespace: `TFlex.DOCs.Model.References.Certificate`)
**Свойства:** Classes: CertificateTypes

### `CertificateType` (Namespace: `TFlex.DOCs.Model.References.Certificate`)
**Свойства:** IsCertificate: Boolean, Classes: CertificateTypes
**Методы:**
- `List`1 GetCertificates()`

### `CertificateTypes` (Namespace: `TFlex.DOCs.Model.References.Certificate`)
**Свойства:** Certificate: CertificateType

### `CharacteristicClassReference` (Namespace: `TFlex.DOCs.Model.References.Characteristics`)
**Свойства:** Classes: CharacteristicClassTypes

### `CharacteristicClassReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Characteristics`)
**Свойства:** Class: CharacteristicClassType, Name: StringParameter, IsPercent: BooleanParameter, IsVariable: BooleanParameter, Symbol: StringParameter, DataType: Int32Parameter, CharacteristicGroupClass: CharacteristicGroupClassReferenceObject, LinkClassCharacteristicMeasure: Unit

### `CharacteristicClassType` (Namespace: `TFlex.DOCs.Model.References.Characteristics`)
**Свойства:** Classes: CharacteristicClassTypes, IsCharacterClass: Boolean

### `CharacteristicClassTypes` (Namespace: `TFlex.DOCs.Model.References.Characteristics`)
**Свойства:** CharacterClass: CharacteristicClassType

### `CharacteristicGroupClassReference` (Namespace: `TFlex.DOCs.Model.References.Characteristics`)
**Свойства:** Classes: CharacteristicGroupClassTypes

### `CharacteristicGroupClassReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Characteristics`)
**Свойства:** Class: CharacteristicGroupClassType, Name: StringParameter, ForVariables: BooleanParameter, CharacteristicClasses: ReferenceObjectCollection`1

### `CharacteristicGroupClassType` (Namespace: `TFlex.DOCs.Model.References.Characteristics`)
**Свойства:** Classes: CharacteristicClassTypes, IsCharacteristicGroupClass: Boolean

### `CharacteristicGroupClassTypes` (Namespace: `TFlex.DOCs.Model.References.Characteristics`)
**Свойства:** CharacteristicGroupClass: CharacteristicGroupClassType

### `CharacteristicGroupReference` (Namespace: `TFlex.DOCs.Model.References.Characteristics`)
**Свойства:** Classes: CharacteristicGroupTypes
**Методы:**
- `Boolean DeleteEmptyCharacteristicGroup(CharacteristicGroupReferenceObject groupObject, ReferenceObjectSaveSet saveSet)` [has Async]
- `CharacteristicGroupReferenceObject FindExistedCharacteristicGroup(CharacteristicClassReferenceObject characteristicClass, IEnumerable`1 characteristics)`

### `CharacteristicGroupReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Characteristics`)
**Свойства:** Class: CharacteristicGroupType, Name: StringParameter, CharacteristicGroupClassObject: GuidParameter, ForVariables: BooleanParameter, LinkGroupCharacteristics: ReferenceObjectCollection, Characteristics: ReferenceObjectCollection`1
**Методы:**
- `ReferenceObject AddLinkGroupCharacteristic(ReferenceObject newLinkedObject)`
- `CharacteristicReferenceObject AddCharacteristic(CharacteristicReferenceObject newLinkedCharacteristic)`
- `Boolean RemoveLinkGroupCharacteristic(ReferenceObject linkedObject)`
- `Boolean RemoveCharacteristic(CharacteristicReferenceObject linkedCharacteristic)`

### `CharacteristicGroupType` (Namespace: `TFlex.DOCs.Model.References.Characteristics`)
**Свойства:** Classes: CharacteristicGroupTypes, IsCharacteristicGroup: Boolean

### `CharacteristicGroupTypes` (Namespace: `TFlex.DOCs.Model.References.Characteristics`)
**Свойства:** CharacteristicGroup: CharacteristicGroupType

### `CharacteristicReference` (Namespace: `TFlex.DOCs.Model.References.Characteristics`)
**Свойства:** Classes: CharacteristicTypes
**Методы:**
- `CharacteristicReferenceObject CreateCharacteristic(CharacteristicClassReferenceObject characteristicClassObject, CharacteristicGroupReferenceObject characteristicGroupObject)` [has Async]
- `IReadOnlyList`1 CreateCharacteristics(IEnumerable`1 characteristicObjects)` [has Async]
- `ReferenceObjectCopySet CopyCharacteristic(CharacteristicReferenceObject prototype)`
- `Boolean RemoveLinkedCharacteristic(OneToManyLink characteristicsLink, CharacteristicReferenceObject characteristicObject, ReferenceObjectSaveSet saveSet) (+1)` [has Async]
- `Boolean ContainsLinkedCharacteristicsGroups(ClassObject classObject)` [has Async]
- `ParameterGroup[] GetLinkedCharacteristicsGroups(ClassObject classObject)` [has Async]
- `Boolean ClearLinkedCharacteristics(OneToManyLink characteristicsLink, ReferenceObjectSaveSet saveSet) (+1)` [has Async]
- `IReadOnlyList`1 CreateCharacteristicsByClassifiers(ReferenceObject referenceObject, Guid linkGuid, List`1 characteristicTypes)`

### `CharacteristicReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Characteristics`)
**Свойства:** Class: CharacteristicType, ValueString: StringParameter, ValueYesNo: BooleanParameter, Summary: StringParameter, IsVariable: BooleanParameter, LowerTolerance: DoubleParameter, IsPercent: BooleanParameter, Name: StringParameter, Symbol: StringParameter, IsTolerancePercent: BooleanParameter, UpperTolerance: DoubleParameter, CharacterTypeID: Int32Parameter, ValueDate: DateTimeParameter, ValueReal: DoubleParameter, MinValue: DoubleParameter, DataType: Int32Parameter, ValueInteger: Int32Parameter, MaxValue: DoubleParameter, Mode: StringParameter, Comment: StringParameter, LinkCharacterMeasure: ReferenceObject, CharacteristicGroup: CharacteristicGroupReferenceObject
**Методы:**
- `Boolean CharacteristicUsagesExists(OneToManyLink toCharacteristicsLink) (+1)` [has Async]

### `CharacteristicType` (Namespace: `TFlex.DOCs.Model.References.Characteristics`)
**Свойства:** Classes: CharacteristicTypes, IsCharacterReference: Boolean

### `CharacteristicTypes` (Namespace: `TFlex.DOCs.Model.References.Characteristics`)
**Свойства:** CharacteristicReference: CharacteristicType

### `ParameterGroupExtensions` (Namespace: `TFlex.DOCs.Model.References.Characteristics.Extensions`)
**Методы:**
- `Boolean IsCharacteristicLink(ParameterGroup parameterGroup)`

### `CodifierReference` (Namespace: `TFlex.DOCs.Model.References.Codifier`)
**Свойства:** Classes: CodifierTypes

### `CodifierReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Codifier`)
**Свойства:** Class: CodifierType, Name: StringParameter, NumberParameter: StringParameter, InnerReference: GuidParameter, Context: StringParameter, ForceIncrementCounter: BooleanParameter, CounterReferenceObjects: List`1, ReferenceObjectTypes: ReferenceObjectCollection, NumberElements: ReferenceObjectCollection
**Методы:**
- `String GetNextNumber(ReferenceObject referenceObject)`
- `String GetTestNumber(IList`1 textParametersValues)`
- `Void AddLinkedCounter(ReferenceObject counterRegistryReferenceObject)`
- `Boolean ResetCounter()`
- `Void RenumberAllObjects(ServerConnection connection, CancellationToken cancellationToken, IProgress`1 progress)`
- `Boolean HasMatchWithCurrentCodeTemplate(ReferenceObject referenceObject, String code)`
- `ReferenceObject CreateNumberElements(Guid listObjectClass) (+1)`

### `CodifierType` (Namespace: `TFlex.DOCs.Model.References.Codifier`)
**Свойства:** Classes: CodifierTypes, IsCodifier: Boolean

### `CodifierTypes` (Namespace: `TFlex.DOCs.Model.References.Codifier`)
**Свойства:** Codifier: CodifierType

### `IGetNumberStrategy` (Namespace: `TFlex.DOCs.Model.References.Codifier.CounterLogic`)
**Методы:**
- `String GetNumber(CodifierReferenceObject autoNumeratorReferenceObject, ReferenceObject contextReferenceObject)`

### `ICounterSelectionRule` (Namespace: `TFlex.DOCs.Model.References.Codifier.Macros`)
**Методы:**
- `String Run(IDictionary`2 calculatedObjects)`

### `CounterElementReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Codifier.NumberElements`)
**Свойства:** IsNullable: BooleanParameter, NullableConditions: Int32Parameter, MaxCounterValue: Int32Parameter, StartWith: Int32Parameter, Step: Int32Parameter, PositionCount: Int32Parameter, RestartDay: Int32Parameter, RestartMonth: Int32Parameter, CounterSelectionRule: StringParameter
**Методы:**
- `String GetTestValue(String parameter)`
- `String GetRegexTemplate(ReferenceObject referenceObject)`

### `CurrentDateElementReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Codifier.NumberElements`)
**Свойства:** Format: StringParameter
**Методы:**
- `String GetTestValue(String parameter)`

### `FormulaElementReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Codifier.NumberElements`)
**Свойства:** FormulaText: StringParameter
**Методы:**
- `String GetTestValue(String parameter)`

### `MonthElementReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Codifier.NumberElements`)
**Свойства:** ShowLeadingZeros: BooleanParameter
**Методы:**
- `String GetTestValue(String parameter)`

### `NumberElementsReference` (Namespace: `TFlex.DOCs.Model.References.Codifier.NumberElements`)
**Свойства:** Classes: NumberElementsTypes

### `NumberElementsReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Codifier.NumberElements`)
**Свойства:** Class: NumberElementsType, Name: StringParameter
**Методы:**
- `String GetTextValue(ReferenceObject referenceObject)`
- `String GetTestValue(String parameter)`
- `String GetRegexTemplate(ReferenceObject referenceObject)`

### `NumberElementsType` (Namespace: `TFlex.DOCs.Model.References.Codifier.NumberElements`)
**Свойства:** Classes: NumberElementsTypes, IsNumberElements: Boolean, IsYear: Boolean, IsMonth: Boolean, IsSomeText: Boolean, IsCounter: Boolean, IsParameterText: Boolean, IsCurrentDate: Boolean, IsFormula: Boolean

### `NumberElementsTypes` (Namespace: `TFlex.DOCs.Model.References.Codifier.NumberElements`)
**Свойства:** NumberElements: NumberElementsType, Year: NumberElementsType, Month: NumberElementsType, SomeText: NumberElementsType, Counter: NumberElementsType, ParameterText: NumberElementsType, CurrentDate: NumberElementsType, Formula: NumberElementsType

### `ParameterTextElementReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Codifier.NumberElements`)
**Свойства:** Parameter: StringParameter, StartPosition: Int32Parameter, SymbolCount: Int32Parameter
**Методы:**
- `String GetTestValue(String parameter)`

### `TextElementReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Codifier.NumberElements`)
**Методы:**
- `String GetTestValue(String parameter)`

### `YearElementReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Codifier.NumberElements`)
**Свойства:** ShowOnlyLastTwoDigits: BooleanParameter
**Методы:**
- `String GetTestValue(String parameter)`

### `CustomCriteriaValue` (Namespace: `TFlex.DOCs.Model.References.Configurators`)
**Свойства:** Criteria: CustomCriteria, Apply: Boolean, Value: PossibleValue, VariableValues: Dictionary`2
**Методы:**
- `XmlSchema GetSchema()`
- `Void ReadXml(XmlReader reader)`
- `Void WriteXml(XmlWriter writer)`

### `CriteriaValueSerializeManager` (Namespace: `TFlex.DOCs.Model.References.Configurators.Criterias`)
**Методы:**
- `Object Deserialize(ConfigurationCriteria criteria, String valueString)`
- `String Serialize(ConfigurationCriteria criteria, Object value)`
- `Boolean IsSerializedValueValid(ConfigurationCriteria criteria, String valueString)`

### `CustomCriteriaValueData` (Namespace: `TFlex.DOCs.Model.References.Configurators.Criterias`)
**Свойства:** CustomCriteriaValues: List`1, SelectRevisionsFilterCriteriaValue: CustomCriteriaValue
**Методы:**
- `XmlSchema GetSchema()`
- `Void ReadXml(XmlReader reader)`
- `Void WriteXml(XmlWriter writer)`

### `ConversionTaskExtension` (Namespace: `TFlex.DOCs.Model.References.ConversionTaskQueue`)
**Методы:**
- `Void UpdateConvertedFiles(ICollection`1 conversionTasks)`

### `ConversionTaskQueueReference` (Namespace: `TFlex.DOCs.Model.References.ConversionTaskQueue`)
**Свойства:** Classes: ConversionTaskQueueTypes
**Методы:**
- `Queue`1 GetConversionTaskQueue(TaskGroupReferenceObject taskGroupReferenceObject, Int32 maxCount) (+2)`
- `ICollection`1 GetActiveTaskGroups()`
- `TaskGroupReferenceObject AddTaskGroup(String name, FolderObject saveObject, ReferenceObject instanceConversionService) (+1)`

### `ConversionTaskQueueReferenceObject` (Namespace: `TFlex.DOCs.Model.References.ConversionTaskQueue`)
**Свойства:** Class: ConversionTaskQueueType, Name: StringParameter, Status: Int32Parameter, ConversionStatus: ConversionStatusType
**Методы:**
- `Void AddConvertedFile(FileReferenceObject file)`

### `ConversionTaskQueueType` (Namespace: `TFlex.DOCs.Model.References.ConversionTaskQueue`)
**Свойства:** Classes: ConversionTaskQueueTypes, IsTaskGroup: Boolean, IsConversionTask: Boolean, IsSecondaryRepresentationTask: Boolean

### `ConversionTaskQueueTypes` (Namespace: `TFlex.DOCs.Model.References.ConversionTaskQueue`)
**Свойства:** TaskGroup: ConversionTaskQueueType, ConversionTask: ConversionTaskQueueType, SecondaryRepresentationTask: ConversionTaskQueueType

### `ConversionTaskReferenceObject` (Namespace: `TFlex.DOCs.Model.References.ConversionTaskQueue`)
**Свойства:** ObjectESP: GuidParameter, ErrorText: StringParameter, ConversionConfigurationSettings: StringParameter, ConversionFormat: StringParameter, OutputFileName: StringParameter, AttachFilesToObject: BooleanParameter, ConversionModule: FileConversionModuleReferenceObject, SourceFile: ReferenceObject, ConversionParameters: ReferenceObject, ConversionFormatObject: ConversionFormatReferenceObject, PathPrototypeFile: StringParameter, ConvertedFiles: ReferenceObjectCollection
**Методы:**
- `Void SetConversionFormat(String format)`

### `SecondaryRepresentationTaskReferenceObject` (Namespace: `TFlex.DOCs.Model.References.ConversionTaskQueue`)
**Свойства:** SecondaryRepresentationTemplate: GuidParameter

### `TaskGroupReferenceObject` (Namespace: `TFlex.DOCs.Model.References.ConversionTaskQueue`)
**Свойства:** SaveFolder: FolderObject, InstanceConversionService: ReferenceObject
**Методы:**
- `ConversionTaskReferenceObject AddConversionTask(FileConversionModuleReferenceObject conversationModule, FileObject sourceFile, Guid objectESP, String conversionFormat, String outputFileName, Boolean attachFilesToObject) (+4)`
- `ICollection`1 AddConversionTasks(FileConversionModuleReferenceObject conversationModule, IReadOnlyCollection`1 sourceFiles, Guid objectESP, ConversionFormatReferenceObject conversionFormatReferenceObject, String outputFileName, Boolean attachFilesToObject, String prototypeFile, String conversionConfigurationSettings) (+9)`
- `ICollection`1 AddSecondaryRepresentationTasks(FileConversionModuleReferenceObject conversationModule, FileObject sourceFile, IReadOnlyCollection`1 secondaryRepresentationParamsList, Boolean attachSecondaryFilesToMasterFile)`
- `Queue`1 GetConversionTaskQueue(Int32 maxCount) (+1)`
- `Void UpdateTaskGroupStatus()`

### `ConversionFormatReference` (Namespace: `TFlex.DOCs.Model.References.ConversionTaskQueue.ConversionFormat`)
**Свойства:** Classes: ConversionFormatTypes

### `ConversionFormatReferenceObject` (Namespace: `TFlex.DOCs.Model.References.ConversionTaskQueue.ConversionFormat`)
**Свойства:** Class: ConversionFormatType, Name: StringParameter, SupportedFileTypes: StringParameter, Disable: BooleanParameter, Format: StringParameter

### `ConversionFormatType` (Namespace: `TFlex.DOCs.Model.References.ConversionTaskQueue.ConversionFormat`)
**Свойства:** Classes: ConversionFormatTypes, IsConversionFormatReferenceObject: Boolean

### `ConversionFormatTypes` (Namespace: `TFlex.DOCs.Model.References.ConversionTaskQueue.ConversionFormat`)
**Свойства:** ConversionFormatReferenceObject: ConversionFormatType

### `FileConversionModuleReference` (Namespace: `TFlex.DOCs.Model.References.ConversionTaskQueue.FileConversionModule`)
**Свойства:** Classes: FileConversionModuleTypes
**Методы:**
- `FileConversionModuleReferenceObject Add(String name, String path, String description)`
- `TFlexCadConversionModuleReferenceObject GetDefaultTFlexCadConversionModule()`

### `FileConversionModuleReferenceObject` (Namespace: `TFlex.DOCs.Model.References.ConversionTaskQueue.FileConversionModule`)
**Свойства:** Class: FileConversionModuleType, Name: StringParameter, Path: StringParameter, Description: StringParameter, ConversionTimeoutMinutes: Int32Parameter, MaxConversionAttempts: ByteParameter, WorkerCount: ByteParameter, LinkToConversionModule: ReferenceObject

### `FileConversionModuleType` (Namespace: `TFlex.DOCs.Model.References.ConversionTaskQueue.FileConversionModule`)
**Свойства:** Classes: FileConversionModuleTypes, IsFileConversionModuleReferenceObject: Boolean, IsTFlexCadConversionModuleReferenceObject: Boolean

### `FileConversionModuleTypes` (Namespace: `TFlex.DOCs.Model.References.ConversionTaskQueue.FileConversionModule`)
**Свойства:** FileConversionModuleReferenceObject: FileConversionModuleType, TFlexCadConversionModuleReferenceObject: FileConversionModuleType

### `TFlexCadConversionModuleReferenceObject` (Namespace: `TFlex.DOCs.Model.References.ConversionTaskQueue.FileConversionModule`)
**Свойства:** PathInRegistry: StringParameter, CADVersionFrom: StringParameter, CADVersionTo: StringParameter, LinkApplicationIntegrationConfigurationRules: ApplicationsRelationsProfileReferenceObject

### `CredentialsReference` (Namespace: `TFlex.DOCs.Model.References.Credentials`)
**Свойства:** Classes: CredentialsTypes

### `CredentialsReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Credentials`)
**Свойства:** Class: CredentialsType, Name: StringParameter, IsEnabled: BooleanParameter, CredentialTenants: ReferenceObjectCollection`1, CredentialTargets: ReferenceObjectCollection`1, CredentialBusinessProcess: ReferenceObjectCollection, CredentialOwnerUsers: ReferenceObjectCollection`1
**Методы:**
- `CredentialsTenantListObject CreateCredentialTenants(Guid listObjectClass) (+1)`
- `CredentialsTargetListObject CreateCredentialTargets(Guid listObjectClass) (+1)`
- `ReferenceObject AddCredentialBusinessProcess(ReferenceObject newLinkedObject)`
- `Boolean RemoveCredentialBusinessProcess(ReferenceObject linkedObject)`
- `ReferenceObject AddCredentialOwnerUsers(UserReferenceObject newLinkedObject)`
- `Boolean RemoveCredentialOwnerUsers(UserReferenceObject linkedObject)`

### `CredentialsTargetList` (Namespace: `TFlex.DOCs.Model.References.Credentials`)
**Свойства:** Classes: CredentialsTargetTypes

### `CredentialsTargetListObject` (Namespace: `TFlex.DOCs.Model.References.Credentials`)
**Свойства:** Class: CredentialsTargetType, Name: StringParameter, ItemGuid: GuidParameter, ItemStringId: StringParameter, AccessGroupGuid: GuidParameter, IsEnabled: BooleanParameter, AccessRightPK: Int32Parameter, CredentialOwner: GuidParameter, Objects: AnyReferenceLink
**Методы:**
- `ReferenceObject AddObject(ReferenceObject newLinkedObject)`
- `Boolean RemoveObject(ReferenceObject linkedObject)`

### `CredentialsTargetType` (Namespace: `TFlex.DOCs.Model.References.Credentials`)
**Свойства:** Classes: CredentialsTargetTypes, IsCredentialsTargetListObject: Boolean

### `CredentialsTargetTypes` (Namespace: `TFlex.DOCs.Model.References.Credentials`)
**Свойства:** CredentialsTargetListObject: CredentialsTargetType

### `CredentialsTenantList` (Namespace: `TFlex.DOCs.Model.References.Credentials`)
**Свойства:** Classes: CredentialsTenantListTypes

### `CredentialsTenantListObject` (Namespace: `TFlex.DOCs.Model.References.Credentials`)
**Свойства:** Class: CredentialsTenantListType, StartDate: DateTimeParameter, EndDate: DateTimeParameter, Name: StringParameter, TenantUser: UserReferenceObject

### `CredentialsTenantListType` (Namespace: `TFlex.DOCs.Model.References.Credentials`)
**Свойства:** Classes: CredentialsTenantListTypes, IsCredentialsTenantListObject: Boolean

### `CredentialsTenantListTypes` (Namespace: `TFlex.DOCs.Model.References.Credentials`)
**Свойства:** CredentialsTenantListObject: CredentialsTenantListType

### `CredentialsType` (Namespace: `TFlex.DOCs.Model.References.Credentials`)
**Свойства:** Classes: CredentialsTypes, IsCredentialType: Boolean

### `CredentialsTypes` (Namespace: `TFlex.DOCs.Model.References.Credentials`)
**Свойства:** CredentialType: CredentialsType

### `DataModelItemReferenceObject` (Namespace: `TFlex.DOCs.Model.References.DataModels`)
**Свойства:** Relation: StringParameter, ReferenceGuid: GuidParameter, LoadToCAD: BooleanParameter, Class: DataModelsType, IsReference: Boolean, IsRelation: Boolean, DisplayName: StringParameter
**Методы:**
- `ReferencePath GetReferencePath()`

### `DataModelObject` (Namespace: `TFlex.DOCs.Model.References.DataModels`)
**Свойства:** IsDefault: BooleanParameter

### `DataModelReferenceObject` (Namespace: `TFlex.DOCs.Model.References.DataModels`)
**Свойства:** Name: StringParameter, Class: DataModelsType

### `DataModelsReference` (Namespace: `TFlex.DOCs.Model.References.DataModels`)
**Свойства:** Classes: DataModelTypes
**Методы:**
- `ReferenceObjectCollection GetDefaultReferenceDataModelsCollection(Guid reference)` [has Async]

### `DataModelsType` (Namespace: `TFlex.DOCs.Model.References.DataModels`)
**Свойства:** Classes: DataModelTypes, IsRelation: Boolean, IsReference: Boolean, IsDataModel: Boolean

### `DataModelTypes` (Namespace: `TFlex.DOCs.Model.References.DataModels`)
**Свойства:** Relation: DataModelsType, Reference: DataModelsType, DataModel: DataModelsType

### `DocumentReference` (Namespace: `TFlex.DOCs.Model.References.Documents`)
**Свойства:** Classes: DocumentTypes

### `DocumentType` (Namespace: `TFlex.DOCs.Model.References.Documents`)
**Свойства:** Classes: DocumentTypes, IsEngineeringDesignDocument: Boolean, IsProductDocument: Boolean, IsDetail: Boolean, IsAssembly: Boolean, IsDrawing: Boolean, IsAssemblyDrawing: Boolean

### `DocumentTypes` (Namespace: `TFlex.DOCs.Model.References.Documents`)
**Свойства:** EngineeringDesignDocument: DocumentType, ProductDocument: DocumentType, Detail: DocumentType, Assembly: DocumentType, Product: DocumentType, Drawing: DocumentType, AssemblyDrawing: DocumentType

### `EngineeringDocumentObject` (Namespace: `TFlex.DOCs.Model.References.Documents`)
**Свойства:** Name: String, Denotation: String, Code: String, Letter: String, Mass: Double
**Методы:**
- `Void AddFile(FileObject file)`
- `List`1 GetFiles()`
- `Void SaveFileContext(Byte[] context, FileObject file)`

### `ProductDocumentObject` (Namespace: `TFlex.DOCs.Model.References.Documents`)
**Свойства:** MaterialsLink: OneToManyRelation, MainMaterialLink: OneToOneLink, BasicMaterial: MaterialReferenceObject, MaterialsMark: AbstractMarkReferenceObject

### `EntryPoint` (Namespace: `TFlex.DOCs.Model.References.Events`)
**Свойства:** Name: String, Title: String, ReturnType: Type
**Методы:**
- `IEntryPointParameter[] GetParameters()`
- `TAttribute GetAttribute()`

### `IEntryPoint` (Namespace: `TFlex.DOCs.Model.References.Events`)
**Свойства:** Name: String, Title: String, ReturnType: Type
**Методы:**
- `IEntryPointParameter[] GetParameters()`

### `IEntryPointParameter` (Namespace: `TFlex.DOCs.Model.References.Events`)
**Свойства:** Name: String, Title: String, Type: Type, IsOptional: Boolean, DefaultValue: Object

### `IEntryPointProvider` (Namespace: `TFlex.DOCs.Model.References.Events`)
**Методы:**
- `IEnumerable`1 GetEntryPoints()`

### `IEntryPointWithConnectionProvider` (Namespace: `TFlex.DOCs.Model.References.Events`)
**Методы:**
- `IEnumerable`1 GetEntryPoints(ServerConnection connection)` [has Async]

### `ISystemEventHandlerProvider` (Namespace: `TFlex.DOCs.Model.References.Events`)
**Свойства:** Guid: Guid, HandlerName: String
**Методы:**
- `Object Run(MacroContext context, String entryPoint, Object[] args)`
- `Boolean SupportsParameterGroup(Guid parameterGroupGuid)`

### `ParameterGroupEvent` (Namespace: `TFlex.DOCs.Model.References.Events`)
**Свойства:** Id: Int32, Guid: Guid, Name: String, IsSystem: Boolean
**Методы:**
- `Int32 CompareTo(ParameterGroupEvent other)`

### `PositionInMenuExtensions` (Namespace: `TFlex.DOCs.Model.References.Events`)
**Методы:**
- `String GetName(PositionInMenu positionInMenu)`

### `UserEvent` (Namespace: `TFlex.DOCs.Model.References.Events`)
**Свойства:** Events: EventCollection, Name: String, Button: UserEventButtonInfo, IsAdded: Boolean, IsModified: Boolean
**Методы:**
- `Boolean Save()` [has Async]
- `Boolean Delete()` [has Async]

### `UserEventButtonInfo` (Namespace: `TFlex.DOCs.Model.References.Events`)
**Свойства:** Event: UserEvent, Text: String, MenuCaption: String, Hint: String, ValidateMethod: String, ShortcutKeys: String, FilterEnable: Filter, FilterVisible: Filter, Position: PositionInMenu, DisplayMode: CommandDisplayMode, EditObject: Boolean, AllowedForUsers: Boolean, XmlUsers: Int32[], XmlUsersIds: Guid[], Users: UserReferenceObject[], ExecuteForEachObject: Boolean, Icon: IconImage, PopupIcon: IconImage, IconBytes: Byte[], PopupIconBytes: Byte[]
**Методы:**
- `Boolean AllowedForUser(Int32 userId)` [has Async]

### `DesktopObjectWithLink` (Namespace: `TFlex.DOCs.Model.References.Extensions`)
**Свойства:** DesktopObject: DesktopObject, Link: ComplexHierarchyLink, ReferenceObjectWithLink: ReferenceObjectWithLink

### `ReferenceObjectCollectionExtensions` (Namespace: `TFlex.DOCs.Model.References.Extensions`)
**Методы:**
- `HashSet`1 GetLoadedObjects(IReferenceObjectCollection objectCollection, Boolean includeParent)`
- `Void FillEmptyParameters(IEnumerable`1 objects)`

### `AdditionalReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Files`)
**Свойства:** ReferenceObject: Guid, HierarchyLink: Guid

### `FileContainerObject` (Namespace: `TFlex.DOCs.Model.References.Files`)
**Свойства:** IsFileContainer: Boolean
**Методы:**
- `String GetHeadRevision(String destinationPath)` [has Async]
- `Boolean DeleteFromWorkingFolder()`
- `Boolean IsActualVersionDownloaded()` [has Async]

### `FileDuplicateAuthorParameter` (Namespace: `TFlex.DOCs.Model.References.Files`)
**Свойства:** IsReadOnly: Boolean
**Методы:**
- `Int32 GetInt32()`
- `TypeCode GetTypeCode()`

### `FileDuplicateSourceParameter` (Namespace: `TFlex.DOCs.Model.References.Files`)
**Свойства:** IsReadOnly: Boolean

### `FileExtensionAttribute` (Namespace: `TFlex.DOCs.Model.References.Files`)
**Свойства:** CanChangeCaption: Boolean, IsSystem: Boolean, Caption: String, Value: Object, CanRemove: Boolean

### `FileLinkAdditionalSettings` (Namespace: `TFlex.DOCs.Model.References.Files`)
**Свойства:** Data: FileLinkAdditionalSettingsData, IsValid: Boolean, IsLoaded: Boolean, LinkGroup: ParameterGroup, LinkClass: ClassObject
**Методы:**
- `Void Reload()`
- `Boolean Save()`
- `Boolean Remove()`

### `FileLinkAdditionalSettingsData` (Namespace: `TFlex.DOCs.Model.References.Files`)
**Свойства:** DefaultPath: String, DefaultFileName: String, CreateUserSubfolder: Boolean, ShowSelectFolderDialog: Boolean, ForbidSelectionFromOtherFolders: Boolean, PathIsMacro: Boolean, FileNameIsMacro: Boolean
**Методы:**
- `String Serialize()`
- `Void Deserialize(String data)`

### `FileLinkAdditionalSettingsDataExtensions` (Namespace: `TFlex.DOCs.Model.References.Files`)
**Методы:**
- `FolderObject GetDefaultFolder(FileLinkAdditionalSettingsData data, LinkInfo link, Boolean createIfNotExist, ReferenceObject parentMasterObject)`
- `String GetDefaultFileName(FileLinkAdditionalSettingsData data, LinkInfo link, FileReferenceObject prototype)`

### `FileObject` (Namespace: `TFlex.DOCs.Model.References.Files`)
**Свойства:** IsFile: Boolean, Size: Int64, LastChangeDate: DateTime, HasLinkedFiles: Boolean, IsModified: Boolean, IsChanged: Boolean, RepresentationType: RepresentationType, MethodRepresentation: MethodRepresentationType, RepresentationCode: String, RepresentationName: String, LOD: Int32
**Методы:**
- `List`1 GetTypicalRepresentations()`
- `Void SetTypicalRepresentations(ICollection`1 secondaryRepresentations)`
- `List`1 GetSecondaryFileRepresentations()`
- `Void SetSecondaryFileRepresentations(IEnumerable`1 secondaryFiles)`
- `Void SetMasterFile(FileObject masterFile)`
- `FileObject GetMasterFile()`
- `Void SetRepresentationTemplate(TypicalRepresentationsReferenceObject typicalRepresentation)`
- `TypicalRepresentationsReferenceObject GetRepresentationTemplate()`
- `List`1 GetLinkedFiles()` [has Async]
- `Void SetLinkedFiles(IEnumerable`1 files)`
- `FileReferenceObject AddLinkedFile(FileReferenceObject file)`
- `Boolean RemoveLinkedFile(FileReferenceObject file)`
- `Boolean IsActualVersionDownloaded()` [has Async]
- `String GetHeadRevision(String destinationPath)` [has Async]
- `String GetFileVersion(Int32 version, Boolean loadLinkedFiles) (+1)` [has Async]
- `Boolean DeleteFileVersion(Int32 version)` [has Async]
- `Boolean DeleteFromWorkingFolder()`
- `Void Export(String destinationPath, Boolean clearReadOnly)`
- `Void SetOpenningDocumentId(String path, Int32 objectId, Int32 referenceId)`
- `Void SetOpenDocumentContext(String path, Guid referenceObject, Guid hierarchyLink, Int32 referenceId, String filter, String mainFileTypeInStructure, Boolean isLaunchedPdm) (+5)`
- `OpenDocumentContext GetOpenDocumentContext(String path) (+1)`
- `Boolean GetOpenningObjectId(String path, Int32& objectId, Int32& referenceId)`
- `Void ClearOpenningDocumentId()`
- `Void SaveFileContext(Byte[] context, DocumentReferenceObject document)`
- `Byte[] FindFileContext(DocumentReferenceObject document)`
- `FileObject CreateCopy(String newName, FolderObject parent, ReferenceObjectSaveSet saveSet) (+1)`
- `FileObject CreateFileDuplicate(FolderObject parentFolder)`
- `Void MoveFileToFolder(FolderObject newParentFolder)`
- `Void CopyTo(FileObject destinationFile)`
- `Byte[] GetSigningReferenceObjectData()`

### `FilePreviewerAttribute` (Namespace: `TFlex.DOCs.Model.References.Files`)
**Свойства:** CanChangeCaption: Boolean, IsSystem: Boolean, Caption: String, Value: Object, CanRemove: Boolean

### `FilePreviewerTypeAttribute` (Namespace: `TFlex.DOCs.Model.References.Files`)
**Свойства:** CanChangeCaption: Boolean, Caption: String, IsSystem: Boolean, Value: Object, CanRemove: Boolean

### `FileReference` (Namespace: `TFlex.DOCs.Model.References.Files`)
**Свойства:** Classes: FileTypes, FileServers: FileServerReference
**Методы:**
- `FileReferenceObject FindByPath(String path)` [has Async]
- `ICollection`1 FindByPaths(ICollection`1 paths)` [has Async]
- `FileReferenceObject FindByRelativePath(String relativePath)` [has Async]
- `ICollection`1 FindByRelativePaths(ICollection`1 relativePaths)` [has Async]
- `FolderObject Import(String sourceFolder, FolderObject destinationFolder) (+1)` [has Async]
- `FileObject AddFile(String fileName, Stream stream, FolderObject folder, Boolean executeCallBack) (+2)` [has Async]
- `List`1 AddFiles(IReadOnlyCollection`1 filesData, FolderObject folder, Boolean executeCallBack) (+1)` [has Async]
- `FolderObject CreateFolder(String description, String name, ImportParameters parameters)` [has Async]
- `FolderObject CreatePath(String path, FolderObject parentFolder, ImportParameters parameters)` [has Async]
- `Void GetHeadRevision(IEnumerable`1 files)` [has Async]
- `Boolean DeleteFilesVersions(List`1 files)` [has Async]
- `FileType GetFileType(String fileName, Boolean createIfNotExists)` [has Async]

### `FileReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Files`)
**Свойства:** Class: FileType, Children: ReferenceObjectCollection`1, IsFile: Boolean, IsFolder: Boolean, IsFileContainer: Boolean, Parent: FolderObject, Server: FileServerParameter, Path: StringParameter, Name: StringParameter, Comment: StringParameter, Code: Int32Parameter, LevelOfDetail: Int32Parameter, LocalPath: String
**Методы:**
- `Boolean IsActualVersionDownloaded()` [has Async]
- `Void ValidateName(String name, Boolean isFolder)`
- `Void GetHeadRevision(Boolean loadLinkedFiles) (+2)` [has Async]
- `Boolean DeleteFromWorkingFolder()`
- `Void Export(String destinationPath, Boolean clearReadOnly)`
- `Boolean CanCopy(ParameterGroup relation)`

### `FileServerParameter` (Namespace: `TFlex.DOCs.Model.References.Files`)
**Методы:**
- `Int32 GetInt32()`
- `Int64 GetInt64()`
- `TypeCode GetTypeCode()`

### `FileSizeParameter` (Namespace: `TFlex.DOCs.Model.References.Files`)
**Свойства:** IsNull: Boolean, IsReadOnly: Boolean
**Методы:**
- `Int64 GetInt64()`
- `TypeCode GetTypeCode()`

### `FileType` (Namespace: `TFlex.DOCs.Model.References.Files`)
**Свойства:** Name: String, Classes: FileTypes, IsFolder: Boolean, IsFile: Boolean, IsFileContainer: Boolean, IsGRBFile: Boolean, CanContainChildren: Boolean, Attributes: FileTypeAttributes, Extension: String, Icon: IconImage, DefaultFilePreviewerGuid: Guid, CustomFilePreviewerGuid: Guid, DefaultFilePreviewerType: FilePreviewerTypes, CustomFilePreviewerType: FilePreviewerTypes

### `FileTypeAttributes` (Namespace: `TFlex.DOCs.Model.References.Files`)
**Свойства:** Extension: FileExtensionAttribute, DefaultFilePreviewer: FilePreviewerAttribute, DefaultFilePreviewerType: FilePreviewerTypeAttribute, ProtectedPreview: ProtectedPreviewAttribute

### `FileTypes` (Namespace: `TFlex.DOCs.Model.References.Files`)
**Свойства:** Folder: FileType, FileBase: FileType, TFlexCADFileBase: FileType, FileContainer: FileType
**Методы:**
- `FileType GetFileTypeByExtension(String extension)`
- `FileType CreateFileType(String name, String comment, String extension, FileType baseType, Guid customFilePreviewer, Guid defaultFilePreviewer, FilePreviewerTypes customFilePreviewerType, FilePreviewerTypes defaultFilePreviewerType) (+2)` [has Async]
- `Void ModifyFileType(FileType type, String name, String comment, String extension, Guid customFilePreviewer, Guid defaultFilePreviewer, FilePreviewerTypes customFilePreviewerType, FilePreviewerTypes defaultFilePreviewerType) (+1)` [has Async]
- `Void DeleteFileType(FileType type)` [has Async]

### `FolderObject` (Namespace: `TFlex.DOCs.Model.References.Files`)
**Свойства:** IsFolder: Boolean
**Методы:**
- `Void Rename(String newName)`
- `Void MoveTo(FolderObject otherFolder)`
- `Boolean SetParent(ReferenceObject parentObject)`
- `FileObject CreateFile(String source, String description, String name, FileType type, ImportParameters parameters)` [has Async]
- `FolderObject CreateFolder(String description, String name, ImportParameters parameters)` [has Async]
- `FileContainerObject CreateFileContainer(String source, String description, String name, ImportParameters parameters)` [has Async]
- `FolderObject CreatePath(String path, ImportParameters parameters)`
- `Void Load(Boolean recursive, Boolean getHeadRevisions, List`1& notActualFiles) (+1)` [has Async]
- `String GetHeadRevision(String destinationPath)` [has Async]
- `Boolean DeleteFromWorkingFolder()`
- `Void Export(String destinationPath, Boolean clearReadOnly, Boolean recursive) (+1)`

### `GrbFileObject` (Namespace: `TFlex.DOCs.Model.References.Files`)
**Методы:**
- `ICollection`1 LoadTfrFiles()`
- `Boolean HasTfrFilesOnServer()`

### `ILinkAdditionalSettingsOwner` (Namespace: `TFlex.DOCs.Model.References.Files`)
**Свойства:** ParameterGroup: ParameterGroup, ClassOfGroup: ClassObject

### `ImportFileCallback` (Namespace: `TFlex.DOCs.Model.References.Files`)
**Методы:**
- `Boolean Invoke(String filePath)`
- `IAsyncResult BeginInvoke(String filePath, AsyncCallback callback, Object object)`
- `Boolean EndInvoke(IAsyncResult result)`

### `ImportParameters` (Namespace: `TFlex.DOCs.Model.References.Files`)
**Свойства:** DestinationFolder: FolderObject, Recursive: Boolean, CreateClasses: Boolean, AutoCheckIn: Boolean, UpdateExistingFiles: Boolean, ImportedObjects: List`1, ImportedFiles: List`1, ImportFileCallback: ImportFileCallback, FileExistsCallback: ImportFileCallback, RequiredParametersExceptionCallback: Func`2

### `LastChangeDateParameter` (Namespace: `TFlex.DOCs.Model.References.Files`)
**Свойства:** IsNull: Boolean, IsReadOnly: Boolean
**Методы:**
- `DateTime GetDateTime()`
- `TypeCode GetTypeCode()`

### `LegacySlaveFileInfo` (Namespace: `TFlex.DOCs.Model.References.Files`)
**Свойства:** Guid: Guid, Path: String, Id1: Int32, Id2: Int32, Id3: Int32, Id4: Int32
**Методы:**
- `List`1 GetSlaveFiles(ServerConnection connection, Guid masterFileGuid) (+1)`

### `OpenDocumentContext` (Namespace: `TFlex.DOCs.Model.References.Files`)
**Свойства:** Reference: Int32, ReferenceObject: Guid, HierarchyLink: Guid, AdditionalReferenceObjects: List`1, StructureConfiguration: String, MainFileTypeInStructure: String, IsLaunchedPdm: Boolean, IsSpecialConfigurationSettings: Boolean, IsVirtualAssembly: Boolean, ReferenceObjectInstance: Guid, WorkSessionParameters: WorkSessionParameters

### `OpenDocumentManager` (Namespace: `TFlex.DOCs.Model.References.Files`)
**Методы:**
- `Void Write(String fileKey, OpenDocumentContext context) (+1)`
- `OpenDocumentContext Read(String fileKey) (+1)`
- `Void Clear()`

### `ProtectedPreviewAttribute` (Namespace: `TFlex.DOCs.Model.References.Files`)
**Свойства:** IsSystem: Boolean, CanChangeCaption: Boolean, Caption: String, Value: Object, CanRemove: Boolean

### `ReferenceFiles` (Namespace: `TFlex.DOCs.Model.References.Files`)
**Методы:**
- `Void Load(FileReferenceObject fileObject, Func`2 condition) (+2)` [has Async]
- `Dictionary`2 LoadAsStream(IEnumerable`1 files)` [has Async]
- `Dictionary`2 HasTfrFilesOnServer(ICollection`1 grbFileObjects, CancellationToken cancellationToken)`
- `Dictionary`2 LoadTfrFiles(ICollection`1 grbFileObjects, Boolean updateTfrIfExists, CancellationToken cancellationToken)`

### `TFlexCadContext` (Namespace: `TFlex.DOCs.Model.References.Files`)
**Свойства:** Keys: ICollection`1, Values: ICollection`1, Item: TFlexCadVariableValue, Count: Int32
**Методы:**
- `Byte[] GetData()`
- `Boolean TryGetReal(String name, Double& real)`
- `Boolean TryGetText(String name, String& text)`
- `Void SetReal(String name, Double real)`
- `Void SetText(String name, String text)`
- `Void Add(String name, TFlexCadVariableValue variable)`
- `Boolean ContainsKey(String name)`
- `Boolean Remove(String name)`
- `Boolean TryGetValue(String name, TFlexCadVariableValue& variable)`
- `Void Clear()`
- `IEnumerator`1 GetEnumerator()`

### `TFlexCadVariableValue` (Namespace: `TFlex.DOCs.Model.References.Files`)
**Свойства:** Text: String, Real: Double

### `WorkSessionParameters` (Namespace: `TFlex.DOCs.Model.References.Files`)
**Свойства:** WorkSessionObject: Guid, LoadFiles: Boolean, UnloadWorkObjects: Boolean, LoadByDataModel: Guid

### `FileHistoryLoader` (Namespace: `TFlex.DOCs.Model.References.Files.FileHistory`)
**Методы:**
- `List`1 Load(Nullable`1 beginDate, Nullable`1 endDate) (+1)` [has Async]
- `DownloadsLogEventDescription GetDownloadsLogEventDetails(Int64 eventId) (+1)` [has Async]
- `DownloadsLogFilterParameters GetDownloadsEventFilterParameters()` [has Async]

### `FileHistoryRecord` (Namespace: `TFlex.DOCs.Model.References.Files.FileHistory`)
**Свойства:** EventId: Int64, File: String, ClientView: String, Timestamp: DateTime, FileGuid: Guid, UserId: Int32, HostName: String, ClientAddress: String, LocalPath: String, FileName: String

### `LocalRepresentationGenerator` (Namespace: `TFlex.DOCs.Model.References.Files.Representation`)
**Методы:**
- `Void Generate(ICollection`1 masterFiles)`
- `Void RegenerateSecondaryFiles(ICollection`1 secondaryFiles)`

### `FileServerObject` (Namespace: `TFlex.DOCs.Model.References.FileServers`)
**Свойства:** Name: StringParameter, ServerAddress: StringParameter, Storage: StringParameter, IsDefault: Boolean

### `FileServerReference` (Namespace: `TFlex.DOCs.Model.References.FileServers`)
**Методы:**
- `FileServerObject GetDefaultFileServer()` [has Async]
- `List`1 GetStorages(String serverAddress)`
- `Void SetDefaultFileServer(FileServerObject fileServer)`

### `GlobalParameter` (Namespace: `TFlex.DOCs.Model.References.GlobalParameters`)
**Свойства:** Class: GlobalParameterType, Name: StringParameter, Comment: StringParameter, Category: StringParameter, Value: Parameter, AdministrativeAccessRequired: BooleanParameter

### `GlobalParameterReference` (Namespace: `TFlex.DOCs.Model.References.GlobalParameters`)
**Свойства:** Instance: GlobalParameterReference, Classes: GlobalParameterTypes, Item: GlobalParameter, GlobalCalendar: CalendarReferenceObject, WorkingFolderPath: FormulaMacro, DisableInplaceEdit: Boolean
**Методы:**
- `GlobalParameter Find(String name)` [has Async]
- `Boolean GetIsStageCommentsRequired()` [has Async]
- `Boolean IsOmitHasChildrenCheckEnabled(Boolean catalog)` [has Async]
- `Boolean GetShowAnnotateCommand()` [has Async]

### `GlobalParameterType` (Namespace: `TFlex.DOCs.Model.References.GlobalParameters`)
**Свойства:** Classes: GlobalParameterTypes, IsString: Boolean, IsInt: Boolean, IsReal: Boolean, IsBoolean: Boolean, IsDateTime: Boolean

### `GlobalParameterTypes` (Namespace: `TFlex.DOCs.Model.References.GlobalParameters`)
**Свойства:** String: GlobalParameterType, Int: GlobalParameterType, Real: GlobalParameterType, Boolean: GlobalParameterType, DateTime: GlobalParameterType

### `ImageReference` (Namespace: `TFlex.DOCs.Model.References.Icons`)
**Методы:**
- `ImageReferenceObject ImportIcon(String file)`

### `ImageReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Icons`)
**Свойства:** Name: StringParameter, Icon: IconParameter, Image: ImageParameter, IsIcon: Boolean, IsImage: Boolean
**Методы:**
- `String GetFilterExtensions()`
- `Void SaveToFile(String fileName)`

### `InstallKitReference` (Namespace: `TFlex.DOCs.Model.References.InstallKits`)
**Свойства:** Classes: InstallKitTypes

### `InstallKitReferenceObject` (Namespace: `TFlex.DOCs.Model.References.InstallKits`)
**Свойства:** Class: InstallKitType, Name: StringParameter, Description: StringParameter, Comment: StringParameter, Folder: StringParameter, UsePackage: BooleanParameter, InstallPacketReference: InstallPacketReference, InstallPackets: ReferenceObjectCollection`1
**Методы:**
- `InstallPacketReferenceObject CreateInstallPacket(Guid listObjectClass) (+1)`

### `InstallKitType` (Namespace: `TFlex.DOCs.Model.References.InstallKits`)
**Свойства:** Classes: InstallKitTypes, IsInstallKit: Boolean

### `InstallKitTypes` (Namespace: `TFlex.DOCs.Model.References.InstallKits`)
**Свойства:** InstallKit: InstallKitType

### `InstallPacketActionReference` (Namespace: `TFlex.DOCs.Model.References.InstallPacketActions`)
**Свойства:** Classes: InstallPacketActionTypes

### `InstallPacketActionReferenceObject` (Namespace: `TFlex.DOCs.Model.References.InstallPacketActions`)
**Свойства:** InstallAction: InstallActions, InstallObjectType: InstallObjectTypes, Class: InstallPacketActionType, Denotation: StringParameter, Action: Int32Parameter, Version: StringParameter, ObjectType: Int32Parameter, ObjectGuid: GuidParameter, Value: StringParameter, GroupGuid: GuidParameter, EventGuid: GuidParameter, Property: StringParameter

### `InstallPacketActionType` (Namespace: `TFlex.DOCs.Model.References.InstallPacketActions`)
**Свойства:** Classes: InstallPacketActionTypes, IsInstallPacketAction: Boolean

### `InstallPacketActionTypes` (Namespace: `TFlex.DOCs.Model.References.InstallPacketActions`)
**Свойства:** InstallPacketAction: InstallPacketActionType

### `InstallPacketReference` (Namespace: `TFlex.DOCs.Model.References.InstallPackets`)
**Свойства:** Classes: InstallPacketTypes

### `InstallPacketReferenceObject` (Namespace: `TFlex.DOCs.Model.References.InstallPackets`)
**Свойства:** Class: InstallPacketType, Name: StringParameter, ReferenceGuid: GuidParameter, Settings: StringParameter, InstallFile: FileObject, Objects: AnyReferenceLink, InstallPacketActionReference: InstallPacketActionReference, InstallPacketActions: ReferenceObjectCollection`1
**Методы:**
- `InstallPacketActionReferenceObject CreateInstallPacketAction(Guid listObjectClass) (+1)`

### `InstallPacketType` (Namespace: `TFlex.DOCs.Model.References.InstallPackets`)
**Свойства:** Classes: InstallPacketTypes, IsInstallPacket: Boolean

### `InstallPacketTypes` (Namespace: `TFlex.DOCs.Model.References.InstallPackets`)
**Свойства:** InstallPacket: InstallPacketType

### `AnyReferenceLink` (Namespace: `TFlex.DOCs.Model.References.Links`)
**Свойства:** IsAnyReference: Boolean, IsModified: Boolean, IsLinkedReferenceInitialized: Boolean, IsLinkedObjectIdsLoaded: Boolean, IsEmptyLinkedObjectsId: Boolean, IsEmptyLinkedObjects: Boolean, IsLoaded: Boolean, State: LoadState, CountLoaded: Int32, Item: ReferenceObject
**Методы:**
- `Dictionary`2 GetLinkedObjectsId()` [has Async]
- `IEnumerable`1 GetLinkedObjects()` [has Async]
- `List`1 GetObjectsFromReference(Func`2 getReference, StaticReferenceLoadSettings loadSettings)` [has Async]
- `Boolean Load(Int32 count) (+1)` [has Async]
- `ReferenceObject AddLinkedObject(ReferenceObject linkedObject)` [has Async]
- `ReferenceObject AddLinkedObjectWithNoCopy(ReferenceObject linkedObject, Func`2 getReference)` [has Async]
- `Boolean RemoveLinkedObject(ReferenceObject linkedObject)` [has Async]
- `Boolean RemoveLinkedObjectWithNoCopy(ReferenceObject linkedObject, Func`2 getReference)` [has Async]
- `Void RemoveAll()` [has Async]
- `Void RemoveAllWithNoCopy(Func`2 getReference)` [has Async]
- `Reference GetOrAddLinkedReference(ReferenceInfo info)`
- `Boolean IsObjectAdded(ReferenceObject object)`
- `Boolean IsObjectRemoved(ReferenceObject object)`
- `Int32 IndexOf(ReferenceObject item)`
- `Boolean Contains(ReferenceObject item)`
- `Void CopyTo(ReferenceObject[] array, Int32 arrayIndex)`
- `IEnumerator`1 GetEnumerator()`
- `List`1 FindMasterObjects(ParameterGroup anyReferenceLink, Reference masterReference, ReferenceObject slaveObject)`

### `AnyReferenceLinkManager` (Namespace: `TFlex.DOCs.Model.References.Links`)
**Свойства:** LinkGroups: ParameterGroupCollection

### `LinkFilterSettings` (Namespace: `TFlex.DOCs.Model.References.Links`)
**Свойства:** Filter: Filter
**Методы:**
- `String Serialize()`
- `Void Deserialize(String data)`

### `LinkInfo` (Namespace: `TFlex.DOCs.Model.References.Links`)
**Свойства:** LinkGroup: ParameterGroup, Master: DesktopObject, MasterObject: ReferenceObject, MasterLink: ComplexHierarchyLink, RootMasterObject: ReferenceObject, RootMasterReference: Reference, Swapped: Boolean, IsOneToOne: Boolean, IsOneToMany: Boolean, IsTableOneToMany: Boolean, IsLinkOneToMany: Boolean, IsAnyReference: Boolean, IsSearchQueryLink: Boolean, IsLinkedReferenceInitialized: Boolean, IsLinkedObjectIdsLoaded: Boolean, IsEmptyLinkedObjectsId: Boolean, IsEmptyLinkedObjects: Boolean, IsModified: Boolean, IsChanged: Boolean, IsLoaded: Boolean, State: LoadState, CountLoaded: Int32, LinkReference: Reference
**Методы:**
- `Boolean IsObjectAdded(ReferenceObject object)`
- `Boolean IsObjectRemoved(ReferenceObject object)`
- `Void Clear(Boolean full) (+1)`
- `ClassObjectCollection GetAllowedClassesToLink()`
- `List`1 GetDeletedObjects()` [has Async]

### `OneToManyLink` (Namespace: `TFlex.DOCs.Model.References.Links`)
**Свойства:** IsModified: Boolean, IsLinkOneToMany: Boolean, IsLinkedObjectIdsLoaded: Boolean, IsEmptyLinkedObjectsId: Boolean
**Методы:**
- `List`1 GetLinkedObjectsId()` [has Async]
- `IEnumerable`1 GetLinkedObjects()` [has Async]
- `List`1 GetObjectsFromReference(Reference reference, StaticReferenceLoadSettings loadSettings, Boolean onlyLoadedData)` [has Async]
- `ReferenceObject AddLinkedObject(ReferenceObject linkedObject)` [has Async]
- `ReferenceObject AddLinkedObjectWithNoCopy(ReferenceObject linkedObject)` [has Async]
- `List`1 AddLinkedObjectsWithNoCopy(ICollection`1 linkedObjects) (+1)` [has Async]
- `Boolean RemoveLinkedObject(ReferenceObject linkedObject)` [has Async]
- `Boolean RemoveLinkedObjectWithNoCopy(ReferenceObject linkedObject)` [has Async]
- `Void RemoveAll()` [has Async]
- `Void RemoveAllWithNoCopy(Reference reference)` [has Async]
- `ReferenceObject GetSwappedLinkedObject()`
- `List`1 GetSwappedLinkedObjects()`
- `Boolean IsObjectAdded(ReferenceObject object)`
- `Boolean IsObjectRemoved(ReferenceObject object)`
- `Byte[] GetContext(ReferenceObject linkedObject)` [has Async]
- `Boolean SetContext(ReferenceObject linkedObject, Byte[] context)` [has Async]

### `OneToManyLinkToComplexHierarchy` (Namespace: `TFlex.DOCs.Model.References.Links`)
**Свойства:** IsModified: Boolean, IsLinkOneToMany: Boolean, LinkReference: Reference, Objects: ReferenceObjectCollection, IsLinkedObjectIdsLoaded: Boolean, IsEmptyLinkedObjectsId: Boolean, IsLinkedReferenceInitialized: Boolean, IsEmptyLinkedObjects: Boolean, IsLoaded: Boolean, State: LoadState, CountLoaded: Int32
**Методы:**
- `IReadOnlyCollection`1 GetLinkedComplexLinks()` [has Async]
- `ComplexHierarchyLink AddLinkedComplexLink(ComplexHierarchyLink linkedComplexLink)` [has Async]
- `Boolean RemoveLinkedComplexLink(ComplexHierarchyLink linkedComplexLink)` [has Async]
- `Void RemoveAll()` [has Async]
- `Boolean IsObjectAdded(ReferenceObject object)`
- `Boolean IsObjectRemoved(ReferenceObject object)`
- `Byte[] GetContext(ReferenceObject linkedObject)`
- `IEnumerator`1 GetEnumerator()`

### `OneToManyLinkToComplexHierarchyManager` (Namespace: `TFlex.DOCs.Model.References.Links`)
**Свойства:** LinkGroups: ParameterGroupCollection
**Методы:**
- `Boolean IsRelationModified(Guid linkGroupGuid)`

### `OneToManyRelation` (Namespace: `TFlex.DOCs.Model.References.Links`)
**Свойства:** LinkReference: Reference, Objects: ReferenceObjectCollection, IsLinkedReferenceInitialized: Boolean, IsEmptyLinkedObjects: Boolean, IsLoaded: Boolean, State: LoadState, CountLoaded: Int32
**Методы:**
- `IEnumerable`1 GetLinkedObjects()` [has Async]
- `IEnumerator`1 GetEnumerator()`

### `OneToManyRelationManager` (Namespace: `TFlex.DOCs.Model.References.Links`)
**Свойства:** Swapped: Boolean, LinkGroups: ParameterGroupCollection
**Методы:**
- `Boolean IsRelationModified(Guid linkGroupGuid)`

### `OneToManyTable` (Namespace: `TFlex.DOCs.Model.References.Links`)
**Свойства:** IsModified: Boolean, IsTableOneToMany: Boolean, IsLinkedObjectIdsLoaded: Boolean, IsEmptyLinkedObjectsId: Boolean, CountLoaded: Int32
**Методы:**
- `IEnumerable`1 GetLinkedObjects()` [has Async]
- `Boolean IsObjectAdded(ReferenceObject object)`
- `Boolean IsObjectRemoved(ReferenceObject object)`
- `Void DeleteAll()` [has Async]

### `OneToOneLink` (Namespace: `TFlex.DOCs.Model.References.Links`)
**Свойства:** LinkReference: Reference, LinkedObject: ReferenceObject, IsAdded: Boolean, IsModified: Boolean, IsDeleted: Boolean, IsChanged: Boolean, IsLoaded: Boolean, IsOneToOne: Boolean, IsLinkedObjectIdsLoaded: Boolean, IsEmptyLinkedObjectsId: Boolean, IsEmptyLinkedObjects: Boolean, IsLinkedReferenceInitialized: Boolean, State: LoadState, CountLoaded: Int32
**Методы:**
- `Nullable`1 GetLinkedObjectId()` [has Async]
- `TReferenceObject GetLinkedObject()` [has Async]
- `Void Load()` [has Async]
- `ReferenceObject GetObjectFromReference(Reference reference, StaticReferenceLoadSettings loadSettings, Boolean onlyLoadedData)` [has Async]
- `Void Reload()` [has Async]
- `ReferenceObject SetLinkedObject(ReferenceObject linkedObject)` [has Async]
- `ReferenceObject SetLinkedObjectWithShallowCopy(ReferenceObject linkedObject)`
- `ReferenceObject SetLinkedObjectWithNoCopy(ReferenceObject linkedObject, Reference reference)` [has Async]
- `List`1 GetSwappedLinkedObjects()`
- `Boolean IsObjectAdded(ReferenceObject object)`
- `Boolean IsObjectRemoved(ReferenceObject object)`

### `OneToOneLinkManager` (Namespace: `TFlex.DOCs.Model.References.Links`)
**Свойства:** Swapped: Boolean, LinkGroups: ParameterGroupCollection
**Методы:**
- `List`1 ToReferenceObjectList()`

### `OneToOneLinkToComplexHierarchy` (Namespace: `TFlex.DOCs.Model.References.Links`)
**Свойства:** LinkReference: Reference, LinkedComplexLink: ComplexHierarchyLink, IsAdded: Boolean, IsDeleted: Boolean, IsChanged: Boolean, IsOneToOne: Boolean, IsTableOneToMany: Boolean, IsLinkOneToMany: Boolean, IsAnyReference: Boolean, IsLinkedReferenceInitialized: Boolean, IsLinkedObjectIdsLoaded: Boolean, IsEmptyLinkedObjectsId: Boolean, IsEmptyLinkedObjects: Boolean, IsModified: Boolean, IsLoaded: Boolean, State: LoadState, CountLoaded: Int32
**Методы:**
- `Boolean IsObjectAdded(ReferenceObject object)`
- `Boolean IsObjectRemoved(ReferenceObject object)`
- `ComplexHierarchyLink SetLinkedComplexLink(ComplexHierarchyLink linkedComplexLink)`
- `Void Reload()`

### `OneToOneLinkToComplexHierarchyManager` (Namespace: `TFlex.DOCs.Model.References.Links`)
**Свойства:** LinkGroups: ParameterGroupCollection
**Методы:**
- `List`1 ToComplexHierarchyLinkList()`

### `ReferenceObjectLinks` (Namespace: `TFlex.DOCs.Model.References.Links`)
**Свойства:** Owner: DesktopObject, IsOneToOneLoaded: Boolean, ToOne: OneToOneLinkManager, ToOneToComplexHierarchy: OneToOneLinkToComplexHierarchyManager, SwappedToOne: OneToOneLinkManager, IsOneToManyLoaded: Boolean, ToMany: OneToManyRelationManager, ToManyToComplexHierarchy: OneToManyLinkToComplexHierarchyManager, SwappedToMany: OneToManyRelationManager, AnyReference: AnyReferenceLinkManager, SearchQuery: SearchQueryLinkManager, OneToOne: OneToOneLinkManager, OneToMany: OneToManyRelationManager
**Методы:**
- `OneToManyRelation FindToManyRelation(ParameterGroup group) (+1)`
- `OneToOneLink FindToOneLink(ParameterGroup group) (+1)`
- `OneToManyLinkToComplexHierarchy FindToManyToComplexHierarchyLink(ParameterGroup group) (+1)`
- `OneToOneLinkToComplexHierarchy FindToOneToComplexHierarchyLink(ParameterGroup group) (+1)`
- `SearchQueryLink FindSearchQueryLink(ParameterGroup group) (+1)`
- `Void Clear(Boolean full) (+1)`
- `Void Fill(LoadSettings settings, Boolean loadOnlyUnloadedRelations)` [has Async]

### `RelationManager`1` (Namespace: `TFlex.DOCs.Model.References.Links`)
**Свойства:** Owner: ReferenceObject, OwnerHierarchyLink: ComplexHierarchyLink, LinkGroups: ParameterGroupCollection, IsModified: Boolean, Item: T, Item: T, Item: T
**Методы:**
- `T Find(ParameterGroup linkGroup) (+2)`
- `Void ReloadAll()`
- `IEnumerator`1 GetEnumerator()`

### `RelationTree` (Namespace: `TFlex.DOCs.Model.References.Links`)
**Свойства:** Mode: LoadingMode, RootObject: ReferenceObject, RootObjects: ReadOnlyCollection`1, RootHierarchyLink: ComplexHierarchyLink, RootHierarchyLinks: ReadOnlyCollection`1, Relations: ReadOnlyCollection`1, SetObjectsInLinks: Boolean, LoadOnlyUnloadedRelations: Boolean, FilterContext: MacroContext
**Методы:**
- `ReadOnlyCollection`1 Fill(IEnumerable`1 rootObjects, LoadSettings settings, Boolean setObjectsInLinks, Boolean loadOnlyUnloadedRelations, MacroContext filterContext) (+2)` [has Async]
- `ReadOnlyCollection`1 Load(DesktopObject rootObject, LoadSettings settings, Boolean setObjectsInLinks, Boolean loadOnlyUnloadedRelations, RecursiveLoadDirection loadDirection, MacroContext filterContext) (+2)` [has Async]
- `Void LoadParameters(IEnumerable`1 rootObjects, IReadOnlyCollection`1 parameterIds) (+2)` [has Async]

### `SearchQueryLink` (Namespace: `TFlex.DOCs.Model.References.Links`)
**Свойства:** Filter: Filter, PathToFilter: ReferencePath, Objects: ReferenceObjectCollection, IsSearchQueryLink: Boolean, LinkReference: Reference, IsLinkedReferenceInitialized: Boolean, IsEmptyLinkedObjects: Boolean, IsModified: Boolean, IsLoaded: Boolean, State: LoadState, CountLoaded: Int32, IsLinkedObjectIdsLoaded: Boolean, IsEmptyLinkedObjectsId: Boolean
**Методы:**
- `IEnumerable`1 GetLinkedObjects()` [has Async]
- `Boolean IsObjectAdded(ReferenceObject object)`
- `Boolean IsObjectRemoved(ReferenceObject object)`

### `SearchQueryLinkManager` (Namespace: `TFlex.DOCs.Model.References.Links`)
**Свойства:** LinkGroups: ParameterGroupCollection

### `LinkExtensions` (Namespace: `TFlex.DOCs.Model.References.Links.Extensions`)
**Методы:**
- `OneToManyRelation FindToManyRelation(DesktopObject desktopObject, Guid linkGuid) (+1)`
- `OneToOneLink FindToOneLink(DesktopObject desktopObject, Guid linkGuid) (+1)`
- `IconImage GetLinkTypeIcon(LinkVisibility linkVisibility, LinkType linkedType, Boolean throwOnError)`
- `IconImage GetSystemLinkTypeIcon(LinkType linkedType)`
- `IconImage GetCorruptedLinkTypeIcon(LinkType linkedType)`
- `String GetLinkTypeDescription(LinkType type)`

### `ReferenceObjectLinksExtensions` (Namespace: `TFlex.DOCs.Model.References.Links.Extensions`)
**Методы:**
- `ReferenceObject GetStorageLinkedObject(DesktopObject desktopObject, Guid linkGuid) (+5)` [has Async]
- `Boolean TryGetStorageLinkedObject(DesktopObject desktopObject, Guid linkGuid, ReferenceObject& linkedObject) (+5)`
- `ICollection`1 GetStorageLinkedObjects(DesktopObject desktopObject, Guid linkGuid) (+3)` [has Async]
- `Boolean TryGetStorageLinkedObjects(DesktopObject desktopObject, Guid linkGuid, ICollection`1& objects) (+3)`
- `ReferenceObject SetStorageLinkedObject(DesktopObject desktopObject, Guid linkGuid, ReferenceObject newLinkedObject, Boolean appendInStorage) (+2)`
- `ReferenceObject AddStorageLinkedObject(DesktopObject desktopObject, Guid linkGuid, ReferenceObject newLinkedObject, Boolean appendInStorage) (+3)`
- `ICollection`1 AddStorageLinkedObjects(DesktopObject desktopObject, Guid linkGuid, ICollection`1 newLinkedObjects, Boolean appendInStorage) (+3)`
- `Boolean RemoveStorageLinkedObject(DesktopObject desktopObject, Guid linkGuid, ReferenceObject linkedObject) (+1)`
- `Void RemoveAllStorageLinkedObjects(DesktopObject desktopObject, Guid linkGuid) (+3)`
- `Void SetStaticReferences(LoadSettings settings, ICollection`1 ignoreLinks, StaticReferenceLoadSettings loadSettings) (+1)`

### `ReferencesStorageExtensions` (Namespace: `TFlex.DOCs.Model.References.Links.Extensions`)
**Методы:**
- `ReferenceObject GetLinkedObject(DesktopObject desktopObject, Guid linkGuid, ReferencesStorage storage, StaticReferenceLoadSettings loadSettings, Boolean onlyLoadedData) (+2)`
- `Boolean TryGetLinkedObject(DesktopObject desktopObject, Guid linkGuid, ReferenceObject& linkedObject, ReferencesStorage storage, StaticReferenceLoadSettings loadSettings, Boolean onlyLoadedData) (+1)`
- `List`1 GetLinkedObjects(DesktopObject desktopObject, Guid linkGuid, ReferencesStorage storage, StaticReferenceLoadSettings loadSettings, Boolean onlyLoadedData) (+1)`
- `Boolean TryGetLinkedObjects(DesktopObject desktopObject, Guid linkGuid, List`1& objects, ReferencesStorage storage, StaticReferenceLoadSettings loadSettings, Boolean onlyLoadedData)`
- `ReferenceObject SetLinkedObject(DesktopObject desktopObject, Guid linkGuid, ReferenceObject newLinkedObject, ReferencesStorage storage, Boolean appendInStorage) (+2)`
- `ReferenceObject AddLinkedObject(DesktopObject desktopObject, Guid linkGuid, ReferenceObject newLinkedObject, ReferencesStorage storage, Boolean appendInStorage) (+5)`
- `List`1 AddLinkedObjectReadOnly(DesktopObject desktopObject, ParameterGroup linkGroup, IReadOnlyCollection`1 newLinkedObjects, ReferencesStorage storage, Boolean appendInStorage)`
- `List`1 AddLinkedObjects(OneToManyLink link, ICollection`1 newLinkedObjects, ReferencesStorage storage, Boolean appendInStorage) (+1)`
- `List`1 AddLinkedObjectsReadOnly(OneToManyLink link, IReadOnlyCollection`1 newLinkedObjects, ReferencesStorage storage, Boolean appendInStorage) (+1)`
- `Boolean RemoveLinkedObject(DesktopObject desktopObject, Guid linkGuid, ReferenceObject linkedObject, ReferencesStorage storage) (+1)`
- `Void RemoveAllLinkedObjects(DesktopObject desktopObject, Guid linkGuid, ReferencesStorage storage) (+3)`
- `Void SetStaticReferences(LoadSettings settings, ReferencesStorage storage, ICollection`1 ignoreLinks, Nullable`1 loadSettings, Boolean useRootConfigurationSettings) (+1)`
- `Boolean ContainsStaticReferenceInAllRelations(LoadSettings settings)`
- `ReferencesStorage FindStorageInReference(Reference reference)`
- `TReference GetIndependentReference(TReference reference)`

### `RelationExtensions` (Namespace: `TFlex.DOCs.Model.References.Links.Extensions`)
**Методы:**
- `String GetRelationObjectsVisualization(RelationTree relationTree, Guid[] objectParameters) (+1)`
- `Void AddParentsRecursiveLoad(RelationLoadSettings loadSettings)`
- `Void AddChildrenRecursiveLoad(RelationLoadSettings loadSettings)`
- `Void AddParentOneLevelLoad(RelationLoadSettings loadSettings)`
- `Void AddChildrenOneLevelLoad(RelationLoadSettings loadSettings)`
- `Void AddRecursiveLoad(RelationLoadSettings loadSettings, RecursiveLoadDirection loadDirection)`
- `Void AddNonRecursiveLoad(RelationLoadSettings loadSettings, RecursiveLoadDirection loadDirection)`
- `String GetVisualization(LoadSettings loadSettings)`
- `IEnumerable`1 GetAllObjects(ReferenceObject mainReferenceObject, ParameterGroup linkGroup)` [has Async]
- `String GetLoadedObjectLinksVisualization(ReferenceObject referenceObject, ReferencesStorage storage) (+1)`

### `CodeMacro` (Namespace: `TFlex.DOCs.Model.References.Macros`)
**Свойства:** IsMethod: Boolean, IsCompiled: Boolean, CompilationResult: CompilationResult
**Методы:**
- `MacroValidationResults Validate()`
- `IEnumerable`1 GetEntryPoints()`
- `Type GetMacroProviderType()`
- `IEnumerable`1 GetReferences()`
- `CompilationResult Compile()`
- `Void DeleteAssembliesFromMacroFolder(ServerConnection connection, Boolean throwOnError)`
- `Int32 GetUserCodeOffset()`

### `CodeManager` (Namespace: `TFlex.DOCs.Model.References.Macros`)
**Методы:**
- `Boolean IsFlowchartCode(String code)`
- `Boolean IsXml(String value)`

### `FlowchartMacro` (Namespace: `TFlex.DOCs.Model.References.Macros`)
**Свойства:** IsCompiled: Boolean, IsLimitedCountEntryPointParameters: Boolean, IsMethod: Boolean
**Методы:**
- `MacroValidationResults Validate()`
- `IEnumerable`1 GetEntryPoints()`

### `Macro` (Namespace: `TFlex.DOCs.Model.References.Macros`)
**Свойства:** Class: MacroType, Name: StringParameter, Comment: StringParameter, References: StringParameter, LogRunHistory: BooleanParameter, DebugMode: BooleanParameter, Code: StringParameter, IsCompiled: Boolean, IsLimitedCountEntryPointParameters: Boolean, IsMethod: Boolean
**Методы:**
- `List`1 GetAdditionalFiles()` [has Async]
- `MacroValidationResults Validate()`
- `Object Run(MacroContext context, String entryPoint, Object[] parameters) (+1)` [has Async]
- `Type GetMacroProviderType()`
- `IEnumerable`1 GetEntryPoints()`

### `MacroObjectSavedArgs` (Namespace: `TFlex.DOCs.Model.References.Macros`)
**Свойства:** Type: ObjectChangeType, Sender: Object

### `MacroReference` (Namespace: `TFlex.DOCs.Model.References.Macros`)
**Свойства:** Instance: MacroReference, Classes: MacroTypes
**Методы:**
- `Macro GetMacro(Guid guid)`
- `Macro Find(Guid guid) (+1)` [has Async]

### `MacroType` (Namespace: `TFlex.DOCs.Model.References.Macros`)
**Свойства:** IsMacro: Boolean, IsTechnologyMacro: Boolean, IsCSharpMacro: Boolean, IsFlowchartMacro: Boolean, IsTemplateFlowchartMacro: Boolean

### `MacroTypes` (Namespace: `TFlex.DOCs.Model.References.Macros`)
**Свойства:** Macro: MacroType, TechnologyMacro: MacroType, CSharpMacro: MacroType, FlowchartMacro: MacroType, TemplateFlowchartMacro: MacroType

### `TechnologyMacro` (Namespace: `TFlex.DOCs.Model.References.Macros`)
**Свойства:** IsMethod: Boolean
**Методы:**
- `IEnumerable`1 GetReferences()`
- `Int32 GetUserCodeOffset()`

### `FlowchartMacroContext` (Namespace: `TFlex.DOCs.Model.References.Macros.MacroFlowchart`)
**Свойства:** MacroProvider: MacroProvider, Parameters: List`1, Result: Object, Sender: Object, Args: Object, FormulaCreator: IFormulaMacroCreator, Mode: FlowchartMacroWorkflowExecuteMode

### `FlowchartMacroHandlerManager` (Namespace: `TFlex.DOCs.Model.References.Macros.MacroFlowchart`)
**Методы:**
- `Boolean CanInvoke(ActivityEventHandler handler)`
- `Object Invoke(ActivityEventHandler handler, Object args)`

### `FlowchartMacroManager` (Namespace: `TFlex.DOCs.Model.References.Macros.MacroFlowchart`)
**Свойства:** Name: String, Code: String, IsCompiled: Boolean, AllowThrowOnValidate: Boolean, FormulaCreator: IFormulaMacroCreator
**Методы:**
- `ValidationResults Validate(String code, Activity& activity) (+2)`
- `Void ValidateAndThrowOnError(String name, String code)`
- `Object Calculate(String code, MacroContext context, Object[] parameters) (+5)`
- `Void Clear()`

### `IPhysicalProperties` (Namespace: `TFlex.DOCs.Model.References.Materials`)
**Свойства:** Density: Double, Stress: Double, CompressionLimit: Double, YieldStrength: Double, SpecificHeat: Double, Elasticity: Double, Puasson: Double, Expansion: Double, ThermalConductivity: Double

### `IVisualProperties` (Namespace: `TFlex.DOCs.Model.References.Materials`)
**Свойства:** Name: String, AmbientColor: Int32, DiffuseColor: Int32, SpecularColor: Int32, EmissiveColor: Int32, Shininess: Double, Reflection: Double, Transparency: Double

### `MaterialPhysicalProperties` (Namespace: `TFlex.DOCs.Model.References.Materials`)
**Свойства:** Density: Double, Stress: Double, CompressionLimit: Double, YieldStrength: Double, SpecificHeat: Double, Elasticity: Double, Puasson: Double, Expansion: Double, ThermalConductivity: Double

### `MaterialReference` (Namespace: `TFlex.DOCs.Model.References.Materials`)
**Свойства:** Classes: MaterialTypes

### `MaterialReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Materials`)
**Свойства:** PhysicalProperties: IPhysicalProperties, VisualProperties: IVisualProperties, Name: StringParameter, Denotation1: StringParameter, Denotation2: StringParameter, Denotation3: StringParameter, Denotation4: StringParameter, MaterialMark: AbstractMarkReferenceObject

### `MaterialType` (Namespace: `TFlex.DOCs.Model.References.Materials`)
**Свойства:** Classes: MaterialTypes, IsMaterial: Boolean

### `MaterialTypes` (Namespace: `TFlex.DOCs.Model.References.Materials`)
**Свойства:** Material: MaterialType

### `ObjectVisualProperties` (Namespace: `TFlex.DOCs.Model.References.Materials`)
**Свойства:** AmbientColor: Int32, DiffuseColor: Int32, SpecularColor: Int32, EmissiveColor: Int32, Shininess: Double, Reflection: Double, Transparency: Double, Name: String

### `HierarchyLinkMatchesReference` (Namespace: `TFlex.DOCs.Model.References.Modifications`)
**Свойства:** Classes: HierarchyLinkMatchesTypes

### `HierarchyLinkMatchesReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Modifications`)
**Свойства:** Class: HierarchyLinkMatchesType, Name: StringParameter, Action: Int32Parameter
**Методы:**
- `NomenclatureHierarchyLink GetSourceHierarchyLink(DesignContextObject designContext, Boolean applyDesignContext, Boolean applyDate)`
- `NomenclatureHierarchyLink GetAddedHierarchyLink(DesignContextObject designContext, Boolean applyDesignContext, Boolean applyDate)`
- `NomenclatureHierarchyLink GetDeletedHierarchyLink(DesignContextObject designContext, Boolean applyDesignContext, Boolean applyDate)`

### `HierarchyLinkMatchesType` (Namespace: `TFlex.DOCs.Model.References.Modifications`)
**Свойства:** Classes: HierarchyLinkMatchesTypes, IsHierarchyLinkMatches: Boolean

### `HierarchyLinkMatchesTypes` (Namespace: `TFlex.DOCs.Model.References.Modifications`)
**Свойства:** MatchesType: HierarchyLinkMatchesType

### `ModificationActionReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Modifications`)
**Свойства:** Class: ModificationActionType, Content: StringParameter, IsAutoText: BooleanParameter

### `ModificationActionsReference` (Namespace: `TFlex.DOCs.Model.References.Modifications`)
**Свойства:** Classes: ModificationActionTypes

### `ModificationActionType` (Namespace: `TFlex.DOCs.Model.References.Modifications`)
**Свойства:** Classes: ModificationActionTypes, IsDeleteAction: Boolean, IsAddAction: Boolean, IsEditAction: Boolean, IsReplaceAction: Boolean, IsEditObjectAction: Boolean

### `ModificationReference` (Namespace: `TFlex.DOCs.Model.References.Modifications`)
**Свойства:** Classes: ModificationTypes

### `ModificationReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Modifications`)
**Свойства:** Class: ModificationType, Name: StringParameter, ModificationContent: StringParameter, UsingAreaContent: StringParameter, ModificationNotice: ReferenceObject, DesignContext: DesignContextObject

### `ModificationType` (Namespace: `TFlex.DOCs.Model.References.Modifications`)
**Свойства:** Classes: ModificationTypes, IsModification: Boolean

### `ModificationTypes` (Namespace: `TFlex.DOCs.Model.References.Modifications`)
**Свойства:** Modification: ModificationType

### `ModificationUsingAreaReference` (Namespace: `TFlex.DOCs.Model.References.Modifications`)
**Свойства:** Classes: ModificationUsingAreaTypes

### `ModificationUsingAreaReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Modifications`)
**Свойства:** Class: ModificationUsingAreaType, Content: StringParameter, IsAutoText: BooleanParameter
**Методы:**
- `List`1 GetHierarchyLinkMatches()`
- `Void CreateHierarchyLinkMatching(ComplexHierarchyLink sourceLink, ComplexHierarchyLink addedLink, ComplexHierarchyLink deletedLink, Int32 action)`

### `ModificationUsingAreaType` (Namespace: `TFlex.DOCs.Model.References.Modifications`)
**Свойства:** Classes: ModificationUsingAreaTypes, IsUsingArea: Boolean

### `ModificationUsingAreaTypes` (Namespace: `TFlex.DOCs.Model.References.Modifications`)
**Свойства:** UsingAreaType: ModificationUsingAreaType

### `SourceRevisionReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Modifications`)
**Свойства:** Class: SourceRevisionsType, Name: StringParameter, ApplyingStage: GuidParameter, SourceRevision: ReferenceObject

### `SourceRevisionsReference` (Namespace: `TFlex.DOCs.Model.References.Modifications`)
**Свойства:** Classes: SourceRevisionsTypes

### `SourceRevisionsType` (Namespace: `TFlex.DOCs.Model.References.Modifications`)
**Свойства:** Classes: SourceRevisionsTypes, IsSourceRevisionType: Boolean

### `SourceRevisionsTypes` (Namespace: `TFlex.DOCs.Model.References.Modifications`)
**Свойства:** SourceRevisionType: SourceRevisionsType

### `GroupObject` (Namespace: `TFlex.DOCs.Model.References.NavigationPanel`)
**Свойства:** Description: GroupDescription, AccessibleInCurrentConfiguration: Boolean

### `NavigationPanelReference` (Namespace: `TFlex.DOCs.Model.References.NavigationPanel`)
**Свойства:** CommonFolder: ReferenceObject, PrivateFolder: ReferenceObject, Classes: NavigationPanelTypes
**Методы:**
- `List`1 GetRootObjects()`
- `Void RefreshDesktop()`
- `Boolean IsDesktop(GroupObject group)`

### `NavigationPanelReferenceObject` (Namespace: `TFlex.DOCs.Model.References.NavigationPanel`)
**Свойства:** NameParameter: StringParameter, IconParameter: IconParameter, DataParameter: StringParameter, Data: String, Icon: IconImage, IconGuid: Guid, Class: NavigationPanelType
**Методы:**
- `Void SetIcon(IconImage icon, Guid iconGuid)`

### `NavigationPanelType` (Namespace: `TFlex.DOCs.Model.References.NavigationPanel`)
**Свойства:** Classes: NavigationPanelTypes, IsFolder: Boolean, IsGroup: Boolean, IsShortcut: Boolean

### `NavigationPanelTypes` (Namespace: `TFlex.DOCs.Model.References.NavigationPanel`)
**Свойства:** Group: NavigationPanelType, Folder: NavigationPanelType, Shortcut: NavigationPanelType

### `ShortcutObject` (Namespace: `TFlex.DOCs.Model.References.NavigationPanel`)
**Свойства:** Description: ShortcutDescription

### `ShortcutTypeExtension` (Namespace: `TFlex.DOCs.Model.References.NavigationPanel`)
**Методы:**
- `String GetText(ShortcutType type)`

### `ApplicabilityInterval` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** StartProductGuid: GuidParameter, EndProductGuid: GuidParameter

### `BillOfMaterials` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Методы:**
- `Void UpdateLinkedBOMFile()`
- `Void GenerateNewReport(FileObject fileObject)`
- `IReadOnlyCollection`1 GetFileObjects()`

### `CategoriesFilterData` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** ShowAllCategories: Boolean, ShowEmptyCategories: Boolean, ProductCategories: List`1

### `DefaultNewObjectFolderAttribute` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** IsInherit: Boolean, FolderIsMacro: Boolean, Macro: String, ShowFolderDialog: Boolean, DefaultFolderKey: Guid, CreateUserSubfolder: Boolean, ForbidSelectionFromOtherFolders: Boolean, IsSystem: Boolean, CanSerialize: Boolean, CanChangeCaption: Boolean, Caption: String, Value: Object, CanRemove: Boolean
**Методы:**
- `ReferenceObject FindDefaultParent(Reference reference)` [has Async]

### `DesignContextObject` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** IsMain: Boolean, Class: DesignContextsType, Name: StringParameter, IsDefaultForUser: GuidParameter
**Методы:**
- `Boolean ValidateEditByCurrentUser(Boolean throwOnError)`
- `IReadOnlyCollection`1 Substitute(IEnumerable`1 referenceObjects, Boolean delete, ReferenceObject parent) (+3)` [has Async]
- `Void MoveChanges(ComplexHierarchyLink hierarchyLink) (+3)` [has Async]
- `Void CopyChanges(ComplexHierarchyLink hierarchyLink) (+3)` [has Async]
- `Void CopyMoveChanges(Dictionary`2 links) (+1)` [has Async]
- `Void DeleteChanges(ComplexHierarchyLink hierarchyLink) (+3)` [has Async]
- `Void ConfirmConflictChanges(IReadOnlyCollection`1 hierarchyLinks) (+1)` [has Async]
- `List`1 GetLinksFromMainContext(IReadOnlyCollection`1 links, Boolean copyToOriginalReference)` [has Async]
- `List`1 GetOriginals(IReadOnlyCollection`1 links, Boolean copyToOriginalReference) (+1)` [has Async]
- `List`1 GetObjectsFromMainContext(IEnumerable`1 referenceObjects, Boolean loadChildren)` [has Async]
- `Dictionary`2 GetObjectsWithChangedApplicability(IEnumerable`1 referenceObjects)` [has Async]
- `Dictionary`2 GetLinksWithChangedApplicability(IEnumerable`1 hierarchyLinks)` [has Async]
- `Void ApplyDesignContextChanges(IReadOnlyCollection`1 changes) (+1)` [has Async]
- `Boolean ValidateParentObject(ReferenceObject parent, ClassObject classObject, Boolean throwOnError)`

### `DesignContextsReference` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** MainContext: DesignContextObject, Classes: DesignContextsTypes, IsDefaultForUserParameterInfo: ParameterInfo
**Методы:**
- `DesignContextObject Find(String name)` [has Async]
- `DesignContextObject FindDefaultDesignContext(User user) (+1)` [has Async]
- `List`1 FindDefaultDesignContexts(User user) (+1)` [has Async]

### `DesignContextsType` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** Classes: DesignContextsTypes, IsDesignContext: Boolean

### `DesignContextsTypes` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** DesignContext: DesignContextsType

### `EntrancesTree` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** Objects: IEnumerable`1, RootObjects: IEnumerable`1, StructuresEntrances: IEnumerable`1

### `IntervalReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** ProductsClassifierObject: ProductsClassifierReferenceObject, ApplicabilitySetMethod: ApplicabilitySetMethod, MilestoneRanges: List`1, DesignNumberRanges: List`1, StructureVariantObject: StructureVariantsReferenceObject, ApplicabilityAction: ApplicabilityActionType, OptionsDescription: String, UseNumberRanges: Boolean, Class: ProductsApplicabilityType, Name: StringParameter, ObjectGuid: GuidParameter, ReferenceGuid: GuidParameter, LinkedObjectID: Int32Parameter, ReferenceID: Int32Parameter, Description: StringParameter, ApplicabilityGroup: ApplicabilityGroupType, Conditions: StringParameter, OptionRecords: List`1, ApplicabilityRecords: List`1
**Методы:**
- `Void SetApplicabilityRecords(List`1 records)`
- `Void SetOptionRecords(List`1 records)`
- `Void CopyRecordsFrom(IntervalReferenceObject other)`

### `NomenclatureHierarchyLink` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** Amount: DoubleParameter, Unit: StringParameter, Remarks: StringParameter, UseInSpecification: BooleanParameter, UseInStructure: BooleanParameter, BomSection: StringParameter, Position: Int32Parameter, AmountForAssembly: DoubleParameter, AmountForComplect: DoubleParameter, XMin: DoubleParameter, XMax: DoubleParameter, YMin: DoubleParameter, YMax: DoubleParameter, ZMin: DoubleParameter, ZMax: DoubleParameter, Placement: StringParameter, StructureTypes: StructureTypesCollection, CadDocumentContext: ByteArrayParameter, CadObjectIdentifier: GuidParameter, PassToCad: BooleanParameter, CategoriesLink: OneToManyLink, ProductStructure: Int32, InsertedProductStructureId: Nullable`1
**Методы:**
- `Void BeginChanges(Boolean forceEndChanges)`
- `Boolean RestoreLink()`
- `Void UpdateFromLink(ComplexHierarchyLink sourceHierarchyLink, Boolean copyParameters, CopyReferenceObjectsContext copyContext, Boolean copyApplicability) (+1)`
- `Boolean SubstituteForCurrentDesignContext()`
- `Void ApplyDesignContextChangesToMainContext()`
- `Void MoveDesignContextChangesToOtherContext(DesignContextObject designContext)`
- `Void CancelDesignContextChanges()`

### `NomenclatureObject` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** Denotation: StringParameter, VariantName: StringParameter, Code: StringParameter, Version: StringParameter, Format: StringParameter, Mass: DoubleParameter, IsEndOfProduct: BooleanParameter, Letter: StringParameter, AltRepName: StringParameter, AltRepCode: StringParameter, AltRepGroupGuid: GuidParameter, IsAltRep: Boolean, BaseVersion: NomenclatureObject, IsVersion: Boolean, IsBaseVersion: Boolean, IsVariant: Boolean, ModificationNotices: ModificationNoticeWithActionsReferenceObject[], HasLinkedObject: Boolean, LinkedObject: ReferenceObject, IsLinkedObjectLoaded: Boolean, LinkedObjectId: Int32, LinkedObjectReferenceId: Int32, IsMaterialObject: Boolean
**Методы:**
- `Void SetBaseVersion(NomenclatureObject newBaseVersion, String versionName, String denotation)`
- `String GetBaseDenotation()`
- `Boolean CanChangeLink(LinkInfo link, ReferenceObject addObject, ReferenceObject removeObject)`
- `List`1 GetVersions()` [has Async]
- `List`1 GetVariants()` [has Async]
- `NomenclatureObject GetMainObject()` [has Async]
- `Boolean IsVariantOf(NomenclatureObject object)`
- `NomenclatureObject CreateVersion(String versionName, Boolean copyChildren, Boolean copyFiles, IEnumerable`1 skip, Boolean changeCopyFilesFolder, String copyFilesFolderPath, String denotation) (+3)`
- `NomenclatureObject CreateAltRep(String altRepName, String altRepCode, Boolean copyChildren, IEnumerable`1 skip, Boolean copyFiles, String copyFilesNewFolderPath) (+1)`
- `NomenclatureObject CreateVariant(String variantName, Boolean copyChildren, Boolean copyFiles, IEnumerable`1 skip, Boolean changeCopyFilesFolder, String copyFilesFolderPath) (+3)`
- `List`1 UpdateByVariant(NomenclatureObject variantObject)`
- `List`1 GetSimilarObjects()`
- `ComplexHierarchyLink AddToParent(NomenclatureReferenceObject parentObject, IDictionary`2 linkParameters)`
- `List`1 GetAltReps()` [has Async]
- `Boolean CanCopy(ParameterInfo parameter) (+1)`
- `NomenclatureObject GetBaseRepresentationObject()` [has Async]

### `NomenclatureObjectEntrancesTree` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** MainObject: NomenclatureObject, ProductStructure: ProductStructureReferenceObject, StructuresEntrances: IEnumerable`1, IncludeStructures: Boolean, ExpandEndProducts: Boolean
**Методы:**
- `Void Reload(Boolean includeStructures, Boolean expandEndProducts)`

### `NomenclatureReference` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** DigitalStructureContext: DigitalStructureContext, Classes: NomenclatureTypes, LinkedToProductStructure: Boolean
**Методы:**
- `NomenclatureObject FindByLinkedObject(ReferenceObject linkedObject)` [has Async]
- `Dictionary`2 FindByLinkedObjects(IEnumerable`1 linkedObjects)` [has Async]
- `NomenclatureObject CreateNomenclatureObject(ReferenceObject linkedObject, NomenclatureReferenceObject parentObject, IDictionary`2 linkParameters, NomenclatureObject source, Boolean checkRevisions, Boolean isRevisionsContainer, Guid revisionsContainerGuid) (+1)`
- `List`1 CreateNomenclatureObjects(CreateObjectsData objectsData)` [has Async]
- `EntrancesTree GetEntrances(NomenclatureObject nomenclatureObject, Boolean includeStructures, Boolean expandEndProducts)`
- `ComplexHierarchyLink CreateEmptyHierarchLink(ReferenceObject childObject)`

### `NomenclatureReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** ProductStructureId: Int32, InsertedProductStructureId: Int32, Parent: NomenclatureReferenceObject, Class: NomenclatureType, Name: StringParameter, MainMaterialLink: OneToOneLink, MainMaterial: MaterialReferenceObject, LinkedTPReference: Reference, CanContainChildren: Boolean
**Методы:**
- `List`1 GetFiles()`
- `ComplexHierarchyLink CreateParentLink(ReferenceObject parentObject)`
- `ComplexHierarchyLink CreateChildLink(ReferenceObject childObject)`
- `ComplexHierarchyLinkInstanceData CreateChildLinkWithInstancesData(ReferenceObject childObject, ReferenceObjectInstance sourceStructureObjectInstance, ReferenceObjectInstance parentObjectInstance)`
- `ComplexHierarchyLinkInstanceData CreateChildLinkWithBaseInstancesData(ReferenceObject childObject, ReferenceObjectInstance baseInstance, ReferenceObjectInstance currentObjectInstance)`
- `Boolean CanCreateChildObject(ClassObject childClass)`

### `NomenclatureRule` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** NomenclatureParameter: ParameterInfo, ReferenceParameter: ParameterInfo
**Методы:**
- `Boolean CanCreateFor(ParameterInfo nomenclatureParameter, ParameterInfo referenceParameter)`

### `NomenclatureType` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** Classes: NomenclatureTypes, IsFolder: Boolean, IsObject: Boolean, IsMaterialObject: Boolean, IsDocument: Boolean, IsBillOfMaterials: Boolean, IsMaterial: Boolean, IsAssembly: Boolean, IsDetail: Boolean, IsStandardItem: Boolean, IsDrawing: Boolean, IsTechnologicalProcess: Boolean, IsEquipment: Boolean, IsTechnologicalNode: Boolean, IsProduct: Boolean, IsPiece: Boolean, IsScheme: Boolean, IsOtherProducts: Boolean, SupportsVersions: Boolean, SupportsAltReps: Boolean, HasLinkedClass: Boolean, LinkedClassId: Int32, LinkedClass: ClassObject, LinkedReferenceId: Int32, LinkedReferenceInfo: ReferenceInfo, LinkedReference: Reference, LinkedInheritClasses: Boolean, BaseNomenclatureType: NomenclatureType, Rules: ReadOnlyCollection`1, Attributes: NomenclatureTypeAttributes
**Методы:**
- `Boolean CreateInheritClasses()`
- `NomenclatureRule FindRule(ParameterInfo parameter)`

### `NomenclatureTypeAttribute` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** HierarchyParameter: ParameterInfo, IsSystem: Boolean, CanChangeCaption: Boolean, Caption: String, Value: Object, CanRemove: Boolean

### `NomenclatureTypeAttributes` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** DefaultNewObjectFolder: DefaultNewObjectFolderAttribute

### `NomenclatureTypeBuilder` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** Classes: NomenclatureTypes, Class: NomenclatureType, Base: NomenclatureType, LinkedClass: ClassObject, LinkedInheritClasses: Boolean

### `NomenclatureTypes` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** Folder: NomenclatureType, Object: NomenclatureType, MaterialObject: NomenclatureType, Document: NomenclatureType, Detail: NomenclatureType, Assembly: NomenclatureType, Drawing: NomenclatureType, StandardItem: NomenclatureType, Material: NomenclatureType, Product: NomenclatureType, TechnologicalProcess: NomenclatureType, TechnologicalNode: NomenclatureType, Complete: NomenclatureType, Complex: NomenclatureType, ElectronicComponent: NomenclatureType, Equipment: NomenclatureType, Piece: NomenclatureType, BillOfMaterials: NomenclatureType, Scheme: NomenclatureType, OtherProduct: NomenclatureType
**Методы:**
- `IReadOnlyCollection`1 GetLinkedGroups()`
- `ParameterGroup FindLinkedGroup(Int32 id) (+1)`

### `OptionsFilter` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Методы:**
- `String Serialize()`
- `OptionsFilter Deserialize(String xml, ServerConnection connection)`
- `OptionsFilter GetOptionFilter(String filterString, ServerConnection connection)`

### `OptionTerm` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** Connection: ServerConnection, Filter: OptionsFilter, Option: ProductOptionsReferenceObject, ParameterName: String

### `ProductOptionsReference` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** AllOptions: IList`1, Classes: ProdutOptionsTypes

### `ProductOptionsReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** Class: ProductOptionsType, Name: StringParameter, Code: String, Comment: String, Status: OptionStatus, DefaultMajorMinor: DefaultProductOptionKind, PossibleOptionValues: ReferenceObjectCollection, ProjectOptions: IEnumerable`1
**Методы:**
- `ReferenceObject AddPossibleOptionValues(ReferenceObject newLinkedObject)`
- `Boolean RemovePossibleOptionValues(ReferenceObject linkedObject)`

### `ProductOptionsType` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** Classes: ProdutOptionsTypes, IsOption: Boolean

### `ProductOptionValuesReference` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** Classes: ProductOptionValuesTypes

### `ProductOptionValuesReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** Class: ProductOptionValuesType, Value: StringParameter, Image: ImageParameter, Comment: StringParameter, Code: String, Status: OptionStatus, Option: ProductOptionsReferenceObject, ProjectOptionValues: IEnumerable`1

### `ProductOptionValuesType` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** Classes: ProductOptionValuesTypes, IsValue: Boolean

### `ProductOptionValuesTypes` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** Value: ProductOptionValuesType

### `ProductsApplicabilityReference` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** Classes: ProductsApplicabilityTypes
**Методы:**
- `ApplicabilityInterval CreateInterval()`
- `ApplicabilityConditions CreateConditions()`
- `IEnumerable`1 GetIntervals(Guid objectGuid) (+1)`
- `List`1 GetApplicabilityObjects(List`1 objectGuids)` [has Async]
- `Void CopyApplicability(DesktopObject fromObject, DesktopObject toObject)`
- `IEnumerable`1 CopyIntervals(Guid fromObject, Guid toObject)`
- `ApplicabilityConditions CopyConditions(Guid fromObject, Guid toObject)`
- `Void DeleteIntervals(Guid objectGuid)`
- `ApplicabilityConditions GetApplicabilityConditions(Guid objectGuid) (+1)`
- `List`1 GetApplicability(IEnumerable`1 objectGuids)` [has Async]
- `Void DeleteApplicabilityConditions(Guid objectGuid)`
- `Void DeleteApplicability(IEnumerable`1 objectGuids)` [has Async]
- `Void AddUnsavedApplicability(IntervalReferenceObject obj)`
- `Void RemoveUnsavedApplicability(IntervalReferenceObject obj)`

### `ProductsApplicabilityType` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** Classes: ProductsApplicabilityTypes, IsInterval: Boolean, IsConditions: Boolean, IsBaseApplicability: Boolean, IsModifiedApplicability: Boolean

### `ProductsApplicabilityTypes` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** Interval: ProductsApplicabilityType, Conditions: ProductsApplicabilityType, BaseApplicability: ProductsApplicabilityType, ModifiedApplicability: ProductsApplicabilityType

### `ProductsClassifierActionReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** Class: ProductsClassifierActionsType, Action: Int32Parameter, AllValues: BooleanParameter, OptionActionType: OptionActionType, SelectedOption: ProductOptionsReferenceObject, SelectedOptionValue: ProductOptionValuesReferenceObject, ChangingOption: ProductOptionsReferenceObject, ChangingOptionValues: ReferenceObjectCollection`1
**Методы:**
- `ReferenceObject AddChangingOptionValue(ProductOptionValuesReferenceObject newLinkedObject)`
- `Boolean RemoveChangingOptionValue(ProductOptionValuesReferenceObject linkedObject)`

### `ProductsClassifierActionsReference` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** Classes: ProductsClassifierActionsTypes

### `ProductsClassifierActionsType` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** Classes: ProductsClassifierActionsTypes

### `ProductsClassifierActionsTypes` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** ActionType: ProductsClassifierActionsType

### `ProductsClassifierOptionsReference` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** Classes: ProductsClassifierOptionsTypes

### `ProductsClassifierOptionsReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** Class: ProductsClassifierOptionsType, IsHidden: BooleanParameter, IsNotEditable: BooleanParameter, OptionGroup: StringParameter, Option: ProductOptionsReferenceObject, DefaultOptionValue: ProductOptionValuesReferenceObject, OptionValues: ReferenceObjectCollection`1
**Методы:**
- `ReferenceObject AddOptionValue(ProductOptionValuesReferenceObject newLinkedObject)`
- `Boolean RemoveOptionValue(ProductOptionValuesReferenceObject linkedObject)`

### `ProductsClassifierOptionsType` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** Classes: ProductsClassifierOptionsTypes, IsOption: Boolean

### `ProductsClassifierOptionsTypes` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** Option: ProductsClassifierOptionsType

### `ProductsClassifierReference` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** Classes: ProductsClassifierTypes
**Методы:**
- `ProductsClassifierReferenceObject Find(String denotation)` [has Async]

### `ProductsClassifierReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** Parent: ProductsClassifierReferenceObject, Class: ProductsClassifierType, Name: StringParameter, Denotation: StringParameter, Image: ImageParameter, SpecificStructure: BooleanParameter, OptionsManagement: Boolean, DesignStructure: NomenclatureReferenceObject, ProjectOptionValues: IEnumerable`1, OptionsObjectList: OneToManyTable, Options: IEnumerable`1, OptionActionsObjectList: OneToManyTable, OptionActions: IEnumerable`1, InstancesLink: OneToManyLink, Instances: IEnumerable`1, Specifications: IEnumerable`1, SpecificationRequirements: ReferenceObject, ProductsSpecificationsLink: OneToManyLink, SerialProductNumbersLink: OneToManyLink, LinkedOptionTableSet: OptionsTableSetReferenceObject, ProjectOptionValueLink: OneToManyLink, OptionSetLink: OneToManyLink, OptionSets: IEnumerable`1, DesignNumbers: IEnumerable`1, SerialProductNumbers: IEnumerable`1, Milestones: IEnumerable`1
**Методы:**
- `Boolean IncludedInApplicabilityInterval(ReferenceObject productsInterval)`
- `Boolean CanChangeLink(LinkInfo link, ReferenceObject addObject, ReferenceObject removeObject)`

### `ProductsClassifierType` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** Classes: ProductsClassifierTypes, IsProject: Boolean, IsProduct: Boolean, IsProductModification: Boolean, IsProductConfiguration: Boolean

### `ProductsClassifierTypes` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** Project: ProductsClassifierType, Product: ProductsClassifierType, ProductModification: ProductsClassifierType, ProductConfiguration: ProductsClassifierType

### `ProductsInstancesReference` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** Classes: ProductsInstancesTypes

### `ProductsInstancesReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** Class: ProductsInstancesType, SerialNumber: StringParameter, Number: Int32Parameter, Product: ProductsClassifierReferenceObject

### `ProductsInstancesType` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** Classes: ProductsInstancesTypes, IsProductInstance: Boolean

### `ProductsInstancesTypes` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** ProductInstance: ProductsInstancesType

### `ProductsTermGroup` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** Connection: ServerConnection, Products: List`1

### `ProdutOptionsTypes` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** Option: ProductOptionsType

### `TypicalConfigurationSettingsObject` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** Configurator: Configurator, Class: TypicalConfigurationSettingsType, Name: StringParameter, ConfiguratorGuid: GuidParameter, Data: StringParameter

### `TypicalConfigurationSettingsReference` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** Classes: TypicalConfigurationSettingsTypes
**Методы:**
- `TypicalConfigurationSettingsObject Find(String name)` [has Async]

### `TypicalConfigurationSettingsType` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** Classes: TypicalConfigurationSettingsTypes, IsTypicalConfiguration: Boolean

### `TypicalConfigurationSettingsTypes` (Namespace: `TFlex.DOCs.Model.References.Nomenclature`)
**Свойства:** TypicalConfiguration: TypicalConfigurationSettingsType

### `ProductsDesignNumbersReference` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.ConfigurationsManager`)
**Свойства:** Classes: ProductsDesignNumbersTypes

### `ProductsDesignNumbersReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.ConfigurationsManager`)
**Свойства:** Class: ProductsDesignNumbersType, Number: Int32Parameter, Description: StringParameter, Product: ProductsClassifierReferenceObject, Configuration: ProductsClassifierReferenceObject

### `ProductsDesignNumbersType` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.ConfigurationsManager`)
**Свойства:** Classes: ProductsDesignNumbersTypes, IsProductDesignNumber: Boolean

### `ProductsDesignNumbersTypes` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.ConfigurationsManager`)
**Свойства:** ProductDesignNumber: ProductsDesignNumbersType

### `ProductsMilestonesReference` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.ConfigurationsManager`)
**Свойства:** Classes: ProductsMilestonesTypes

### `ProductsMilestonesReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.ConfigurationsManager`)
**Свойства:** Class: ProductsMilestonesType, MilestoneNumber: Int32Parameter, Description: StringParameter, Product: ProductsClassifierReferenceObject, Configuration: ReferenceObject

### `ProductsMilestonesType` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.ConfigurationsManager`)
**Свойства:** Classes: ProductsMilestonesTypes, IsProductMilestone: Boolean

### `ProductsMilestonesTypes` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.ConfigurationsManager`)
**Свойства:** ProductMilestone: ProductsMilestonesType

### `ComplexHierarchyLinkExtensions` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.DesignContexts`)
**Методы:**
- `Boolean IsSubstituteInActiveDesignContext(ComplexHierarchyLink hierarchyLink)`
- `Boolean IsSubstituteInDesignContext(ComplexHierarchyLink hierarchyLink, DesignContextObject designContext)`
- `DesignContextChangeStatus GetDesignContextStatus(ComplexHierarchyLink hierarchyLink)`

### `ReferenceObjectExtensions` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.DesignContexts`)
**Методы:**
- `Boolean IsSubstituteInActiveDesignContext(ReferenceObject referenceObject)`
- `Boolean IsSubstituteInDesignContext(ReferenceObject referenceObject, DesignContextObject designContext)`
- `DesignContextChangeStatus GetDesignContextStatus(ReferenceObject referenceObject)`

### `ActionReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.ModificationNotices`)
**Свойства:** Class: ActionType, Name: StringParameter, IsAutomatic: Boolean, ChangingObj: NomenclatureReferenceObject, ActionDescription: StringParameter, ActionState: Int32Parameter
**Методы:**
- `DesktopOperationInfo GenerateDesktopOperationInfo(IEnumerable`1 items)`
- `NomenclatureObject GetChangingObject()`
- `Guid GetChangingObjectGuid()`
- `Guid GetChangingNodeGuid()`
- `Void SetApplyingState()`
- `Void SetAppliedState()`
- `Boolean Apply(Boolean& needCheckInNomenclatureObject)`

### `ActionsReference` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.ModificationNotices`)
**Свойства:** Classes: ActionsTypes

### `ActionType` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.ModificationNotices`)
**Свойства:** IsAssemblyAction: Boolean, IsDeleteAction: Boolean, IsAddAction: Boolean, IsEditAction: Boolean, IsEntrancesChangingAction: Boolean, IsReplaceAction: Boolean, IsVariantReplaceAction: Boolean, IsReplaceFileEditAction: Boolean, IsCancelEditAction: Boolean, Classes: ActionsTypes

### `AddActionReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.ModificationNotices`)
**Свойства:** Count: Int32Parameter, Comment: StringParameter

### `AssemblyActionReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.ModificationNotices`)
**Свойства:** IsAutomatic: Boolean, ChangingNode: NomenclatureReferenceObject
**Методы:**
- `Guid GetChangingNodeGuid()`
- `Void SetChangingNode(NomenclatureReferenceObject newObject)`
- `NomenclatureObject GetChangingObject()`

### `EntrancesChangingActionReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.ModificationNotices`)
**Свойства:** Count: Int32Parameter, Remarks: StringParameter

### `ModificationNoticeBaseReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.ModificationNotices`)
**Свойства:** Name: StringParameter, AssignedTo: StringParameter, ReleaseDate: DateTimeParameter, ChangingDate: DateTimeParameter, ReserveComment: StringParameter, IntroductionComment: StringParameter, Text: StringParameter, ChangingReason: StringParameter, ChangingState: Int32Parameter, EditStageParameter: GuidParameter, ReadonlyStageParameter: GuidParameter, EditStage: Stage, ReadonlyStage: Stage, LinkedFiles: ReferenceObject[], LinkedObjects: ReferenceObject[], LinkedObjectsGuid: Guid, LinkedFilesGuid: Guid
**Методы:**
- `Void SetReadyToApplyState()`

### `ModificationNoticeType` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.ModificationNotices`)
**Свойства:** Classes: ModificationNoticeTypes, IsNFC: Boolean, IsAdditionalNFC: Boolean, IsPreliminaryNFC: Boolean, IsAdditionalPreliminaryNFC: Boolean, IsProposalToChange: Boolean

### `ModificationNoticeTypes` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.ModificationNotices`)
**Методы:**
- `ClassObject GetModificationNoticeBaseClass()`

### `ModificationNoticeWithActionsReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.ModificationNotices`)
**Свойства:** ActionObjects: ActionReferenceObject[]
**Методы:**
- `Void RaiseActionChangingObjectChange(ActionReferenceObject action, Boolean isAdded, ReferenceObject oldLinkedObject)`
- `Void RaiseActionDeleted(ActionReferenceObject action)`
- `Void SetReadyToApplyState()`
- `Void SetApplyingState()`
- `Boolean SetAppliedState(Boolean useStatusForActions) (+1)`

### `ReadOnlyGuidParameter` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.ModificationNotices`)
**Свойства:** IsReadOnly: Boolean

### `ReplaceActionReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.ModificationNotices`)
**Свойства:** NewObject: NomenclatureReferenceObject

### `ReplaceFileEditActionReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.ModificationNotices`)
**Свойства:** IsAutomatic: Boolean, DestinationFile: FileObject, SourceFile: FileObject

### `VariantReplaceActionReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.ModificationNotices`)
**Свойства:** IsAutomatic: Boolean, VariantObj: NomenclatureObject

### `ConstraintReference` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.OptionsTables`)
**Свойства:** Classes: ConstraintTypes

### `ConstraintReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.OptionsTables`)
**Свойства:** Class: ConstraintType, Name: String, ConstraintFormula: String, LogicalId: Guid, ConditionFormula: String, Comment: String, TableSet: OptionsTableSetReferenceObject

### `ConstraintType` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.OptionsTables`)
**Свойства:** Classes: ConstraintTypes, IsConstraint: Boolean

### `ConstraintTypes` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.OptionsTables`)
**Свойства:** Constraint: ConstraintType

### `IncompleteConfigurationReference` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.OptionsTables`)
**Свойства:** Classes: IncompleteConfigurationTypes

### `IncompleteConfigurationReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.OptionsTables`)
**Свойства:** Class: IncompleteConfigurationType, Type: IncompleteConfigurationKind, OptionValues: IList`1, Relations: IList`1, Table: OptionsTableReferenceObject
**Методы:**
- `ReferenceObject AddOptionValue(ReferenceObject optionValue)`
- `ReferenceObject AddRelation(IncompleteConfigurationsRelationReferenceObject relation)`
- `Boolean RemoveOptionValue(ReferenceObject optionValue)`
- `Boolean RemoveRelation(IncompleteConfigurationsRelationReferenceObject relation)`

### `IncompleteConfigurationsRelationReference` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.OptionsTables`)
**Свойства:** Classes: IncompleteConfigurationsRelationTypes

### `IncompleteConfigurationsRelationReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.OptionsTables`)
**Свойства:** Class: IncompleteConfigurationsRelationType, Type: RelationType, Column: IncompleteConfigurationReferenceObject, Row: IncompleteConfigurationReferenceObject

### `IncompleteConfigurationsRelationType` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.OptionsTables`)
**Свойства:** Classes: IncompleteConfigurationsRelationTypes, IsIncompleteConfigurationsRelation: Boolean

### `IncompleteConfigurationsRelationTypes` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.OptionsTables`)
**Свойства:** IncompleteConfigurationsRelation: IncompleteConfigurationsRelationType

### `IncompleteConfigurationType` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.OptionsTables`)
**Свойства:** Classes: IncompleteConfigurationTypes, IsIncompleteConfiguration: Boolean

### `IncompleteConfigurationTypes` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.OptionsTables`)
**Свойства:** IncompleteConfiguration: IncompleteConfigurationType

### `OptionsTableReference` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.OptionsTables`)
**Свойства:** Classes: OptionsTableTypes

### `OptionsTableReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.OptionsTables`)
**Свойства:** Class: OptionsTableType, Name: String, Kind: OptionTableKind, OptionsTableSet: OptionsTableSetReferenceObject, IncompleteConfigurations: IList`1, OptionsInRows: IList`1, OptionsInColumns: IList`1
**Методы:**
- `IncompleteConfigurationReferenceObject AddIncompleteConfiguration(IncompleteConfigurationReferenceObject configuration)`
- `ReferenceObject AddOptionToColumns(ProjectOptionsReferenceObject option)`
- `ReferenceObject AddOptionToRows(ProjectOptionsReferenceObject option)`
- `IncompleteConfigurationReferenceObject[] GetIncompleteConfigurationsInColumns()`
- `IncompleteConfigurationReferenceObject[] GetIncompleteConfigurationsInRows()`
- `Boolean RemoveIncompleteConfiguration(IncompleteConfigurationReferenceObject configuration)`
- `Boolean RemoveOptionFromColumns(ProjectOptionsReferenceObject option)`
- `Boolean RemoveOptionFromRows(ProjectOptionsReferenceObject option)`
- `ProjectOptionsReferenceObject[] GetOrderedOptions(IList`1 options, String[] optionsOrder)`

### `OptionsTableSetReference` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.OptionsTables`)
**Свойства:** Classes: OptionsTableSetTypes

### `OptionsTableSetReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.OptionsTables`)
**Свойства:** ConfigurationManagerObject: ReferenceObject, Class: OptionsTableSetType, Name: String, Comment: String, OptionsTables: IList`1, Constraints: IList`1, ProjectOptions: IList`1, ProjectOptionValues: IList`1
**Методы:**
- `OptionsTableReferenceObject AddTable(OptionsTableReferenceObject table)`
- `OptionsTableSetReferenceObject CopyTableSet()`
- `OptionsTableReferenceObject[] GetMajorTables()`
- `OptionsTableReferenceObject[] GetMinorTables()`
- `Boolean RemoveTable(OptionsTableReferenceObject table)`

### `OptionsTableSetType` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.OptionsTables`)
**Свойства:** Classes: OptionsTableSetTypes, IsOptionsTableSet: Boolean

### `OptionsTableSetTypes` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.OptionsTables`)
**Свойства:** OptionsTableSet: OptionsTableSetType

### `OptionsTableType` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.OptionsTables`)
**Свойства:** Classes: OptionsTableTypes, IsOptionsTable: Boolean, IsDiversityTable: Boolean

### `OptionsTableTypes` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.OptionsTables`)
**Свойства:** OptionsTable: OptionsTableType, DiversityTable: OptionsTableType

### `ProjectOptionsReference` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.OptionsTables`)
**Свойства:** Classes: ProjectOptionsTypes

### `ProjectOptionsReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.OptionsTables`)
**Свойства:** Class: ProjectOptionsType, Code: String, Kind: ProjectOptionsKind, IsMajor: Boolean, Values: IList`1, ProductImpact: OptionImpactKind, ProductOption: ProductOptionsReferenceObject, OptionsTableSets: OptionsTableSetReferenceObject, OptionsTableInColumns: ReferenceObjectCollection, OptionsTableInRows: ReferenceObjectCollection, OptionsTableInHiddenOptions: ReferenceObjectCollection
**Методы:**
- `String GetProductOptionName()`

### `ProjectOptionsType` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.OptionsTables`)
**Свойства:** Classes: ProjectOptionsTypes, IsProjectOption: Boolean

### `ProjectOptionsTypes` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.OptionsTables`)
**Свойства:** ProjectOption: ProjectOptionsType

### `ProjectOptionValuesReference` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.OptionsTables`)
**Свойства:** Classes: ProjectOptionValuesTypes

### `ProjectOptionValuesReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.OptionsTables`)
**Свойства:** Class: ProjectOptionValuesType, Name: StringParameter, Code: String, ProjectOption: ProjectOptionsReferenceObject, ProductOptionValue: ProductOptionValuesReferenceObject, ConfigurationManagerObjects: IEnumerable`1, IncompleteEquipments: IEnumerable`1, OptionsTableSet: OptionsTableSetReferenceObject

### `ProjectOptionValuesType` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.OptionsTables`)
**Свойства:** Classes: ProjectOptionValuesTypes, IsProjectOptionValues: Boolean

### `ProjectOptionValuesTypes` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.OptionsTables`)
**Свойства:** ProjectOptionValue: ProjectOptionValuesType

### `ApplicabilityRecordsCreator` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.ProductsApplicability.Creators`)
**Методы:**
- `Task`1 CreateDefaultBaseApplicabilityRecord(IntervalReferenceObject object, CancellationToken cancellationToken)`
- `Task`1 CreateApplicabilityRecord(IntervalReferenceObject applicabilityObject, NumberRange numberRange, CancellationToken cancellationToken)`

### `NumberRange` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.ProductsApplicability.Creators`)
**Свойства:** StartNumber: Int32, EndNumber: Int32
**Методы:**
- `String GetRangeMask(ServerConnection connection)`
- `Boolean Validate(Int32 startNumber, Int32 endNumber, ServerConnection connection)`
- `String GetValidationMessage(String numberRangeString)`

### `OptionRecordsCreator` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.ProductsApplicability.Creators`)
**Методы:**
- `Task`1 CreateOptionRecords(IntervalReferenceObject applicabilityObject, OptionsFilter optionsFilter, CancellationToken cancellationToken)`

### `ProductsClassifierReferenceObjectHelper` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.ProductsClassifier.Helpers`)
**Методы:**
- `List`1 GetAvalableProducOptionValues(ProductsClassifierReferenceObject product, Nullable`1 activityDate, Boolean& isLimitedByProduct)`
- `Boolean MatchByActivityDate(ProductsClassifierReferenceObject product, Nullable`1 activityDate)`
- `ProductsClassifierReferenceObject GetParentProduct(ProductsClassifierReferenceObject obj)`

### `MappingOptions` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.StructuresMapping`)
**Свойства:** Mode: TreeMovingMode, Macros: Macro, MethodName: String, UniqueKeyPath: IEnumerable`1, ValidateConflicts: Boolean

### `StructuresMappingMacroContext` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.StructuresMapping`)
**Свойства:** SourceInstance: IEnumerable`1, TargetInstance: IEnumerable`1

### `StructureVariantFilterData` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.StructureVariants`)
**Свойства:** ActiveStructureVariant: StructureVariantsReferenceObject, LoadingStructureVariants: List`1

### `NomenclatureAnalogueReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.Substitutes`)
**Свойства:** AnalogueLinkedObject: ReferenceObject

### `NomenclatureEquivalentGroupReference` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.Substitutes`)
**Свойства:** Classes: NomenclatureEquivalentGroupTypes

### `NomenclatureEquivalentGroupReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.Substitutes`)
**Свойства:** Class: NomenclatureEquivalentGroupType, Comment: StringParameter, Count: DoubleParameter, EquivalentLinkedObject: ReferenceObject

### `NomenclatureEquivalentGroupType` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.Substitutes`)
**Свойства:** Classes: NomenclatureEquivalentGroupTypes, IsNomenclatureEquivalentGroupReferenceObject: Boolean

### `NomenclatureEquivalentGroupTypes` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.Substitutes`)
**Свойства:** NomenclatureEquivalentGroupReferenceObject: NomenclatureEquivalentGroupType

### `NomenclatureEquivalentReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.Substitutes`)
**Свойства:** EquivalentGroup: IEnumerable`1
**Методы:**
- `ReferenceObject CreateEquivalentGroup(Guid listObjectClass) (+1)`
- `Void FormatComment()`

### `NomenclatureSubstituteReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.Substitutes`)
**Свойства:** Comment: StringParameter, Class: NomenclatureSubstitutesType

### `NomenclatureSubstitutesReference` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.Substitutes`)
**Свойства:** Classes: NomenclatureSubstitutesTypes

### `NomenclatureSubstitutesType` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.Substitutes`)
**Свойства:** Classes: NomenclatureSubstitutesTypes, IsNomenclatureAnalogueReferenceObject: Boolean, IsNomenclatureEquivalentReferenceObject: Boolean

### `NomenclatureSubstitutesTypes` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.Substitutes`)
**Свойства:** NomenclatureAnalogueReferenceObject: NomenclatureSubstitutesType, NomenclatureEquivalentReferenceObject: NomenclatureSubstitutesType

### `SubstitutesHelper` (Namespace: `TFlex.DOCs.Model.References.Nomenclature.Substitutes`)
**Методы:**
- `List`1 Replace(NomenclatureHierarchyLink sourceLink, NomenclatureSubstituteReferenceObject replacement)`

### `InstancesMappingTypeHelper` (Namespace: `TFlex.DOCs.Model.References.ObjectsInstances`)
**Методы:**
- `String GetInstanceMappingTypeStringValue(InstancesMappingType mappingType)`

### `ReferenceObjectExtensions` (Namespace: `TFlex.DOCs.Model.References.ObjectsInstances`)
**Методы:**
- `IEnumerable`1 GetObjectInstances(ReferenceObject referenceObject, Boolean loadParents)` [has Async]
- `IEnumerable`1 GetObjectInstancesWithoutConfiguration(ReferenceObject referenceObject)` [has Async]
- `ReferenceObject GetReferenceObject(ReferenceObjectInstance referenceObject, Reference reference)` [has Async]

### `ReferenceObjectInstance` (Namespace: `TFlex.DOCs.Model.References.ObjectsInstances`)
**Свойства:** ReferenceObject: ReferenceObject, LinkedComplexLink: ComplexHierarchyLink, ComplexHierarchyLink: ComplexHierarchyLink, SourceStructureInstances: IEnumerable`1, AltRepGroupGuidParameter: Parameter
**Методы:**
- `Int32 ReplaceRepresentation(Int32 sourceNewAltRepId)` [has Async]
- `IEnumerable`1 GetTargetStructureInstances(StructureTypesReferenceObject targetStructure)` [has Async]
- `IEnumerable`1 GetTargetStructuresInstances()` [has Async]

### `PrintingProfileReference` (Namespace: `TFlex.DOCs.Model.References.PrintingProfiles`)
**Свойства:** Classes: PrintingProfileTypes

### `PrintingProfileType` (Namespace: `TFlex.DOCs.Model.References.PrintingProfiles`)
**Свойства:** Classes: PrintingProfileTypes, IsTimeChartPrintingProfile: Boolean

### `PrintingProfileTypes` (Namespace: `TFlex.DOCs.Model.References.PrintingProfiles`)
**Свойства:** TimeChartPrintingProfile: PrintingProfileType

### `TimeChartPrintingProfile` (Namespace: `TFlex.DOCs.Model.References.PrintingProfiles`)
**Свойства:** PageNumberPosition: PageNumberPosition, FitContentMode: FitContentMode, Class: PrintingProfileType, Name: StringParameter, Default: BooleanParameter, Shared: BooleanParameter, Begin: DateTimeParameter, End: DateTimeParameter, PageWidth: DoubleParameter, PageHeight: DoubleParameter, Landscape: BooleanParameter, LeftMargin: DoubleParameter, RightMargin: DoubleParameter, TopMargin: DoubleParameter, BottomMargin: DoubleParameter, PageNumberPositionParameter: Int32Parameter, FitContentModeParameter: Int32Parameter, TransparentBackground: BooleanParameter, HighlightToday: BooleanParameter, RepeatHeader: BooleanParameter, PrintLegend: BooleanParameter, PrintTimeChart: BooleanParameter, CopyCount: Int32Parameter, Resolution: Int32Parameter, PageHeader: StringParameter, PageFooter: StringParameter, FirstPageHeader: StringParameter, FirstPageFooter: StringParameter, LastPageHeader: StringParameter, LastPageFooter: StringParameter, DifferentFirstPage: BooleanParameter, DifferentLastPage: BooleanParameter

### `BaseTaskObject` (Namespace: `TFlex.DOCs.Model.References.Projects`)
**Свойства:** DateTimeInterval: TimeInterval, IsStartTimeSet: Boolean, IsEndTimeSet: Boolean, IsDurationSet: Boolean, StartTime: DateTime, EndTime: DateTime, Duration: Int32, Description: String, OrderParameter: OrderParameter, Order: String, TaskOrderNumber: Int32, Color: Int32, Style: ProjectStylesReferenceObject, IsFixingTask: Boolean, ResourceLinks: ReferenceObjectCollection`1, Labels: IEnumerable`1, StartTaskMessageGuid: GuidParameter, StartMailTask: MailTask, Progress: Double, Plan: BaseTaskObject, State: TaskState, AutoSendMessage: Boolean, MainWorkflowGuid: Guid, MainProcedureGuid: Guid, WorkflowObjects: ReferenceObjectCollection, WorkflowReference: Reference, ProceduresReference: Reference, ProcedureObjects: ReferenceObjectCollection, AnyReferenceObjectsLink: AnyReferenceLink, Owner: UserReferenceObject, Costs: Double, CurrentCosts: Double, WorkTimeManager: WorkTimeManager, Texts: TaskTexts, TopText: String, BottomText: String, LeftText: String, RightText: String, InsideText: String
**Методы:**
- `Boolean SetParent(ReferenceObject parentObject)`
- `Void AddToSaveSet(ReferenceObjectSaveSet saveSet)`
- `Void UpdateStartTime(DateTime startTime, ReferenceObjectSaveSet saveSet)`
- `Void UpdateEndTime(DateTime endTime, ReferenceObjectSaveSet saveSet)`
- `Void MoveStartTimeTo(DateTime date)`
- `Void MoveEndTimeTo(DateTime date)`
- `List`1 GetChildTasks()`
- `Void StartTask()`
- `Void SetStartMailTask(Guid mailTaskGuid)`
- `Void UpdateCosts(ReferenceObjectSaveSet saveSet)`
- `Void OnCalendarChanged()`
- `DateTime CalcSummTime(DateTime startTime, Int32 duration)`
- `Boolean ChangeEndTimeByDuration(Int32 duration)`
- `Void ChangeStartTimeByDuration(Int32 duration)`
- `Int32 CalcDuration(DateTime start, DateTime end)`
- `Void ChangeDurationByEndTime(DateTime endTime)`
- `Void ChangeDurationByStartTime(DateTime startTime)`

### `DictionaryComparer` (Namespace: `TFlex.DOCs.Model.References.Projects`)
**Методы:**
- `Int32 Compare(Guid x, Guid y)`

### `LabelReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Projects`)
**Свойства:** Time: DateTime, Name: String, Comment: String, ImageObject: ImageReferenceObject

### `LabelsReference` (Namespace: `TFlex.DOCs.Model.References.Projects`)
**Свойства:** Classes: ResourceLinkTypes

### `LabelType` (Namespace: `TFlex.DOCs.Model.References.Projects`)
**Свойства:** Classes: LabelTypes, IsLabel: Boolean

### `LabourResourceLinkObject` (Namespace: `TFlex.DOCs.Model.References.Projects`)
**Свойства:** Employment: Double, BasicRate: Double
**Методы:**
- `Void SetDuration(Int32 duration)`
- `Void UpdateValue()`

### `MaterialResourceLinkObject` (Namespace: `TFlex.DOCs.Model.References.Projects`)
**Свойства:** SpendingType: ResourceSpendingType, AutoCalc: Boolean, Price: Double
**Методы:**
- `Void UpdateValue()`

### `OrderParameter` (Namespace: `TFlex.DOCs.Model.References.Projects`)
**Свойства:** Value: String, IsNull: Boolean
**Методы:**
- `Int32 GetTaskPostition(BaseTaskObject task)`
- `Void AddTask(BaseTaskObject task)`
- `Void MoveTask(BaseTaskObject movedTask, BaseTaskObject targetTask, Int32 shift)`
- `Void InsertTasks(BaseTaskObject targetTask, List`1 tasks, Boolean before)`
- `Void InsertBefore(BaseTaskObject targetTask, List`1 tasks)`
- `Void InsertAfter(BaseTaskObject targetTask, List`1 tasks)`
- `Void RemoveTask(BaseTaskObject task)`
- `Void RemoveAllTasks()`
- `TypeCode GetTypeCode()`
- `Void Refresh()`

### `ProjectObject` (Namespace: `TFlex.DOCs.Model.References.Projects`)
**Свойства:** Calendar: CalendarReferenceObject
**Методы:**
- `List`1 GetPlanList()`
- `Void AddPlan(ProjectObject plan)`

### `ProjectReference` (Namespace: `TFlex.DOCs.Model.References.Projects`)
**Свойства:** Classes: ProjectTypes
**Методы:**
- `Boolean ShowErrorLinkDialog(TaskLinkReferenceObject errorLink)`
- `IComparer`1 GetAfterLoadSortComparer()`

### `ProjectReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Projects`)
**Свойства:** Name: String, AutoCheckOut: Boolean
**Методы:**
- `Void CheckInProject(Boolean ShowSaveDialog)`

### `ProjectTasksOrder` (Namespace: `TFlex.DOCs.Model.References.Projects`)
**Свойства:** Tasks: List`1

### `ProjectType` (Namespace: `TFlex.DOCs.Model.References.Projects`)
**Свойства:** Classes: ProjectTypes, IsBaseTask: Boolean, IsTask: Boolean, IsProject: Boolean, IsProjectsFolder: Boolean

### `ProjectTypes` (Namespace: `TFlex.DOCs.Model.References.Projects`)
**Свойства:** Project: ProjectType, Task: ProjectType, BaseTask: ProjectType

### `ResourceLinkComputingParameter` (Namespace: `TFlex.DOCs.Model.References.Projects`)
**Методы:**
- `Void ForceSetValue(Object value)`

### `ResourceLinkReference` (Namespace: `TFlex.DOCs.Model.References.Projects`)
**Свойства:** Classes: ResourceLinkTypes

### `ResourceLinkReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Projects`)
**Свойства:** Resource: ResourceReferenceObject, Quantity: Double, Value: Double, SpendingValueType: Int32, BaseTask: BaseTaskObject
**Методы:**
- `Void UpdateValue()`

### `ResourceLinkType` (Namespace: `TFlex.DOCs.Model.References.Projects`)
**Свойства:** Classes: ResourceLinkTypes, IsBaseResource: Boolean, IsLabourResource: Boolean, IsMaterialResource: Boolean, IsSpendingResource: Boolean

### `SpendingResourceLinkObject` (Namespace: `TFlex.DOCs.Model.References.Projects`)
**Свойства:** Price: Double
**Методы:**
- `Void UpdateValue()`

### `TaskLinkClassType` (Namespace: `TFlex.DOCs.Model.References.Projects`)
**Свойства:** Classes: TaskLinkTypes

### `TaskLinkObject` (Namespace: `TFlex.DOCs.Model.References.Projects`)
**Свойства:** ChildObject: TaskObject, LinkType: TaskLinkType, ParentObject: BaseTaskObject, Delay: Int32
**Методы:**
- `Void UpdateParentObject()`

### `TaskLinkReference` (Namespace: `TFlex.DOCs.Model.References.Projects`)
**Свойства:** Classes: TaskLinkTypes

### `TaskLinkReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Projects`)
**Свойства:** LinkType: TaskLinkType, ParentTask: TaskObject, Delay: Int32, ChildTask: TaskObject
**Методы:**
- `Void UpdateParentObject()`
- `Boolean Check()`

### `TaskObject` (Namespace: `TFlex.DOCs.Model.References.Projects`)
**Свойства:** ParentTasksList: ReferenceObjectCollection, LinksToParentTasks: ReferenceObjectCollection`1
**Методы:**
- `IEnumerable`1 GetLinksFromChild()`
- `Boolean AddNewLink(TaskObject source, TaskLinkType linkType, Int32 delay)`
- `Void RemoveLinkToParentObject(BaseTaskObject parent)`
- `Void UpdatePositionByTask(BaseTaskObject task)`

### `TaskTexts` (Namespace: `TFlex.DOCs.Model.References.Projects`)
**Свойства:** Top: TaskText, Bottom: TaskText, Left: TaskText, Right: TaskText, Inside: TaskText
**Методы:**
- `TaskTexts Merge(TaskTexts other)`

### `TimeIntervalExt` (Namespace: `TFlex.DOCs.Model.References.Projects`)
**Методы:**
- `DateTimeInterval ToDateTimeInterval(TimeInterval interval, Boolean includeStart, Boolean includeEnd) (+1)`

### `CheckStatusTypeConverter` (Namespace: `TFlex.DOCs.Model.References.RecordControlCards.Mail`)
**Методы:**
- `Boolean GetStandardValuesSupported(ITypeDescriptorContext context)`
- `StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)`
- `Boolean CanConvertTo(ITypeDescriptorContext context, Type destinationType)`
- `Object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, Object value, Type destinationType)`

### `Converter` (Namespace: `TFlex.DOCs.Model.References.RecordControlCards.Mail`)
**Методы:**
- `String GetString(CheckStatusType type)`

### `MailResolution` (Namespace: `TFlex.DOCs.Model.References.RecordControlCards.Mail`)
**Свойства:** CanReject: Boolean, Responsible: User, CheckStatus: CheckStatusType, Comment: String

### `ResolutionMailData` (Namespace: `TFlex.DOCs.Model.References.RecordControlCards.Mail`)
**Свойства:** CheckStatus: Int32, ResponsibleGuid: Guid, Responsible: User, Comment: String

### `RelationTableFormulaMacro` (Namespace: `TFlex.DOCs.Model.References.RelationTables.Macros`)
**Свойства:** CodeOffset: Int32

### `RelationTableMacroContext` (Namespace: `TFlex.DOCs.Model.References.RelationTables.Macros`)
**Свойства:** Column: ReferenceObject, Row: ReferenceObject

### `RelationTableMacroProvider` (Namespace: `TFlex.DOCs.Model.References.RelationTables.Macros`)
**Свойства:** Context: RelationTableMacroContext, ColumnObject: RefObj, RowObject: RefObj

### `RemarkContext` (Namespace: `TFlex.DOCs.Model.References.Remarks`)
**Свойства:** ReferenceGuid: Guid, ReferenceObjectGuid: Guid, ReferenceObjectVersion: Int32, XmlData: String, BinaryData: Byte[]

### `RemarkReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Remarks`)
**Свойства:** Text: StringParameter, Status: Int32Parameter, Importance: Int32Parameter, ClosingDate: DateTimeParameter, ClosingAuthor: StringParameter, ClosingComment: StringParameter, Class: RemarkType, Dependencies: ReferenceObjectCollection, StatusType: RemarkStatus
**Методы:**
- `Int32 GetReferenceObjectVersion(Guid referenceObject)`
- `ReferenceObject AddDependency(ReferenceObject newLinkedObject)`
- `Boolean RemoveDependency(ReferenceObject linkedObject)`
- `ICollection`1 FindDependencies(ReferenceObject referenceObject)`

### `RemarksReference` (Namespace: `TFlex.DOCs.Model.References.Remarks`)
**Свойства:** Classes: RemarkTypes, Context: RemarkContext
**Методы:**
- `List`1 FindRemarks(ReferenceObject referenceObject, Filter filter) (+1)` [has Async]
- `IReadOnlyCollection`1 FindRequestRemarkObjects(ReferenceObject referenceObject)`
- `RemarkReferenceObject AddPdfRemark(Guid reference, Guid referenceObject, String text, String xmlData, RequestRemarkObject requestRemark) (+1)`
- `RemarkReferenceObject AddImageRemark(Guid reference, Guid referenceObject, String text, String xmlData, RequestRemarkObject requestRemark) (+1)`
- `RemarkReferenceObject AddCadRemark(Guid reference, Guid referenceObject, String text, Byte[] binaryData, RequestRemarkObject requestRemark) (+1)`
- `RemarkReferenceObject AddTextRemark(Guid reference, Guid referenceObject, String text, RequestRemarkObject requestRemark) (+1)`
- `RemarkReferenceObject AddRequestRemark(Guid reference, Guid referenceObject, String text, RequestRemarkObject requestRemark) (+1)`

### `RemarkType` (Namespace: `TFlex.DOCs.Model.References.Remarks`)
**Свойства:** Classes: RemarkTypes, IsPdfRemark: Boolean, IsTextRemark: Boolean, IsCadRemark: Boolean, IsRequestRemark: Boolean, IsImageRemark: Boolean

### `RemarkTypes` (Namespace: `TFlex.DOCs.Model.References.Remarks`)
**Свойства:** PdfRemark: RemarkType, TextRemark: RemarkType, CadRemark: RemarkType, RequestRemark: RemarkType, ImageRemark: RemarkType

### `RequestRemarkObject` (Namespace: `TFlex.DOCs.Model.References.Remarks`)
**Свойства:** Performers: ReferenceObjectCollection
**Методы:**
- `ReferenceObject AddPerformer(ReferenceObject newLinkedObject)`
- `Boolean RemovePerformer(ReferenceObject linkedObject)`

### `TextRemarkObject` (Namespace: `TFlex.DOCs.Model.References.Remarks`)
**Свойства:** Detail: StringParameter, Files: ReferenceObjectCollection
**Методы:**
- `ReferenceObject AddFile(ReferenceObject newLinkedObject)`
- `Boolean RemoveFile(ReferenceObject linkedObject)`

### `DependenciesOfRemarksReference` (Namespace: `TFlex.DOCs.Model.References.Remarks.DependeciesOfRemarks`)
**Свойства:** Classes: DependencyOfRemarkTypes
**Методы:**
- `List`1 FindDependencies(ReferenceObject referenceObject) (+1)`

### `DependencyOfRemarkReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Remarks.DependeciesOfRemarks`)
**Свойства:** Class: DependencyOfRemarkType, ReferenceGuid: GuidParameter, ReferenceObjectGuid: GuidParameter, ReferenceObjectVersion: Int32Parameter, Binary_Data: ByteArrayParameter, XML_Data: StringParameter, LinkToRemark: RemarkReferenceObject

### `DependencyOfRemarkType` (Namespace: `TFlex.DOCs.Model.References.Remarks.DependeciesOfRemarks`)
**Свойства:** Classes: DependencyOfRemarkTypes, IsDependenciesOfRemarks: Boolean

### `DependencyOfRemarkTypes` (Namespace: `TFlex.DOCs.Model.References.Remarks.DependeciesOfRemarks`)
**Свойства:** DependenciesOfRemarks: DependencyOfRemarkType

### `AuthorInfo` (Namespace: `TFlex.DOCs.Model.References.Reporting`)
**Свойства:** Connection: ServerConnection, AuthorFullName: String, AuthorFirstName: String, AuthorLastName: String, AuthorPatronymic: String, AuthorShortName: String

### `IReportGenerationContextWithConfigurationSettings` (Namespace: `TFlex.DOCs.Model.References.Reporting`)
**Свойства:** ConfigurationSettingsInfo: String

### `OutputDocument` (Namespace: `TFlex.DOCs.Model.References.Reporting`)
**Свойства:** Class: ReportType, Name: StringParameter, ContentType: ReportContentType, OpenFile: Boolean, OverwriteReportFile: Boolean, ContentReference: StringParameter, ContentReferenceGuid: Guid, ObjectList: StringParameter, ContentReferencePath: ReferencePath, MenuName: StringParameter, FilterXml: StringParameter, OpenType: OpenReportType, HasTemplateProperties: Boolean, AllowCustomizeDesign: Boolean, CanExecute: Boolean, ContentReferenceObjectClassGuids: Guid[], ContentReferenceReportClassGuids: Guid[], TemplateFile: FileObject, Generator: ReportGenerator
**Методы:**
- `Void EditTemplateProperties(IWin32Window owner)`
- `Void CustomizeDesign(IWin32Window owner) (+1)`
- `Void CustomizeDesign_Core(IntPtr owner)`
- `Filter GetFilter()`
- `ReferenceFilterObject GetFilterObject()`
- `Void SetFilter(Filter filter)`
- `Void SetFilterObject(ReferenceFilterObject filterObject)`
- `Boolean CanGenerateForClass(ClassObject classObject, Boolean throwOnError)`
- `ReportGenerationContext Generate(ReferenceObject obj, ComplexHierarchyLink link) (+4)`
- `Boolean ValidateLicense(Boolean throwOnError)`
- `Void InitializeContext(ReportGenerationContext context, Boolean generating)`

### `ReferenceObjectWithLink` (Namespace: `TFlex.DOCs.Model.References.Reporting`)
**Свойства:** ReferenceObject: ReferenceObject, Link: ComplexHierarchyLink, Signature: Signature

### `Report` (Namespace: `TFlex.DOCs.Model.References.Reporting`)
**Свойства:** ReportFileName: StringParameter, OutputFileFormat: StringParameter, ReportResultProcessingFormula: StringParameter, DefaultFolder: FolderObject, AutoAttachLink: GuidParameter, MethodExtraLoading: StringParameter, UserDialog: UserDialogObject, MacroExtraLoading: CodeMacro, HasTemplateProperties: Boolean, TemplateProperties: ByteArrayParameter, AllowCustomizeDesign: Boolean
**Методы:**
- `Void EditTemplateProperties(IWin32Window owner)`
- `Void CustomizeDesign(IntPtr owner)`
- `Boolean ValidateMacroExtraLoading(Macro macro, String method, Boolean throwOnError)`
- `Boolean ValidateLicense(Boolean throwOnError)`
- `ReportGenerationContext Generate(ReferenceObject obj, ComplexHierarchyLink link, ReferenceObject resultFileOwner, ParameterGroup fileLink) (+5)`
- `Void InitializeContext(ReportGenerationContext context, Boolean generating)`

### `ReportConfigurationSettingsData` (Namespace: `TFlex.DOCs.Model.References.Reporting`)
**Свойства:** TypicalConfiguration: Guid, Product: Guid, DesignContext: Guid, ApplyStructureType: Boolean, ActiveStructureTypeGuid: Guid, ActiveStructureTypes: List`1, DisplayStructureTypes: List`1, ShowBaseStructure: Boolean, StatusesDate: String, ShowEmptyCategories: Boolean, ShowAllCategories: Boolean, ShowDeletedInDesignContextLinks: Boolean, ApplyProductFilter: Boolean, ApplyCategoriesFilter: Boolean, ApplyTerms: Boolean, ApplyDate: Boolean, ApplyDesignContext: Boolean, ApplyOptionValues: Boolean, ApplyProductDesignNumber: Boolean, ApplyProductMilestoneNumber: Boolean, ProductCategories: List`1, SelectRevisionsTermsObject: Guid, SelectRevisionsTerms: FilterDataCollection, ProductMilestoneNumber: Nullable`1, ProductDesignNumber: Nullable`1, SerialNumberGuid: Guid, OptionValues: List`1, CustomCriteriaValueData: CustomCriteriaValueData
**Методы:**
- `String Serialize(ServerConnection connection, ConfigurationSettings configurationSettings)`
- `ConfigurationSettings Deserialize(ServerConnection connection, String contextData)`

### `ReportGenerationContext` (Namespace: `TFlex.DOCs.Model.References.Reporting`)
**Свойства:** PathToParameterGroup: String, IsPreview: Boolean, IsPreviewSaving: Boolean, ReportId: Int32, RootObjectId: Int32, ParameterGroup: ParameterGroup, Reference: Reference, Objects: List`1, UserDialogTypeGuid: Guid, UserDialog: UserDialogObject, ContentFilter: Filter, ReportFolderPath: String, ReportFileName: String, ReportGenerationCode: String, DebugMode: Boolean, ReportGenerationCodeReferences: String, GenerationErrors: String, SetOfReportFilePaths: List`1, RequiredParametersExceptionCallback: Func`2, OverwriteReportFile: Nullable`1, OpenFile: Nullable`1, MacroContext: ReportMacroContext, ReferenceId: Int32, ObjectsInfo: List`1, ObjectPositions: List`1, RefreshPositons: Boolean, ContentType: ReportContentType, UpdateOutputDocument: Boolean, AuthorInfo: IAuthorInfo, ReportFilePath: String, TemplateFilePath: String, DefaultFolderObject: FolderObject, ReportFileObject: FileReferenceObject, DefaultFolder: String, UserName: String, Password: String, Server: String, WindowsAuthentication: Boolean, ConnectionParametersData: String, TemplateProperties: Byte[], ProductStructureId: Int32, GeneratorProgramFilePath: String, ReportDataSetReader: IReportDataSetReader, SetOfReportsParameters: List`1, TotalPages: Int32, PagesNumerationInfo: PageNumerationInfoCollection, MacroExtraLoading: ValueTuple`2
**Методы:**
- `Dictionary`2 GetLinksWithNewPositions()`
- `Void CopyTemplateFile()`
- `Void CopyFrom(IReportGenerationContext context)`
- `Byte[] Serialize(IReportGenerationContext context)`
- `IReportGenerationContext Deserialize(Byte[] contextData)`

### `ReportGenerationContextFactory` (Namespace: `TFlex.DOCs.Model.References.Reporting`)
**Методы:**
- `ReportGenerationContext CreateContext(ReferenceObject referenceObject, ComplexHierarchyLink link, ReportContentType contentType) (+4)`

### `ReportGenerationContextWithConfigurationSettings` (Namespace: `TFlex.DOCs.Model.References.Reporting`)
**Свойства:** ConfigurationSettingsInfo: String, Reference: Reference
**Методы:**
- `Void CopyFrom(IReportGenerationContext context)`

### `ReportGenerationResult` (Namespace: `TFlex.DOCs.Model.References.Reporting`)
**Свойства:** ReportFilePath: String, GenerationErrors: String, ReportFileObject: FileReferenceObject

### `ReportGenerator` (Namespace: `TFlex.DOCs.Model.References.Reporting`)
**Свойства:** Name: StringParameter, ModuleName: StringParameter, GeneratorClassName: StringParameter, SupportsTemplateProperties: BooleanParameter, CustomizableDesign: BooleanParameter, StandAlone: BooleanParameter, RefreshPositions: BooleanParameter, OperationTimeout: Int32Parameter
**Методы:**
- `IEnumerable`1 GetCommands()`
- `IReportGenerator CreateGenerator(Boolean forGeneration)`

### `ReportGeneratorCommand` (Namespace: `TFlex.DOCs.Model.References.Reporting`)
**Свойства:** Name: StringParameter, Identifier: StringParameter, Icon: IconParameter

### `ReportGeneratorProxy` (Namespace: `TFlex.DOCs.Model.References.Reporting`)
**Свойства:** DefaultReportFileExtension: String
**Методы:**
- `Void Generate(IReportGenerationContext context)`
- `Boolean EditTemplateProperties(Byte[]& data, IReportGenerationContext context, IWin32Window owner)`

### `ReportPreviewContext` (Namespace: `TFlex.DOCs.Model.References.Reporting`)
**Свойства:** IsPreview: Boolean, FilePath: String, Parameters: String, ShowToolsButtons: Boolean, LinkedObjectId: Int32, LinkedReferenceId: Int32, AsyncModeSupported: Boolean, LinkedObjectGuid: Guid, EnablePrint: Boolean, ConfigurationSettings: String, CustomFilePreviewerGuid: Guid, DefaultFilePreviewerGuid: Guid, ObjectContext: String, LastModificationTime: DateTime, CanEditDocument: Boolean, FileExtension: String, HierarchyLink: Guid
**Методы:**
- `Void GenerateFile()`
- `Boolean IsChanged(IFilePreviewContext filePreviewContext)`

### `ReportPreviewContextWithConfigurationSettings` (Namespace: `TFlex.DOCs.Model.References.Reporting`)
**Свойства:** ConfigurationSettingsInfo: String, Reference: Reference
**Методы:**
- `Void CopyFrom(IReportGenerationContext context)`

### `ReportReference` (Namespace: `TFlex.DOCs.Model.References.Reporting`)
**Методы:**
- `OutputDocument FindByTemplate(FileObject template)`

### `ReportType` (Namespace: `TFlex.DOCs.Model.References.Reporting`)
**Свойства:** Classes: ReportTypes, IsReport: Boolean, IsSetOfReports: Boolean

### `SetOfReports` (Namespace: `TFlex.DOCs.Model.References.Reporting`)
**Свойства:** CanExecute: Boolean
**Методы:**
- `Boolean ValidateLicense(Boolean throwOnError)`
- `Void Generate(ReportGenerationContext context)`
- `Void InitializeContext(ReportGenerationContext context, Boolean generating)`
- `IEnumerable`1 GetReportParameters()`

### `ReportFormulaMacro` (Namespace: `TFlex.DOCs.Model.References.Reporting.ReportMacro`)
**Свойства:** CodeOffset: Int32

### `ReportGenerationResultFormulaCreator` (Namespace: `TFlex.DOCs.Model.References.Reporting.ReportMacro`)
**Методы:**
- `FormulaMacro CreateFormula(String formula)`

### `ReportGenerationResultFormulaMacro` (Namespace: `TFlex.DOCs.Model.References.Reporting.ReportMacro`)
**Свойства:** IsReturnValue: Boolean, CodeOffset: Int32

### `ReportGenerationResultMacroContext` (Namespace: `TFlex.DOCs.Model.References.Reporting.ReportMacro`)
**Свойства:** ReportGenerationResult: ReportGenerationResult

### `ReportGenerationResultMacroProvider` (Namespace: `TFlex.DOCs.Model.References.Reporting.ReportMacro`)
**Свойства:** Context: ReportGenerationResultMacroContext, ReportResult: ReportResult, РезультатОтчёта: РезультатОтчёта [RU only]

### `ReportMacro` (Namespace: `TFlex.DOCs.Model.References.Reporting.ReportMacro`)
**Свойства:** CodeOffset: Int32

### `ReportMacroContext` (Namespace: `TFlex.DOCs.Model.References.Reporting.ReportMacro`)
**Свойства:** Signature: Signature, ObjectsIds: Int32[], ReferenceId: Int32, ReportId: Int32, ContentType: ReportContentType, ReportFilePath: String, TemplateFilePath: String, AuthorFullName: String, AuthorFirstName: String, AuthorLastName: String, AuthorPatronymic: String, AuthorShortName: String, UserName: String, Server: String, WindowsAuthentication: Boolean, RefreshPositons: Boolean, UserDialog: UserDialogObject
**Методы:**
- `Object GetValue(Int32 objectId, String parameter)`
- `String GetString(Int32 objectId, String parameter)`
- `List`1 GetKitParameters()`

### `ReportMacroParameterAccessor` (Namespace: `TFlex.DOCs.Model.References.Reporting.ReportMacro`)
**Свойства:** Item: DynamicType

### `ReportMacroProvider` (Namespace: `TFlex.DOCs.Model.References.Reporting.ReportMacro`)
**Свойства:** Context: ReportMacroContext, CurrentSignature: SignatureObj, KitParameter: ReportMacroParameterAccessor [RU: ПараметрКомплекта], ТекущаяПодпись: Подпись [RU only]

### `ContextVariables` (Namespace: `TFlex.DOCs.Model.References.Reporting.ReportMacro.Flowchart.Classes`)
**Свойства:** Variables: ContextVariableInfo[]

### `ActivityContextExtensions` (Namespace: `TFlex.DOCs.Model.References.Reporting.ReportMacro.Flowchart.Extensions`)
**Методы:**
- `ReportMacroContext GetReportMacroContext(ActivityContext context)`
- `ReportMacroProvider GetReportMacroProvider(ActivityContext context)`

### `ReportMacroProviderAccessorExtensions` (Namespace: `TFlex.DOCs.Model.References.Reporting.ReportMacro.ObjectModel.Types.Extensions`)
**Методы:**
- `SignatureAccessor GetCurrentSignature(ReportMacroProvider macroProvider)`

### `ReportKitDocument` (Namespace: `TFlex.DOCs.Model.References.Reporting.ReportsKit`)
**Свойства:** Name: StringParameter, Report: OutputDocument
**Методы:**
- `Filter GetFilter()`

### `SetOfReportsParameter` (Namespace: `TFlex.DOCs.Model.References.Reporting.ReportsKit`)
**Свойства:** Name: StringParameter, Type: StringParameter, IsConstant: BooleanParameter, Value: StringParameter
**Методы:**
- `SetOfReportsParameter CreateContextParameter()`

### `ResourceReference` (Namespace: `TFlex.DOCs.Model.References.Resources`)
**Свойства:** Classes: ResourceTypes

### `ResourceReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Resources`)
**Свойства:** Name: String

### `ResourceType` (Namespace: `TFlex.DOCs.Model.References.Resources`)
**Свойства:** Classes: ResourceTypes, IsBaseResource: Boolean, IsLabourResource: Boolean, IsMaterialResource: Boolean, IsSpending: Boolean

### `AlphabeticRevisionLevelObject` (Namespace: `TFlex.DOCs.Model.References.Revisions`)
**Свойства:** EmptyValue: StringParameter, MinLength: Int32Parameter, RegexTemplate: String
**Методы:**
- `String GetNextValue(String previousValue)`
- `Int32 Compare(String x, String y)`
- `Boolean CanChangeParameter(Parameter p, Object newValue)`

### `NumericRevisionLevelObject` (Namespace: `TFlex.DOCs.Model.References.Revisions`)
**Свойства:** RegexTemplate: String
**Методы:**
- `String GetNextValue(String previousValue)`
- `Int32 Compare(String x, String y)`

### `RevisionLevelObject` (Namespace: `TFlex.DOCs.Model.References.Revisions`)
**Свойства:** RegexTemplate: String, Class: RevisionLevelsType, Name: StringParameter, FirstValue: StringParameter, Format: StringParameter, Icon: IconParameter
**Методы:**
- `String GetNextValue(String previousValue)`
- `Int32 Compare(String x, String y)`
- `Boolean CanChangeParameter(Parameter p, Object newValue)`

### `RevisionLevelsReference` (Namespace: `TFlex.DOCs.Model.References.Revisions`)
**Свойства:** Classes: RevisionLevelsTypes

### `RevisionLevelsType` (Namespace: `TFlex.DOCs.Model.References.Revisions`)
**Свойства:** Classes: RevisionLevelsTypes, IsAlphabeticRevisionLevel: Boolean, IsNumericRevisionLevel: Boolean

### `RevisionLevelsTypes` (Namespace: `TFlex.DOCs.Model.References.Revisions`)
**Свойства:** AlphabeticRevisionLevel: RevisionLevelsType, NumericRevisionLevel: RevisionLevelsType

### `RevisionNamingRuleObject` (Namespace: `TFlex.DOCs.Model.References.Revisions`)
**Свойства:** Class: RevisionNamingRulesType, Name: StringParameter, Template: StringParameter, FirstRevisionName: StringParameter, RevisionLevels: ReferenceObjectCollection`1
**Методы:**
- `String GetFirstRevisionName()`
- `String GetNewRevisionName(List`1 existingRevisionNames, String baseRevisionName, RevisionLevelObject forLevel)`
- `String GetLastRevisionName(List`1 existingNames)`
- `List`1 SortRevisionNames(List`1 revisionNames)`
- `Void RenameExistingRevisions(RevisionNamingRuleObject oldRule, Int32 referenceId, Int32 classObjectId)` [has Async]
- `RevisionLevelObject CreateRevisionLevel(Guid listObjectClass) (+1)`

### `RevisionNamingRulesReference` (Namespace: `TFlex.DOCs.Model.References.Revisions`)
**Свойства:** DefaultRevisionNamingRule: RevisionNamingRuleObject, Classes: RevisionNamingRulesTypes

### `RevisionNamingRulesType` (Namespace: `TFlex.DOCs.Model.References.Revisions`)
**Свойства:** Classes: RevisionNamingRulesTypes, IsRule: Boolean

### `RevisionNamingRulesTypes` (Namespace: `TFlex.DOCs.Model.References.Revisions`)
**Свойства:** Rule: RevisionNamingRulesType

### `ReferenceFilterObject` (Namespace: `TFlex.DOCs.Model.References.Search`)
**Свойства:** ShowButton: BooleanParameter, ButtonIcon: IconParameter, ButtonText: StringParameter, ButtonHint: StringParameter, Filter: Filter, IsPublic: Boolean, FilterReference: ReferenceInfo
**Методы:**
- `Boolean IsRightGroup(ParameterGroup masterGroup)`
- `Void SaveFilter()`

### `ReferenceFiltersGroupObject` (Namespace: `TFlex.DOCs.Model.References.Search`)
**Свойства:** FiltersGroup: FilterDataCollection

### `SearchQueryObject` (Namespace: `TFlex.DOCs.Model.References.Search`)
**Свойства:** ResultParameters: ReferencePathCollection, Filter: Filter, MainReference: ReferenceInfo

### `SearchQueryReference` (Namespace: `TFlex.DOCs.Model.References.Search`)
**Свойства:** PrivateFolder: SearchQueryFolderObject, CommonFolder: SearchQueryFolderObject, Classes: SearchQueryTypes, IsAvailable: Boolean
**Методы:**
- `List`1 FindReferenceFilters(Guid referenceId) (+2)` [has Async]
- `SearchQueryReferenceObject Find(String name)` [has Async]

### `SearchQueryReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Search`)
**Свойства:** Class: SearchQueryType, Caption: StringParameter

### `SearchQueryType` (Namespace: `TFlex.DOCs.Model.References.Search`)
**Свойства:** Classes: SearchQueryTypes, IsQuery: Boolean, IsFilter: Boolean, IsFolder: Boolean, IsFiltersGroup: Boolean

### `SearchQueryTypes` (Namespace: `TFlex.DOCs.Model.References.Search`)
**Свойства:** Instance: SearchQueryTypes, Folder: SearchQueryType, Query: SearchQueryType, Filter: SearchQueryType, FiltersGroup: SearchQueryType

### `TasksReference` (Namespace: `TFlex.DOCs.Model.References.Tasks`)
**Свойства:** Classes: TasksTypes
**Методы:**
- `Void Complete(TasksReferenceObject[] tasks)`
- `Void Uncomplete(TasksReferenceObject[] tasks)`
- `Void Cancel(TasksReferenceObject[] tasks, String comment)`
- `TasksReferenceObject[] GetSubtasksInProgress(TasksReferenceObject[] tasks)`
- `TasksReferenceObject[] GetSubtasksWithDeterminedStatus(TasksReferenceObject[] tasks, TaskStatus status)`
- `List`1 GetParentTasks(TasksReferenceObject[] tasks)`

### `TasksReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Tasks`)
**Свойства:** Class: TasksType, IsTask: Boolean, IsTheme: Boolean, IsCompleted: Boolean, InProgress: Boolean, IsCancelled: Boolean, IsOverdue: Boolean, IsSupportedAutoCreationAssignments: Boolean, StatusType: TaskStatus, Percent: Double, AutoCalculation: Boolean, Name: StringParameter, TargetDate: DateTimeParameter, Progress: ProgressParameter, Description: StringParameter, Importance: Int32Parameter, LocalElement: BooleanParameter, NoAssignment: BooleanParameter, AssignmentsAutoCreationIsDisabled: BooleanParameter, Status: Int32Parameter, AutomaticCalculation: BooleanParameter, Executor: User, Assignments: ReferenceObjectCollection`1, LinkedMaterials: AnyReferenceLink
**Методы:**
- `Boolean CanChangeParameter(Parameter p, Object newValue)`
- `Boolean CanChangeStatus(TaskStatus status)`
- `Boolean CanChangeLink(LinkInfo link, ReferenceObject addObject, ReferenceObject removeObject)`
- `Boolean IsAutoCalculateProgress()`
- `AssignmentReferenceObject CreateAssignment(User executor, AssignmentType type, Boolean isBasic, Boolean shouldSaved) (+1)`
- `Void RecalculateProgressByAssignments()`
- `Boolean AppointExecutor(User newExecutor)`
- `Void Complete()`
- `Void Uncomplete()`
- `Void Cancel(String comment)`
- `TasksReferenceObject[] GetSubtasksInProgress()`
- `Void RecalculateProgress()`
- `Void ChangeAutoCalculation(Boolean autoCalculation)`
- `Boolean CanChangeAutoCalculation()`
- `Boolean CanChangePercent()`
- `Void ChangePercent(Double percent)`
- `ReferenceObject AddLinkedAssignment(ReferenceObject newLinkedObject)`
- `Boolean RemoveLinkedAssignment(ReferenceObject linkedObject)`
- `ReferenceObject AddLinkedMaterial(ReferenceObject newLinkedObject)`
- `Boolean RemoveLinkedMaterial(ReferenceObject linkedObject)`

### `TasksType` (Namespace: `TFlex.DOCs.Model.References.Tasks`)
**Свойства:** Classes: TasksTypes, IsTask: Boolean, IsTheme: Boolean

### `TasksTypes` (Namespace: `TFlex.DOCs.Model.References.Tasks`)
**Свойства:** Task: TasksType, Theme: TasksType

### `ProgressParameter` (Namespace: `TFlex.DOCs.Model.References.Tasks.Parameters`)
**Свойства:** IsReadOnly: Boolean, Value: Double

### `ProductTechnicalRequirementsReference` (Namespace: `TFlex.DOCs.Model.References.TechnicalRequirements.ProductTechnicalRequirements`)
**Свойства:** Classes: ProductTechnicalRequirementsTypes

### `ProductTechnicalRequirementsReferenceObject` (Namespace: `TFlex.DOCs.Model.References.TechnicalRequirements.ProductTechnicalRequirements`)
**Свойства:** Class: ProductTechnicalRequirementsType, Name: StringParameter, RequirementText: StringParameter, TechnicalCADData: ByteArrayParameter, Number: Int32Parameter, OriginalTTLink: ReferenceObject
**Методы:**
- `Void SetOriginalTTObject(ReferenceObject newLinkedObject)`

### `ProductTechnicalRequirementsType` (Namespace: `TFlex.DOCs.Model.References.TechnicalRequirements.ProductTechnicalRequirements`)
**Свойства:** Classes: ProductTechnicalRequirementsTypes, IsProductTechnicalRequirementsReferenceObject: Boolean

### `ProductTechnicalRequirementsTypes` (Namespace: `TFlex.DOCs.Model.References.TechnicalRequirements.ProductTechnicalRequirements`)
**Свойства:** ProductTechnicalRequirementsReferenceObject: ProductTechnicalRequirementsType

### `RequirementsDictionaryReferenceObject` (Namespace: `TFlex.DOCs.Model.References.TechnicalRequirementsDictionary`)
**Свойства:** Class: TechnicalRequirementsDictionaryType, Name: StringParameter, Text: StringParameter, Number: Int32Parameter, TechnicalCADData: ByteArrayParameter

### `TechnicalRequirementsDictionaryReference` (Namespace: `TFlex.DOCs.Model.References.TechnicalRequirementsDictionary`)
**Свойства:** Classes: TechnicalRequirementsDictionaryTypes

### `TechnicalRequirementsDictionaryType` (Namespace: `TFlex.DOCs.Model.References.TechnicalRequirementsDictionary`)
**Свойства:** Classes: TechnicalRequirementsDictionaryTypes, IsFolderRequirementsDictionaryReferenceObject: Boolean, IsTechnicalRequirementsDictionaryReferenceObject: Boolean

### `TechnicalRequirementsDictionaryTypes` (Namespace: `TFlex.DOCs.Model.References.TechnicalRequirementsDictionary`)
**Свойства:** FolderRequirementsDictionaryReferenceObject: TechnicalRequirementsDictionaryType, TechnicalRequirementsDictionaryReferenceObject: TechnicalRequirementsDictionaryType

### `ToleranceReference` (Namespace: `TFlex.DOCs.Model.References.Tolerance`)
**Методы:**
- `List`1 GetToleranceLetters()`
- `List`1 GetToleranceDigits()`
- `List`1 GetToleranceDigitsInUse(Double dimension, String letter)`
- `List`1 GetTolerance(Double dimension, String letter, Int32 digit)`

### `Unit` (Namespace: `TFlex.DOCs.Model.References.Units`)
**Свойства:** Class: UnitType, Name: StringParameter, ShortName: StringParameter, RCUMCode: Int32Parameter, Description: StringParameter, InternationalName: StringParameter, IsBase: Boolean, Coefficient: DoubleParameter, Offset: SingleParameter
**Методы:**
- `Double Convert(Double value, Unit unit)`

### `UnitReference` (Namespace: `TFlex.DOCs.Model.References.Units`)
**Свойства:** Instance: UnitReference, Classes: UnitTypes
**Методы:**
- `Double Convert(Double value, Unit fromUnit, Unit toUnit)`

### `UnitType` (Namespace: `TFlex.DOCs.Model.References.Units`)
**Свойства:** Classes: UnitTypes
**Методы:**
- `List`1 GetUnits()` [has Async]
- `Unit GetBaseUnit()` [has Async]
- `Void SetBaseUnit(Unit unit)` [has Async]

### `UnitTypes` (Namespace: `TFlex.DOCs.Model.References.Units`)
**Свойства:** Instance: UnitTypes, Length: UnitType, Area: UnitType, Volume: UnitType, Weight: UnitType, Temperature: UnitType, Angle: UnitType, Velocity: UnitType, AngularVelocity: UnitType, Density: UnitType, Pressure: UnitType, Energy: UnitType, Power: UnitType, Voltage: UnitType, Amperage: UnitType, Resistance: UnitType, Electrocapacity: UnitType, Frequency: UnitType, Time: UnitType

### `UserDialogsReference` (Namespace: `TFlex.DOCs.Model.References.UserDialogs`)
**Свойства:** Classes: UserDialogClassTree
**Методы:**
- `UserDialogObject Find(User user, String dialogTypeName) (+1)` [has Async]

### `UserDialogType` (Namespace: `TFlex.DOCs.Model.References.UserDialogs`)
**Свойства:** IsUserDialog: Boolean, IsUserPanel: Boolean

### `ActiveDirectoryUsersGroup` (Namespace: `TFlex.DOCs.Model.References.Users`)
**Методы:**
- `Boolean CanCreateChildObject(ClassObject childClass)`
- `Boolean UpdateUsers(ICollection`1 updateList, CallbackSolutions callback)` [has Async]

### `PasswordParameter` (Namespace: `TFlex.DOCs.Model.References.Users`)
**Свойства:** IsReadOnly: Boolean, IsEmpty: Boolean
**Методы:**
- `String GetString()`
- `TypeCode GetTypeCode()`

### `PasswordPolicySettings` (Namespace: `TFlex.DOCs.Model.References.Users`)
**Свойства:** Connection: ServerConnection, Interface: String, ParameterGroupId: Int32, SupportsViews: Boolean, SharingType: SettingsSharingType
**Методы:**
- `Void ValidateNewPassword(String oldPassword, String newPassword)`
- `Void Save()` [has Async]

### `PasswordPolicySettingsData` (Namespace: `TFlex.DOCs.Model.References.Users`)
**Свойства:** Duration: Nullable`1, MinimumLength: Int32, NecessaryUseDigits: Boolean, NecessaryUseCapitalLetters: Boolean, NecessaryUseSpecialSymbols: Boolean, MinimalCountOfChangedSymbols: Int32

### `ProductionUnit` (Namespace: `TFlex.DOCs.Model.References.Users`)
**Свойства:** Number: StringParameter, Code: StringParameter, ShortName: StringParameter, FunctionType: Int32Parameter, AreaFixingType: Int32Parameter, PurposeType: Int32Parameter, EquipmentLink: OneToManyLink, LinkedEquipmentReference: Reference
**Методы:**
- `Boolean CanChangeLink(LinkInfo link, ReferenceObject addObject, ReferenceObject removeObject)`

### `User` (Namespace: `TFlex.DOCs.Model.References.Users`)
**Свойства:** FirstName: StringParameter, LastName: StringParameter, Patronymic: StringParameter, ShortName: StringParameter, Login: StringParameter, Password: PasswordParameter, MasterPassword: PasswordParameter, ForbidChangePassword: Boolean, PasswordExpirationDate: DateTimeParameter, Sex: Int32Parameter, Birthday: DateTimeParameter, Email: StringParameter, MailSendType: ByteParameter, MailSendMode: MailSendMode, BusinessPhone: StringParameter, InternalPhone: StringParameter, MobilePhone: StringParameter, HomePhone: StringParameter, Fax: StringParameter, BlockingDate: DateTimeParameter, BlockingReason: StringParameter, Sid: StringParameter, SidOnly: BooleanParameter, Photo: ImageParameter, FirstWorkDay: DateTimeParameter, MailSettings: StringParameter, Signature: ImageParameter, SharedWorkspace: BooleanParameter, CachingFileServer: CachingFileServerObject, IsCurrent: Boolean, IsSystem: Boolean
**Методы:**
- `Boolean ChangePassword(String oldPassword, String newPassword, Boolean changeOnLogin)`
- `Boolean SetMasterPassword(String masterPassword)`
- `Boolean IsMasterPasswordExists()` [has Async]
- `Boolean SetPasswordFromMasterPassword()` [has Async]
- `CachingFileServerObject GetCurrentCachingFileServer()` [has Async]
- `Void RestoreSettings(String application)`
- `List`1 GetAllInternalUsers(Boolean reloadChildren)`
- `List`1 GetAllInternalUsersAndGroups(Boolean reloadChildren)`
- `User LockAccount(Action`1 actionOnDisconnectedUser) (+1)`
- `User UnlockAccount()`

### `UserReference` (Namespace: `TFlex.DOCs.Model.References.Users`)
**Свойства:** Instance: UserReference, Classes: UserTypes
**Методы:**
- `List`1 GetAllUsersGroup()` [has Async]
- `List`1 GetAllUsers()` [has Async]
- `IEnumerable`1 FindUsersAndGroups(IEnumerable`1 objects)`
- `Boolean CanDeleteHierarchyLink(ComplexHierarchyLink link)`
- `User FindUser(String fullName)` [has Async]

### `UserReferenceObject` (Namespace: `TFlex.DOCs.Model.References.Users`)
**Свойства:** Class: UserType, IsUser: Boolean, IsGroup: Boolean, FullName: StringParameter, Description: StringParameter, WorkTimeManager: WorkTimeManager, CurrentWorkTimeElements: IEnumerable`1, Calendar: CalendarReferenceObject, CalendarChanges: IEnumerable`1
**Методы:**
- `ReferenceObject BeginChanges(ClassObject newClass)` [has Async]
- `List`1 GetAllInternalUsers(Boolean reloadChildren) (+1)` [has Async]
- `List`1 GetAllInternalUsersAndGroups(Boolean reloadChildren) (+1)`
- `Void UpdateWorkTimeElements()`
- `List`1 GetWorkTimeElements()`
- `Void ClearWorkTimeElements()`

### `UsersGroup` (Namespace: `TFlex.DOCs.Model.References.Users`)
**Свойства:** PostAddress: StringParameter, ContactPhone: StringParameter, Email: StringParameter, Sid: StringParameter, CachingFileServer: CachingFileServerObject

### `UserType` (Namespace: `TFlex.DOCs.Model.References.Users`)
**Свойства:** Classes: UserTypes, IsGroup: Boolean, IsActiveDirectoryGroup: Boolean, IsSubdivision: Boolean, IsEnterprise: Boolean, IsDivision: Boolean, IsUser: Boolean, IsEmployee: Boolean, IsDisconnectedUser: Boolean, IsOutsideUser: Boolean, IsAdministrator: Boolean, IsPost: Boolean, IsProductionUnit: Boolean, IsShop: Boolean, IsArea: Boolean, IsWorkplace: Boolean, IsWorkcenter: Boolean, IsWorkflowRole: Boolean, IsRole: Boolean

### `UserTypes` (Namespace: `TFlex.DOCs.Model.References.Users`)
**Свойства:** GroupBaseType: UserType, ActiveDirectoryGroup: UserType, SubdivisionType: UserType, EnterpriseType: UserType, DivisionType: UserType, UserBaseType: UserType, EmployerType: UserType, DisconnectedUserType: UserType, OutsideUserType: UserType, AdministratorType: UserType, PostBaseType: UserType, ProductionUnitType: UserType, ShopType: UserType, AreaType: UserType, WorkplaceType: UserType, WorkflowRoleType: UserType

### `WorkflowRole` (Namespace: `TFlex.DOCs.Model.References.Users`)
**Методы:**
- `List`1 GetAllInternalUsers(Boolean reloadChildren)`
- `List`1 GetAllInternalUsersAndGroups(Boolean reloadChildren)`

### `WorkingAreaDesktopObject` (Namespace: `TFlex.DOCs.Model.References.WorkingAreas`)
**Свойства:** UserPageData: ByteArrayParameter
**Методы:**
- `Void DeleteWithAllObjects()`

### `WorkingAreaFolder` (Namespace: `TFlex.DOCs.Model.References.WorkingAreas`)
**Свойства:** IsFolderTypeTreeChildren: Boolean
**Методы:**
- `WorkingAreaShortcut CreateShortcutToReference(Reference reference)`
- `WorkingAreaShortcut CreateShortcutToWindow(Int32 windowType, String windowName, IconImage windowIcon)`
- `WorkingAreaShortcut CreateShortcutToReferenceObject(ReferenceObject referenceObject)`
- `WorkingAreaShortcut CreateShortcutToMacros(Macro macros, String macrosName)`
- `WorkingAreaShortcut CreateShortcutToSearchQuery(SearchQueryObject searchQuery)`
- `WorkingAreaShortcut CreateShortcutToWorkPage(WorkingPage workingPage)`

### `WorkingAreaReference` (Namespace: `TFlex.DOCs.Model.References.WorkingAreas`)
**Свойства:** Instance: WorkingAreaReference, PrivateFolder: WorkingAreaFolder, CommonFolder: WorkingAreaFolder, Classes: WorkingAreaTypes

### `WorkingAreaReferenceObject` (Namespace: `TFlex.DOCs.Model.References.WorkingAreas`)
**Свойства:** Class: WorkingAreaType, Name: StringParameter, ObjectType: Int32Parameter, Icon: IconParameter, WorkingAreaIcon: IconImage

### `WorkingAreaShortcut` (Namespace: `TFlex.DOCs.Model.References.WorkingAreas`)
**Свойства:** ReferenceGuid: GuidParameter, MacrosGuid: GuidParameter, WorkPageGuid: GuidParameter, MacrosName: StringParameter, FilterParameter: StringParameter, LinkedReference: ReferenceInfo, LinkedObject: ReferenceObject, WorkingAreaIcon: IconImage, Filter: Filter, ToReference: Boolean, ToReferenceObject: Boolean, ToMacros: Boolean, ToWorkPage: Boolean, ToSearchQuery: Boolean

### `WorkingAreaType` (Namespace: `TFlex.DOCs.Model.References.WorkingAreas`)
**Свойства:** Classes: WorkingAreaTypes, IsWorkingAreaFolder: Boolean, IsWorkingAreaShortcut: Boolean, IsWorkingAreaDesktop: Boolean

### `WorkingAreaTypes` (Namespace: `TFlex.DOCs.Model.References.WorkingAreas`)
**Методы:**
- `WorkingAreaType GetWorkingAreaFolderClass()`
- `WorkingAreaType GetWorkingAreaShortcutClass()`
- `WorkingAreaType GetWorkingAreaDesktopClass()`

### `WorkTimeReferenceObjectChange` (Namespace: `TFlex.DOCs.Model.References.WorkTime`)
**Свойства:** StartDate: DateTime, WorkTimeIntervals: IEnumerable`1, PriorityValue: Int32
**Методы:**
- `Boolean ContainsDate(DateTime date)`

### `BaseVisualSetting` (Namespace: `TFlex.DOCs.Model.ScheduleChart.Styles`)
**Свойства:** Color: Int32, IsVisible: Boolean, Size: Double, CanChangeIsVisible: Boolean, CanChangeColor: Boolean, CanChangeSize: Boolean

### `ChartSettings` (Namespace: `TFlex.DOCs.Model.ScheduleChart.Styles`)
**Свойства:** RowEvenColor: Int32, RowOddColor: Int32, RowReadOnlyColor: Nullable`1, SelectedRowHighlightColor: Int32, ColumnHolidayColor: Int32, ColumnCurrentColor: Int32, LineColor: Int32, LineThickness: Double, RowHeightStep: Double, ElementRelativeRowHeight: Double, HeaderColor: Int32, HeaderCurrentColor: Int32, HeaderFontSettings: FontSetting, SelectedElementBorderColor: Int32, FocusedElementBorderColor: Int32

### `FontSetting` (Namespace: `TFlex.DOCs.Model.ScheduleChart.Styles`)
**Свойства:** FontFamily: String, Bold: Boolean, Italic: Boolean, FontSize: Double, FontColor: Int32
**Методы:**
- `FontSetting CreateDefault()`

### `LabelSetting` (Namespace: `TFlex.DOCs.Model.ScheduleChart.Styles`)
**Свойства:** Figure: Int32

### `TextSetting` (Namespace: `TFlex.DOCs.Model.ScheduleChart.Styles`)
**Свойства:** Indent: Double, IsVisible: Boolean, SourceType: TextSourceType, Parameter: String, Formula: FormulaMacro, UniversalPath: String, Format: String, Font: FontSetting

### `TextToColorConverter` (Namespace: `TFlex.DOCs.Model.ScheduleChart.Styles`)
**Методы:**
- `Void TrySetColor(String text, Action`1 setter, Nullable`1 defaultValue) (+1)`
- `String GetText(Nullable`1 color) (+1)`

### `VisualSetting` (Namespace: `TFlex.DOCs.Model.ScheduleChart.Styles`)
**Свойства:** Radius: Double, BorderColor: Int32, BorderThickness: Double, UnderlaymentColor: Nullable`1, CanChangeBorderParams: Boolean, CanChangeRadius: Boolean

### `CalendarCategory` (Namespace: `TFlex.DOCs.Model.Scheduler`)
**Свойства:** Manager: CalendarCategoryManager, Id: Guid, IsSystem: Boolean, IsVisible: Boolean, IsPublic: Boolean, Text: String, HasBrokenSettings: Boolean, SettingsCount: Int32, Name: String, IsChanged: Boolean, IsDeleted: Boolean, IsAdded: Boolean
**Методы:**
- `ReadOnlyCollection`1 GetSettings(Boolean getDeleted)`
- `ReadOnlyCollection`1 GetBrokenSettings()`
- `Void UpdateSetting(CalendarCategorySettingBase calendarCategorySetting)`
- `Boolean DeleteSetting(CalendarCategorySettingBase set)`
- `Boolean Add(CalendarCategorySettingBase set)`
- `Void ClearSettings()`

### `CalendarCategorySettingBase` (Namespace: `TFlex.DOCs.Model.Scheduler`)
**Свойства:** Id: Guid, Connection: ServerConnection, IsSystem: Boolean, Category: CalendarCategory, Editable: Boolean, AppointmentColor: Int32, AppointmentLabel: Int32, AppointmentStatus: Int32, AllDay: Nullable`1, Duration: Nullable`1, IsDeleted: Boolean, IsAdded: Boolean
**Методы:**
- `String GetName()`
- `IEnumerable`1 GetAppointments(Nullable`1 start, Nullable`1 end, CancellationToken cancellationToken)`
- `Void ResetCache()`
- `CalendarSettingDescription CreateDescription()`
- `Void Refresh()`

### `CalendarCategorySettingsWithParameters` (Namespace: `TFlex.DOCs.Model.Scheduler`)
**Свойства:** HeaderParameter: ICalendarCategorySettingParameter`1, CommentParameter: ICalendarCategorySettingParameter`1, StartTimeParameter: ICalendarCategorySettingParameter`1, EndTimeParameter: ICalendarCategorySettingParameter`1, AllDayParameter: ICalendarCategorySettingParameter`1, HeaderCalculatedParameter: CalculationInfo, DescriptionCalculatedParameter: CalculationInfo, StartTimeCalculatedParameter: CalculationInfo, EndTimeCalculatedParameter: CalculationInfo, AllDayCalculatedParameter: CalculationInfo
**Методы:**
- `Void FillParameters(ICalendarAppointment calendarAppointment, CancellationToken cancellationToken)`
- `Boolean GetAllDayParameterValue(ICalendarAppointment calendarAppointment)`
- `Boolean GetAllDayParameterValueWithSetting(ICalendarAppointment calendarAppointmentWithParameters)`
- `DateTime GetStartTimeParameterValue(ICalendarAppointment calendarAppointment)`
- `DateTime GetEndTimeParameterValue(ICalendarAppointment calendarAppointment)`
- `String GetCommentParameterValue(ICalendarAppointment calendarAppointment)`
- `String GetHeaderParameterValue(ICalendarAppointment calendarAppointment)`
- `Void SetAllDayParameterValue(ICalendarAppointment calendarAppointment, Boolean value)`
- `Void SetStartTimeParameterValue(ICalendarAppointment calendarAppointment, DateTime value)`
- `Void SetEndTimeParameterValue(ICalendarAppointment calendarAppointment, DateTime value)`
- `CalendarSettingDescription CreateDescription()`

### `CalendarMailFolderSetting` (Namespace: `TFlex.DOCs.Model.Scheduler`)
**Свойства:** FolderGuid: Guid, AccountGuid: Guid, IsTaskFolder: Boolean, Filter: Filter
**Методы:**
- `String GetName()`
- `IconImage GetIcon()`
- `Void Refresh()`
- `Account GetAccount()`
- `Void ResetCache()`
- `CalendarSettingDescription CreateDescription()`

### `CalendarReferenceSetting` (Namespace: `TFlex.DOCs.Model.Scheduler`)
**Свойства:** Reference: Reference, ReferenceFilter: Filter, RootReferenceObject: ReferenceObject, CanCreateObjects: Boolean
**Методы:**
- `String GetName()`
- `CalendarSettingDescription CreateDescription()`
- `Void AddToLoadSettings(Reference reference)`
- `IEnumerable`1 GetAppointments(Reference reference, Nullable`1 start, Nullable`1 end, CancellationToken cancellationToken)`
- `IEnumerable`1 FillDataSourceByLoadedObjects(IEnumerable`1 referenceObjects, CancellationToken cancellationToken)`
- `Void ResetCache()`
- `Void Refresh()`

### `CalendarReminderSetting` (Namespace: `TFlex.DOCs.Model.Scheduler`)
**Методы:**
- `String GetName()`
- `String GetCommentParameterValue(ICalendarAppointment calendarAppointment)`
- `String GetHeaderParameterValue(ICalendarAppointment calendarAppointment)`
- `DateTime GetStartTimeParameterValue(ICalendarAppointment calendarAppointment)`
- `Void SetStartTimeParameterValue(ICalendarAppointment calendarAppointment, DateTime value)`
- `Void ResetCache()`
- `CalendarSettingDescription CreateDescription()`
- `Void Refresh()`

### `BrokenSetting` (Namespace: `TFlex.DOCs.Model.Scheduler.BrokenSettings`)
**Свойства:** Description: CalendarSettingDescription, ErrorMessage: String

### `CalendarAppointmentBase` (Namespace: `TFlex.DOCs.Model.Scheduler.Entities`)
**Свойства:** Header: String, Comment: String, Start: DateTime, End: DateTime, AllDay: Nullable`1, Key: Int32, LabelId: Int32, Status: Int32, Setting: CalendarCategorySettingBase, CanChangeTimeInterval: Boolean
**Методы:**
- `MacroContext GetMacroContext()`
- `Void SetMacroContext(MacroContext context)`
- `Object GetCoreObject()`
- `Void Release()`

### `CalendarAppointmentWithParameters` (Namespace: `TFlex.DOCs.Model.Scheduler.Entities`)
**Свойства:** Setting: CalendarCategorySettingsWithParameters
**Методы:**
- `Boolean GetAllDayParameterValueWithSetting()`
- `Boolean GetAllDayParameterValue()`
- `DateTime GetStartTimeParameterValue()`
- `DateTime GetEndTimeParameterValue()`
- `String GetCommentParameterValue()`
- `String GetHeaderParameterValue()`
- `Void SetAllDayParameterValue(Boolean value)`
- `Void SetStartTimeParameterValue(DateTime newIntervalStartTime)`
- `Void SetEndTimeParameterValue(DateTime newIntervalEndTime)`

### `ICalendarAppointment` (Namespace: `TFlex.DOCs.Model.Scheduler.Entities`)
**Свойства:** Header: String, Comment: String, Start: DateTime, End: DateTime, AllDay: Nullable`1, Key: Int32, LabelId: Int32, Status: Int32, Setting: CalendarCategorySettingBase, CanChangeTimeInterval: Boolean
**Методы:**
- `Void SetMacroContext(MacroContext context)`
- `MacroContext GetMacroContext()`
- `Object GetCoreObject()`
- `Void Release()`

### `MailAppointment` (Namespace: `TFlex.DOCs.Model.Scheduler.Entities`)
**Свойства:** IsTask: Boolean
**Методы:**
- `Object GetCoreObject()`
- `MacroContext GetMacroContext()`
- `Void Release()`

### `ReferenceAppointment` (Namespace: `TFlex.DOCs.Model.Scheduler.Entities`)
**Методы:**
- `Object GetCoreObject()`
- `Void Release()`
- `MacroContext GetMacroContext()`

### `ReminderAppointment` (Namespace: `TFlex.DOCs.Model.Scheduler.Entities`)
**Методы:**
- `Object GetCoreObject()`
- `Void Release()`
- `Boolean IsEqual(Reminder reminder)`

### `CalendarCategoryFactory` (Namespace: `TFlex.DOCs.Model.Scheduler.Logic`)
**Методы:**
- `CalendarCategory Create(CalendarCategoryDescription calendarCategoryDescription, CalendarCategoryManager manager, ICalculateParameterValue parameterValueCalculator)`

### `CalendarCategoryManager` (Namespace: `TFlex.DOCs.Model.Scheduler.Logic`)
**Свойства:** IsPublicChanged: Boolean, Connection: ServerConnection, SettingsFactory: CalendarSettingsFactory
**Методы:**
- `Boolean CheckBrokenCategories()`
- `ReadOnlyCollection`1 GetCategories()`
- `ReadOnlyCollection`1 GetBrokenCategories()`
- `ReadOnlyCollection`1 GetReferenceSettings()`
- `ReadOnlyCollection`1 GetVisibleReferenceSettings()`
- `Void InitializeCategories(String userContext)`
- `Void SaveCategories(String userContext)`
- `Boolean Add(CalendarCategory cat)`
- `Boolean Delete(CalendarCategory calendarCategory)`
- `Void DeleteSetting(CalendarCategorySettingBase setting)`
- `Void UpdateCategorySetting(Guid categoryGuid, CalendarCategorySettingBase setting)`
- `Void UpdateCategories(List`1 categories)`
- `IEnumerable`1 FillDataSource(Nullable`1 start, Nullable`1 end, CancellationToken cancellationToken)`
- `IEnumerable`1 FillDataSourceForSetting(CalendarCategorySettingBase setting, Nullable`1 start, Nullable`1 end, CancellationToken cancellationToken)`
- `Void SetObjectKey(ICalendarAppointment appointment)`
- `Void ClearCategories()`
- `CalendarCategorySettingBase GenerateSettingByType(SettingType settingType, CalendarCategory calendarCategory)`
- `Void BeginChanges()`
- `Void CancelChanges()`

### `CalendarSettingsFactory` (Namespace: `TFlex.DOCs.Model.Scheduler.Logic`)
**Методы:**
- `CalendarMailFolderSetting GenerateMailFolderSetting(CalendarCategory category)`
- `CalendarReferenceSetting GenerateReferenceSetting(CalendarCategory category)`
- `CalendarCategorySettingBase Create(CalendarSettingDescription description, ServerConnection connection, ICalculateParameterValue parameterValueCalculator)`

### `ICalculateParameterValue` (Namespace: `TFlex.DOCs.Model.Scheduler.Logic`)
**Методы:**
- `Object Calculate(CalculationInfo parameter, ICalendarAppointment categorySetting)`

### `ICalculateParameterValue`1` (Namespace: `TFlex.DOCs.Model.Scheduler.Logic`)
**Методы:**
- `T Calculate(CalculationInfo parameter)`

### `ICalendarCategorySettingParameter`1` (Namespace: `TFlex.DOCs.Model.Scheduler.Logic`)
**Методы:**
- `T GetValue(ICalendarAppointment calendarAppointment)`
- `Void SetValue(ICalendarAppointment calendarAppointment, Object value)`

### `IMailFolder` (Namespace: `TFlex.DOCs.Model.Scheduler.Logic`)
**Свойства:** Guid: Guid
**Методы:**
- `Int32 GetId()`
- `IEnumerable`1 GetMailItems(Filter filter)`
- `String GetFolderName()`
- `MailItemFolder GetMailItemFolder()`
- `IconImage GetIcon()`

### `MailSettingFolder` (Namespace: `TFlex.DOCs.Model.Scheduler.Logic`)
**Свойства:** Guid: Guid
**Методы:**
- `Int32 GetId()`
- `String GetFolderName()`
- `IconImage GetIcon()`
- `MailItemFolder GetMailItemFolder()`
- `IEnumerable`1 GetMailItems(Filter filter)`

### `TaskSettingFolder` (Namespace: `TFlex.DOCs.Model.Scheduler.Logic`)
**Свойства:** Guid: Guid, IsVirtual: Boolean
**Методы:**
- `Int32 GetId()`
- `IEnumerable`1 GetMailItems(Filter filter)`
- `String GetFolderName()`
- `MailItemFolder GetMailItemFolder()`
- `IconImage GetIcon()`

### `TimeIntervalManager` (Namespace: `TFlex.DOCs.Model.Scheduler.Logic`)
**Свойства:** Intervals: IReadOnlyCollection`1
**Методы:**
- `Boolean IsExists(TimeInterval newInterval)`
- `IEnumerable`1 Merge(TimeInterval newInterval)`

### `CalendarCategorySettingMailParameter`1` (Namespace: `TFlex.DOCs.Model.Scheduler.Logic.CalendarCategoryParameters`)
**Свойства:** MailField: MailField
**Методы:**
- `T GetValue(ICalendarAppointment calendarAppointment)`
- `Void SetValue(ICalendarAppointment calendarAppointment, Object value)`

### `CalendarCategorySettingReferenceParameter`1` (Namespace: `TFlex.DOCs.Model.Scheduler.Logic.CalendarCategoryParameters`)
**Свойства:** ReferencePath: ReferencePath
**Методы:**
- `T GetValue(ICalendarAppointment calendarAppointment)`
- `Void SetValue(ICalendarAppointment calendarAppointment, Object value)`

### `CalendarCategorySettingParameterExt` (Namespace: `TFlex.DOCs.Model.Scheduler.Logic.CalendarCategoryParameters.Extensions`)
**Методы:**
- `ReferencePath AsReferencePath(ICalendarCategorySettingParameter`1 parameter)`
- `MailField AsMailField(ICalendarCategorySettingParameter`1 parameter)`

### `CalculationInfo` (Namespace: `TFlex.DOCs.Model.Scheduler.Xml`)
**Свойства:** CalculationType: Int32, Path: String, MacrosGuid: String, MacrosMethod: String, Formula: String, LinkGuid: String, LinkFilter: String, LinkParameter: String, ParameterSet: String, Splitter: String, AdditionalCalculatorId: Guid, AdditionalCalculatorSettings: String
**Методы:**
- `XmlSchema GetSchema()`
- `Void ReadXml(XmlReader reader)`
- `Void WriteXml(XmlWriter writer)`

### `ComparisonOperator` (Namespace: `TFlex.DOCs.Model.Search`)
**Свойства:** Type: ComparisonOperatorType, SupportsSecondOperand: Boolean, RequireValueList: Boolean
**Методы:**
- `ComparisonOperator GetOperator(ComparisonOperatorType type)`
- `List`1 GetAllOperators()`
- `Boolean Compare(Object firstOperand, Object secondOperand)`

### `CustomComparisonOperator` (Namespace: `TFlex.DOCs.Model.Search`)
**Свойства:** Type: ComparisonOperatorType

### `Filter` (Namespace: `TFlex.DOCs.Model.Search`)
**Свойства:** Connection: ServerConnection, Terms: TermGroup, Variables: VariableCollection, MainReference: ReferenceInfo, MasterGroup: ParameterGroup, SourceReference: ReferenceInfo, SerializationMode: ObjectSerializationMode, ValuesTable: DynamicDataTable
**Методы:**
- `Void Validate(MacroContext formulaContext)`
- `Boolean IsValid(MacroContext formulaContext)`
- `Boolean Match(Object obj, MacroContext formulaContext)`
- `ParameterInfoCollection GetParameters()`
- `IEnumerable`1 GetReferencePaths()`
- `Filter Parse(String str, ParameterGroup masterGroup)`
- `Boolean TryParse(String str, ParameterGroup masterGroup, Filter& filter)`
- `LoadOptionsParameters GetLoadOptions(MacroContext formulaContext)`
- `ConfigurationSettings GetConfigurationSettings(MacroContext formulaContext)`
- `String Serialize()`
- `Filter Deserialize(String xml, ServerConnection connection)`
- `Void ReadXml(XmlReader reader)`
- `Void WriteXml(XmlWriter writer)`
- `Filter Merge(Filter firstFilter, Filter secondFilter, LogicalOperator logicalOperator) (+2)`
- `Void MergeVariables(Filter otherFilter)`
- `IReadOnlyCollection`1 GetSearchRulesOfElseGroup(Int32 elseGroupOrder) (+1)`
- `IReadOnlyCollection`1 GetSearchRules(TermGroupItem termGroupItem)`
- `IReadOnlyCollection`1 GetAllSearchRules(TermGroupItem termGroupItem)`
- `Void AddSearchRule(TermGroupItem termGroupItem, SearchRule searchRule)`
- `Void AddSearchRules(TermGroupItem termGroupItem, IEnumerable`1 searchRules)`
- `Void RemoveSearchRule(SearchRule searchRule)`
- `Void ChangeSearchRuleOwner(TermGroupItem termGroupItem, SearchRule searchRule)`
- `Void ChangeSearchRulesOwner(TermGroupItem termGroupItem, IEnumerable`1 searchRules)`

### `FilterData` (Namespace: `TFlex.DOCs.Model.Search`)
**Свойства:** Connection: ServerConnection, LinkGroupGuid: Guid, Name: String, IsCustom: Boolean, CustomFilter: Filter, FilterObject: ReferenceFilterObject, Filter: Filter
**Методы:**
- `XmlSchema GetSchema()`
- `Void ReadXml(XmlReader reader)`
- `Void WriteXml(XmlWriter writer)`

### `FilterTerm` (Namespace: `TFlex.DOCs.Model.Search`)
**Свойства:** ParameterName: String, Value: Filter, Operator: ComparisonOperator
**Методы:**
- `Void Clear()`

### `ReferenceFilter` (Namespace: `TFlex.DOCs.Model.Search`)
**Свойства:** Id: Int32, Guid: Guid, Reference: ReferenceInfo, Name: String, IsPrivate: Boolean, IsAdded: Boolean, IsModified: Boolean
**Методы:**
- `List`1 GetFilters(ReferenceInfo reference)`
- `Filter GetFilter()`
- `Void SetFilter(Filter filter)`
- `Boolean Save()`
- `Boolean Delete()`
- `Int32 CompareTo(ReferenceFilter other)`

### `ReferenceObjectTerm` (Namespace: `TFlex.DOCs.Model.Search`)
**Свойства:** Path: ReferencePath, ParameterName: String, Operator: ComparisonOperator, AsReferenceObjectTerm: ReferenceObjectTerm, SkipUsingServerTermValue: Boolean
**Методы:**
- `Void Clear()`
- `Void ReplaceVariablesByValues()`
- `IReadOnlyCollection`1 GetAvailableOperators()`

### `ReferenceWithFilterObject` (Namespace: `TFlex.DOCs.Model.Search`)
**Свойства:** ReferenceInfo: ReferenceInfo, ReferenceGuid: Guid, Filter: Filter

### `SearchQuery` (Namespace: `TFlex.DOCs.Model.Search`)
**Свойства:** Id: Int32, Guid: Guid, Folder: SearchQueryFolder, Name: String, IsPrivate: Boolean, IsAdded: Boolean, IsModified: Boolean
**Методы:**
- `Filter GetFilter()`
- `Void SetFilter(Filter filter)`
- `ReferencePathCollection GetOutput()`
- `Void SetOutput(ReferencePathCollection output)`
- `Boolean Save()`
- `Boolean Delete()`
- `Int32 CompareTo(SearchQuery other)`

### `SearchQueryFolder` (Namespace: `TFlex.DOCs.Model.Search`)
**Свойства:** Connection: ServerConnection, Id: Int32, Guid: Guid, IsPrivate: Boolean, ParentFolder: SearchQueryFolder, Folders: ReadOnlyCollection`1, Queries: ReadOnlyCollection`1, Name: String, IsAdded: Boolean, IsModified: Boolean
**Методы:**
- `List`1 GetRootFolders(ServerConnection connection)`
- `Boolean Save()`
- `Boolean Delete()`

### `SourceReferenceObjectTerm` (Namespace: `TFlex.DOCs.Model.Search`)
**Свойства:** Path: ReferencePath, SourcePath: ReferencePath, Coefficient: Double, CoefficientVariableName: String, CoefficientVariable: Variable`1, Factor: Double, FactorVariableName: String, FactorVariable: Variable`1, IsNumberSource: Boolean, ParameterName: String
**Методы:**
- `Void Clear()`
- `Boolean SupportsOperator(ComparisonOperator operator)`
- `Void ReplaceVariablesByValues()`

### `Term` (Namespace: `TFlex.DOCs.Model.Search`)
**Свойства:** ParameterName: String, Operator: ComparisonOperator, AllQuantifier: Boolean, CaseInsensitive: Nullable`1, Value: Object, SkipUsingServerTermValue: Boolean, AsTerm: Term
**Методы:**
- `Void Clear()`
- `Boolean Match(Object obj, MacroContext formulaContext)`

### `TermGroup` (Namespace: `TFlex.DOCs.Model.Search`)
**Свойства:** Filter: Filter, IsRoot: Boolean, AsGroup: TermGroup, Item: TermGroupItem, Count: Int32, IsReadOnly: Boolean
**Методы:**
- `ReferenceObjectTerm AddTerm(LogicalOperator lo, ParameterInfo parameter, ComparisonOperator op, Object value, ParameterGroup[] groups) (+9)`
- `TermGroup AddGroup(LogicalOperator logicalOperator, TermGroupItemType termGroupItemType)`
- `TermGroup GroupTerms(IEnumerable`1 terms)`
- `List`1 Ungroup()`
- `List`1 RemoveEmptyGroups(Boolean recursive)`
- `List`1 Normalize()`
- `List`1 RemoveErrorItems(Boolean recursive)`
- `Boolean Match(Object obj, MacroContext formulaContext)`
- `String GetText()`
- `Int32 IndexOf(TermGroupItem item)`
- `Void Insert(Int32 index, TermGroupItem item)`
- `Void Exchange(TermGroupItem firstItem, TermGroupItem secondItem, Boolean exchangeLogicalOperator)`
- `Void RemoveAt(Int32 index)`
- `Void Add(TermGroupItem item)`
- `Void Clear()`
- `Boolean Contains(TermGroupItem item)`
- `Void CopyTo(TermGroupItem[] array, Int32 arrayIndex)`
- `Boolean Remove(TermGroupItem item)`
- `IEnumerator`1 GetEnumerator()`
- `Void ReplaceVariablesByValues()`

### `TermGroupItem` (Namespace: `TFlex.DOCs.Model.Search`)
**Свойства:** IsGroup: Boolean, IsTerm: Boolean, IsReferenceObjectTerm: Boolean, ItemType: TermGroupItemType, AsGroup: TermGroup, AsTerm: Term, AsReferenceObjectTerm: ReferenceObjectTerm, LogicalOperator: LogicalOperator, Not: Boolean, Owner: TermGroup, IsError: Boolean
**Методы:**
- `Void ReplaceVariablesByValues()`
- `Boolean Match(Object obj, MacroContext formulaContext)`
- `Boolean IsOwnerOfSearchRule(SearchRule searchRule, Boolean recursive)`

### `FilterExtensions` (Namespace: `TFlex.DOCs.Model.Search.Extensions`)
**Методы:**
- `Filter ChangeBasePath(Filter filter, ReferencePath basePath)`
- `Filter MergeRepetitiveHierarchy(Filter filter)`
- `Filter AddAnalyzerToFormulaTerms(Filter filter)`
- `Filter MergeWithSearchRules(Filter first, Filter second)`
- `ValueTask`1 RunAndSetSpecialValues(Filter filter, MacroContext context, CancellationToken token)`
- `Filter CloneWithFormulaOptimization(Filter filter)`
- `Filter MassMerge(ICollection`1 filters, LogicalOperator logicalOperator)`
- `Filter OptimizeFilterStructure(Filter filter)`
- `Filter RemoveOnlyErrorTerms(Filter filter)`
- `ValueTuple`2 PartialMatchFilter(Filter filter, ReferenceObjectTerm termForRemove, Boolean isMatch)`
- `Nullable`1 MatchWithCustomPathGetter(Filter filter, DesktopObject value, Func`3 getter)`
- `Boolean CanMatchWithCustomPathGetter(Filter filter)`
- `Dictionary`2 RemoveSharedParts(Dictionary`2 sourceMap)`
- `Boolean IsEqualsByContent(Filter first, Filter second)`
- `Boolean IsNullOrEmpty(Filter filter)`
- `String VisualizeFilterRaw(Filter filter)`
- `String VisualizeFilter(Filter filter)`
- `Void SetDataTableValues(Filter filter, IReadOnlyCollection`1 parameterInfos, IReadOnlyCollection`1 values)`
- `List`1 GetTerms(Filter filter)`

### `IReferenceObjectMatcher` (Namespace: `TFlex.DOCs.Model.Search.Extensions`)
**Методы:**
- `Boolean Match(ReferenceObjectTerm term, Object obj, MacroContext formulaContext)`

### `FilterFormulaDetector` (Namespace: `TFlex.DOCs.Model.Search.Helpers`)
**Методы:**
- `Boolean ContainsFormula(Filter filter)`

### `AllChildObjectsPathItem` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** Icon: IconImage, Name: String, Type: PathItemType, SupportSearchType: SupportSearchTypes
**Методы:**
- `Boolean IsOneToMany()`

### `AllParentObjectsPathItem` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** Icon: IconImage, Name: String, Type: PathItemType, SupportSearchType: SupportSearchTypes
**Методы:**
- `Boolean IsOneToMany()`

### `ApplicabilityPathItem` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** Name: String, Type: PathItemType, SupportSearchType: SupportSearchTypes

### `AuthorPathItem` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** Name: String, Type: PathItemType, SystemType: SystemParameterType

### `BaseRepresentationObjectPathItem` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** DefaultName: String, Name: String, Type: PathItemType

### `CharacteristicsTypePathItem` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** Icon: IconImage, CharacteristicsType: CharacteristicClassReferenceObject, CharacteristicsDataType: CharacteristicsDataType, Name: String, Type: PathItemType

### `CharacteristicsTypesPathItem` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** Icon: IconImage, Name: String, SupportSearchType: SupportSearchTypes, Type: PathItemType

### `ChildHierarchyLinksPathItem` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** Type: PathItemType, Name: String
**Методы:**
- `Boolean IsOneToMany()`

### `ChildObjectsPathItem` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** Name: String, Type: PathItemType, SupportSearchType: SupportSearchTypes
**Методы:**
- `Boolean IsOneToMany()`

### `CredentialPathItem` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** Name: String, Type: PathItemType, SystemType: SystemParameterType

### `EditorPathItem` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** Name: String, Type: PathItemType, SystemType: SystemParameterType

### `EndProductPathItem` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** Name: String, Type: PathItemType

### `ExtendedGroupPathItem` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** Name: String, Type: PathItemType, Group: ParameterGroup, SupportSearchType: SupportSearchTypes

### `ExtendedParameterPathItem` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** Name: String, Parameter: ParameterInfo, Type: PathItemType

### `ExtendedParametersPathItem` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** Name: String, Type: PathItemType, Group: ParameterGroup, SupportSearchType: SupportSearchTypes

### `FileContentPathItem` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** Icon: IconImage, DefaultName: String, Name: String, Type: PathItemType, SupportSearchType: SupportSearchTypes

### `FilePreviewImagePathItem` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** Icon: IconImage, Name: String, Type: PathItemType, PageIndex: Int32, SupportSearchType: SupportSearchTypes

### `GroupPathItem` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** Group: ParameterGroup, Path: ReferencePath, Name: String, Type: PathItemType, Icon: IconImage, SupportSearchType: SupportSearchTypes
**Методы:**
- `Boolean IsOneToMany()`

### `HierarchyGroupPathItem` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** Name: String, SupportSearchType: SupportSearchTypes
**Методы:**
- `Boolean IsOneToMany()`

### `IndirectLinkPath` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** Connection: ServerConnection, CurrentItem: IndirectLinkPathItem
**Методы:**
- `Void AddItem(IndirectLinkPathItem item)`
- `Void RemoveItem(IndirectLinkPathItem item)`
- `String Serialize(ObjectSerializationMode mode, Boolean includeRootItem) (+1)`
- `Boolean SetNextItemAsSelectFromChilden(IndirectLinkPathItem item)`
- `Boolean RemoveItemAsSelectFromChilden(IndirectLinkPathItem item)`
- `IndirectLinkPath Deserialize(String xml, ServerConnection connection)`

### `IndirectLinkPathItem` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** IsReadOnly: Boolean, IsSelectFromChilden: Boolean, Name: String, Type: PathItemType, Path: IndirectLinkPath
**Методы:**
- `Filter GetFilter()`
- `Void SetFilter(Filter filter)`
- `String GetFromLinkText()`
- `String GetToLinkText()`
- `ParameterGroup GetFromGroup()`
- `ParameterGroup GetToGroup()`
- `Boolean GetCanSetNextItemAsSelectFromChilden()`

### `IndirectLinkPathItemXML` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** IsReadOnly: Boolean, IsSelectFromChilden: Boolean, Filter: String, ReferenceGuid: Guid, GroupGuid: Guid

### `LinkedObjectPathItem` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** Icon: IconImage, Name: String, Type: PathItemType, SupportSearchType: SupportSearchTypes

### `LinkPathItem` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** Type: PathItemType, UseLinkSeparator: Boolean, SupportSearchType: SupportSearchTypes, Swapped: Boolean
**Методы:**
- `Boolean IsOneToMany()`

### `MasterObjectPathItem` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** Name: String, Type: PathItemType, SupportSearchType: SupportSearchTypes
**Методы:**
- `Boolean IsOneToMany()`

### `MasterServerPathItem` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** Name: String, Type: PathItemType, SystemType: SystemParameterType

### `NamePathItem` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** Name: String, Type: PathItemType, Icon: IconImage, SupportSearchType: SupportSearchTypes

### `NomenclatureObjectPathItem` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** Name: String, Type: PathItemType, UseLinkSeparator: Boolean, SupportSearchType: SupportSearchTypes

### `ObjectInstancePathItem` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** Icon: IconImage, Name: String, Type: PathItemType, SupportSearchType: SupportSearchTypes
**Методы:**
- `Boolean IsOneToMany()`

### `ObjectRemarksPathItem` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** Name: String, Type: PathItemType, SupportSearchType: SupportSearchTypes

### `ObjectStageParameterPathItem` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** Icon: IconImage, Name: String, Parameter: ObjectStageParameter, SupportSearchType: SupportSearchTypes, Type: PathItemType

### `ObjectStagesPathItem` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** Icon: IconImage, Name: String, Group: ParameterGroup, SupportSearchType: SupportSearchTypes, Type: PathItemType
**Методы:**
- `Boolean IsOneToMany()`

### `OnBehalfOfPathItem` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** Name: String, Type: PathItemType, SystemType: SystemParameterType

### `OwnerPathItem` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** Name: String, Type: PathItemType, SystemType: SystemParameterType

### `ParameterPathItem` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** Parameter: ParameterInfo, Name: String, Type: PathItemType, Icon: IconImage
**Методы:**
- `Boolean SkipUsingServerTermValue(ComparisonOperator comparisonOperator)`

### `ParentHierarchyLinksPathItem` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** Type: PathItemType, Name: String
**Методы:**
- `Boolean IsOneToMany()`

### `ParentObjectPathItem` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** Name: String, Type: PathItemType, SupportSearchType: SupportSearchTypes
**Методы:**
- `Boolean IsOneToMany()`

### `PathCalculationSettings` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** Distinct: Boolean, WithStaticReference: Boolean

### `PathItem` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** Parent: PathItem, Items: ReadOnlyCollection`1, Operators: ReadOnlyCollection`1, Path: ReferencePath, Name: String, Type: PathItemType, Icon: IconImage, SupportsSearch: Boolean, SupportSearchType: SupportSearchTypes, UseLinkSeparator: Boolean
**Методы:**
- `Void ReloadItems()`
- `String GetUniqueId()`
- `Boolean IsOneToMany()`
- `Boolean SkipUsingServerTermValue(ComparisonOperator comparisonOperator)`
- `Boolean IsSame(PathItem other)`

### `ReferencePath` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** MasterGroup: ParameterGroup, CurrentItem: PathItem, Item: PathItem, Count: Int32
**Методы:**
- `Boolean AddGroup(ParameterGroup group, Boolean throwOnError) (+1)`
- `Boolean AddGroupAt(ParameterGroup group, Int32 index, Boolean throwOnError) (+1)`
- `Boolean AddSwappedLink(ParameterGroup linkGroup, Boolean throwOnError) (+1)`
- `Boolean AddChildObjects(Boolean throwOnError) (+1)`
- `Boolean AddAllChildObjects(Boolean throwOnError) (+1)`
- `Boolean AddObjectRemarks(Boolean throwOnError)`
- `Boolean AddRevisions(Boolean throwOnError)`
- `Boolean AddBaseRepresentationObject(Boolean throwOnError)`
- `Boolean AddStructureTypes(Boolean throwOnError)`
- `Boolean AddStructureType(Boolean throwOnError)`
- `Boolean AddApplicability(Boolean throwOnError)`
- `Boolean AddStartProduct(Boolean throwOnError)`
- `Boolean AddEndProduct(Boolean throwOnError)`
- `Boolean AddAuthor(Boolean throwOnError)`
- `Boolean AddEditor(Boolean throwOnError)`
- `Boolean AddOwner(Boolean throwOnError)`
- `Boolean AddOnBehalfOf(Boolean throwOnError)`
- `Boolean AddCredential(Boolean throwOnError)`
- `Boolean AddMasterServer(Boolean throwOnError)`
- `Boolean AddLoadOptions(Boolean throwOnError)`
- `Boolean AddLoadOptionsParameter(LoadOptionsParameter parameter, Boolean throwOnError)`
- `Boolean AddConfigurationGroup(Boolean throwOnError)`
- `Boolean AddConfigurationParameter(ConfigurationParameter parameter, Boolean throwOnError)`
- `Boolean AddParentComplexHierarchyLinks(Boolean throwOnError) (+1)`
- `Boolean AddChildrenComplexHierarchyLinks(Boolean throwOnError) (+1)`
- `Boolean AddAllParentObjects(Boolean throwOnError) (+1)`
- `Boolean AddParameter(ParameterInfo parameter, Boolean throwOnError) (+1)`
- `Boolean AddSignature(SignatureType type, Boolean throwOnError) (+2)`
- `Boolean AddSignatureParameter(SignatureParameter parameter, Boolean throwOnError)`
- `Boolean AddFilePreviewImage(Int32 pageIndex, Boolean throwOnError)`
- `Boolean AddFileContent(Boolean throwOnError)`
- `Boolean AddNomenclatureObject(Boolean throwOnError)`
- `Boolean AddLinkedObject(Boolean throwOnError)`
- `Boolean AddMasterObject(Boolean throwOnError)`
- `Boolean AddParentObject(Boolean throwOnError)`
- `Boolean AddRootObject(Boolean thrownOnError)`
- `Boolean AddObjectStages(Boolean throwOnError)`
- `Boolean AddObjectStageParameter(ObjectStageParameter parameter, Boolean throwOnError)`
- `Boolean AddObjectInstancePathItem(Boolean throwOnError)`
- `Boolean AddCharacteristics(Boolean throwOnError)`
- `Boolean AddNamePathItem(Boolean throwOnError)`
- `Void RemoveLast()`
- `Void RemoveRange(Int32 index, Int32 count)`
- `Void SetPathToItem(PathItem item)`
- `ReferencePath CreatePathToItem(PathItem item)`
- `ReferencePath GetSubpath(ParameterGroup group)`
- `ReferencePath GetRelativePath(ReferencePath initialPath, Boolean throwOnError)`
- `Boolean IsOneToMany()`
- `Boolean ContainsInLoadSettings(LoadSettings loadSettings)`
- `LoadSettings AddToLoadSettings(LoadSettings loadSettings)`
- `String Serialize(ObjectSerializationMode mode, Boolean includeRootItem)`
- `Boolean IsSame(ReferencePath other)`
- `Int32 IndexOf(PathItem item)`
- `Void Add(PathItem item)`
- `Boolean TryAdd(PathItem item)`
- `Void Clear()`
- `Boolean Contains(PathItem item)`
- `Void CopyTo(PathItem[] array, Int32 arrayIndex)`
- `IEnumerator`1 GetEnumerator()`
- `ReferencePath Parse(String str, ServerConnection connection) (+1)`
- `Boolean TryParse(String str, ServerConnection connection, ReferencePath& path) (+1)`
- `ReferencePath CreateToReferenceGroup(ParameterGroup group) (+1)`

### `ReferencePathCache` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** Connection: ServerConnection
**Методы:**
- `Boolean TryParse(String str, ParameterGroup masterGroup, ReferencePath& path) (+1)`
- `Boolean TryParseAny(String str, ParameterGroup masterGroup, ReferencePath& path) (+1)`
- `Void Reset()`

### `ReferencePathExtensions` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Методы:**
- `Boolean IsEmptyPath(ReferencePath path, Boolean skipParameterGroups) (+1)`
- `Boolean TryAddCopy(ReferencePath path, PathItem item)`
- `ReferencePath[] SplitPath(ReferencePath path, Func`2 predicate)`
- `ReferencePath[] TrySplitPath(ReferencePath path, Func`2 predicate) (+1)`
- `Int32 GetPathItemIndex(ReferencePath path, Func`2 predicate)`
- `ReferencePath ConcatPath(ReferencePath first, ReferencePath second)`
- `ReferencePath ReversePath(ReferencePath path)`
- `ParameterGroup GetOutputGroup(ReferencePath path)`
- `ReferencePath SubPath(ReferencePath source, Int32 start, Int32 count) (+1)`
- `ReferencePath TrimEndParameterGroup(ReferencePath path)`
- `ReferencePath MergeRepetitiveHierarchy(ReferencePath path)`
- `Boolean IsHierarchyOneToMany(ReferencePath path)`

### `RevisionsPathItem` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** Name: String, Icon: IconImage, Type: PathItemType, SupportSearchType: SupportSearchTypes

### `RootObjectPathItem` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** Name: String, Type: PathItemType, SupportSearchType: SupportSearchTypes
**Методы:**
- `Boolean IsOneToMany()`

### `SignatureParameterItem` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** Icon: IconImage, Name: String, Type: PathItemType, Parameter: SignatureParameter, SignatureIndex: Int32

### `SignaturePathItem` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** Name: String, Type: PathItemType
**Методы:**
- `Boolean IsOneToMany()`

### `SignatureTypeItem` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** Icon: IconImage, Name: String, SupportSearchType: SupportSearchTypes, Type: PathItemType, SignatureType: SignatureType

### `SignatureTypesItem` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** Name: String, Type: PathItemType, Icon: IconImage, SupportSearchType: SupportSearchTypes

### `SpecialLinkPathItem` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** SupportSearchType: SupportSearchTypes, SystemType: SystemParameterType

### `SpecialToManyLinkPathItem` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Методы:**
- `Boolean IsOneToMany()`

### `StartProductPathItem` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** Name: String, Type: PathItemType

### `StructureTypePathItem` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** Name: String, Type: PathItemType, SystemType: SystemParameterType

### `StructureTypesPathItem` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** Name: String, Type: PathItemType, SupportSearchType: SupportSearchTypes
**Методы:**
- `Boolean IsOneToMany()`

### `SystemParametersPathItem` (Namespace: `TFlex.DOCs.Model.Search.Path`)
**Свойства:** Name: String, Type: PathItemType, Group: ParameterGroup, SupportSearchType: SupportSearchTypes

### `ConfigurationGroupPathItem` (Namespace: `TFlex.DOCs.Model.Search.Path.Configuration`)
**Свойства:** Name: String, Type: PathItemType, SupportSearchType: SupportSearchTypes

### `ConfigurationParameterExtensions` (Namespace: `TFlex.DOCs.Model.Search.Path.Configuration`)
**Методы:**
- `String GetName(ConfigurationParameter configurationItemType)`

### `ConfigurationParameterPathItem` (Namespace: `TFlex.DOCs.Model.Search.Path.Configuration`)
**Свойства:** Name: String, Type: PathItemType, Parameter: ConfigurationParameter

### `LoadOptionsGroupPathItem` (Namespace: `TFlex.DOCs.Model.Search.Path.LoadOptions`)
**Свойства:** Name: String, Type: PathItemType, SupportSearchType: SupportSearchTypes

### `LoadOptionsParameterExtensions` (Namespace: `TFlex.DOCs.Model.Search.Path.LoadOptions`)
**Методы:**
- `String GetName(LoadOptionsParameter parameter)`

### `LoadOptionsParameterPathItem` (Namespace: `TFlex.DOCs.Model.Search.Path.LoadOptions`)
**Свойства:** Name: String, Type: PathItemType, Parameter: LoadOptionsParameter

### `LoadOptionsParameters` (Namespace: `TFlex.DOCs.Model.Search.Path.LoadOptions`)
**Свойства:** Offset: Nullable`1, Count: Nullable`1, OnlyChildrenObjects: Nullable`1, HasRootObjectFilter: Boolean, SortFields: ReadOnlyCollection`1, HasSortFields: Boolean
**Методы:**
- `ReferenceObject GetRootObject(Reference reference)` [has Async]

### `LoadOptionsRootObjectPathItem` (Namespace: `TFlex.DOCs.Model.Search.Path.LoadOptions`)
**Свойства:** Name: String, Type: PathItemType

### `LoadOptionsSortFieldPathItem` (Namespace: `TFlex.DOCs.Model.Search.Path.LoadOptions`)
**Свойства:** Name: String, Type: PathItemType

### `PathPerformer` (Namespace: `TFlex.DOCs.Model.Search.Path.Performers`)
**Методы:**
- `PathPerformer Build(PathPerformerSettings settings)`
- `ValueTuple`2 GetValueLanguageType()`
- `ValueTuple`2 GetOneValueLanguageType()`
- `Object GetValue(ReferenceObject referenceObject, ComplexHierarchyLink hierarchyLink)`
- `Object GetOneValue(ReferenceObject referenceObject, ComplexHierarchyLink hierarchyLink)`

### `PathPerformerSettings` (Namespace: `TFlex.DOCs.Model.Search.Path.Performers`)
**Свойства:** Type: PathPerformerType, FromHierarchy: Boolean
**Методы:**
- `PathPerformerSettings Create(ReferencePath path)`

### `OrderingSearchRule` (Namespace: `TFlex.DOCs.Model.Search.Rules`)
**Свойства:** Descending: Boolean

### `SearchRule` (Namespace: `TFlex.DOCs.Model.Search.Rules`)
**Свойства:** Owner: TermGroupItem, Value: Object

### `DigitalSignatureContent` (Namespace: `TFlex.DOCs.Model.Signatures`)
**Свойства:** ParametersGuids: Guid[], Parameters: String[], ReferenceObjectData: Byte[]
**Методы:**
- `Byte[] GetData()` [has Async]
- `String GetSigningParametersValuesString()`
- `List`1 Parse(String parametersString, Boolean& hasReferenceObjectData) (+1)`

### `Signature` (Namespace: `TFlex.DOCs.Model.Signatures`)
**Свойства:** SignatureTypeCollection: List`1, Signatures: SignatureCollection, Id: Int32, TypeId: Int32, SignatureObjectType: SignatureType, UserObject: UserReferenceObject, State: SignatureState, UserId: Int32, UserName: String, OnBehalfOfName: String, OnBehalfOfId: Int32, OnBehalfOfUser: User, SignatureDate: Nullable`1, Resolution: String, Actual: Boolean, DigitalSignature: Byte[], SignedParameters: String, HasDigitalSignature: Boolean, ErrorMessage: String, CanDelete: Boolean, CanUpdate: Boolean, Item: Object
**Методы:**
- `Boolean Edit()`
- `Boolean UpdateDate()`
- `Boolean Update()`
- `Boolean Delete()`
- `Nullable`1 ValidateDigitalSignature()`
- `String FindParametersNames()`
- `List`1 GetParsedSignedParameters()`
- `Byte[] GenerateDigitalSignature(ReferenceObject referenceObject, X509Certificate2 certificate, Byte[] signingData)`

### `SignatureParameterExtensions` (Namespace: `TFlex.DOCs.Model.Signatures`)
**Методы:**
- `String GetName(SignatureParameter parameter)`
- `String GetTypeDescription(SignatureParameter parameter)`
- `Type GetValueType(SignatureParameter parameter)`

### `SignatureStateExtensions` (Namespace: `TFlex.DOCs.Model.Signatures`)
**Методы:**
- `String GetName(SignatureState state)`

### `SignatureType` (Namespace: `TFlex.DOCs.Model.Signatures`)
**Свойства:** Types: SignatureTypes, Connection: ServerConnection, Id: Int32, Guid: Guid, Name: String, Description: String, UsersAccessType: ItemListUseType, IsSignatureTypeInUse: Boolean
**Методы:**
- `List`1 GetAccessUsers()`
- `Void SetAccessUsers(IEnumerable`1 users)`
- `Boolean ValidateUserAccess(UserReferenceObject user, SignatureState state, Boolean throwException)`
- `Boolean ValidateParameterGroupAccess(ParameterGroup group, Boolean throwException)`
- `Boolean Edit()`
- `Boolean Delete()`
- `SignatureType Find(ServerConnection connection, Int32 id) (+2)`

### `SignatureTypes` (Namespace: `TFlex.DOCs.Model.Signatures`)
**Свойства:** Connection: ServerConnection
**Методы:**
- `SignatureType AddSignatureType(String name, String description)`
- `List`1 GetAccessibleSignatureTypes(User user)`
- `IEnumerator`1 GetEnumerator()`

### `SigningParametersAttribute` (Namespace: `TFlex.DOCs.Model.Signatures`)
**Свойства:** Caption: String, IsSystem: Boolean, CanChangeCaption: Boolean, CanRemove: Boolean, Value: Object

### `SigningParametersInfo` (Namespace: `TFlex.DOCs.Model.Signatures`)
**Свойства:** SigningParameters: List`1, IsInherit: Boolean, IsDefault: Boolean

### `SignatureFormulaMacro` (Namespace: `TFlex.DOCs.Model.Signatures.Macros`)
**Свойства:** CodeOffset: Int32

### `SignatureMacroContext` (Namespace: `TFlex.DOCs.Model.Signatures.Macros`)
**Свойства:** Signature: Signature

### `SignatureMacroProvider` (Namespace: `TFlex.DOCs.Model.Signatures.Macros`)
**Свойства:** Context: SignatureMacroContext, CurrentSignature: SignatureObj, ТекущаяПодпись: Подпись [RU only]

### `ObjectStage` (Namespace: `TFlex.DOCs.Model.Stages`)
**Свойства:** Stage: Stage, StartDate: DateTime, EndDate: Nullable`1, User: User, IsActual: Boolean, Comment: String, Item: Object

### `ObjectStageInfo` (Namespace: `TFlex.DOCs.Model.Stages`)
**Методы:**
- `ObjectStage GetActualObjectStage()`
- `IReadOnlyList`1 GetStagesHistory(Boolean reload)`

### `ObjectStageParameterExtension` (Namespace: `TFlex.DOCs.Model.Stages`)
**Методы:**
- `String GetName(ObjectStageParameter parameter)`

### `Scheme` (Namespace: `TFlex.DOCs.Model.Stages`)
**Свойства:** Connection: ServerConnection, Id: Int32, Guid: Guid, Name: String, Comment: String, Stages: ReadOnlyCollection`1, Changing: Boolean
**Методы:**
- `SchemeStage AddStage(Stage stage)`
- `Void BeginChanges()`
- `Boolean EndChanges()` [has Async]
- `Void CancelChanges()`
- `Boolean Delete()` [has Async]
- `List`1 GetSchemes(ServerConnection connection) (+1)` [has Async]

### `SchemeStage` (Namespace: `TFlex.DOCs.Model.Stages`)
**Свойства:** Id: Int32, Guid: Guid, Scheme: Scheme, Stage: Stage, IsAdded: Boolean, IsDeleted: Boolean, Transitions: ReadOnlyCollection`1
**Методы:**
- `List`1 GetNextStages()`
- `SchemeStageTransition AddTransition(SchemeStage stage, Boolean isAutomatic, Boolean isManual)`
- `Void Delete()`
- `IEnumerator`1 GetEnumerator()`

### `SchemeStageTransition` (Namespace: `TFlex.DOCs.Model.Stages`)
**Свойства:** FromStage: SchemeStage, ToStage: SchemeStage, IsAutomatic: Boolean, IsManual: Boolean, IsAdded: Boolean, IsDeleted: Boolean
**Методы:**
- `Void Delete()`

### `Stage` (Namespace: `TFlex.DOCs.Model.Stages`)
**Свойства:** Connection: ServerConnection, Id: Int32, Guid: Guid, Name: String, Comment: String, Changing: Boolean, RequireModificationNotice: Boolean, SetModificationNoticeReady: Boolean
**Методы:**
- `Void BeginChanges()`
- `Boolean EndChanges()` [has Async]
- `Void CancelChanges()`
- `Boolean Delete()` [has Async]
- `List`1 Clear(ServerConnection connection, IEnumerable`1 objects, String comment) (+1)` [has Async]
- `List`1 Set(IEnumerable`1 objects, String comment) (+1)` [has Async]
- `List`1 Change(IEnumerable`1 objects, String comment) (+1)` [has Async]
- `List`1 AutomaticChange(IEnumerable`1 objects, String comment) (+1)` [has Async]
- `List`1 GetStages(ServerConnection connection) (+2)` [has Async]
- `Stage Find(ServerConnection connection, Int32 id) (+4)` [has Async]
- `Dictionary`2 LoadSimpleStages(ServerConnection connection)` [has Async]
- `List`1 SetObjectsStage(Stage stage, IEnumerable`1 objects)`
- `List`1 ChangeObjectsStage(Stage stage, IEnumerable`1 objects)`
- `List`1 AutomaticChangeObjectsStage(Stage stage, IEnumerable`1 objects)`

### `ExtendedParameterReferenceLink` (Namespace: `TFlex.DOCs.Model.Structure`)
**Свойства:** Key: GuidKey, Guid: Guid, Id: Int32, ParameterId: Int32, GroupId: Int32, ClassId: Int32, Alias: String, Comment: String, ParameterGroup: ParameterGroup, ClassObject: ClassObject, Connection: ServerConnection
**Методы:**
- `ExtendedParameterReferenceLink Create(ServerConnection connection, Int32 parameterId, Int32 groupId, Int32 classId, String alias, String comment) (+3)`
- `ClassObject GetLinkedClassObject(ClassObject classObject, Int32 groupId)`
- `Boolean IsLinkedClass(Int32 groupId, Int32 classId)`
- `String GetParameterGroupName()`

### `ExtendedParameterReferenceLinksList` (Namespace: `TFlex.DOCs.Model.Structure`)
**Свойства:** IsEditable: Boolean, IsEditableList: Boolean, CanAdd: Boolean, CanEdit: Boolean, CanDelete: Boolean, CanEditList: Boolean, Item: ExtendedParameterReferenceLink, Count: Int32, IsReadOnly: Boolean
**Методы:**
- `Boolean ContainsAlias(String aliasToFind, Int32 groupId, Int32 classId)`
- `Boolean IsLinkedGroup(Int32 groupId)`
- `Boolean IsLinkedGroupOnly(Int32 groupId)`
- `Boolean IsLinkedClassAndGroup(Int32 groupId, Int32 classId)`
- `Boolean IsLinkedClassOrGroup(Int32 groupId, Int32 classId)`
- `ClassObject GetLinkedClassObject(ClassObject classObject, Int32 groupId)`
- `Int32 IndexOf(ExtendedParameterReferenceLink item)`
- `Void Insert(Int32 index, ExtendedParameterReferenceLink item)`
- `Void RemoveAt(Int32 index)`
- `Void Add(ExtendedParameterReferenceLink item)`
- `Void Clear()`
- `Boolean Contains(ExtendedParameterReferenceLink item)`
- `Void CopyTo(ExtendedParameterReferenceLink[] array, Int32 arrayIndex)`
- `Boolean Remove(ExtendedParameterReferenceLink item)`
- `IEnumerator`1 GetEnumerator()`

### `ExtendedParametersStorage` (Namespace: `TFlex.DOCs.Model.Structure`)
**Свойства:** Source: ExtendedParametersStorage, Id: Int32, Guid: Guid, Name: String, Comment: String

### `InstancesGroupInfo` (Namespace: `TFlex.DOCs.Model.Structure`)
**Свойства:** MasterReferenceId: Int32, InstancesReferenceId: Int32, LinkToObjectId: Int32, LinkToHierarchyId: Int32, LinkToAltRepId: Int32, InstanceMainClassId: Int32, MasterReference: ReferenceInfo, InstancesReference: ReferenceInfo, LinkToObject: ParameterGroup, LinkToHierarchy: ParameterGroup, InstanceMainClass: ClassObject, LinkToSourceStructureInstances: ParameterGroup, LinkToAltRep: ParameterGroup
**Методы:**
- `Boolean IsInstanceSystemLink(ParameterGroup link)`

### `LinkRequiredExtensions` (Namespace: `TFlex.DOCs.Model.Structure`)
**Методы:**
- `String GetName(LinkRequired requirement)`

### `LinkTypeExtensions` (Namespace: `TFlex.DOCs.Model.Structure`)
**Методы:**
- `String GetName(LinkType type)`

### `LinkVisibilityExtensions` (Namespace: `TFlex.DOCs.Model.Structure`)
**Методы:**
- `String GetName(LinkVisibility visibility)`

### `ListValue` (Namespace: `TFlex.DOCs.Model.Structure`)
**Свойства:** List: ParameterValueList, Key: Guid, Name: String, Value: Object, Icon: IconImage

### `ParameterActivityStatusExtensions` (Namespace: `TFlex.DOCs.Model.Structure`)
**Методы:**
- `String GetActivityStatusName(ParameterActivityStatus status)`

### `ParameterEditTypeExtension` (Namespace: `TFlex.DOCs.Model.Structure`)
**Методы:**
- `InplaceEditType ToInplaceEditType(ParameterEditType parameterEditType) (+1)`
- `String GetText(InplaceEditType inplaceEditType)`
- `Boolean IsEditingInDialogAllowed(ParameterEditType parameterEditType)`
- `Boolean IsEditingInGridForbidden(ParameterEditType parameterEditType)`

### `ParameterEditTypeExtensions` (Namespace: `TFlex.DOCs.Model.Structure`)
**Методы:**
- `String GetEditTypeName(ParameterEditType type)`

### `ParameterGroup` (Namespace: `TFlex.DOCs.Model.Structure`)
**Свойства:** Connection: ServerConnection, Id: Int32, Guid: Guid, IsClassesLoaded: Boolean, SystemObjectType: SystemObjectType, ReferenceInfo: ReferenceInfo, ReferenceGroup: ParameterGroup, HierarchyGroup: ParameterGroup, Classes: ClassTree, ActivityStatus: GroupActivityStatus, Type: ParameterGroupType, HierarchyType: ReferenceHierarchyType, MasterGroup: ParameterGroup, SlaveGroupId: Int32, SlaveGroup: ParameterGroup, Name: String, CheckAccess: AccessRightsMode, SupportsEncryption: Boolean, SupportsExtendedParameters: Boolean, SupportsMandatoryAccess: Boolean, ExtendedParameteresStorage: ExtendedParametersStorage, Comment: String, Visibility: ReferenceVisibility, Icon: IconImage, TypeIcon: IconImage, TableName: String, TableNameGenerated: Boolean, SupportsDesktop: Boolean, SupportsRecycleBin: Boolean, SupportsDataChangeLog: Boolean, SupportsRevisions: Boolean, SupportsClasses: Boolean, SupportsSystemObjects: Boolean, SupportsPrototypes: Boolean, SupportsOrder: Boolean, SupportsOwner: Boolean, SupportsSignature: Boolean, UseAllSignatureTypes: Boolean, SupportsObjectsInstances: Boolean, SupportsStructureTypes: Boolean, IsObjectsInstancesImpl: Boolean, SignatureTypes: List`1, SupportsPrivateFolders: Boolean, SupportsNomenclature: Boolean, SupportsConfigurationSettings: Boolean, SupportsDesignContexts: Boolean, SupportsActivityDates: Boolean, SupportsApplicability: Boolean, SupportsSubstitutesInContext: Boolean, Swapped: Boolean, LinkToSameReference: Boolean, LinkType: LinkType, LinkVisibility: LinkVisibility, LinkRequired: LinkRequired, DoubleDirectionLink: Boolean, IsAsymmetricLink: Boolean, DefaultVisibleParameter: ParameterInfo, UserControl: String, SelectionPath: String, CanEdit: Boolean, CanEditExtendedParameters: Boolean, CanDelete: Boolean, Parameters: ParameterInfoCollection, SearchQueryLinkFilter: Filter, SearchQueryLinkPathToFilter: ReferencePath, SystemParameters: ParameterInfoCollection, Indexes: ReadOnlyCollection`1, UniqueIndex: UniqueIndex, EventHandlers: EventHandlerCollection, Dialog: Dialog, Dialogs: DialogManager, WebDialog: Dialog, WebDialogs: WebDialogManager, RevisionNamingRule: RevisionNamingRuleObject, ConfiguratorGuid: Guid, Configurator: Configurator, InstancesGroupInfo: InstancesGroupInfo, HasHierarchy: Boolean, IsReference: Boolean, IsLinkGroup: Boolean, IsCorruptedLink: Boolean, IsTableOneToOne: Boolean, IsLinkToOne: Boolean, IsToManyRelation: Boolean, IsLinkToMany: Boolean, IsAnyReferenceLink: Boolean, IsSearchQueryLink: Boolean, IsTableOneToMany: Boolean, IsHierarchyTable: Boolean, IsCommonTableOneToOne: Boolean, Item: ParameterInfo, Item: ParameterInfo, Item: ParameterInfo, Item: ParameterInfo, ClassParameterInfo: ParameterInfo, AuthorParameterInfo: ParameterInfo, EditorParameterInfo: ParameterInfo, CreationDateParameterInfo: ParameterInfo, EditDateParameterInfo: ParameterInfo, ParentParameterInfo: ParameterInfo, SupportsStages: Boolean, Scheme: Scheme, DefaultStage: SchemeStage, CanChangeClass: Boolean, SupportMultiAttachment: Boolean, XmlId: String, AuthorAccess: AccessGroup, ObjectFormat: ObjectFormat
**Методы:**
- `Boolean ReloadOneToOneParameters()`
- `Boolean TryParseXmlId(String xmlId, Int32& id, Boolean& deleted)`
- `T GetGroupSettings(Guid settingsId, Int32 classId)` [has Async]
- `Boolean SetGroupSettings(Guid settingsId, Int32 classId, T settings)` [has Async]
- `Boolean ClearGroupSettings(Guid settingsId, Int32 classId)` [has Async]
- `ClassObjectCollection GetAllowedClassesToLink()`
- `SigningParametersInfo GetSigningParametersInfo()` [has Async]
- `Void SetSigningParameters(List`1 signingParameters)` [has Async]
- `Boolean Contains(ParameterGroup group)`
- `Boolean HasSlaveGroup()`
- `String GetSecondCaption()`
- `ParameterGroup GetSwappedLink()`
- `List`1 GetEventHandlers(ParameterGroupEvent event)` [has Async]

### `ParameterGroupType` (Namespace: `TFlex.DOCs.Model.Structure`)
**Свойства:** Type: Int32, Name: String, IsLink: Boolean, IsReference: Boolean, IsTable: Boolean, Icon: IconImage
**Методы:**
- `Int32 CompareTo(ParameterGroupType other)`

### `ParameterGroupVisibilityExtensions` (Namespace: `TFlex.DOCs.Model.Structure`)
**Методы:**
- `String GetName(ParameterGroupVisibility visibility)`

### `ParameterInfo` (Namespace: `TFlex.DOCs.Model.Structure`)
**Свойства:** Group: ParameterGroup, Connection: ServerConnection, Type: ParameterType, Name: String, Comment: String, Length: Int32, Format: String, DecimalPlaces: Int32, FieldName: String, IsVisible: Boolean, EditType: ParameterEditType, Nullable: Boolean, IsRequired: Boolean, DefaultValue: Object, IsIndexed: Boolean, IsFullTextSearchEnabled: Boolean, Encrypted: Boolean, CertificateGuid: Guid, Certificate: Certificate, PreviousPropertiesState: ObjectPropertiesState, ValueList: ParameterValueList, RangeInfo: ParameterRangeInfo, UserControl: String, IsPrimaryKey: Boolean, ActivityStatus: ParameterActivityStatus, Unit: Unit, TypeName: String, IsSystem: Boolean, SystemType: SystemParameterType, CanEdit: Boolean, CanBeIndexed: Boolean, CanBeFullTextSearchEnabled: Boolean, CanDelete: Boolean, IsSystemKey: Boolean, IsCheckOutStateField: Boolean, SystemObjectType: SystemObjectType, TypeIcon: IconImage, SupportsSearch: Boolean, Alias: ExtendedParameterReferenceLink, Aliases: ExtendedParameterReferenceLinksList, IsExtended: Boolean, IsVirtual: Boolean, IsGenerated: Boolean, IsSupportedAliases: Boolean, XmlId: String
**Методы:**
- `Void FixParameterState()`
- `Boolean IsLinkedToGroup(Int32 groupId)`
- `Boolean IsLinkedToGroupClass(Int32 groupId, Int32 classId)`
- `List`1 GetComparisonOperators()`
- `List`1 GetIndexes()`
- `Int32 CompareTo(ParameterInfo other)`
- `Object GetFormat(Type formatType)`
- `Boolean SkipUsingServerTermValue(ComparisonOperator comparisonOperator)`
- `Boolean TryParseXmlId(String xmlId, Int32& id, Boolean& deleted)`
- `String SerializeValue(Object value, ObjectSerializationMode mode)`
- `Object ParseValue(String value)`

### `ParameterRangeInfo` (Namespace: `TFlex.DOCs.Model.Structure`)
**Свойства:** Id: Int32, Guid: Guid, Name: String, MinParameterId: Int32, MaxParameterId: Int32, Owner: ParameterInfo
**Методы:**
- `Boolean ValidateRange(ParameterInfo parameterInfo, Object value, ReferenceObject referenceObject, Boolean throwOnError)`

### `ParameterType` (Namespace: `TFlex.DOCs.Model.Structure`)
**Свойства:** DefaultMantissaLength: Int32, Id: Int32, Name: String, FullName: String, MaxSize: Int32, IsVariantSize: Boolean, IsSigned: Boolean, IsString: Boolean, IsInt: Boolean, IsFloat: Boolean, IsMoney: Boolean, IsNumber: Boolean, IsDateTime: Boolean, IsBoolean: Boolean, IsBlob: Boolean, IsSupported: Boolean, CanBeEncrypted: Boolean, CanBeIndexed: Boolean, CanBeFullTextSearchEnabled: Boolean, CanUseInUniqueIndex: Boolean, SupportsNullable: Boolean, AlwaysNullable: Boolean, SupportsSize: Boolean, SupportsMaxSize: Boolean, AlwaysMaxSize: Boolean, SupportsValueList: Boolean, SupportsRange: Boolean, ParameterFormatter: ICustomFormatter, LanguageType: Type, DefaultValue: Object, MinValue: Object, MaxValue: Object
**Методы:**
- `List`1 GetTypeList(Boolean supportedOnly)`
- `Object Parse(String s, IFormatProvider provider, ComparisonOperator operator) (+1)`
- `Boolean TryParse(String s, IFormatProvider provider, Object& result) (+1)`
- `Object ParseDefaultValue(String s, IFormatProvider provider) (+1)`
- `Boolean TryParseDefaultValue(String s, IFormatProvider provider, Object& result) (+1)`
- `String ConvertToString(Object value)`
- `Boolean SupportsConversionTo(Int32 typeId) (+1)`
- `List`1 GetConversionList()`
- `List`1 GetComparisonOperators()`
- `Type GetObjectParameterType()`

### `ParameterValueList` (Namespace: `TFlex.DOCs.Model.Structure`)
**Свойства:** ParameterInfo: ParameterInfo, Type: ParameterType, Nullable: Boolean, IsFixed: Boolean, IsEditable: Boolean, IsExpandable: Boolean, IsEditableList: Boolean, CanAdd: Boolean, CanEditList: Boolean, CanChangeReferenceStructure: Boolean, CanEdit: Boolean, CanDelete: Boolean, Item: ListValue, Count: Int32, IsReadOnly: Boolean
**Методы:**
- `ListValue AddValue(Object value, IconImage icon, String name) (+1)`
- `Void UpdateValue(Int32 index, Object value)`
- `Void UpdateListValue(Int32 index, ListValue value)`
- `Void DeleteValue(Int32 index)`
- `Void SetValues(IEnumerable`1 values)`
- `Object GetValue(String name)`
- `String GetName(Object value)`
- `Int32 IndexOf(ListValue item)`
- `Void Insert(Int32 index, ListValue item)`
- `Void RemoveAt(Int32 index)`
- `Void Add(ListValue item)`
- `Void Clear()`
- `Boolean Contains(ListValue item)`
- `Void CopyTo(ListValue[] array, Int32 arrayIndex)`
- `Boolean Remove(ListValue item)`
- `IEnumerator`1 GetEnumerator()`

### `ReferenceHierarchyTypeExtensions` (Namespace: `TFlex.DOCs.Model.Structure`)
**Методы:**
- `String GetName(ReferenceHierarchyType type)`

### `ReferenceVisibilityExtensions` (Namespace: `TFlex.DOCs.Model.Structure`)
**Методы:**
- `String GetName(ReferenceVisibility visibility)`

### `SystemObjectTypeExtensions` (Namespace: `TFlex.DOCs.Model.Structure`)
**Методы:**
- `String GetName(SystemObjectType type)`

### `UniqueIndex` (Namespace: `TFlex.DOCs.Model.Structure`)
**Свойства:** Group: ParameterGroup, Id: Int32, Guid: Guid, Name: String, Parameters: ReadOnlyCollection`1

### `AliasedParameterInfoBuilder` (Namespace: `TFlex.DOCs.Model.Structure.Builders`)
**Свойства:** IsVirtualParameterBuilder: Boolean
**Методы:**
- `Void FixParameterState()`
- `Void CopyFrom(ParameterInfo sourceParameter)`

### `ClassObjectBuilder` (Namespace: `TFlex.DOCs.Model.Structure.Builders`)
**Свойства:** Classes: ClassTree, Class: ClassObject, Base: ClassObject, ParameterGroups: ParameterGroupCollection, SwappedToSelfParameterGroups: SwappedToSelfCollection, ChildObjectClasses: ClassObjectCollection, ParentObjectClasses: ClassObjectCollection, MasterObjectClasses: ClassObjectCollection, Attributes: ClassObjectAttributes, IsAdded: Boolean, IsModified: Boolean, Icon: IconImage, CanChangeIcon: Boolean, Name: String, Comment: String, IsAbstract: Boolean, IsSealed: Boolean, UseBaseClassIcon: Boolean, CanCreateInRoot: Boolean, SupportsSaveAndCreate: Boolean, ShowChangeCommandInObjectProperties: Nullable`1, PropertiesDisplayType: PropertiesDisplayType, CreateFromPrototype: Boolean, Hidden: Boolean, IsStandaloneProductByDefault: Boolean, IsSchemeInherit: Boolean, IsDefaultStageInherit: Boolean, UniqueIndex: UniqueIndex, Scheme: Scheme, DefaultStage: SchemeStage, InheritCanChange: Boolean, CanChange: Boolean, InheritMasterObjectClasses: Boolean, ObjectFormat: ObjectFormat, SupportMultiAttachment: Boolean, IsSupportMultiAttachmentInherit: Boolean, InheritRevisionNamingRule: Boolean, RevisionNamingRule: RevisionNamingRuleObject
**Методы:**
- `Void Save()` [has Async]
- `Void Delete(ClassObject classObject)` [has Async]

### `ExtendedParameterInfoBuilder` (Namespace: `TFlex.DOCs.Model.Structure.Builders`)
**Свойства:** IsExtendedParameterBuilder: Boolean, Aliases: ExtendedParameterReferenceLinksList, Connection: ServerConnection, ParameterClass: ClassObject, IsAliasesEditable: Boolean, IsAliasesEditableList: Boolean, CanAddAliases: Boolean, CanEditAliases: Boolean, CanDeleteAliases: Boolean, CanEditAliasesList: Boolean
**Методы:**
- `Void FixParameterState()`
- `Void CopyFrom(ParameterInfo sourceParameter)`
- `Void Save()` [has Async]
- `ExtendedParameterReferenceLink AddAlias(ParameterInfo parameter, ParameterGroup group, ClassObject classObject, String alias, String comment) (+2)`
- `ParameterInfo BuildAliasedParameterInfo(ParameterGroup group, ExtendedParameterReferenceLink aliasInfo) (+1)`
- `Void UpdateAlias(Int32 index, String alias) (+1)`
- `Void UpdateComment(Int32 index, String comment)`
- `Void DeleteAlias(Int32 index)`
- `Void SetAliases(IEnumerable`1 aliases)`
- `ParameterInfo FindAliasParameterInfo(String aliasToFind, ParameterGroup group)`
- `ExtendedParameterReferenceLink FindAlias(String aliasToFind, ParameterGroup group, ClassObject classObject) (+3)`
- `Boolean ContainsAlias(String aliasToFind, Int32 groupId, Int32 classId)`

### `ExtendedParametersStorageBuilder` (Namespace: `TFlex.DOCs.Model.Structure.Builders`)
**Свойства:** ValuesStorage: ExtendedParametersStorage, IsAdded: Boolean, IsModified: Boolean, Name: String, Comment: String

### `LinkGroupBuilder` (Namespace: `TFlex.DOCs.Model.Structure.Builders`)
**Свойства:** MasterName: String, SlaveName: String, SlaveGroup: ParameterGroup, Swapped: Boolean, LinkType: LinkType, LinkVisibility: LinkVisibility, CheckAccess: AccessRightsMode, LinkRequired: LinkRequired, SelectionPath: String, DoubleDirection: Boolean, SupportsVersions: Boolean, IsAsymmetricLink: Boolean, SearchQueryLinkFilter: String, SearchQueryLinkPathToFilter: String
**Методы:**
- `Void Save()` [has Async]

### `ObjectFormatBuilder` (Namespace: `TFlex.DOCs.Model.Structure.Builders`)
**Свойства:** MasterGroup: ParameterGroup, Type: ObjectFormatType, Format: String
**Методы:**
- `ObjectFormat Save()`

### `ObjectParameterFormatBuilder` (Namespace: `TFlex.DOCs.Model.Structure.Builders`)
**Свойства:** Link: ParameterGroup, Parameter: ParameterInfo, Format: String

### `ParameterGroupBuilder` (Namespace: `TFlex.DOCs.Model.Structure.Builders`)
**Свойства:** DefaultVisibleParameter: ParameterInfo, ObjectFormat: ObjectFormat, SupportsOrder: Boolean, SupportsSignature: Boolean, UseAllSignatureTypes: Boolean, SignatureTypes: List`1, CanChangeClass: Boolean
**Методы:**
- `Void Save(Boolean createNameParameter) (+1)` [has Async]

### `ParameterGroupBuilderBase` (Namespace: `TFlex.DOCs.Model.Structure.Builders`)
**Свойства:** Connection: ServerConnection, ParameterGroup: ParameterGroup, Type: ParameterGroupType, MasterGroup: ParameterGroup, Name: String, TableName: String, TableNameGenerated: Boolean, Comment: String, Icon: IconImage, CanChangeIcon: Boolean, UserControl: String, IsAdded: Boolean, IsModified: Boolean
**Методы:**
- `Void Save()` [has Async]
- `Void Delete(ParameterGroup group)` [has Async]

### `ParameterInfoBuilder` (Namespace: `TFlex.DOCs.Model.Structure.Builders`)
**Свойства:** ParameterInfo: ParameterInfo, ParameterGroup: ParameterGroup, Connection: ServerConnection, IsAdded: Boolean, IsModified: Boolean, IsExtendedParameterBuilder: Boolean, IsVirtualParameterBuilder: Boolean, Type: ParameterType, Name: String, Comment: String, MaxLength: Int32, ParameterFormat: String, FieldName: String, IsVisible: Boolean, CertificateGuid: Guid, AllowEdit: Boolean, AllowChangeFromGrid: Boolean, InplaceEditType: InplaceEditType, AllowNull: Boolean, AlwaysPresent: Boolean, DefaultValue: Object, IsIndexed: Boolean, IsFullTextSearchEnabled: Boolean, Unit: Unit, ContainsValueList: Boolean, IsNewValueList: Boolean, ParameterValueList: ValueList, ParameterRangeInfo: ParameterRangeInfo, ContainsRangeInfo: Boolean, UserControl: String, PreviousPropertiesState: ObjectPropertiesState
**Методы:**
- `Void CopyFrom(ParameterInfo sourceParameter)`
- `Void Save()` [has Async]
- `Void Delete(ParameterInfo parameter)` [has Async]
- `Void FixParameterState()`

### `ParameterInfoBuildersFactory` (Namespace: `TFlex.DOCs.Model.Structure.Builders`)
**Методы:**
- `ParameterInfoBuilder CreateParameterInfoBuilder(ParameterInfo parameterInfo)`
- `ParameterInfoBuilder Create(ParameterGroup parameterGroup, ParameterInfo parameterInfo)`

### `ReferenceBuilder` (Namespace: `TFlex.DOCs.Model.Structure.Builders`)
**Свойства:** ReferenceInfo: ReferenceInfo, Folder: ReferenceCatalogFolder, HierarchyType: ReferenceHierarchyType, Visibility: ReferenceVisibility, SupportsDesktop: Boolean, SupportsRecycleBin: Boolean, SupportsDataChangeLog: Boolean, SupportsRevisions: Boolean, RevisionNamingRule: RevisionNamingRuleObject, Configurator: Configurator, SupportsPrototypes: Boolean, SupportsStages: Boolean, SupportsExtendedParameters: Boolean, SupportsMandatoryAccess: Boolean, SupportsOwner: Boolean, SupportsObjectsInstances: Boolean, SupportsStructureTypes: Boolean, SupportsConfigurationSettings: Boolean, SupportsDesignContexts: Boolean, SupportsSubstitutesInContext: Boolean, SupportsActivityDates: Boolean, SupportsApplicability: Boolean, UniqueIndex: UniqueIndex, Scheme: Scheme, DefaultStage: SchemeStage, AuthorAccess: AccessGroup, CheckAccess: AccessRightsMode, SupportsEncryption: Boolean, IsActive: Boolean
**Методы:**
- `Void Save(Boolean createNameParameter)` [has Async]
- `Void Delete(ReferenceInfo reference)` [has Async]
- `Void MoveToAnotherFolder(ReferenceCatalogFolder folder)` [has Async]
- `Void Deactivate()` [has Async]
- `Void Activate()` [has Async]
- `Void RefreshInheritedAccesses()`
- `Void RefreshMainAccesses()`
- `Void ResetUsersSettings(Boolean clearViews)`
- `Void ResetUserWebSettings(User user, WebConfiguration configuration)`
- `Void RecreateFullTextSearchIndicies()` [has Async]

### `UniqueIndexBuilder` (Namespace: `TFlex.DOCs.Model.Structure.Builders`)
**Свойства:** IsAdded: Boolean, IsModified: Boolean, ParameterGroup: ParameterGroup, Reference: ReferenceInfo, Index: UniqueIndex, Name: String, Parameters: ParameterCollection
**Методы:**
- `UniqueIndex Save()` [has Async]
- `Boolean Delete(UniqueIndex index)` [has Async]

### `ExtendedParametersManager` (Namespace: `TFlex.DOCs.Model.Structure.ExtendedParameters`)
**Методы:**
- `IReadOnlyCollection`1 GetExtendedParameters()` [has Async]
- `ParameterInfo AttachExtendedParameterToParameterGroup(ParameterInfo parameter, ParameterGroup group, ClassObject classObject) (+1)` [has Async]
- `ParameterInfo AttachExtendedParameterToClassObject(ParameterInfo parameter, ParameterGroup group, ClassObject classObject)` [has Async]
- `Boolean DetachExtendedParameterFromParameterGroup(ParameterInfo parameter, ParameterGroup group, ClassObject classObject)` [has Async]
- `ExtendedParameterReferenceLinksList GetExtendedParameterInfoAliases(ParameterInfo extendedParameter)` [has Async]

### `ParameterInfoExtensions` (Namespace: `TFlex.DOCs.Model.Structure.Extensions`)
**Методы:**
- `Type GetRealType(ParameterInfo parameterInfo)`
- `Type GetRealServerType(ParameterInfo parameterInfo)`
- `ParameterType GetRealParameterType(ParameterInfo parameterInfo)`

### `ReferenceIndexManager` (Namespace: `TFlex.DOCs.Model.Structure.Indexes`)
**Методы:**
- `List`1 GetIndexes(ReferenceInfo referenceInfo)` [has Async]

### `TableIndex` (Namespace: `TFlex.DOCs.Model.Structure.Indexes`)
**Свойства:** Group: ParameterGroup, Name: String, Enable: Boolean, IndexSize: Int64, Fragmentation: Double

### `CustomUndoBlock` (Namespace: `TFlex.DOCs.Model.Undo`)
**Свойства:** Name: String, AllowAddAction: Boolean
**Методы:**
- `Boolean CanClose()`

### `UndoBlock` (Namespace: `TFlex.DOCs.Model.Undo`)
**Свойства:** Name: String
**Методы:**
- `Boolean CanClose()`

### `UndoBlockBase` (Namespace: `TFlex.DOCs.Model.Undo`)
**Свойства:** Name: String, State: BlockState, AllowAddAction: Boolean
**Методы:**
- `Boolean CanClose()`
- `Void Undo()`
- `Void Redo()`
- `Void Close()`

### `UndoManager` (Namespace: `TFlex.DOCs.Model.Undo`)
**Свойства:** ClosedBlocks: List`1, RestoredBlocks: List`1, BlockOpened: Boolean, Saving: Boolean
**Методы:**
- `Void SetReference(Reference reference)`
- `Void StartUndoBlock(String blockName)`
- `Void EndUndoBlock()`
- `Void CancelUndoBlock()`
- `Void Undo()`
- `Void Redo()`
- `Void CreateCustomUndoBlock(String blockName, Action undoAction, Action redoAction)`

### `UndoManagerActionHandler` (Namespace: `TFlex.DOCs.Model.Undo`)
**Методы:**
- `Void Invoke(Object sender, UndoManagerEventArgs args)`
- `IAsyncResult BeginInvoke(Object sender, UndoManagerEventArgs args, AsyncCallback callback, Object object)`
- `Void EndInvoke(IAsyncResult result)`

### `UndoManagerExtensions` (Namespace: `TFlex.DOCs.Model.Undo`)
**Методы:**
- `T RunOperationWithUndo(UndoManager undoManager, String blockName, Func`2 operation, TArg arg) (+4)`

### `CallbackSolution` (Namespace: `TFlex.DOCs.Model.Utils`)
**Свойства:** Text: String, Tag: Object, IsAccepted: Boolean

### `CallbackSolutions` (Namespace: `TFlex.DOCs.Model.Utils`)
**Методы:**
- `Boolean Invoke(ICollection`1 solutions)`
- `IAsyncResult BeginInvoke(ICollection`1 solutions, AsyncCallback callback, Object object)`
- `Boolean EndInvoke(IAsyncResult result)`

### `CallbackSolutionsAsync` (Namespace: `TFlex.DOCs.Model.Utils`)
**Методы:**
- `Task`1 Invoke(ICollection`1 solutions)`
- `IAsyncResult BeginInvoke(ICollection`1 solutions, AsyncCallback callback, Object object)`
- `Task`1 EndInvoke(IAsyncResult result)`

### `DesktopObjectResources` (Namespace: `TFlex.DOCs.Model.Utils`)
**Методы:**
- `IconImage GetLockStateIcon(ReferenceObjectLockState lockState)`
- `String GetLockStateDescription(ReferenceObjectLockState lockState)`

### `PackageHelper` (Namespace: `TFlex.DOCs.Model.Utils`)
**Методы:**
- `Package Create(Stream stream) (+1)`
- `Package Open(Stream stream) (+1)`
- `Uri CreateRelativeUri(String filePath)`
- `Stream AddXmlContent(Package package, String fileName)`
- `Stream AddFile(Package package, String fileName)`
- `Stream GetFile(Package package, String fileName)`

### `ParameterInfoGeneralType` (Namespace: `TFlex.DOCs.Model.Utils`)
**Свойства:** AllParameterInfoGeneralTypes: ReadOnlyCollection`1, ExtendedParameterInfoGeneralTypes: ReadOnlyCollection`1, Name: String
**Методы:**
- `ParameterInfoGeneralType GetParameterGeneralType(ParameterType parameterType)`
- `List`1 GetCompatibleTypes(ParameterInfoGeneralType generalType)`

### `ParameterInfoGeneralTypeExtensions` (Namespace: `TFlex.DOCs.Model.Utils`)
**Методы:**
- `Int32 GetId(ParameterInfoGeneralType type)`
- `ParameterInfoGeneralType GetGeneralType(Int32 id)`

### `ServerConnectionDictionary`1` (Namespace: `TFlex.DOCs.Model.Utils`)
**Свойства:** Keys: ICollection`1, Values: ICollection`1, Item: T, Count: Int32, IsReadOnly: Boolean
**Методы:**
- `T Get(ServerConnection connection)`
- `T GetOrAdd(ServerConnection connection, Func`1 valueFactory)`
- `Void Add(ServerConnection connection, T value)`
- `Boolean ContainsKey(ServerConnection connection)`
- `Boolean Remove(ServerConnection connection)`
- `Boolean TryGetValue(ServerConnection connection, T& value)`
- `Void Clear()`
- `IEnumerator`1 GetEnumerator()`

### `WorkingFolderInfo` (Namespace: `TFlex.DOCs.Model.WorkingFolder`)
**Свойства:** Id: Int32, WorkingFolder: String, PendingWorkingFolder: String, PendingStatus: WorkingFolderPendingStatus, Changing: Boolean, IsModified: Boolean
**Методы:**
- `Void BeginChanges()`
- `Void CancelChanges()`
- `List`1 GetClientWorkingFolders(ServerConnection connection, IEnumerable`1 clientViews)` [has Async]
- `Void EndChanges(IEnumerable`1 workingFolders)` [has Async]

### `WorkingFolderPendingStatusExtensions` (Namespace: `TFlex.DOCs.Model.WorkingFolder`)
**Методы:**
- `String GetName(WorkingFolderPendingStatus status)`
- `Byte[] GetIconBytes(WorkingFolderPendingStatus status)`
- `String GetIconKey(WorkingFolderPendingStatus status)`

### `ProjectMailTask` (Namespace: `TFlex.DOCs.Projects.Objects.Mail`)
**Свойства:** CanReject: Boolean, Responsible: User, Project: ProjectReferenceObject, CopyTo: User[]
**Методы:**
- `Void Send()`

### `ProjectMailTaskData` (Namespace: `TFlex.DOCs.Projects.Objects.Mail.Data`)
**Свойства:** Task: ProjectMailTask, ResponsibleGuid: Guid, Responsible: User, ProjectGuid: Guid, Project: ProjectReferenceObject, CopyTo: List`1
**Методы:**
- `User[] GetCopyTo()`
- `Void SetCopyTo(User[] users)`

### `ApplicationRelationReference` (Namespace: `TFlex.DOCs.References.ApplicationRelation`)
**Свойства:** Classes: ApplicationRelationTypes

### `ApplicationRelationReferenceObject` (Namespace: `TFlex.DOCs.References.ApplicationRelation`)
**Свойства:** Class: ApplicationRelationType, Name: StringParameter, ApplicationParameter: StringParameter, DOCsParameterType: Int32Parameter, DOCsParameter: StringParameter, DOCsLink: StringParameter, DOCsParameterPath: StringParameter, ApplicationParameterType: StringParameter, TypeRelation: Int32Parameter

### `ApplicationRelationType` (Namespace: `TFlex.DOCs.References.ApplicationRelation`)
**Свойства:** Classes: ApplicationRelationTypes, IsApplicationRelationType: Boolean

### `ApplicationRelationTypes` (Namespace: `TFlex.DOCs.References.ApplicationRelation`)
**Свойства:** ApplicationRelationType: ApplicationRelationType

### `TypesRelationsReference` (Namespace: `TFlex.DOCs.References.ApplicationRelation`)
**Свойства:** Classes: TypesRelationsTypes

### `TypesRelationsReferenceObject` (Namespace: `TFlex.DOCs.References.ApplicationRelation`)
**Свойства:** Class: TypesRelationsType, Name: StringParameter, AppTypeName: StringParameter, NomenclatureType: StringParameter, LinkToFiles: StringParameter, ReferenceObjectType: Guid, PathForLinkToFiles: StringParameter

### `TypesRelationsType` (Namespace: `TFlex.DOCs.References.ApplicationRelation`)
**Свойства:** Classes: TypesRelationsTypes, IsTypesRelationsClass: Boolean

### `TypesRelationsTypes` (Namespace: `TFlex.DOCs.References.ApplicationRelation`)
**Свойства:** TypesRelationsClass: TypesRelationsType

### `ApplicationsRelationsProfileReference` (Namespace: `TFlex.DOCs.References.ApplicationsRelationsProfile`)
**Свойства:** Guid: Guid, CurrentProflie: ApplicationsRelationsProfileReferenceObject, Classes: ApplicationsRelationsProfileTypes

### `ApplicationsRelationsProfileReferenceObject` (Namespace: `TFlex.DOCs.References.ApplicationsRelationsProfile`)
**Свойства:** Class: ApplicationsRelationsProfileType, Name: StringParameter, AppCode: StringParameter, LocalFilePaths: StringParameter, LoadingFolderPaths: StringParameter, DisallowEditStructure: BooleanParameter, ReferenceForDataExchange: Guid, UseSimplifiedRepresentations: SimplifiedRepresentationState, ApplicationsRelations: ReferenceObjectCollection, TypeRelations: ReferenceObjectCollection, AssocParameters: ReferenceObjectCollection, EnvironmentSettingsFile: FileObject
**Методы:**
- `ReferenceObject AddApplicationRelation(Guid listObjectClass) (+1)`

### `ApplicationsRelationsProfileType` (Namespace: `TFlex.DOCs.References.ApplicationsRelationsProfile`)
**Свойства:** Classes: ApplicationsRelationsProfileTypes, IsApplicationsRelationsProfileType: Boolean

### `ApplicationsRelationsProfileTypes` (Namespace: `TFlex.DOCs.References.ApplicationsRelationsProfile`)
**Свойства:** ApplicationsRelationsProfileType: ApplicationsRelationsProfileType

### `CachingFileServerObject` (Namespace: `TFlex.DOCs.References.CachingFileServer`)
**Свойства:** Class: CachingFileServerType, Name: StringParameter, ServerAddress: StringParameter, Users: ReferenceObjectCollection`1

### `CachingFileServerReference` (Namespace: `TFlex.DOCs.References.CachingFileServer`)
**Свойства:** Classes: CachingFileServerTypes

### `CachingFileServerType` (Namespace: `TFlex.DOCs.References.CachingFileServer`)
**Свойства:** Classes: CachingFileServerTypes, IsCachingFileServer: Boolean

### `CachingFileServerTypes` (Namespace: `TFlex.DOCs.References.CachingFileServer`)
**Свойства:** CachingFileServer: CachingFileServerType

### `CadWorkSessionsReference` (Namespace: `TFlex.DOCs.References.CadWorkSessions`)
**Свойства:** Classes: CadWorkSessionsTypes
**Методы:**
- `String CreateWorkSessionName(ReferenceObject referenceObject)`

### `CadWorkSessionsType` (Namespace: `TFlex.DOCs.References.CadWorkSessions`)
**Свойства:** Classes: CadWorkSessionsTypes, IsWorkSessionElementReferenceObject: Boolean, IsWorkSessionReferenceObject: Boolean

### `CadWorkSessionsTypes` (Namespace: `TFlex.DOCs.References.CadWorkSessions`)
**Свойства:** WorkSessionElementReferenceObject: CadWorkSessionsType, WorkSessionReferenceObject: CadWorkSessionsType

### `WorkSessionElementReferenceObject` (Namespace: `TFlex.DOCs.References.CadWorkSessions`)
**Свойства:** Class: CadWorkSessionsType, Name: StringParameter
**Методы:**
- `ReferenceObject AddReferenceObject(ReferenceObject newLinkedObject)` [has Async]
- `Boolean RemoveReferenceObject(ReferenceObject linkedObject)` [has Async]
- `Boolean ReferenceObjectIsContains(ReferenceObject linkedObject)` [has Async]

### `WorkSessionInstanceParameters` (Namespace: `TFlex.DOCs.References.CadWorkSessions`)
**Свойства:** Reference: Guid, ReferenceObject: Guid, ReferenceObjectInstance: Guid

### `WorkSessionObjectPath` (Namespace: `TFlex.DOCs.References.CadWorkSessions`)
**Свойства:** ObjectId: Int32, PathToParent: List`1

### `WorkSessionOpenParameters` (Namespace: `TFlex.DOCs.References.CadWorkSessions`)
**Свойства:** ReferenceObjectInstance: List`1, ParentPaths: List`1

### `WorkSessionReferenceObject` (Namespace: `TFlex.DOCs.References.CadWorkSessions`)
**Свойства:** StructureSettings: ByteArrayParameter, CadSettings: ByteArrayParameter, ActionCloseInCAD: Int32Parameter, MethodBeforeGetStructure: StringParameter, SessionFile: ReferenceObject, Context: ReferenceObject, DataModel: DataModelReferenceObject, Macro: Macro
**Методы:**
- `WorkSessionOpenParameters GetSessionOpenParameters()`
- `Void SetSessionOpenParameters(WorkSessionOpenParameters value)`

### `CategoryCriteria` (Namespace: `TFlex.DOCs.References.Configurators`)
**Методы:**
- `Object GetValueFromConfigurationSettings(ConfigurationSettings configurationSettings, Boolean& apply)`
- `Void ApplyToConfigurationSettings(ConfigurationSettings configurationSettings, Object value, Boolean apply)`

### `ConfigurationCriteria` (Namespace: `TFlex.DOCs.References.Configurators`)
**Свойства:** Class: ConfigurationCriteriaType, Name: StringParameter, DefaultValue: StringParameter, IsHidden: BooleanParameter, IsReadOnly: BooleanParameter, IsRequired: BooleanParameter, AllowManualInput: BooleanParameter
**Методы:**
- `Void ApplyToConfigurationSettings(ConfigurationSettings configurationSettings, Object value, Boolean apply)`
- `Object GetValueFromConfigurationSettings(ConfigurationSettings configurationSettings, Boolean& apply)`
- `Boolean DefaultValueIsNotNull()`
- `Object GetDefaultValue()` [has Async]
- `Void SetDefaultValue(Object value)` [has Async]

### `ConfigurationCriteriasReference` (Namespace: `TFlex.DOCs.References.Configurators`)
**Свойства:** Classes: ConfigurationCriteriasTypes

### `ConfigurationCriteriasTypes` (Namespace: `TFlex.DOCs.References.Configurators`)
**Свойства:** ConfigurationCriteria: ConfigurationCriteriaType, CustomCriteria: ConfigurationCriteriaType, SelectRevisionsFilterCriteria: ConfigurationCriteriaType, SystemCriteria: ConfigurationCriteriaType, ProductMilestoneCriteria: ConfigurationCriteriaType, DateCriteria: ConfigurationCriteriaType, ProductCriteria: ConfigurationCriteriaType, DesignContextCriteria: ConfigurationCriteriaType, ProductConfigurationCriteria: ConfigurationCriteriaType, OptionsCriteria: ConfigurationCriteriaType, SerialNumberCriteria: ConfigurationCriteriaType, StructureTypeCriteria: ConfigurationCriteriaType, CategoryTypeCriteria: ConfigurationCriteriaType, StructureVariantCriteria: ConfigurationCriteriaType

### `ConfigurationCriteriaType` (Namespace: `TFlex.DOCs.References.Configurators`)
**Свойства:** Classes: ConfigurationCriteriasTypes, IsConfigurationCriteria: Boolean, IsCustomCriteria: Boolean, IsSelectRevisionsFilterCriteria: Boolean, IsSystemCriteria: Boolean, IsProductMilestoneCriteria: Boolean, IsDateCriteria: Boolean, IsProductCriteria: Boolean, IsDesignContextCriteria: Boolean, IsProductDesignNumberCriteria: Boolean, IsOptionsCriteria: Boolean, IsSerialNumberCriteria: Boolean, IsStructureTypeCriteria: Boolean, IsCategoryTypeCriteria: Boolean, IsStructureVariantCriteria: Boolean

### `ConfigurationFilterTerm` (Namespace: `TFlex.DOCs.References.Configurators`)
**Свойства:** Name: StringParameter, ReferenceGroup: GuidParameter, LinkGroup: GuidParameter, Term: StringParameter

### `ConfigurationTerm` (Namespace: `TFlex.DOCs.References.Configurators`)
**Свойства:** Class: ConfigurationTermType

### `ConfigurationTermsReference` (Namespace: `TFlex.DOCs.References.Configurators`)
**Свойства:** Classes: ConfigurationTermsTypes

### `ConfigurationTermsTypes` (Namespace: `TFlex.DOCs.References.Configurators`)
**Свойства:** ConfigurationTerms: ConfigurationTermType, FilterTerm: ConfigurationTermType

### `ConfigurationTermType` (Namespace: `TFlex.DOCs.References.Configurators`)
**Свойства:** Classes: ConfigurationTermsTypes, IsConfigurationTerms: Boolean, IsFilterTerm: Boolean

### `ConfigurationVariable` (Namespace: `TFlex.DOCs.References.Configurators`)
**Свойства:** Variable: Variable, VariableName: String, VariableType: Type, IsArray: Boolean, AllowNullValue: Boolean, IsNull: Boolean, ReferenceGuid: Guid, DefaultObjectValue: Object, PossibleObjectValues: IEnumerable`1, Class: ConfigurationVariableType, Alias: StringParameter, DefaultValue: StringParameter, Data: StringParameter, PossibleVariableValues: StringParameter

### `ConfigurationVariablesReference` (Namespace: `TFlex.DOCs.References.Configurators`)
**Свойства:** Classes: ConfigurationVariablesTypes

### `ConfigurationVariablesTypes` (Namespace: `TFlex.DOCs.References.Configurators`)
**Свойства:** ConfigurationVariable: ConfigurationVariableType

### `ConfigurationVariableType` (Namespace: `TFlex.DOCs.References.Configurators`)
**Свойства:** Classes: ConfigurationVariablesTypes, IsConfigurationVariable: Boolean

### `Configurator` (Namespace: `TFlex.DOCs.References.Configurators`)
**Свойства:** Class: ConfiguratorType, Name: StringParameter, Description: StringParameter, ConfigurationCriterias: ReferenceObjectCollection`1
**Методы:**
- `Void LoadCriterias()` [has Async]
- `ConfigurationSettings GetDefaultConfigurationSettings()` [has Async]
- `ConfigurationCriteria CreateConfigurationCriteria(Guid listObjectClass) (+1)`

### `ConfiguratorsReference` (Namespace: `TFlex.DOCs.References.Configurators`)
**Свойства:** DefaultConfigurator: Configurator, Classes: ConfiguratorsTypes

### `ConfiguratorsTypes` (Namespace: `TFlex.DOCs.References.Configurators`)
**Свойства:** Configurator: ConfiguratorType

### `ConfiguratorType` (Namespace: `TFlex.DOCs.References.Configurators`)
**Свойства:** Classes: ConfiguratorsTypes, IsConfigurator: Boolean

### `CustomCriteria` (Namespace: `TFlex.DOCs.References.Configurators`)
**Свойства:** PossibleValues: ReferenceObjectCollection`1, Code: StringParameter
**Методы:**
- `Void CopyDefaultValue(CustomCriteria source)`
- `Void ApplyToConfigurationSettings(ConfigurationSettings configurationSettings, Object value, Boolean apply)`
- `Object GetValueFromConfigurationSettings(ConfigurationSettings configurationSettings, Boolean& apply)`
- `PossibleValue AddPossibleValue(Guid listObjectClass) (+1)`

### `DateCriteria` (Namespace: `TFlex.DOCs.References.Configurators`)
**Методы:**
- `Void ApplyToConfigurationSettings(ConfigurationSettings configurationSettings, Object value, Boolean apply)`
- `Object GetValueFromConfigurationSettings(ConfigurationSettings configurationSettings, Boolean& apply)`

### `DesignContextCriteria` (Namespace: `TFlex.DOCs.References.Configurators`)
**Методы:**
- `Void ApplyToConfigurationSettings(ConfigurationSettings configurationSettings, Object value, Boolean apply)`
- `Object GetValueFromConfigurationSettings(ConfigurationSettings configurationSettings, Boolean& apply)`

### `OptionsCriteria` (Namespace: `TFlex.DOCs.References.Configurators`)
**Методы:**
- `Void ApplyToConfigurationSettings(ConfigurationSettings configurationSettings, Object value, Boolean apply)`
- `Object GetValueFromConfigurationSettings(ConfigurationSettings configurationSettings, Boolean& apply)`
- `String Serialize(List`1 optionValues)`

### `PossibleValue` (Namespace: `TFlex.DOCs.References.Configurators`)
**Свойства:** Class: PossibleValueType, Name: StringParameter, ConfigurationTerms: ReferenceObjectCollection`1, ConfigurationVariables: ReferenceObjectCollection`1
**Методы:**
- `Task GenerateVariables(CancellationToken cancellationToken)`
- `ConfigurationTerm CreateConfigurationTerm(Guid listObjectClass) (+1)`
- `ConfigurationVariable CreateConfigurationVariable(Guid listObjectClass) (+1)`

### `PossibleValuesReference` (Namespace: `TFlex.DOCs.References.Configurators`)
**Свойства:** Classes: PossibleValuesTypes

### `PossibleValuesTypes` (Namespace: `TFlex.DOCs.References.Configurators`)
**Свойства:** PossibleValue: PossibleValueType

### `PossibleValueType` (Namespace: `TFlex.DOCs.References.Configurators`)
**Свойства:** Classes: PossibleValuesTypes, IsPossibleValue: Boolean

### `ProductCriteria` (Namespace: `TFlex.DOCs.References.Configurators`)
**Методы:**
- `Void ApplyToConfigurationSettings(ConfigurationSettings configurationSettings, Object value, Boolean apply)`
- `Object GetValueFromConfigurationSettings(ConfigurationSettings configurationSettings, Boolean& apply)`

### `ProductDesignNumberCriteria` (Namespace: `TFlex.DOCs.References.Configurators`)
**Методы:**
- `Void ApplyToConfigurationSettings(ConfigurationSettings configurationSettings, Object value, Boolean apply)`
- `Object GetValueFromConfigurationSettings(ConfigurationSettings configurationSettings, Boolean& apply)`

### `ProductMilestoneCriteria` (Namespace: `TFlex.DOCs.References.Configurators`)
**Методы:**
- `Void ApplyToConfigurationSettings(ConfigurationSettings configurationSettings, Object value, Boolean apply)`
- `Object GetValueFromConfigurationSettings(ConfigurationSettings configurationSettings, Boolean& apply)`

### `SelectRevisionsFilterCriteria` (Namespace: `TFlex.DOCs.References.Configurators`)
**Методы:**
- `Void ApplyToConfigurationSettings(ConfigurationSettings configurationSettings, Object value, Boolean apply)`
- `Object GetValueFromConfigurationSettings(ConfigurationSettings configurationSettings, Boolean& apply)`

### `SerialNumberCriteria` (Namespace: `TFlex.DOCs.References.Configurators`)
**Методы:**
- `Void ApplyToConfigurationSettings(ConfigurationSettings configurationSettings, Object value, Boolean apply)`
- `Object GetValueFromConfigurationSettings(ConfigurationSettings configurationSettings, Boolean& apply)`

### `StructureTypeCriteria` (Namespace: `TFlex.DOCs.References.Configurators`)
**Методы:**
- `Boolean DefaultValueIsNotNull()`
- `Object GetDefaultValue()` [has Async]
- `Void ApplyToConfigurationSettings(ConfigurationSettings configurationSettings, Object value, Boolean apply)`
- `Object GetValueFromConfigurationSettings(ConfigurationSettings configurationSettings, Boolean& apply)`

### `StructureVariantCriteria` (Namespace: `TFlex.DOCs.References.Configurators`)
**Методы:**
- `Object GetValueFromConfigurationSettings(ConfigurationSettings configurationSettings, Boolean& apply)`
- `Void ApplyToConfigurationSettings(ConfigurationSettings configurationSettings, Object value, Boolean apply)`

### `ContactReferenceObject` (Namespace: `TFlex.DOCs.References.Contacts`)
**Свойства:** Class: ContactType, LastName: StringParameter, Name: StringParameter, MiddleName: StringParameter, Job: StringParameter, Title: StringParameter, FullName: StringParameter, Department: StringParameter, Sex: StringParameter, Birthday: DateTimeParameter, Family: StringParameter, SpouseName: StringParameter, Description: StringParameter, WorkPhone1: StringParameter, WorkPhone2: StringParameter, MobilePhone: StringParameter, HomePhone: StringParameter, Fax: StringParameter, Email: StringParameter, Web: StringParameter, Phone: StringParameter, Addressee: StringParameter, AddresseePhone: StringParameter, Address: StringParameter, MailIndex: StringParameter, FullAddress: StringParameter, OwnerID: Int32Parameter, ContactCity: ReferenceObject, ContactCustomerAgreements: ReferenceObjectCollection, ContactExecutorAgreements: ReferenceObjectCollection, ActionContact: ReferenceObjectCollection, CompanyContacts: ReferenceObject
**Методы:**
- `ReferenceObject BeginChanges(ClassObject newClass)` [has Async]
- `ReferenceObject AddContactCustomerAgreements(ReferenceObject newLinkedObject)`
- `Boolean RemoveContactCustomerAgreements(ReferenceObject linkedObject)`
- `ReferenceObject AddContactExecutorAgreements(ReferenceObject newLinkedObject)`
- `Boolean RemoveContactExecutorAgreements(ReferenceObject linkedObject)`
- `ReferenceObject AddActionContact(ReferenceObject newLinkedObject)`
- `Boolean RemoveActionContact(ReferenceObject linkedObject)`

### `ContactsReference` (Namespace: `TFlex.DOCs.References.Contacts`)
**Свойства:** Classes: ContactsTypes

### `ContactsTypes` (Namespace: `TFlex.DOCs.References.Contacts`)
**Свойства:** Contact: ContactType, PrivateContact: ContactType

### `ContactType` (Namespace: `TFlex.DOCs.References.Contacts`)
**Свойства:** Classes: ContactsTypes, IsContact: Boolean, IsPrivateContact: Boolean

### `CoordinatesReference` (Namespace: `TFlex.DOCs.References.Coordinates`)
**Свойства:** Classes: CoordinatesTypes

### `CoordinatesReferenceObject` (Namespace: `TFlex.DOCs.References.Coordinates`)
**Свойства:** X_MAX: DoubleParameter, X_MIN: DoubleParameter, Placement: StringParameter, Z_MIN: DoubleParameter, Y_MAX: DoubleParameter, Y_MIN: DoubleParameter, Z_MAX: DoubleParameter, StructureType: Int32Parameter

### `CoordinatesType` (Namespace: `TFlex.DOCs.References.Coordinates`)
**Свойства:** Classes: CoordinatesTypes, IsCoordinatesReferenceObject: Boolean

### `CoordinatesTypes` (Namespace: `TFlex.DOCs.References.Coordinates`)
**Свойства:** CoordinatesReferenceObject: CoordinatesType

### `EasyFilterObject` (Namespace: `TFlex.DOCs.References.EasyFilters`)
**Свойства:** Class: EasyFilterType, Name: StringParameter, Path: StringParameter
**Методы:**
- `ReferencePath GetReferencePath()`

### `EasyFilterReference` (Namespace: `TFlex.DOCs.References.EasyFilters`)
**Свойства:** Classes: EasyFilterTypes

### `EasyFilterSetObject` (Namespace: `TFlex.DOCs.References.EasyFilters`)
**Свойства:** ReferenceGroup: ParameterGroup, Class: EasyFilterSetType, TargetReference: GuidParameter, ObjectList: StringParameter, EasyFilterObjects: ReferenceObjectCollection
**Методы:**
- `ReferenceObject CreateEasyFilter(Guid listObjectClass) (+1)`

### `EasyFilterSetReference` (Namespace: `TFlex.DOCs.References.EasyFilters`)
**Свойства:** Classes: EasyFilterSetTypes
**Методы:**
- `List`1 Find(ParameterGroup referenceGroup)`
- `List`1 GetEasyFilterObjects(ParameterGroup referenceGroup)`

### `EasyFilterSetType` (Namespace: `TFlex.DOCs.References.EasyFilters`)
**Свойства:** Classes: EasyFilterSetTypes, IsEasyFilterSetObject: Boolean

### `EasyFilterSetTypes` (Namespace: `TFlex.DOCs.References.EasyFilters`)
**Свойства:** EasyFilterSetObject: EasyFilterSetType

### `EasyFilterType` (Namespace: `TFlex.DOCs.References.EasyFilters`)
**Свойства:** Classes: EasyFilterTypes, IsEasyFilterObject: Boolean

### `EasyFilterTypes` (Namespace: `TFlex.DOCs.References.EasyFilters`)
**Свойства:** EasyFilterObject: EasyFilterType

### `InstanceConversionServiceReferenceObject` (Namespace: `TFlex.DOCs.References.instanceConversionServices`)
**Свойства:** Class: InstanceConversionServicesType, Name: StringParameter, ClientTFlexDOCs: StringParameter, CreateFileVersion: BooleanParameter, StopService: BooleanParameter, Comment: StringParameter

### `InstanceConversionServicesReference` (Namespace: `TFlex.DOCs.References.instanceConversionServices`)
**Свойства:** Classes: InstanceConversionServicesTypes

### `InstanceConversionServicesType` (Namespace: `TFlex.DOCs.References.instanceConversionServices`)
**Свойства:** Classes: InstanceConversionServicesTypes
**Методы:**
- `Boolean GetIsInstanceConversionServiceReferenceObject()`

### `InstanceConversionServicesTypes` (Namespace: `TFlex.DOCs.References.instanceConversionServices`)
**Свойства:** InstanceConversionServiceReferenceObject: InstanceConversionServicesType

### `MacroRunHistoryReference` (Namespace: `TFlex.DOCs.References.MacroRunHistory`)
**Свойства:** Classes: MacroRunHistoryTypes

### `MacroRunHistoryReferenceObject` (Namespace: `TFlex.DOCs.References.MacroRunHistory`)
**Свойства:** Class: MacroRunHistoryType, StartTime: DateTime, EndTime: DateTime, Duration: Int64, User: String, Host: String, ErrorText: String, EntryPoint: String, Macro: Macro

### `MacroRunHistoryType` (Namespace: `TFlex.DOCs.References.MacroRunHistory`)
**Свойства:** Classes: MacroRunHistoryTypes, IsRecord: Boolean

### `MacroRunHistoryTypes` (Namespace: `TFlex.DOCs.References.MacroRunHistory`)
**Свойства:** Record: MacroRunHistoryType

### `AbstractMarkReferenceObject` (Namespace: `TFlex.DOCs.References.MaterialMark`)
**Свойства:** Name: StringParameter, Mark: StringParameter, NTDType: Int32Parameter, NTDCode: StringParameter, Code: StringParameter, Description: StringParameter, IsCoating: BooleanParameter, OKPCode: StringParameter, Density: DoubleParameter, HeatConductivity: DoubleParameter, HeatCapacity: DoubleParameter, HeatExpansionLinear: DoubleParameter, HeatExpansionVolume: DoubleParameter, ElectroConductivity: DoubleParameter, KickViscosity: DoubleParameter, HB: DoubleParameter, HR: DoubleParameter, HV: DoubleParameter, SpinSoundness: DoubleParameter, BreakSoundness: DoubleParameter, DielectricSoundness: DoubleParameter

### `MaterialMarkReference` (Namespace: `TFlex.DOCs.References.MaterialMark`)
**Свойства:** Classes: MaterialMarkTypes

### `MaterialMarkReferenceObject` (Namespace: `TFlex.DOCs.References.MaterialMark`)
**Свойства:** Class: MaterialMarkType, PhysicalProperties: ReferenceObjectCollection, VisualProperties: MaterialsVisualPropertiesReferenceObject, MaterialInterChangeLink: ReferenceObjectCollection, MaterialIncompatibilityLink: ReferenceObjectCollection, MaterialWeldabilityLink: ReferenceObjectCollection, MaterialChemicalStructure: ReferenceObjectCollection, MaterialMarkLinksAssortment: ReferenceObjectCollection, MaterialLinksMark: ReferenceObjectCollection, PartMaterial: ReferenceObjectCollection
**Методы:**
- `ReferenceObject CreatePhysicalProperties(Guid listObjectClass) (+1)`
- `ReferenceObject AddMaterialInterChangeLink(ReferenceObject newLinkedObject)`
- `Boolean RemoveMaterialInterChangeLink(ReferenceObject linkedObject)`
- `ReferenceObject AddMaterialIncompatibilityLink(ReferenceObject newLinkedObject)`
- `Boolean RemoveMaterialIncompatibilityLink(ReferenceObject linkedObject)`
- `ReferenceObject AddMaterialWeldabilityLink(ReferenceObject newLinkedObject)`
- `Boolean RemoveMaterialWeldabilityLink(ReferenceObject linkedObject)`
- `ReferenceObject CreateMaterialChemicalStructure(Guid listObjectClass) (+1)`
- `ReferenceObject AddMaterialMarkLinksAssortment(ReferenceObject newLinkedObject)`
- `Boolean RemoveMaterialMarkLinksAssortment(ReferenceObject linkedObject)`
- `ReferenceObject AddMaterialLinksMark(ReferenceObject newLinkedObject)`
- `Boolean RemoveMaterialLinksMark(ReferenceObject linkedObject)`
- `ReferenceObject AddPartMaterial(ReferenceObject newLinkedObject)`
- `Boolean RemovePartMaterial(ReferenceObject linkedObject)`

### `MaterialMarkType` (Namespace: `TFlex.DOCs.References.MaterialMark`)
**Свойства:** Classes: MaterialMarkTypes, IsAbstractMaterialMark: Boolean, IsFolder: Boolean

### `MaterialMarkTypes` (Namespace: `TFlex.DOCs.References.MaterialMark`)
**Свойства:** MaterialMark: MaterialMarkType, Folder: MaterialMarkType, Metal: MaterialMarkType, Steel: MaterialMarkType, Alloys: MaterialMarkType, CastIron: MaterialMarkType, Polymer: MaterialMarkType

### `MaterialMarkPhysicalPropertiesReference` (Namespace: `TFlex.DOCs.References.MaterialMarkPhysicalProperties`)
**Свойства:** Classes: MaterialMarkPhysicalPropertiesTypes

### `MaterialMarkPhysicalPropertiesReferenceObject` (Namespace: `TFlex.DOCs.References.MaterialMarkPhysicalProperties`)
**Свойства:** Class: MaterialMarkPhysicalPropertiesType, Density: DoubleParameter, HeatConductivity: DoubleParameter, HeatCapacity: DoubleParameter, HeatExpansionLinear: DoubleParameter, HeatExpansionVolume: DoubleParameter, ElectroConductivity: DoubleParameter, KickViscosity: DoubleParameter, HB: DoubleParameter, HR: DoubleParameter, HV: DoubleParameter, SpinSoundness: DoubleParameter, BreakSoundness: DoubleParameter, DielectricSoundness: DoubleParameter, Condition: StringParameter, ThermalTreatment: StringParameter, Temperature: DoubleParameter, MeltingTemperature: DoubleParameter, YieldStrength: DoubleParameter, CompressionStrength: DoubleParameter, ElasticModulus: DoubleParameter, PoissonRatio: DoubleParameter

### `MaterialMarkPhysicalPropertiesType` (Namespace: `TFlex.DOCs.References.MaterialMarkPhysicalProperties`)
**Свойства:** Classes: MaterialMarkPhysicalPropertiesTypes, IsPhysicalProperties: Boolean

### `MaterialMarkPhysicalPropertiesTypes` (Namespace: `TFlex.DOCs.References.MaterialMarkPhysicalProperties`)
**Свойства:** PhysicalProperties: MaterialMarkPhysicalPropertiesType

### `MaterialsVisualPropertiesReference` (Namespace: `TFlex.DOCs.References.MaterialsVisualProperties`)
**Свойства:** Classes: MaterialsVisualPropertiesTypes

### `MaterialsVisualPropertiesReferenceObject` (Namespace: `TFlex.DOCs.References.MaterialsVisualProperties`)
**Свойства:** Class: MaterialsVisualPropertiesType, Name: StringParameter, AmbientColor: Int32Parameter, DiffuseColor: Int32Parameter, SpecularColor: Int32Parameter, EmissiveColor: Int32Parameter, Shininess: DoubleParameter, Reflection: DoubleParameter, Transparency: DoubleParameter, BumpType: Int32Parameter, PatternType: StringParameter, PatternScale: DoubleParameter, MaterialTexture: ReferenceObject, MaterialBumpTexture: ReferenceObject

### `MaterialsVisualPropertiesType` (Namespace: `TFlex.DOCs.References.MaterialsVisualProperties`)
**Свойства:** Classes: MaterialsVisualPropertiesTypes, IsMaterialMarkVisualProperties: Boolean

### `MaterialsVisualPropertiesTypes` (Namespace: `TFlex.DOCs.References.MaterialsVisualProperties`)
**Свойства:** MaterialMarkVisualProperties: MaterialsVisualPropertiesType

### `MessageTemplate` (Namespace: `TFlex.DOCs.References.MessageTemplates`)
**Свойства:** FormatType: FormatType, Class: MessageTemplateType, Name: StringParameter, Header: StringParameter, Body: StringParameter, TextFormat: Int32Parameter
**Методы:**
- `String FormatHeader(MacroContext context, IFormulaMacroCreator formulaCreator) (+1)`
- `ValueTuple`2 FormatHeaderWithExceptionsFixation(MacroContext context)`
- `String FormatBody(MacroContext context, IFormulaMacroCreator formulaCreator) (+1)`
- `ValueTuple`2 FormatBodyWithExceptionsFixation(MacroContext context)`
- `Object Calculate(String formula, MacroContext context)`
- `ValueTuple`2 CalculateWithExceptionFixation(String formula, MacroContext context)`
- `Object GetValue(String formula, MacroContext context)`
- `String FormatText(String text, MacroContext context, FormatType formatType, IFormulaMacroCreator formulaCreator) (+1)`
- `ValueTuple`2 FormatTextWithExceptionsFixation(String text, MacroContext context, FormatType formatType, IFormulaMacroCreator formulaCreator) (+1)`

### `MessageTemplateReference` (Namespace: `TFlex.DOCs.References.MessageTemplates`)
**Свойства:** Classes: MessageTemplateTypes

### `MessageTemplateType` (Namespace: `TFlex.DOCs.References.MessageTemplates`)
**Свойства:** Classes: MessageTemplateTypes, IsMessageTemplate: Boolean

### `MessageTemplateTypes` (Namespace: `TFlex.DOCs.References.MessageTemplates`)
**Свойства:** MessageTemplate: MessageTemplateType

### `FormulaProcessor` (Namespace: `TFlex.DOCs.References.MessageTemplates.FormulaProcessers`)
**Методы:**
- `String Replace(String text, MacroContext context, FormatType formatType, IFormulaMacroCreator formulaMacroCreator)`
- `ValueTuple`2 ReplaceWithExceptionsFixation(String text, MacroContext context, FormatType formatType, IFormulaMacroCreator formulaMacroCreator)`

### `ApplicabilityRecordsReference` (Namespace: `TFlex.DOCs.References.Nomenclature.ApplicabilityRecords`)
**Свойства:** Classes: ApplicabilityRecordsTypes

### `ApplicabilityRecordsReferenceObject` (Namespace: `TFlex.DOCs.References.Nomenclature.ApplicabilityRecords`)
**Свойства:** Class: ApplicabilityRecordsType, LinkedObjectID: Int32Parameter, ReferenceID: Int32Parameter, ProjectID: Int32Parameter, ProductID: Int32Parameter, Modification: StringParameter, StartMilestoneOfRange: Int32Parameter, EndMilestoneOfRange: Int32Parameter, StartNumberOfRange: Int32Parameter, EndNumberOfRange: Int32Parameter, StartActionDate: DateTimeParameter, EndActionDate: DateTimeParameter, ApplicabilityAction: ApplicabilityActionType, StructureVariantID: Int32Parameter, ApplicabilityGroup: ApplicabilityGroupType, UseNumberRanges: BooleanParameter

### `ApplicabilityRecordsType` (Namespace: `TFlex.DOCs.References.Nomenclature.ApplicabilityRecords`)
**Свойства:** Classes: ApplicabilityRecordsTypes, IsApplicabilityRecordsType: Boolean

### `ApplicabilityRecordsTypes` (Namespace: `TFlex.DOCs.References.Nomenclature.ApplicabilityRecords`)
**Свойства:** ApplicabilityRecordsType: ApplicabilityRecordsType

### `CategoriesReference` (Namespace: `TFlex.DOCs.References.Nomenclature.Categories`)
**Свойства:** Classes: CategoriesTypes
**Методы:**
- `CategoriesReferenceObject Find(String name)` [has Async]

### `CategoriesReferenceObject` (Namespace: `TFlex.DOCs.References.Nomenclature.Categories`)
**Свойства:** Class: CategoriesType, Name: StringParameter

### `CategoriesType` (Namespace: `TFlex.DOCs.References.Nomenclature.Categories`)
**Свойства:** Classes: CategoriesTypes, IsLinkCategory: Boolean

### `CategoriesTypes` (Namespace: `TFlex.DOCs.References.Nomenclature.Categories`)
**Свойства:** LinkCategory: CategoriesType

### `OptionRecordsReference` (Namespace: `TFlex.DOCs.References.Nomenclature.OptionRecords`)
**Свойства:** Classes: OptionRecordsTypes

### `OptionRecordsReferenceObject` (Namespace: `TFlex.DOCs.References.Nomenclature.OptionRecords`)
**Свойства:** Class: OptionRecordsType, LinkedObjectID: Int32Parameter, ReferenceID: Int32Parameter, ApplicabilityGroup: ApplicabilityGroupType, OptionCodes: StringParameter, OptionValueCodes: StringParameter

### `OptionRecordsType` (Namespace: `TFlex.DOCs.References.Nomenclature.OptionRecords`)
**Свойства:** Classes: OptionRecordsTypes, IsOptionRecordsType: Boolean

### `OptionRecordsTypes` (Namespace: `TFlex.DOCs.References.Nomenclature.OptionRecords`)
**Свойства:** OptionRecordsType: OptionRecordsType

### `ParametersAssocReference` (Namespace: `TFlex.DOCs.References.ParametersAssoc`)
**Свойства:** Classes: ParametersAssocTypes

### `ParametersAssocReferenceObject` (Namespace: `TFlex.DOCs.References.ParametersAssoc`)
**Свойства:** Class: ParametersAssocType, Name: StringParameter, ApplicationParameter: StringParameter, ApplicationParameterType: StringParameter, DOCsParameterDescription: StringParameter, DOCsParameterType: Int32Parameter, LinkDirection: Int32Parameter

### `ParametersAssocType` (Namespace: `TFlex.DOCs.References.ParametersAssoc`)
**Свойства:** Classes: ParametersAssocTypes, IsParametersAssociation: Boolean

### `ParametersAssocTypes` (Namespace: `TFlex.DOCs.References.ParametersAssoc`)
**Свойства:** ParametersAssociation: ParametersAssocType

### `PhysicalStructureObject` (Namespace: `TFlex.DOCs.References.PhysicalStructure`)
**Свойства:** Name: StringParameter, Date: DateTimeParameter, ProductStructureId: Int32Parameter
**Методы:**
- `Void SetProduct(NomenclatureObject product)`
- `Void Export()`

### `PhysicalStructureReference` (Namespace: `TFlex.DOCs.References.PhysicalStructure`)
**Свойства:** Classes: PhysicalStructureTypes

### `PhysicalStructureReferenceObject` (Namespace: `TFlex.DOCs.References.PhysicalStructure`)
**Свойства:** Class: PhysicalStructureType, PhysicalStructureFiles: ReferenceObjectCollection, PhysicalStructureFolder: ReferenceObject, Product: ReferenceObject
**Методы:**
- `ReferenceObject AddPhysicalStructureFiles(ReferenceObject newLinkedObject)`
- `Boolean RemovePhysicalStructureFiles(ReferenceObject linkedObject)`

### `PhysicalStructureType` (Namespace: `TFlex.DOCs.References.PhysicalStructure`)
**Свойства:** Classes: PhysicalStructureTypes, IsPhysicalStructure: Boolean, IsStructureElement: Boolean

### `PhysicalStructureTypes` (Namespace: `TFlex.DOCs.References.PhysicalStructure`)
**Свойства:** PhysicalStructure: PhysicalStructureType, StructureElement: PhysicalStructureType

### `StructureElementObject` (Namespace: `TFlex.DOCs.References.PhysicalStructure`)
**Свойства:** Name: StringParameter, Date: DateTimeParameter, ProductStructureId: Int32Parameter, Amount: DoubleParameter, Position: Int32Parameter, BOMSection: StringParameter, VariationDescription: StringParameter, Denotation: StringParameter
**Методы:**
- `Void SetProductProperties(NomenclatureReferenceObject nomRefObject, NomenclatureHierarchyLink link)`
- `XElement GetXElement()`

### `ProductStructureReference` (Namespace: `TFlex.DOCs.References.ProductStructure`)
**Свойства:** Classes: ProductStructureTypes
**Методы:**
- `Void InsertProductStructure(ProductStructureReferenceObject parentStructure, NomenclatureObject parentObject, ProductStructureReferenceObject structure)`
- `Void ReplaceByStructure(ProductStructureReferenceObject parentStructure, Guid hierarchyLinkId, ProductStructureReferenceObject replacementStructure)`

### `ProductStructureReferenceObject` (Namespace: `TFlex.DOCs.References.ProductStructure`)
**Свойства:** Class: ProductStructureType, Name: StringParameter, Processes: ReferenceObjectCollection, NomenclatureObject: NomenclatureObject
**Методы:**
- `ReferenceObject AddTP(ReferenceObject newLinkedObject)`
- `Boolean RemoveTP(ReferenceObject linkedObject)`

### `ProductStructureType` (Namespace: `TFlex.DOCs.References.ProductStructure`)
**Свойства:** Classes: ProductStructureTypes, IsProductStructureReferenceObject: Boolean, IsTechnologicalStructureReferenceObject: Boolean

### `ProductStructureTypes` (Namespace: `TFlex.DOCs.References.ProductStructure`)
**Свойства:** ProductStructure: ProductStructureType, TechnologicalStructure: ProductStructureType

### `ProjectStylesReference` (Namespace: `TFlex.DOCs.References.ProjectStyles`)
**Свойства:** Classes: ProjectStylesTypes

### `ProjectStylesReferenceObject` (Namespace: `TFlex.DOCs.References.ProjectStyles`)
**Свойства:** Class: ProjectStylesType, Name: StringParameter, TaskHeight: PercentParameter, ProgressHeight: PercentParameter, TaskShift: PercentParameter, ProgressShift: PercentParameter, TaskColor: Int32Parameter, ProgressColor: Int32Parameter, LineSkew: Boolean, LeftEndStyle: ImageReferenceObject, RightEndStyle: ImageReferenceObject

### `ProjectStylesType` (Namespace: `TFlex.DOCs.References.ProjectStyles`)
**Свойства:** Classes: ProjectStylesTypes, IsProjectStyle: Boolean

### `ProjectStylesTypes` (Namespace: `TFlex.DOCs.References.ProjectStyles`)
**Свойства:** ProjectStyle: ProjectStylesType

### `StructureTypesReference` (Namespace: `TFlex.DOCs.References.StructureTypes`)
**Свойства:** BaseStructureType: StructureTypesReferenceObject, Item: StructureTypesReferenceObject, AllStructureTypes: IList`1, Classes: StructureTypesTypes
**Методы:**
- `StructureTypesReferenceObject GetDefaultEditableStructureType(Guid designContextGuid)`
- `StructureTypesReferenceObject Find(String name)` [has Async]

### `StructureTypesReferenceObject` (Namespace: `TFlex.DOCs.References.StructureTypes`)
**Свойства:** Class: StructureTypesType, Name: StringParameter, Comment: StringParameter, IsDefault: BooleanParameter, ShareAccessForAllContexts: BooleanParameter, ParentStructure: StructureTypesReferenceObject
**Методы:**
- `Boolean ValidateEditByCurrentUser(Boolean throwOnError)`
- `Boolean ValidateEditByCurrentUserInContext(Guid designContextGuid, Boolean throwOnError)`
- `Boolean CanChangeParameter(Parameter p, Object newValue)`

### `StructureTypesType` (Namespace: `TFlex.DOCs.References.StructureTypes`)
**Свойства:** Classes: StructureTypesTypes, IsStructureType: Boolean

### `StructureTypesTypes` (Namespace: `TFlex.DOCs.References.StructureTypes`)
**Свойства:** StructureType: StructureTypesType

### `StructureVariantsReference` (Namespace: `TFlex.DOCs.References.StructureVariants`)
**Свойства:** BaseStructureVariant: StructureVariantsReferenceObject, Classes: StructureVariantsTypes

### `StructureVariantsReferenceObject` (Namespace: `TFlex.DOCs.References.StructureVariants`)
**Свойства:** IsBaseStructureVariant: Boolean, Class: StructureVariantsType, Name: StringParameter

### `StructureVariantsType` (Namespace: `TFlex.DOCs.References.StructureVariants`)
**Свойства:** Classes: StructureVariantsTypes, IsStructureVariant: Boolean

### `StructureVariantsTypes` (Namespace: `TFlex.DOCs.References.StructureVariants`)
**Свойства:** StructureVariant: StructureVariantsType

### `MasterFileReferenceObject` (Namespace: `TFlex.DOCs.References.TypicalRepresentations`)
**Свойства:** ByDefault: BooleanParameter
**Методы:**
- `List`1 GetDefaultSecondaryRepresentations()`
- `SecondaryRepresentationReferenceObject AddLinkDefaultRepresentation(SecondaryRepresentationReferenceObject secondaryRepresentation)`
- `Boolean RemoveLinkDefaultRepresentation(SecondaryRepresentationReferenceObject secondaryRepresentation)`

### `SecondaryRepresentationReferenceObject` (Namespace: `TFlex.DOCs.References.TypicalRepresentations`)
**Свойства:** LOD: Int32Parameter, FileExportParameters: ConversionFormatReferenceObject, AutoGenerationRepresentation: BooleanParameter, Disable: BooleanParameter, ConversionModule: FileConversionModuleReferenceObject

### `TypicalRepresentationsReference` (Namespace: `TFlex.DOCs.References.TypicalRepresentations`)
**Свойства:** Classes: TypicalRepresentationsTypes

### `TypicalRepresentationsReferenceObject` (Namespace: `TFlex.DOCs.References.TypicalRepresentations`)
**Свойства:** Class: TypicalRepresentationsType, Name: StringParameter, PresentationCode: StringParameter

### `TypicalRepresentationsType` (Namespace: `TFlex.DOCs.References.TypicalRepresentations`)
**Свойства:** Classes: TypicalRepresentationsTypes, IsTypicalRepresentationsReferenceObject: Boolean, IsSecondaryRepresentationReferenceObject: Boolean, IsMasterFileReferenceObject: Boolean

### `TypicalRepresentationsTypes` (Namespace: `TFlex.DOCs.References.TypicalRepresentations`)
**Свойства:** TypicalRepresentationsReferenceObject: TypicalRepresentationsType, SecondaryRepresentationReferenceObject: TypicalRepresentationsType, MasterFileReferenceObject: TypicalRepresentationsType

### `DailyObject` (Namespace: `TFlex.DOCs.References.WorkTime`)
**Методы:**
- `Boolean ContainsDate(DateTime date)`

### `DateWorkingTime` (Namespace: `TFlex.DOCs.References.WorkTime`)
**Методы:**
- `Boolean ContainsDate(DateTime date)`

### `IWorkingInterval` (Namespace: `TFlex.DOCs.References.WorkTime`)
**Свойства:** Duration: TimeSpan, StartTime: DateTime, EndTime: DateTime, StartSpan: TimeSpan, EndSpan: TimeSpan

### `IWorkTimeChange` (Namespace: `TFlex.DOCs.References.WorkTime`)
**Свойства:** IsWorkDay: Boolean, Date: DateTime, WorkTimeIntervals: IEnumerable`1

### `IWorkTimeObject` (Namespace: `TFlex.DOCs.References.WorkTime`)
**Свойства:** StartDate: DateTime, WorkTimeIntervals: IEnumerable`1, PriorityValue: Int32
**Методы:**
- `Boolean ContainsDate(DateTime date)`

### `MonthelyObject` (Namespace: `TFlex.DOCs.References.WorkTime`)
**Методы:**
- `Boolean ContainsDate(DateTime date)`

### `Periodical` (Namespace: `TFlex.DOCs.References.WorkTime`)
**Свойства:** PeriodValue: Int32, Period: Int32Parameter
**Методы:**
- `Boolean ContainsDate(DateTime date)`

### `WeeklyObject` (Namespace: `TFlex.DOCs.References.WorkTime`)
**Методы:**
- `Boolean ContainsDate(DateTime date)`

### `WorkingInterval` (Namespace: `TFlex.DOCs.References.WorkTime`)
**Свойства:** Duration: TimeSpan, StartTime: DateTime, EndTime: DateTime, StartSpan: TimeSpan, EndSpan: TimeSpan, Class: WorkingIntervalType, Name: StringParameter, Start: DateTimeParameter, End: DateTimeParameter, ChangeNumber: Int32Parameter

### `WorkingIntervalReference` (Namespace: `TFlex.DOCs.References.WorkTime`)
**Свойства:** Classes: WorkingIntervalTypes

### `WorkingIntervalType` (Namespace: `TFlex.DOCs.References.WorkTime`)
**Свойства:** Classes: WorkingIntervalTypes, IsWorkingInterval: Boolean

### `WorkingIntervalTypes` (Namespace: `TFlex.DOCs.References.WorkTime`)
**Свойства:** WorkingInterval: WorkingIntervalType

### `WorkTimeManager` (Namespace: `TFlex.DOCs.References.WorkTime`)
**Свойства:** IsEmpty: Boolean
**Методы:**
- `Void Refresh()`
- `IEnumerable`1 GetAllIntervals()`
- `Boolean IsIncludedInWorkingInterval(DateTime dateTime, Boolean planInDays)`
- `Boolean ExistWorkDaysLessThan(DateTime date)`
- `Boolean ExistWorkDaysMoreThan(DateTime date)`
- `List`1 GetWorkingIntervals(DateTime startTime, DateTime endTime, IWorkTimeChangeCollection changeCollection) (+2)`
- `TimeSpan GetRemainingTime(DateTime date)`
- `DateTime GetNextWorkTime(DateTime date)`
- `List`1 GetHolydayIntervals(TimeInterval interval)`
- `List`1 CalcHolydayIntervals(TimeInterval interval)`
- `List`1 GetUnworkingIntervals(TimeInterval interval)`
- `List`1 CalcUnworkingIntervals(TimeInterval resultInterval, IWorkTimeChangeCollection changeCollection) (+1)`
- `List`1 PlanTask(DateTime startTime, TimeSpan duration)`
- `DateTime CalcTaskStartTime(DateTime endTime, Int32 duration)`
- `DateTime CalculateTaskStartTime(DateTime endTime, TimeSpan duration)`
- `DateTime GetValidEndDate(DateTime dateTime)`
- `DateTimeInterval FromWorkingInterval(DateTime startTime, IWorkingInterval wInterval)`
- `WorkTimeManager Union(WorkTimeManager[] workTimeManagers)`
- `TimeSpan ConvertDaysToTimeSpan(DateTime startDate, Int32 days)`
- `Int32 ConvertTimeSpanToDays(DateTime startDate, TimeSpan timeSpan)`
- `DateTime CalcTaskEndTime(DateTime startDate, TimeSpan duration, IWorkTimeChangeCollection changeCollection) (+2)`
- `Int32 CalcDurationInDays(DateTime startTime, DateTime endTime)`
- `TimeSpan CalcTaskDuration(DateTime startDate, DateTime endDate, IWorkTimeChangeCollection changeCollection) (+1)`
- `IWorkTimeObject GetWorkTimeObject(DateTime date)`
- `DateTime GetNextWorkDayStart(DateTime date, IWorkTimeChangeCollection changeCollection) (+1)`
- `DateTime GetWorkDayStart(DateTime date)`
- `DateTime GetPreviousWorkDayEnd(DateTime date, IWorkTimeChangeCollection changeCollection) (+1)`
- `DateTime GetPreviousWorkTimeEnd(DateTime date)`
- `DateTime GetWorkDayEnd(DateTime date)`

### `WorkTimeReference` (Namespace: `TFlex.DOCs.References.WorkTime`)
**Свойства:** Classes: WorkTimeTypes

### `WorkTimeReferenceObject` (Namespace: `TFlex.DOCs.References.WorkTime`)
**Свойства:** DayOfWeekValue: Int32, DayOfMonthValue: Int32, MonthValue: Int32, YearValue: Int32, StartDateValue: DateTime, IsStartDateSetValue: Boolean, EndDateValue: DateTime, IsEndDateSetValue: Boolean, PriorityValue: Int32, Priority: Int32Parameter, Class: WorkTimeType, Name: StringParameter, DayOfWeek: Int32Parameter, DayOfMonth: Int32Parameter, Month: Int32Parameter, Year: Int32Parameter, StartDate: DateTime, IsStartDateSet: Boolean, EndDate: DateTime, IsEndDateSet: Boolean, WorkTimeIntervals: IEnumerable`1, Users: ReferenceObjectCollection, Equipment: ReferenceObjectCollection
**Методы:**
- `Boolean ContainsDate(DateTime date)`
- `ReferenceObject CreateWorkTimeInterval(Guid listObjectClass) (+1)`
- `ReferenceObject AddUser(ReferenceObject newLinkedObject)`
- `Boolean RemoveUser(ReferenceObject linkedObject)`
- `ReferenceObject AddEquipment(ReferenceObject newLinkedObject)`
- `Boolean RemoveEquipment(ReferenceObject linkedObject)`

### `WorkTimeType` (Namespace: `TFlex.DOCs.References.WorkTime`)
**Свойства:** Classes: WorkTimeTypes, IsWorkTime: Boolean, IsYearly: Boolean, IsDaily: Boolean, IsMonthely: Boolean, IsWeekl: Boolean, IsDate: Boolean, IsPeriodical: Boolean

### `WorkTimeTypes` (Namespace: `TFlex.DOCs.References.WorkTime`)
**Свойства:** WorkTime: WorkTimeType, Yearly: WorkTimeType, Daily: WorkTimeType, Monthely: WorkTimeType, Weekl: WorkTimeType, Date: WorkTimeType, Periodical: WorkTimeType

### `YearlyObject` (Namespace: `TFlex.DOCs.References.WorkTime`)
**Методы:**
- `Boolean ContainsDate(DateTime date)`

## Сборка: TFlex.DOCs.Common.dll

### `AccessCommandIdExtensions` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `Boolean IsViewsOrCatalogsReferenceCommand(AccessCommandId commandId)`
- `Boolean TryGetSystemViewsOrCatalogsCommand(AccessCommandId commandId, AccessCommandId& systemCommandId)`
- `AccessTypeID GetAccessType(AccessCommandId commandId, Int32 linkId)`

### `AccessCommandTypeEnumExtensions` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `String GetName(AccessCommandType type)`

### `AccessCommandTypeExtensions` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `AccessCommandId[] GetCommands(AccessCommandType type)`

### `AccessDirectionExtensions` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `String GetName(AccessDirection type)`

### `AccessDirectionHolder` (Namespace: `TFlex.DOCs.Common`)
**Свойства:** Value: AccessDirection
**Методы:**
- `AccessDirectionHolder GetHolder(AccessDirection value)`

### `AccessInheritedFromExtension` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `Boolean IsIdentical(AccessInheritedFrom value, AccessInheritedFrom other)`
- `Boolean SupportsAccessDirection(AccessInheritedFrom value, AccessInheritedFrom[] additional)`
- `String GetString(AccessInheritedFrom value)`

### `AccessInheritedFromExtensions` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `String GetName(AccessInheritedFrom type)`

### `AccessRightsHelper` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `Boolean CheckAccess(Int32 mode) (+1)`
- `Boolean CheckFromAtoB(Int32 mode) (+1)`
- `Boolean CheckFromBtoA(Int32 mode) (+1)`
- `Boolean CheckBiDirectional(Int32 mode) (+1)`
- `Boolean CheckAnyLinkedAccess(Int32 mode) (+1)`
- `Int32 GetAnyLinkedAccess(Int32 mode) (+1)`
- `Int32 SwapAnyLinkedAccess(Int32 mode) (+1)`
- `Boolean CheckWithParents(Int32 mode) (+1)`
- `Boolean CheckAnyAccess(Int32 mode) (+1)`
- `Int32 RemoveAnyLinkedAccess(Int32 mode)`

### `AccessRightsModeExtensions` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `String GetName(AccessRightsMode mode)`

### `AccessTypeIDExtensions` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `String GetName(AccessTypeID type)`

### `AccessTypeIdHelper` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `String GetTableName(AccessTypeID accessTypeID)`

### `Adler32` (Namespace: `TFlex.DOCs.Common`)
**Свойства:** Value: UInt32
**Методы:**
- `Void Reset()`
- `UInt32 CalculateChecksum(DirectoryInfo directory) (+2)`
- `Void Update(ReadOnlySpan`1 buffer)`

### `AssemblyBuildDateAttribute` (Namespace: `TFlex.DOCs.Common`)
**Свойства:** BuildDate: DateTime

### `BinarySerializerHelper` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `T OpenBinary(String fileName, DefragmentationMethod`1 defragmentationMethod, SerializationBinder serializationBinder) (+4)`
- `Void SaveBinary(String fileName, T data, FragmentationMethod`1 fragmentationMethod, SerializationBinder serializationBinder) (+4)`

### `BoolHelper` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `String AsReadable(Boolean value)`
- `String AsCheckable(Boolean value)`
- `Boolean ParseString(String value, Boolean defaultValue)`

### `CacheStringBuilder` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `StringBuilder GetBuilder()`
- `String GetString(StringBuilder builder)`

### `CollectionExtensions` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `IEnumerable`1 MultipleOrderBy(IEnumerable`1 source, String properties) (+1)`
- `Type GetGenericIEnumerableType(Object value)`

### `CompressionAlgorithmExtensions` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `Boolean IsNone(CompressionAlgorithm compression)`
- `String GetPrefix(CompressionAlgorithm compression)`
- `CompressionAlgorithm ParsePrefix(String compression)`
- `String GetName(CompressionAlgorithm compression)`

### `CurrentOperatingSystem` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `Boolean IsWindows()`
- `Boolean IsLinux()`
- `Int32 GetProcessId()`

### `DataFormatterSettings` (Namespace: `TFlex.DOCs.Common`)
**Свойства:** DefaultCommunicationMode: CommunicationMode, DefaultDataSerializerAlgorithm: DataSerializerAlgorithm, DefaultCompressionAlgorithm: CompressionAlgorithm, SupportingCommunicationMode: CommunicationMode, SupportingDataSerializerAlgorithms: DataSerializerAlgorithm[], SupportingCompressionAlgorithms: CompressionAlgorithm[]
**Методы:**
- `Void InitializeClientAppSettings(Assembly assembly)`
- `Void InitializeServerAppSettings(Assembly assembly)`

### `DataSerializerAlgorithmExtensions` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `Boolean IsDefault(DataSerializerAlgorithm dataSerializer)`
- `String GetPrefix(DataSerializerAlgorithm dataSerializer)`
- `DataSerializerAlgorithm ParsePrefix(String dataSerializer)`
- `String GetName(DataSerializerAlgorithm dataSerializer)`

### `diff_match_patch` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `List`1 diff_main(String text1, String text2, Boolean checklines) (+1)`
- `Int32 diff_commonPrefix(String text1, String text2)`
- `Int32 diff_commonSuffix(String text1, String text2)`
- `Void diff_cleanupSemantic(List`1 diffs)`
- `Void diff_cleanupSemanticLossless(List`1 diffs)`
- `Void diff_cleanupEfficiency(List`1 diffs)`
- `Void diff_cleanupMerge(List`1 diffs)`
- `Int32 diff_xIndex(List`1 diffs, Int32 loc)`
- `String diff_prettyHtml(List`1 diffs)`
- `String diff_text1(List`1 diffs)`
- `String diff_text2(List`1 diffs)`
- `Int32 diff_levenshtein(List`1 diffs)`
- `String diff_toDelta(List`1 diffs)`
- `List`1 diff_fromDelta(String text1, String delta)`
- `Int32 match_main(String text, String pattern, Int32 loc)`
- `List`1 patch_make(String text1, String text2, List`1 diffs) (+3)`
- `List`1 patch_deepCopy(List`1 patches)`
- `Object[] patch_apply(List`1 patches, String text)`
- `String patch_addPadding(List`1 patches)`
- `Void patch_splitMax(List`1 patches)`
- `String patch_toText(List`1 patches)`
- `List`1 patch_fromText(String textline)`
- `String encodeURI(String str)`

### `EnumDescriptionConverter` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `String GetEnumClassDescription(Enum value)`
- `String GetTypeDescription(Type type)`
- `String GetEnumDescription(Type value, String name) (+1)`
- `Object GetEnumValue(Type value, String description, Object defaultValue) (+1)`

### `EnumExtensions` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `Object Parse(Type enumType, String value, Int32 startIndex)`

### `FormatVersionValidator` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `Void ThrowIfDatabaseVersionWrong(Int32 databaseVersion, String productVersion)`

### `FrameworkInfoManager` (Namespace: `TFlex.DOCs.Common`)
**Свойства:** InstalledVersions: ReadOnlyCollection`1, LastInstalledVersion: Version, SupportDotNet45: Boolean
**Методы:**
- `Boolean CheckFrameworkVersion()`
- `String GetTargetFrameworkName()`
- `String GetUITargetFrameworkName()`
- `String GetArchitecture()`
- `String GetInfo()`

### `GarbageCollectorRunner` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `Void Collect()`
- `Void CollectSync()`

### `GZipHelper` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `Boolean IsCompressed(ArraySegment`1 buffer) (+1)`
- `Stream GetCompressStream(Stream stream)`
- `Stream GetDecompressStream(Stream stream)`
- `Byte[] Compress(Byte[] bytes) (+1)`
- `Byte[] Decompress(Byte[] bytes) (+1)`
- `ArraySegment`1 CompressBuffer(ArraySegment`1 buffer, BufferManager bufferManager, Int32 messageOffset)`
- `ArraySegment`1 DecompressBuffer(ArraySegment`1 buffer, BufferManager bufferManager)`
- `Void CompressFile(String fileName, String outFileName)`
- `Void CompressDirectory(String directory, String outFilePath, UInt32 checkshum)`
- `UInt32 ReadContentChecksum(String compressedFile)`
- `Void DecompressToDirectory(String compressedFile, String outPath)`

### `ICollectionExtensions` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `String JoinCollection(ICollection`1 collection, Func`2 convertToStringFn, String prefix, String separator, String leftBracket, String rightBracket) (+2)`
- `String JoinCollectionWithPrepareSQL(ICollection collection, String prefix, String separator, String leftBracket, String rightBracket)`
- `Boolean IsNullOrEmpty(ICollection`1 collection) (+1)`
- `Boolean IsNullOrEmptyReadOnlyCollection(IReadOnlyCollection`1 collection)`

### `IEnumerableExtensions` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `Boolean IsNullOrEmpty(IEnumerable collection)`
- `Boolean IsEmpty(IEnumerable collection)`
- `Boolean CountMoreThan(IEnumerable`1 enumerableCollection, Int32 checkCount)`
- `Object FirstOrDefaultEnumerable(IEnumerable collection)`
- `Boolean IdenticalItemsEqual(IEnumerable`1 source1, IEnumerable`1 source2, IComparer`1 comparer) (+1)`
- `IEnumerable`1 Concat(IEnumerable`1[] sequences)`
- `Int32 TakeCapacity(IEnumerable`1 source, Int32 defaultCapacity)`

### `IFixedForClone` (Namespace: `TFlex.DOCs.Common`)
**Свойства:** FixedForCloning: Boolean

### `IGuiOutputView` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `Void ShowOutputView()`
- `Void ActivateCategory(String category)`

### `IniFile` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `String Read(String filePath, String section, String key)`
- `Boolean Write(String filePath, String section, String key, String value)`

### `IOutputView` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `Void Clear()`
- `Void ClearCategory(String category)`
- `Void WriteString(String category, String text, DateTime date) (+1)`

### `IRequestResult` (Namespace: `TFlex.DOCs.Common`)
**Свойства:** Error: IRequestResultError

### `IRequestResultError` (Namespace: `TFlex.DOCs.Common`)
**Свойства:** Code: Int32, Data: List`1, IsDatabaseError: Boolean, IsInternalError: Boolean

### `ITrafficMeter` (Namespace: `TFlex.DOCs.Common`)
**Свойства:** BytesSent: Int64, BytesReceived: Int64, UnzippedBytesSent: Int64, UnzippedBytesReceived: Int64, SentPercentsZipped: Double, ReceivedPercentsZipped: Double

### `LevelSwitch` (Namespace: `TFlex.DOCs.Common`)
**Свойства:** DefaultLevel: SourceLevels

### `LinkedListExtensions` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `Void RemoveAll(LinkedList`1 linkedList, Func`2 predicate)`

### `LinqExtension` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `IEnumerable`1 Batch(IEnumerable`1 source, Int32 batchSize)`
- `IEnumerable`1 AsEnumerable(TSource source)`
- `TSource[] AsArray(TSource source)`

### `ListExensions` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `List`1 IListToList(IList`1 collection)`
- `List`1 ICollectionToList(ICollection`1 collection)`
- `List`1 IReadOnlyCollectionToList(IReadOnlyCollection`1 collection)`
- `PageEnumerable`1 SplitBy(IList`1 collection, Int32 pageSize) (+1)`
- `IList`1 GetRange(IList`1 collection, Int32 index, Int32 count) (+1)`
- `Boolean ContainsStringFrom(List`1 stringsList, List`1 comparedStrings, StringComparison comparison)`
- `T[] GetListArray(List`1 list)`
- `List`1 RemoveByCondition(List`1 list, Func`2 condition)`
- `Void EnsureCapacity(List`1 list, Int32 minCapacity)`
- `T AddOrReplace(IList`1 list, Predicate`1 predicate, T newObj)`
- `Int32 IndexOf(IList`1 list, Predicate`1 predicate)`

### `LocalizationExceptionsHelper` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `Boolean IsValidGuid(Guid guid)`

### `MemoryHelper` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `UInt64 GetAvailableSystemMemory()`
- `UInt64 GetTotalSystemMemory()`
- `UInt64 GetApplicationMemory()`
- `UInt64 GetTotalSystemMemoryWindows()`
- `Void GlobalMemoryStatusEx(MEMORYSTATUSEX& lpBuffer)`

### `NullableAttribute` (Namespace: `TFlex.DOCs.Common`)
**Свойства:** IsNullable: Boolean

### `ObjectsCompareHelper` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `Boolean CompareObjects(Object obj1, Object obj2)`
- `Boolean CompareLists(IList list1, IList list2) (+1)`
- `Boolean CompareDictionaries(IDictionary dict1, IDictionary dict2)`
- `Int32 CompareTo(Object value1, Object value2)`
- `Int32 CompareStrings(String value1, String value2, Boolean ignoreCase)`
- `Boolean EquivalentTo(String value1, String value2, Boolean ignoreCase)`
- `Int32 CompareWithDecimal(Double doubleValue, Decimal decimalValue) (+1)`

### `PageEnumerable`1` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `IEnumerator`1 GetEnumerator()`

### `Postprocessor` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `String Postprocess(Preprocessor preprocessor, String value)`

### `Preprocessor` (Namespace: `TFlex.DOCs.Common`)
**Свойства:** PreserveRemarks: Boolean
**Методы:**
- `Preprocessor Reset(Boolean withVariables, Boolean withReaders)`
- `Int32 IncludeVars(String variables)`
- `Int32 IncludeVarsFile(String varsFile, Encoding encoding)`
- `Preprocessor IncludeVarsFiles(Encoding encoding, String[] varsFiles)`
- `Preprocessor PreprocessFile(String fromFileName, String toFileName, Encoding encoding, Boolean postProcess) (+1)`
- `String Preprocess(String value, Dictionary`2 variables, Boolean postProcess) (+4)`
- `Object GetVariableValue(String variable, Object defaultValue)`
- `Object[] GetArrayVariableValue(String variable, Object defaultValue)`
- `Preprocessor DefineVariable(String variable, Object value)`
- `Preprocessor AppendVariable(String variable, Object value)`
- `Preprocessor UndefineVariable(String variable)`
- `Preprocessor DefineVariables(Object value, String[] variables)`
- `Preprocessor UndefineVariables(String[] variables)`
- `Boolean IsVariableDefined(String variable)`
- `Boolean IsArrayVariable(String variable)`
- `ItemType GetArrayVariableType(String variable)`
- `Void RegisterFunction(String functionName, Func`1 body, Boolean isGlobal) (+2)`
- `PreprocessorFuncResult ExecuteFunction(String function, Int32 lineNumber, String[] arguments)`
- `Void RegisterDirective(BaseDirectiveParser directiveParser)`

### `ProcessManager` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `Void OpenInApp(String filePath, String associatedProgramName, Boolean asAdmin) (+1)`
- `Void OpenApp(String name, Boolean asAdmin)`
- `Void OpenFolder(String folderPath)`
- `Void OpenFileFolder(String filePath)`
- `Void ExecuteNoWait(String fileName, String arguments) (+1)`
- `Process Start(String fileName, String arguments)`

### `ProductConfiguration` (Namespace: `TFlex.DOCs.Common`)
**Свойства:** RegistryKey: String, LocalMachineRegistryKey: String, ProductKey: String, TFlexCadProductName: String, TFlexCadLanguage: String, RegistryLanguageKeyName: String, UICulture: CultureInfo, DefaultClientConfiguration: Guid, BuildTime: DateTime
**Методы:**
- `T GetSetupDefaultValue(String name, T defaultValue)`
- `String GetDefaultProductName()`
- `String GetConfigRegistryName(String path)`
- `String GetSetupInstanceName()`
- `String GetSetupProductName()`
- `String GetSetupPlatformName()`
- `String GetApplicationServerServiceName()`
- `String GetFileServerServiceName()`
- `String GetCachingFileServerServiceName()`
- `String GetEventServiceName()`
- `String GetFileConversionServiceName()`
- `String GetUpdateClientConfigurationFileName()`
- `Void SetCurrentThreadCulture()`
- `Boolean IsPackageStream(Stream dataStream)`
- `Boolean CheckFrameworkVersion()`

### `PropertiesState`1` (Namespace: `TFlex.DOCs.Common`)
**Свойства:** Item: TPropertyType
**Методы:**
- `Void PutObject(IEnumerable`1 properties, TObjectType obj)`

### `ReaderLock` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `ReaderLock Lock(ReaderWriterLock readerWriterLock, TimeSpan timeout) (+1)`

### `ReferenceSettingsRestrictions` (Namespace: `TFlex.DOCs.Common`)
**Свойства:** SupportsRevisions: IReadOnlyDictionary`2, SupportsOrder: IReadOnlyDictionary`2, SupportsDesktop: IReadOnlyDictionary`2

### `RegexHelper` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `String RemoveHTML(String value)`
- `Regex ToRegex(String fileMask, Boolean ignoreCase)`
- `String ToRegexString(String fileMask)`
- `List`1 ToRegexList(List`1 fileMasks)`
- `Boolean IsMatch(List`1 regexMasks, String fileName)`

### `RegistryConfig` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `Void Load()`
- `Void Save()`

### `RegistryExtensions` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `T GetRegistryValue(RegistryKey settingsKey, String name, T defaultValue)`
- `Void SetRegistryValue(RegistryKey settingsKey, String name, T value)`
- `List`1 RecursiveEnumKeys(RegistryKey parent, Func`2 checkFn)`

### `RegistryManager` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `String GetAssociatedProgram(String filePath)`
- `String GetAssociatedProgramPath(String filePath)`
- `RegistryKey GetFileExtensionKey(String filePath)`

### `RtfHtmlManager` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `Boolean IsHtmlString(String text)`
- `Boolean IsRtfString(String text)`
- `String GetRtfUnicodeEscapedString(String value)`

### `RunParameter` (Namespace: `TFlex.DOCs.Common`)
**Свойства:** Key: String, AsArg: String
**Методы:**
- `String AsBoolArg(Boolean enabled)`

### `Service` (Namespace: `TFlex.DOCs.Common`)
**Свойства:** IsRunning: Boolean, Name: String
**Методы:**
- `Void InitServiceActionDefaultTimeout()`
- `String GetImagePath()`
- `Boolean IsCurrentVersion()`
- `Void Start(Int32 timeOut)`
- `Void Stop(Int32 timeOut)`

### `ServiceAuthorizationSettings` (Namespace: `TFlex.DOCs.Common`)
**Свойства:** AccountType: ServiceAccount, UserName: String, Password: String
**Методы:**
- `Void Save(RegistryKey key) (+1)`
- `Void Load(RegistryKey key) (+1)`

### `SimpleElement` (Namespace: `TFlex.DOCs.Common`)
**Свойства:** UniqueID: Int32, SupportsHTML: Boolean
**Методы:**
- `Void Clear()`
- `Void Assign(Object source)`
- `Int32 CompareTo(Object obj)`

### `SimpleFunctionsProcessor` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `Boolean IsCorrectFunctionName(Preprocessor preprocessor, String functionName)`
- `String Process(Preprocessor preprocessor, String line)`

### `SimpleLink` (Namespace: `TFlex.DOCs.Common`)
**Свойства:** LinkID: Int32, MasterGroupID: Int32, MasterID: Int32, SlaveGroupID: Int32, SlaveID: Int32, UniqueID: Int32
**Методы:**
- `Void Clear()`
- `Void Assign(Object source)`

### `SimpleObject` (Namespace: `TFlex.DOCs.Common`)
**Свойства:** ReferenceID: Int32, s_ObjectID: Int32, Name: String, s_Guid: Guid, UniqueID: Int32
**Методы:**
- `Void Clear()`
- `Void Assign(Object source)`

### `SimpleReference` (Namespace: `TFlex.DOCs.Common`)
**Свойства:** PK: Int32, TableName: String, Caption: String, UniqueID: Int32
**Методы:**
- `Void Clear()`
- `Void Assign(Object source)`

### `SimpleReferenceObject` (Namespace: `TFlex.DOCs.Common`)
**Свойства:** Reference: SimpleReference
**Методы:**
- `Void Clear()`
- `Void Assign(Object source)`

### `SimpleStage` (Namespace: `TFlex.DOCs.Common`)
**Свойства:** PK: Int32, Name: String, Comment: String, UniqueID: Int32
**Методы:**
- `Void Clear()`
- `Void Assign(Object source)`

### `SortDirection` (Namespace: `TFlex.DOCs.Common`)
**Свойства:** PropertyName: String, Direction: ListSortDirection

### `StatesDelta`1` (Namespace: `TFlex.DOCs.Common`)
**Свойства:** Item: TPropertyType
**Методы:**
- `Void PutDelta(IEnumerable`1 properties, TObjectType prevObject, TObjectType currentObject, Boolean clearCollection)`
- `Void Put(String property, TPropertyType prevValue, TPropertyType newValue)`

### `StaticParameterGroups` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `Boolean Contains(Guid reference)`

### `StringLegacyExtensions` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `String[] Split(String str, Char separator, StringSplitOptions options) (+1)`
- `Boolean Contains(String str, String value, StringComparison comparisonType) (+1)`
- `Boolean StartsWith(String str, Char value)`
- `Boolean EndsWith(String str, Char value)`
- `String SubstringWithConcat(String str, String str2, String str3, Int32 start, Int32 length) (+3)`
- `Boolean ValueEqual(Group group, String value)`
- `Boolean TryParseInt(String str, Int32 index, Int32& value)`

### `StringsHelper` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `String ExtractFrom(String line, String left, String right)`
- `Int32 ContainsCount(String value, String substring, StringComparison stringComparison)`
- `String GetWord(String line, Int32& startIndex, List`1 dividers)`
- `String TrimMax(String value, Int32 maxLen, String endString)`
- `String TrimStringWithCenterCrop(String stringToLimit, Int32 limit, String replaceString)`
- `String TrimStringWithEllipsis(String stringToLimit, Int32 limit, String replaceString)`
- `String CropWithServiceInfo(String stringToLimit, Int32 limit)`
- `List`1 SplitString(String value, Int32 maxLen)`
- `MemoryStream GenerateStreamFromString(String value, Encoding encoding) (+1)`
- `Byte[] GenerateBytesFromString(String value)`
- `String GetStringFromStream(Stream stream, Encoding encoding) (+1)` [has Async]
- `String GetStringFromStreamNoClose(Stream stream)` [has Async]
- `String GetStringFromOpenStream(Stream stream, Int32 length)` [has Async]
- `Boolean Exists(String value, String subString, Boolean caseSensitive, Boolean wholeWords) (+1)`
- `String GetSecondFileExtension(String fileName)`
- `Boolean IsNumericFileExtension(String fileName)`
- `Boolean IsDecimalNumeric(String value)`
- `Boolean IsSqlNumeric(String value)`
- `Boolean IsInQuotes(String value)`
- `Boolean IsBalancedParentheses(String input)`
- `String TrimBrackets(String value, Char leftBracket, Char rightBracket)`
- `Boolean IsInBrackets(String value)`
- `Boolean IsContainsCyrillic(String value)`
- `Boolean IsEMailString(String value)`
- `Boolean IsValidGuid(String text, Boolean checkIsNotEmpty)`
- `Boolean TryReplace(String& sourceString, String targetSubstring, String newSubstring)`
- `String RemoveSubstrings(String value, String left, String right)`
- `String RemoveSubstringsAndInsertHint(String value, String left, String right, String hint)`
- `String GetTypeName(Type type)`
- `String CheckTypeNameString(String typeNameString)`
- `String Normalize(String value, Boolean classStyle)`
- `IEnumerable`1 GetCommonSubstrings(String searchString, String[] strings)`
- `String[] SplitByNewLine(String value, StringSplitOptions options)`
- `Boolean IsMultiLine(String value)`
- `String Trim(String name)`
- `Boolean TryTakeRows(String& value, Int32 rows)`
- `String TrimEnd255(String value)`
- `String ParseFormatString(String formatString)`

### `StringsHexHelper` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `Byte[] HEX2Bytes(String hex)`
- `String Bytes2HEX(Byte[] val)`
- `String HEX2String(String hex)`
- `MemoryStream HEX2Stream(String hex)`
- `List`1 HEX2Identifiers(String hex)`
- `String IntToHex(Int32 value) (+1)`
- `Int32 HexToInt32(String value)`
- `Int64 HexToInt64(String value)`

### `SystemAccessGroups` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `Guid[] GetGroups()`

### `SystemFieldNames` (Namespace: `TFlex.DOCs.Common`)
**Свойства:** Count: Int32
**Методы:**
- `String GetName(SystemFields field)`

### `SystemParameterGroups` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `Boolean IsSystemParameterGroup(Guid parameterGroupGuid)`

### `SystemRevisionNameFields` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `ReadOnlyCollection`1 GetFields()`

### `ThreadSafeRandom` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `Int32 Next(Int32 minValue, Int32 maxValue) (+2)`
- `Double NextDouble()`
- `Void NextBytes(Byte[] buffer)`

### `Transliterator` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `String TransliterateString(String originalString, IList`1 nonSkippedChars)`

### `TypeHolder` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `Int32 CompareTo(Object obj) (+1)`

### `UnitToStringHelper` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `String GetTimeSpanString(TimeSpan delta, Boolean alignValues)`
- `String GetSizeString(Int64 size)`
- `String GetSpeedString(Double speed)`

### `Utf8StringWriter` (Namespace: `TFlex.DOCs.Common`)
**Свойства:** Encoding: Encoding

### `ValuesHelper` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `Int32 GetInt32Value(DataRow row, String columnName, Int32 defValue) (+2)`
- `Int64 GetInt64Value(DataRow row, String columnName, Int64 defValue) (+2)`
- `Single GetSingleValue(DataRow row, String columnName, Single defValue) (+2)`
- `Double GetDoubleValue(DataRow row, String columnName, Double defValue) (+2)`
- `Decimal GetDecimalValue(DataRow row, String columnName, Decimal defValue) (+2)`
- `DateTime GetDateTimeValue(DataRow row, String columnName, DateTime defValue) (+2)`
- `String GetStringValue(Object value, String defValue, CultureInfo culture) (+2)`
- `Guid GetGuidValue(DataRow row, String columnName, Guid defValue) (+2)`
- `Boolean GetBooleanValue(DataRow row, String columnName, Boolean defValue) (+2)`
- `Object Convert(Type itemType, Object value, Object defaultValue) (+1)`
- `Object TryGetValueByReflection(Object value)`
- `Object TryGetNativeSimpleValue(Object value, CultureInfo& culture) (+1)`
- `Object TryGetNativeValue(Object value, Boolean isList)`
- `DateTime Correct(DateTime value, Boolean isUtc)`
- `Boolean IsGenericCollection(Type collectionType) (+1)`
- `Type GetGenericCollectionItemType(Type collectionType) (+1)`
- `Object CreateGenericCollection(Type collectionType, Int32 capacity) (+1)`
- `Type AddToGenericCollection(T collection, Object value)`
- `Type AddToCollection(Object collection, Object value)`

### `VariablesProcessor` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `String Process(Preprocessor preprocessor, String line)`

### `WinServicesHelper` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `Boolean IsServiceExists(String serviceName)`
- `ServiceDispatcher GetService(String serviceName, Action`1 exceptionLogger)`

### `WriterLock` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `WriterLock Lock(ReaderWriterLock readerWriterLock, TimeSpan timeout) (+1)`

### `XDocumentExtensions` (Namespace: `TFlex.DOCs.Common`)
**Методы:**
- `Stream CreateStream(XDocument xDocument, Encoding encoding, Boolean omitDeclaration, String indentChars)`
- `String Serialize(XDocument xDocument, Encoding encoding, Boolean omitDeclaration)`

### `XsdDeclarationInfo` (Namespace: `TFlex.DOCs.Common`)
**Свойства:** TargetEncoding: String, OmitDeclaration: Boolean, StrictForm: Boolean, DatesInUTC: Boolean

### `CollectionExecution` (Namespace: `TFlex.DOCs.Common.Collections`)
**Методы:**
- `CollectionExecution Get(Boolean parallel)`
- `Void For(Int32 startIndex, Int32 endIndex, Action`1 action)`
- `Void ForEach(IEnumerable`1 source, List`1 result, Func`2 action) (+1)` [has Async]

### `ConcurrentHashSet`1` (Namespace: `TFlex.DOCs.Common.Collections`)
**Свойства:** Count: Int32
**Методы:**
- `Boolean Add(T item) (+3)`
- `Boolean RemoveAdd(T itemToRemove, T itemToAdd)`
- `Void Clear()`
- `Boolean Contains(T item)`
- `Boolean Remove(T item) (+2)`
- `List`1 ToList()`
- `T First()`
- `T FirstOrDefault()`
- `T Last()`
- `T LastOrDefault()`
- `T ElementAt(Int32 index)`
- `Boolean Any()`
- `Void UnionWith(IEnumerable`1 other) (+1)`
- `Void ExceptWith(IEnumerable`1 other) (+1)`
- `IEnumerable`1 Where(Func`2 predicate)`
- `Dictionary`2 ToDictionary(Func`2 keySelector)`
- `IEnumerator`1 GetEnumerator()`
- `List`1 WhereToList(Func`2 predicate)`
- `T[] WhereToArray(Func`2 predicate)`
- `Boolean ContainsAny(T[] items)`
- `T PopFirst()`
- `T PopFirstOrDefault()`
- `T PopLast()`
- `T PopLastOrDefault()`

### `ConcurrentLinkedList`1` (Namespace: `TFlex.DOCs.Common.Collections`)
**Свойства:** Count: Int32
**Методы:**
- `LinkedListNode`1 AddFirst(T item)`
- `LinkedListNode`1 AddBefore(LinkedListNode`1 node, T item)`
- `LinkedListNode`1 AddAfter(LinkedListNode`1 node, T item)`
- `LinkedListNode`1 AddLast(T item) (+1)`
- `Void Clear()`
- `Boolean Contains(T item)`
- `Void Remove(LinkedListNode`1 item) (+1)`
- `List`1 ToList()`
- `LinkedListNode`1 FirstNode()`
- `T First()`
- `T FirstOrDefault(Func`2 predicate) (+1)`
- `LinkedListNode`1 LastNode()`
- `T Last()`
- `T LastOrDefault(Func`2 predicate) (+1)`
- `T ElementAt(Int32 index)`
- `Boolean Any()`
- `IEnumerable`1 Where(Func`2 predicate)`
- `IEnumerator`1 GetEnumerator()`

### `ConsistentlyCollectionExecution` (Namespace: `TFlex.DOCs.Common.Collections`)
**Свойства:** Instance: CollectionExecution
**Методы:**
- `Void For(Int32 startIndex, Int32 endIndex, Action`1 action)`
- `Void ForEach(IEnumerable`1 source, List`1 result, Func`2 action) (+1)` [has Async]

### `ExecuteBatchQueue`1` (Namespace: `TFlex.DOCs.Common.Collections`)
**Методы:**
- `Void Add(T item)`
- `Void AddRange(IReadOnlyCollection`1 items)`

### `ExecuteLastQueue` (Namespace: `TFlex.DOCs.Common.Collections`)
**Методы:**
- `Void Add(Action action)`

### `FlexibleList`1` (Namespace: `TFlex.DOCs.Common.Collections`)
**Методы:**
- `FlexibleEnumerator GetEnumerator()`

### `IHierarchyItem`1` (Namespace: `TFlex.DOCs.Common.Collections`)
**Свойства:** Owner: IHierarchyItem`1, Parent: IHierarchyItem`1, TopParent: IHierarchyItem`1

### `ParallelCollectionExecution` (Namespace: `TFlex.DOCs.Common.Collections`)
**Свойства:** Instance: CollectionExecution
**Методы:**
- `Void For(Int32 startIndex, Int32 endIndex, Action`1 action)`
- `Void ForEach(IEnumerable`1 source, List`1 result, Func`2 action) (+1)` [has Async]

### `RangeElement` (Namespace: `TFlex.DOCs.Common.Collections`)
**Свойства:** From: Int32, To: Int32
**Методы:**
- `Boolean Contains(Int32 value)`

### `TypeDictionary`1` (Namespace: `TFlex.DOCs.Common.Collections`)
**Методы:**
- `TValue Get()`
- `Void Set(TValue value)`
- `Void Add(TValue value)`
- `Boolean Remove()`
- `Boolean TryGetValue(TValue& value)`
- `Boolean ContainsKey()`

### `ClientDataRegister` (Namespace: `TFlex.DOCs.Common.Communication`)
**Методы:**
- `Void Prepare(IDataSerializerPrepareManager manager)`

### `IClientDataRegistrator` (Namespace: `TFlex.DOCs.Common.Communication`)
**Методы:**
- `Action`1[] Register()`

### `DateTimeComparison` (Namespace: `TFlex.DOCs.Common.Comparers`)
**Методы:**
- `Boolean WithoutMilliseconds(DateTime left, DateTime right)`

### `NaturalStringComparer` (Namespace: `TFlex.DOCs.Common.Comparers`)
**Методы:**
- `Int32 Compare(String x, String y)`

### `CheckHeaderStream` (Namespace: `TFlex.DOCs.Common.Compression`)
**Свойства:** Header: Byte[], CanRead: Boolean, CanSeek: Boolean, CanWrite: Boolean, Length: Int64, Position: Int64
**Методы:**
- `Void Flush()`
- `Int32 Read(Byte[] buffer, Int32 offset, Int32 count)`
- `Int64 Seek(Int64 offset, SeekOrigin origin)`
- `Void SetLength(Int64 value)`
- `Void Write(Byte[] buffer, Int32 offset, Int32 count)`

### `CompressionManager` (Namespace: `TFlex.DOCs.Common.Compression`)
**Свойства:** AllowSevenZipArchive: Boolean
**Методы:**
- `Func`2 GetCompressor(CompressionAlgorithm compression)`
- `Func`2 GetDecompressor(CompressionAlgorithm compression)`
- `Func`2 GetSegmentCompressor(CompressionAlgorithm compression)`
- `Func`2 GetSegmentDecompressor(CompressionAlgorithm compression)`
- `ArraySegment`1 CompressBuffer(ArraySegment`1 buffer, BufferManager bufferManager, Int32 messageOffset, CompressionAlgorithm compression)`
- `ArraySegment`1 DecompressBuffer(ArraySegment`1 buffer, BufferManager bufferManager, CompressionAlgorithm compression)`
- `Stream GetCompressStream(Stream stream, CompressionAlgorithm compression)`
- `Stream GetDecompressStream(Stream stream, CompressionAlgorithm compression)`
- `ICollection`1 GetSevenZipArchiveFileNames(Stream fileStream)`
- `Stream DecompressFromSevenZipArchive(Stream fileStream, String fileName)`

### `ICompressionManager` (Namespace: `TFlex.DOCs.Common.Compression`)
**Свойства:** Algorithm: CompressionAlgorithm
**Методы:**
- `Func`2 GetCompressor()`
- `Func`2 GetDecompressor()`
- `Func`2 GetSegmentCompressor()`
- `Func`2 GetSegmentDecompressor()`
- `Stream GetCompressStream(Stream stream)`
- `Stream GetDecompressStream(Stream stream)`
- `ArraySegment`1 CompressBuffer(ArraySegment`1 buffer, BufferManager bufferManager, Int32 messageOffset)`
- `ArraySegment`1 DecompressBuffer(ArraySegment`1 buffer, BufferManager bufferManager)`

### `ICompressionRegistrator` (Namespace: `TFlex.DOCs.Common.Compression`)
**Методы:**
- `IEnumerable`1 GetManagers()`

### `ISevenZipManager` (Namespace: `TFlex.DOCs.Common.Compression`)
**Свойства:** AllowSevenZipArchive: Boolean
**Методы:**
- `ICollection`1 GetSevenZipArchiveFileNames(Stream fileStream)`
- `Stream DecompressFromSevenZipArchive(Stream fileStream, String fileName)`

### `ConfigurationProvider` (Namespace: `TFlex.DOCs.Common.Configuration`)
**Методы:**
- `IConfiguration FromXml(String filePath, Boolean writable)`
- `IConfiguration FromJson(String filePath, Boolean writable)`
- `IConfiguration FromRegistry(String registryKey, Boolean writable, Boolean useCurrentUserAsRoot)`
- `IConfiguration GetPlatformDependentConfiguration(String registryKey, String filePath, Boolean writable, Boolean useCurrentUserAsRoot, Boolean isJson)`

### `IConfiguration` (Namespace: `TFlex.DOCs.Common.Configuration`)
**Свойства:** Name: String
**Методы:**
- `IConfiguration CreateNode(String key)`
- `IConfiguration GetNode(String key)`
- `IEnumerable`1 GetNodes()`
- `Void DeleteNode(String key)`
- `T GetValue(String key, T defaultValue)`
- `Void SetValue(String key, T value)`
- `Void DeleteValue(String key)`

### `IConfigurationExtension` (Namespace: `TFlex.DOCs.Common.Configuration`)
**Методы:**
- `IConfiguration GetOrCreateNode(IConfiguration configuration, String key)`

### `BytesHexConverter` (Namespace: `TFlex.DOCs.Common.Converters`)
**Методы:**
- `String BytesToHex(Byte[] bytes)`

### `ObjectToStringConverter` (Namespace: `TFlex.DOCs.Common.Converters`)
**Методы:**
- `String Convert(Object value, ObjectToStringParameters parameters)`

### `ObjectToStringParameters` (Namespace: `TFlex.DOCs.Common.Converters`)
**Свойства:** ElementsInCollection: Int32

### `DataSerializerManager` (Namespace: `TFlex.DOCs.Common.DataSerializers`)
**Методы:**
- `Void Prepare(DataSerializerAlgorithm dataSerializer, Action`1 registrator) (+1)` [has Async]
- `Func`2 GetSerializer(DataSerializerAlgorithm dataSerializer)`
- `Func`2 GetDeserializer(DataSerializerAlgorithm dataSerializer)`
- `Func`2 GetSegmentSerializer(DataSerializerAlgorithm dataSerializer)`
- `Func`2 GetSegmentDeserializer(DataSerializerAlgorithm dataSerializer)`
- `Action`2 GetStreamSerializer(DataSerializerAlgorithm dataSerializer)`
- `Func`2 GetStreamDeserializer(DataSerializerAlgorithm dataSerializer)`
- `Boolean ConfigureEndpoint(ServiceEndpoint endpoint, DataSerializerAlgorithm dataSerializer)`
- `Func`2 GetDefaultCustomSerializer()`
- `Func`2 GetDefaultCustomDeserializer()`
- `Func`2 GetDefaultCustomSegmentSerializer()`
- `Func`2 GetDefaultCustomSegmentDeserializer()`
- `Action`2 GetDefaultCustomStreamSerializer()`
- `Func`2 GetDefaultCustomStreamDeserializer()`
- `DataSerializerAlgorithm GetDefaultDataSerializerAlgorithm(CommunicationMode communicationMode)`
- `DataSerializerAlgorithm SelectDataSerializerAlgorithm(DataSerializerAlgorithm dataSerializer, CommunicationMode communicationMode)`
- `DataSerializerAlgorithm GetCustomDataSerializerAlgorithm(DataSerializerAlgorithm dataSerializer)`

### `DataSerializersExtensions` (Namespace: `TFlex.DOCs.Common.DataSerializers`)
**Методы:**
- `Boolean HasDataContractAttribute(Type type)`
- `Boolean HasCollectionDataContractAttribute(Type type)`
- `Boolean HasInternalDataContractAttribute(Type type)`
- `KnownTypeAttribute[] GetKnownTypeAttributes(Type type)`
- `Type[] GetKnownTypes(Type type, Type& baseType) (+1)`
- `Boolean HasKnownTypes(Type type)`
- `Int32 GetTypeNestingCount(Type type)`
- `Type GetRealDataContractType(Type type)`
- `Boolean IsDynamicType(Type type) (+1)`

### `IDataContractSurrogate`1` (Namespace: `TFlex.DOCs.Common.DataSerializers`)
**Методы:**
- `T GetObjectToSerialize(T obj)`
- `T GetDeserializedObject(T obj)`

### `IDataContractSurrogateFactory` (Namespace: `TFlex.DOCs.Common.DataSerializers`)
**Методы:**
- `IDataContractSurrogate`1 CreateGenericSurrogate()`

### `IDataSerializerManager` (Namespace: `TFlex.DOCs.Common.DataSerializers`)
**Свойства:** Algorithm: DataSerializerAlgorithm
**Методы:**
- `Func`2 GetSerializer()`
- `Func`2 GetDeserializer()`
- `Func`2 GetSegmentSerializer()`
- `Func`2 GetSegmentDeserializer()`
- `Action`2 GetStreamSerializer()`
- `Func`2 GetStreamDeserializer()`
- `Void ConfigureEndpoint(ServiceEndpoint endpoint)`

### `IDataSerializerPrepareManager` (Namespace: `TFlex.DOCs.Common.DataSerializers`)
**Методы:**
- `Void Prepare()`

### `IDataSerializerRegistrator` (Namespace: `TFlex.DOCs.Common.DataSerializers`)
**Методы:**
- `IEnumerable`1 GetManagers()`

### `IRegisterKnownTypes` (Namespace: `TFlex.DOCs.Common.DataSerializers`)
**Методы:**
- `Void Register()`

### `JsonDataSerializer` (Namespace: `TFlex.DOCs.Common.DataSerializers`)
**Методы:**
- `Void SerializeToFile(Object data, String fileName)`
- `T DeserializeFromFile(String fileName)`

### `StandardDataContractResolver` (Namespace: `TFlex.DOCs.Common.DataSerializers`)
**Свойства:** Instance: StandardDataContractResolver
**Методы:**
- `Type ResolveName(String typeName, String typeNamespace, Type declaredType, DataContractResolver knownTypeResolver)`
- `Boolean TryResolveType(Type type, Type declaredType, DataContractResolver knownTypeResolver, XmlDictionaryString& typeName, XmlDictionaryString& typeNamespace)`

### `StandardDataSerializer` (Namespace: `TFlex.DOCs.Common.DataSerializers`)
**Методы:**
- `Stream Serialize(T obj)`
- `T Deserialize(Stream stream)`
- `T DeepClone(T obj)`
- `HashSet`1 GetKnownTypes(Type type)`
- `Boolean HasKnownType(Type type, Type knownType)`

### `ChunkedHashSet`1` (Namespace: `TFlex.DOCs.Common.DataStructures`)
**Методы:**
- `Boolean TryAdd(T item)`
- `Boolean Contains(T item)`

### `ChunkedList`1` (Namespace: `TFlex.DOCs.Common.DataStructures`)
**Свойства:** Count: Int32, Item: T, Item: IEnumerable`1
**Методы:**
- `IndexRange AddRange(IEnumerable`1 range)`
- `Int32 Add(T item)`
- `Void Clear()`
- `T Pop()`
- `T RemoveAt(Int32 index)`
- `Void TrimExcess()`
- `IEnumerable`1 GetByChunks()`
- `IEnumerator`1 GetEnumerator()`

### `FastCompactedList`1` (Namespace: `TFlex.DOCs.Common.DataStructures`)
**Свойства:** Count: Int32, Item: T
**Методы:**
- `Void Add(T item)`
- `Void Remove(Int32 index) (+1)`
- `Int32 IndexOf(T item)`
- `Void Clear()`
- `IEnumerator`1 GetEnumerator()`

### `IndexedStorage` (Namespace: `TFlex.DOCs.Common.DataStructures`)
**Свойства:** DebugView: DebugViewNode
**Методы:**
- `Void SetDebugNames(IEnumerable`1 names)`
- `Void RegisterType(Guid typeId)`
- `Void RegisterIndex(Guid typeId, Guid id, Func`2 keyGetter, Boolean singleItem)`
- `Void RegisterLink(Guid firtsTypeId, Guid secondTypeId, Guid linkId, Func`2 fromFirst, Func`2 fromSecond)`
- `IIndexedLink`2 GetLink(Guid typeId, Guid linkId, TType key)`
- `Boolean Contains(Guid typeId, TType value)`
- `IEnumerable`1 GetByIndex(Guid indexId, TKey key)`
- `TKey GetIndex(Guid indexId, TType item, TKey defaultValue)`
- `IEnumerable`1 GetByType(Guid typeId)`
- `Void Reset()`
- `Void AddLink(Guid linkId, TFirst first, TSecond second)`
- `Void RemoveLink(Guid linkId, TFirst first, TSecond second)`
- `Void Add(Guid typeId, TItem obj)`
- `Void RefreshIndexes(Guid typeId, TItem obj)`
- `Void Remove(Guid typeId, TItem obj)`

### `IndexedTwoDirectionOneToOneMap`2` (Namespace: `TFlex.DOCs.Common.DataStructures`)
**Свойства:** Count: Int32, IsReadOnly: Boolean, Keys: ICollection`1, Values: ICollection`1, Item: TValue, Item: TKey
**Методы:**
- `Boolean Remove(KeyValuePair`2 item) (+2)`
- `Void Add(KeyValuePair`2 item) (+1)`
- `Void Clear()`
- `Boolean Contains(KeyValuePair`2 item) (+2)`
- `Boolean ContainsKey(TKey key)`
- `Void AddRange(IEnumerable`1 items)`
- `Boolean TryGetValue(TKey key, TValue& value)`
- `Boolean TryGetKey(TValue key, TKey& value)`
- `IEnumerator`1 GetEnumerator()`
- `Void CopyTo(KeyValuePair`2[] array, Int32 arrayIndex)`

### `TwoDirectionLinkMap`3` (Namespace: `TFlex.DOCs.Common.DataStructures`)
**Методы:**
- `Void Add(TLink link)`
- `Void Remove(TLink link)`
- `TLink GetLink(T1 t1, T2 t2)`
- `IEnumerable`1 GetLinksByT1(T1 key)`
- `IEnumerable`1 GetLinksByT2(T2 key)`
- `IEnumerable`1 GetT2ValuesByT1(T1 key)`
- `IEnumerable`1 GetT1ValuesByT2(T2 key)`
- `Void Clear()`

### `TwoDirectionMultiMap`2` (Namespace: `TFlex.DOCs.Common.DataStructures`)
**Свойства:** Count: Int32, Version: UInt64, T1Keys: IEnumerable`1, T2Keys: IEnumerable`1, Item: IEnumerable`1, Item: IEnumerable`1
**Методы:**
- `Boolean TryAdd(T1 first, T2 second)`
- `Int32 GetByT1Count(T1 key)`
- `Int32 GetByT2Count(T2 key)`
- `IEnumerable`1 GetByT1Values(T1 key)`
- `IEnumerable`1 GetByT2Values(T2 key)`
- `Void RemoveByT1(T1 key)`
- `Void RemoveByT2(T2 key)`
- `Void Remove(T1 first, T2 second)`
- `Boolean Contains(T1 key) (+1)`
- `Void Clear()`

### `TwoDirectionOneToManyMap`2` (Namespace: `TFlex.DOCs.Common.DataStructures`)
**Свойства:** Item: IEnumerable`1
**Методы:**
- `Void Add(T1 first, T2 second) (+1)`
- `IEnumerable`1 GetItems(T1 key)`
- `T1 GetKey(T2 item)`
- `Void RemoveByT1(T1 value)`
- `Void RemoveByT2(T2 value)`
- `Void Clear()`
- `Boolean Contains(T1 key) (+1)`

### `TwoDirectionOneToOneMap`2` (Namespace: `TFlex.DOCs.Common.DataStructures`)
**Свойства:** Count: Int32, IsReadOnly: Boolean, Keys: ICollection`1, Values: ICollection`1, Item: TValue, Item: TKey
**Методы:**
- `IEnumerator`1 GetEnumerator()`
- `Void Clear()`
- `Void Add(TKey key, TValue value) (+1)`
- `Boolean ContainsKey(TKey key)`
- `Boolean TryAdd(TKey key, TValue value)`
- `Boolean Remove(TKey key) (+2)`
- `TValue GetValueByKey(TKey key)`
- `TKey GetKeyByValue(TValue key)`
- `Boolean TryGetValueByKey(TKey key, TValue& value)`
- `Boolean TryGetKeyByValue(TValue key, TKey& value)`
- `Boolean TryGetValue(TKey key, TValue& value) (+1)`
- `Boolean Contains(TKey key) (+2)`
- `Void CopyTo(KeyValuePair`2[] array, Int32 arrayIndex)`

### `DateTimeIntervalC` (Namespace: `TFlex.DOCs.Common.DataStructures.Intervals`)
**Свойства:** Interval: DateTimeInterval, Operations: DateTimeOperations, Start: DateTime, End: DateTime, IncludeStart: Boolean, IncludeEnd: Boolean, IsEndInfinite: Boolean, IsStartInfinite: Boolean, Length: TimeSpan, Middle: Nullable`1
**Методы:**
- `DateTimeIntervalC Create(DateTime start, DateTime end, Boolean includeStart, Boolean includeEnd)`
- `Void SetInterval(DateTimeInterval newInterval)`

### `DateTimeIntervalExtensions` (Namespace: `TFlex.DOCs.Common.DataStructures.Intervals`)
**Методы:**
- `Void SetInterval(IEditableIntervalContainer`4 item, DateTime start, DateTime end)`
- `Void SetStart(IEditableIntervalContainer`4 item, DateTime start)`
- `Void SetEnd(IEditableIntervalContainer`4 item, DateTime end)`
- `Void SetLenght(IEditableIntervalContainer`4 item, TimeSpan lenght)`
- `Void SetIntervalLimitEnd(IEditableIntervalContainer`4 item, DateTime start, DateTime end)`
- `Void ExpandOnExtremeBoarders(IEditableIntervalContainer`4 item, DateTime from, Boolean include) (+1)`
- `Void ExcludeFromLeft(IEditableIntervalContainer`4 item, DateTime from)`
- `Void ExcludeFromRight(IEditableIntervalContainer`4 item, DateTime from)`
- `Boolean IsIntersected(IIntervalContainer`4 a, IIntervalContainer`4 b)`
- `Boolean IsIntersectedExcludingBorder(IIntervalContainer`4 a, IIntervalContainer`4 b)`
- `Boolean IsLabelLess(IIntervalContainer`4 interval, DateTime timeStamp)`
- `Boolean IsLabelAbove(IIntervalContainer`4 interval, DateTime timeStamp)`
- `Boolean IsAdjoinedFromLeftTo(IIntervalContainer`4 a, IIntervalContainer`4 b)`
- `Boolean IsAdjoinedFromRightTo(IIntervalContainer`4 a, IIntervalContainer`4 b)`
- `Boolean IsAdjoinedTo(IIntervalContainer`4 a, IIntervalContainer`4 b)`
- `Boolean IsEmpty(IIntervalContainer`4 a)`
- `T Shift(T item, TimeSpan offset) (+1)`
- `IEnumerable`1 Union(IEnumerable`1 items, Func`2 intervalGetter, Func`4 combine, Func`2 exInfoHashGetter, Boolean sorted, Boolean allBordersIncluded, Boolean notGroupByExHash) (+4)`
- `IEnumerable`1 SplitInterval(T container, TimeSpan partSize, Boolean fromEnd)`
- `IEnumerable`1 GetAllIntersections(IEnumerable`1 items)`
- `Boolean IsPartOf(IIntervalContainer`4 interval, IIntervalContainer`4 superSet)`
- `TimeSpan GetLength(IIntervalContainer`4 interval)`
- `Nullable`1 GetMiddle(IIntervalContainer`4 interval)`
- `String GetTextRepresentation(IIntervalContainer`4 interval)`
- `IEnumerable`1 Except(T0 a, IEnumerable`1 b) (+1)`
- `Boolean IsSubSet(IIntervalContainer`4 superSet, IIntervalContainer`4 b)`
- `IEnumerable`1 SubSets(IEnumerable`1 superSet, IEnumerable`1 set, Boolean isSuperSetSorted, Boolean isSetSorted)`
- `IEnumerable`1 Intersection(IEnumerable`1 firstSet, IEnumerable`1 secondSet, Boolean isFirstSetSorted, Boolean isSecondSetSorted) (+3)`
- `Boolean Contains(IIntervalContainer`4 a, DateTime point, Boolean allBordersIncluded)`

### `DateTimeIntervalMitC`1` (Namespace: `TFlex.DOCs.Common.DataStructures.Intervals`)
**Свойства:** Interval: DateTimeInterval, Value: T
**Методы:**
- `Void SetInterval(DateTimeInterval newInterval)`
- `DateTimeIntervalMitC`1 Create(DateTime start, DateTime end, Boolean includeStart, Boolean includeEnd)`

### `DoubleIntervalC` (Namespace: `TFlex.DOCs.Common.DataStructures.Intervals`)
**Свойства:** Interval: DoubleInterval, Operations: DoubleOperations, Start: Double, End: Double, IncludeStart: Boolean, IncludeEnd: Boolean, IsEndInfinite: Boolean, IsStartInfinite: Boolean, Length: Double, Middle: Nullable`1
**Методы:**
- `DoubleIntervalC Create(Double start, Double end, Boolean includeStart, Boolean includeEnd)`
- `Void SetInterval(DoubleInterval newInterval)`

### `DoubleIntervalExtensions` (Namespace: `TFlex.DOCs.Common.DataStructures.Intervals`)
**Методы:**
- `Void SetInterval(IEditableIntervalContainer`4 item, Double start, Double end)`
- `Void SetStart(IEditableIntervalContainer`4 item, Double start)`
- `Void SetEnd(IEditableIntervalContainer`4 item, Double end)`
- `Void SetLenght(IEditableIntervalContainer`4 item, Double lenght)`
- `Void SetIntervalLimitEnd(IEditableIntervalContainer`4 item, Double start, Double end)`
- `Void ExpandOnExtremeBoarders(IEditableIntervalContainer`4 item, Double from, Boolean include) (+1)`
- `Void ExcludeFromLeft(IEditableIntervalContainer`4 item, Double from)`
- `Void ExcludeFromRight(IEditableIntervalContainer`4 item, Double from)`
- `Boolean IsIntersected(IIntervalContainer`4 a, IIntervalContainer`4 b)`
- `Boolean IsIntersectedExcludingBorder(IIntervalContainer`4 a, IIntervalContainer`4 b)`
- `Boolean IsLabelLess(IIntervalContainer`4 interval, Double timeStamp)`
- `Boolean IsLabelAbove(IIntervalContainer`4 interval, Double timeStamp)`
- `Boolean IsAdjoinedFromLeftTo(IIntervalContainer`4 a, IIntervalContainer`4 b)`
- `Boolean IsAdjoinedFromRightTo(IIntervalContainer`4 a, IIntervalContainer`4 b)`
- `Boolean IsAdjoinedTo(IIntervalContainer`4 a, IIntervalContainer`4 b)`
- `Boolean IsEmpty(IIntervalContainer`4 a)`
- `T Shift(T item, Double offset) (+1)`
- `IEnumerable`1 Union(IEnumerable`1 items, Func`2 intervalGetter, Func`4 combine, Func`2 exInfoHashGetter, Boolean sorted, Boolean allBordersIncluded, Boolean notGroupByExHash) (+4)`
- `IEnumerable`1 SplitInterval(T container, Double partSize, Boolean fromEnd)`
- `IEnumerable`1 GetAllIntersections(IEnumerable`1 items)`
- `Boolean IsPartOf(IIntervalContainer`4 interval, IIntervalContainer`4 superSet)`
- `Double GetLength(IIntervalContainer`4 interval)`
- `Nullable`1 GetMiddle(IIntervalContainer`4 interval)`
- `String GetTextRepresentation(IIntervalContainer`4 interval)`
- `IEnumerable`1 Except(T0 a, IEnumerable`1 b) (+1)`
- `Boolean IsSubSet(IIntervalContainer`4 superSet, IIntervalContainer`4 b)`
- `IEnumerable`1 SubSets(IEnumerable`1 superSet, IEnumerable`1 set, Boolean isSuperSetSorted, Boolean isSetSorted)`
- `IEnumerable`1 Intersection(IEnumerable`1 firstSet, IEnumerable`1 secondSet, Boolean isFirstSetSorted, Boolean isSecondSetSorted) (+3)`
- `Boolean Contains(IIntervalContainer`4 a, Double point, Boolean allBordersIncluded)`

### `DoubleIntervalMitC`1` (Namespace: `TFlex.DOCs.Common.DataStructures.Intervals`)
**Свойства:** Interval: DoubleInterval, Value: T
**Методы:**
- `Void SetInterval(DoubleInterval newInterval)`
- `DoubleIntervalMitC`1 Create(Double start, Double end, Boolean includeStart, Boolean includeEnd)`

### `IEditableIntervalContainer`4` (Namespace: `TFlex.DOCs.Common.DataStructures.Intervals`)
**Методы:**
- `Void SetInterval(TInterval newInterval)`

### `IInterval`4` (Namespace: `TFlex.DOCs.Common.DataStructures.Intervals`)
**Свойства:** Operations: TIntervalOperations, Start: TPoint, End: TPoint, IncludeStart: Boolean, IncludeEnd: Boolean, IsEndInfinite: Boolean, IsStartInfinite: Boolean

### `IIntervalContainer`4` (Namespace: `TFlex.DOCs.Common.DataStructures.Intervals`)
**Свойства:** Interval: TInterval

### `IIntervalContainerWithCreation`5` (Namespace: `TFlex.DOCs.Common.DataStructures.Intervals`)
**Методы:**
- `TCreatedInterval Create(TPoint start, TPoint end, Boolean includeStart, Boolean includeEnd)`

### `IIntervalOperations`3` (Namespace: `TFlex.DOCs.Common.DataStructures.Intervals`)
**Свойства:** NegInf: TPoint, PosInf: TPoint, DeltaNegInf: TDelta, DeltaPosInf: TDelta, DeltaZero: TDelta
**Методы:**
- `TDelta Subtraction(TPoint a, TPoint b)`
- `TPoint Addition(TPoint a, TDelta b)`
- `TDelta Invert(TDelta a)`
- `Double ToDouble(TDelta delta)`
- `TDelta FromDouble(Double d)`

### `IntervalExtensions` (Namespace: `TFlex.DOCs.Common.DataStructures.Intervals`)
**Методы:**
- `Void SetInterval(TIntervalContainer item, T start, T end)`
- `Void SetStart(TIntervalContainer item, T start)`
- `Void SetEnd(TIntervalContainer item, T end)`
- `Void SetLenght(TIntervalContainer item, TDelta lenght)`
- `Void SetIntervalLimitEnd(TIntervalContainer item, T start, T end)`
- `Void ExpandOnExtremeBoarders(TEditIntervalContainer item, T from, Boolean include) (+1)`
- `Void ExcludeFromLeft(TEditIntervalContainer item, T from)`
- `Void ExcludeFromRight(TEditIntervalContainer item, T from)`
- `Boolean IsIntersected(TIntervalContainer a, TIntervalContainer b)`
- `Boolean IsIntersectedExcludingBorder(TIntervalContainer a, TIntervalContainer b)`
- `Boolean IsPartOf(TInterval interval, TInterval superSet)`
- `Boolean IsLabelLess(TIntervalContainer interval, T timeStamp)`
- `Boolean IsLabelAbove(TIntervalContainer interval, T timeStamp)`
- `Boolean IsAdjoinedFromLeftTo(TIntervalContainer a, TIntervalContainer b)`
- `Boolean IsAdjoinedFromRightTo(TIntervalContainer a, TIntervalContainer b)`
- `Boolean IsAdjoinedTo(TIntervalContainer a, TIntervalContainer b)`
- `Boolean IsEmpty(TIntervalContainer a)`
- `TIntervalContainerWithCreation Shift(TIntervalContainerWithCreation item, TDelta offset) (+1)`
- `TDelta GetLength(TInterval interval)`
- `Nullable`1 FromDouble(Double d, LengthRoundMode roundMode, TIntervalOperations operations)`
- `Nullable`1 GetMiddle(TInterval interval)`
- `String GetTextRepresentation(TInterval interval)`
- `IEnumerable`1 Except(TCreate a, IEnumerable`1 b) (+1)`
- `IEnumerable`1 Union(IEnumerable`1 items, Func`2 intervalGetter, Func`4 combine, Func`2 exInfoHashGetter, Boolean sorted, Boolean allBordersIncluded, Boolean notGroupByExHash) (+4)`
- `Boolean IsSubSet(TInterval superSet, TInterval b)`
- `IEnumerable`1 SubSets(IEnumerable`1 superSet, IEnumerable`1 set, Boolean isSuperSetSorted, Boolean isSetSorted)`
- `IEnumerable`1 SplitInterval(TContainer container, TDelta partSize, Boolean fromEnd)`
- `IEnumerable`1 GetAllIntersections(IEnumerable`1 items)`
- `IEnumerable`1 Intersection(IEnumerable`1 firstSet, IEnumerable`1 secondSet, Boolean isFirstSetSorted, Boolean isSecondSetSorted) (+3)`
- `Boolean Contains(TInterval a, T point, Boolean allBordersIncluded)`

### `IntervalMap`5` (Namespace: `TFlex.DOCs.Common.DataStructures.Intervals`)
**Свойства:** Start: TIntervalPoint, End: TIntervalPoint, Count: Int32
**Методы:**
- `Void Add(T item)`
- `Void AddRange(IList`1 items)`
- `IEnumerable`1 Get(TIntervalPoint start, TIntervalPoint end)`
- `Void Remove(T item)`
- `Void Clear()`
- `IEnumerator`1 GetEnumerator()`

### `IntervalSet`6` (Namespace: `TFlex.DOCs.Common.DataStructures.Intervals`)
**Свойства:** Start: TIntervalPoint, End: TIntervalPoint, Count: Int32
**Методы:**
- `Boolean HaveNonZeroIntersection(TInterval interval)`
- `IList`1 GetIntervals(TIntervalPoint minPoint, TIntervalPoint maxPoint, Func`2 predicate, Func`2 selector) (+1)`
- `Void Clear()`
- `Void SetFrom(IntervalSet`6 set)`
- `Void AddRange(IList`1 intervals)`
- `Void Add(T interval)`
- `IEnumerator`1 GetEnumerator()`

### `IntIntervalC` (Namespace: `TFlex.DOCs.Common.DataStructures.Intervals`)
**Свойства:** Interval: IntInterval, Operations: IntOperations, Start: Int32, End: Int32, IncludeStart: Boolean, IncludeEnd: Boolean, IsEndInfinite: Boolean, IsStartInfinite: Boolean, Length: Int32, Middle: Nullable`1
**Методы:**
- `IntIntervalC Create(Int32 start, Int32 end, Boolean includeStart, Boolean includeEnd)`
- `Void SetInterval(IntInterval newInterval)`

### `IntIntervalExtensions` (Namespace: `TFlex.DOCs.Common.DataStructures.Intervals`)
**Методы:**
- `Void SetInterval(IEditableIntervalContainer`4 item, Int32 start, Int32 end)`
- `Void SetStart(IEditableIntervalContainer`4 item, Int32 start)`
- `Void SetEnd(IEditableIntervalContainer`4 item, Int32 end)`
- `Void SetLenght(IEditableIntervalContainer`4 item, Int32 lenght)`
- `Void SetIntervalLimitEnd(IEditableIntervalContainer`4 item, Int32 start, Int32 end)`
- `Void ExpandOnExtremeBoarders(IEditableIntervalContainer`4 item, Int32 from, Boolean include) (+1)`
- `Void ExcludeFromLeft(IEditableIntervalContainer`4 item, Int32 from)`
- `Void ExcludeFromRight(IEditableIntervalContainer`4 item, Int32 from)`
- `Boolean IsIntersected(IIntervalContainer`4 a, IIntervalContainer`4 b)`
- `Boolean IsIntersectedExcludingBorder(IIntervalContainer`4 a, IIntervalContainer`4 b)`
- `Boolean IsLabelLess(IIntervalContainer`4 interval, Int32 timeStamp)`
- `Boolean IsLabelAbove(IIntervalContainer`4 interval, Int32 timeStamp)`
- `Boolean IsAdjoinedFromLeftTo(IIntervalContainer`4 a, IIntervalContainer`4 b)`
- `Boolean IsAdjoinedFromRightTo(IIntervalContainer`4 a, IIntervalContainer`4 b)`
- `Boolean IsAdjoinedTo(IIntervalContainer`4 a, IIntervalContainer`4 b)`
- `Boolean IsEmpty(IIntervalContainer`4 a)`
- `T Shift(T item, Int32 offset) (+1)`
- `IEnumerable`1 Union(IEnumerable`1 items, Func`2 intervalGetter, Func`4 combine, Func`2 exInfoHashGetter, Boolean sorted, Boolean allBordersIncluded, Boolean notGroupByExHash) (+4)`
- `IEnumerable`1 SplitInterval(T container, Int32 partSize, Boolean fromEnd)`
- `IEnumerable`1 GetAllIntersections(IEnumerable`1 items)`
- `Boolean IsPartOf(IIntervalContainer`4 interval, IIntervalContainer`4 superSet)`
- `Int32 GetLength(IIntervalContainer`4 interval)`
- `Nullable`1 GetMiddle(IIntervalContainer`4 interval)`
- `String GetTextRepresentation(IIntervalContainer`4 interval)`
- `IEnumerable`1 Except(T0 a, IEnumerable`1 b) (+1)`
- `Boolean IsSubSet(IIntervalContainer`4 superSet, IIntervalContainer`4 b)`
- `IEnumerable`1 SubSets(IEnumerable`1 superSet, IEnumerable`1 set, Boolean isSuperSetSorted, Boolean isSetSorted)`
- `IEnumerable`1 Intersection(IEnumerable`1 firstSet, IEnumerable`1 secondSet, Boolean isFirstSetSorted, Boolean isSecondSetSorted) (+3)`
- `Boolean Contains(IIntervalContainer`4 a, Int32 point, Boolean allBordersIncluded)`

### `IntIntervalMitC`1` (Namespace: `TFlex.DOCs.Common.DataStructures.Intervals`)
**Свойства:** Interval: IntInterval, Value: T
**Методы:**
- `Void SetInterval(IntInterval newInterval)`
- `IntIntervalMitC`1 Create(Int32 start, Int32 end, Boolean includeStart, Boolean includeEnd)`

### `LongIntervalC` (Namespace: `TFlex.DOCs.Common.DataStructures.Intervals`)
**Свойства:** Interval: LongInterval, Operations: LongOperations, Start: Int64, End: Int64, IncludeStart: Boolean, IncludeEnd: Boolean, IsEndInfinite: Boolean, IsStartInfinite: Boolean, Length: Int64, Middle: Nullable`1
**Методы:**
- `LongIntervalC Create(Int64 start, Int64 end, Boolean includeStart, Boolean includeEnd)`
- `Void SetInterval(LongInterval newInterval)`

### `LongIntervalExtensions` (Namespace: `TFlex.DOCs.Common.DataStructures.Intervals`)
**Методы:**
- `Void SetInterval(IEditableIntervalContainer`4 item, Int64 start, Int64 end)`
- `Void SetStart(IEditableIntervalContainer`4 item, Int64 start)`
- `Void SetEnd(IEditableIntervalContainer`4 item, Int64 end)`
- `Void SetLenght(IEditableIntervalContainer`4 item, Int64 lenght)`
- `Void SetIntervalLimitEnd(IEditableIntervalContainer`4 item, Int64 start, Int64 end)`
- `Void ExpandOnExtremeBoarders(IEditableIntervalContainer`4 item, Int64 from, Boolean include) (+1)`
- `Void ExcludeFromLeft(IEditableIntervalContainer`4 item, Int64 from)`
- `Void ExcludeFromRight(IEditableIntervalContainer`4 item, Int64 from)`
- `Boolean IsIntersected(IIntervalContainer`4 a, IIntervalContainer`4 b)`
- `Boolean IsIntersectedExcludingBorder(IIntervalContainer`4 a, IIntervalContainer`4 b)`
- `Boolean IsLabelLess(IIntervalContainer`4 interval, Int64 timeStamp)`
- `Boolean IsLabelAbove(IIntervalContainer`4 interval, Int64 timeStamp)`
- `Boolean IsAdjoinedFromLeftTo(IIntervalContainer`4 a, IIntervalContainer`4 b)`
- `Boolean IsAdjoinedFromRightTo(IIntervalContainer`4 a, IIntervalContainer`4 b)`
- `Boolean IsAdjoinedTo(IIntervalContainer`4 a, IIntervalContainer`4 b)`
- `Boolean IsEmpty(IIntervalContainer`4 a)`
- `T Shift(T item, Int64 offset) (+1)`
- `IEnumerable`1 Union(IEnumerable`1 items, Func`2 intervalGetter, Func`4 combine, Func`2 exInfoHashGetter, Boolean sorted, Boolean allBordersIncluded, Boolean notGroupByExHash) (+4)`
- `IEnumerable`1 SplitInterval(T container, Int64 partSize, Boolean fromEnd)`
- `IEnumerable`1 GetAllIntersections(IEnumerable`1 items)`
- `Boolean IsPartOf(IIntervalContainer`4 interval, IIntervalContainer`4 superSet)`
- `Int64 GetLength(IIntervalContainer`4 interval)`
- `Nullable`1 GetMiddle(IIntervalContainer`4 interval)`
- `String GetTextRepresentation(IIntervalContainer`4 interval)`
- `IEnumerable`1 Except(T0 a, IEnumerable`1 b) (+1)`
- `Boolean IsSubSet(IIntervalContainer`4 superSet, IIntervalContainer`4 b)`
- `IEnumerable`1 SubSets(IEnumerable`1 superSet, IEnumerable`1 set, Boolean isSuperSetSorted, Boolean isSetSorted)`
- `IEnumerable`1 Intersection(IEnumerable`1 firstSet, IEnumerable`1 secondSet, Boolean isFirstSetSorted, Boolean isSecondSetSorted) (+3)`
- `Boolean Contains(IIntervalContainer`4 a, Int64 point, Boolean allBordersIncluded)`

### `LongIntervalMitC`1` (Namespace: `TFlex.DOCs.Common.DataStructures.Intervals`)
**Свойства:** Interval: LongInterval, Value: T
**Методы:**
- `Void SetInterval(LongInterval newInterval)`
- `LongIntervalMitC`1 Create(Int64 start, Int64 end, Boolean includeStart, Boolean includeEnd)`

### `TimeSpanIntervalC` (Namespace: `TFlex.DOCs.Common.DataStructures.Intervals`)
**Свойства:** Interval: TimeSpanInterval, Operations: TimeSpanOperations, Start: TimeSpan, End: TimeSpan, IncludeStart: Boolean, IncludeEnd: Boolean, IsEndInfinite: Boolean, IsStartInfinite: Boolean, Length: TimeSpan, Middle: Nullable`1
**Методы:**
- `TimeSpanIntervalC Create(TimeSpan start, TimeSpan end, Boolean includeStart, Boolean includeEnd)`
- `Void SetInterval(TimeSpanInterval newInterval)`

### `TimeSpanIntervalExtensions` (Namespace: `TFlex.DOCs.Common.DataStructures.Intervals`)
**Методы:**
- `Void SetInterval(IEditableIntervalContainer`4 item, TimeSpan start, TimeSpan end)`
- `Void SetStart(IEditableIntervalContainer`4 item, TimeSpan start)`
- `Void SetEnd(IEditableIntervalContainer`4 item, TimeSpan end)`
- `Void SetLenght(IEditableIntervalContainer`4 item, TimeSpan lenght)`
- `Void SetIntervalLimitEnd(IEditableIntervalContainer`4 item, TimeSpan start, TimeSpan end)`
- `Void ExpandOnExtremeBoarders(IEditableIntervalContainer`4 item, TimeSpan from, Boolean include) (+1)`
- `Void ExcludeFromLeft(IEditableIntervalContainer`4 item, TimeSpan from)`
- `Void ExcludeFromRight(IEditableIntervalContainer`4 item, TimeSpan from)`
- `Boolean IsIntersected(IIntervalContainer`4 a, IIntervalContainer`4 b)`
- `Boolean IsIntersectedExcludingBorder(IIntervalContainer`4 a, IIntervalContainer`4 b)`
- `Boolean IsLabelLess(IIntervalContainer`4 interval, TimeSpan timeStamp)`
- `Boolean IsLabelAbove(IIntervalContainer`4 interval, TimeSpan timeStamp)`
- `Boolean IsAdjoinedFromLeftTo(IIntervalContainer`4 a, IIntervalContainer`4 b)`
- `Boolean IsAdjoinedFromRightTo(IIntervalContainer`4 a, IIntervalContainer`4 b)`
- `Boolean IsAdjoinedTo(IIntervalContainer`4 a, IIntervalContainer`4 b)`
- `Boolean IsEmpty(IIntervalContainer`4 a)`
- `T Shift(T item, TimeSpan offset) (+1)`
- `IEnumerable`1 Union(IEnumerable`1 items, Func`2 intervalGetter, Func`4 combine, Func`2 exInfoHashGetter, Boolean sorted, Boolean allBordersIncluded, Boolean notGroupByExHash) (+4)`
- `IEnumerable`1 SplitInterval(T container, TimeSpan partSize, Boolean fromEnd)`
- `IEnumerable`1 GetAllIntersections(IEnumerable`1 items)`
- `Boolean IsPartOf(IIntervalContainer`4 interval, IIntervalContainer`4 superSet)`
- `TimeSpan GetLength(IIntervalContainer`4 interval)`
- `Nullable`1 GetMiddle(IIntervalContainer`4 interval)`
- `String GetTextRepresentation(IIntervalContainer`4 interval)`
- `IEnumerable`1 Except(T0 a, IEnumerable`1 b) (+1)`
- `Boolean IsSubSet(IIntervalContainer`4 superSet, IIntervalContainer`4 b)`
- `IEnumerable`1 SubSets(IEnumerable`1 superSet, IEnumerable`1 set, Boolean isSuperSetSorted, Boolean isSetSorted)`
- `IEnumerable`1 Intersection(IEnumerable`1 firstSet, IEnumerable`1 secondSet, Boolean isFirstSetSorted, Boolean isSecondSetSorted) (+3)`
- `Boolean Contains(IIntervalContainer`4 a, TimeSpan point, Boolean allBordersIncluded)`

### `TimeSpanIntervalMitC`1` (Namespace: `TFlex.DOCs.Common.DataStructures.Intervals`)
**Свойства:** Interval: TimeSpanInterval, Value: T
**Методы:**
- `Void SetInterval(TimeSpanInterval newInterval)`
- `TimeSpanIntervalMitC`1 Create(TimeSpan start, TimeSpan end, Boolean includeStart, Boolean includeEnd)`

### `WeightedDateTimeIntervalExtensions` (Namespace: `TFlex.DOCs.Common.DataStructures.Intervals`)
**Методы:**
- `Nullable`1 GetMiddle(WeightedDateTimeInterval interval)`
- `TimeSpan GetLength(WeightedDateTimeInterval interval)`
- `String GetTextRepresentation(WeightedDateTimeInterval interval)`
- `IEnumerable`1 GetAllIntersections(IEnumerable`1 items)`
- `IEnumerable`1 Except(WeightedDateTimeInterval a, IEnumerable`1 b) (+1)`

### `BuilderHelper` (Namespace: `TFlex.DOCs.Common.DynamicAssembly`)
**Методы:**
- `String CreateClassName(String moduleName, Type type, String suffix)`
- `String CreateIntrefaceName(String moduleName, String typeName, String suffix)`
- `ConstructorInfo GetBaseGenericCtor(Type baseType, Type baseGenericType, BindingFlags bindingAttr, Type[] types)`
- `Void BuildDefaultCtor(TypeBuilder builder, Type baseType)`
- `Void DefineParameterName(ConstructorBuilder constructor, String name) (+1)`
- `Void DefineParameterNames(ConstructorBuilder constructor, String[] names, ParameterAttributes[] attributes) (+3)`
- `Void DefineParameterNamesWithLastOptional(MethodBuilder method, String[] names)`
- `Void EmitLdarg(ILGenerator il, Int32 index)`

### `DynamicAssemblyBuilder` (Namespace: `TFlex.DOCs.Common.DynamicAssembly`)
**Свойства:** Assembly: AssemblyBuilder, ModuleName: String, Module: ModuleBuilder

### `AESHelper` (Namespace: `TFlex.DOCs.Common.Encryption`)
**Методы:**
- `String EncryptSettingsPassword(String password, String key)`
- `String Encrypt(String plainText, String key)`
- `Byte[] EncryptBytes(String plainText, String key)`
- `String DecryptSettingsPassword(String password, String key)`
- `String Decrypt(String ciphertext, String key)`
- `String DecryptBytes(Byte[] ciphertext, String key)`

### `AesOldHelper` (Namespace: `TFlex.DOCs.Common.Encryption`)
**Методы:**
- `Byte[] EncryptBytes(String plainText, String key)`
- `String DecryptBytes(Byte[] ciphertext, String key)`

### `AesPassManager` (Namespace: `TFlex.DOCs.Common.Encryption`)
**Методы:**
- `String EncryptPassword(String password, String key) (+1)`
- `String DecryptPassword(String password, Boolean throwException, Boolean& isLegacyPassword) (+1)`
- `String GenerateKey()`

### `CertificateSessionItem` (Namespace: `TFlex.DOCs.Common.Encryption`)
**Свойства:** CertificateGuid: Guid, Name: String, Hash: String, ProtectedHash: String, IsProtected: Boolean, BestBefore: DateTime, ExpirationDate: DateTime
**Методы:**
- `Boolean IsReadOnly()`
- `Void Clear()`
- `Void Assign(Object source)`
- `Void CheckBestBefore()`
- `Int32 CompareTo(Object obj) (+1)`
- `Void Protect(String key)`
- `CertificateSessionItem Unprotect(String key)`

### `Crc32` (Namespace: `TFlex.DOCs.Common.Encryption`)
**Свойства:** HashSize: Int32
**Методы:**
- `Void Initialize()`
- `UInt32 Compute(UInt32 polynomial, UInt32 seed, Byte[] buffer) (+3)`

### `Crc64` (Namespace: `TFlex.DOCs.Common.Encryption`)
**Свойства:** HashSize: Int32
**Методы:**
- `Void Initialize()`

### `Crc64Iso` (Namespace: `TFlex.DOCs.Common.Encryption`)
**Методы:**
- `UInt64 Compute(UInt64 seed, Byte[] buffer) (+2)`

### `EncryptedParameterInfo` (Namespace: `TFlex.DOCs.Common.Encryption`)
**Свойства:** GroupPK: Int32, GroupType: CommonParameterGroupType, GroupCaption: String, GroupTable: String, ParamPK: Int32, ParamCaption: String, ParamField: String
**Методы:**
- `Void Clear()`
- `Void Assign(Object source)`

### `EncryptionMethodsHelper` (Namespace: `TFlex.DOCs.Common.Encryption`)
**Методы:**
- `AsymmEncryptionMethod GetAsymmEncryptionMethod(String method)`
- `SymmEncryptionMethod GetSymmEncryptionMethod(String method)`

### `Encryptor` (Namespace: `TFlex.DOCs.Common.Encryption`)
**Методы:**
- `String GetPrimaryHash(String password)`
- `Byte[] GetPasswordHash(Byte[] passwordHashBytes, Int32 times4hash) (+1)`
- `String GetPasswordAsString(Byte[] passwordHashBytes)`
- `String GetKeyName(Guid certificateGuid, Boolean isAsymmetricKey)`
- `String MaskSQL(String sql)`

### `MD5HashString` (Namespace: `TFlex.DOCs.Common.Encryption`)
**Методы:**
- `String Encrypt(String str)`

### `RC2CryptoServiceProviderFactory` (Namespace: `TFlex.DOCs.Common.Encryption`)
**Методы:**
- `ICryptoTransform CreateDecryptor(String seedString)`
- `ICryptoTransform CreateEncryptor(String seedString)`
- `RC2 CreateProvider(String seedString)`

### `CustomErrorData` (Namespace: `TFlex.DOCs.Common.Errors`)
**Свойства:** TypeFullName: String, Data: Byte[]
**Методы:**
- `CustomErrorData Create(T data)`
- `T Get()`

### `CustomErrorDataLegacy` (Namespace: `TFlex.DOCs.Common.Errors`)
**Свойства:** TypeFullName: String, Data: Byte[]
**Методы:**
- `CustomErrorDataLegacy Create(T data)`
- `T Get()`

### `DatabaseErrorData` (Namespace: `TFlex.DOCs.Common.Errors`)
**Свойства:** DatabaseErrorType: DatabaseErrorType

### `DatabaseErrorDataLegacy` (Namespace: `TFlex.DOCs.Common.Errors`)
**Свойства:** DatabaseErrorType: DatabaseErrorType

### `IntegerErrorData` (Namespace: `TFlex.DOCs.Common.Errors`)
**Свойства:** Data: Int32

### `IntegerErrorDataLegacy` (Namespace: `TFlex.DOCs.Common.Errors`)
**Свойства:** Data: Int32

### `RethrowErrorData` (Namespace: `TFlex.DOCs.Common.Errors`)
**Свойства:** Message: String

### `RethrowErrorDataLegacy` (Namespace: `TFlex.DOCs.Common.Errors`)
**Свойства:** Message: String

### `SessionErrorData` (Namespace: `TFlex.DOCs.Common.Errors`)
**Свойства:** Message: String, KeepAlive: Boolean

### `StringArrayErrorData` (Namespace: `TFlex.DOCs.Common.Errors`)
**Свойства:** Data: String[]

### `StringArrayErrorDataLegacy` (Namespace: `TFlex.DOCs.Common.Errors`)
**Свойства:** Data: String[]

### `StringErrorData` (Namespace: `TFlex.DOCs.Common.Errors`)
**Свойства:** Data: String

### `StringErrorDataLegacy` (Namespace: `TFlex.DOCs.Common.Errors`)
**Свойства:** Data: String

### `SystemErrorData` (Namespace: `TFlex.DOCs.Common.Errors`)
**Свойства:** Message: String

### `SystemErrorDataLegacy` (Namespace: `TFlex.DOCs.Common.Errors`)
**Свойства:** Message: String

### `ExpressionCalculate` (Namespace: `TFlex.DOCs.Common.Expressions`)
**Методы:**
- `Double Calculate(String expressionText, Parameter[] parameters) (+1)`

### `AdvancedEnumExtensions` (Namespace: `TFlex.DOCs.Common.Extensions`)
**Методы:**
- `String EnumToString(T value, String delimeter) (+2)`
- `String ToDescription(T value, String key) (+1)`
- `Boolean HasAnyFlag(T value, T flags)`
- `IEnumerable`1 GetFlags(T input)`
- `Boolean TryParseEnum(Type enumType, String s, Boolean useDescriptions, Enum& result)`

### `ArrayExtensions` (Namespace: `TFlex.DOCs.Common.Extensions`)
**Методы:**
- `Int32 BinarySearch(T[] array, Int32 startIndex, Int32 count, T value, BinarySearchType searchType)`
- `T[] SubArray(T[] array, Int32 startIndex, Int32 length) (+1)`
- `Int32 ReplaceRange(T[]& array, Int32 count, Int32 startIndex, Int32 endIndex, IList`1 source, Int32 sourceCount, Func`2 getter)`
- `Void InsertInArray(T[]& array, Int32 count, T item, Int32 index)`
- `Void ExpandArray(T[]& array, Int32 newSize, Boolean copyOldBuffer)`
- `Byte[] GetFromBuffer(Byte[] buffer, Int32& offset, Int32 count)`
- `Void AddToBuffer(Byte[]& buffer, Int32& offset, Byte[] value, Boolean copyOldBuffer)`
- `T[] Merge(T[] array1, T[] array2)`

### `AsyncEnumerableExtensions` (Namespace: `TFlex.DOCs.Common.Extensions`)
**Методы:**
- `IAsyncEnumerable`1 SkipNulls(IAsyncEnumerable`1 items)`
- `IAsyncEnumerable`1 ToTaskedAsyncEnumerable(IEnumerable`1 source)`

### `BuildStringExtensions` (Namespace: `TFlex.DOCs.Common.Extensions`)
**Методы:**
- `String BuildStringFromEnumerable(IEnumerable`1 collection)`
- `String BuildStringFromByteArray(Byte[] byteArray)`

### `CancellationTokenSourceExtensions` (Namespace: `TFlex.DOCs.Common.Extensions`)
**Методы:**
- `ICancellationTokenSource AsSafe(CancellationTokenSource cts)`
- `Void Close(CancellationTokenSource cts)`

### `CharExtensions` (Namespace: `TFlex.DOCs.Common.Extensions`)
**Методы:**
- `String ToBase64(Char c)`

### `DictionaryExtensions` (Namespace: `TFlex.DOCs.Common.Extensions`)
**Методы:**
- `Boolean ContainsKeyStringFrom(Dictionary`2 stringsList, List`1 comparedStrings, StringComparison comparison)`
- `Boolean TryAdd(IDictionary`2 dictionary, TKey key, Func`1 factory) (+1)`
- `TValue GetOrAdd(Dictionary`2 dictionary, TKey key, Func`1 factory) (+3)`
- `TValue GetOrAddWithWriteLock(IDictionary`2 dictionary, TKey key, Func`1 factory) (+1)`
- `TValue GetDictValueOrDefault(IDictionary`2 dictionary, TKey key, TValue defaultValue)`
- `Dictionary`2 ToChildToParentMap(IEnumerable`1 items, Func`2 childGetter)`
- `List`1 FindAll(IDictionary`2 dictionary, Func`3 predicate) (+2)`
- `Void RemoveAll(IDictionary`2 dictionary, Func`2 predicate) (+2)`
- `Void Add(IDictionary`2 dic, KeyValuePair`2 kvp)`

### `DisposableExtensions` (Namespace: `TFlex.DOCs.Common.Extensions`)
**Методы:**
- `IDisposable DoOnDispose(Action action)`
- `IDisposable Combine(IDisposable first, IDisposable[] second) (+1)`
- `Void SafeDisposeMany(IDisposable[] disposables) (+1)`

### `DynamicDataExtensions` (Namespace: `TFlex.DOCs.Common.Extensions`)
**Методы:**
- `Boolean IsNumeric(DynamicDataType currentType)`

### `EnumerableExtensions` (Namespace: `TFlex.DOCs.Common.Extensions`)
**Методы:**
- `ConcurrentDictionary`2 ToConcurrentDictionary(IEnumerable`1 items, Func`2 keySelector)`
- `HashSet`1 ConvertToHashSet(IEnumerable`1 items, Func`2 selector)`
- `Void ForEach(IEnumerable`1 items, Action`1 action) (+1)`
- `Boolean TrueForAll(IEnumerable`1 items, Func`2 action)`
- `Boolean AllAndNotEmpty(IEnumerable`1 items, Func`2 action)`
- `IEnumerable`1 Flatten(T startObj, Func`2 nextGetter, Func`2 predicate) (+1)`
- `IEnumerable`1 FlattenAggregated(T startObj, TAggregated seed, Func`3 nextGetter, Func`3 combiner, Func`2 predicate)`
- `IEnumerable`1 FlattenManyUnique(T startSequence, Func`2 predicate, Func`2 nextGetter, Func`2 keyGetter) (+7)`
- `IEnumerable`1 FlattenMany(IEnumerable`1 startSequence, Func`2 nextGetter, Func`2 historyItemGetter, Func`3 predicate) (+3)`
- `IEnumerable`1 AsEnumerable(T item)`
- `IEnumerable`1 SkipNulls(IEnumerable`1 items)`
- `IEnumerable`1 SafeCast(IEnumerable items)`
- `IEnumerable`1 Except(IEnumerable`1 items, T exceptedItem)`
- `IEnumerable`1 Concat(IEnumerable`1 items, T addedItem)`
- `T MinOrDefault(IEnumerable`1 items, Func`2 valueGetter, T defaultValue)`
- `T MaxOrDefault(IEnumerable`1 items, Func`2 valueGetter, T defaultValue)`
- `T MinItem(IEnumerable`1 items, Func`2 keyGetter, T defaultValue)`
- `T MaxItem(IEnumerable`1 items, Func`2 keyGetter, T defaultValue)`
- `IEnumerable`1 Do(IEnumerable`1 items, Action`1 action)`
- `IEnumerable`1 Distinct(IEnumerable`1 items, Func`2 getter) (+1)`
- `IEnumerable`1 DistinctByReferenceEquals(IEnumerable`1 items)`
- `IEnumerable`1 DistinctChunked(IEnumerable`1 items, Func`2 getter)`
- `IList`1 Duplicates(IEnumerable`1 items, Func`2 keyGetter)`
- `ChunkedList`1 ToChunkedList(IEnumerable`1 items, Int32 chunkSizePower)`
- `IEnumerable`1 MergeOrdered(IEnumerable`1 a, IEnumerable`1 b, Func`2 keyGetter, IComparer`1 comparer) (+1)`
- `TOut Median(IEnumerable`1 items, Func`2 selector, Func`2 creator) (+1)`
- `Double KahanSum(IEnumerable`1 items)`
- `T[] TakeArray(IEnumerable`1 items)`
- `IList`1 TakeIList(IEnumerable`1 items)`
- `IList`1 TakeIListWithOrderBy(IEnumerable`1 items)`
- `List`1 TakeList(IEnumerable`1 items)`
- `ICollection`1 TakeCollection(IEnumerable`1 items)`
- `IReadOnlyCollection`1 TakeReadOnlyCollection(IEnumerable`1 items) (+1)`
- `IReadOnlyList`1 TakeReadOnlyList(IEnumerable`1 items)`
- `IEnumerable`1 TakePairs(IEnumerable`1 items)`

### `EnumMemberAttributesExtension` (Namespace: `TFlex.DOCs.Common.Extensions`)
**Методы:**
- `TAttr GetAttributeOfType(TEnum enumVal)`

### `EventExtensions` (Namespace: `TFlex.DOCs.Common.Extensions`)
**Методы:**
- `Void Raise(EventHandler eventHandler, Object sender, EventArgs eventArgs) (+1)`

### `FrameworkExtensions` (Namespace: `TFlex.DOCs.Common.Extensions`)
**Методы:**
- `String GetName(Type type)`
- `String TrimLines(String value)`
- `List`1 RemoveRange(List`1 source, List`1 remove)`
- `Void SafeSet(EventWaitHandle waitEvent)`
- `String GetCompressedTypeName(Type type, Boolean trimVersion)`
- `String GetDisplayName(Type type)`
- `String GetSimpleName(Type type)`
- `Boolean IsInherit(Type type, Type baseType) (+1)`
- `Boolean IsNullable(Type type)`
- `Type GetGenericArgumentType(Type type)`
- `Type GetGenericArgumentValueType(Type type)`
- `Type[] GetGenericArgumentTypes(Type type)`
- `Boolean IsCollectionItemType(Type type)`
- `Boolean TryGetCollectionItemType(Type type, Type& elementType)`
- `Boolean TryGetDictionaryItemType(Type type, Type& keyType, Type& valueType)`
- `Boolean IsSystemType(Type type)`
- `Boolean IsSimpleType(Type type)`
- `IEnumerable`1 GetAttributes(MemberInfo member)`
- `T GetAttribute(MemberInfo member)`
- `Boolean HasAttribute(MemberInfo member)`

### `HashSetExtensions` (Namespace: `TFlex.DOCs.Common.Extensions`)
**Методы:**
- `Void AddRange(HashSet`1 hashSet, IEnumerable`1 items)`
- `Void RemoveRange(HashSet`1 hashSet, IEnumerable`1 items)`

### `IsIdentityAttributeExtension` (Namespace: `TFlex.DOCs.Common.Extensions`)
**Методы:**
- `Boolean IsIdentity(T systemField)`

### `MethodInfoExtension` (Namespace: `TFlex.DOCs.Common.Extensions`)
**Методы:**
- `Void RedirectTo(MethodInfo origin, MethodInfo target)`

### `MethodInfoExtensions` (Namespace: `TFlex.DOCs.Common.Extensions`)
**Методы:**
- `T CreateDelegate(MethodInfo method)`
- `Func`3 CreateFunc(MethodInfo method)`
- `Action`3 CreateAction(MethodInfo method)`

### `NullableAttributeExtension` (Namespace: `TFlex.DOCs.Common.Extensions`)
**Методы:**
- `Boolean IsNullable(T systemField)`

### `ObjectsExtensions` (Namespace: `TFlex.DOCs.Common.Extensions`)
**Методы:**
- `Boolean IsDefault(T obj)`
- `Boolean IsNotDefault(T obj)`
- `Boolean OneOf(T obj, T[] set) (+1)`
- `Boolean NotOneOf(T obj, T[] set)`
- `Boolean AllOf(T obj, T[] set)`

### `ProcessStartExtensions` (Namespace: `TFlex.DOCs.Common.Extensions`)
**Методы:**
- `Int32 Execute(String filePath, String parameters, StringBuilder& output, Boolean hidden, ProcessWindowStyle windowStyle, Int32 timeOut, Boolean killAfterTimeout, String[] envVariables) (+1)`
- `Process CreateProcess(String filePath, String parameters, Boolean createNoWindow, ProcessWindowStyle windowStyle, Boolean redirectStandardOutput, Boolean redirectStandardError, Boolean start, String[] envVariables)`

### `PropertyInfoExtensions` (Namespace: `TFlex.DOCs.Common.Extensions`)
**Методы:**
- `Func`2 BuildTypedGetter(PropertyInfo propertyInfo)`
- `Action`2 BuildTypedSetter(PropertyInfo propertyInfo)`

### `RandomExtensions` (Namespace: `TFlex.DOCs.Common.Extensions`)
**Методы:**
- `Decimal NextDecimal(Random rnd, Decimal max, Decimal min, Decimal step)`
- `TimeSpan NextTimeSpan(Random rnd, TimeSpan max, TimeSpan min, TimeSpan step)`
- `Int64 NextLong(Random rnd, Int64 max)`
- `T NextEnum(Random rnd, T[] exceptedValues)`
- `Boolean NextBool(Random rnd)`

### `RealNumbersExtensions` (Namespace: `TFlex.DOCs.Common.Extensions`)
**Методы:**
- `Boolean IsApproximatelyEqualTo(Single initialValue, Single value, Single maximumDifferenceAllowed) (+1)`

### `RwrLockSlimExtensions` (Namespace: `TFlex.DOCs.Common.Extensions`)
**Методы:**
- `Void ReadAction(ReaderWriterLockSlim lockItem, Action action)`
- `T1 ReadFunc(ReaderWriterLockSlim lockItem, Func`1 func)`
- `Void WriteAction(ReaderWriterLockSlim lockItem, Action action)`
- `T1 WriteFunc(ReaderWriterLockSlim lockItem, Func`1 func)`

### `SpanSplitterExtensions` (Namespace: `TFlex.DOCs.Common.Extensions`)
**Методы:**
- `SpanSplitter`1 Split(ReadOnlySpan`1 source, ReadOnlySpan`1 separator) (+1)`

### `SplitBySegmentsExtensions` (Namespace: `TFlex.DOCs.Common.Extensions`)
**Методы:**
- `IEnumerable`1 SplitBySegments(IEnumerable`1 source, Int32 pageSize)`

### `StreamExtension` (Namespace: `TFlex.DOCs.Common.Extensions`)
**Методы:**
- `Boolean IsPackageStream(Stream dataStream)`
- `Byte[] ReadExactly(Stream stream, Int32 count)`
- `Boolean StartsWith(Stream stream, Byte[] expectedData)`
- `MemoryStream ToMemoryStream(Stream stream)`
- `String ToBase64String(MemoryStream stream)`
- `String ToUnicodeString(MemoryStream stream)`

### `StringBuilderExtensions` (Namespace: `TFlex.DOCs.Common.Extensions`)
**Методы:**
- `StringBuilder AppendLineWithIndent(StringBuilder builder, String value, String indent) (+1)`
- `StringBuilder TrimEnd(StringBuilder builder)`

### `StringComparerExtensions` (Namespace: `TFlex.DOCs.Common.Extensions`)
**Методы:**
- `StringComparer GetEqualityComparer(StringComparison comparison)`

### `StringExtensions` (Namespace: `TFlex.DOCs.Common.Extensions`)
**Методы:**
- `Boolean IsNullOrEmpty(String s)`
- `Boolean IsNullOrWhiteSpace(String s)`
- `Boolean IsNotNullOrWhiteSpace(String s)`
- `Boolean IsNotNullOrEmpty(String s)`
- `String Join(IEnumerable`1 source, String separator)`
- `Int32 GetMaxLineWidth(String s)`
- `String FormatWith(String s, Object[] args)`
- `String RemoveSymbols(String s, Func`2 predicateToDelete)`
- `String ReplaceSymbols(String s, Func`2 replacer)`
- `String EscapeSymbolsBase64(String s, Func`2 predicateToEscape)`
- `String NormalizePath(String path)`
- `String GetCanonicalPath(String path)`
- `String GetShortFilePath(String path)`
- `Boolean ContainsChars(String str, Char[] values)`
- `Guid GetGuid(String guid)`
- `Double GetDoublePart(String input)`

### `TaskEventHandler`2` (Namespace: `TFlex.DOCs.Common.Extensions`)
**Методы:**
- `ValueTask`1 Invoke(Object sender, TEventArgs e)`
- `IAsyncResult BeginInvoke(Object sender, TEventArgs e, AsyncCallback callback, Object object)`
- `ValueTask`1 EndInvoke(IAsyncResult result)`

### `TaskNoLockCall` (Namespace: `TFlex.DOCs.Common.Extensions`)
**Методы:**
- `Void Execute(Func`13 call, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, T8 param8, T9 param9, T10 param10, T11 param11, T12 param12) (+55)`

### `TasksExtensions` (Namespace: `TFlex.DOCs.Common.Extensions`)
**Методы:**
- `Task StartOnDefaultScheduler(Action`2 action, TState1 state1, TState2 state2) (+4)`
- `Task StartWithDelay(TimeSpan delayTime) (+1)`
- `Task`1 OnComplete(Task`1 sourceTask, Action`2 continuation, TState state) (+1)`
- `Task OnFault(Task sourceTask, Action`1 continuation) (+1)`
- `Task`1 WhenAll(Task`1 resultGetter, Task[] additionalAwaiters)`
- `Task Subscribe(Task task, TaskCompletionSource`1 tcs, Boolean tryCastToGeneric, TResult successValue) (+2)`
- `Task`1 ToGeneric(Task task, T successValue) (+1)`
- `Task WithCancel(Task task, CancellationToken token) (+1)`

### `TaskSyncCall` (Namespace: `TFlex.DOCs.Common.Extensions`)
**Методы:**
- `Void Execute(Func`13 call, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, T8 param8, T9 param9, T10 param10, T11 param11, T12 param12) (+55)`

### `TypeExtensions` (Namespace: `TFlex.DOCs.Common.Extensions`)
**Методы:**
- `Boolean IsNumeric(Type currentType)`
- `Type GetNonTaskType(Type type)`
- `Type GetCommonType(Type[] types)`

### `TypeReflectionExtensions` (Namespace: `TFlex.DOCs.Common.Extensions`)
**Методы:**
- `Func`2 GetPropertyGetter(Type type, String propertyName)`
- `Type GetPropertyType(Type type, String propertyName)`
- `Func`3 GetFunc(Type type, String methodName)`
- `Action`3 GetAction(Type type, String methodName)`

### `ValidationResultExtensions` (Namespace: `TFlex.DOCs.Common.Extensions`)
**Методы:**
- `String GetErrorTextOrNull(ValidationResult vr)`
- `String GetWarningTextOrNull(ValidationResult vr)`
- `ValidationResult AddObjects(ValidationResult vr, Object[] objects)`
- `ValidationResult FilterByObjectsAll(ValidationResult vr, Func`2 predicate)`
- `ValidationResult FilterByObjectsAny(ValidationResult vr, Func`2 predicate)`
- `ValidationResult FilterByObjectsAnyOr(ValidationResult vr, Func`2 predicate1, Func`2 predicate2)`

### `EnumDescriptionHelper`1` (Namespace: `TFlex.DOCs.Common.Extensions.AdvancedEnumsExtensions`)
**Методы:**
- `String ToDescription(T value, String key)`
- `Boolean TryParse(String s, Boolean useDescriptions, T& value)`
- `T Parse(String s, Boolean useDescriptions)`

### `EnumHelper`1` (Namespace: `TFlex.DOCs.Common.Extensions.AdvancedEnumsExtensions`)
**Свойства:** AllValues: T[], ZeroValue: T, AllNames: ICollection`1, Count: Int32
**Методы:**
- `Dictionary`2 MapAllValues()`
- `Dictionary`2 MapValuesWithExclude(T[] excludeValues)`
- `String ToStringFast(T value, String delimeter) (+1)`

### `KeyDescriptionAttribute` (Namespace: `TFlex.DOCs.Common.Extensions.AdvancedEnumsExtensions`)
**Свойства:** Key: String, IsDefault: Boolean

### `ResourceKeyDescriptionAttribute` (Namespace: `TFlex.DOCs.Common.Extensions.AdvancedEnumsExtensions`)
**Свойства:** DefaultKey: String, Description: String

### `BasicAuthenticationCredentials` (Namespace: `TFlex.DOCs.Common.FullTextSearch`)
**Свойства:** UserName: String, Password: String
**Методы:**
- `Void Save(RegistryKey key) (+1)`
- `Void Load(RegistryKey key) (+1)`

### `FullTextSearchConnectionSettings` (Namespace: `TFlex.DOCs.Common.FullTextSearch`)
**Свойства:** ServerAddress: String, BasicAuthenticationEnabled: Boolean, BasicAuthenticationCredentials: BasicAuthenticationCredentials
**Методы:**
- `Void Save(RegistryKey key) (+1)`
- `Void Load(RegistryKey key) (+1)`

### `AdvancedConsoleCursor` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Свойства:** Length: Int32
**Методы:**
- `AdvancedConsoleCursor Get(Int32 length)`
- `Void Set(AdvancedConsoleCursor cursor)`

### `ApplicationChecker` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `UiAssembliesCheckResult HasUiAssemblies(String appName, String assemblyPath)`

### `ArrayBuffers`1` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `T[] RentStrong(Int32 length) (+1)`
- `Void ReturnStrong(T[] array)`
- `Void ClearStrong(T[]& array)`
- `ArraySegment`1 RentSegment(Int32 length)`
- `Void ReturnSegment(ArraySegment`1 buffer, Boolean clearArray)`
- `Void ClearSegment(ArraySegment`1& buffer, Boolean clearArray)`
- `T[] RentCheck(Int32 minimumLength) (+1)`
- `Void ReturnCheck(T[] array, Boolean clearArray)`
- `Void ClearCheck(T[]& array, Boolean clearArray)`
- `T[] Rent(Int32 minimumLength) (+1)`
- `Void Return(T[] array, Boolean clearArray)`
- `Void Clear(T[]& array, Boolean clearArray)`

### `BitFlagSwitcher` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `Int32 TurnBitOn(Int32 value, Int32 bitToTurnOn) (+1)`
- `Int32 TurnBitOff(Int32 value, Int32 bitToTurnOff) (+1)`
- `Int32 FlipBit(Int32 value, Int32 bitToFlip) (+1)`

### `ByteArrayComparer` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Свойства:** Instance: ByteArrayComparer

### `BytesConverter` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `Byte[] Get(Int32 value) (+5)`
- `Int32 ReadInt32(Byte[]& bytes, Int32 offset)`
- `Int32[] ReadInt32Array(Byte[]& bytes, Int32 length, Int32 offset)`
- `Int64[] ReadInt64Array(Byte[]& bytes, Int32 length, Int32 offset)`

### `CancellationContext` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `T RunWithCancel(CancellationToken token, Func`2 operation) (+3)`

### `ChunkStream` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Свойства:** Position: Int64, Length: Int64, CanWrite: Boolean, CanTimeout: Boolean, CanSeek: Boolean, CanRead: Boolean, ReadTimeout: Int32, WriteTimeout: Int32
**Методы:**
- `Void Flush()`
- `Int32 Read(Byte[] buffer, Int32 offset, Int32 count)`
- `Int64 Seek(Int64 offset, SeekOrigin origin)`
- `Void SetLength(Int64 value)`
- `Void Write(Byte[] buffer, Int32 offset, Int32 count)` [has Async]

### `Clock` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `DateTime GetNow()`

### `ConcatenatedStream` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Свойства:** CanRead: Boolean, CanSeek: Boolean, CanWrite: Boolean, Length: Int64, Position: Int64, LeaveOpen: Boolean
**Методы:**
- `Stream Create(IEnumerable`1 streams, Int32 capacity) (+4)`
- `Stream CreateLeaveOpen(IEnumerable`1 streams, Int32 capacity) (+2)`
- `Void Add(Stream stream) (+1)`
- `Stream Optimize()`
- `Boolean TryGetBufferStream(MemoryStream& output)`
- `Boolean TryGetStreams(IReadOnlyCollection`1 lengths, List`1& streams) (+1)`
- `Void Flush()`
- `Int32 Read(Byte[] buffer, Int32 offset, Int32 count)`
- `Int64 Seek(Int64 offset, SeekOrigin origin)`
- `Void SetLength(Int64 value)`
- `Void WriteByte(Byte value)`
- `Void Write(Byte[] buffer, Int32 offset, Int32 count)`

### `ConsoleCursor` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Свойства:** Left: Int32, Top: Int32, ForegroundColor: ConsoleColor
**Методы:**
- `ConsoleCursor Get()`
- `Void Set(ConsoleCursor cursor)`

### `ConsoleExceptionsHelper` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `Void SetUnhandledExceptionHandler(Action`2 handler) (+1)`
- `Void UnhandledExceptionHandler(Object sender, UnhandledExceptionEventArgs args)`
- `String GetExceptionMessage(UnhandledExceptionEventArgs exceptionObject)`

### `ConsoleHelper` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Свойства:** CursorLeft: Int32, CursorTop: Int32, BufferWidth: Int32, BufferHeight: Int32, TextLength: Int32
**Методы:**
- `Void Write(Int32 x, Int32 y, String text, ConsoleColor color) (+1)`
- `Void WriteLn(String text, ConsoleColor color)`
- `String CreateProgressBar(Int32 position, Int32 max, Byte steps, String progressFormat, Char progresSymbol, Char spaceSympol)`
- `Void SetFullScreen()`

### `ConsoleOutputHelper` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Свойства:** SilentMode: Boolean, Current: String, Tick: String, Fail: String, CursorLeft: Int32, CursorTop: Int32, BufferWidth: Int32, BufferHeight: Int32
**Методы:**
- `Void WriteCurrent(Int32 x, Int32 y, String current, ConsoleColor currentColor)`
- `Void WriteTick(Int32 x, Int32 y, String tick, ConsoleColor tickColor)`
- `Void WriteFail(Int32 x, Int32 y, String fail, ConsoleColor failColor)`
- `Void WriteLine()`
- `Void WriteAboutInfo(String consoleAppName)`
- `Void WriteArgumentsInfo(Boolean closeApp)`
- `Void WriteParsedArgumentsInfo(List`1 switches, String configurationFilePath)`
- `Void Write(String text, Nullable`1 color)`
- `Void WriteLn(String text, Nullable`1 color)`
- `Void WriteInfo(String text, ConsoleColor color)`
- `Void WriteWarning(String warningText, ConsoleColor color)`
- `Void WriteError(String errorText, Boolean withNewLine, ConsoleColor color)`
- `Void PressAnyKey()`
- `Void SubscribeOutput(IConsoleActionsOutput actionsOutput)`
- `Void UnsubscribeOutput(IConsoleActionsOutput actionsOutput)`
- `Void OnActionInactive(Object sender, String message)`
- `Object OnActionStarted(Object sender, String message)`
- `Object OnActionShowProgress(Object sender, String progressBar, String message)`
- `Void OnActionHideProgress(Object sender, Object tag)`
- `Void OnActionFinished(Object sender, Object tag)`
- `Void OnActionFailed(Object sender, Object tag)`

### `DataTransferMonitor` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `DataTransferStatistics GetStatistics()`
- `Void UpdateStatistics(Int32 bytesTransferred)`

### `DataTransferStatistics` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Свойства:** TotalBytesTransferred: Int32, TotalBytes: Int32, CompletionPercentage: Int32, EstimatedTimeLeft: Nullable`1

### `DataTransferStatisticsExtension` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `String FormatTransferredToTotal(DataTransferStatistics statistics)`
- `String FormatCompletionPercentage(DataTransferStatistics statistics)`
- `String FormatEstimatedTimeLeft(DataTransferStatistics statistics)`

### `DateTimeHelper` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `Int32 GetDayOfWeekNumber(DayOfWeek dayOfWeek, DayOfWeek firstDayOfWeek)`
- `DateTime GetStartOfWeek(DateTime dt, DayOfWeek firstDayOfWeek)`
- `DateTime GetEndOfWeek(DateTime dt, DayOfWeek firstDayOfWeek, Boolean round)`
- `DateTime GetStartOfMonth(DateTime dt)`
- `DateTime GetEndOfMonth(DateTime dt)`
- `DateTime GetStartOfQuarter(DateTime dt)`
- `DateTime GetEndOfQuarter(DateTime dt)`
- `DateTime GetStartOfYear(DateTime dt)`
- `DateTime GetEndOfYear(DateTime dt)`
- `DateTime Min(DateTime date1, DateTime date2)`
- `DateTime Max(DateTime date1, DateTime date2)`

### `DoubleHelper` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `Boolean AreClose(Double value1, Double value2, Double tolerance)`
- `Int32 Compare(Double value1, Double value2, Double tolerance)`
- `Int32 Divide(Double value, Double divisor, Double tolerance)`
- `Double Reminder(Double value, Double divisor, Double tolerance)`

### `EntityNameGenerator` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `String FindFreeNameInList(ICollection`1 names, String prefixName, Boolean startsWithNumber) (+1)`

### `ExceptionHelper` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `String ToStringCustom(Exception exception)`
- `String GetMessage(Exception exception)`
- `String GetMessagesAsTree(Exception exception)`
- `String GetAggregatedMessagesTreeText(Exception exception, Boolean withMinimalLeftOffset) (+1)`
- `String ExceptionToString(Exception exception, String messageType, String customMessage, Boolean rootException, Boolean includeSystemInfo, Boolean fullInfo)`
- `Void FormatLogMessage(String messageType, Action`1 getMessage, Action`1 write, Boolean includeSystemInfo) (+1)`

### `ExecutionContextHelper` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `IActionContextBuilder Make(Action`2 contextProvider, T contextArg) (+3)`

### `ExecutionContextHelper`1` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `IFuncContextBuilder Make(Func`3 contextProvider, T contextArg) (+3)`

### `FileExtensionHelper` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `String GetDescription(String fileExtension)`

### `FileHelper` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `Int32 GetLineCount(String filePath)`

### `FilesByMaskSelector` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `FilesByMaskSelector Create(IEnumerable`1 folders, IEnumerable`1 fileMasks, IEnumerable`1 excludeFileMasks, Boolean recursive, Boolean excludedMasksIgnoreCase) (+4)`
- `IEnumerable`1 CreateEnumerable(IEnumerable`1 folders, IEnumerable`1 fileMasks, IEnumerable`1 excludeFileMasks, Boolean recursive, Boolean excludedMasksIgnoreCase) (+6)`
- `IEnumerator`1 GetEnumerator()`

### `FilesManager` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `TemporaryFile CreateTemporaryFileWithSpecificExtension(String extension, String tempFileDirectory)`
- `Boolean TryGetTemporaryFile(String fileName, TemporaryFile& tempFile, String tempFileDirectory)`
- `TemporaryFile GetOrCreateTemporaryFile(String fileName, String tempFileDirectory)`
- `Boolean Exists(String fileName, String tempFileDirectory)`
- `Void ClearTempDirectory(String directoryPath)`
- `Void CloseTempFile(String fileName, String tempFileDirectory)` [has Async]
- `Void Close()` [has Async]

### `FileSystemHelper` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `String ReplaceInvalidChars(String fileName, Boolean trimDot)`
- `String ReplaceXhtmlSpecialChars(String content)`
- `Void DeleteFile(FileInfo fileInfo)`
- `Void DeleteDirectoryRecursively(DirectoryInfo directoryInfo)`
- `Void DeleteDirectory(DirectoryInfo directoryInfo)`
- `Void ReplaceSymbolicLinkWithFileCopy(String filePath)`
- `Void CreateSymbolicLink(String linkPath, String targetPath)`

### `FlashWindow` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `Boolean FlashWindowEx(IntPtr hWnd)`

### `GraphAlgorithms` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `List`1 TopologicalSort(IEnumerable`1 nodes, IEnumerable`1 edges)`
- `IEnumerable`1 EnumerateGraphInWigth(TNode from, Func`2 neighborsGetter)`
- `List`1 FindPath(TNode from, TNode to, Func`2 neighborsGetter, Func`2 predicate)`
- `ValueTuple`2 FindMaxFlowMinCut(TNode from, TNode to, Func`2 neighborsGetter, Func`2 weightGetter)`
- `ValueTuple`4 GetGraphPartition(TNode from, TNode to, IList`1 nodesToCut, Func`2 neighborsGetter, Func`2 inverseNeighborsGetter)`

### `HierarchyHelper` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `List`1 GetRootsByParents(IEnumerable`1 objects, Func`2 keyGetter, Func`2 parentsGetter)`
- `List`1 GetRoots(IEnumerable`1 objects, Func`2 keyGetter, Func`2 childsGetter)`
- `List`1 GetLeaves(IEnumerable`1 objects, Func`2 keyGetter, Func`2 childsGetter)`
- `List`1 GetLeavesByParents(IEnumerable`1 objects, Func`2 keyGetter, Func`2 parentsGetter)`
- `IEnumerable`1 DepthFlattenTreeByParent(IEnumerable`1 items, Func`2 keyGetter, Func`2 parentGetter, FlattenParentPosition parentPosition)`
- `IEnumerable`1 DepthFlattenTree(IEnumerable`1 items, Func`2 keyGetter, Func`2 childGetter, FlattenParentPosition parentPosition) (+1)`

### `IClock` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `DateTime GetNow()`

### `IConsoleActionsOutput` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Свойства:** UnsubscribeOutput: Action

### `ImageOperations` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `Image ConvertIconToImage(Icon icon, Size size, Boolean deleteSourceIcon)`
- `Image GetGrayscaleImage(Icon sourceIcon, Size size, Boolean deleteSourceIcon) (+1)`
- `Byte[] ConvertImageToByteArray(Image bitmap)`
- `Icon GetSolidIcon(Size size, Int32 rgbColor, Int32 transparentBorderWidth, Int32 borderWidth, Int32 blur)`
- `Byte[] ConvertWindowsFormatImageToPng(Byte[] sourceBytes)`
- `Icon PngIconFromImage(Image img, Int32 size)`
- `Icon GetIconFromBitmap(Bitmap bitmap, Int32 size)`
- `Byte[] ConvertImageToIconByteArray(Byte[] pngImage, Int32 size)`

### `InSyncRunHelper` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Свойства:** InSync: Boolean
**Методы:**
- `Void RunInSync(Action act)`
- `Boolean RunWithChangeCounter(Action act)`
- `Void SyncAll(Action[] actions)`

### `IntHelper` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `Int32 Clamp(Int32 value, Int32 minValue, Int32 maxValue)`

### `JsonHelper` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `Boolean IsJson(String text)`

### `ListAdapter`2` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Свойства:** Count: Int32, IsReadOnly: Boolean, Item: TExternal
**Методы:**
- `Void Add(TExternal item)`
- `Void Clear()`
- `Boolean Contains(TExternal item)`
- `Int32 IndexOf(TExternal item)`
- `Void Insert(Int32 index, TExternal item)`
- `Boolean Remove(TExternal item)`
- `Void RemoveAt(Int32 index)`
- `Void CopyTo(TExternal[] array, Int32 arrayIndex)`
- `IEnumerator`1 GetEnumerator()`

### `ListToHierarchyLevelsUnwrapper` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `IEnumerable`1 Build(IReadOnlyCollection`1 dataObjects, Func`2 getKey, Func`2 getParentKey)`

### `MathHelper` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `Int32 Gcd(Int32 x, Int32 y)`
- `Int32 Combine(Int32 a, Int32 b)`

### `Md5Helper` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `Byte[] GetHashBytes(Byte[] bytes) (+1)`
- `String GetHashString(Byte[] bytes) (+1)`
- `String GetMD5Hash(String sourceString)`

### `NetworkConnectionsHelper` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `List`1 GetAllTcpConnections()`
- `List`1 GetAllUdpConnections()`

### `Networks` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `Boolean IsTcpPortBusy(Int32 port)`
- `Int32 GetAvailableDynamicTcpPort()`
- `Int32 GetAvailableTcpPort(Int32 startingPort)`
- `IPAddress GetMachineIPAddress()`
- `IPAddress GetLoopbackIPAddress()`
- `PhysicalAddress GetCurrentMacAddress()`
- `Void Reset()`

### `NoCloseStream` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Свойства:** Position: Int64, Length: Int64, CanWrite: Boolean, CanTimeout: Boolean, CanSeek: Boolean, CanRead: Boolean, ReadTimeout: Int32, WriteTimeout: Int32
**Методы:**
- `IAsyncResult BeginRead(Byte[] buffer, Int32 offset, Int32 count, AsyncCallback callback, Object state)`
- `IAsyncResult BeginWrite(Byte[] buffer, Int32 offset, Int32 count, AsyncCallback callback, Object state)`
- `Int32 EndRead(IAsyncResult asyncResult)`
- `Void EndWrite(IAsyncResult asyncResult)`
- `Void Flush()` [has Async]
- `Int32 Read(Byte[] buffer, Int32 offset, Int32 count)` [has Async]
- `Int32 ReadByte()`
- `Int64 Seek(Int64 offset, SeekOrigin origin)`
- `Void SetLength(Int64 value)`
- `Void Write(Byte[] buffer, Int32 offset, Int32 count)` [has Async]
- `Void WriteByte(Byte value)`

### `NonGenericTaskExecutor` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `Object Run(Object value)` [has Async]

### `ObjectHelper` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `TElement[] EmptyArray()`
- `IDictionary`2 EmptyDictionary()`
- `ReadOnlyCollection`1 EmptyReadOnlyCollection()`
- `TElement[] ToArray(TElement source) (+1)`
- `IEnumerable`1 CastToEnumerable(Object source)`
- `Array CastArrayType(Type newItemType, Object[] array) (+2)`
- `IList CastListTypeOrNull(IEnumerable enumerable, Type defaultItemType) (+2)`
- `Object[] CastToObjectArray(Object arrayValue)`
- `IReadOnlyList`1 CastToObjectReadOnlyList(Object arrayValue)`
- `Object GetDefaultValue(Type type)`
- `Byte[] GetCollectionBytes(ICollection`1 collection)`
- `Type GetElementType(IEnumerable enumerable)`

### `OnActionFailedDelegate` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `Void Invoke(Object sender, Object tag)`
- `IAsyncResult BeginInvoke(Object sender, Object tag, AsyncCallback callback, Object object)`
- `Void EndInvoke(IAsyncResult result)`

### `OnActionFinishedDelegate` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `Void Invoke(Object sender, Object tag)`
- `IAsyncResult BeginInvoke(Object sender, Object tag, AsyncCallback callback, Object object)`
- `Void EndInvoke(IAsyncResult result)`

### `OnActionHideProgressDelegate` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `Void Invoke(Object sender, Object tag)`
- `IAsyncResult BeginInvoke(Object sender, Object tag, AsyncCallback callback, Object object)`
- `Void EndInvoke(IAsyncResult result)`

### `OnActionInactiveDelegate` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `Void Invoke(Object sender, String message)`
- `IAsyncResult BeginInvoke(Object sender, String message, AsyncCallback callback, Object object)`
- `Void EndInvoke(IAsyncResult result)`

### `OnActionShowProgressDelegate` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `Object Invoke(Object sender, String progressBar, String message)`
- `IAsyncResult BeginInvoke(Object sender, String progressBar, String message, AsyncCallback callback, Object object)`
- `Object EndInvoke(IAsyncResult result)`

### `OnActionStartedDelegate` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `Object Invoke(Object sender, String message)`
- `IAsyncResult BeginInvoke(Object sender, String message, AsyncCallback callback, Object object)`
- `Object EndInvoke(IAsyncResult result)`

### `PathBuilder` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `String RelativePath(String fromPath, String toPath, Boolean baseIsDirectory, Boolean makeCanonical) (+2)`

### `PathHelper` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Свойства:** AssemblyDirectory: String
**Методы:**
- `String GetProductAppDataFolder(AppDataFolderType folderType)`
- `String EscapePathInvalidChars(String s)`
- `String EscapeFileNameInvalidChars(String s)`
- `String TrimFileNameToMaxPathLength(String directory, String fileName)`
- `String GetEntryFileFullPath()`
- `String PathAddBackslash(String path)`
- `String PathRemoveBackslash(String path)`
- `String GetPlatformDependentPath(String path)`
- `Boolean IsASCIIPath(String path)`
- `Boolean IsValidPath(String path)`
- `Boolean IsValidFilePath(String filePath)`
- `Boolean IsValidDirectoryPath(String directoryPath)`
- `String GetLongPath(String path, Boolean required)`
- `Boolean IsLongPathEquals(String pathA, String pathB)`
- `String EncodeURI(String str)`

### `PortNumberValidator` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `Boolean CheckPortInRangeOfPossibleValues(Int32 port)`

### `ProgressCounter` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Свойства:** CurrentPercent: Double, MaxCount: Double
**Методы:**
- `ProgressCounter GetSubCounter(Double parentPart, Double childMaxCount, Action`1 countAction)`
- `Void Count(Double count)`
- `Void Refresh()`

### `ProxyBufferStream` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Свойства:** Position: Int64, Length: Int64, CanWrite: Boolean, CanTimeout: Boolean, CanSeek: Boolean, CanRead: Boolean, ReadTimeout: Int32, WriteTimeout: Int32
**Методы:**
- `IAsyncResult BeginRead(Byte[] buffer, Int32 offset, Int32 count, AsyncCallback callback, Object state)`
- `IAsyncResult BeginWrite(Byte[] buffer, Int32 offset, Int32 count, AsyncCallback callback, Object state)`
- `Void Close()`
- `Int32 EndRead(IAsyncResult asyncResult)`
- `Void EndWrite(IAsyncResult asyncResult)`
- `Void Flush()` [has Async]
- `Int32 Read(Byte[] buffer, Int32 offset, Int32 count)` [has Async]
- `Int32 ReadByte()`
- `Int64 Seek(Int64 offset, SeekOrigin origin)`
- `Void SetLength(Int64 value)`
- `Void Write(Byte[] buffer, Int32 offset, Int32 count)` [has Async]
- `Void WriteByte(Byte value)`

### `ServiceDispatcher` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Свойства:** IsRunning: Boolean, TraceLogger: Action`1
**Методы:**
- `String GetImagePath()`
- `Boolean IsCurrentVersion()`
- `Void Start(Int32 timeOut)`
- `Void Stop(Int32 timeOut)`

### `ShaHelper` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `Byte[] GetHashBytes(Byte[] bytes) (+1)`
- `String GetHashString(Byte[] bytes) (+1)`
- `String GetSha512Hash(String sourceString)`
- `String GetCollectionHash(ICollection`1 collection)`
- `Guid GetCollectionHashAsGuid(ICollection`1 collection)`

### `StartupArgumentsParser` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `String Parse(String[] args, List`1 possibleSwitches, List`1& cmdSwitches)`
- `String ParseAsDictionary(String[] args, List`1 possibleSwitches, Dictionary`2& cmdSwitches)`

### `StopwatchHelper` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `Int64 StopwatchTicksToDateTimeTicks(Int64 stTicks)`
- `Int64 DateTimeTicksToStopWatch(Int64 dtTicks)`
- `TimeSpan StopwatchTicksToTimeSpan(Int64 stTicks)`
- `Int64 TimeSpanToStopwatchTicks(TimeSpan span)`

### `StreamHelper` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `Void Copy(Stream source, Stream destination, Int32 bufferSize, Boolean autoBufferSize, Boolean seekToBegin) (+1)` [has Async]
- `Byte[] ToBytes(Stream source, Int32 bufferSize, Boolean seekToBegin)` [has Async]
- `ArraySegment`1 ToSegmentBytes(Stream source, Int32 bufferSize)`
- `Byte[] Convert(Byte[] bytes, Func`2 func) (+1)`
- `Byte[] ConvertToBytes(ArraySegment`1 buffer)`
- `ArraySegment`1 Combine(ArraySegment`1 first, ArraySegment`1 second, Int32 firstOffset) (+3)`
- `ArraySegment`1 CombineRent(ArraySegment`1 first, ArraySegment`1 second, Int32 firstOffset)`
- `Byte[] Trim(Byte[] buffer, Int32 bytesRead)`
- `Boolean EqualBytesLongUnrolled(Byte[] data1, Byte[] data2)`
- `IReadOnlyList`1 ReadStreams(Stream source, IReadOnlyCollection`1 lengths, Int32 bufferSize) (+1)` [has Async]
- `Void SaveToFile(String filePath, Stream stream)`
- `FileStream Open(String filePath)`

### `StringsDatabaseHelper` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `String PrepareString(String preparedString)`
- `String PrepareStringSQL(String preparedString)`
- `String GetDatabaseFriendlyName(String sourceName, String prefixForDigits, String defaultName)`

### `StringsDateHelper` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `String DateToEditableStr(DateTime value)`
- `DateTime DateFromEditableStr(String value, DateTime defValue)`
- `String DateToStr(DateTime value)`
- `String DateToStrHex(DateTime value)`
- `DateTime DateFromStr(String value)`
- `DateTime DateFromStrHex(String value)`

### `TaskbarProgress` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `Void SetState(IntPtr windowHandle, TaskbarStates taskbarState)`
- `Void SetValue(IntPtr windowHandle, Double progressValue, Double progressMax)`

### `TaskHelpers` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `Task Canceled() (+1)`
- `Task Completed()`
- `Task FromError(Exception exception) (+1)`
- `Task`1 NullResult()`

### `TemporaryFile` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Свойства:** Name: String, Directory: String, FullPath: String, Guid: Guid
**Методы:**
- `FileStream OpenFileStream(FileMode mode, FileAccess access, FileShare share)`

### `TextFormatter` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `String Format(String text, Settings settings)`
- `String FormatWithUnescape(String text)`
- `String FormatXml(String xml, Settings settings)`
- `String FormatJson(String json, Settings settings)`

### `TimeHelper` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `String TimeSpanToString(TimeSpan value)`

### `TypeDescriptorContext` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Свойства:** InstanceContext: TypeDescriptorContext, Container: IContainer, Instance: Object, PropertyDescriptor: PropertyDescriptor
**Методы:**
- `Object GetService(Type serviceType)`
- `Void OnComponentChanged()`
- `Boolean OnComponentChanging()`

### `TypeReflectionHelper` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `Func`2 BuildGetter(String propertyName)`
- `Action`2 BuildSetter(String propertyName)`

### `TypesHelper` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `Boolean AreTypesCompatible(Object source, Type destinationType) (+1)`
- `Boolean AreReferenceTypesCompatible(Type sourceType, Type destinationType)`
- `IEnumerable`1 GetCompatibleTypes(IEnumerable`1 enumerable, Type targetType)`
- `Boolean ContainsCompatibleType(IEnumerable`1 enumerable, Type targetType)`
- `IEnumerable`1 GetImplementedTypes(Type type)`
- `Boolean CanConvert(Type sourceType, Type destinationType)`
- `Object Convert(Object value, Type type, Boolean throwOnError) (+1)`
- `Boolean TryConvert(Object value, Type type, Object& result)`
- `Object GetDefaultValue(Type type)`
- `Object GetDOCsDefaultValue(Type type)`
- `Object GetVariableDefaultValue(Type type)`
- `Boolean IsDefaultValue(Object value)`
- `Boolean IsDefaultTValue(T value)`
- `Boolean IsNullableValueType(Type type)`
- `Boolean IsNonNullableValueType(Type type)`
- `String GetTypeName(Type type)`
- `Boolean IsEnumLanguageType(Type type)`
- `Object ChangeType(Object value, Type type)`
- `Boolean IsImplicitReferenceConversion(Type sourceType, Type destinationType)`
- `Boolean IsNullableType(Type type)`
- `Type GetLanguageType(String type)`

### `UiAssembliesCheckResult` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Свойства:** HasUiAssemblies: Nullable`1, AddInfo: String

### `ValidationResult` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Свойства:** HasError: Boolean, HasWarning: Boolean, HasInfo: Boolean, HasItems: Boolean, Messages: ReadOnlyCollection`1
**Методы:**
- `Void Add(String message, ValidationItemLevel level, Object[] objects) (+2)`
- `Void ThrowError()`
- `String GetErrorText()`

### `WindowsHelper` (Namespace: `TFlex.DOCs.Common.Helpers`)
**Методы:**
- `String GetSid()`
- `String GetIdentityName()`
- `Boolean IsUserAdministrator()`
- `Boolean IsComputerNameLocal(String computerName)`

### `AspChildControlTypeAttribute` (Namespace: `TFlex.DOCs.Common.Helpers.Annotations`)
**Свойства:** TagName: String, ControlType: Type

### `AspMvcActionAttribute` (Namespace: `TFlex.DOCs.Common.Helpers.Annotations`)
**Свойства:** AnonymousProperty: String

### `AspMvcAreaAttribute` (Namespace: `TFlex.DOCs.Common.Helpers.Annotations`)
**Свойства:** AnonymousProperty: String

### `AspMvcAreaMasterLocationFormatAttribute` (Namespace: `TFlex.DOCs.Common.Helpers.Annotations`)
**Свойства:** Format: String

### `AspMvcAreaPartialViewLocationFormatAttribute` (Namespace: `TFlex.DOCs.Common.Helpers.Annotations`)
**Свойства:** Format: String

### `AspMvcAreaViewLocationFormatAttribute` (Namespace: `TFlex.DOCs.Common.Helpers.Annotations`)
**Свойства:** Format: String

### `AspMvcControllerAttribute` (Namespace: `TFlex.DOCs.Common.Helpers.Annotations`)
**Свойства:** AnonymousProperty: String

### `AspMvcMasterLocationFormatAttribute` (Namespace: `TFlex.DOCs.Common.Helpers.Annotations`)
**Свойства:** Format: String

### `AspMvcPartialViewLocationFormatAttribute` (Namespace: `TFlex.DOCs.Common.Helpers.Annotations`)
**Свойства:** Format: String

### `AspMvcViewLocationFormatAttribute` (Namespace: `TFlex.DOCs.Common.Helpers.Annotations`)
**Свойства:** Format: String

### `AspRequiredAttributeAttribute` (Namespace: `TFlex.DOCs.Common.Helpers.Annotations`)
**Свойства:** Attribute: String

### `AspTypePropertyAttribute` (Namespace: `TFlex.DOCs.Common.Helpers.Annotations`)
**Свойства:** CreateConstructorReferences: Boolean

### `AssertionConditionAttribute` (Namespace: `TFlex.DOCs.Common.Helpers.Annotations`)
**Свойства:** ConditionType: AssertionConditionType

### `BaseTypeRequiredAttribute` (Namespace: `TFlex.DOCs.Common.Helpers.Annotations`)
**Свойства:** BaseType: Type

### `CollectionAccessAttribute` (Namespace: `TFlex.DOCs.Common.Helpers.Annotations`)
**Свойства:** CollectionAccessType: CollectionAccessType

### `ContractAnnotationAttribute` (Namespace: `TFlex.DOCs.Common.Helpers.Annotations`)
**Свойства:** Contract: String, ForceFullStates: Boolean

### `HtmlAttributeValueAttribute` (Namespace: `TFlex.DOCs.Common.Helpers.Annotations`)
**Свойства:** Name: String

### `HtmlElementAttributesAttribute` (Namespace: `TFlex.DOCs.Common.Helpers.Annotations`)
**Свойства:** Name: String

### `LocalizationRequiredAttribute` (Namespace: `TFlex.DOCs.Common.Helpers.Annotations`)
**Свойства:** Required: Boolean

### `MacroAttribute` (Namespace: `TFlex.DOCs.Common.Helpers.Annotations`)
**Свойства:** Expression: String, Editable: Int32, Target: String

### `MeansImplicitUseAttribute` (Namespace: `TFlex.DOCs.Common.Helpers.Annotations`)
**Свойства:** UseKindFlags: ImplicitUseKindFlags, TargetFlags: ImplicitUseTargetFlags

### `MustUseReturnValueAttribute` (Namespace: `TFlex.DOCs.Common.Helpers.Annotations`)
**Свойства:** Justification: String

### `NotifyPropertyChangedInvocatorAttribute` (Namespace: `TFlex.DOCs.Common.Helpers.Annotations`)
**Свойства:** ParameterName: String

### `PathReferenceAttribute` (Namespace: `TFlex.DOCs.Common.Helpers.Annotations`)
**Свойства:** BasePath: String

### `RazorDirectiveAttribute` (Namespace: `TFlex.DOCs.Common.Helpers.Annotations`)
**Свойства:** Directive: String

### `RazorImportNamespaceAttribute` (Namespace: `TFlex.DOCs.Common.Helpers.Annotations`)
**Свойства:** Name: String

### `RazorInjectionAttribute` (Namespace: `TFlex.DOCs.Common.Helpers.Annotations`)
**Свойства:** Type: String, FieldName: String

### `RazorPageBaseTypeAttribute` (Namespace: `TFlex.DOCs.Common.Helpers.Annotations`)
**Свойства:** BaseType: String, PageName: String

### `StringFormatMethodAttribute` (Namespace: `TFlex.DOCs.Common.Helpers.Annotations`)
**Свойства:** FormatParameterName: String

### `UsedImplicitlyAttribute` (Namespace: `TFlex.DOCs.Common.Helpers.Annotations`)
**Свойства:** UseKindFlags: ImplicitUseKindFlags, TargetFlags: ImplicitUseTargetFlags

### `ValueProviderAttribute` (Namespace: `TFlex.DOCs.Common.Helpers.Annotations`)
**Свойства:** Name: String

### `ConfigurationManagerHelper` (Namespace: `TFlex.DOCs.Common.Helpers.Configuration`)
**Свойства:** AppConfig: Configuration
**Методы:**
- `String GetStringSetting(String settingName)`
- `String[] GetStringArraySetting(String settingName, Char separator)`
- `Boolean GetBoolSetting(String settingName, Boolean defaultValue)`
- `Int32 GetInt32Setting(String settingName, Int32 defaultValue)`
- `Double GetDoubleSetting(String settingName, Double defaultValue)`
- `TEnum GetEnumSetting(String settingName, TEnum defaultValue)`
- `TEnum[] GetEnumArraySetting(String settingName, Nullable`1 defaultItem)`

### `LinuxCommandAction` (Namespace: `TFlex.DOCs.Common.Helpers.Linux`)
**Методы:**
- `String Execute(String arguments, String command)`

### `LinuxDomainHelper` (Namespace: `TFlex.DOCs.Common.Helpers.Linux`)
**Методы:**
- `String GetUserName(String uniqueKey)`

### `LinuxService` (Namespace: `TFlex.DOCs.Common.Helpers.Linux`)
**Свойства:** IsRunning: Boolean, Name: String
**Методы:**
- `String GetImagePath()`
- `Boolean IsCurrentVersion()`
- `Void Start(Int32 timeOut)`
- `Void Stop(Int32 timeOut)`

### `LinuxServiceController` (Namespace: `TFlex.DOCs.Common.Helpers.Linux`)
**Свойства:** ServiceName: String, ServiceStatus: LinuxServiceStatus, UnitName: String
**Методы:**
- `Void Start()`
- `Void Stop()`
- `LinuxServiceController[] GetServices()`

### `LinuxServicesHelper` (Namespace: `TFlex.DOCs.Common.Helpers.Linux`)
**Методы:**
- `Boolean IsServiceExists(String serviceName)`
- `ServiceDispatcher GetService(String serviceName, Action`1 exceptionLogger)`

### `ActionThrottler` (Namespace: `TFlex.DOCs.Common.Helpers.MultiThreading`)
**Методы:**
- `Void Run()`

### `Pool`1` (Namespace: `TFlex.DOCs.Common.Helpers.MultiThreading`)
**Методы:**
- `T Get()`
- `Void Return(T item)`

### `SingleForManyRequestTaskRunner` (Namespace: `TFlex.DOCs.Common.Helpers.MultiThreading`)
**Методы:**
- `Task Run()`

### `SingleForManyRequestTaskRunnerEx`1` (Namespace: `TFlex.DOCs.Common.Helpers.MultiThreading`)
**Методы:**
- `Task Run(T arg)`

### `TaskChain` (Namespace: `TFlex.DOCs.Common.Helpers.MultiThreading`)
**Свойства:** ChainEnd: Task
**Методы:**
- `Task Add(Func`1 asyncAction, TaskScheduler scheduler)`

### `ActionExecutorExtensions` (Namespace: `TFlex.DOCs.Common.Helpers.MultiThreading.Executors`)
**Методы:**
- `Void RunSync(IActionExecutor executor, Action`1 action, TArg arg) (+1)`
- `Void Add(IActionExecutor executor, Action`1 action, TArg arg)`
- `Task Run(IActionExecutor executor, Action action) (+1)` [has Async]

### `DedicatedThreadExecutor` (Namespace: `TFlex.DOCs.Common.Helpers.MultiThreading.Executors`)
**Свойства:** Name: String
**Методы:**
- `Void Add(Action action)`

### `ExceptionHandledFifoActionExecutor` (Namespace: `TFlex.DOCs.Common.Helpers.MultiThreading.Executors`)
**Свойства:** Name: String
**Методы:**
- `Void Add(Action action)`

### `FifoActionExecutor` (Namespace: `TFlex.DOCs.Common.Helpers.MultiThreading.Executors`)
**Свойства:** Name: String
**Методы:**
- `Void Add(Action action)`

### `FifoActionExecutorSynchronizationContext` (Namespace: `TFlex.DOCs.Common.Helpers.MultiThreading.Executors`)
**Методы:**
- `Void Post(SendOrPostCallback d, Object state)`
- `Void Send(SendOrPostCallback d, Object state)`
- `SynchronizationContext CreateCopy()`

### `IActionExecutor` (Namespace: `TFlex.DOCs.Common.Helpers.MultiThreading.Executors`)
**Методы:**
- `Void Add(Action action)`

### `IFifoActionExecutor` (Namespace: `TFlex.DOCs.Common.Helpers.MultiThreading.Executors`)
**Свойства:** Name: String

### `ISingleThreadExecutor` (Namespace: `TFlex.DOCs.Common.Helpers.MultiThreading.Executors`)
**Методы:**
- `Void Add(Func`1 task)`

### `MaxConcurrencyExecutor` (Namespace: `TFlex.DOCs.Common.Helpers.MultiThreading.Executors`)
**Свойства:** Name: String
**Методы:**
- `Void Add(Func`1 action)`

### `MaxConcurrencySynchronizationContext` (Namespace: `TFlex.DOCs.Common.Helpers.MultiThreading.Executors`)
**Методы:**
- `Void Post(SendOrPostCallback d, Object state)`
- `Void Send(SendOrPostCallback d, Object state)`

### `SingleThreadExecutor` (Namespace: `TFlex.DOCs.Common.Helpers.MultiThreading.Executors`)
**Методы:**
- `Void Add(Func`1 task)`

### `SynchronizationContextRegion` (Namespace: `TFlex.DOCs.Common.Helpers.MultiThreading.Executors`)
**Методы:**
- `IDisposable Enter(SynchronizationContext newContext)`

### `ThreadPoolExecutor` (Namespace: `TFlex.DOCs.Common.Helpers.MultiThreading.Executors`)
**Методы:**
- `Void Add(Action action)`

### `FifoItemsProcessor`1` (Namespace: `TFlex.DOCs.Common.Helpers.MultiThreading.Processors`)
**Методы:**
- `Void Add(TItem item)`
- `Void AddRange(TItem[] items)`
- `Void DropQueue()`

### `IFifoItemsProcessor`1` (Namespace: `TFlex.DOCs.Common.Helpers.MultiThreading.Processors`)
**Методы:**
- `Void Add(TItem item)`

### `ProducerConsumerQueue`1` (Namespace: `TFlex.DOCs.Common.Helpers.MultiThreading.Processors`)
**Свойства:** Current: TItem, Count: Int32
**Методы:**
- `Void Add(TItem item)`
- `Void Clear()`
- `IEnumerator`1 GetEnumerator()`

### `LinuxNetworkConnectionsHelper` (Namespace: `TFlex.DOCs.Common.Helpers.Network.Linux`)
**Методы:**
- `IReadOnlyCollection`1 GetTcpUdpPorts()`

### `LinuxPort` (Namespace: `TFlex.DOCs.Common.Helpers.Network.Linux`)
**Свойства:** Port: Int32, State: MibTcpState

### `HierarсhyUnitFormatter` (Namespace: `TFlex.DOCs.Common.Helpers.TextFormatters`)
**Методы:**
- `String Format(Decimal number, Int32 maxFractionalSize, String[] unitStrings, Decimal[] upperLimitMultipliers)`

### `SizeFormatter` (Namespace: `TFlex.DOCs.Common.Helpers.TextFormatters`)
**Методы:**
- `String FormatFileSize(Int64 size)`

### `TimeFormatter` (Namespace: `TFlex.DOCs.Common.Helpers.TextFormatters`)
**Методы:**
- `String FormatTime(TimeSpan time)`

### `CharDataMatrix` (Namespace: `TFlex.DOCs.Common.Helpers.TextTables`)
**Свойства:** Item: Char
**Методы:**
- `Void SetValue(Int32 row, Int32 column, String value)`
- `Void WriteString(Action`1 write)`
- `String GetString()`

### `DataTable` (Namespace: `TFlex.DOCs.Common.Helpers.TextTables`)
**Методы:**
- `String[,] ToStringMatrix()`

### `GraphToTextConversionHelper` (Namespace: `TFlex.DOCs.Common.Helpers.TextTables`)
**Методы:**
- `String GraphToText(Func`2 edgeGetter, Func`3 edgeParametersGetter, Func`2 nodeNameGetter, Func`2 keyGetter, TNode[] startNodes) (+1)`

### `StringDataMatrix` (Namespace: `TFlex.DOCs.Common.Helpers.TextTables`)
**Свойства:** WriteByOneSymbol: Boolean, WriteByOneLine: Boolean, Item: String
**Методы:**
- `String[,] GetMatrix()`
- `String GetString(Int32 additionalSpace)`

### `TreeToTextConversionHelper`1` (Namespace: `TFlex.DOCs.Common.Helpers.TextTables`)
**Методы:**
- `String TreeToText(TNode rootNode, Func`2 nextLevelGetter, Func`2 nameGetter, Boolean withMinimalLeftOffset, Boolean useArrow) (+3)`

### `EnumerableExtensions` (Namespace: `TFlex.DOCs.Common.Helpers.TextTables.Extensions`)
**Методы:**
- `DataTable ToDataTable(IEnumerable`1 rowsEnum)`

### `StringMatrixFormatExtensions` (Namespace: `TFlex.DOCs.Common.Helpers.TextTables.Extensions`)
**Методы:**
- `String FormatAsPaddedTable(String[,] matrix, Int32 additionalSpace)`
- `String FormatAsCsv(String[,] matrix)`

### `DelegateTimerAction` (Namespace: `TFlex.DOCs.Common.Helpers.Timers`)
**Методы:**
- `Void Run()`

### `ITimerAction` (Namespace: `TFlex.DOCs.Common.Helpers.Timers`)
**Методы:**
- `Void Run()`

### `WeakMultiTimer` (Namespace: `TFlex.DOCs.Common.Helpers.Timers`)
**Методы:**
- `IDisposable Start(ITimerAction action, TimeSpan delay)`
- `IDisposable Repeat(ITimerAction action, TimeSpan delay)`

### `MailFolders` (Namespace: `TFlex.DOCs.Common.Mail`)
**Методы:**
- `Boolean IsSystem(Guid folderGuid)`
- `Guid[] GetFolders()`

### `SmtpServerSettings` (Namespace: `TFlex.DOCs.Common.Mail`)
**Свойства:** ServerName: String, Login: String, Password: String, Name: String, EMail: String, UseDOCsName: Boolean, UseDOCsEMail: Boolean, UseSSL: Boolean, Port: Int32, IsEmpty: Boolean
**Методы:**
- `RC2 GetRC2()`
- `SmtpClient CreateClient()`
- `String Serialize()`
- `SmtpServerSettings Deserialize(String xml)`
- `XmlSchema GetSchema()`
- `Void ReadXml(XmlReader reader)`
- `Void WriteXml(XmlWriter writer)`
- `SmtpServerSettings LoadFromRegistry(String registryKey)`

### `AssemblyCache` (Namespace: `TFlex.DOCs.Common.Plugins`)
**Методы:**
- `TExtension RegisterExtension(Func`2 createLoader)`

### `AssemblyCacheExtension` (Namespace: `TFlex.DOCs.Common.Plugins`)
**Методы:**
- `Void LoadAssembly(AssemblyLoader loader, AssemblyInfo assemblyInfo)`
- `Void Remove(Assembly assembly)`
- `Void Clear()`

### `AssemblyInfo` (Namespace: `TFlex.DOCs.Common.Plugins`)
**Свойства:** Assembly: Assembly, Types: Type[], TypesByName: SortedList`2

### `AssemblyLoader` (Namespace: `TFlex.DOCs.Common.Plugins`)
**Свойства:** CurrentAssemblyDirectory: String, ClassName: String, AssemblyFileName: String, RunningException: String, LoadedAssemblyes: ReadOnlyCollection`1
**Методы:**
- `Object GetClassObject() (+1)`
- `Assembly LoadAssembly(String fileName) (+1)`
- `Boolean IsAssemblyLoaded(String assemblyFileName) (+1)`
- `Type GetInterfaceInAssembly(Type interfaceType, Boolean throwOnNotFound) (+1)`

### `ItemTypeExtensions` (Namespace: `TFlex.DOCs.Common.PreprocessorArrays`)
**Методы:**
- `Object GetArrayTypeValue(String variable, Object value)`
- `Boolean IsArrayTypeVariable(String variableName)`
- `String ExtractVariableTypeName(String variableName)`
- `ItemType ExtractVariableType(String variable)`
- `String ExtractVariableName(String variableName)`
- `ItemType ToVariableType(String variableTypeName)`
- `String ToItemTypeName(ItemType itemType)`
- `Type ToItemType(ItemType itemType)`
- `Object ReadItemValue(ItemType itemType, Object value)`
- `Object[] ExtractArrayItems(Object arrayItems, ItemType valueItemType)`

### `BaseDirectiveParser` (Namespace: `TFlex.DOCs.Common.PreprocessorDirectives`)
**Свойства:** Directive: String
**Методы:**
- `Void Parse(Preprocessor preprocessor, String directive, String rest, String[] args, Int32 lineNumber)`
- `String GetLineNumber(Int32 lineNumber)`
- `Boolean DirectiveEnabled(Preprocessor preprocessor)`

### `BasePreprocessorFunction` (Namespace: `TFlex.DOCs.Common.PreprocessorFunctions`)
**Свойства:** Name: String
**Методы:**
- `PreprocessorFuncResult Execute(Preprocessor preprocessor, Int32 lineNumber, String[] arguments)`

### `PreprocessorFuncResult` (Namespace: `TFlex.DOCs.Common.PreprocessorFunctions`)
**Свойства:** Result: Object, ParsedArgs: Int32, IsEmpty: Boolean
**Методы:**
- `PreprocessorFuncResult Build(Object result, Int32 parsedArgs)`

### `IQueryInfo` (Namespace: `TFlex.DOCs.Common.Profiler`)
**Свойства:** Text: String, StartTime: DateTime, EndTime: DateTime, CommandTimeout: Int32, IsEmpty: Boolean, IsSecret: Boolean

### `TimeWatcher` (Namespace: `TFlex.DOCs.Common.Profiler`)
**Методы:**
- `Void AppendCommonLogger(ICommonLogger appendedCoommonLogger)`

### `Resources` (Namespace: `TFlex.DOCs.Common.Resources`)
**Свойства:** ResourceManager: ResourceManager, Culture: CultureInfo, AccessCommandTypeAll: String, AccessCommandTypeLink: String, AccessCommandTypeObject: String, AccessCommandTypeReference: String, AccessCommandTypeSystem: String, AccessDirectionChildren: String, AccessDirectionDefault: String, AccessDirectionEntity: String, AccessInheritedFromCredentials: String, AccessInheritedFromLink: String, AccessInheritedFromLinkedObject: String, AccessInheritedFromLinkedObjectReference: String, AccessInheritedFromLinkedObjectStage: String, AccessInheritedFromLinkedOwners: String, AccessInheritedFromObject: String, AccessInheritedFromOwn: String, AccessInheritedFromOwner: String, AccessInheritedFromParentObjects: String, AccessInheritedFromReference: String, AccessInheritedFromStage: String, AccessLevelMinimal: String, AccessLevelMinimalAlias: String, AccessRightsModeAccessEnabled: String, AccessRightsModeBiDirectional: String, AccessRightsModeDisabled: String, AccessRightsModeFromAtoB: String, AccessRightsModeFromBtoA: String, AccessRightsModeFull: String, AccessRightsModeWithParents: String, AccessTypeIDAll: String, AccessTypeIDLink: String, AccessTypeIDLinkedObjects: String, AccessTypeIDLinkedStage: String, AccessTypeIDObject: String, AccessTypeIDOwner: String, AccessTypeIDReference: String, AccessTypeIDStage: String, AccessTypeIDSystem: String, AesPassManagerCreateWritableConfigurationExceptionMessage: String, ByteArrayEncryptionError: String, Bytes: String, Bytes_sec: String, CiphertextTooShort: String, CommunicationErrorExceptionMessage: String, ConvertValueError: String, Days: String, ErrorStartingService: String, ErrorStoppingService: String, ExpressionCommonExceptionMessage: String, ExpressionEvaluationMessage: String, ExpressionFunctionNotFoundMessage: String, ExpressionInvalidResultMessage: String, ExpressionIsEmptyMessage: String, ExpressionIsInfinityResultMessage: String, ExpressionNameFormat: String, ExpressionParameterNotDefinedMessage: String, ExpressionParametersFormat: String, ExpressionParserMessage: String, ExpressionParserPositionMessage: String, FileOpenException: String, Gigabytes: String, Gigabytes_sec: String, GroupTable: String, Hours: String, InvalidDatabaseVersion: String, InvalidHmacError: String, InvalidNewCiphertextFormat: String, InvalidOldCiphertextFormat: String, Kilobytes: String, Kilobytes_sec: String, logChecked: String, logNo: String, logUnchecked: String, logYes: String, Megabytes: String, Megabytes_sec: String, Milliseconds: String, Minutes: String, NoneAlgorithm: String, Parameter: String, ParameterField: String, PasswordDecryptionError: String, PasswordEncryptionError: String, Preprocessor_DuplicateElse: String, Preprocessor_EmptyAPPENDSection: String, Preprocessor_EmptyDEFINESection: String, Preprocessor_EmptyIFSection: String, Preprocessor_EmptyUNDEFINESection: String, Preprocessor_IFSectionNotOpened: String, Preprocessor_UnknownDirective: String, Preprocessor_UnknownOperator: String, RestoreObjects: String, Seconds: String, SizeInBytes: String, SizeInGygabytes: String, SizeInKilobytes: String, SizeInMegabytes: String, StandardAlgorithm: String, Terabytes: String, Terabytes_sec: String, TextDecryptionError: String, TextEncryptionError: String, TimeInDays: String, TimeInHours: String, TimeInMilliseconds: String, TimeInMinutes: String, TimeInSeconds: String, TimeLeft: String

### `JsonDataReader` (Namespace: `TFlex.DOCs.Common.TextSerializers`)
**Свойства:** IsXml: Boolean, IsJson: Boolean, IsElement: Boolean
**Методы:**
- `String GetName()`
- `String GetNextName()`
- `Void Read()`
- `Void ReadStartDocument()`
- `Boolean ReadStartArray(String name)`
- `Void ReadEndArray()`
- `Boolean ReadStartObject(String name, Action readAttrubutes) (+1)`
- `Boolean ReadForcedStartObject(String name, Action readAttrubutes) (+1)`
- `String ReadForcedStartObjectName()`
- `Void ReadEndObject()`
- `Boolean IsObject(String name)`
- `Boolean ReadStartElement(String name)`
- `Boolean IsEndElement(String name)`
- `Void ReadEndElement()`
- `String ReadNextAttributeName()`
- `String ReadStringAttribute(String name)`
- `T Deserialize()`
- `Void Skip()`
- `Void Close()`
- `Boolean ReadBoolValue()`
- `Char ReadCharValue()`
- `SByte ReadSByteValue()`
- `Byte ReadByteValue()`
- `Int16 ReadShortValue()`
- `UInt16 ReadUShortValue()`
- `Int32 ReadIntValue()`
- `UInt32 ReadUIntValue()`
- `Int64 ReadLongValue()`
- `UInt64 ReadULongValue()`
- `Single ReadFloatValue()`
- `Double ReadDoubleValue()`
- `Decimal ReadDecimalValue()`
- `String ReadStringValue()`
- `Guid ReadGuidValue()`
- `DateTime ReadDateTimeValue()`
- `TimeSpan ReadTimeSpanValue()`
- `Boolean ReadBoolOuterValue()`
- `Char ReadCharOuterValue()`
- `SByte ReadSByteOuterValue()`
- `Byte ReadByteOuterValue()`
- `Int16 ReadShortOuterValue()`
- `UInt16 ReadUShortOuterValue()`
- `Int32 ReadIntOuterValue()`
- `UInt32 ReadUIntOuterValue()`
- `Int64 ReadLongOuterValue()`
- `UInt64 ReadULongOuterValue()`
- `Single ReadFloatOuterValue()`
- `Double ReadDoubleOuterValue()`
- `Decimal ReadDecimalOuterValue()`
- `String ReadStringOuterValue()`
- `Guid ReadGuidOuterValue()`
- `DateTime ReadDateTimeOuterValue()`
- `TimeSpan ReadTimeSpanOuterValue()`
- `Void ReadArray(String name, ICollection`1 values) (+16)`
- `Boolean ReadBoolElement(String name)`
- `Nullable`1 ReadNullableBoolElement(String name)`
- `Char ReadCharElement(String name)`
- `Nullable`1 ReadNullableCharElement(String name)`
- `SByte ReadSByteElement(String name)`
- `Nullable`1 ReadNullableSByteElement(String name)`
- `Byte ReadByteElement(String name)`
- `Nullable`1 ReadNullableByteElement(String name)`
- `Int16 ReadShortElement(String name)`
- `Nullable`1 ReadNullableShortElement(String name)`
- `UInt16 ReadUShortElement(String name)`
- `Nullable`1 ReadNullableUShortElement(String name)`
- `Int32 ReadIntElement(String name)`
- `Nullable`1 ReadNullableIntElement(String name)`
- `UInt32 ReadUIntElement(String name)`
- `Nullable`1 ReadNullableUIntElement(String name)`
- `Int64 ReadLongElement(String name)`
- `Nullable`1 ReadNullableLongElement(String name)`
- `UInt64 ReadULongElement(String name)`
- `Nullable`1 ReadNullableULongElement(String name)`
- `Single ReadFloatElement(String name)`
- `Nullable`1 ReadNullableFloatElement(String name)`
- `Double ReadDoubleElement(String name)`
- `Nullable`1 ReadNullableDoubleElement(String name)`
- `Decimal ReadDecimalElement(String name)`
- `Nullable`1 ReadNullableDecimalElement(String name)`
- `String ReadStringElement(String name)`
- `Guid ReadGuidElement(String name)`
- `Nullable`1 ReadNullableGuidElement(String name)`
- `DateTime ReadDateTimeElement(String name)`
- `Nullable`1 ReadNullableDateTimeElement(String name)`
- `TimeSpan ReadTimeSpanElement(String name)`
- `Nullable`1 ReadNullableTimeSpanElement(String name)`
- `T ReadElement(String name, T defaultValue)`

### `JsonDataWriter` (Namespace: `TFlex.DOCs.Common.TextSerializers`)
**Свойства:** IsXml: Boolean, IsJson: Boolean
**Методы:**
- `Void WriteStartDocument()`
- `Void WriteEndDocument()`
- `Void WriteStartArray(String name)`
- `Void WriteEndArray()`
- `Void WriteForcedStartObject(String name)`
- `Void WriteStartObject(String name)`
- `Void WriteEndObject()`
- `Void WriteStartElement(String name)`
- `Void WriteEndElement()`
- `Void WriteStartAttribute(String name, String prefix, String ns) (+1)`
- `Void WriteEndAttribute()`
- `Void WriteComment(String comment)`
- `Void Serialize(T obj)`
- `Void WriteInnerElement(String name, String value, Boolean writeDefault, Boolean write) (+2)`
- `Void Flush()`
- `Void Close()`
- `Void WriteValue(Boolean value, Boolean write) (+17)`
- `Void WriteOuterValue(Boolean value) (+16)`
- `Void WriteForcedArray(String name, IReadOnlyCollection`1 values) (+16)`
- `Void WriteForcedElement(String name, Guid value, Boolean writeDefault, Boolean write) (+18)`
- `Void WriteElement(String name, Boolean value, Boolean writeDefault, Boolean write) (+49)`
- `Void WriteReaderLevelData(JsonDataReader reader)`

### `TextDataManager` (Namespace: `TFlex.DOCs.Common.TextSerializers`)
**Методы:**
- `TextDataWriter CreateWriter(Stream output, TextFormats format, Boolean indent)`
- `TextDataReader CreateReader(Stream input, Boolean supportCustomFormats) (+1)`
- `ValueTuple`2 ParseFormat(Stream input, Boolean supportCustomFormats, Boolean readInnerFormat) (+1)`
- `Byte[] ToBson(Object source)`
- `T FromBson(Byte[] bytes)`

### `TextDataReader` (Namespace: `TFlex.DOCs.Common.TextSerializers`)
**Свойства:** IsXml: Boolean, IsJson: Boolean, Input: Stream, IsElement: Boolean
**Методы:**
- `String GetName()`
- `String GetNextName()`
- `Void Read()`
- `Void ReadStartDocument()`
- `Boolean ReadStartArray(String name)`
- `Void ReadEndArray()`
- `Boolean ReadStartObject(String name, Action readAttrubutes) (+1)`
- `Boolean ReadForcedStartObject(String name, Action readAttrubutes) (+1)`
- `String ReadForcedStartObjectName()`
- `Void ReadEndObject()`
- `Boolean IsObject(String name)`
- `Boolean ReadStartElement(String name)`
- `Boolean IsEndElement(String name)`
- `Void ReadEndElement()`
- `T Deserialize()`
- `Void Skip()`
- `Void Close()`
- `Boolean ReadBoolValue()`
- `Char ReadCharValue()`
- `SByte ReadSByteValue()`
- `Byte ReadByteValue()`
- `Int16 ReadShortValue()`
- `UInt16 ReadUShortValue()`
- `Int32 ReadIntValue()`
- `UInt32 ReadUIntValue()`
- `Int64 ReadLongValue()`
- `UInt64 ReadULongValue()`
- `Single ReadFloatValue()`
- `Double ReadDoubleValue()`
- `Decimal ReadDecimalValue()`
- `String ReadStringValue()`
- `Guid ReadGuidValue()`
- `DateTime ReadDateTimeValue()`
- `TimeSpan ReadTimeSpanValue()`
- `Boolean ReadBoolOuterValue()`
- `Char ReadCharOuterValue()`
- `SByte ReadSByteOuterValue()`
- `Byte ReadByteOuterValue()`
- `Int16 ReadShortOuterValue()`
- `UInt16 ReadUShortOuterValue()`
- `Int32 ReadIntOuterValue()`
- `UInt32 ReadUIntOuterValue()`
- `Int64 ReadLongOuterValue()`
- `UInt64 ReadULongOuterValue()`
- `Single ReadFloatOuterValue()`
- `Double ReadDoubleOuterValue()`
- `Decimal ReadDecimalOuterValue()`
- `String ReadStringOuterValue()`
- `Guid ReadGuidOuterValue()`
- `DateTime ReadDateTimeOuterValue()`
- `TimeSpan ReadTimeSpanOuterValue()`
- `Void ReadArray(String name, ICollection`1 values) (+16)`
- `String ReadNextAttributeName()`
- `Boolean ReadBoolAttribute(String name)`
- `Nullable`1 ReadNullableBoolAttribute(String name)`
- `Char ReadCharAttribute(String name)`
- `Nullable`1 ReadNullableCharAttribute(String name)`
- `SByte ReadSByteAttribute(String name)`
- `Nullable`1 ReadNullableSByteAttribute(String name)`
- `Byte ReadByteAttribute(String name)`
- `Nullable`1 ReadNullableByteAttribute(String name)`
- `Int16 ReadShortAttribute(String name)`
- `Nullable`1 ReadNullableShortAttribute(String name)`
- `UInt16 ReadUShortAttribute(String name)`
- `Nullable`1 ReadNullableUShortAttribute(String name)`
- `Int32 ReadIntAttribute(String name)`
- `Nullable`1 ReadNullableIntAttribute(String name)`
- `UInt32 ReadUIntAttribute(String name)`
- `Nullable`1 ReadNullableUIntAttribute(String name)`
- `Int64 ReadLongAttribute(String name)`
- `Nullable`1 ReadNullableLongAttribute(String name)`
- `UInt64 ReadULongAttribute(String name)`
- `Nullable`1 ReadNullableULongAttribute(String name)`
- `Single ReadFloatAttribute(String name)`
- `Nullable`1 ReadNullableFloatAttribute(String name)`
- `Double ReadDoubleAttribute(String name)`
- `Nullable`1 ReadNullableDoubleAttribute(String name)`
- `Decimal ReadDecimalAttribute(String name)`
- `Nullable`1 ReadNullableDecimalAttribute(String name)`
- `String ReadStringAttribute(String name)`
- `Guid ReadGuidAttribute(String name)`
- `Nullable`1 ReadNullableGuidAttribute(String name)`
- `DateTime ReadDateTimeAttribute(String name)`
- `Nullable`1 ReadNullableDateTimeAttribute(String name)`
- `TimeSpan ReadTimeSpanAttribute(String name)`
- `Nullable`1 ReadNullableTimeSpanAttribute(String name)`
- `Byte[] ReadBytesAttribute(String name)`
- `T ReadAttribute(String name, T defaultValue) (+1)`
- `Boolean ReadBoolElement(String name)`
- `Nullable`1 ReadNullableBoolElement(String name)`
- `Char ReadCharElement(String name)`
- `Nullable`1 ReadNullableCharElement(String name)`
- `SByte ReadSByteElement(String name)`
- `Nullable`1 ReadNullableSByteElement(String name)`
- `Byte ReadByteElement(String name)`
- `Nullable`1 ReadNullableByteElement(String name)`
- `Int16 ReadShortElement(String name)`
- `Nullable`1 ReadNullableShortElement(String name)`
- `UInt16 ReadUShortElement(String name)`
- `Nullable`1 ReadNullableUShortElement(String name)`
- `Int32 ReadIntElement(String name)`
- `Nullable`1 ReadNullableIntElement(String name)`
- `UInt32 ReadUIntElement(String name)`
- `Nullable`1 ReadNullableUIntElement(String name)`
- `Int64 ReadLongElement(String name)`
- `Nullable`1 ReadNullableLongElement(String name)`
- `UInt64 ReadULongElement(String name)`
- `Nullable`1 ReadNullableULongElement(String name)`
- `Single ReadFloatElement(String name)`
- `Nullable`1 ReadNullableFloatElement(String name)`
- `Double ReadDoubleElement(String name)`
- `Nullable`1 ReadNullableDoubleElement(String name)`
- `Decimal ReadDecimalElement(String name)`
- `Nullable`1 ReadNullableDecimalElement(String name)`
- `String ReadStringElement(String name)`
- `Guid ReadGuidElement(String name)`
- `Nullable`1 ReadNullableGuidElement(String name)`
- `DateTime ReadDateTimeElement(String name)`
- `Nullable`1 ReadNullableDateTimeElement(String name)`
- `TimeSpan ReadTimeSpanElement(String name)`
- `Nullable`1 ReadNullableTimeSpanElement(String name)`
- `T ReadElement(String name, T defaultValue)`
- `Byte[] ReadBytesElement(String name)`

### `TextDataWriter` (Namespace: `TFlex.DOCs.Common.TextSerializers`)
**Свойства:** IsXml: Boolean, IsJson: Boolean, Output: Stream
**Методы:**
- `Void WriteElement(String name, Char value, Boolean writeDefault, Boolean write) (+51)`
- `Void WriteForcedElement(String name, Guid value, Boolean writeDefault, Boolean write) (+19)`
- `Void WriteStartDocument()`
- `Void WriteEndDocument()`
- `Void WriteStartArray(String name)`
- `Void WriteEndArray()`
- `Void WriteForcedStartObject(String name)`
- `Void WriteStartObject(String name)`
- `Void WriteEndObject()`
- `Void WriteStartElement(String name)`
- `Void WriteEndElement()`
- `Void WriteStartAttribute(String name, String prefix, String ns) (+1)`
- `Void WriteEndAttribute()`
- `Void WriteComment(String comment)`
- `Void Serialize(T obj)`
- `Void WriteInnerElement(String name, String value, Boolean writeDefault, Boolean write) (+2)`
- `Void Flush()`
- `Void Close()`
- `Void WriteValue(Boolean value, Boolean write) (+17)`
- `Void WriteOuterValue(Boolean value) (+16)`
- `Void WriteForcedArray(String name, IReadOnlyCollection`1 values) (+16)`
- `Void WriteArray(String name, IReadOnlyCollection`1 values) (+16)`
- `Void WriteForcedAttribute(String name, String value, String prefix, String ns) (+19)`
- `Void WriteAttribute(String name, Boolean value, Boolean defaultValue, Boolean writeDefault, Boolean write) (+45)`

### `WeakAsyncEvent` (Namespace: `TFlex.DOCs.Common.WeakEvents`)
**Методы:**
- `IWeakAsyncEvent`1 Create()`

### `WeakDelegateList` (Namespace: `TFlex.DOCs.Common.WeakEvents`)
**Методы:**
- `IWeakDelegateList`2 Create()`

### `WeakEvent` (Namespace: `TFlex.DOCs.Common.WeakEvents`)
**Методы:**
- `IWeakEvent`1 Create()`

### `WeakEventManagerExBase`2` (Namespace: `TFlex.DOCs.Common.WeakEvents`)
**Методы:**
- `Void SubscribeEvent(TEventSource source, TEventHandler handler)`
- `Void UnSubscribeEvent(TEventSource source, TEventHandler handler)`

### `WeakKeyDictionary`2` (Namespace: `TFlex.DOCs.Common.WeakEvents`)
**Свойства:** Keys: ICollection`1, Values: ICollection`1, Count: Int32, IsReadOnly: Boolean, Item: TValue
**Методы:**
- `Boolean TryGetValue(TKey key, TValue& value)`
- `Void Add(TKey key, TValue value) (+1)`
- `TValue GetOrAdd(TKey key, Func`2 valueFactory)`
- `Boolean ContainsKey(TKey key)`
- `Boolean Remove(TKey key) (+1)`
- `Void Clear()`
- `Boolean Contains(KeyValuePair`2 item)`
- `Void CopyTo(KeyValuePair`2[] array, Int32 arrayIndex)`
- `IEnumerator`1 GetEnumerator()`

### `WeakReferenceExtensions` (Namespace: `TFlex.DOCs.Common.WeakEvents`)
**Методы:**
- `T GetTarget(WeakReference`1 reference)`

### `WeakStorage` (Namespace: `TFlex.DOCs.Common.WeakEvents`)
**Свойства:** IsAlive: Boolean, Value: Object

### `ArrayDynamicData`1` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** IsArray: Boolean, ItemsCount: Int32
**Методы:**
- `IEnumerable GetUniqueValues()`

### `BooleanArrayDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Boolean[]
**Методы:**
- `BooleanArrayDynamicData Create(Boolean[] value)`
- `Object GetWrappedValue()`
- `Boolean[] GetValue()`
- `Void SetValue(Boolean[] value)`
- `Boolean IsNull()`

### `BooleanDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Boolean
**Методы:**
- `BooleanDynamicData Create(Boolean value)`
- `Object GetWrappedValue()`
- `Boolean GetValue()`
- `Void SetValue(Boolean value)`
- `Boolean IsNull()`
- `String GetString()`

### `BooleanDynamicDataColumn` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType
**Методы:**
- `Boolean GetBooleanValue(Int32& row)`
- `String GetStringValue(Int32& row)`

### `BooleanListDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: List`1
**Методы:**
- `BooleanListDynamicData Create(List`1 value)`
- `Object GetWrappedValue()`
- `List`1 GetValue()`
- `Void SetValue(List`1 value)`
- `Boolean IsNull()`

### `ByteArrayDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Byte[]
**Методы:**
- `ByteArrayDynamicData Create(Byte[] value)`
- `Object GetWrappedValue()`
- `Byte[] GetValue()`
- `Void SetValue(Byte[] value)`
- `Boolean IsNull()`

### `ByteArrayDynamicDataColumn` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, IsArray: Boolean
**Методы:**
- `List`1 GetValues(Int32 offset, Int32 count) (+1)`
- `Void AppendValues(HashSet`1 collection, Int32 count)`
- `String GetStringValue(Int32& row)`
- `Byte[] GetByteArrayValue(Int32& row)`

### `ByteDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Byte
**Методы:**
- `ByteDynamicData Create(Byte value)`
- `Object GetWrappedValue()`
- `Byte GetValue()`
- `Void SetValue(Byte value)`
- `Boolean IsNull()`
- `String GetString()`

### `ByteDynamicDataColumn` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType
**Методы:**
- `Byte GetByteValue(Int32& row)`
- `String GetStringValue(Int32& row)`

### `ByteListDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: List`1
**Методы:**
- `ByteListDynamicData Create(List`1 value)`
- `Object GetWrappedValue()`
- `List`1 GetValue()`
- `Void SetValue(List`1 value)`
- `Boolean IsNull()`

### `CharArrayDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Char[]
**Методы:**
- `CharArrayDynamicData Create(Char[] value)`
- `Object GetWrappedValue()`
- `Char[] GetValue()`
- `Void SetValue(Char[] value)`
- `Boolean IsNull()`

### `CharDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Char
**Методы:**
- `CharDynamicData Create(Char value)`
- `Object GetWrappedValue()`
- `Char GetValue()`
- `Void SetValue(Char value)`
- `Boolean IsNull()`
- `String GetString()`

### `CharDynamicDataColumn` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType
**Методы:**
- `String GetStringValue(Int32& row)`

### `CharListDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: List`1
**Методы:**
- `CharListDynamicData Create(List`1 value)`
- `Object GetWrappedValue()`
- `List`1 GetValue()`
- `Void SetValue(List`1 value)`
- `Boolean IsNull()`

### `DateTimeArrayDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: DateTime[]
**Методы:**
- `DateTimeArrayDynamicData Create(DateTime[] value)`
- `Object GetWrappedValue()`
- `DateTime[] GetValue()`
- `Void SetValue(DateTime[] value)`
- `Boolean IsNull()`

### `DateTimeDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: DateTime
**Методы:**
- `DateTimeDynamicData Create(DateTime value)`
- `Object GetWrappedValue()`
- `DateTime GetValue()`
- `Void SetValue(DateTime value)`
- `Boolean IsNull()`
- `String GetString()`

### `DateTimeDynamicDataColumn` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType
**Методы:**
- `DateTime GetDateTimeValue(Int32& row)`
- `String GetStringValue(Int32& row)`

### `DateTimeListDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: List`1
**Методы:**
- `DateTimeListDynamicData Create(List`1 value)`
- `Object GetWrappedValue()`
- `List`1 GetValue()`
- `Void SetValue(List`1 value)`
- `Boolean IsNull()`

### `DecimalArrayDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Decimal[]
**Методы:**
- `DecimalArrayDynamicData Create(Decimal[] value)`
- `Object GetWrappedValue()`
- `Decimal[] GetValue()`
- `Void SetValue(Decimal[] value)`
- `Boolean IsNull()`

### `DecimalDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Decimal
**Методы:**
- `DecimalDynamicData Create(Decimal value)`
- `Object GetWrappedValue()`
- `Decimal GetValue()`
- `Void SetValue(Decimal value)`
- `Boolean IsNull()`
- `String GetString()`

### `DecimalDynamicDataColumn` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType
**Методы:**
- `String GetStringValue(Int32& row)`

### `DecimalListDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: List`1
**Методы:**
- `DecimalListDynamicData Create(List`1 value)`
- `Object GetWrappedValue()`
- `List`1 GetValue()`
- `Void SetValue(List`1 value)`
- `Boolean IsNull()`

### `DoubleArrayDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Double[]
**Методы:**
- `DoubleArrayDynamicData Create(Double[] value)`
- `Object GetWrappedValue()`
- `Double[] GetValue()`
- `Void SetValue(Double[] value)`
- `Boolean IsNull()`

### `DoubleDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Double
**Методы:**
- `DoubleDynamicData Create(Double value)`
- `Object GetWrappedValue()`
- `Double GetValue()`
- `Void SetValue(Double value)`
- `Boolean IsNull()`
- `String GetString()`

### `DoubleDynamicDataColumn` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType
**Методы:**
- `Double GetDoubleValue(Int32& row)`
- `String GetStringValue(Int32& row)`

### `DoubleListDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: List`1
**Методы:**
- `DoubleListDynamicData Create(List`1 value)`
- `Object GetWrappedValue()`
- `List`1 GetValue()`
- `Void SetValue(List`1 value)`
- `Boolean IsNull()`

### `DynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** DataType: Type, Type: DynamicDataType, StructureType: DynamicDataStructureType, IsArray: Boolean, IsList: Boolean, ItemsCount: Int32, IsNullable: Boolean
**Методы:**
- `DynamicData Create(Object value) (+1)`
- `Object GetWrappedValue()`
- `DynamicData CloneWrapped()`
- `DynamicData UnwrapNullable()`
- `Boolean IsNull()`
- `String GetString()`
- `T[] GetArray()`
- `List`1 GetList()`
- `IEnumerable GetUniqueValues()`

### `DynamicData`1` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** DataType: Type, IsNullable: Boolean
**Методы:**
- `T GetValue()`
- `Void SetValue(T value)`
- `DynamicData CloneWrapped()`

### `DynamicDataColumn` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Name: String, DataType: Type, Type: DynamicDataType, IsNullable: Boolean, IsArray: Boolean
**Методы:**
- `Object GetWrappedValue(Int32& row)`
- `Void SetWrappedValue(Int32& row, Object value)`
- `Void ClearValue(Int32& row)`
- `Void SetDataValue(Int32& row, DynamicData value)`
- `Boolean IsEmpty(Int32& row)`
- `Boolean IsNull(Int32& row)`
- `IEnumerable`1 GetWrappedValues(Int32 count)`
- `Void Compress()`
- `DynamicDataColumn Copy(Int32 rowCapacity, Boolean withRows)`
- `DynamicDataColumn CopyNonNullable(Int32 rowCapacity)`
- `Void CopyTo(DynamicDataColumn destination, Int32& row, Int32& destinationRow)`
- `Object GetWrappedFallbackValue()`
- `Void ClearRows(Int32& capacity)`
- `Void ClearRowsWithFill(Int32 count)`
- `Int32 GetRowCount()`
- `FrozenDynamicDataColumn ToWrappedFrozen()`
- `Boolean GetBooleanValue(Int32& row)`
- `Byte GetByteValue(Int32& row)`
- `Int32 GetInt32Value(Int32& row)`
- `Int64 GetInt64Value(Int32& row)`
- `Single GetSingleValue(Int32& row)`
- `Double GetDoubleValue(Int32& row)`
- `String GetStringValue(Int32& row)`
- `DateTime GetDateTimeValue(Int32& row)`
- `Guid GetGuidValue(Int32& row)`
- `Byte[] GetByteArrayValue(Int32& row)`
- `ValueTuple`2 GetNonRepeatingColumn(Int32 count, Boolean withEmptyValues)`
- `IEnumerable`1 FindDuplicateRows(Int32 rowCount, StringComparison comparison)`
- `IEnumerable`1 GetKnownTypes()`
- `DynamicDataColumn Create(String name, Type type, Int32 capacity, Boolean nullable)`
- `DynamicDataColumn CreateWithCastType(Int32 rowCount, Type newType, Nullable`1 newNullable)`
- `Void Optimize(Int32& tableRowCount)`
- `Void InitRows()`

### `DynamicDataColumn`1` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** DataType: Type, RowValues: List`1
**Методы:**
- `T GetValue(Int32& row)`
- `Void SetValue(Int32& row, T& value)`
- `Void SetDataValue(Int32& row, DynamicData value)`
- `Object GetWrappedValue(Int32& row)`
- `Void SetWrappedValue(Int32& row, Object value)`
- `Void ClearValue(Int32& row)`
- `Boolean IsEmpty(Int32& row)`
- `Boolean IsNull(Int32& row)`
- `List`1 GetValues(Int32 offset, Int32 count) (+1)`
- `IEnumerable`1 GetWrappedValues(Int32 count)`
- `Void Compress()`
- `Void AppendValues(HashSet`1 collection, Int32 count)`
- `T GetLastOrDefaultValue(Int32 count)`
- `DynamicDataColumn Copy(Int32 rowCapacity, Boolean withRows)`
- `DynamicDataColumn CopyNonNullable(Int32 rowCapacity)`
- `Void CopyTo(DynamicDataColumn destination, Int32& row, Int32& destinationRow)`
- `ICollection`1 GetNonRepeatingValues(Int32 count)`
- `ValueTuple`2 GetNonRepeatingColumn(Int32 count, Boolean withEmptyValues)`
- `IEnumerable`1 FindDuplicateRows(Int32 rowCount, StringComparison comparison)`
- `Void ClearRows(Int32& capacity)`
- `Void ClearRowsWithFill(Int32 count)`
- `Void InitRows()`
- `FrozenDynamicDataColumn ToWrappedFrozen()`
- `FrozenDynamicDataColumn`1 ToFrozen()`

### `DynamicDataColumnWithFallbackValue`1` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** FallbackValue: T
**Методы:**
- `List`1 GetValues(Int32 offset, Int32 count) (+1)`
- `Void AppendValues(HashSet`1 collection, Int32 count)`
- `T GetLastOrDefaultValue(Int32 count)`
- `T GetValue(Int32& row)`
- `Void SetValue(Int32& row, T& value)`
- `Object GetWrappedValue(Int32& row)`
- `Void SetWrappedValue(Int32& row, Object value)`
- `Object GetWrappedFallbackValue()`
- `Void ClearRows(Int32& capacity)`
- `Void ClearRowsWithFill(Int32 count)`
- `DynamicDataColumn Copy(Int32 rowCapacity, Boolean withRows)`
- `FrozenDynamicDataColumn`1 ToFrozen()`

### `DynamicDataSet` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Name: String, Tables: List`1, Capacity: Int32
**Методы:**
- `DynamicDataTable Find(String name)`
- `Boolean ContainsTable(String name)`
- `DynamicDataTable Pop(String name)`
- `DynamicDataTable AddTable(String name, Int32 columnsCapacity)`
- `Void Clear()`
- `Void ClearRows(Int32 capacity)`
- `Void ClearRowsWithFill(Int32 count)`
- `Void BeginChanges()`
- `Void EndChanges()`
- `DataSet ConvertToDataSet()`
- `DynamicDataSet ConvertFrom(DataSet dataSet)`
- `String GetDataVisualization()`
- `String GetMatrixVisualization()`
- `Void Optimize()`

### `DynamicDataTable` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Name: String, RowCount: Int32, Columns: List`1, Capacity: Int32, Changing: Boolean, Tags: Dictionary`2
**Методы:**
- `Boolean ContainsColumn(String name)`
- `Boolean ContainsColumnIgnoreCase(String name)`
- `DynamicDataColumn Find(String name) (+2)`
- `DynamicDataColumn FindIgnoreCase(String name) (+1)`
- `Int32 IndexOf(String name)`
- `Int32 IndexOfIgnoreCase(String name)`
- `Void Clear()`
- `Void ClearRows(Int32 capacity)`
- `Void ClearRowsWithFill(Int32 count)`
- `Void BeginChanges()`
- `Int32 AddRow()`
- `Void RemoveRow(Int32 row)`
- `Void RejectRow(Int32 row)`
- `Void EndChanges()`
- `DynamicDataColumn AddColumn(String name, Type type, Boolean nullable) (+1)`
- `BooleanDynamicDataColumn AddBooleanColumn(String name)`
- `NullableBooleanDynamicDataColumn AddNullableBooleanColumn(String name)`
- `CharDynamicDataColumn AddCharColumn(String name)`
- `NullableCharDynamicDataColumn AddNullableCharColumn(String name)`
- `SByteDynamicDataColumn AddSByteColumn(String name)`
- `NullableSByteDynamicDataColumn AddNullableSByteColumn(String name)`
- `ByteDynamicDataColumn AddByteColumn(String name)`
- `NullableByteDynamicDataColumn AddNullableByteColumn(String name)`
- `Int16DynamicDataColumn AddInt16Column(String name)`
- `NullableInt16DynamicDataColumn AddNullableInt16Column(String name)`
- `UInt16DynamicDataColumn AddUInt16Column(String name)`
- `NullableUInt16DynamicDataColumn AddNullableUInt16Column(String name)`
- `Int32DynamicDataColumn AddInt32Column(String name)`
- `NullableInt32DynamicDataColumn AddNullableInt32Column(String name)`
- `UInt32DynamicDataColumn AddUInt32Column(String name)`
- `NullableUInt32DynamicDataColumn AddNullableUInt32Column(String name)`
- `Int64DynamicDataColumn AddInt64Column(String name)`
- `NullableInt64DynamicDataColumn AddNullableInt64Column(String name)`
- `UInt64DynamicDataColumn AddUInt64Column(String name)`
- `NullableUInt64DynamicDataColumn AddNullableUInt64Column(String name)`
- `SingleDynamicDataColumn AddSingleColumn(String name)`
- `NullableSingleDynamicDataColumn AddNullableSingleColumn(String name)`
- `DoubleDynamicDataColumn AddDoubleColumn(String name)`
- `NullableDoubleDynamicDataColumn AddNullableDoubleColumn(String name)`
- `DecimalDynamicDataColumn AddDecimalColumn(String name)`
- `NullableDecimalDynamicDataColumn AddNullableDecimalColumn(String name)`
- `StringDynamicDataColumn AddStringColumn(String name)`
- `NullableStringDynamicDataColumn AddNullableStringColumn(String name)`
- `DateTimeDynamicDataColumn AddDateTimeColumn(String name)`
- `NullableDateTimeDynamicDataColumn AddNullableDateTimeColumn(String name)`
- `TimeSpanDynamicDataColumn AddTimeSpanColumn(String name)`
- `NullableTimeSpanDynamicDataColumn AddNullableTimeSpanColumn(String name)`
- `GuidDynamicDataColumn AddGuidColumn(String name)`
- `NullableGuidDynamicDataColumn AddNullableGuidColumn(String name)`
- `ByteArrayDynamicDataColumn AddByteArrayColumn(String name)`
- `DynamicDataColumn InsertColumn(Int32 index, String name, Type type, Boolean nullable) (+1)`
- `BooleanDynamicDataColumn InsertBooleanColumn(Int32 index, String name)`
- `NullableBooleanDynamicDataColumn InsertNullableBooleanColumn(Int32 index, String name)`
- `CharDynamicDataColumn InsertCharColumn(Int32 index, String name)`
- `NullableCharDynamicDataColumn InsertNullableCharColumn(Int32 index, String name)`
- `SByteDynamicDataColumn InsertSByteColumn(Int32 index, String name)`
- `NullableSByteDynamicDataColumn InsertNullableSByteColumn(Int32 index, String name)`
- `ByteDynamicDataColumn InsertByteColumn(Int32 index, String name)`
- `NullableByteDynamicDataColumn InsertNullableByteColumn(Int32 index, String name)`
- `Int16DynamicDataColumn InsertInt16Column(Int32 index, String name)`
- `NullableInt16DynamicDataColumn InsertNullableInt16Column(Int32 index, String name)`
- `UInt16DynamicDataColumn InsertUInt16Column(Int32 index, String name)`
- `NullableUInt16DynamicDataColumn InsertNullableUInt16Column(Int32 index, String name)`
- `Int32DynamicDataColumn InsertInt32Column(Int32 index, String name)`
- `NullableInt32DynamicDataColumn InsertNullableInt32Column(Int32 index, String name)`
- `UInt32DynamicDataColumn InsertUInt32Column(Int32 index, String name)`
- `NullableUInt32DynamicDataColumn InsertNullableUInt32Column(Int32 index, String name)`
- `Int64DynamicDataColumn InsertInt64Column(Int32 index, String name)`
- `NullableInt64DynamicDataColumn InsertNullableInt64Column(Int32 index, String name)`
- `UInt64DynamicDataColumn InsertUInt64Column(Int32 index, String name)`
- `NullableUInt64DynamicDataColumn InsertNullableUInt64Column(Int32 index, String name)`
- `SingleDynamicDataColumn InsertSingleColumn(Int32 index, String name)`
- `NullableSingleDynamicDataColumn InsertNullableSingleColumn(Int32 index, String name)`
- `DoubleDynamicDataColumn InsertDoubleColumn(Int32 index, String name)`
- `NullableDoubleDynamicDataColumn InsertNullableDoubleColumn(Int32 index, String name)`
- `DecimalDynamicDataColumn InsertDecimalColumn(Int32 index, String name)`
- `NullableDecimalDynamicDataColumn InsertNullableDecimalColumn(Int32 index, String name)`
- `StringDynamicDataColumn InsertStringColumn(Int32 index, String name)`
- `NullableStringDynamicDataColumn InsertNullableStringColumn(Int32 index, String name)`
- `DateTimeDynamicDataColumn InsertDateTimeColumn(Int32 index, String name)`
- `NullableDateTimeDynamicDataColumn InsertNullableDateTimeColumn(Int32 index, String name)`
- `TimeSpanDynamicDataColumn InsertTimeSpanColumn(Int32 index, String name)`
- `NullableTimeSpanDynamicDataColumn InsertNullableTimeSpanColumn(Int32 index, String name)`
- `GuidDynamicDataColumn InsertGuidColumn(Int32 index, String name)`
- `NullableGuidDynamicDataColumn InsertNullableGuidColumn(Int32 index, String name)`
- `ByteArrayDynamicDataColumn InsertByteArrayColumn(Int32 index, String name)`
- `DataTable ConvertToDataTable()`
- `DynamicDataTable ConvertFrom(DataTable dataTable)`
- `String GetDataVisualization()`
- `String GetDataRowVisualization(Int32 row)`
- `String GetMatrixVisualization()`
- `Boolean IsEmptyRow(Int32 row)`
- `Void ClearLastEmptyRows()`
- `Void Optimize()`
- `Void Compress()`

### `DynamicDataTypeExtensions` (Namespace: `TFlex.DOCs.Data`)
**Методы:**
- `Type GetLanguageType(DynamicDataType type)`

### `FrozenDefaultDynamicDataColumn`1` (Namespace: `TFlex.DOCs.Data`)
**Методы:**
- `T GetValue(Int32& row)`

### `FrozenDynamicDataColumn` (Namespace: `TFlex.DOCs.Data`)
**Методы:**
- `Object GetWrappedValue(Int32& row)`
- `Boolean IsNull(Int32& row)`
- `FrozenDynamicDataColumn`1 FromDefault(T fallbackValue)`

### `FrozenDynamicDataColumn`1` (Namespace: `TFlex.DOCs.Data`)
**Методы:**
- `Object GetWrappedValue(Int32& row)`
- `T GetValue(Int32& row)`
- `T GetLastOrDefaultValue(Int32 count)`
- `List`1 GetValues(Int32 count)`
- `Void AppendValues(HashSet`1 collection, Int32 count)`
- `ICollection`1 GetNonRepeatingValues(Int32 count)`
- `Boolean IsNull(Int32& row)`

### `FrozenStringDynamicDataColumn` (Namespace: `TFlex.DOCs.Data`)
**Методы:**
- `String GetValue(Int32& row)`
- `List`1 GetValues(Int32 count)`
- `Boolean IsNull(Int32& row)`

### `FrozenStructDynamicDataColumn`1` (Namespace: `TFlex.DOCs.Data`)
**Методы:**
- `Boolean IsNull(Int32& row)`

### `GuidArrayDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Guid[]
**Методы:**
- `GuidArrayDynamicData Create(Guid[] value)`
- `Object GetWrappedValue()`
- `Guid[] GetValue()`
- `Void SetValue(Guid[] value)`
- `Boolean IsNull()`

### `GuidDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Guid
**Методы:**
- `GuidDynamicData Create(Guid value)`
- `Object GetWrappedValue()`
- `Guid GetValue()`
- `Void SetValue(Guid value)`
- `Boolean IsNull()`
- `String GetString()`

### `GuidDynamicDataColumn` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType
**Методы:**
- `Guid GetGuidValue(Int32& row)`
- `String GetStringValue(Int32& row)`

### `GuidListDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: List`1
**Методы:**
- `GuidListDynamicData Create(List`1 value)`
- `Object GetWrappedValue()`
- `List`1 GetValue()`
- `Void SetValue(List`1 value)`
- `Boolean IsNull()`

### `Int16ArrayDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Int16[]
**Методы:**
- `Int16ArrayDynamicData Create(Int16[] value)`
- `Object GetWrappedValue()`
- `Int16[] GetValue()`
- `Void SetValue(Int16[] value)`
- `Boolean IsNull()`

### `Int16DynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Int16
**Методы:**
- `Int16DynamicData Create(Int16 value)`
- `Object GetWrappedValue()`
- `Int16 GetValue()`
- `Void SetValue(Int16 value)`
- `Boolean IsNull()`
- `String GetString()`

### `Int16DynamicDataColumn` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType
**Методы:**
- `Int32 GetInt32Value(Int32& row)`
- `String GetStringValue(Int32& row)`

### `Int16ListDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: List`1
**Методы:**
- `Int16ListDynamicData Create(List`1 value)`
- `Object GetWrappedValue()`
- `List`1 GetValue()`
- `Void SetValue(List`1 value)`
- `Boolean IsNull()`

### `Int32ArrayDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Int32[]
**Методы:**
- `Int32ArrayDynamicData Create(Int32[] value)`
- `Object GetWrappedValue()`
- `Int32[] GetValue()`
- `Void SetValue(Int32[] value)`
- `Boolean IsNull()`

### `Int32DynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Int32
**Методы:**
- `Int32DynamicData Create(Int32 value)`
- `Object GetWrappedValue()`
- `Int32 GetValue()`
- `Void SetValue(Int32 value)`
- `Boolean IsNull()`
- `String GetString()`

### `Int32DynamicDataColumn` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType
**Методы:**
- `Int32 GetInt32Value(Int32& row)`
- `String GetStringValue(Int32& row)`

### `Int32ListDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: List`1
**Методы:**
- `Int32ListDynamicData Create(List`1 value)`
- `Object GetWrappedValue()`
- `List`1 GetValue()`
- `Void SetValue(List`1 value)`
- `Boolean IsNull()`

### `Int64ArrayDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Int64[]
**Методы:**
- `Int64ArrayDynamicData Create(Int64[] value)`
- `Object GetWrappedValue()`
- `Int64[] GetValue()`
- `Void SetValue(Int64[] value)`
- `Boolean IsNull()`

### `Int64DynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Int64
**Методы:**
- `Int64DynamicData Create(Int64 value)`
- `Object GetWrappedValue()`
- `Int64 GetValue()`
- `Void SetValue(Int64 value)`
- `Boolean IsNull()`
- `String GetString()`

### `Int64DynamicDataColumn` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType
**Методы:**
- `Int32 GetInt32Value(Int32& row)`
- `Int64 GetInt64Value(Int32& row)`
- `String GetStringValue(Int32& row)`

### `Int64ListDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: List`1
**Методы:**
- `Int64ListDynamicData Create(List`1 value)`
- `Object GetWrappedValue()`
- `List`1 GetValue()`
- `Void SetValue(List`1 value)`
- `Boolean IsNull()`

### `ListDynamicData`1` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** IsList: Boolean, ItemsCount: Int32
**Методы:**
- `IEnumerable GetUniqueValues()`

### `NullableBooleanArrayDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Nullable`1[]
**Методы:**
- `NullableBooleanArrayDynamicData Create(Nullable`1[] value)`
- `Object GetWrappedValue()`
- `Nullable`1[] GetValue()`
- `Void SetValue(Nullable`1[] value)`
- `Boolean IsNull()`

### `NullableBooleanDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Nullable`1
**Методы:**
- `NullableBooleanDynamicData Create(Nullable`1 value)`
- `Object GetWrappedValue()`
- `Nullable`1 GetValue()`
- `Void SetValue(Nullable`1 value)`
- `Boolean IsNull()`
- `DynamicData UnwrapNullable()`

### `NullableBooleanDynamicDataColumn` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType
**Методы:**
- `String GetStringValue(Int32& row)`

### `NullableBooleanListDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: List`1
**Методы:**
- `NullableBooleanListDynamicData Create(List`1 value)`
- `Object GetWrappedValue()`
- `List`1 GetValue()`
- `Void SetValue(List`1 value)`
- `Boolean IsNull()`

### `NullableByteArrayDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Nullable`1[]
**Методы:**
- `NullableByteArrayDynamicData Create(Nullable`1[] value)`
- `Object GetWrappedValue()`
- `Nullable`1[] GetValue()`
- `Void SetValue(Nullable`1[] value)`
- `Boolean IsNull()`

### `NullableByteDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Nullable`1
**Методы:**
- `NullableByteDynamicData Create(Nullable`1 value)`
- `Object GetWrappedValue()`
- `Nullable`1 GetValue()`
- `Void SetValue(Nullable`1 value)`
- `Boolean IsNull()`
- `DynamicData UnwrapNullable()`

### `NullableByteDynamicDataColumn` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType
**Методы:**
- `String GetStringValue(Int32& row)`

### `NullableByteListDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: List`1
**Методы:**
- `NullableByteListDynamicData Create(List`1 value)`
- `Object GetWrappedValue()`
- `List`1 GetValue()`
- `Void SetValue(List`1 value)`
- `Boolean IsNull()`

### `NullableCharArrayDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Nullable`1[]
**Методы:**
- `NullableCharArrayDynamicData Create(Nullable`1[] value)`
- `Object GetWrappedValue()`
- `Nullable`1[] GetValue()`
- `Void SetValue(Nullable`1[] value)`
- `Boolean IsNull()`

### `NullableCharDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Nullable`1
**Методы:**
- `NullableCharDynamicData Create(Nullable`1 value)`
- `Object GetWrappedValue()`
- `Nullable`1 GetValue()`
- `Void SetValue(Nullable`1 value)`
- `Boolean IsNull()`
- `DynamicData UnwrapNullable()`

### `NullableCharDynamicDataColumn` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType
**Методы:**
- `String GetStringValue(Int32& row)`

### `NullableCharListDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: List`1
**Методы:**
- `NullableCharListDynamicData Create(List`1 value)`
- `Object GetWrappedValue()`
- `List`1 GetValue()`
- `Void SetValue(List`1 value)`
- `Boolean IsNull()`

### `NullableDateTimeArrayDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Nullable`1[]
**Методы:**
- `NullableDateTimeArrayDynamicData Create(Nullable`1[] value)`
- `Object GetWrappedValue()`
- `Nullable`1[] GetValue()`
- `Void SetValue(Nullable`1[] value)`
- `Boolean IsNull()`

### `NullableDateTimeDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Nullable`1
**Методы:**
- `NullableDateTimeDynamicData Create(Nullable`1 value)`
- `Object GetWrappedValue()`
- `Nullable`1 GetValue()`
- `Void SetValue(Nullable`1 value)`
- `Boolean IsNull()`
- `DynamicData UnwrapNullable()`

### `NullableDateTimeDynamicDataColumn` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType
**Методы:**
- `String GetStringValue(Int32& row)`

### `NullableDateTimeListDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: List`1
**Методы:**
- `NullableDateTimeListDynamicData Create(List`1 value)`
- `Object GetWrappedValue()`
- `List`1 GetValue()`
- `Void SetValue(List`1 value)`
- `Boolean IsNull()`

### `NullableDecimalArrayDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Nullable`1[]
**Методы:**
- `NullableDecimalArrayDynamicData Create(Nullable`1[] value)`
- `Object GetWrappedValue()`
- `Nullable`1[] GetValue()`
- `Void SetValue(Nullable`1[] value)`
- `Boolean IsNull()`

### `NullableDecimalDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Nullable`1
**Методы:**
- `NullableDecimalDynamicData Create(Nullable`1 value)`
- `Object GetWrappedValue()`
- `Nullable`1 GetValue()`
- `Void SetValue(Nullable`1 value)`
- `Boolean IsNull()`
- `DynamicData UnwrapNullable()`

### `NullableDecimalDynamicDataColumn` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType
**Методы:**
- `String GetStringValue(Int32& row)`

### `NullableDecimalListDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: List`1
**Методы:**
- `NullableDecimalListDynamicData Create(List`1 value)`
- `Object GetWrappedValue()`
- `List`1 GetValue()`
- `Void SetValue(List`1 value)`
- `Boolean IsNull()`

### `NullableDoubleArrayDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Nullable`1[]
**Методы:**
- `NullableDoubleArrayDynamicData Create(Nullable`1[] value)`
- `Object GetWrappedValue()`
- `Nullable`1[] GetValue()`
- `Void SetValue(Nullable`1[] value)`
- `Boolean IsNull()`

### `NullableDoubleDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Nullable`1
**Методы:**
- `NullableDoubleDynamicData Create(Nullable`1 value)`
- `Object GetWrappedValue()`
- `Nullable`1 GetValue()`
- `Void SetValue(Nullable`1 value)`
- `Boolean IsNull()`
- `DynamicData UnwrapNullable()`

### `NullableDoubleDynamicDataColumn` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType
**Методы:**
- `String GetStringValue(Int32& row)`

### `NullableDoubleListDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: List`1
**Методы:**
- `NullableDoubleListDynamicData Create(List`1 value)`
- `Object GetWrappedValue()`
- `List`1 GetValue()`
- `Void SetValue(List`1 value)`
- `Boolean IsNull()`

### `NullableDynamicDataColumn`1` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** IsNullable: Boolean
**Методы:**
- `Void SetDataValue(Int32& row, DynamicData value)`

### `NullableGuidArrayDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Nullable`1[]
**Методы:**
- `NullableGuidArrayDynamicData Create(Nullable`1[] value)`
- `Object GetWrappedValue()`
- `Nullable`1[] GetValue()`
- `Void SetValue(Nullable`1[] value)`
- `Boolean IsNull()`

### `NullableGuidDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Nullable`1
**Методы:**
- `NullableGuidDynamicData Create(Nullable`1 value)`
- `Object GetWrappedValue()`
- `Nullable`1 GetValue()`
- `Void SetValue(Nullable`1 value)`
- `Boolean IsNull()`
- `DynamicData UnwrapNullable()`

### `NullableGuidDynamicDataColumn` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType
**Методы:**
- `String GetStringValue(Int32& row)`

### `NullableGuidListDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: List`1
**Методы:**
- `NullableGuidListDynamicData Create(List`1 value)`
- `Object GetWrappedValue()`
- `List`1 GetValue()`
- `Void SetValue(List`1 value)`
- `Boolean IsNull()`

### `NullableInt16ArrayDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Nullable`1[]
**Методы:**
- `NullableInt16ArrayDynamicData Create(Nullable`1[] value)`
- `Object GetWrappedValue()`
- `Nullable`1[] GetValue()`
- `Void SetValue(Nullable`1[] value)`
- `Boolean IsNull()`

### `NullableInt16DynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Nullable`1
**Методы:**
- `NullableInt16DynamicData Create(Nullable`1 value)`
- `Object GetWrappedValue()`
- `Nullable`1 GetValue()`
- `Void SetValue(Nullable`1 value)`
- `Boolean IsNull()`
- `DynamicData UnwrapNullable()`

### `NullableInt16DynamicDataColumn` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType
**Методы:**
- `Int32 GetInt32Value(Int32& row)`
- `String GetStringValue(Int32& row)`

### `NullableInt16ListDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: List`1
**Методы:**
- `NullableInt16ListDynamicData Create(List`1 value)`
- `Object GetWrappedValue()`
- `List`1 GetValue()`
- `Void SetValue(List`1 value)`
- `Boolean IsNull()`

### `NullableInt32ArrayDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Nullable`1[]
**Методы:**
- `NullableInt32ArrayDynamicData Create(Nullable`1[] value)`
- `Object GetWrappedValue()`
- `Nullable`1[] GetValue()`
- `Void SetValue(Nullable`1[] value)`
- `Boolean IsNull()`

### `NullableInt32DynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Nullable`1
**Методы:**
- `NullableInt32DynamicData Create(Nullable`1 value)`
- `Object GetWrappedValue()`
- `Nullable`1 GetValue()`
- `Void SetValue(Nullable`1 value)`
- `Boolean IsNull()`
- `DynamicData UnwrapNullable()`

### `NullableInt32DynamicDataColumn` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType
**Методы:**
- `Int32 GetInt32Value(Int32& row)`
- `String GetStringValue(Int32& row)`

### `NullableInt32ListDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: List`1
**Методы:**
- `NullableInt32ListDynamicData Create(List`1 value)`
- `Object GetWrappedValue()`
- `List`1 GetValue()`
- `Void SetValue(List`1 value)`
- `Boolean IsNull()`

### `NullableInt64ArrayDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Nullable`1[]
**Методы:**
- `NullableInt64ArrayDynamicData Create(Nullable`1[] value)`
- `Object GetWrappedValue()`
- `Nullable`1[] GetValue()`
- `Void SetValue(Nullable`1[] value)`
- `Boolean IsNull()`

### `NullableInt64DynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Nullable`1
**Методы:**
- `NullableInt64DynamicData Create(Nullable`1 value)`
- `Object GetWrappedValue()`
- `Nullable`1 GetValue()`
- `Void SetValue(Nullable`1 value)`
- `Boolean IsNull()`
- `DynamicData UnwrapNullable()`

### `NullableInt64DynamicDataColumn` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType
**Методы:**
- `Int32 GetInt32Value(Int32& row)`
- `String GetStringValue(Int32& row)`

### `NullableInt64ListDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: List`1
**Методы:**
- `NullableInt64ListDynamicData Create(List`1 value)`
- `Object GetWrappedValue()`
- `List`1 GetValue()`
- `Void SetValue(List`1 value)`
- `Boolean IsNull()`

### `NullableSByteArrayDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Nullable`1[]
**Методы:**
- `NullableSByteArrayDynamicData Create(Nullable`1[] value)`
- `Object GetWrappedValue()`
- `Nullable`1[] GetValue()`
- `Void SetValue(Nullable`1[] value)`
- `Boolean IsNull()`

### `NullableSByteDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Nullable`1
**Методы:**
- `NullableSByteDynamicData Create(Nullable`1 value)`
- `Object GetWrappedValue()`
- `Nullable`1 GetValue()`
- `Void SetValue(Nullable`1 value)`
- `Boolean IsNull()`
- `DynamicData UnwrapNullable()`

### `NullableSByteDynamicDataColumn` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType
**Методы:**
- `Int32 GetInt32Value(Int32& row)`
- `String GetStringValue(Int32& row)`

### `NullableSByteListDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: List`1
**Методы:**
- `NullableSByteListDynamicData Create(List`1 value)`
- `Object GetWrappedValue()`
- `List`1 GetValue()`
- `Void SetValue(List`1 value)`
- `Boolean IsNull()`

### `NullableSingleArrayDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Nullable`1[]
**Методы:**
- `NullableSingleArrayDynamicData Create(Nullable`1[] value)`
- `Object GetWrappedValue()`
- `Nullable`1[] GetValue()`
- `Void SetValue(Nullable`1[] value)`
- `Boolean IsNull()`

### `NullableSingleDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Nullable`1
**Методы:**
- `NullableSingleDynamicData Create(Nullable`1 value)`
- `Object GetWrappedValue()`
- `Nullable`1 GetValue()`
- `Void SetValue(Nullable`1 value)`
- `Boolean IsNull()`
- `DynamicData UnwrapNullable()`

### `NullableSingleDynamicDataColumn` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType
**Методы:**
- `String GetStringValue(Int32& row)`

### `NullableSingleListDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: List`1
**Методы:**
- `NullableSingleListDynamicData Create(List`1 value)`
- `Object GetWrappedValue()`
- `List`1 GetValue()`
- `Void SetValue(List`1 value)`
- `Boolean IsNull()`

### `NullableStringDynamicDataColumn` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, IsNullable: Boolean
**Методы:**
- `String GetStringValue(Int32& row)`
- `IEnumerable`1 FindDuplicateRows(Int32 rowCount, StringComparison comparison)`

### `NullableTimeSpanArrayDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Nullable`1[]
**Методы:**
- `NullableTimeSpanArrayDynamicData Create(Nullable`1[] value)`
- `Object GetWrappedValue()`
- `Nullable`1[] GetValue()`
- `Void SetValue(Nullable`1[] value)`
- `Boolean IsNull()`

### `NullableTimeSpanDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Nullable`1
**Методы:**
- `NullableTimeSpanDynamicData Create(Nullable`1 value)`
- `Object GetWrappedValue()`
- `Nullable`1 GetValue()`
- `Void SetValue(Nullable`1 value)`
- `Boolean IsNull()`
- `DynamicData UnwrapNullable()`

### `NullableTimeSpanDynamicDataColumn` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType
**Методы:**
- `String GetStringValue(Int32& row)`

### `NullableTimeSpanListDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: List`1
**Методы:**
- `NullableTimeSpanListDynamicData Create(List`1 value)`
- `Object GetWrappedValue()`
- `List`1 GetValue()`
- `Void SetValue(List`1 value)`
- `Boolean IsNull()`

### `NullableUInt16ArrayDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Nullable`1[]
**Методы:**
- `NullableUInt16ArrayDynamicData Create(Nullable`1[] value)`
- `Object GetWrappedValue()`
- `Nullable`1[] GetValue()`
- `Void SetValue(Nullable`1[] value)`
- `Boolean IsNull()`

### `NullableUInt16DynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Nullable`1
**Методы:**
- `NullableUInt16DynamicData Create(Nullable`1 value)`
- `Object GetWrappedValue()`
- `Nullable`1 GetValue()`
- `Void SetValue(Nullable`1 value)`
- `Boolean IsNull()`
- `DynamicData UnwrapNullable()`

### `NullableUInt16DynamicDataColumn` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType
**Методы:**
- `Int32 GetInt32Value(Int32& row)`
- `String GetStringValue(Int32& row)`

### `NullableUInt16ListDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: List`1
**Методы:**
- `NullableUInt16ListDynamicData Create(List`1 value)`
- `Object GetWrappedValue()`
- `List`1 GetValue()`
- `Void SetValue(List`1 value)`
- `Boolean IsNull()`

### `NullableUInt32ArrayDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Nullable`1[]
**Методы:**
- `NullableUInt32ArrayDynamicData Create(Nullable`1[] value)`
- `Object GetWrappedValue()`
- `Nullable`1[] GetValue()`
- `Void SetValue(Nullable`1[] value)`
- `Boolean IsNull()`

### `NullableUInt32DynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Nullable`1
**Методы:**
- `NullableUInt32DynamicData Create(Nullable`1 value)`
- `Object GetWrappedValue()`
- `Nullable`1 GetValue()`
- `Void SetValue(Nullable`1 value)`
- `Boolean IsNull()`
- `DynamicData UnwrapNullable()`

### `NullableUInt32DynamicDataColumn` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType
**Методы:**
- `Int32 GetInt32Value(Int32& row)`
- `String GetStringValue(Int32& row)`

### `NullableUInt32ListDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: List`1
**Методы:**
- `NullableUInt32ListDynamicData Create(List`1 value)`
- `Object GetWrappedValue()`
- `List`1 GetValue()`
- `Void SetValue(List`1 value)`
- `Boolean IsNull()`

### `NullableUInt64ArrayDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Nullable`1[]
**Методы:**
- `NullableUInt64ArrayDynamicData Create(Nullable`1[] value)`
- `Object GetWrappedValue()`
- `Nullable`1[] GetValue()`
- `Void SetValue(Nullable`1[] value)`
- `Boolean IsNull()`

### `NullableUInt64DynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Nullable`1
**Методы:**
- `NullableUInt64DynamicData Create(Nullable`1 value)`
- `Object GetWrappedValue()`
- `Nullable`1 GetValue()`
- `Void SetValue(Nullable`1 value)`
- `Boolean IsNull()`
- `DynamicData UnwrapNullable()`

### `NullableUInt64DynamicDataColumn` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType
**Методы:**
- `Int32 GetInt32Value(Int32& row)`
- `String GetStringValue(Int32& row)`

### `NullableUInt64ListDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: List`1
**Методы:**
- `NullableUInt64ListDynamicData Create(List`1 value)`
- `Object GetWrappedValue()`
- `List`1 GetValue()`
- `Void SetValue(List`1 value)`
- `Boolean IsNull()`

### `SByteArrayDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: SByte[]
**Методы:**
- `SByteArrayDynamicData Create(SByte[] value)`
- `Object GetWrappedValue()`
- `SByte[] GetValue()`
- `Void SetValue(SByte[] value)`
- `Boolean IsNull()`

### `SByteDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: SByte
**Методы:**
- `SByteDynamicData Create(SByte value)`
- `Object GetWrappedValue()`
- `SByte GetValue()`
- `Void SetValue(SByte value)`
- `Boolean IsNull()`
- `String GetString()`

### `SByteDynamicDataColumn` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType
**Методы:**
- `Int32 GetInt32Value(Int32& row)`
- `String GetStringValue(Int32& row)`

### `SByteListDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: List`1
**Методы:**
- `SByteListDynamicData Create(List`1 value)`
- `Object GetWrappedValue()`
- `List`1 GetValue()`
- `Void SetValue(List`1 value)`
- `Boolean IsNull()`

### `SingleArrayDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Single[]
**Методы:**
- `SingleArrayDynamicData Create(Single[] value)`
- `Object GetWrappedValue()`
- `Single[] GetValue()`
- `Void SetValue(Single[] value)`
- `Boolean IsNull()`

### `SingleDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: Single
**Методы:**
- `SingleDynamicData Create(Single value)`
- `Object GetWrappedValue()`
- `Single GetValue()`
- `Void SetValue(Single value)`
- `Boolean IsNull()`
- `String GetString()`

### `SingleDynamicDataColumn` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType
**Методы:**
- `Single GetSingleValue(Int32& row)`
- `String GetStringValue(Int32& row)`

### `SingleListDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: List`1
**Методы:**
- `SingleListDynamicData Create(List`1 value)`
- `Object GetWrappedValue()`
- `List`1 GetValue()`
- `Void SetValue(List`1 value)`
- `Boolean IsNull()`

### `StringArrayDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: String[]
**Методы:**
- `StringArrayDynamicData Create(String[] value)`
- `Object GetWrappedValue()`
- `String[] GetValue()`
- `Void SetValue(String[] value)`
- `Boolean IsNull()`

### `StringDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: String
**Методы:**
- `StringDynamicData Create(String value)`
- `Object GetWrappedValue()`
- `String GetValue()`
- `Void SetValue(String value)`
- `Boolean IsNull()`
- `String GetString()`

### `StringDynamicDataColumn` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, FallbackValue: String
**Методы:**
- `List`1 GetValues(Int32 offset, Int32 count) (+1)`
- `String GetValue(Int32& row)`
- `Void SetValue(Int32& row, String& value)`
- `Object GetWrappedValue(Int32& row)`
- `Void SetWrappedValue(Int32& row, Object value)`
- `String GetStringValue(Int32& row)`
- `Boolean IsNull(Int32& row)`
- `IEnumerable`1 FindDuplicateRows(Int32 rowCount, StringComparison comparison)`
- `FrozenDynamicDataColumn`1 ToFrozen()`

### `StringListDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: List`1
**Методы:**
- `StringListDynamicData Create(List`1 value)`
- `Object GetWrappedValue()`
- `List`1 GetValue()`
- `Void SetValue(List`1 value)`
- `Boolean IsNull()`

### `StructDynamicDataColumn`1` (Namespace: `TFlex.DOCs.Data`)
**Методы:**
- `Boolean IsNull(Int32& row)`
- `FrozenDynamicDataColumn`1 ToFrozen()`

### `TimeSpanArrayDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: TimeSpan[]
**Методы:**
- `TimeSpanArrayDynamicData Create(TimeSpan[] value)`
- `Object GetWrappedValue()`
- `TimeSpan[] GetValue()`
- `Void SetValue(TimeSpan[] value)`
- `Boolean IsNull()`

### `TimeSpanDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: TimeSpan
**Методы:**
- `TimeSpanDynamicData Create(TimeSpan value)`
- `Object GetWrappedValue()`
- `TimeSpan GetValue()`
- `Void SetValue(TimeSpan value)`
- `Boolean IsNull()`
- `String GetString()`

### `TimeSpanDynamicDataColumn` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType
**Методы:**
- `String GetStringValue(Int32& row)`

### `TimeSpanListDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: List`1
**Методы:**
- `TimeSpanListDynamicData Create(List`1 value)`
- `Object GetWrappedValue()`
- `List`1 GetValue()`
- `Void SetValue(List`1 value)`
- `Boolean IsNull()`

### `UInt16ArrayDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: UInt16[]
**Методы:**
- `UInt16ArrayDynamicData Create(UInt16[] value)`
- `Object GetWrappedValue()`
- `UInt16[] GetValue()`
- `Void SetValue(UInt16[] value)`
- `Boolean IsNull()`

### `UInt16DynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: UInt16
**Методы:**
- `UInt16DynamicData Create(UInt16 value)`
- `Object GetWrappedValue()`
- `UInt16 GetValue()`
- `Void SetValue(UInt16 value)`
- `Boolean IsNull()`
- `String GetString()`

### `UInt16DynamicDataColumn` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType
**Методы:**
- `Int32 GetInt32Value(Int32& row)`
- `String GetStringValue(Int32& row)`

### `UInt16ListDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: List`1
**Методы:**
- `UInt16ListDynamicData Create(List`1 value)`
- `Object GetWrappedValue()`
- `List`1 GetValue()`
- `Void SetValue(List`1 value)`
- `Boolean IsNull()`

### `UInt32ArrayDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: UInt32[]
**Методы:**
- `UInt32ArrayDynamicData Create(UInt32[] value)`
- `Object GetWrappedValue()`
- `UInt32[] GetValue()`
- `Void SetValue(UInt32[] value)`
- `Boolean IsNull()`

### `UInt32DynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: UInt32
**Методы:**
- `UInt32DynamicData Create(UInt32 value)`
- `Object GetWrappedValue()`
- `UInt32 GetValue()`
- `Void SetValue(UInt32 value)`
- `Boolean IsNull()`
- `String GetString()`

### `UInt32DynamicDataColumn` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType
**Методы:**
- `Int32 GetInt32Value(Int32& row)`
- `String GetStringValue(Int32& row)`

### `UInt32ListDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: List`1
**Методы:**
- `UInt32ListDynamicData Create(List`1 value)`
- `Object GetWrappedValue()`
- `List`1 GetValue()`
- `Void SetValue(List`1 value)`
- `Boolean IsNull()`

### `UInt64ArrayDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: UInt64[]
**Методы:**
- `UInt64ArrayDynamicData Create(UInt64[] value)`
- `Object GetWrappedValue()`
- `UInt64[] GetValue()`
- `Void SetValue(UInt64[] value)`
- `Boolean IsNull()`

### `UInt64DynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: UInt64
**Методы:**
- `UInt64DynamicData Create(UInt64 value)`
- `Object GetWrappedValue()`
- `UInt64 GetValue()`
- `Void SetValue(UInt64 value)`
- `Boolean IsNull()`
- `String GetString()`

### `UInt64DynamicDataColumn` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType
**Методы:**
- `Int32 GetInt32Value(Int32& row)`
- `String GetStringValue(Int32& row)`

### `UInt64ListDynamicData` (Namespace: `TFlex.DOCs.Data`)
**Свойства:** Type: DynamicDataType, StructureType: DynamicDataStructureType, Value: List`1
**Методы:**
- `UInt64ListDynamicData Create(List`1 value)`
- `Object GetWrappedValue()`
- `List`1 GetValue()`
- `Void SetValue(List`1 value)`
- `Boolean IsNull()`

### `DataColumnReadAction` (Namespace: `TFlex.DOCs.Data.Extensions`)
**Методы:**
- `Void Read(DbDataReader reader, Int32 row)`

### `DataRowReadAction` (Namespace: `TFlex.DOCs.Data.Extensions`)
**Методы:**
- `Void Read()`

### `DynamicDataTableExtensions` (Namespace: `TFlex.DOCs.Data.Extensions`)
**Методы:**
- `DynamicDataTable Copy(DynamicDataTable dataTable, Int32 rowCapacity, Boolean withRows)`
- `Void CopyRow(DynamicDataTable dataTable, DynamicDataTable destination, Int32& row, Int32 destinationRow)`
- `ICollection`1 FindDuplicateRows(DynamicDataTable dataTable, StringComparison comparison)`
- `ICollection`1 FindDuplicateRowsWithGrouping(DynamicDataTable dataTable, Int32& count, StringComparison comparison) (+1)`
- `List`1 FindOrderedDuplicateRows(DynamicDataTable dataTable, StringComparison comparison)`
- `DynamicDataTable Concat(IReadOnlyList`1 dataTables)`
- `ValueTuple`2 CreateDataColumnWithNonRepeatingValues(IReadOnlyList`1 dataTables, String name)`
- `Void TrimStringColumnsValues(DynamicDataTable dataTable)`
- `Void RenameColumns(DynamicDataTable dataTable, String[] columnNames) (+1)`
- `Void ReplaceInValues(DynamicDataColumnWithFallbackValue`1 dataColumn, Char oldValue, String newValue)`
- `Void ReplaceValues(DynamicDataColumnWithFallbackValue`1 dataColumn, Char oldValue, Char newValue)`

