using DS.Handlers.Abstract;

namespace DS.Handlers.Responses
{
    public class GetTheCheapestDeliveryCostHandlerResponse : HandlerResponseBase
    {
        public int? TheCheapestDeliveryCos { get; }

        public GetTheCheapestDeliveryCostHandlerResponse(int? theCheapestDeliveryCos)
        {
            TheCheapestDeliveryCos = theCheapestDeliveryCos;
        }
    }
}
