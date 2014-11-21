param(
    [string] $name
)

$name = "*$name*"
$Env:Path.Split(';') | foreach {
    if (-not (Test-Path $_ -PathType Container)) {
        Write-Host "The folder $_ in PATH doesn't exist." -ForegroundColor Red
        continue
    }
    Resolve-Path $_ | Get-ChildItem -Filter $name | where {-not $_.PSIsContainer} | foreach {
        $_.FullName
    }
}