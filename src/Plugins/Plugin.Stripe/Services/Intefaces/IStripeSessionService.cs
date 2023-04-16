using Plugin.Stripe.Models;
using Plugin.Stripe.Models.ParamModels;
using System.Threading.Tasks;

namespace Plugin.Stripe.Services.Intefaces
{
    public interface IStripeSessionService
    {
        Task<StripeSession> CreateStripeSessionAsync(StripeSessionCreateModel model);
    }
}
