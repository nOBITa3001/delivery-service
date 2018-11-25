using DS.DomainModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DS.Contracts.DataAccess.Repositories
{
    public interface IReadOnlyRepository<TEntity, IDType>
        where TEntity : EntityBase
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
    }
}
