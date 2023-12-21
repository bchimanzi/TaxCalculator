namespace TaxCalculator.CalculationModule
{
    using Microsoft.Extensions.Logging;

    using TaxCalculator.Api.Enums;

    public class FlatValueCalculator : BaseCalculator
    {
        private readonly ILogger logger;
        public FlatValueCalculator(ILogger<FlatValueCalculator> logger) : base(logger) 
        {
            this.logger = logger;
        }

        public override CalculationType CalculationType => CalculationType.FlatValue;

        public override decimal PerformCalculation(decimal annualIncome)
        {
            if (annualIncome < 200000m)
            { 
                return annualIncome * 0.05m;
            }

            return 10000m;
        }
    }
}
