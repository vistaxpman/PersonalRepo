namespace Tax
{
    internal class Bonus
    {
        internal Bonus(double amount)
        {
            Amount = amount;
        }

        internal double Amount { get; private set; }

        internal double IncomeTax
        {
            get
            {
                var entrty = TaxPolicy.GetIncomeRateEntry(Amount / 12);
                return Amount * entrty.IncomeRate - entrty.Deduction;
            }
        }

        internal double Net
        {
            get
            {
                return Amount - IncomeTax;
            }
        }
    }
}