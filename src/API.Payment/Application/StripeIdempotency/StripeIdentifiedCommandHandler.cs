using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace API.Payment.Application.StripeIdempotency
{
    public class StripeIdentifiedCommandHandler<T, R> : IRequestHandler<StripeIdentifiedCommand<T, R>, R>
        where T : IRequest<R>
    {
        private readonly IMediator _mediator;
        private readonly IStripeEventManager _eventManager;

        public StripeIdentifiedCommandHandler(
            IMediator mediator,
            IStripeEventManager eventManager)
        {
            _mediator = mediator;
            _eventManager = eventManager;
        }

        protected virtual R CreateResultForDuplicateRequest()
        {
            return default;
        }

        public async Task<R> Handle(StripeIdentifiedCommand<T, R> message, CancellationToken cancellationToken)
        {
            var alreadyExists = await _eventManager.ExistAsync(message.Id);
            if (alreadyExists)
            {
                return CreateResultForDuplicateRequest();
            }
            else
            {
                await _eventManager.CreateEventForCommandAsync(message.Id, message.Type);

                var command = message.Command;

                var result = await _mediator.Send(command, cancellationToken);

                return result;
            }
        }
    }
}