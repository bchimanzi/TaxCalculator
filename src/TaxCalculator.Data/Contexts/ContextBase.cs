namespace TaxCalculator.Data.Contexts
{
    using System;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    
    using TaxCalculator.Data.Anemics;

    public class ContextBase<T> : DbContext where T : DbContext
    {
        #region Constructors 
        
        public ContextBase()
        {

        }

        public ContextBase(DbContextOptions<T> options)
            : base(options)
        {

        }

        #endregion Constructors

        #region Methods

        public override int SaveChanges()
        {
            AddTimeStamps();
            return base.SaveChanges();
        }

        #endregion Methods

        #region Private Methods

        private void AddTimeStamps()
        { 
            foreach (EntityEntry item in from x in ChangeTracker.Entries()
                                         where x.Entity is AnemicBase && (x.State == EntityState.Added || x.State == EntityState.Modified)
                                         select x)
            {
                if (item.State == EntityState.Added)
                {
                    ((AnemicBase)item.Entity).Created = DateTimeOffset.UtcNow;
                }
                else
                { 
                    Entry(item.Entity).Property("Created").IsModified = false;                   
                }

                //if there is modified or updated date, it can be set here

            }
        }

        #endregion Private Methods


    }

}
