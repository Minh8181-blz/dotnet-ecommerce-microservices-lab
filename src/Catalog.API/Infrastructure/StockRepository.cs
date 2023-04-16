using API.Catalog.Domain.Entities;
using API.Catalog.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Catalog.Infrastructure
{
    public class StockRepository : IStockRepository
    {
        private readonly CatalogContext _context;

        public StockRepository(CatalogContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Stock Add(Stock entity)
        {
            return _context.Stocks.Add(entity).Entity;
        }

        public async Task<Stock> GetAsync(int id)
        {
            var stock = await _context
                .Stocks
                .FirstOrDefaultAsync(o => o.Id == id);

            return stock;
        }

        public void Update(Stock entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task<IEnumerable<Stock>> GetByProductIdsAsync(IEnumerable<int> ids)
        {
            if (ids == null || !ids.Any())
                return null;

            var stocks = await _context.Stocks
                .Where(x => ids.Contains(x.ProductId))
                .ToListAsync();

            return stocks;
        }
    }
}
