using DS.Handlers.Abstract;

namespace DS.Handlers.Responses
{
    public class GetTheCheapestDeliveryCostHandlerResponse : HandlerResponseBase
    {
        public int? TheCheapestDeliveryCost { get; }

        public GetTheCheapestDeliveryCostHandlerResponse(int? theCheapestDeliveryCost)
        {
            TheCheapestDeliveryCost = theCheapestDeliveryCost;
        }
    }
}
