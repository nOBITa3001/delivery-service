using FluentValidation;
using DS.Contracts.Handlers;
using DS.Contracts.OperationResponse;
using DS.Handlers.Abstract;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace DS.Handlers.Strategies.Exceptions
{
    public class HandlerValidationExceptionStrategy<TRequest, TResponse> : HandlerExceptionStrategyBase<TRequest, TResponse>
        where TRequest : class, IHandlerRequest
        where TResponse : class, IHandlerResponse
    {
        private readonly ValidationException _exception;

        public HandlerValidationExceptionStrategy(TRequest request, ILogger logger, ValidationException exception)
            : base(request, logger)
        {
            _exception = exception;
        }

        public override IOperationResponse<TResponse> Handle()
        {
            var errorMessages = _exception.Errors.Select(error => error.ErrorMessage).ToArray();
            LogWarning(_exception, errorMessages);

            return BadRequest(errorMessages);
        }
    }
}
