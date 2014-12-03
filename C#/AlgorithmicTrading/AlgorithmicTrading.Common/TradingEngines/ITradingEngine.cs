using AlgorithmicTrading.Common.Events;
using AlgorithmicTrading.Common.EventSources;
using System;
using System.Collections.Generic;

namespace AlgorithmicTrading.Common.TradingEngines
{
    public interface ITradingEngine : IEventSource
    {
        void ExcuteOrders(IEnumerable<OrderEvent> orders);

        event EventHandler<EventEventArgs> TradeSuccessed;

        event EventHandler<EventEventArgs> TradeFailed;
    }
}