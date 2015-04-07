###################################################################################
# 
# This script deploys all reports specified in a CSV file.
# A deployment file is needed, in a csv format, and it must be in the same
# as directory report files. You'll have to set the $SourceDirectory and the
# $DeploymentFileName variables.
# 
###################################################################################

# Parameters to
param([string]$SourceDirectory = $PSScriptRoot, [string]$DeploymentFileName = "ListOfReports.csv", [string]$ReportServerName = "localhost")
Write-Host "Source Directory: $SourceDirectory"
Write-Host "Deployment File Name: $DeploymentFileName"
Write-Host "Report Server Name: $ReportServerName"

$FullDeploymentFileName = $SourceDirectory + "\" + $DeploymentFileName

Import-Csv $FullDeploymentFileName | foreach {
    $ReportServiceURI = "http://" + $ReportServerName + "/ReportServer/ReportService2005.asmx"
    $RDLFileName = $SourceDirectory + "\" + $_.RDLFileName
    
    write-host "----------------------------------------------------------------------------------------------------------------------------------------"
    write-host "Deploying" $RDLFileName
    write-host " to" $_.TargetReportFolder "as" $_.ReportName
    
    $SSRSProxy = new-webserviceproxy -Uri $ReportServiceURI -Namespace SSRS.ReportingService2005 -UseDefaultCredential
    
    $Bytes = [System.IO.File]::ReadAllBytes($RDLFileName)
        try
        {
            $SSRSProxy.CreateFolder($_.TargetReportFolder, "/", $null)
            Write-Host "[Install-SSRSRDL()] Created new folder: $_.TargetReportFolder"
        }
        catch [System.Web.Services.Protocols.SoapException]
        {
            if ($_.Exception.Detail.InnerText -match "[^rsItemAlreadyExists400]")
            {
                Write-Host "[Install-SSRSRDL()] Folder: $_.TargetReportFolder already exists."
            }
            else
            {
                $msg = "[Install-SSRSRDL()] Error creating folder: $_.TargetReportFolder. Msg: '{0}'" -f $_.Exception.Detail.InnerText
                Write-Error $msg
            }
        }


    $Warnings = $SSRSProxy.CreateReport($_.ReportName, "/" + $_.TargetReportFolder, $True, $Bytes, $Null) 

    if ($Warnings) {
        foreach ($Warning in $Warnings) {
            write-warning $Warning.Message
        }
    }
}
