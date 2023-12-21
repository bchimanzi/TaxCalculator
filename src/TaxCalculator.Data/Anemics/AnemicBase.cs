namespace TaxCalculator.Data.Anemics
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class AnemicBase : IAnemicBase
    {
        [Key]
        public Guid Id { get; set; }
        public DateTimeOffset Created { get; set; }
    }
}
