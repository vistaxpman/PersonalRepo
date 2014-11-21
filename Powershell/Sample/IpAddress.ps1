$hostName = [Net.Dns]::GetHostName()
$ipHostEntry = [Net.Dns]::GetHostEntry($hostName)
foreach ($ip in $ipHostEntry.AddressList)
{
	if ($ip.AddressFamily -eq [Net.Sockets.AddressFamily]::InterNetwork)
	{
		$ip.IPAddressToString
	}
}