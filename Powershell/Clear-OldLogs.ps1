$tenDaysBefore = [DateTime]::Today.AddDays(-10)
Get-ChildItem C:\PrcmLogs -File -Recurse | ? {$_.LastWriteTime -ge $tenDaysBefore} | % {
    $_
    $_.Delete()
}