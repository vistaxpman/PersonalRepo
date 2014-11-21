namespace Tax
{
    internal class Salary
    {
        internal Salary(double amount)
        {
            Amount = amount;
        }

        internal double Amount { get; private set; }

        internal double InsuranceBase
        {
            get
            {
                return GetCalculateBase(TaxPolicy.InsuranceRange);
            }
        }

        internal double HousingFundBase
        {
            get
            {
                return GetCalculateBase(TaxPolicy.HousingFundRange);
            }
        }

        internal double EndowmentInsurance
        {
            get
            {
                return InsuranceBase * TaxPolicy.EndowmentInsuranceRate;
            }
        }

        internal double HealthInsuracne
        {
            get
            {
                return InsuranceBase * TaxPolicy.HealthInsuracneRate;
            }
        }

        internal double UnemploymentInsurance
        {
            get
            {
                return InsuranceBase * TaxPolicy.UnemploymentInsuranceRate;
            }
        }

        internal double Insurance
        {
            get
            {
                return EndowmentInsurance + HealthInsuracne + UnemploymentInsurance;
            }
        }

        internal double HousingFund
        {
            get
            {
                return HousingFundBase * TaxPolicy.HousingFundRate;
            }
        }

        internal double TaxableImcome
        {
            get
            {
                return Amount - Insurance - HousingFund - TaxPolicy.IncomeTaxExemption;
            }
        }

        internal double IncomeTax
        {
            get
            {
                var entrty = TaxPolicy.GetIncomeRateEntry(TaxableImcome);
                return TaxableImcome * entrty.IncomeRate - entrty.Deduction;
            }
        }

        internal double Net
        {
            get
            {
                return Amount - Insurance - HousingFund - IncomeTax;
            }
        }

        private double GetCalculateBase(Range range)
        {
            var maxValue = range.MaxValue;
            if (Amount > maxValue)
                return range.MaxValue;

            var minValue = range.MinValue;
            if (Amount < minValue)
                return range.MinValue;

            return Amount;
        }
    }
}