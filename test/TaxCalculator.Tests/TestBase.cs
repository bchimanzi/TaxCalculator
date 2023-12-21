namespace TaxCalculator.Tests
{
    using System;
    using System.IO;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TestBase
    {
        private readonly IConfiguration configuration;
        private readonly IServiceProvider serviceProvider;

        public TestBase()
        {
            var serviceCollection = new ServiceCollection();

            this.configuration = this.BuildConfiguration(serviceCollection: serviceCollection);
            this.serviceProvider = this.ConfigureServices(serviceCollection: serviceCollection);
        }

        private IConfigurationRoot BuildConfiguration(ServiceCollection serviceCollection) 
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            serviceCollection.AddSingleton<IConfiguration>(configuration);

            return configuration;
        }

        private IServiceProvider ConfigureServices(ServiceCollection serviceCollection)
        { 
            return serviceCollection.BuildServiceProvider();
        }
    }
}
