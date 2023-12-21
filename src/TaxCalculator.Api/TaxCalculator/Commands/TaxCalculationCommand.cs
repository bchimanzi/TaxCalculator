namespace TaxCalculator.Api.TaxCalculator.Commands
{
    using Innovation.Api.Commanding;

    using Criteria;
    public class TaxCalculationCommand : ICommand
    {
        public TaxCalculationCommand(TaxCalculationCriteria taxCalculationCriteria)
        {
            this.Criteria = taxCalculationCriteria;
        }

        public TaxCalculationCriteria Criteria { get; }
        public string EventName => nameof(TaxCalculationCommand);
    }
}
