namespace TaxCalculator.Data.Anemics
{
    using System;

    public interface IAnemicBase
    {
        Guid Id { get; set; }
        DateTimeOffset Created { get; set; }
    }
}
