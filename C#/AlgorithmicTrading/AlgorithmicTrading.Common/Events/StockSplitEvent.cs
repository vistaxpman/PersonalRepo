namespace AlgorithmicTrading.Common.Events
{
    public class StockSplitEvent : InstrumentEvent
    {
        public double OldShares { get; set; }

        public double NewShares { get; set; }

        public override string ToString()
        {
            return string.Format("{0} OldShares: {1} NewShares: {2}", base.ToString(), OldShares, NewShares);
        }
    }
}