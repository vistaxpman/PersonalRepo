param(
    [ValidateScript({Test-Path $_ -PathType Leaf -Filter '*.rdl'})]
    [string] $fileName
)

$fileName = Resolve-Path $fileName
$rdl = [Xml](Get-Content $fileName)
$rdl.Save("$fileName.bak")

$rows = $rdl.Report.ReportSections.ReportSection.Body.ReportItems.Tablix.TablixBody.TablixRows
$headerNodes = $rows.FirstChild.TablixCells.ChildNodes
$contentNodes = $rows.FirstChild.NextSibling.TablixCells.ChildNodes

$i = 0
for ( ; $i -lt $headerNodes.Count; $i++)
{
    $headerTextBox = $headerNodes.get_ItemOf($i).CellContents.Textbox
    $contentTextBox = $contentNodes.get_ItemOf($i).CellContents.Textbox
    if ($headerTextBox.UserSort -ne $null)
    {
        continue
    }
    $contentValue = $contentTextBox.Paragraphs.Paragraph.TextRuns.TextRun.Value
    $headerTextBox.InnerXml += "<UserSort><SortExpression>$contentValue</SortExpression></UserSort>"
}

$rdl.Save("$fileName")