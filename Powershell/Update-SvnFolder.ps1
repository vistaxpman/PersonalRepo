param(
    [ValidateScript({Test-Path $_ -PathType Container})]
    [string] $folderName = '.'
)

Resolve-Path $folderName | Get-ChildItem | foreach {
    . "C:\Program Files\TortoiseSVN\bin\TortoiseProc.exe" "/command:update /path:`"$_`" /closeonend:2"
}