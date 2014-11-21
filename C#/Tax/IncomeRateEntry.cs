namespace Tax
{
    internal class IncomeRateEntry
    {
        internal IncomeRateEntry(double incomeRate, double deduction)
        {
            IncomeRate = incomeRate;
            Deduction = deduction;
        }

        internal double IncomeRate { get; private set; }

        internal double Deduction { get; private set; }

        internal static IncomeRateEntry GetIncomeRateEntry(double taxableAmount)
        {
            if (taxableAmount < 0)
                return _incomeRateEntries[0];

            if (taxableAmount < 1500)
                return _incomeRateEntries[1];

            if (taxableAmount < 4500)
                return _incomeRateEntries[2];

            if (taxableAmount < 9000)
                return _incomeRateEntries[3];

            if (taxableAmount < 35000)
                return _incomeRateEntries[4];

            if (taxableAmount < 55000)
                return _incomeRateEntries[5];

            if (taxableAmount < 80000)
                return _incomeRateEntries[6];

            return _incomeRateEntries[7];
        }

        private static IncomeRateEntry[] _incomeRateEntries = new[]
            {
                new IncomeRateEntry(0D, 0D),            // 0: < 0
                new IncomeRateEntry(3D / 100, 0D),      // 1: < 1500
                new IncomeRateEntry(10D / 100, 105D),   // 2: < 4500
                new IncomeRateEntry(20D / 100, 555D),   // 3: < 9000
                new IncomeRateEntry(25D / 100, 1005D),  // 4: < 35000
                new IncomeRateEntry(30D / 100, 2755D),  // 5: < 55000
                new IncomeRateEntry(35D / 100, 5505D),  // 6: < 80000
                new IncomeRateEntry(45D / 100, 13505D), // 7: >= 80000
           };
    }
}