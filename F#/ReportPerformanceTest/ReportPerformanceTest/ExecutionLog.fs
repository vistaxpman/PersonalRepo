module ExecutionLog

open System.Data.SqlClient
open System

open ExecutionLogResult

type ExecutionLog(startTime : DateTime, endTime : DateTime) =
    let startTime = startTime
    let endTime = endTime
    let connection = new SqlConnection("Data Source=PRCMNSQLDEV;Initial Catalog=ReportServer;Integrated Security=SSPI;")
    let command = new SqlCommand()
    do connection.Open()
    do command.Connection <- connection

    member x.GetExecutionLogResult() =
        command.CommandText <- String.Format("select\n" +
                                             "    DATEDIFF(ms,TimeStart,TimeEnd) TimeRequest\n" +
                                             "   ,ReportPath\n" +
                                             "   ,ReportAction\n" +
                                             "   ,TimeDataRetrieval\n" +
                                             "   ,TimeProcessing\n" +
                                             "   ,TimeRendering\n" +
                                             "   ,Status\n" +
                                             "   ,ByteCount\n" +
                                             "   ,[RowCount]\n" +
                                             "from\n" +
                                             "   ExecutionLog2\n" +
                                             "where\n" +
                                             "   UserName like '%dong%'\n" +
                                             "   and\n" +
                                             "   TimeStart between '{0}' and '{1}'\n" +
                                             "order by TimeStart DESC", startTime, endTime)

        let reader = command.ExecuteReader()
        seq {
            while reader.Read() do
                yield
                    {
                        TimeRequest = unbox (reader.["TimeRequest"])
                        ReportPath = unbox (reader.["ReportPath"])
                        ReportAction = unbox (reader.["ReportAction"])
                        TimeDataRetrieval = unbox (reader.["TimeDataRetrieval"])
                        TimeProcessing = unbox (reader.["TimeProcessing"])
                        TimeRendering = unbox (reader.["TimeRendering"])
                        Status = unbox (reader.["Status"])
                        ByteCount = unbox (reader.["ByteCount"])
                        RowCount = unbox (reader.["RowCount"])
                    }
        } |> List.ofSeq
