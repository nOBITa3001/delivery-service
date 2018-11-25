using DS.DomainModel.Factories;
using DS.DomainModel.Mappers;
using DS.Dtos.Exceptions;
using DS.Dtos.ResponseMessages;
using DS.Dtos.Routes;
using System.ComponentModel.DataAnnotations;

namespace DS.DomainModel.Entities
{
    public class Route : EntityBase
    {
        #region Factory

        public class RouteFactory : IRouteFactory
        {
            public Route Create(CreateRouteDto dto)
            {
                if (dto is null)
                    throw new DomainModelException(ResponseMessages.Route.CreateRouteDtoRequired);

                var entity = new Route();
                ObjectMapper.Map(dto, entity);

                return new ValidatedEntity<Route>(entity).Entity;
            }
        }

        #endregion

        [Required(ErrorMessage = ResponseMessages.Validation.StartRequired)]
        public string Start { get; protected set; }

        [Required(ErrorMessage = ResponseMessages.Validation.EndRequired)]
        public string End { get; protected set; }

        protected Route()
        {
        }
    }
}
