# K2NE.Projects

## K2NE.PowerShellScripts
### SSRS deployment
This script deploys all reports specified in a CSV file. The structure of csv file is pretty much simple:
 - RDLFileName;
 - ReportServerName;
 - TargetReportFolder;
 - ReportName.

A deployment file is needed in a csv format, and it must be in the same directory as directory report files.
When running the script, you can specify the following parameters:
 - -SourceDirectory - where all Reports and CSV files are stored (Default Value - current directory of the script);
 - -DeploymentFileName - the name of the CSV file (Default Value - ListOfReports);
 - -ReportServername - the host name of your Reporting Services Web Service URL (Default Value - localhost)
