using DS.Contracts.DataAccess.Repositories;
using DS.DomainModel.Entities;
using DS.DomainModel.Factories;
using DS.Dtos.Builders;
using DS.Dtos.ResponseMessages;
using DS.Dtos.Routes;
using DS.Handlers.Requests;
using DS.Handlers.Responses;
using DS.Handlers.Strategies.Factories;
using DS.Handlers.Validators;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Xunit;
using static DS.DomainModel.Entities.DeliveryRoute;

namespace DS.Handlers.UnitTests
{
    public class GetDeliveryCostHandlerTests : HandlerTestsBase<GetDeliveryCostHandler, GetDeliveryCostHandlerRequest, GetDeliveryCostHandlerResponse>
    {
        private const string _validRoute = "A-B-E";
        private const string _routeShouldNotBeEmpty = "'Route' should not be empty.";

        private readonly IDeliveryRouteReadOnlyRepository _deliveryRouteReadOnlyRepository;
        private readonly IRouteFactory _routeFactory;
        private readonly Dictionary<string, DeliveryRoute[]> _deliveryRoutes = new Dictionary<string, DeliveryRoute[]>
        {
            { "A", new [] { new DeliveryRouteFactory().Create(new CreateDeliveryRouteDtoBuilder().With(x => x.Start = "A").With(x => x.End = "B").With(x => x.Cost = 1)) }},
            { "B", new [] { new DeliveryRouteFactory().Create(new CreateDeliveryRouteDtoBuilder().With(x => x.Start = "B").With(x => x.End = "E").With(x => x.Cost = 3)) }},
        };

        public GetDeliveryCostHandlerTests()
        {
            _deliveryRouteReadOnlyRepository = Substitute.For<IDeliveryRouteReadOnlyRepository>();
            _routeFactory = Substitute.For<IRouteFactory>();
            Logger = Substitute.For<ILogger<GetDeliveryCostHandler>>();
            Handler = new GetDeliveryCostHandler(_deliveryRouteReadOnlyRepository, _routeFactory, Logger, new GetDeliveryCostHandlerRequestValidator(), new HandlerExceptionStrategyFactory());
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

        [Fact]
        public void HandleAsync_WithValidRequest_ReturnsDeliveryCost()
        {
            GivenDeliveryRouteData();
            GivenRouteFactory();
            GivenValidRequest();

            WhenRequestIsHandled();

            ThenResponseShouldBeSuccessful();
            ThenPayloadShouldBeCorrect();
            ThenRouteRepositoryShouldBeCalledOnce();
            ThenLoggerShouldNotBeCalled();
        }

        #region Givens

        private void GivenRequest(string route) => Request = new GetDeliveryCostHandlerRequest(route);

        private void GivenValidRequest() => GivenRequest(_validRoute);

        private void GivenDeliveryRouteData()
        {
            _deliveryRouteReadOnlyRepository
                .GetDictionaryAsync()
                .Returns(_deliveryRoutes);
        }

        private void GivenRouteFactory()
        {
            foreach (var deliveryRoute in _deliveryRoutes)
            {
                var (start, end) = (deliveryRoute.Key, deliveryRoute.Value.First().End);

                var deliveryRouteStub = Substitute.For<DeliveryRoute>();
                deliveryRouteStub.Start.Returns(start);
                deliveryRouteStub.End.Returns(end);

                _routeFactory
                    .Create(Arg.Is<CreateRouteDto>(route => route.Start == start && route.End == end))
                    .Returns(deliveryRouteStub);
            }
        }

        #endregion

        #region Thens

        private void ThenRouteRepositoryShouldNotBeCalled() => _deliveryRouteReadOnlyRepository.DidNotReceive();

        private void ThenDeliveryCostShouldBeNull() => Response.Payload.DeliveryCost.Should().BeNull();

        private void ThenRouteRepositoryShouldBeCalledOnce() => _deliveryRouteReadOnlyRepository.Received(1).GetDictionaryAsync();

        private void ThenPayloadShouldBeCorrect() => Response.Payload.DeliveryCost.Should().Be(_deliveryRoutes.SelectMany(deliveryRoute => deliveryRoute.Value).Sum(x => x.Cost));

        #endregion
    }
}
