namespace AlgorithmicTrading.Common.Events
{
    public class FeedbackEvent : ControllEvent
    {
        public FeedbackEvent(string feedback, Event payload)
        {
            Feedback = feedback;
            Payload = payload;
        }

        public string Feedback { get; private set; }

        public Event Payload { get; private set; }

        public override string ToString()
        {
            return string.Format("{0} {1}\n{2}", base.ToString(), Feedback, Payload);
        }
    }
}