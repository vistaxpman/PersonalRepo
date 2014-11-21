param(
    [ValidateScript({Test-Path $_ -PathType Leaf -Filter '*.xml'})]
    [string] $idFile
)

$idFile = Resolve-Path $idFile
[xml]$ids = Get-Content $idFile
Select-Xml '//DocumentClauseDTO' $ids | % {'"' + $_.Node.Code + '",'}