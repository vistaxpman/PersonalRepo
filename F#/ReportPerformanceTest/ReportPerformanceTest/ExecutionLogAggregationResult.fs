module ExecutionLogAggregationResult

open System

type ExecutionLogAggregationResult =
    {
        ReportPath : string
        Count : int64
        TimeRequest : int64
        TimeDataRetrieval : int64
        TimeProcessing : int64
        TimeRendering : int64
    }

    override x.ToString() =
        String.Format("{0, -18}: {1}\n" +
                      "{2, -18}: {3}\n" +
                      "{4, -18}: {5}ms\n" +
                      "{6, -18}: {7}ms\n" +
                      "{8, -18}: {9}ms\n" +
                      "{10, -18}: {11}ms\n",
                      "ReportPath", x.ReportPath,
                      "Count", x.Count,
                      "TimeRequest", x.TimeRequest,
                      "TimeDataRetrieval", x.TimeDataRetrieval,
                      "TimeProcessing", x.TimeProcessing,
                      "TimeRendering", x.TimeRendering
        )
