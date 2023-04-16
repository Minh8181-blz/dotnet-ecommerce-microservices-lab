using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace API.Payment.Infrastructure.StripeIdempotency
{
    public interface IStripeEventManagerDbContext
    {
        DbSet<StripeEvent> StripeEvents { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
