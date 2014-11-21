SELECT
    [Column].name 'Column Name'
  , [Type].Name 'Data Type'
  , [Column].max_length 'Max Length'
  , [Column].precision 'Precision'
  , [Column].scale 'Is Scale'
  , [Column].is_nullable 'Is Nullable'
  , ISNULL([Index].is_primary_key, 0) 'Is Primary Key'
FROM
    sys.columns [Column]
    INNER JOIN sys.types [Type]
        ON  [Column].system_type_id = [Type].system_type_id
    LEFT OUTER JOIN sys.index_columns [ColumnIndex]
        ON  [ColumnIndex].object_id = [Column].object_id AND [ColumnIndex].column_id = [Column].column_id
    LEFT OUTER JOIN sys.indexes [Index]
        ON [ColumnIndex].object_id = [Index].object_id AND [ColumnIndex].index_id = [Index].index_id
WHERE
    [Column].object_id = OBJECT_ID('YourTableName')