$errors = Get-EventLog -Source "SourceCode.Logging.Extension.EventLogExtension" -LogName "Application" -EntryType Error
$error_count = $errors.Count
write-output $error_count