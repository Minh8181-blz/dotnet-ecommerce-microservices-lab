using API.Carts.Domain.Entities;
using API.Carts.Domain.Enums;
using API.Carts.Domain.Interfaces;
using Infrastructure.Base.Database;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace API.Carts.Infrastructure
{
    public class CartsRepository : RepositoryBase<CartContext, Cart, int>, ICartsRepository
    {
        public CartsRepository(CartContext cartContext) : base(cartContext)
        {
        }

        public async Task<List<Cart>> GetAllCartsHavingProductAsync(int productId)
        {
            var cartIds = await _context.CartItems
                .Where(x => x.ProductId == productId)
                .Select(x => x.CartId)
                .Distinct()
                .ToListAsync();

            var carts = await _context.Carts.Where(x => cartIds.Contains(x.Id)).ToListAsync();
            return carts;
        }

        public async Task<Cart> GetActiveCartByCustomerAsync(int customerId)
        {
            return await _context.Carts
                .IncludeFilter(x => x.CartItems.Where(i => i.Status == CartItemStatus.Active.Id))
                .FirstOrDefaultAsync(x => x.CustomerId == customerId && x.Status == CartStatus.Active.Id);
        }
    }
}
