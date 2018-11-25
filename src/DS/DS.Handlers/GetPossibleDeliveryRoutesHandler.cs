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
    public class GetPossibleDeliveryRoutesHandler : HandlerBase<GetPossibleDeliveryRoutesHandlerRequest, GetPossibleDeliveryRoutesHandlerResponse>
    {
        private int possibleRoute = 0;

        private readonly IDeliveryRouteReadOnlyRepository _deliveryRouteReadOnlyRepository;
        private readonly IRouteFactory _routeFactory;

        public GetPossibleDeliveryRoutesHandler(
            IDeliveryRouteReadOnlyRepository deliveryRouteReadOnlyRepository,
            IRouteFactory routeFactory,
            ILogger<GetPossibleDeliveryRoutesHandler> logger,
            IValidator<GetPossibleDeliveryRoutesHandlerRequest> validator,
            IHandlerExceptionStrategyFactory handlerExceptionStrategyFactory)
            : base(logger, validator, handlerExceptionStrategyFactory)
        {
            _deliveryRouteReadOnlyRepository = deliveryRouteReadOnlyRepository;
            _routeFactory = routeFactory;
        }

        protected override async Task<IOperationResponse<GetPossibleDeliveryRoutesHandlerResponse>> HandleAsync(GetPossibleDeliveryRoutesHandlerRequest request)
        {
            var deliveryRoutes = await _deliveryRouteReadOnlyRepository.GetAllAsync();
            if (deliveryRoutes == null || !deliveryRoutes.Any())
                throw new DataAccessNotFoundException(ResponseMessages.Route.DoesNotExist);

            var routes = BuildRoute(request.Route);
            var possibleRoute = CalculatePossibleDeliveryRoute(routes, request.MaxRouteRepeat, request.MaxDeliveryCost, request.MaxStop, deliveryRoutes);

            return Success(new GetPossibleDeliveryRoutesHandlerResponse(possibleRoute));
        }

        #region Private Methods

        private Route BuildRoute(string route)
        {
            var routes = route.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            var dto = new CreateRouteDto
            {
                Start = routes[0].ToUpper(),
                End = routes[1].ToUpper()
            };

            return _routeFactory.Create(dto);
        }

        private int CalculatePossibleDeliveryRoute(Route route, int maxRouteRepeat, int? maxDeliveryCost, int? maxStop, IEnumerable<DeliveryRoute> deliveryRoutes)
        {
            var path = string.Empty;
            var visited = new Dictionary<string, int>();

            var deliveryRoutesOfStart = deliveryRoutes.Where(deliveryRoute => deliveryRoute.Start.Equals(route.Start, StringComparison.OrdinalIgnoreCase));
            foreach (var deliveryRoute in deliveryRoutesOfStart)
            {
                FindRoutes
                (
                    coveredRoute: path,
                    visited: ref visited,
                    route: deliveryRoute,
                    end: route.End,
                    maxStop: (maxStop ?? int.MaxValue),
                    currentStop: 1,
                    aggregateDeliveryCost: deliveryRoute.Cost,
                    currentCost: 0,
                    maxDeliveryCost: (maxDeliveryCost ?? int.MaxValue),
                    maxRouteRepeat: maxRouteRepeat,
                    allDeliveryRoutes: deliveryRoutes
                );
            }

            return possibleRoute;
        }

        private void FindRoutes(string coveredRoute, ref Dictionary<string, int> visited, DeliveryRoute route, string end, int maxStop, int currentStop,
            int aggregateDeliveryCost, int currentCost, int maxDeliveryCost, int maxRouteRepeat, IEnumerable<DeliveryRoute> allDeliveryRoutes)
        {
            aggregateDeliveryCost = UpdateAggregateDeliveryCost(aggregateDeliveryCost, currentCost);

            if (CanRepeatRoute(maxRouteRepeat))
                maxRouteRepeat = UpdateMaxRouteRepeat(maxRouteRepeat);

            if (!string.IsNullOrWhiteSpace(coveredRoute))
            {
                UpdateVisited(coveredRoute, ref visited);

                if (ExceedMaxRouteRepeat(coveredRoute, visited, maxRouteRepeat))
                    return;
            }

            if (ExceedMaxDeliveryCost(aggregateDeliveryCost, maxDeliveryCost))
                return;

            if (route.End.Equals(end, StringComparison.OrdinalIgnoreCase))
            {
                IncreasePossibleDeliveryRoute();

                if (AbleToRepeatTheSameRoute(maxRouteRepeat))
                    return;
            }

            if (ExceedMaxStop(maxStop, currentStop))
                return;

            currentStop = UpdateCurrentStop(currentStop);

            var deliveryRoutes = allDeliveryRoutes.Where(deliveryRoute => deliveryRoute.Start.Equals(route.End, StringComparison.OrdinalIgnoreCase));
            foreach (var deliveryRoute in deliveryRoutes)
            {
                coveredRoute = UpdateCoveredRoute(route.End, deliveryRoute);

                FindRoutes
                (
                    coveredRoute: coveredRoute,
                    visited: ref visited,
                    route: deliveryRoute,
                    end: end,
                    maxStop: maxStop,
                    currentStop: currentStop,
                    aggregateDeliveryCost: aggregateDeliveryCost,
                    currentCost: deliveryRoute.Cost,
                    maxDeliveryCost: maxDeliveryCost,
                    maxRouteRepeat: maxRouteRepeat,
                    allDeliveryRoutes: allDeliveryRoutes
                );

                UnMarkVisited(coveredRoute, visited);
            }
        }

        private int UpdateAggregateDeliveryCost(int aggregateDeliveryCost, int currentCost) => aggregateDeliveryCost + currentCost;

        private bool CanRepeatRoute(int maxRouteRepeat) => maxRouteRepeat > 1;

        private int UpdateMaxRouteRepeat(int maxRouteRepeat) => ++maxRouteRepeat;

        private void UpdateVisited(string coveredRoute, ref Dictionary<string, int> visited)
        {
            if (visited.TryGetValue(coveredRoute, out var count))
                visited[coveredRoute]++;
            else
                visited.Add(coveredRoute, 1);
        }

        private bool ExceedMaxRouteRepeat(string coveredRoute, Dictionary<string, int> visited, int maxRouteRepeat) => visited[coveredRoute] > maxRouteRepeat;

        private bool ExceedMaxDeliveryCost(int aggregateDeliveryCost, int maxDeliveryCost) => aggregateDeliveryCost >= maxDeliveryCost;

        private void IncreasePossibleDeliveryRoute() => ++possibleRoute;

        private bool AbleToRepeatTheSameRoute(int maxRouteRepeat) => maxRouteRepeat < 2;

        private bool ExceedMaxStop(int maxStop, int currentStop) => currentStop >= maxStop;

        private int UpdateCurrentStop(int currentStop) => ++currentStop;

        private string UpdateCoveredRoute(string start, DeliveryRoute deliveryRoute) => start + deliveryRoute.End;

        private void UnMarkVisited(string coveredRoute, Dictionary<string, int> visited)
        {
            visited[coveredRoute]--;

            if (visited[coveredRoute] < 0)
                visited[coveredRoute] = 0;
        }

        #endregion
    }
}
