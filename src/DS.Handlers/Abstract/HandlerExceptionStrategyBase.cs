using DS.Contracts.Handlers;
using DS.Contracts.OperationResponse;
using DS.Handlers.OperationResponse;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace DS.Handlers.Abstract
{
    public abstract class HandlerExceptionStrategyBase<TRequest, TResponse>
        where TRequest : class, IHandlerRequest
        where TResponse : class, IHandlerResponse
    {
        private readonly TRequest _request;
        private readonly ILogger _logger;

        protected HandlerExceptionStrategyBase(TRequest request, ILogger logger)
        {
            _request = request;
            _logger = logger;
        }

        public abstract IOperationResponse<TResponse> Handle();

        protected IOperationResponse<TResponse> BadRequest(params string[] messages) => OperationResponse<TResponse>.BadRequest(messages);

        protected IOperationResponse<TResponse> NotFound(params string[] messages) => OperationResponse<TResponse>.NotFound(messages);

        protected IOperationResponse<TResponse> InternalServerError(params string[] messages) => OperationResponse<TResponse>.InternalServerError(messages);

        protected IOperationResponse<TResponse> ServiceUnavailable(params string[] messages) => OperationResponse<TResponse>.ServiceUnavailable(messages);

        protected virtual void LogWarning(Exception exception, params string[] messages) => _logger.LogWarning(exception, BuildLogMessage(messages));

        protected virtual void LogError(Exception exception, params string[] messages) => _logger.LogError(exception, BuildLogMessage(messages));

        private string BuildLogMessage(params string[] messages)
        {
            var seperator = $"{Environment.NewLine}{Environment.NewLine}";
            var splitMessages = string.Join(seperator, messages);
            var request = JsonConvert.SerializeObject(_request);
            var formattedMessage = string.Join(seperator, splitMessages, request);

            return $"Handler<{typeof(TRequest).Name}, {typeof(TResponse).Name}>: {formattedMessage}";
        }
    }
}
