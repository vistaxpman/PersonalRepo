using AlgorithmicTrading.Common.Utilities;
using System;

namespace AlgorithmicTrading.Common.Events
{
    public abstract class Event
    {
        public DateTime EventTime { get; set; }

        public override string ToString()
        {
            return string.Format("{0}: {1}", GetType().Name, EventTime.GetDisplayString());
        }
    }
}