param
(
    $fileName  = $(throw 'Please specify an input file name.')
)

if (!(Test-Path($fileName)))
{
	throw "The file: $inputFileName doesn't exist."
}

$original = Get-Item $fileName
$originalName = $original.FullName
$changedName = $originalName + '.bak'

if (Test-Path($changedName))
{
	throw "The bak file: $inputFileName does exist."
}

$gbkEncoding = [Text.Encoding]::GetEncoding("GBK");
$fileStream = New-Object IO.FileStream ($originalName, [IO.FileMode]::Open)
$reader = New-Object IO.BinaryReader ($fileStream, $gbkEncoding)
$fileInfo = New-Object IO.FileInfo ($originalName)
$gbkBytes = $reader.ReadBytes([int]$fileInfo.Length)
$reader.Close()

$unicodeEncoding = [Text.Encoding]::Unicode
$unicodeBytes = [Text.Encoding]::Convert($gbkEncoding, $unicodeEncoding, $gbkBytes)
$unicodeCharCount = $unicodeEncoding.GetCharCount($unicodeBytes, 0, $unicodeBytes.Length)
$unicodeChars = [Array]::CreateInstance([Type]::GetType('System.Char'), $unicodeCharCount)
$unicodeEncoding.GetChars($unicodeBytes, 0, $unicodeBytes.Length, $unicodeChars, 0) | Out-Null
$unicodeString = New-Object String (, $unicodeChars)

$original.MoveTo($changedName)
$writer = New-Object IO.StreamWriter ($originalName)
$writer.Write($unicodeString)
$writer.Close()