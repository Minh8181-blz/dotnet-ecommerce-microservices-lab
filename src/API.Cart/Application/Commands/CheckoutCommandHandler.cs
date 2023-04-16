using API.Carts.Application.Dto;
using API.Carts.Application.IntegrationEvents.Pub;
using API.Carts.Domain.Interfaces;
using Application.Base.SeedWork;
using Domain.Base.SeedWork;
using MediatR;
using Service.CommunicationStandard.Const;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Carts.Application.Commands
{
    public class CheckoutCommandHandler : IRequestHandler<CheckoutCommand, CommandResultModel>
    {
        private readonly ICartsRepository _cartsRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIntegrationEventService _integrationEventService;

        public CheckoutCommandHandler(
            ICartsRepository cartsRepository,
            IUnitOfWork unitOfWork,
            IIntegrationEventService integrationEventService)
        {
            _cartsRepository = cartsRepository;
            _unitOfWork = unitOfWork;
            _integrationEventService = integrationEventService;
        }

        public async Task<CommandResultModel> Handle(CheckoutCommand request, CancellationToken cancellationToken)
        {
            var result = new CommandResultModel();

            var cart = await _cartsRepository.GetActiveCartByCustomerAsync(request.CustomerId);

            if (cart == null)
            {
                result.Code = ActionCode.BadCommand;
                result.Message = "No cart available";
                return result;
            }

            var items = cart.CartItems.Select(x => new CheckoutItemDto
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity
            }).ToList();

            cart.Lock();
            string checkoutDescription = string.Join(",", cart.CartItems.Select(x => x.ProductName)).TrimEnd();
            var @event = new CheckoutIntegrationEvent(request.CustomerId, items, checkoutDescription);

            await _integrationEventService.SaveEventAsync(@event);
            await _unitOfWork.SaveEntitiesAsync();
            await _integrationEventService.PublishAsync(@event);

            result.Succeeded = true;
            result.Code = ActionCode.Success;

            return result;
        }
    }
}
