module ExecutionLogResult

open System

type ExecutionLogResult = 
    {
        ReportPath : string
        ReportAction : string
        TimeRequest : int
        TimeDataRetrieval : int
        TimeProcessing : int
        TimeRendering : int
        Status : string
        ByteCount : int64
        RowCount : int64
    }

    override x.ToString() =
        String.Format("{0, -18}: {1}\n" +
                      "{2, -18}: {3}\n" +
                      "{4, -18}: {5}ms\n" +
                      "{6, -18}: {7}ms\n" +
                      "{8, -18}: {9}ms\n" +
                      "{10, -18}: {11}ms\n" +
                      "{12, -18}: {13}\n" +
                      "{14, -18}: {15}\n" +
                      "{16, -18}: {17}\n",
                      "ReportPath", x.ReportPath,
                      "ReportAction", x.ReportAction,
                      "TimeRequest", x.TimeRequest,
                      "TimeDataRetrieval", x.TimeDataRetrieval,
                      "TimeProcessing", x.TimeProcessing,
                      "TimeRendering", x.TimeRendering,
                      "Status", x.Status,
                      "ByteCount", x.ByteCount,
                      "RowCount", x.RowCount
        )
