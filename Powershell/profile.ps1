function Get-Batchfile ($file) {
    $cmd = "`"$file`" & set"
    cmd /c $cmd | Foreach-Object {
        $p, $v = $_.split('=')
        Set-Item -path env:$p -value $v
    }
}

function VsVars32($version = '12.0')
{
    if ([System.Environment]::Is64BitOperatingSystem)
    {
        $key = 'HKLM:SOFTWARE\Wow6432Node\Microsoft\VisualStudio\'
    }
    else
    {
        $key = 'HKLM:SOFTWARE\Microsoft\VisualStudio\'
    }
    $key += $version
    $VsKey = get-ItemProperty $key
    $VsInstallPath = [System.IO.Path]::GetDirectoryName($VsKey.InstallDir)
    $VsToolsDir = [System.IO.Path]::GetDirectoryName($VsInstallPath)
    $VsToolsDir = [System.IO.Path]::Combine($VsToolsDir, "Tools")
    $BatchFile = [System.IO.Path]::Combine($VsToolsDir, "vsvars32.bat")
    Get-Batchfile $BatchFile
    [System.Console]::Title = "Visual Studio " + $version + " Windows Powershell"
}

function prompt()
{
    "$(Get-Location) >"
}

VsVars32