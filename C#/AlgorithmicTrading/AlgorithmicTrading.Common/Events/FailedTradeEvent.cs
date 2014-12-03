namespace AlgorithmicTrading.Common.Events
{
    public class FailedTradeEvent : InstrumentEvent
    {
        public double Price { get; set; }

        public double Quantity { get; set; }

        public string Comment { get; set; }
    }
}