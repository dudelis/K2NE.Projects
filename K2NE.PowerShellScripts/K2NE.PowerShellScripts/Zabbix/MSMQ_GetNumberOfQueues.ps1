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
$k2configPath = "$(GetK2InstallPath)Host Server\Bin\K2HostServer.exe.config"
[xml] $k2config = Get-Content -Path $k2configPath

$msmqPath = $k2config.configuration."sourcecode.eventbus".msmqpath;
$msmqErrorpath = $k2config.configuration."sourcecode.eventbus".msmqerrorpath;

$msmq = Get-MsmqQueue | Where-Object {$_.Path -eq $msmqPath}
$msmqError = Get-MsmqQueue | Where-Object {$_.Path -eq $msmqErrorpath}


#Output results as JSON
try
    {
    #open json object
    write-output "{"
    #write results
    Write-Output "`"MsmqMessageCount : $($msmq.MessageCount)`""
    Write-Output "`"MsmqErrorMessageCount : $($msmqError.MessageCount)`""
    #close json object
    write-output "}"
    }
catch
    {
    Write-Output "{`"Error in Output results : $($_.Exception)`"}"
    Return
    }
    

