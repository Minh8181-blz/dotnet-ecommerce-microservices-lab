using API.Payment.Application.IntegrationEvents;
using API.Payment.Domain.Entities;
using API.Payment.Domain.Enums;
using API.Payment.Domain.Interfaces;
using Application.Base.SeedWork;
using Domain.Base.SeedWork;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace API.Payment.Application.Commands
{
    public class CreateDraftPaymentCommandHandler : IRequestHandler<CreateDraftPaymentCommand, bool>
    {
        public IUnitOfWork _unitOfWork;
        public IPaymentOperationRepository _paymentOperationRepository;
        public IIntegrationEventService _integrationEventService;

        public CreateDraftPaymentCommandHandler(
            IUnitOfWork unitOfWork,
            IPaymentOperationRepository paymentOperationRepository,
            IIntegrationEventService integrationEventService)
        {
            _unitOfWork = unitOfWork;
            _paymentOperationRepository = paymentOperationRepository;
            _integrationEventService = integrationEventService;
        }

        public async Task<bool> Handle(CreateDraftPaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = PaymentOperation.CreateDraftPayment(request.CustomerId, PaymentPurpose.OrderPurchase, request.OrderId, request.Amount);
            _paymentOperationRepository.Add(payment);

            var @event = new DraftPaymentCreatedIntegrationEvent(payment);
            await _integrationEventService.SaveEventAsync(@event);
            await _unitOfWork.SaveEntitiesAsync(cancellationToken);

            await _integrationEventService.PublishAsync(@event);

            return true;
        }
    }

    public class CreateDraftPaymentIdentifiedCommandHandler : IdentifiedCommandHandler<CreateDraftPaymentCommand, bool>
    {
        public CreateDraftPaymentIdentifiedCommandHandler(
            IMediator mediator,
            IRequestManager requestManager)
            : base(mediator, requestManager)
        {
        }

        protected override bool CreateResultForDuplicateRequest()
        {
            return true;
        }
    }
}
