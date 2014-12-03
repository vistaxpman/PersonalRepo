namespace AlgorithmicTrading.Common.Events
{
    public class OrderEvent : BusinessEvent
    {
        public string InstrumentKey { get; set; }

        public double Quantity { get; set; }

        public double? Price { get; set; }

        public override string ToString()
        {
            return string.Format("{0}\n[{1}] Quantity: {2:F} Price: {3}", base.ToString(), InstrumentKey, Quantity, Price.HasValue ? Price.Value.ToString("F") : "N/A");
        }
    }
}