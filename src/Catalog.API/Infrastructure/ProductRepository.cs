using API.Catalog.Domain.Entities;
using API.Catalog.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Catalog.Infrastructure
{
    public class ProductRepository : IProductRepository
    {
        private readonly CatalogContext _context;

        public ProductRepository(CatalogContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Product Add(Product product)
        {
            return _context.Products.Add(product).Entity;
        }

        public async Task<Product> GetAsync(int id)
        {
            var product = await _context
                .Products
                .FirstOrDefaultAsync(o => o.Id == id);

            return product;
        }

        public void Update(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
        }

        public async Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<int> ids)
        {
            if (ids == null || !ids.Any())
                return null;

            var products = await _context.Products
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();

            return products;
        }
    }
}
