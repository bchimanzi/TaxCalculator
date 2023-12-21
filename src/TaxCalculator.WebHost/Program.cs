namespace TaxCalculator.WebHost
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;
   
    using Serilog;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = CreateHostBuilder(args);

            builder.UseSerilog((hostingContext, loggerConfiguration) =>
            loggerConfiguration
            .ReadFrom.Configuration(hostingContext.Configuration)
            .Enrich.FromLogContext());

            var host = builder.Build();

            await host.RunAsync();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webHostBuilder => {
                webHostBuilder.UseStartup<Startup>();
            });
    }
}