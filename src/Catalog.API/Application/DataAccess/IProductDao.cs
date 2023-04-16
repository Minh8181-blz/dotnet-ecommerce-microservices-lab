using API.Catalog.Application.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Catalog.Application.DataAccess
{
    public interface IProductDao
    {
        Task<IEnumerable<ProductDto>> GetLatestProductAsync(int limit);
        Task<IEnumerable<ProductDto>> GetProductsByIdsAsync(int[] ids);
    }
}
