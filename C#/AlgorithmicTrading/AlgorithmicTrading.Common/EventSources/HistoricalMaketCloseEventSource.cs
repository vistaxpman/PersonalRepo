using AlgorithmicTrading.Common.Events;
using AlgorithmicTrading.Common.Utilities;
using System.Linq;

namespace AlgorithmicTrading.Common.EventSources
{
    public class HistoricalMaketCloseEventSource : HistoricalEventSource
    {
        public HistoricalMaketCloseEventSource()
        {
            OutputEventTypes = new[] { typeof(DailyMarketCloseEvent), };
        }

        public override void Initialize()
        {
            base.Initialize();

            Events = (from date in FromTime.GetDateRange(ToTime)
                      select new DailyMarketCloseEvent { EventTime = date }).ToArray();
        }

        public override void Start()
        {
        }
    }
}