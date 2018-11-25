using DS.Handlers.Abstract;

namespace DS.Handlers.Requests
{
    public class GetDeliveryCostHandlerRequest : HandlerRequestBase
    {
        public string Route { get; }

        public GetDeliveryCostHandlerRequest(string route)
        {
            Route = route;
        }
    }
}
