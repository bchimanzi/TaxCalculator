namespace TaxCalculator.CalculationModule
{
    using Microsoft.Extensions.Logging;
    using TaxCalculator.Api.Enums;

    public abstract class BaseCalculator
    {
        private readonly ILogger<BaseCalculator> logger;
        public BaseCalculator(ILogger<BaseCalculator> logger)
        {
             this.logger = logger;
        }

        public abstract CalculationType CalculationType { get; }

        public abstract decimal PerformCalculation(decimal annualIncome);
    }
}