using DS.Contracts.DataAccess.Repositories;
using DS.Dtos.ResponseMessages;
using DS.Handlers.Requests;
using DS.Handlers.Responses;
using DS.Handlers.Strategies.Factories;
using DS.Handlers.Validators;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Net;
using Xunit;

namespace DS.Handlers.UnitTests
{
    public class GetDeliveryCostHandlerTests : HandlerTestsBase<GetDeliveryCostHandler, GetDeliveryCostHandlerRequest, GetDeliveryCostHandlerResponse>
    {
        private const string _validRoute = "A-B-E";
        private const string _routeShouldNotBeEmpty = "'Route' should not be empty.";

        private readonly IDeliveryRouteReadOnlyRepository _deliveryRouteReadOnlyRepository;

        public GetDeliveryCostHandlerTests()
        {
            _deliveryRouteReadOnlyRepository = Substitute.For<IDeliveryRouteReadOnlyRepository>();
            Logger = Substitute.For<ILogger<GetDeliveryCostHandler>>();
            Handler = new GetDeliveryCostHandler(_deliveryRouteReadOnlyRepository, Logger, new GetDeliveryCostHandlerRequestValidator(), new HandlerExceptionStrategyFactory());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void HandleAsync_WhenRouteIsNullOrEmpty_ReturnsBadRequest(string route)
        {
            GivenRequest(route);

            WhenRequestIsHandled();

            ThenResponseShouldBeUnsuccessfulWithStatusCode(HttpStatusCode.BadRequest);
            ThenMessageShouldBe(_routeShouldNotBeEmpty);
            ThenPayloadShouldBeNull();
            ThenLoggerShouldBeCalledAndContainsMessage(LogLevel.Warning, 1, _routeShouldNotBeEmpty);
            ThenRouteRepositoryShouldNotBeCalled();
        }

        [Fact]
        public void HandleAsync_WhenRouteDoesNotExist_ReturnsNotFound()
        {
            GivenValidRequest();

            WhenRequestIsHandled();

            ThenResponseShouldBeUnsuccessfulWithStatusCode(HttpStatusCode.NotFound);
            ThenMessageShouldBe(ResponseMessages.Route.DoesNotExist);
            ThenPayloadShouldBeNull();
            ThenLoggerShouldBeCalledAndContainsMessage(LogLevel.Warning, 1, ResponseMessages.Route.DoesNotExist);
            ThenRouteRepositoryShouldBeCalledOnce();
        }

        #region Givens

        private void GivenRequest(string route) => Request = new GetDeliveryCostHandlerRequest(route);

        private void GivenValidRequest() => GivenRequest(_validRoute);

        #endregion

        #region Thens

        private void ThenRouteRepositoryShouldNotBeCalled() => _deliveryRouteReadOnlyRepository.DidNotReceive();

        private void ThenDeliveryCostShouldBeNull() => Response.Payload.DeliveryCost.Should().BeNull();

        private void ThenRouteRepositoryShouldBeCalledOnce() => _deliveryRouteReadOnlyRepository.Received(1).GetAllAsync();

        #endregion
    }
}
