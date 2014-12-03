using AlgorithmicTrading.Common.EventSources;
using AlgorithmicTrading.Common.Exceptions;
using AlgorithmicTrading.Common.PortfolioManagers;
using AlgorithmicTrading.Common.Strategies;
using AlgorithmicTrading.Common.TradingEngines;
using System;
using System.Linq;
using System.Threading;

namespace AlgorithmicTrading.Common.Contexts
{
    public abstract class Context : IContext
    {
        protected Context(RunMode runMode)
        {
            RunMode = runMode;
            FromTime = ToTime = DateTime.MaxValue;
        }

        public RunMode RunMode { get; private set; }

        public string[] InstrumentKeys { get; set; }

        public DateTime FromTime { get; set; }

        public DateTime ToTime { get; set; }

        public IStrategy Strategy { get; set; }

        public IInstrumentManager InstrumentManager { get; set; }

        public IPortfolioManager PortfolioManager { get; set; }

        public ITradingEngine TradingEngine { get; set; }

        public IEventSource[] EventSources { get; set; }

        public virtual void Initialize()
        {
            PreValidate();

            InstrumentManager.InstrumentKeys = InstrumentKeys;
            InstrumentManager.Initialize();

            PortfolioManager.Initialize();

            Strategy.Context = this;
            Strategy.Initialize();

            EventSources = EventSources.Union(new[] { TradingEngine }).ToArray();
            foreach (var eventSource in EventSources)
            {
                eventSource.InstrumentKeys = InstrumentKeys;
                eventSource.FromTime = FromTime;
                eventSource.ToTime = ToTime;
                eventSource.Initialize();
                eventSource.NewEvent += (sender, e) => Strategy.EventEngine.SendEvent(e.Event);
            }
            TradingEngine.Initialize();

            PostValidate();
        }

        public virtual void Start()
        {
            TradingEngine.Start();
            Strategy.Start();

            foreach (var eventSource in EventSources)
            {
                eventSource.Start();
            }
        }

        public event EventHandler<EventEventArgs> Event;

        internal void OnEvent(EventEventArgs e)
        {
            var handler = Interlocked.CompareExchange(ref Event, null, null);
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void PreValidate()
        {
            if (InstrumentKeys == null || !InstrumentKeys.Any())
            {
                throw new InvalidContextException("No Instruments are monitored.");
            }

            if (Strategy == null)
            {
                throw new InvalidContextException("Strategy is null.");
            }

            if (InstrumentManager == null)
            {
                throw new InvalidContextException("InstrumentManager is null.");
            }

            if (PortfolioManager == null)
            {
                throw new InvalidContextException("PortfolioManager is null.");
            }

            if (TradingEngine == null)
            {
                throw new InvalidContextException("TradingEngine is null.");
            }

            if (EventSources == null || !EventSources.Any())
            {
                throw new InvalidContextException("EventSources is null or has no elements.");
            }

            if (EventSources.Any(source => source.RunMode != RunMode))
            {
                throw new InvalidContextException("Context has some EventSource with different runmode.");
            }
        }

        protected virtual void PostValidate()
        {
            var initialPosition = PortfolioManager.GetPosition();
            var instrumentsInPosition = initialPosition.Select(pair => pair.Key);
            if (instrumentsInPosition.Except(InstrumentKeys).Any())
            {
                throw new InvalidContextException("PortifolioManager has some instruments which are not in InsturmentManager.");
            }

            var eventSourceOutputEventTypes = (from eventSource in EventSources
                                               from eventType in eventSource.OutputEventTypes
                                               select eventType).ToArray();

            var inputEventTypes = Strategy.InputEventTypes;
            if (!inputEventTypes.Except(eventSourceOutputEventTypes).Any())
            {
                throw new InvalidContextException("Some EventSources required by Strategy are missing.");
            }
        }
    }
}