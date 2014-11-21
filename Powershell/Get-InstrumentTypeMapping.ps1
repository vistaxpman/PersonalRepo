# should run under x86 Powershell!

$strFileName = "C:\Users\dong.jia\Desktop\Paladyne\InstrumentTypeAndConvertMap.xlsx"
$strSheetName = 'Mapping$'
$strProvider = "Provider=Microsoft.Jet.OLEDB.4.0"
$strDataSource = "Data Source = $strFileName"
$strExtend = "Extended Properties=Excel 8.0"
$strQuery = "Select * from [$strSheetName]"

$objConn = New-Object System.Data.OleDb.OleDbConnection("$strProvider;$strDataSource;$strExtend")
$sqlCommand = New-Object System.Data.OleDb.OleDbCommand($strQuery)
$sqlCommand.Connection = $objConn
$objConn.open()
$dataReader = $sqlCommand.ExecuteReader()

While($dataReader.read())
{
    if (-not [string]::IsNullOrWhiteSpace($dataReader[3]))
    {
        [String]::Format("{{`"{0}`", typeof({1})}},", $dataReader[3], $dataReader[2])
    }
}  
$dataReader.close()
$objConn.close()