namespace TaxCalculator.WebHost.Controllers
{
    using Innovation.Api.CommandHelpers;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;
    using TaxCalculator.Api.TaxCalculator.Commands;
    using TaxCalculator.Api.TaxCalculator.Criteria;

    [ApiController]
    [Route("api/[controller]")]
    public class TaxCalculatorController : BaseController
    {

        [HttpPost("calculate")]
        [ProducesResponseType(typeof(CommandResult), 200)]
        [ProducesResponseType(typeof(CommandResult), 400)]
        public async Task<IActionResult> InsertTax([FromBody] TaxCalculationCriteria taxCalculationCriteria)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    return this.HandleModelStateErrors();
                }

                var taxCalculatorCommand = new TaxCalculationCommand(taxCalculationCriteria: taxCalculationCriteria);

                var result = await this.Command(command: taxCalculatorCommand);

                return result;
            }
            catch (Exception ex)
            {
                return this.GenerateReturnResult(ex: ex);
            }
        }
    }
}
