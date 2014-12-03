using AlgorithmicTrading.Common.Events;
using AlgorithmicTrading.Common.EventSources;
using AlgorithmicTrading.Qai.Repositories;

namespace AlgorithmicTrading.Qai
{
    public class QaiPriceEventSource : HistoricalEventSource
    {
        public QaiPriceEventSource()
        {
            OutputEventTypes = new[] { typeof(PriceEvent), };
        }

        public override void Initialize()
        {
            base.Initialize();

            Events = new QaiRepository().GetPrices(InstrumentKeys, FromTime, ToTime);
        }

        public override void Start()
        {
        }
    }
}