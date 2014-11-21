module Main

open System
open System.Diagnostics
open System.IO
open System.Text
open System.Text.RegularExpressions
open System.Collections.Generic

open ReportResult
open ExecutionLogResult
open ExecutionLogAggregationResult
open Excel
open ReportWindow
open ExecutionLog

let ConvertBeijingTimeToCentralAmericaStandardTime(time:DateTime) =
    let utcTime = TimeZoneInfo.ConvertTimeToUtc(time)
    let timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central America Standard Time")
    TimeZoneInfo.ConvertTimeFromUtc(utcTime, timeZoneInfo)

let RunReports() =
    let reportWindow = new ReportWindow()
    reportWindow.GetReportManagerMasterPage() |> reportWindow.GetReports |>
    List.map (fun reportName ->
        Console.Write("Refreshing {0} report ...... ", reportName)
        let stopwatch = Stopwatch.StartNew()
        reportWindow.RefreshReport(reportName)
        let refreshElapsed = stopwatch.Elapsed
        Console.WriteLine("Done {0} ms", refreshElapsed.TotalMilliseconds)

        Console.Write("Renderring {0} report ...... ", reportName)
        let stopwatch = Stopwatch.StartNew()
        let excelFileName, errorMessage = reportWindow.RenderReport(reportName)
        let renderElapsed = stopwatch.Elapsed
        Console.WriteLine("Done {0} ms", renderElapsed.TotalMilliseconds)

        {
            ReportName = reportName
            RenderFileName = excelFileName
            TimeRefresh = int(refreshElapsed.TotalMilliseconds)
            TimeRender = int(renderElapsed.TotalMilliseconds)
            ErrorMessage = errorMessage
        }
    )

let CalculateExecutionLogAggregationResults (logResults: list<ExecutionLogResult>) =
    let logAggragationResult = new Dictionary<string, ExecutionLogAggregationResult>()
    logResults |> List.iter (fun x ->
        match logAggragationResult.TryGetValue(x.ReportPath) with
        | (true, result) ->
            let newValue = { result with
                                    Count = result.Count + 1L
                                    TimeRequest = result.TimeRequest + int64(x.TimeRequest)
                                    TimeDataRetrieval = result.TimeDataRetrieval + int64(x.TimeDataRetrieval)
                                    TimeProcessing = result.TimeProcessing + int64(x.TimeProcessing)
                                    TimeRendering = result.TimeRendering + int64(x.TimeRendering)
                            }
            logAggragationResult.[x.ReportPath] <- newValue
        | (false, _) ->
            logAggragationResult.Add(x.ReportPath, {
                ReportPath = x.ReportPath
                Count = 1L
                TimeRequest = int64(x.TimeRequest)
                TimeDataRetrieval = int64(x.TimeDataRetrieval)
                TimeProcessing = int64(x.TimeProcessing)
                TimeRendering = int64(x.TimeRendering)
            }
        )
    )
    logAggragationResult.Values |> List.ofSeq

[<EntryPoint>]
let Main(args) =
    let startTime = ConvertBeijingTimeToCentralAmericaStandardTime(DateTime.Now.AddMinutes(-1.0))
    let reportResults = RunReports() |> List.sortWith (fun x y ->
        y.TimeRefresh - x.TimeRefresh
    )
    let endTime = ConvertBeijingTimeToCentralAmericaStandardTime(DateTime.Now.AddMinutes(1.0))

    let log = new ExecutionLog(startTime, endTime)
    let logResults = log.GetExecutionLogResult() |> List.sortWith (fun x y -> y.TimeRequest - x.TimeRequest)
    let logAggregationResults =
        CalculateExecutionLogAggregationResults logResults
            |> List.sortWith (fun (x:ExecutionLogAggregationResult) (y:ExecutionLogAggregationResult) ->
                int(y.TimeRequest- x.TimeRequest)
            )

    do
        use excel = new Excel()
        excel.RenderForRepertResults reportResults
        excel.RenderForExecutionLogResults logAggregationResults

    let stringBuilder = new StringBuilder()
    stringBuilder.AppendLine("Time to refresh/render report:\n") |> ignore
    reportResults |> List.iter (fun x -> stringBuilder.AppendLine(x.ToString()) |> ignore)

    stringBuilder.AppendLine("Aggregated Time for ReportActions:\n") |> ignore
    logAggregationResults |> List.iter (fun x -> stringBuilder.AppendLine(x.ToString()) |> ignore)

    stringBuilder.AppendLine("Time for ReportActions:\n") |> ignore
    logResults |> List.iter (fun x -> stringBuilder.AppendLine(x.ToString()) |> ignore)

    let logFile = Helper.fileName + ".log"
    File.WriteAllText(logFile, stringBuilder.ToString())

    let duration = endTime - startTime
    let totalReports = reportResults |> List.length
    let totalRenderred = reportResults |> List.filter (fun x -> not(String.IsNullOrEmpty(x.RenderFileName))) |> List.length

    stringBuilder.Clear().AppendLine(DateTime.Now.ToString("yyyy-MM-dd")).AppendLine(
        String.Format(
            "Run {0} reports totally, and {1} reports are renderred successfully in {2:F} minutes.",
            totalReports, totalRenderred, duration.TotalMinutes)) |> ignore

    Mail.sendResult(["dong.jia@prcm.com; fuxin.li@prcm.com; pengpeng.li@prcm.com; ming.yan@prcm.com"], "SQL Report Performace Result on " + DateTime.Now.ToString("MM/dd/yyyy"),
        stringBuilder.ToString(), [logFile; Helper.fileName + ".xlsx"])
    0
