using AlgorithmicTrading.Common.EventSources;
using AlgorithmicTrading.Common.PortfolioManagers;
using AlgorithmicTrading.Common.Strategies;
using AlgorithmicTrading.Common.TradingEngines;
using System;

namespace AlgorithmicTrading.Common.Contexts
{
    public interface IContext
    {
        RunMode RunMode { get; }

        string[] InstrumentKeys { get; set; }

        DateTime FromTime { get; set; }

        DateTime ToTime { get; set; }

        IStrategy Strategy { get; set; }

        IInstrumentManager InstrumentManager { get; set; }

        IPortfolioManager PortfolioManager { get; set; }

        ITradingEngine TradingEngine { get; set; }

        IEventSource[] EventSources { get; set; }

        void Initialize();

        void Start();

        event EventHandler<EventEventArgs> Event;
    }
}