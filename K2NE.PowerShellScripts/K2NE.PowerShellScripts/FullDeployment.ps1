###################################################################################
# 
# This script reads settings xml, deploys the DB, refreshes the Service Instance and deploys the reports.
# 
###################################################################################

#Getting all deployment configurations
$settingsPath = $PSScriptRoot + "\Settings.config.xml"
$settings = [xml](get-content $settingsPath)

#region Deploying DBs
#If (@($settings.Settings.DataBases.DataBase).Count -gt 0)
#{
#	$dllDacPacPath = $PSScriptRoot + "\DLL\Microsoft.SqlServer.Dac.dll";
#	add-type -path $dllDacPacPath
#	$dacServiceParam = "server=" + $settings.Settings.SqlServerName;
#	$dacService = new-object Microsoft.SqlServer.Dac.DacServices $dacServiceParam
#	$settings.Settings.DataBases
#	ForEach ($db in $settings.Settings.DataBases.DataBase)
#	{
#		$dp = [Microsoft.SqlServer.Dac.DacPackage]::Load($db.DacPacName)
#		$dacOptions = new-object Microsoft.SqlServer.Dac.DacDeployOptions
#		ForEach ($var in $db.SqlCmdVariables.variable)
#		{        
#			$dacOptions.SqlCommandVariableValues.Add($var.Name, $var.Value)
#		}
#		Write-Host "Started deploying DataBase " $db.Name		
#		$dacService.deploy($dp, $db.Name, "True", $dacOptions)
#		Write-Host "Deployment finished!"
#	}
#	Write-Host -NoNewline "Press any key to continue . . . "
#	$null = $host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
#}
#endregion

#region Refreshing Service Instances
#If (@($settings.Settings.ServiceInstances.ServiceInstance).Count -gt 0)
#{
#	[System.Reflection.Assembly]::LoadWithPartialName("SourceCode.Hosting.Client.BaseAPI")
#	[System.Reflection.Assembly]::LoadWithPartialName("SourceCode.SmartObjects.Services.Management")
#	[System.Reflection.Assembly]::LoadWithPartialName("SourceCode.SmartObjects.Management")
#	$con = New-Object -TypeName SourceCode.Hosting.Client.BaseAPI.SCConnectionStringBuilder
#	$con.Host = $settings.Settings.Host
#	$con.Port = 5555
#	$con.Authenticate = $true
#	$con.IsPrimaryLogin = $true
#	$con.Integrated = $true
#	$smoSrv = New-Object -TypeName SourceCode.SmartObjects.Management.SmartObjectManagementServer
#	$mngSrv = New-Object -TypeName SourceCode.SmartObjects.Services.Management.ServiceManagementServer
#	Write-Host "Opening Connection to K2 Blackpearl"
#	$smoSrv.CreateConnection() | Out-Null 
#	$smoSrv.Connection.Open($con.ConnectionString)
#	$mngSrv.Connection = $smoSrv.Connection
#	ForEach ($si in $settings.settings.ServiceInstances.ServiceInstance)
#	{
#		If (-Not $smoSrv.ServiceInstanceExists($si.Name))
#		{
#			write-Host "Service instance " $si.Name " cannot be found!"
#		}
#		else
#		{
#			$siGuid = $smoSrv.GetServiceInstanceGuid([GUID]($settings.Settings.SqlBrokerGuid), $si.Name)
#			Write-Host "Refreshing Service Instance " $si.Name
#			$isRefreshed = $mngSrv.RefreshServiceInstance($siGuid)
#			If ($isRefreshed)
#			{
#				Write-Host "Service Instance " $si.Name " was successfully refreshed!"
#			}
#			else
#			{
#				Write-Host "Service Instance " $si.Name " was not refreshed!"
#			}
#		}
#	}
#	$mngSrv.Connection.Close()
#	$mngSrv.DeleteConnection()
#	Write-Host -NoNewline "Press any key to continue . . . "
#	$null = $host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
#}
#endregion

#region Deploying SSRS
$ReportServiceURI = "http://" + $settings.Settings.ReportServerName + "/ReportServer/ReportService2005.asmx"
ForEach ($report in $settings.Settings.Reports.Report)
{
    $RDLFileName = $PSScriptRoot + "\" + $report.RdlFileName
    
    write-host "----------------------------------------------------------------------------------------------------------------------------------------"
    write-host "Deploying" $report.RdlFileName
    write-host " to" $report.FolderName "as" $report.Name
    Write-Host -NoNewline "Press any key to continue . . . "
    $null = $host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
    $SSRSProxy = new-webserviceproxy -Uri $ReportServiceURI -Namespace SSRS.ReportingService2005 -UseDefaultCredential
    
    $Bytes = [System.IO.File]::ReadAllBytes($RDLFileName)
        try
        {
            $SSRSProxy.CreateFolder($report.FolderName, "/", $null)
            Write-Host "[Install-SSRSRDL()] Created new folder: $report.FolderName"
        }
        catch [System.Web.Services.Protocols.SoapException]
        {
            if ($_.Exception.Detail.InnerText -match "[^rsItemAlreadyExists400]")
            {
                Write-Host "[Install-SSRSRDL()] Folder: $report.FolderName already exists."
            }
            else
            {
                $msg = "[Install-SSRSRDL()] Error creating folder: $report.FolderName. Msg: '{0}'" -f $_.Exception.Detail.InnerText
                Write-Error $msg
            }
        }
    $Warnings = $SSRSProxy.CreateReport($report.Name, "/" + $report.FolderName, $True, $Bytes, $Null) 
    if ($Warnings) {
        foreach ($Warning in $Warnings) {
            write-warning $Warning.Message
        }
    }
}

#endregion

