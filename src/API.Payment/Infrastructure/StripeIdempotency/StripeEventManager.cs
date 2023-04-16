using API.Payment.Application.StripeIdempotency;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace API.Payment.Infrastructure.StripeIdempotency
{
    public class StripeEventManager<T> : IStripeEventManager where T : IStripeEventManagerDbContext
    {
        protected readonly IStripeEventManagerDbContext _context;

        public StripeEventManager(T context)
        {
            _context = context;
        }

        public async Task CreateEventForCommandAsync(string id, string type)
        {
            var request = new StripeEvent()
            {
                Id = id,
                Type = type,
                Time = DateTime.UtcNow
            };

            _context.StripeEvents.Add(request);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistAsync(string id)
        {
            var request = await _context.StripeEvents.FirstOrDefaultAsync(x => x.Id == id);

            return request != null;
        }
    }
}
