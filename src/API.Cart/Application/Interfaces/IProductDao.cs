using API.Carts.Application.Dto;
using System.Threading.Tasks;

namespace API.Carts.Application.Interfaces
{
    public interface IProductDao
    {
        Task<ProductDto> GetAsync(int id);
        Task<int> AddAsync(ProductDto product);
        Task<int> UpdateAsync(ProductDto product);
    }
}
