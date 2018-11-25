using DS.Contracts.DataAccess.Repositories;
using DS.Contracts.OperationResponse;
using DS.DomainModel.Entities;
using DS.DomainModel.Factories;
using DS.Dtos.Exceptions;
using DS.Dtos.ResponseMessages;
using DS.Dtos.Routes;
using DS.Handlers.Abstract;
using DS.Handlers.Requests;
using DS.Handlers.Responses;
using DS.Handlers.Strategies.Factories;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DS.Handlers
{
    public class GetDeliveryCostHandler : HandlerBase<GetDeliveryCostHandlerRequest, GetDeliveryCostHandlerResponse>
    {
        private readonly IDeliveryRouteReadOnlyRepository _deliveryRouteReadOnlyRepository;
        private readonly IRouteFactory _routeFactory;

        public GetDeliveryCostHandler(
            IDeliveryRouteReadOnlyRepository deliveryRouteReadOnlyRepository,
            IRouteFactory routeFactory,
            ILogger<GetDeliveryCostHandler> logger,
            IValidator<GetDeliveryCostHandlerRequest> validator,
            IHandlerExceptionStrategyFactory handlerExceptionStrategyFactory)
            : base(logger, validator, handlerExceptionStrategyFactory)
        {
            _deliveryRouteReadOnlyRepository = deliveryRouteReadOnlyRepository;
            _routeFactory = routeFactory;
        }

        protected override async Task<IOperationResponse<GetDeliveryCostHandlerResponse>> HandleAsync(GetDeliveryCostHandlerRequest request)
        {
            var deliveryRoutes = await _deliveryRouteReadOnlyRepository.GetDictionaryAsync();
            if (deliveryRoutes == null || !deliveryRoutes.Any())
                throw new DataAccessNotFoundException(ResponseMessages.Route.DoesNotExist);

            var routes = BuildRoutes(request.Route);
            var deliveryCost = CalculateDeliveryCost(routes, deliveryRoutes);

            return Success(new GetDeliveryCostHandlerResponse(deliveryCost));
        }

        #region Private Methods
   
        private Route[] BuildRoutes(string route)
        {
            var result = new List<Route>();

            var routes = route.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < routes.Length - 1; i++)
            {
                var dto = new CreateRouteDto
                {
                    Start = routes[i].ToUpper(),
                    End = routes[i + 1].ToUpper()
                };

                result.Add(_routeFactory.Create(dto));
            }

            return result.ToArray();
        }

        private int? CalculateDeliveryCost(Route[] routes, Dictionary<string, DeliveryRoute[]> deliveryRoutes)
        {
            var result = default(int?);

            for (var i = 0; i < routes.Count(); i++)
            {
                var deliveryRoutesOfStart = GetDeliveryRoutesOfStartingPoint(deliveryRoutes, routes, i);

                var nestedRoute = GetNestedDeliveryRoute(routes, i, deliveryRoutesOfStart);
                if (nestedRoute == null)
                    throw new DataAccessNotFoundException(ResponseMessages.Route.DoesNotExist);

                result = (result ?? 0) + nestedRoute.Cost;
            }

            return result;
        }

        private DeliveryRoute[] GetDeliveryRoutesOfStartingPoint(Dictionary<string, DeliveryRoute[]> deliveryRoutes, Route[] routes, int index)
        {
            if (deliveryRoutes.TryGetValue(routes[index].Start, out var deliveryRoutesOfStart))
            {
                return deliveryRoutesOfStart;
            }

            throw new DataAccessNotFoundException(ResponseMessages.Route.DoesNotExist);
        }

        private DeliveryRoute GetNestedDeliveryRoute(Route[] routes, int i, DeliveryRoute[] deliveryRoutesOfStart)
        {
            return deliveryRoutesOfStart.FirstOrDefault(route => (route.End?.Equals(routes[i].End, StringComparison.OrdinalIgnoreCase) ?? false));
        }

        #endregion
    }
}
