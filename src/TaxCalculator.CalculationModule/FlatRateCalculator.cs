namespace TaxCalculator.CalculationModule
{
    using Microsoft.Extensions.Logging;

    using TaxCalculator.Api.Enums;

    public class FlatRateCalculator : BaseCalculator
    {
        private readonly ILogger logger;
        public FlatRateCalculator(ILogger<FlatRateCalculator> logger) : base(logger)
        {
            this.logger = logger;
        }

        public override CalculationType CalculationType => CalculationType.FlatRate;

        public override decimal PerformCalculation(decimal annualIncome)
        {
            return annualIncome * 0.175m;
        }
    }
}
