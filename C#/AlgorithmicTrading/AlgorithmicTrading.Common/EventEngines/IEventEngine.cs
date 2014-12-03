using AlgorithmicTrading.Common.Events;
using AlgorithmicTrading.Common.Strategies;
using System;

namespace AlgorithmicTrading.Common.EventEngines
{
    public interface IEventEngine
    {
        RunMode RunMode { get; }

        IStrategy Strategy { get; set; }

        DateTime CurrenTime { get; }

        void Initialize();

        void Start();

        void SendEvent(Event @event);

        event EventHandler<EventEventArgs> NewEvent;

        event EventHandler<EventEventArgs> Feedback;
    }
}