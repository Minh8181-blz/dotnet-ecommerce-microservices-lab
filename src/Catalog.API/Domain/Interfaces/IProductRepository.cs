using API.Catalog.Domain.Entities;
using Domain.Base.SeedWork;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Catalog.Domain.Interfaces
{
    public interface IProductRepository : IRepository<Product, int>
    {
        Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<int> ids);
    }
}
