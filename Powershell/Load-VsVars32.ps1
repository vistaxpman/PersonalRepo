$vsvars32FullPath = [System.IO.Path]::Combine($Env:VS100COMNTOOLS, "vsvars32.bat")

$cmd = "`"$vsvars32FullPath`" & set"
cmd /c $cmd | foreach {
    $p, $v = $_.split('=')
    Set-Item -path env:$p -value $v
}

if (Test-IsAdministrator) {
    [System.Console]::Title = "Administrator: Visual Studio 2010 Windows PowerShell"
}
else {
    [System.Console]::Title = "Visual Studio 2010 Windows PowerShell"
}