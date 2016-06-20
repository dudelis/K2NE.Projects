# The script can be used to update Process Instance Fields

param([int]$ProcInstId, [string]$dataFieldName, [string]$dataFieldValue)

$ProcInstId = Read-Host "Enter your process Instance ID"
$dataFieldName = Read-Host "Enter the name of the Data Field you want to change"
$dataFieldValue = Read-Host "Enter the New Value of the data Field"

Add-Type -AssemblyName ("SourceCode.Workflow.Client, Version=4.0.0.0, Culture=neutral, PublicKeyToken=16a2c5aaaa1b130d")
$conn = New-Object -TypeName SourceCode.Workflow.Client.Connection
$conn.Open("localhost")
$pi = $conn.OpenProcessInstance($ProcInstId)

$dField = $pi.DataFields | ? {$_.Name -eq $dataFieldName}
if ($dField){
    $dField.Value = $dataFieldValue
    $pi.Update()
} else{
    [System.Console]::WriteLine("!!!Data Field $dataFieldName does not exist!!!")
}

$conn.close() 