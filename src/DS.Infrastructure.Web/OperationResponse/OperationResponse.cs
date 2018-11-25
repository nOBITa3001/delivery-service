using DS.Contracts.OperationResponse;
using System.Collections.Generic;
using System.Net;

namespace DS.Infrastructure.Web
{
    public class OperationResponse : IOperationResponse
    {
        public bool IsSuccessful => (StatusCode == HttpStatusCode.OK);

        public IEnumerable<string> Messages { get; private set; }

        public HttpStatusCode StatusCode { get; private set; }

        private OperationResponse()
        {
        }

        public static IOperationResponse InternalServerError(params string[] messages)
        {
            return new OperationResponse { StatusCode = HttpStatusCode.InternalServerError, Messages = messages };
        }
    }
}
