param(
    [ValidateScript({Test-Path $_ -PathType Leaf -Filter "*.rsd.sql"})]
    [string] $sqlFile
)

$sqlFile = Resolve-Path $sqlFile
$rsdFile = "$sqlFile".Replace('.sql', '')
$rsd = New-Object XML
$rsd.Load($rsdFile)
$sqlCommand = [IO.File]::ReadAllText($sqlFile).TrimEnd([Environment]::NewLine)
$rsd.SharedDataSet.DataSet.Query.CommandText = $sqlCommand
$rsd.Save($rsdFile)