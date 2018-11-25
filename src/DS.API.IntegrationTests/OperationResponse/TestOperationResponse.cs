using DS.Contracts.OperationResponse;
using System.Collections.Generic;
using System.Net;

namespace DS.API.IntegrationTests.OperationResponse
{
    public class TestOperationResponse<TResponse> : IOperationResponse<TResponse>
    {
        public bool IsSuccessful { get; set; }
        public IEnumerable<string> Messages { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public TResponse Payload { get; set; }
    }
}
