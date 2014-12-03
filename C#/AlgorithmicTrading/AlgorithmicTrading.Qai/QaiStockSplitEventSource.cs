using AlgorithmicTrading.Common.Events;
using AlgorithmicTrading.Common.EventSources;
using AlgorithmicTrading.Qai.Repositories;

namespace AlgorithmicTrading.Qai
{
    public class QaiStockSplitEventSource : HistoricalEventSource
    {
        public QaiStockSplitEventSource()
        {
            OutputEventTypes = new[] { typeof(StockSplitEvent), };
        }

        public override void Initialize()
        {
            base.Initialize();

            Events = new QaiRepository().GetStockSplits(InstrumentKeys, FromTime, ToTime);
        }

        public override void Start()
        {
        }
    }
}