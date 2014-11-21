module ReportWindow

open System
open System.IO
open System.Net
open System.Text.RegularExpressions
open System.Windows.Forms
open Microsoft.Reporting.WinForms

#nowarn "67"
type ReportWindow() =
    let form = new Form(Width = 500, Height = 500)
    let viewer = new ReportViewer()
    do viewer.Dock <- DockStyle.Fill
    do viewer.ProcessingMode <- ProcessingMode.Remote
    do viewer.ServerReport.ReportServerUrl <- new Uri("http://prcmnsqldev/reportserver", UriKind.Absolute);
    do form.Controls.Add(viewer)

    member x.GetReportManagerMasterPage() =
        let request = WebRequest.Create("http://prcmnsqldev/Reports/Pages/Folder.aspx?ItemPath=%2fMaster&ViewMode=List")
        request.Credentials <- CredentialCache.DefaultCredentials;
        let response = request.GetResponse()
        let stream = response.GetResponseStream()
        let reader = new StreamReader(stream)
        reader.ReadToEnd()

    member x.GetReports(masterPage) =
        let matches = Regex.Matches(masterPage, @"(?<=/Reports/Pages/Report\.aspx\?ItemPath=%2fMaster%2f)[^""]*")
        seq {
            for m in matches ->
                m.Value.Replace('+', ' ')
        } |> Set.ofSeq |> Set.toList

    member x.RefreshReport(reportName) =
        let reportPath = "/Master/" + reportName
        viewer.ServerReport.ReportPath <- reportPath
        viewer.RefreshReport()

    member x.RenderReport(reportName) =
        let reportPath = "/Master/" + reportName
        viewer.ServerReport.ReportPath <- reportPath

        let mutable fileName = String.Concat(DateTime.Now.ToString("yyMMddhhmm "), reportName, ".xls")
        let mutable errorMessage = ""

        try
            let bytes, mimeType, encoding, extension, streamids, warnings = viewer.ServerReport.Render("Excel", null)
            File.WriteAllBytes(fileName, bytes)
        with
            | :? Exception as e ->
                fileName <- ""
                errorMessage <- e.Message
        fileName, errorMessage
