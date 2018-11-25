using DS.Handlers.Abstract;

namespace DS.Handlers.Responses
{
    public class GetDeliveryCostHandlerResponse : HandlerResponseBase
    {
        public int DeliveryCost { get; }

        public GetDeliveryCostHandlerResponse(int deliveryCost)
        {
            DeliveryCost = deliveryCost;
        }
    }
}
