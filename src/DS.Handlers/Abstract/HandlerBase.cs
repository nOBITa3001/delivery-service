using DS.Contracts.Handlers;
using DS.Contracts.OperationResponse;
using DS.Dtos.ResponseMessages;
using DS.Handlers.OperationResponse;
using DS.Handlers.Strategies.Factories;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DS.Handlers.Abstract
{
    public abstract class HandlerBase<TRequest, TResponse>
        where TRequest : class, IHandlerRequest
        where TResponse : class, IHandlerResponse
    {
        private readonly ILogger _logger;
        private readonly IValidator<TRequest> _validator;
        private readonly IHandlerExceptionStrategyFactory _handlerExceptionStrategyFactory;

        protected HandlerBase(ILogger logger, IValidator<TRequest> validator, IHandlerExceptionStrategyFactory handlerExceptionStrategyFactory)
        {
            _logger = logger;
            _validator = validator;
            _handlerExceptionStrategyFactory = handlerExceptionStrategyFactory;
        }

        public async Task<IOperationResponse<TResponse>> ExecuteHandlerAsync(TRequest request)
        {
            if (request == null)
                return BadRequest(ResponseMessages.Handler.NullRequest);

            try
            {
                _validator.ValidateAndThrow(request);
                return await HandleAsync(request);
            }
            catch (Exception ex)
            {
                var handlerExceptionStrategy = _handlerExceptionStrategyFactory.Create<TRequest, TResponse>(request, _logger, ex);
                return handlerExceptionStrategy.Handle();
            }
        }

        protected abstract Task<IOperationResponse<TResponse>> HandleAsync(TRequest request);

        protected IOperationResponse<TResponse> Success(TResponse payload) => OperationResponse<TResponse>.Success(payload);

        private IOperationResponse<TResponse> BadRequest(params string[] messages) => OperationResponse<TResponse>.BadRequest(messages);
    }
}
