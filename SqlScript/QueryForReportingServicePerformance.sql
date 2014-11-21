select top 100
    DATEDIFF(ms,TimeStart,TimeEnd) Request
   ,ReportPath
   ,replace(UserName, 'PINERIVERCAPITA\', '') UserName
   ,Parameters
   ,ReportAction
   ,TimeStart
   ,TimeEnd
   ,TimeDataRetrieval
   ,TimeProcessing
   ,TimeRendering
   ,Source
   ,Status
   ,ByteCount
   ,[RowCount]
   ,AdditionalInfo
from
    ReportServer.dbo.ExecutionLog2
where
    UserName like '%%'
    and
    ReportPath like '%%'
order by TimeStart DESC