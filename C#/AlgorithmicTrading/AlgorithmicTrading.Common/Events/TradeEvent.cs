namespace AlgorithmicTrading.Common.Events
{
    public class TradeEvent : InstrumentEvent
    {
        public double Price { get; set; }

        public double Quantity { get; set; }

        public string Comment { get; set; }

        public override string ToString()
        {
            return string.Format("{0} Quantity: {1:F} Price: {2:F} Comment: {3}", base.ToString(), Quantity, Price, Comment); ;
        }
    }
}