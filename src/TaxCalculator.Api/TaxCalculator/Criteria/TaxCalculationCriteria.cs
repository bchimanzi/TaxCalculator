namespace TaxCalculator.Api.TaxCalculator.Criteria
{

    public class TaxCalculationCriteria
    {
        #region Constructor

        public TaxCalculationCriteria(string postalCode, decimal annualIncome)//, decimal taxAmount)
        {
            this.PostalCode = postalCode;
            this.AnnualIncome = annualIncome;
            //this.TaxAmount = taxAmount;
        }

        #endregion Constructor


        #region Properties

        public string PostalCode { get; set; }
        public decimal AnnualIncome { get; set; }
        //public decimal TaxAmount { get; set; }

        #endregion Properties
    }
}
