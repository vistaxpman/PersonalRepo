param(
    [ValidateScript({Test-Path $_ -PathType Leaf -Filter "*.rsd"})]
    [string] $rsdFile
)

$rsdFile = Resolve-Path $rsdFile
$rsd = New-Object XML
$rsd.Load($rsdFile)
$sql = "$rsdFile.sql"
[IO.File]::WriteAllText($sql, $rsd.SharedDataSet.DataSet.Query.CommandText)