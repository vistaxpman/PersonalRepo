declare @FromDate datetime;
set @FromDate = DATEADD(m, -1, GETDATE());
declare @ToDate datetime;
set @ToDate = GETDATE();

with DaysCTE as
(
    select
        @FromDate Day
    union all
    select
        DATEADD(d, 1, Day)
    from
        DaysCTE
    where
        Day < @ToDate
)
select
    Day
from
    DaysCTE;