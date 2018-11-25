using DS.DomainModel.Entities;
using System.Collections.Generic;

namespace DS.DataAccess.Database
{
    public interface IDbContext
    {
        IEnumerable<DeliveryRoute> DeliveryRoutes { get; }
    }
}
