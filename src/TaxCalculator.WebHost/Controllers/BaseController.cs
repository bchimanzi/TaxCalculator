namespace TaxCalculator.WebHost.Controllers
{
    using Innovation.Api.CommandHelpers;
    using Innovation.Api.Commanding;
    using Innovation.Api.Dispatching;
    using Innovation.Api.Querying;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using System.Threading.Tasks;
    using System;
    using Microsoft.Extensions.DependencyInjection;

    [Route("api/[controller)")]
    public class BaseController : ControllerBase
    {
        #region Properties
        public IDispatcher Dispatcher => this.GetDispatcher();
        public IHostEnvironment HostingEnvironment => this.GetHostingEnvironment();
        public ILogger Logger => this.GetLogger();

        #endregion Properties

        #region Methods

        protected async Task<ObjectResult> Query<TQuery, TQueryResult>(TQuery query) where TQuery : IQuery
        where TQueryResult : IQueryResult
        {
            var queryResult = await this.Dispatcher.Query<TQuery, TQueryResult>(query);
            return this.Ok(value: queryResult);
        }

        protected async Task<ObjectResult> Query<TQuery, TQueryResult>(TQuery query, Guid id) where TQuery : IQuery
        where TQueryResult : IQueryResult
        {
            var queryResult = await this.Dispatcher.Query<TQuery, TQueryResult>(query);
            return this.GenerateReturnResult(queryResult: queryResult, id: id);
        }

        protected async Task<ObjectResult> Command<TCommand>(TCommand command, bool suppressExceptions = true) where TCommand : ICommand
        {
            var commandResult = await this.Dispatcher.Command(command: command, suppressExceptions: suppressExceptions);
            return this.GenerateReturnResult(commandResult: commandResult);
        }
        // Command => Dispatcher =>
        protected ObjectResult GenerateReturnResult(IQueryResult queryResult, Guid id)
        {


            if (queryResult == null)
            {
                return this.NotFound(value: $"Resource with identity '{id}' not found");
            }
            else
            {
                return this.Ok(value: queryResult);
            }
        }

        protected ObjectResult GenerateReturnResult(Exception ex, bool logAsWarning = false)
        {
            if (logAsWarning)
            {
                this.GetLogger().LogWarning(exception: ex, message: ex.GetInnerMostMessage());
            }
            else
            {

                this.GetLogger().LogError(exception: ex, message: ex.GetInnerMostMessage());
            }

            var finalMessage = this.HostingEnvironment.IsProduction() ? "An Exception Occurred" : ex.Message;

            var commandResult = new CommandResult(success: false, errors: new[] { finalMessage });

            return this.BadRequest(error: commandResult);

        }

        protected ObjectResult GenerateReturnResult(ICommandResult commandResult)
        {
            if (!commandResult.Success)
            {
                return this.BadRequest(error: commandResult);
            }
            else
            {
                return this.Ok(value: commandResult);
            }
        }

        protected ObjectResult HandleModelStateErrors()
        {
            if (this.ModelState.IsValid)
            {
                throw new InvalidOperationException(message: "No Model Errors Found");
            }
            var commandResult = new CommandResult();

            foreach (var propertyErrorPair in this.ModelState)
            {
                foreach (var error in propertyErrorPair.Value.Errors)
                {
                    commandResult.Fail(errorMessage: $" {error.ErrorMessage} On Property '{propertyErrorPair.Key}");
                }
            }
            return this.BadRequest(error: commandResult);
        }


        protected IHeaderDictionary GetHeaders()
        {
            return this.HttpContext.Request.Headers;
        }

        protected bool HeaderExists(string key)
        {
            return this.HttpContext.Request.Headers.ContainsKey(key);
        }
        protected string GetHeader(string key)
        {
            var headerValue = string.Empty;
            if (this.HttpContext == null || this.HttpContext.Request == null || this.HttpContext.Request.Headers == null || string.IsNullOrWhiteSpace(value: key))
            {
                return headerValue;
            }
            if (this.HttpContext.Request.Headers.ContainsKey(key: key))
            {
                return this.HttpContext.Request.Headers[key];
            }
            return headerValue;
        }
        #endregion Methods

        #region PrivateMethods

        private IDispatcher GetDispatcher()
        {
            return this.HttpContext.RequestServices.GetRequiredService<IDispatcher>();
        }

        private IHostEnvironment GetHostingEnvironment()
        {
            return this.HttpContext.RequestServices.GetRequiredService<IHostEnvironment>();
        }
        private ILogger GetLogger()
        {
            return this.HttpContext.RequestServices.GetRequiredService<ILogger<BaseController>>();
        }

        #endregion PrivateMethods
    }
}
