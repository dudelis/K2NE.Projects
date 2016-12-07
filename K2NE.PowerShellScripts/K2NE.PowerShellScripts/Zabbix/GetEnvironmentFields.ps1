param([string]$K2Server = "localhost", [string]$K2ManagementPort = "5555")

[Reflection.Assembly]::LoadWithPartialName("SourceCode.HostClientAPI") | out-null
[Reflection.Assembly]::LoadWithPartialName("SourceCode.SmartObjects.Client") | out-null

#Build and open connection to SmartObject Client API
$builder = new-object SourceCode.Hosting.Client.BaseAPI.SCConnectionStringBuilder;
$builder.Authenticate = $true;
$builder.Host = $K2Server;
$builder.Port = $K2ManagementPort;
$builder.Integrated = $true;
$builder.IsPrimaryLogin = $true;
$builder.SecurityLabelName = "K2";

$soServer = New-Object SourceCode.SmartObjects.Client.SmartObjectClientServer;
$soServer.CreateConnection() | out-null
$soServer.Connection.Open($builder.ConnectionString) | out-null;


#Get Default Environment
$soServiceInstance = $soServer.GetSmartObject("com_K2_System_Management_SmartObject_Environment")
$soServiceInstance.MethodToExecute = "GetDefaultEnvironment"

$DefaultEnvironment = $soServer.ExecuteScalar($soServiceInstance)
$defEnvGUID = $DefaultEnvironment.Properties["ID"].Value



#Get Environment Fields for Default Environment
$soServiceInstance2 = $soServer.GetSmartObject("com_K2_System_Management_SmartObject_EnvironmentField")
$soServiceInstance2.MethodToExecute = "List"

$soServiceInstance2.ListMethods["List"].InputProperties["EnvironmentId"].Value = $defEnvGUID

$EnvironmentFields = $soServer.ExecuteList($soServiceInstance2)

#Count Fields
$EnvFieldCount = $EnvironmentFields.SmartObjectsList.Count

Write-Output $EnvFieldCount