using DS.Contracts.Handlers;
using DS.Contracts.OperationResponse;
using DS.Handlers.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DS.API.Controllers
{
    public abstract class DSControllerBase : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;

        protected DSControllerBase(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// The only access point to all handlers in the DS.Handlers
        /// </summary>
        protected async Task<ObjectResult> CallHandlerAsync<THandlerRequest, THandlerResponse>(THandlerRequest request)
            where THandlerRequest : class, IHandlerRequest
            where THandlerResponse : class, IHandlerResponse
        {
            var handler = _serviceProvider.GetService<HandlerBase<THandlerRequest, THandlerResponse>>();
            var response = await handler.ExecuteHandlerAsync(request);

            return BuildObjectResult(response);
        }

        private ObjectResult BuildObjectResult<THandlerResponse>(IOperationResponse<THandlerResponse> response)
            where THandlerResponse : class, IHandlerResponse
        {
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode };
        }
    }
}