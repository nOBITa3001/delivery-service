using System;

namespace DS.DomainModel
{
    public struct ValidatedEntity<TEntity>
        where TEntity : EntityBase
    {
        public TEntity Entity { get; }

        public ValidatedEntity(TEntity entity)
        {
            Entity = (entity ?? throw new ArgumentNullException(nameof(entity)));
            Entity.Validate();
        }
    }
}
