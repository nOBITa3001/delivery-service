using DS.DomainModel.Entities;
using DS.Dtos.Routes;

namespace DS.DomainModel.Factories
{
    public interface IRouteFactory
    {
        Route Create(CreateRouteDto dto);
    }
}
