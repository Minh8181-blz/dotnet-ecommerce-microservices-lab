using Application.Base.SeedWork;
using MediatR;

namespace API.Carts.Application.Commands
{
    public class CheckoutCommand : IRequest<CommandResultModel>
    {
        public int CustomerId { get; set; }

        public CheckoutCommand(int customerId)
        {
            CustomerId = customerId;
        }
    }
}
