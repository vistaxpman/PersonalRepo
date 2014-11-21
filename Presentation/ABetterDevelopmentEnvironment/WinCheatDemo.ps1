[Reflection.Assembly]::LoadWithPartialName('System.Windows.Forms') | Out-Null
$form = New-Object Windows.Forms.Form
$textBox = New-Object Windows.Forms.TextBox
$textBox.Text = "I am a secret!"
$textBox.PassWordChar = '*'
$form.controls.add($textBox)
$form.ShowDialog() | Out-Null