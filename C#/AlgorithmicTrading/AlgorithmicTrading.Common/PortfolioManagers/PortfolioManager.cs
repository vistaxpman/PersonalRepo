using System;
using System.Collections.Generic;
using System.Linq;
using AlgorithmicTrading.Common.Events;

namespace AlgorithmicTrading.Common.PortfolioManagers
{
    public abstract class PortfolioManager : IPortfolioManager
    {
        protected readonly List<TradeEvent> Trades = new List<TradeEvent>();

        public abstract void Initialize();

        public Dictionary<string, double> GetPosition()
        {
            return GetPosition(DateTime.MaxValue);
        }

        public Dictionary<string, double> GetPosition(DateTime asofTime)
        {
            return (from trade in Trades
                    where trade.EventTime <= asofTime
                    group trade by trade.InstrumentKey).ToDictionary(grp => grp.Key, grp => grp.Sum(t => t.Quantity));
        }

        public void ChangePosition(TradeEvent trade)
        {
            Trades.Add(trade);
        }

        public void ChangePosition(IEnumerable<TradeEvent> trades)
        {
            Trades.AddRange(trades);
        }
    }
}