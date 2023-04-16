using API.Catalog.Domain.Entities;
using Domain.Base.SeedWork;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Catalog.Domain.Interfaces
{
    public interface IStockRepository : IRepository<Stock,int>
    {
        Task<IEnumerable<Stock>> GetByProductIdsAsync(IEnumerable<int> ids);
    }
}
