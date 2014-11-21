select
    COLUMN_NAME
   ,DATA_TYPE
   ,IS_NULLABLE
   ,TYPE.TYPE
   ,'[Column] public ' + TYPE + NULLABLE + ' ' + COLUMN_NAME + '{get; set;}' 
NO
from
    INFORMATION_SCHEMA.COLUMNS
    outer apply
    (
        select 
            case
                when DATA_TYPE = 'bit' then 'bool'
                when DATA_TYPE in ('date', 'datetime') then 'DateTime'
                when DATA_TYPE in ('varchar', 'nvarchar') then 'string'
                when DATA_TYPE = 'float' then 'double'
                else DATA_TYPE
            end TYPE
    ) TYPE
    outer apply
    (
        select
            case
                when IS_NULLABLE = 'YES' and TYPE != 'string' then '?'
                else ''
            end NULLABLE
    ) NULLABLE
where
    TABLE_NAME like 'vw_Trades2'