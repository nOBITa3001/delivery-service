using DS.DomainModel.Factories;
using DS.DomainModel.Mappers;
using DS.Dtos.Exceptions;
using DS.Dtos.ResponseMessages;
using DS.Dtos.Routes;
using System.ComponentModel.DataAnnotations;

namespace DS.DomainModel.Entities
{
    public class DeliveryRoute : Route
    {
        #region Factory

        public class DeliveryRouteFactory : IDeliveryRouteFactory
        {
            public DeliveryRoute Create(CreateDeliveryRouteDto dto)
            {
                if (dto is null)
                    throw new DomainModelException(ResponseMessages.DeliveryRoute.CreateDeliveryRouteDtoRequired);

                var entity = new DeliveryRoute();
                ObjectMapper.Map(dto, entity);

                return new ValidatedEntity<DeliveryRoute>(entity).Entity;
            }
        }

        #endregion

        [Range(0, int.MaxValue, ErrorMessage = ResponseMessages.Validation.RangeInvalidTemplate)]
        public int Cost { get; protected set; }

        protected DeliveryRoute()
        {
        }
    }
}
