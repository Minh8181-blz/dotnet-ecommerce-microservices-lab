using API.Payment.Application.Dto;
using API.Payment.Domain.Entities;
using API.Payment.Domain.Interfaces;
using Application.Base.SeedWork;
using Domain.Base.SeedWork;
using MediatR;
using Plugin.Stripe.Models;
using Plugin.Stripe.Models.ParamModels;
using Plugin.Stripe.Services.Intefaces;
using Service.CommunicationStandard.Const;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace API.Payment.Application.Commands
{
    public class PayForOrderWithStripeCommandHandler : IRequestHandler<PayForOrderWithStripeCommand, PayForOrderWithStripeResultDto>
    {
        private readonly IPaymentOperationRepository _paymentOperationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStripeSessionService _stripeSessionService;

        public PayForOrderWithStripeCommandHandler(
            IPaymentOperationRepository paymentOperationRepository,
            IUnitOfWork unitOfWork,
            IStripeSessionService stripeSessionService)
        {
            _paymentOperationRepository = paymentOperationRepository;
            _unitOfWork = unitOfWork;
            _stripeSessionService = stripeSessionService;
        }

        public async Task<PayForOrderWithStripeResultDto> Handle(PayForOrderWithStripeCommand request, CancellationToken cancellationToken)
        {
            var result = new PayForOrderWithStripeResultDto();

            var payment = await _paymentOperationRepository.GetPaymentOperationByOrderIdAsync(request.OrderId);

            if (payment == null || payment.CustomerId != request.CustomerId)
            {
                result.Code = ActionCode.BadCommand;
                return result;
            }

            var session = await CreateStripePaymentSessionAsync(payment, request);
            payment.InitStripePayment(session.Id, session.ClientReferenceId, request.SuccessUrl, request.CancelUrl);

            _paymentOperationRepository.Update(payment);
            await _unitOfWork.SaveEntitiesAsync(cancellationToken);

            result.Succeeded = true;
            result.PaymentInstruction = new StripePaymentInstructionDto(payment, session);
            result.Code = ActionCode.Success;

            return result;
        }

        private async Task<StripeSession> CreateStripePaymentSessionAsync(PaymentOperation payment, PayForOrderWithStripeCommand request)
        {
            var model = new StripeSessionCreateModel
            {
                Name = string.Format("Payment for order #{0}", payment.OrderId),
                Amount = payment.Amount,
                PaymentMethods = new List<string> { "card" },
                Currency = "usd",
                SuccessUrl = request.SuccessUrl,
                CancelUrl = request.CancelUrl,
                ClientReferenceId = string.Format("order_#{0}_{1}", payment.OrderId, DateTime.UtcNow.Ticks)
            };

            var session = await _stripeSessionService.CreateStripeSessionAsync(model);
            return session;
        }
    }

    public class PayForOrderIdentifiedCommandHandler : IdentifiedCommandHandler<PayForOrderWithStripeCommand, PayForOrderWithStripeResultDto>
    {
        public PayForOrderIdentifiedCommandHandler(
            IMediator mediator,
            IRequestManager requestManager)
            : base(mediator, requestManager)
        {
        }

        protected override PayForOrderWithStripeResultDto CreateResultForDuplicateRequest()
        {
            var result = new PayForOrderWithStripeResultDto
            {
                Succeeded = false,
                Code = ActionCode.DuplicateCommand,
                Message = "Request is already handled"
            };

            return result;
        }
    }
}
