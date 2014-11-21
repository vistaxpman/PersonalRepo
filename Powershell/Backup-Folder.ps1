param(
    [ValidateScript({Test-Path $_ -PathType Container})]
    [string] $source = '.',
    [ValidateScript({Test-Path $_ -PathType Container})]
    [string] $destination = 'V:\'
)

$source = (Resolve-Path $source).Path
$sourceInfo = New-Object 'System.IO.DirectoryInfo' $source
$destName = [IO.Path]::Combine($destination, $sourceInfo.Name + [DateTime]::Now.ToString('MMdd-HHmmss'))
Copy-Item $source -Destination $destName -Recurse