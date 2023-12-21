namespace TaxCalculator.Data.Contexts
{
    using Microsoft.EntityFrameworkCore;

    using TaxCalculator.Data.Anemics.TaxCalculations;

    public class PrimaryContext : ContextBase<PrimaryContext>
    {
        #region Constructor

        public PrimaryContext(DbContextOptions<PrimaryContext> options)
            : base(options)
        {
        }

        #endregion Constructor

        #region Properties

        public DbSet<TaxCalculationAnemic> TaxCalculations { get; set; }

        #endregion Properties

        #region Methods 
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        #endregion Methods
    }
}
