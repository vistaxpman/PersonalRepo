module Excel

open System
open System.Collections.Generic
open System.IO
open Microsoft.Office.Interop.Excel

open ReportResult
open ExecutionLogAggregationResult

type Excel() =
    let application = new ApplicationClass()
    let workbook = application.Workbooks.Add(XlWBATemplate.xlWBATWorksheet)

    member x.RenderForRepertResults reprotResults =
        let worksheet = workbook.Worksheets.[1] :?> Worksheet
        let title = "Performance for Reports"
        let titles = [| "Name"; "TimeReport"; "TimeRender" |]
        let reportDataArray1D = (reprotResults |> List.map (fun x ->
                [|
                    x.ReportName :> Object
                    x.TimeRefresh :> Object
                    x.TimeRender :> Object
                |]
            ) |> Array.ofList)
        let reportDataArray2D = Array2D.init reportDataArray1D.Length titles.Length (fun i j ->
            reportDataArray1D.[i].[j]
        )
        x.RenderWorksheet worksheet title titles reportDataArray2D

    member x.RenderForExecutionLogResults logAggregationResults =
        let worksheet = workbook.Worksheets.Add(After = workbook.Worksheets.[1]) :?> Worksheet
        let title = "Performance for ReportActions"
        let titles = [| "ReportPath"; "Count"; "TimeRequest"; "TimeDataRetrieval"; "TimeProcessing"; "TimeRendering" |]
        let reportDataArray1D = (logAggregationResults |> List.map (fun x ->
                [|
                    x.ReportPath :> Object
                    x.Count :> Object
                    x.TimeRequest / x.Count  :> Object
                    x.TimeDataRetrieval / x.Count :> Object
                    x.TimeProcessing / x.Count :> Object
                    x.TimeRendering / x.Count :> Object
                |]
            ) |> Array.ofList
        )

        let reportDataArray2D = Array2D.init reportDataArray1D.Length titles.Length (fun i j ->
            reportDataArray1D.[i].[j]
        )
        x.RenderWorksheet worksheet title titles reportDataArray2D

    member x.RenderWorksheet worksheet title titles reportDataArray2D =
        worksheet.Name <- title
        let endColumn = string('A' + char(titles.Length - 1))
        let endRow = string(Array2D.length1(reportDataArray2D) + 1)
        worksheet.Range("A1", endColumn + "1").Value2 <- titles
        
        worksheet.Range("A2", endColumn + endRow).Value2 <- reportDataArray2D
        let chartObjects = (worksheet.ChartObjects()) :?> ChartObjects
        let chartObject = chartObjects.Add(200.0, 20.0, 750.0, 350.0)

        chartObject.Chart.ChartWizard(
            Title = title,
            Source = worksheet.Range("A1", endColumn + endRow),
            Gallery = XlChartType.xl3DColumn,
            PlotBy = XlRowCol.xlColumns,
            SeriesLabels = 1,
            CategoryLabels = 1,
            CategoryTitle = "",
            ValueTitle = "ms"
        )

    interface IDisposable with
        member x.Dispose() =
            let xlsxFile = Helper.fileName + ".xlsx"

            if File.Exists(xlsxFile) then
                File.Delete(xlsxFile)

            workbook.SaveAs(xlsxFile)
            workbook.Close()
            application.Quit()
