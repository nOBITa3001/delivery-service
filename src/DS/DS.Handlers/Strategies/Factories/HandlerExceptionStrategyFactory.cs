using DS.Contracts.Handlers;
using DS.Dtos.Exceptions;
using DS.Handlers.Abstract;
using DS.Handlers.Strategies.Exceptions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System;

namespace DS.Handlers.Strategies.Factories
{
    public class HandlerExceptionStrategyFactory : IHandlerExceptionStrategyFactory
    {
        public HandlerExceptionStrategyBase<TRequest, TResponse> Create<TRequest, TResponse>(TRequest request, ILogger logger, Exception exception)
            where TRequest : class, IHandlerRequest
            where TResponse : class, IHandlerResponse
        {
            switch (exception)
            {
                case DomainModelException ex:
                    return new HandlerDomainModelExceptionStrategy<TRequest, TResponse>(request, logger, ex);
                case DataAccessNotFoundException ex:
                    return new HandlerDataAccessNotFoundExceptionStrategy<TRequest, TResponse>(request, logger, ex);
                case InvalidArgumentException ex:
                    return new HandlerInvalidArgumentExceptionStrategy<TRequest, TResponse>(request, logger, ex);
                case ValidationException ex:
                    return new HandlerValidationExceptionStrategy<TRequest, TResponse>(request, logger, ex);
                default:
                    return new HandlerExceptionStrategy<TRequest, TResponse>(request, logger, exception);
                case null:
                    throw new ArgumentNullException(nameof(exception));
            }
        }
    }
}
