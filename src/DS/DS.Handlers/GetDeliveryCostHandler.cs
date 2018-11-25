using DS.Contracts.DataAccess.Repositories;
using DS.Contracts.OperationResponse;
using DS.Dtos.Exceptions;
using DS.Dtos.ResponseMessages;
using DS.Handlers.Abstract;
using DS.Handlers.Requests;
using DS.Handlers.Responses;
using DS.Handlers.Strategies.Factories;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DS.Handlers
{
    public class GetDeliveryCostHandler : HandlerBase<GetDeliveryCostHandlerRequest, GetDeliveryCostHandlerResponse>
    {
        private readonly IDeliveryRouteReadOnlyRepository _deliveryRouteReadOnlyRepository;

        public GetDeliveryCostHandler(
            IDeliveryRouteReadOnlyRepository deliveryRouteReadOnlyRepository,
            ILogger<GetDeliveryCostHandler> logger,
            IValidator<GetDeliveryCostHandlerRequest> validator,
            IHandlerExceptionStrategyFactory handlerExceptionStrategyFactory)
            : base(logger, validator, handlerExceptionStrategyFactory)
        {
            _deliveryRouteReadOnlyRepository = deliveryRouteReadOnlyRepository;
        }

        protected override async Task<IOperationResponse<GetDeliveryCostHandlerResponse>> HandleAsync(GetDeliveryCostHandlerRequest request)
        {
            var deliveryRoutes = await _deliveryRouteReadOnlyRepository.GetAllAsync();

            throw new DataAccessNotFoundException(ResponseMessages.Route.DoesNotExist);
        }
    }
}
