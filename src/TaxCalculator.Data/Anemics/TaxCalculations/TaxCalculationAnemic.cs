namespace TaxCalculator.Data.Anemics.TaxCalculations
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table(TableName)]
    public class TaxCalculationAnemic : AnemicBase
    {
        #region Fields

        public const string TableName = "TaxCalculations";

        #endregion Fields

        #region Constructor 

        public TaxCalculationAnemic(
            Guid id,
            string postalCode,
            decimal annualIncome,
            decimal taxAmount)
        {
            this.Id = id;
            this.PostalCode = postalCode;
            this.AnnualIncome = annualIncome;
            this.TaxAmount = taxAmount;
        }

        #endregion Constructor

        #region Properties

        public string PostalCode { get; set; }
        [Precision(18,2)]
        public decimal AnnualIncome { get; set; }
        [Precision(18, 2)]
        public decimal TaxAmount { get; set; }

        #endregion Properties

        #region Methods

        public static TaxCalculationAnemic New(
            string postalCode,
            decimal annualIncome,
            decimal taxAmount)
        {
            return new TaxCalculationAnemic(
                id: Guid.NewGuid(),
                postalCode: postalCode,
                annualIncome: annualIncome,
                taxAmount: taxAmount);

        }

        #endregion Methods

    }
}
