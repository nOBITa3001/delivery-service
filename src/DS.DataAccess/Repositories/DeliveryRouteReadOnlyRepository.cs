using DS.Contracts.DataAccess.Repositories;
using DS.DataAccess.Database;
using DS.DomainModel.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DS.DataAccess.Repositories
{
    public class DeliveryRouteReadOnlyRepository : IDeliveryRouteReadOnlyRepository
    {
        private readonly IDbContext _dbContext;

        public DeliveryRouteReadOnlyRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<IEnumerable<DeliveryRoute>> GetAllAsync()
        {
            return Task.FromResult(_dbContext.DeliveryRoutes);
        }

        public async Task<Dictionary<string, DeliveryRoute[]>> GetDictionaryAsync()
        {
            var deliveryRoutes = await GetAllAsync();

            return deliveryRoutes.GroupBy(deliveryRoute => deliveryRoute.Start).ToDictionary(group => group.Key, group => group.ToArray());
        }
    }
}
