namespace AlgorithmicTrading.Common.Events
{
    public class PriceEvent : InstrumentEvent
    {
        public double Price { get; set; }

        public override string ToString()
        {
            return string.Format("{0} Price: {1:F}", base.ToString(), Price);
        }
    }
}