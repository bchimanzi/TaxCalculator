namespace TaxCalculator.BaseModule.Handlers.TaxCalculator
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    
    using Innovation.Api.Commanding;
    using Innovation.Api.CommandHelpers;
    
    using Api.Enums;
    using Data.Contexts;
    using CalculationModule;
    using Api.TaxCalculator.Commands;
    using Data.Anemics.TaxCalculations;

    public class TaxCalculationCommandPersistor : ICommandHandler<TaxCalculationCommand>
    {
        private readonly CalculatorEngine calculatorEngine;
        private readonly PrimaryContext context;
        private readonly ILogger logger;

        public TaxCalculationCommandPersistor(
            ILogger<TaxCalculationCommandPersistor> logger,
            CalculatorEngine calculatorEngine,
            PrimaryContext context)
        {
            this.context = context;
            this.calculatorEngine = calculatorEngine;
            this.logger = logger;
        }

        public async Task<ICommandResult> Handle(TaxCalculationCommand command)
        {
            return await this.Persist(command: command);
        }

        private async Task<ICommandResult> Persist(TaxCalculationCommand command)
        {
            var commandResult = new CommandResult();

            try
            {
                if (command.Criteria == null)
                {
                    commandResult.Fail(errorMessage: "Criteria cannot be null");
                    return commandResult;   
                }
                var calculationType = GetCalculationType(command.Criteria.PostalCode);

                var calculatedTax = this.calculatorEngine.CalculateTax(calculationType: calculationType, annualIncome: command.Criteria.AnnualIncome );

                var taxCalculationAnemic = TaxCalculationAnemic.New(
                    postalCode: command.Criteria.PostalCode,
                    annualIncome: command.Criteria.AnnualIncome,
                    taxAmount: calculatedTax);

                this.context.TaxCalculations.Add(entity: taxCalculationAnemic);

                this.context.SaveChanges();

                commandResult.SetRecord(taxCalculationAnemic.Id);

               return commandResult;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.GetInnerMostMessage());

                commandResult.Fail(errorMessage: "Failed to save calculated results");

                return commandResult;
            }
        }

        private static CalculationType GetCalculationType(string postalCode)
        {
            switch (postalCode)
            {
                case "7441":
                case "1000":
                    {
                        return CalculationType.Progressive;
                    }
                case "A100":
                    {
                        return CalculationType.FlatValue;
                    }
                case "7000":
                    {
                        return CalculationType.FlatRate;
                    }
                default:
                    {
                        return CalculationType.UnKnown;
                    }
            }
        }
    }
}
