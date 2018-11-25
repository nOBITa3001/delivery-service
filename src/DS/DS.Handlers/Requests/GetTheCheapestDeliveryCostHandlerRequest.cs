﻿using DS.Handlers.Abstract;

namespace DS.Handlers.Requests
{
    public class GetTheCheapestDeliveryCostHandlerRequest : HandlerRequestBase
    {
        public string Route { get; }

        public GetTheCheapestDeliveryCostHandlerRequest(string route, int maxRouteRepeat, int? maxDeliveryCost, int? maxStop)
        {
            Route = route;
        }
    }
}
