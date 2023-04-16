using API.Carts.Application.Dto;
using API.Carts.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Carts.Infrastructure
{
    public class ProductDao : IProductDao
    {
        private readonly CartContext _context;
        private readonly ILogger<ProductDao> _logger;

        public ProductDao(CartContext context, ILogger<ProductDao> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ProductDto> GetAsync(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int> AddAsync(ProductDto product)
        {
            try
            {
                var result = await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                return result.Entity.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return 0;
            }
        }

        public async Task<int> UpdateAsync(ProductDto product)
        {
            try
            {
                _context.Entry(product).State = EntityState.Modified;
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return 0;
            }
        }
    }
}
