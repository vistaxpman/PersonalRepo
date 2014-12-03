namespace AlgorithmicTrading.Common.Events
{
    public abstract class InstrumentEvent : BusinessEvent
    {
        public string InstrumentKey { get; set; }

        public override string ToString()
        {
            return string.Format("{0}\n[{1}]", base.ToString(), InstrumentKey);
        }
    }
}