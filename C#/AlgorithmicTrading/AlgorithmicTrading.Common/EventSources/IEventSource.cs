using System;

namespace AlgorithmicTrading.Common.EventSources
{
    public interface IEventSource
    {
        RunMode RunMode { get; }

        DateTime FromTime { get; set; }

        DateTime ToTime { get; set; }

        string[] InstrumentKeys { get; set; }

        Type[] OutputEventTypes { get; }

        void Initialize();

        void Start();

        event EventHandler<EventEventArgs> NewEvent;
    }
}