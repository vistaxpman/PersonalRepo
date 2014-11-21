function EncodeAndDisplay([string]$sourceString, [System.Text.Encoding]$encoding)
{
	Write-Host $encoding
	$encodingBytes = $encoding.GetBytes($sourceString)
	$encodingBytes | ForEach-Object -Process {Write-Host ('{0:X2} ' -f $_) -NoNewline}
	''
	$encodingBytes | ForEach-Object -Process {Write-Host ('{0} ' -f ('{0, 8}' -f [Convert]::ToString($_, 2) -replace ' ', '0')) -NoNewline}
	''
}

$asciiEncoding = [System.Text.Encoding]::ASCII
$utf8Encoding = [System.Text.Encoding]::UTF8
$utf32Encoding = [System.Text.Encoding]::UTF32

$sourceString = 'Hello!中国'
EncodeAndDisplay $sourceString $asciiEncoding
EncodeAndDisplay $sourceString $utf8Encoding
EncodeAndDisplay $sourceString $utf32Encoding