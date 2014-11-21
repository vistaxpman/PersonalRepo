select
    *
from
    INFORMATION_SCHEMA.COLUMNS
where
    COLUMN_NAME like '%%'
    and
    TABLE_NAME like '%%'

select
    *
from
    INFORMATION_SCHEMA.TABLES
where
    TABLE_NAME like '%%'