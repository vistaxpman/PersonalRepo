DECLARE @Today DATETIME = getdate()

SELECT
    CONVERT(varchar(25), DATEADD(dd, -(DAY(@Today)), @Today), 101) [Date],
    'Last Day of Previous Month' [Meaning]

UNION

SELECT
    CONVERT(varchar(25), DATEADD(dd, -(DAY(@Today) - 1), @Today), 101),
    'First Day of Current Month' AS Date_Type

UNION

SELECT
    CONVERT(varchar(25), @Today, 101),
    'Today' AS Date_Type

UNION

SELECT
    CONVERT(varchar(25), DATEADD(dd, -(DAY(DATEADD(mm, 1, @Today))), DATEADD(mm, 1, @Today)), 101),
    'Last Day of Current Month'

UNION

SELECT
    CONVERT(varchar(25), DATEADD(dd, -(DAY(DATEADD(mm, 1, @Today)) - 1), DATEADD(mm, 1, @Today)), 101),
    'First Day of Next Month'