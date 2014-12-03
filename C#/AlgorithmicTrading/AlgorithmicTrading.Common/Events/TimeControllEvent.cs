using System;
using AlgorithmicTrading.Common.Utilities;

namespace AlgorithmicTrading.Common.Events
{
    public class TimeControllEvent : ControllEvent
    {
        public TimeControllEvent(DateTime timestamp)
        {
            Timestamp = timestamp;
        }

        public DateTime Timestamp { get; private set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", base.ToString(), Timestamp.GetDisplayString());
        }
    }
}