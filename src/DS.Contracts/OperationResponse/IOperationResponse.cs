using System.Collections.Generic;
using System.Net;

namespace DS.Contracts.OperationResponse
{
    public interface IOperationResponse
    {
        bool IsSuccessful { get; }
        IEnumerable<string> Messages { get; }
        HttpStatusCode StatusCode { get; }
    }

    public interface IOperationResponse<out TPayload> : IOperationResponse
    {
        TPayload Payload { get; }
    }
}
