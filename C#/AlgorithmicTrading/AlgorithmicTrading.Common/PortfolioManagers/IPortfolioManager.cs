using System;
using System.Collections.Generic;
using AlgorithmicTrading.Common.Events;

namespace AlgorithmicTrading.Common.PortfolioManagers
{
    public interface IPortfolioManager
    {
        void Initialize();

        Dictionary<string, double> GetPosition();

        Dictionary<string, double> GetPosition(DateTime asofTime);

        void ChangePosition(TradeEvent trade);

        void ChangePosition(IEnumerable<TradeEvent> trades);
    }
}