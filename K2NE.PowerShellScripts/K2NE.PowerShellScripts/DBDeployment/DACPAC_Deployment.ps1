###################################################################################
# 
# This script deploys the dataBase to the SQL server, including SQLCMD variables
# 
###################################################################################

# Parameters to
param([string]$DacPacFilename, [string]$ServerName, [string]$DBname)

$DacPacFilename = Read-Host "Enter the name of the DACPAC file"
$ServerName = Read-Host "Enter the name of the your SQL Server"
$DBname = Read-Host "Enter the name of the your DataBase"

#Adding a laibrary
add-type -path "C:\Program Files (x86)\Microsoft SQL Server\110\DAC\bin\Microsoft.SqlServer.Dac.dll"
#Instantiating a new dacService object
$dacServiceParam = "server=" + $ServerName
$dacService = new-object Microsoft.SqlServer.Dac.DacServices $dacServiceParam
#Loading the dacpac file
$dp = [Microsoft.SqlServer.Dac.DacPackage]::Load($DacPacFilename)

$yes = New-Object System.Management.Automation.Host.ChoiceDescription "&Yes",""
$no = New-Object System.Management.Automation.Host.ChoiceDescription "&No",""
$choices = [System.Management.Automation.Host.ChoiceDescription[]]($yes,$no)
$caption = "Warning!"
$message = "Are there any SQLCMD variables in your package?"
$result = $Host.UI.PromptForChoice($caption,$message,$choices,0)
$dacOptions = new-object Microsoft.SqlServer.Dac.DacDeployOptions
#Adding SQLCMD variables if there are any
if($result -eq 0)
{
	
	while ($result -eq 0)
	{
		$varName = Read-Host "Enter the name of the SQLCMD variable"
		
		if ($dacOptions.SqlCommandVariableValues.ContainsKey($varName))
		{
			Write-Host "Current SQLCMD variable has already been added"
		}
		else
		{
			$varValue = Read-Host "Enter the value of " $varName	
			$dacOptions.SqlCommandVariableValues.Add($varName, $varValue)
		}
		$message = "Are there any other SQLCMD variables?"
		$result = $Host.UI.PromptForChoice($caption,$message,$choices,0)
	}
}
Write-Host "Deployment started"
$dacService.deploy($dp, $DBname, "True", $dacOptions)
Write-Host "Deployment finished!"
Write-Host -NoNewline "Press any key to continue . . . "
$null = $host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")