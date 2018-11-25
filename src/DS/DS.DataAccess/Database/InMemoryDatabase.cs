using DS.DataAccess.Database.InMemory;
using DS.DomainModel.Entities;
using System.Collections.Generic;

namespace DS.DataAccess.Database
{
    public class InMemoryDatabase : IDbContext
    {
        private static readonly InMemoryDeliveryRoute[] _givenDeliveryRoutes = new InMemoryDeliveryRoute[]
        {
            new InMemoryDeliveryRoute("A", "B", 1),
            new InMemoryDeliveryRoute("A", "C", 4),
            new InMemoryDeliveryRoute("A", "D", 10),
            new InMemoryDeliveryRoute("B", "E", 3),
            new InMemoryDeliveryRoute("C", "D", 4),
            new InMemoryDeliveryRoute("C", "F", 2),
            new InMemoryDeliveryRoute("D", "E", 1),
            new InMemoryDeliveryRoute("E", "B", 3),
            new InMemoryDeliveryRoute("E", "A", 2),
            new InMemoryDeliveryRoute("F", "D", 1)
        };

        public IEnumerable<DeliveryRoute> DeliveryRoutes => _givenDeliveryRoutes;
    }
}
