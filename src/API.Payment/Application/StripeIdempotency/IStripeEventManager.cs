using System.Threading.Tasks;

namespace API.Payment.Application.StripeIdempotency
{
    public interface IStripeEventManager
    {
        Task<bool> ExistAsync(string id);
        Task CreateEventForCommandAsync(string id, string type);
    }
}
