namespace TaxCalculator.CalculationModule
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.DependencyInjection;

    using TaxCalculator.Api.Enums;

    public class CalculatorEngine
    {
        private readonly ILogger logger;
        private readonly IEnumerable<BaseCalculator> baseCalculators;
        private readonly IServiceProvider serviceProvider;
        public CalculatorEngine(
             ILogger<CalculatorEngine> logger,
             IEnumerable<BaseCalculator> baseCalculators,
             IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this.baseCalculators = baseCalculators;
            this.serviceProvider = serviceProvider;
        }

        public decimal CalculateTax(CalculationType calculationType, decimal annualIncome)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var calculator = this.baseCalculators.First(x => x.CalculationType.Equals(calculationType));

                return calculator.PerformCalculation(annualIncome);
            }
        }
    }
}
