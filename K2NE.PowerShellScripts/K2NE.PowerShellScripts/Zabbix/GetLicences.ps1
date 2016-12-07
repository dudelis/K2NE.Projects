param([string]$K2Server = "localhost", [string]$K2ManagementPort = "5555")

#Find K2 Directory
    Function GetK2InstallPath([string]$machine = $env:computername) {
        $registryKeyLocation = "SOFTWARE\SourceCode\BlackPearl\blackpearl Core\" 
        $registryKeyName = "InstallDir"

                    Write-Debug "Getting K2 install path from $machine "
    
        $reg = [Microsoft.Win32.RegistryKey]::OpenRemoteBaseKey([Microsoft.Win32.RegistryHive]::LocalMachine, $machine)
        $regKey= $reg.OpenSubKey($registryKeyLocation)
        $installDir = $regKey.GetValue($registryKeyName)
        return $installDir
    }
    $k2path = "$(GetK2InstallPath)Host Server\Bin"

[Reflection.Assembly]::LoadWithPartialName("SourceCode.HostClientAPI") | out-null
[Reflection.Assembly]::LoadFrom("$($k2path)\SourceCode.HostServerInterfaces.dll") | out-null
[Reflection.Assembly]::LoadWithPartialName("K2Licensing") | out-null
[Reflection.Assembly]::LoadWithPartialName("SourceCode.LicenseManagementAPI") | out-null
[Reflection.Assembly]::LoadWithPartialName("SourceCode.SmartObjects.Client") | out-null

$builder = new-object SourceCode.Hosting.Client.BaseAPI.SCConnectionStringBuilder;
$builder.Authenticate = $true;
$builder.Host = $K2Server;
$builder.Port = $K2ManagementPort;
$builder.Integrated = $true;
$builder.IsPrimaryLogin = $true;
$builder.SecurityLabelName = "K2";

#Connect to License API
$liServer = New-Object SourceCode.Hosting.Client.LicenseManagementAPI;
$liServer.CreateConnection() | out-null
$liServer.Connection.Open($builder.ConnectionString) | out-null;


#Connect to Smart Object API
$soServer = New-Object SourceCode.SmartObjects.Client.SmartObjectClientServer;
$soServer.CreateConnection() | out-null
$soServer.Connection.Open($builder.ConnectionString) | out-null;

#Get Licensed User Smart Object & Count active Users
$soServiceInstance = $soServer.GetSmartObject("com_K2_System_Licensing_SmartObject_LicensedUser")
$soServiceInstance.MethodToExecute = "List"
$ActiveUsers = $soServer.ExecuteList($soServiceInstance)
$ActiveUsersCount = $ActiveUsers.SmartObjectsList.Count


#Get License Details
#$liServer.GetLicenseProductsWithServerKeys()
$licenseTable = $liServer.GetLicenseKeysTable()

#Open JSON object
Write-Output "{"

foreach ($license in $licenseTable.Rows)
{
    #Open JSON inner object
    Write-Output "{"
    write-output "`"Product : $($license.LicensedProduct)`""
    write-output "`"Expiry Date : $($license.ExpiryDate)`""
    if ($license.LicensedProduct -eq "K2 blackpearl") 
        {write-output "`"Licensed Users : $($license.LicensedUsers)`"" 
         write-output "`"Active Users : $($ActiveUsersCount)`""}
    #Close JSON inner object
    Write-Output "}"
}

#Close JSON object
Write-Output "}"


$liServer.Connection.Close()
$soServer.Connection.Close()