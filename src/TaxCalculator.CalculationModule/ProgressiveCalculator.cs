namespace TaxCalculator.CalculationModule
{
    using System;
    using Microsoft.Extensions.Logging;
    
    using TaxCalculator.Api.Enums;

    public class ProgressiveCalculator : BaseCalculator
    {
        private readonly ILogger logger;
        public ProgressiveCalculator(ILogger<ProgressiveCalculator> logger) : base(logger) 
        {
            this.logger = logger;
        }

        public override CalculationType CalculationType => CalculationType.Progressive;

        public override decimal PerformCalculation(decimal annualIncome)
        {

            decimal[] taxBrackets = { 8350m, 33950m, 82250m, 171550m, 372950m, decimal.MaxValue };
            decimal[] taxRates = { 0.1m, 0.15m, 0.25m, 0.28m, 0.33m, 0.35m };

            decimal totalTax = 0m;
            decimal remainingIncome = annualIncome;

            for (int i = 0; i < taxBrackets.Length; i++)
            {
                if (remainingIncome <= 0) 
                {
                    break;
                }

                var taxRate = taxRates[i];
                var taxBracket = taxBrackets[i];

                var previousBracket = i == 0 ? 0 : taxBrackets[i - 1];

                var minimumValueRemaining = taxBracket - previousBracket;
                minimumValueRemaining = remainingIncome == 0 ? minimumValueRemaining : Math.Min(remainingIncome, minimumValueRemaining);
                
                decimal taxableAmount = Math.Min(minimumValueRemaining, taxBracket);

                totalTax += taxableAmount * taxRate;

                remainingIncome -= taxableAmount;

            }

            return totalTax;           

        }
    }
}
