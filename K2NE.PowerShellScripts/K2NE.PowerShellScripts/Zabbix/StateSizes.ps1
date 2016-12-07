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

    #SQL Query String
        [string] $sqlCommand = "
                                SELECT AVG(DATALENGTH(State)) AS AVGStateSize
                                FROM [K2].Server.ProcInst WITH (NOLOCK)
                                WHERE Status IN (1, 2) 

                                SELECT AVG(StateSize) as AVGStateSizeTop10
                                FROM (SELECT TOP 10 ProcInst.ID, DATALENGTH(State) AS StateSize
                                FROM [K2].Server.ProcInst WITH (NOLOCK)
                                WHERE Status IN (1, 2) 
                                ORDER BY DATALENGTH(State) DESC) as top10

                                SELECT COUNT(ID) as StateSizeGT1MB
                                FROM K2.Server.ProcInst WITH(NOLOCK)
                                WHERE Status IN (1, 2) AND DATALENGTH(State)/1048576.0 >= 1
                                "

    #Open SQL Connection 
    $connection = new-object system.data.SqlClient.SQLConnection($connectionString)
    $command = new-object system.data.sqlclient.sqlcommand($sqlCommand,$connection)
    $connection.Open()
    }
catch
    {
    Write-Output "{`"Error in Open SQL Connection : $($_.Exception)`"}"
    Return
    }
#Execute Query
try
    {
    $adapter = New-Object System.Data.sqlclient.sqlDataAdapter $command
    $dataset = New-Object System.Data.DataSet
    $adapter.Fill($dataSet) | Out-Null

    $connection.Close()
    }
catch
    {
    Write-Output "{`"Error in Execute Query : $($_.Exception)`"}"
    Return
    }


#Output results as JSON
try
    {
    #open json object
    write-output "{"
    #write results
    Write-Output "`"AVGStateSize : $($dataSet.Tables[0].Rows[0].AVGStateSize)`""
    Write-Output "`"AVGStateSizeTop10 : $($dataSet.Tables[1].Rows[0].AVGStateSizeTop10)`""
    Write-Output "`"StateSizeGT1MB : $($dataSet.Tables[2].Rows[0].StateSizeGT1MB)`""
    #close json object
    write-output "}"
    }
catch
    {
    Write-Output "{`"Error in Output results : $($_.Exception)`"}"
    Return
    }
    