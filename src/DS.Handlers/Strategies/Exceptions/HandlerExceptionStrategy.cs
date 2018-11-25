using DS.Contracts.Handlers;
using DS.Contracts.OperationResponse;
using DS.Dtos.ResponseMessages;
using DS.Handlers.Abstract;
using Microsoft.Extensions.Logging;
using System;

namespace DS.Handlers.Strategies.Exceptions
{
    public class HandlerExceptionStrategy<TRequest, TResponse> : HandlerExceptionStrategyBase<TRequest, TResponse>
        where TRequest : class, IHandlerRequest
        where TResponse : class, IHandlerResponse
    {
        private readonly Exception _exception;

        public HandlerExceptionStrategy(TRequest request, ILogger logger, Exception exception)
            : base(request, logger)
        {
            _exception = exception;
        }

        public override IOperationResponse<TResponse> Handle()
        {
            LogError(_exception, new[] { _exception.Message });
            return InternalServerError(ResponseMessages.Handler.InternalServerError);
        }
    }
}
