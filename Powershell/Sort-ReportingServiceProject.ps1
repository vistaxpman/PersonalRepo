param(
    [ValidateScript({Test-Path $_ -PathType Leaf -Filter '*.rptproj'})]
    [string] $projectFile
)

function Sort-ProjectItemsUnderNode($node) {
    $sortedChildNodes = $node.ChildNodes | sort -Property Name
    $node.ChildNodes | foreach {
        $node.RemoveChild($_) | Out-Null
    }
    $sortedChildNodes | foreach {
        $node.AppendChild($_) | Out-Null
    }
}

$projectFile = Resolve-Path $projectFile
$project = New-Object XML
$project.Load($projectFile)

$dataSetsNode = $project.Project.DataSets
$reportsNode = $project.Project.Reports

Sort-ProjectItemsUnderNode($dataSetsNode)
Sort-ProjectItemsUnderNode($reportsNode)

$project.Save($projectFile)