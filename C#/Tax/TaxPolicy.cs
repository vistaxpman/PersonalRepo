namespace Tax
{
    internal static class TaxPolicy
    {
        #region Rates

        internal const double EndowmentInsuranceRate = 8D / 100;
        internal const double HealthInsuracneRate = 2D / 100;
        internal const double UnemploymentInsuranceRate = 0.2D / 100;
        internal const double HousingFundRate = 12D / 100;
        internal const double IncomeTaxExemption = 3500D;

        #endregion Rates

        #region Ranges

        internal static Range InsuranceRange = new Range(1869, 14016);
        internal static Range HousingFundRange = new Range(1160, 14016);

        #endregion Ranges

        internal static IncomeRateEntry GetIncomeRateEntry(double taxableAmount)
        {
            return IncomeRateEntry.GetIncomeRateEntry(taxableAmount);
        }
    }
}