using API.Payment.Application.IntegrationEvents;
using API.Payment.Application.StripeIdempotency;
using API.Payment.Domain.Interfaces;
using Application.Base.SeedWork;
using Domain.Base.SeedWork;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace API.Payment.Application.Commands
{
    public class CompleteStripePaymentCommandHandler : IRequestHandler<CompleteStripePaymentCommand, bool>
    {
        private readonly IPaymentOperationRepository _paymentOperationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIntegrationEventService _integrationEventService;

        public CompleteStripePaymentCommandHandler(
            IPaymentOperationRepository paymentOperationRepository,
            IUnitOfWork unitOfWork,
            IIntegrationEventService integrationEventService)
        {
            _paymentOperationRepository = paymentOperationRepository;
            _unitOfWork = unitOfWork;
            _integrationEventService = integrationEventService;
        }

        public async Task<bool> Handle(CompleteStripePaymentCommand request, CancellationToken cancellationToken)
        {
            if (request.Reference.StartsWith("order"))
            {
                return await HandlePaymentForOrderComplete(request, cancellationToken);
            }

            return true;
        }

        private async Task<bool> HandlePaymentForOrderComplete(CompleteStripePaymentCommand request, CancellationToken cancellationToken)
        {
            int orderId = GetOrderIdFromReference(request.Reference);

            if (orderId <= 0)
                return false;

            var payment = await _paymentOperationRepository.GetPaymentOperationByOrderIdAsync(orderId);

            if (payment == null)
                return false;

            payment.CompleteStripePaymentForOrder(request.SessionId);

            var @event = new OrderPaidIntegrationEvent(payment.OrderId.Value, payment.Amount);
            await _integrationEventService.SaveEventAsync(@event);

            await _unitOfWork.SaveEntitiesAsync(cancellationToken);
            await _integrationEventService.PublishAsync(@event);

            return true;
        }

        private int GetOrderIdFromReference(string reference)
        {
            var arr = reference.Split('_');

            if (arr.Length < 3)
                return 0;

            string orderIdStr = arr[1].TrimStart('#');
            int.TryParse(orderIdStr, out int orderId);

            return orderId;
        }
    }

    public class CompleteStripePaymentIdentifiedCommandHandler : StripeIdentifiedCommandHandler<CompleteStripePaymentCommand, bool>
    {
        public CompleteStripePaymentIdentifiedCommandHandler(
            IMediator mediator,
            IStripeEventManager requestManager)
            : base(mediator, requestManager)
        {
        }

        protected override bool CreateResultForDuplicateRequest()
        {
            return true;
        }
    }
}
