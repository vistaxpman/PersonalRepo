using AlgorithmicTrading.Common.Events;
using AlgorithmicTrading.Common.Utilities;
using com.espertech.esper.client;
using com.espertech.esper.client.time;
using System;

namespace AlgorithmicTrading.Common.EventEngines
{
    public abstract class EsperEventEngine : EventEngine
    {
        protected EPRuntime Runtime;
        protected EPAdministrator Administrator;

        public override DateTime CurrenTime
        {
            get { return Runtime.CurrentTime.ToTime(); }
        }

        public override void Initialize()
        {
            base.Initialize();

            var config = new Configuration();

            if (RunMode == RunMode.History)
            {
                config.EngineDefaults.ThreadingConfig.IsInternalTimerEnabled = false;
            }

            foreach (var eventType in Strategy.InputEventTypes)
            {
                config.AddEventType(eventType);
            }

            var provider = EPServiceProviderManager.GetDefaultProvider(config);
            Administrator = provider.EPAdministrator;
            Runtime = provider.EPRuntime;
        }

        public override void Start()
        {
            if (RunMode == RunMode.History)
            {
                Runtime.SendEvent(new CurrentTimeEvent(0));
            }
        }

        public override void SendEvent(Event @event)
        {
            base.SendEvent(@event);

            var timeControlEvent = @event as TimeControllEvent;
            if (timeControlEvent != null)
            {
                Runtime.SendEvent(new CurrentTimeEvent(timeControlEvent.Timestamp.ToMilliseconds()));
            }
            Runtime.SendEvent(@event);
        }

        protected override void PreValidate()
        {
        }

        protected override void PostValidate()
        {
        }
    }
}