param([string]$K2Server = "localhost", [string]$K2ManagementPort = "5555")
[Reflection.Assembly]::LoadWithPartialName("SourceCode.Workflow.Management") | out-null
Add-Type -AssemblyName ('SourceCode.Workflow.Client, Version=4.0.0.0, Culture=neutral, PublicKeyToken=16a2c5aaaa1b130d')

$currentUser = [System.Security.Principal.WindowsIdentity]::GetCurrent().Name

try
{
$K2ServerManagementConn = New-Object SourceCode.Workflow.Management.WorkflowManagementServer($K2Server, $K2ManagementPort)
$K2ServerManagementConn.Open()
}
catch
{
Write-Output "{`"Cannot connect K2 Server : $($_.Exception)`"}"
Return
}
$ErrorProfile = $K2ServerManagementConn.GetErrorProfile("All")
$ErrorLogs = $K2ServerManagementConn.GetErrorLogs($ErrorProfile.ID)

$processes = $ErrorLogs.ProcessName | get-unique | sort

#open json object
write-output "{"

$pcount = $processes.Count
$pos = 0

foreach ($process in $processes)
    {
    $count = ($ErrorLogs | where {$_.ProcessName -eq $processes[$pos]}).Count; $pos++
    #write single JSON Object paars - only last row without comma at the end
    If ($pos -ne $pcount) {Write-Output "`"$($process)`": $($count),"}
    Else {Write-Output "`"$($process)`": $($count)"}
    
    }

#close json object
write-output "}"


$K2ServerManagementConn.Connection.Close()