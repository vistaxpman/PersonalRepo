module ReportResult

open System

type ReportResult =
    {
        ReportName : string
        RenderFileName : string
        TimeRefresh : int
        TimeRender : int
        ErrorMessage : string
    }

    override x.ToString() =
        let basicInformation = String.Format("{0, -35}{1}ms\nIsRendered:{2, -24}{3}ms\n",
                                   x.ReportName, x.TimeRefresh,
                                   not(String.IsNullOrEmpty(x.RenderFileName)), x.TimeRender)
        match String.IsNullOrEmpty(x.ErrorMessage) with
        | true -> basicInformation
        | _ -> String.Format("{0}{1}\n", basicInformation, x.ErrorMessage)
