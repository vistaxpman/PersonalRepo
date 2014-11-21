namespace Tax
{
    internal class Range
    {
        internal Range(double minValue, double maxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }

        internal double MinValue { get; private set; }

        internal double MaxValue { get; private set; }
    }
}