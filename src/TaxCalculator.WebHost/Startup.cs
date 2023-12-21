namespace TaxCalculator.WebHost
{
    using System;
    using System.IO;
    using Microsoft.OpenApi.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Hosting;
    using Microsoft.AspNetCore.Builder;
    using System.Text.Json.Serialization;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    
    using Newtonsoft.Json;
    using Innovation.Api.CommandHelpers;
    
    using TaxCalculator.Infrastructure;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }
        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.Configure<DatabaseSettings>(this.Configuration.GetSection("DatabaseSettings"));

            serviceCollection.AddDataModule();
            serviceCollection.AddBaseModule();
            serviceCollection.AddInnovation();
            serviceCollection.AddCalculatorModule();

            serviceCollection.AddControllersWithViews()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            serviceCollection.AddEndpointsApiExplorer();
            serviceCollection.AddSwaggerGen(swaggerGenOptions =>
            {
                var applicationVersion = System.Reflection.Assembly.GetEntryAssembly()?.GetName().Version.ToString();
                var basePath = AppContext.BaseDirectory;
                var apiXmlPath = Path.Combine(basePath, "TaxCalculator.Api.xml");

                swaggerGenOptions.IncludeXmlComments(apiXmlPath);
                swaggerGenOptions.SwaggerDoc(name: applicationVersion, info: new OpenApiInfo
                {
                    Title = "TaxCalculator",
                    Version = applicationVersion,
                    Contact = new OpenApiContact
                    {
                        Url = new Uri(uriString: "https://taxcalculator.test/"),
                        Name = "TaxCalculator"
                    }
                });
            });
        }

        public void Configure(IApplicationBuilder applicationBuilder, IHostEnvironment hostEnvironment)
        {
            if (hostEnvironment.IsDevelopment())
            {
                applicationBuilder.UseDeveloperExceptionPage();
            }
            else
            {
                applicationBuilder.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        var result = new CommandResult();
                        try
                        {
                            result.Fail("An error occured.");
                            context.Response.StatusCode = 400;
                            context.Response.ContentType = "application/json";
                            await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
                        }
                        catch (Exception)
                        {
                            context.Response.StatusCode = 400;
                            context.Response.ContentType = "text/html";
                            await context.Response.WriteAsync("An error occured");
                        }
                    });
                });
                applicationBuilder.UseHsts();
            }

            if (!hostEnvironment.IsProduction())
            {
                applicationBuilder.UseSwagger();

                applicationBuilder.UseSwaggerUI(c =>
                {
                    var applicationVersion = System.Reflection.Assembly.GetEntryAssembly()?.GetName().Version.ToString();

                    c.SwaggerEndpoint($"/swagger/{applicationVersion}/swagger.json", "TaxCalculator Api");
                });
            }

            applicationBuilder.UseHttpsRedirection();

            applicationBuilder.UseStaticFiles();
            applicationBuilder.UseRouting();
            applicationBuilder.UseCors();

            applicationBuilder.UseAuthentication();
            applicationBuilder.UseAuthorization();

            applicationBuilder.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=index}/{id?}"
                    );
            });
        }
    }
}
