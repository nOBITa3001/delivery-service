using DS.DomainModel.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DS.Contracts.DataAccess.Repositories
{
    public interface IDeliveryRouteReadOnlyRepository : IReadOnlyRepository<DeliveryRoute, string>
    {
        Task<Dictionary<string, DeliveryRoute[]>> GetDictionaryAsync();
    }
}
