$warnings = Get-EventLog -Source "SourceCode.Logging.Extension.EventLogExtension" -LogName "Application" -EntryType Warning -ErrorAction SilentlyContinue
$warning_count = $warnings.Count
write-output $warning_count