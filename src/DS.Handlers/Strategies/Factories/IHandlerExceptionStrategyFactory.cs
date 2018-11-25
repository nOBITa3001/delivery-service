using DS.Contracts.Handlers;
using DS.Handlers.Abstract;
using Microsoft.Extensions.Logging;
using System;

namespace DS.Handlers.Strategies.Factories
{
    public interface IHandlerExceptionStrategyFactory
    {
        HandlerExceptionStrategyBase<TRequest, TResponse> Create<TRequest, TResponse>(TRequest request, ILogger logger, Exception exception)
            where TRequest : class, IHandlerRequest
            where TResponse : class, IHandlerResponse;
    }
}
