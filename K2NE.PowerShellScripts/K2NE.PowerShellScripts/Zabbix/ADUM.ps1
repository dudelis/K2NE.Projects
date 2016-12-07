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

#Select the latest Adum log file
$files = Get-ChildItem -Path $k2path -Filter Adum*.log 

$counter = 0
foreach ($file in $files) 
{

    $reader = New-Object IO.StreamReader $file.FullName

    #read all lines and count actual entries
    
    while (($line = $reader.ReadLine()) -ne $null) 
    {
        if ($line -like "`"20*") {$counter++}
    }

}
Write-Output $counter