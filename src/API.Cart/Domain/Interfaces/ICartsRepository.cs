using API.Carts.Domain.Entities;
using Domain.Base.SeedWork;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Carts.Domain.Interfaces
{
    public interface ICartsRepository : IRepository<Cart, int>
    {
        Task<List<Cart>> GetAllCartsHavingProductAsync(int productId);
        Task<Cart> GetActiveCartByCustomerAsync(int customerId);
    }
}
