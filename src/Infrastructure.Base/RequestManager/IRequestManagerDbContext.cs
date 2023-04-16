using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Base.RequestManager
{
    public interface IRequestManagerDbContext
    {
        DbSet<RequestEntry> RequestEntries { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
