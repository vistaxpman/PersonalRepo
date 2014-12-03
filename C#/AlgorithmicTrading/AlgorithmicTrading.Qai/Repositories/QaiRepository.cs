using Dapper;
using AlgorithmicTrading.Common;
using AlgorithmicTrading.Common.Events;
using AlgorithmicTrading.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace AlgorithmicTrading.Qai.Repositories
{
    public class QaiRepository
    {
        public Instrument[] GetInstruments(IEnumerable<string> ids)
        {
            const string query = "SELECT InfoCode AS [Key], DsQtName AS Name FROM qai.dbo.DS2CtryQtInfo WHERE InfoCode IN @Ids";
            using (var connection = getOpenedConnection())
            {
                var result = (from partition in ids.Partition()
                              from instrument in connection.Query<Instrument>(query, new { Ids = partition })
                              select instrument).ToArray();
                return result;
            }
        }

        public Event[] GetPrices(IEnumerable<string> ids, DateTime fromTime, DateTime toTime)
        {
            const string query =
                "SELECT MarketDate AS EventTime, InfoCode AS InstrumentKey,  Close_ AS Price FROM qai.dbo.DS2PrimQtPrc WHERE InfoCode IN @Ids AND MarketDate BETWEEN @FromTime AND @ToTime";
            using (var connection = getOpenedConnection())
            {
                var result = (from partition in ids.Partition()
                              from priceEvent in connection.Query<PriceEvent>(query, new { Ids = partition, FromTime = fromTime, ToTime = toTime })
                              select priceEvent).ToArray();
                return result;
            }
        }

        public Event[] GetStockSplits(IEnumerable<string> ids, DateTime fromTime, DateTime toTime)
        {
            const string query =
                "SELECT EffectiveDate AS EventTime, InfoCode AS InstrumentKey,  NumOldShares AS OldShares, NumNewShares AS NewShares FROM qai.dbo.DS2CapEvent WHERE InfoCode IN @Ids AND ActionTypeCode = 'SPLT' AND EffectiveDate BETWEEN @FromTime AND @ToTime";
            using (var connection = getOpenedConnection())
            {
                var result = (from partition in ids.Partition()
                              from priceEvent in connection.Query<StockSplitEvent>(query, new { Ids = partition, FromTime = fromTime, ToTime = toTime })
                              select priceEvent).ToArray();
                return result;
            }
        }

        private SqlConnection getOpenedConnection()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Qai"].ConnectionString;
            var connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }
    }
}