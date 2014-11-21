param
(
    $dirctory  = $(throw 'Please specify an directory name.')
)

$list = Get-ChildItem $dirctory | sort

for($i = 0; $i -lt $list.Length; $i++)
{
	$item = $list[$i]
	$newName = '{0}\{1:D2}.mp3' -f $item.DirectoryName, ($i+1)
	$item.MoveTo($newName)
}