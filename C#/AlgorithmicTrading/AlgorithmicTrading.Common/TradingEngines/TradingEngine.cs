using AlgorithmicTrading.Common.Events;
using AlgorithmicTrading.Common.EventSources;
using System;
using System.Collections.Generic;
using System.Threading;

namespace AlgorithmicTrading.Common.TradingEngines
{
    public abstract class TradingEngine : EventSource, ITradingEngine
    {
        protected TradingEngine(RunMode runMode)
            : base(runMode)
        {
            OutputEventTypes = new[] { typeof(TradeEvent), typeof(FailedTradeEvent) };
        }

        public abstract void ExcuteOrders(IEnumerable<OrderEvent> orders);

        public event EventHandler<EventEventArgs> TradeSuccessed;

        public event EventHandler<EventEventArgs> TradeFailed;

        protected void OnTradeTradeSuccessed(EventEventArgs e)
        {
            OnNewEvent(e);
            var handler = Interlocked.CompareExchange(ref TradeSuccessed, null, null);
            if (handler != null)
            {
                handler(this, e);
            }

        }

        protected void OnTradeFailed(EventEventArgs e)
        {
            OnNewEvent(e);
            var handler = Interlocked.CompareExchange(ref TradeFailed, null, null);
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}