param
(
    $executableFileName = $(throw 'Please specify an executable file name.'),
    $enable             = $false,
    $isWow              = $false
)

if (-not $executableFileName.EndsWith('.exe'))
{
    $executableFileName += '.exe'
}
if (-not $isWow)
{
    $registryKey = "HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\$executableFileName"
}
else
{
    $registryKey = "HKLM:\SOFTWARE\Wow6432Node\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\$executableFileName"
}
if ($enable)
{
    if (-not (Test-Path $registryKey))
    {
        $item = New-Item $registryKey
    }
    Set-ItemProperty -Path $registryKey -Name Debugger -Value 'vsjitdebugger.exe'
}
else
{
    if (Test-Path $registryKey)
    {
        Remove-Item $registryKey
    }
}
