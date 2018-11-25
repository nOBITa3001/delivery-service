using DS.Dtos.Builders;
using DS.Dtos.Exceptions;
using DS.Dtos.ResponseMessages;
using DS.Handlers.Requests;
using DS.Handlers.Responses;
using DS.Handlers.Strategies.Exceptions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Net;
using Xunit;

namespace DS.Handlers.UnitTests.Strategies.Exceptions
{
    public class HandlerDataAccessNotFoundExceptionStrategyTests : HandlerExceptionStrategyTestsBase<GetDeliveryCostHandlerRequest, GetDeliveryCostHandlerResponse>
    {
        public HandlerDataAccessNotFoundExceptionStrategyTests()
        {
            Logger = Substitute.For<ILogger>();
        }

        [Fact]
        public void Handle_Always_ReturnsNotFoundResponse()
        {
            GivenRequest();
            GivenHandler(new DataAccessNotFoundException(ResponseMessages.DeliveryRoute.NotFound));

            WhenRequestIsHandled();

            ThenResponseShouldBeUnsuccessfulWithStatusCode(HttpStatusCode.NotFound);
            ThenErrorMessageShouldBe(ResponseMessages.DeliveryRoute.NotFound);
            ThenPayloadShouldBeNull();
            ThenLoggerShouldBeCalledAndContainsMessage(LogLevel.Warning, 1, ResponseMessages.DeliveryRoute.NotFound);
        }

        #region Givens

        private void GivenRequest() => Request = new GetDeliveryCostHandlerRequest(StringGenerator.Random(20));

        private void GivenHandler(DataAccessNotFoundException exception)
        {
            Handler = new HandlerDataAccessNotFoundExceptionStrategy<GetDeliveryCostHandlerRequest, GetDeliveryCostHandlerResponse>(Request, Logger, exception);
        }

        #endregion
    }
}
