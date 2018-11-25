using DS.Contracts.Handlers;
using DS.Contracts.OperationResponse;
using DS.Handlers.Abstract;
using DS.Handlers.UnitTests.Extensions;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DS.Handlers.UnitTests
{
    public abstract class HandlerTestsBase<THandler, TRequest, TResponse>
        where THandler : HandlerBase<TRequest, TResponse>
        where TRequest : class, IHandlerRequest
        where TResponse : class, IHandlerResponse
    {
        protected HandlerBase<TRequest, TResponse> Handler;
        protected TRequest Request;
        protected IOperationResponse<TResponse> Response;
        protected ILogger<THandler> Logger;

        #region Whens

        protected void WhenRequestIsHandled() => Response = Task.Run(async () => await Handler.ExecuteHandlerAsync(Request)).Result;

        #endregion

        #region Thens

        protected void ThenResponseShouldBeUnsuccessfulWithStatusCode(HttpStatusCode httpStatusCode)
        {
            using (new AssertionScope())
            {
                Response.IsSuccessful.Should().BeFalse();
                Response.StatusCode.Should().Be(httpStatusCode);
            }
        }

        protected void ThenResponseShouldBeSuccessful()
        {
            using (new AssertionScope())
            {
                Response.IsSuccessful.Should().BeTrue();
                Response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        protected void ThenMessageShouldContain(string errorMessage) => Response.Messages.Should().Contain(errorMessage);

        protected void ThenMessageShouldBe(string errorMessage) => Response.Messages.Single().Should().Be(errorMessage);

        protected void ThenPayloadShouldBeNull() => Response.Payload.Should().BeNull();

        protected void ThenLoggerShouldBeCalledAndContainsMessage(LogLevel level, int numberOfCalls, string message)
        {
            Logger.ShouldBeCalledAndContainsMessage(level, numberOfCalls, message);
        }

        protected void ThenLoggerShouldNotBeCalled() => Logger.DidNotReceive();

        #endregion
    }
}
