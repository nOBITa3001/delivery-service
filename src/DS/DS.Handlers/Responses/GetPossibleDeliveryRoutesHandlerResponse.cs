using DS.Handlers.Abstract;

namespace DS.Handlers.Responses
{
    public class GetPossibleDeliveryRoutesHandlerResponse : HandlerResponseBase
    {
        public int? PossibleRoute { get; }

        public GetPossibleDeliveryRoutesHandlerResponse(int? possibleRoute)
        {
            PossibleRoute = possibleRoute;
        }
    }
}
