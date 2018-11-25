using DS.DomainModel.Entities;

namespace DS.Contracts.DataAccess.Repositories
{
    public interface IDeliveryRouteReadOnlyRepository : IReadOnlyRepository<DeliveryRoute, string>
    {
    }
}
