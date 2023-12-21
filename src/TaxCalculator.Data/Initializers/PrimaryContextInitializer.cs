namespace TaxCalculator.Data.Initializers
{
    using System;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using TaxCalculator.Data.Contexts;

    public class PrimaryContextInitializer
    {
        private readonly ILogger logger;
        private readonly PrimaryContext context;

        public PrimaryContextInitializer(ILogger<PrimaryContextInitializer> logger, PrimaryContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public void Seed()
        {
            try
            {
                var pendingMigrations = this.context.Database.GetPendingMigrations();
                if (pendingMigrations.Count() > 0)
                {
                    this.context.Database.Migrate();
                }
            }
            catch (Exception ex)
            {

                this.logger.LogError(ex, ex.Message);
            }
        }
    }
}
