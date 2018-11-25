using DS.DomainModel.Entities;

namespace DS.DataAccess.Database.InMemory
{
    public class InMemoryDeliveryRoute : DeliveryRoute
    {
        public InMemoryDeliveryRoute(string start, string end, int cost)
        {
            Start = start;
            End = end;
            Cost = cost;
        }
    }
}
