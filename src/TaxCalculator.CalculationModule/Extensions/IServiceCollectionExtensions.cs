namespace Microsoft.Extensions.DependencyInjection
{
    using TaxCalculator.CalculationModule;

    public static class IServiceCollectionExtensions
    {
        public static void AddCalculatorModule(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<CalculatorEngine>();
            serviceCollection.AddTransient<BaseCalculator, FlatRateCalculator>();
            serviceCollection.AddTransient<BaseCalculator, FlatValueCalculator>();
            serviceCollection.AddTransient<BaseCalculator, ProgressiveCalculator>();
        }
    }
}
