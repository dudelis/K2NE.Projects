#Get the K2 config file
try
    {
    #Catch current Path
    $scriptpath = (Get-Item -Path ".\" -Verbose).FullName

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

    #Copy K2HostServer.exe.Config
    Copy-Item -Path "$($k2path)\K2HostServer.exe.config" -Destination "$($scriptpath.ToString())\web.config" #| out-null
    }
catch
    {
    Write-Output "{`"Error in Copy K2HostServer.exe.Config : $($_.Exception)`"}"
    Return
    }
#Decrypt K2 config file
try
    {
    #Decrypt Connection Strings
    cd "$(($(Get-ChildItem env:windir).Value).ToString())\Microsoft.NET\Framework\v4.0.30319\"
    .\aspnet_regiis.exe -pdf "connectionStrings" $scriptpath | out-null
    cd $scriptpath
    }
catch
    {
    Write-Output "{`"Error in Decrypt Connection Strings : $($_.Exception)`"}"
    Return
    }
#Connect to SQL
try
    {
    #Get Connection String from encrypted K2HostConfig
    [ xml ]$webConfig = Get-Content -Path "$($scriptpath)\web.config"
    $connectionString = ($webConfig.configuration.connectionStrings.add | Where-Object {$_.name -eq "HostserverDB"}).connectionString

    #Remove decrypted config file
    Remove-Item "$($scriptpath)\web.config" -Force -ErrorAction Continue

    #Open SQL Connection 
	$connection = new-object system.data.SqlClient.SQLConnection
	$connection.ConnectionString = $connectionString
	$command = new-object system.data.sqlclient.sqlcommand($sqlCommand,$connection)
    $connection.Open()
    }
catch
    {
    Write-Output "{`"Error in Open SQL Connection : $($_.Exception)`"}"
	$connection.Close()
    Return
    }

#Execute Query
try
    {	
	#Users
    $command = $connection.CreateCommand()
    $command.CommandText = "SELECT COUNT(ID) FROM [Identity].[Identity] WHERE [Type] = 1 AND [Enabled]=1 AND [ExpireOn] < GETDATE()"
    $outdatedUsers = $command.ExecuteScalar()
	#Roles
	$command.CommandText = "SELECT COUNT(ID) FROM [Identity].[Identity] WHERE [Type] = 2 AND [Enabled]=1 AND [ExpireOn] < GETDATE()"
    $outdatedRoles = $command.ExecuteScalar()
	#Groups	
	$command.CommandText = "SELECT COUNT(ID) FROM [Identity].[Identity] WHERE [Type] = 3 AND [Enabled]=1 AND [ExpireOn] < GETDATE()"
    $outdatedGroups = $command.ExecuteScalar()

    $connection.Close()
    }
catch
    {
    Write-Output "{`"Error in Execute Query : $($_.Exception)`"}"
	$connection.close()
    Return
    }

#Output results as JSON
try
    {
    #open json object
    write-output "{"
    #write results
    Write-Output "`"OutdatedUsers : $($outdatedUsers)`""
    Write-Output "`"OutdatedRoles : $($outdatedRoles)`""
    Write-Output "`"OutdatedGroups : $($outdatedGroups)`""
    #close json object
    write-output "}"
    }
catch
    {
    Write-Output "{`"Error in Output results : $($_.Exception)`"}"
    Return
    }