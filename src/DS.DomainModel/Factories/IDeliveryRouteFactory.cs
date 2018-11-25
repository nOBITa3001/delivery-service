using DS.DomainModel.Entities;
using DS.Dtos.Routes;

namespace DS.DomainModel.Factories
{
    public interface IDeliveryRouteFactory
    {
        DeliveryRoute Create(CreateDeliveryRouteDto dto);
    }
}
