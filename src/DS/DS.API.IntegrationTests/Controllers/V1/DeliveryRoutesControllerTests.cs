using DS.Handlers.Responses;
using FluentAssertions;
using System.Net;
using System.Net.Http;
using Xunit;

namespace DS.API.IntegrationTests.Controllers.V1
{
    public class DeliveryRoutesControllerTests : ControllerTestsBase<GetDeliveryCostHandlerResponse>, IClassFixture<TestFixture>
    {
        private const string _url = "api/v1/DeliveryRoutes";
        private readonly string _costUrlTemplate = $"{_url}/{{0}}/Cost";
        private const string _invalidRoute = "AB";
        private const string _nonExistingRoute = "A-D-F";

        public DeliveryRoutesControllerTests(TestFixture fixture)
            : base(fixture.Client)
        {
        }

        [Fact]
        public void GetCost_WhenRouteIsInvalid_ReturnsBadRequest()
        {
            GivenCostRequest(_invalidRoute);

            WhenRequestIsHandled();

            ThenResponseShouldBeUnsuccessfulWithStatusCode(HttpStatusCode.BadRequest);
            ThenPayloadShouldBeNull();
        }

        [Fact]
        public void GetCost_WhenRouteDoesNotExist_ReturnsNotFound()
        {
            GivenCostRequest(_nonExistingRoute);

            WhenRequestIsHandled();

            ThenResponseShouldBeUnsuccessfulWithStatusCode(HttpStatusCode.NotFound);
            ThenResponseShouldContainMessage("No Such Route");
            ThenPayloadShouldBeNull();
        }

        [Theory]
        [InlineData("a-b-e", 4)]
        [InlineData("A-B-E", 4)]
        [InlineData("A-D", 10)]
        [InlineData("E-A-C-F", 8)]
        public void GetCost_WhenRouteExists_ReturnsCost(string route, int expected)
        {
            GivenCostRequest(route);

            WhenRequestIsHandled();

            ThenResponseShouldBeSuccessful();
            ThenPayloadShouldBeCorrect(expected);
        }

        #region Givens

        private void GivenCostRequest(string route) => Request = new HttpRequestMessage(HttpMethod.Get, string.Format(_costUrlTemplate, route));

        #endregion

        #region Thens

        private void ThenPayloadShouldBeCorrect(int expected) => Response.Payload.DeliveryCost.Should().Be(expected);

        #endregion
    }
}
