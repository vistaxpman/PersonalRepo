using AlgorithmicTrading.Common.Events;
using System;

namespace AlgorithmicTrading.Common.PortfolioManagers
{
    public class WeightedPortfolioPortfolioManager : PortfolioManager
    {
        public override void Initialize()
        {
            Trades.AddRange(new[]
                {
                    new TradeEvent { InstrumentKey = "72990", Quantity = 10000, EventTime = DateTime.MinValue, Comment = "Initial"},
                    new TradeEvent { InstrumentKey = "39988", Quantity = 10000, EventTime = DateTime.MinValue, Comment = "Initial"},
                    new TradeEvent { InstrumentKey = "46244", Quantity = 10000, EventTime = DateTime.MinValue, Comment = "Initial"},
                });
        }
    }
}