using DS.Contracts.OperationResponse;
using System.Collections.Generic;
using System.Net;

namespace DS.Handlers.OperationResponse
{
    public class OperationResponse<TPayload> : IOperationResponse<TPayload>
    {
        public bool IsSuccessful => (StatusCode == HttpStatusCode.OK);

        public IEnumerable<string> Messages { get; private set; }

        public HttpStatusCode StatusCode { get; private set; }

        public TPayload Payload { get; private set; }

        private OperationResponse()
        {
        }

        public static IOperationResponse<TPayload> Success(TPayload payload, params string[] messages)
        {
            return new OperationResponse<TPayload> { StatusCode = HttpStatusCode.OK, Messages = messages, Payload = payload };
        }

        public static IOperationResponse<TPayload> BadRequest(params string[] messages)
        {
            return new OperationResponse<TPayload> { StatusCode = HttpStatusCode.BadRequest, Messages = messages };
        }

        public static IOperationResponse<TPayload> NotFound(params string[] messages)
        {
            return new OperationResponse<TPayload> { StatusCode = HttpStatusCode.NotFound, Messages = messages };
        }

        public static IOperationResponse<TPayload> InternalServerError(params string[] messages)
        {
            return new OperationResponse<TPayload> { StatusCode = HttpStatusCode.InternalServerError, Messages = messages };
        }

        public static IOperationResponse<TPayload> ServiceUnavailable(params string[] messages)
        {
            return new OperationResponse<TPayload> { StatusCode = HttpStatusCode.ServiceUnavailable, Messages = messages };
        }
    }
}
