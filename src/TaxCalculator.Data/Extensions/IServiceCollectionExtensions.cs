namespace Microsoft.Extensions.DependencyInjection
{
    using Microsoft.Extensions.Options;
    using Microsoft.EntityFrameworkCore;

    using TaxCalculator.Data.Contexts;
    using TaxCalculator.Infrastructure;
    using TaxCalculator.Data.Initializers;

    public static class IServiceCollectionExtensions
    {
        public static void AddDataModule(this IServiceCollection serviceCollection)
        {
            var provider = serviceCollection.BuildServiceProvider();

            var databaseSettings = provider.GetService<IOptions<DatabaseSettings>>().Value;

            serviceCollection.AddDbContext<PrimaryContext>(options =>
                options.UseSqlServer(connectionString: databaseSettings.ConnectionString));

            serviceCollection.AddScoped<PrimaryContextInitializer>();
            using (var scope = serviceCollection.BuildServiceProvider().CreateScope())
            { 
                var initializer = scope.ServiceProvider.GetRequiredService<PrimaryContextInitializer>();

                initializer.Seed();
            }
        }
    }
}
