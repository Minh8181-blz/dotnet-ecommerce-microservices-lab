using Application.Base.SeedWork;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace API.Ordering.Application.Commands
{
    public class UpdateOrderOnItemReservedCommandHandler : IRequestHandler<UpdateOrderOnItemReservedCommand, bool>
    {
        private readonly IMediator _mediator;

        public UpdateOrderOnItemReservedCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<bool> Handle(UpdateOrderOnItemReservedCommand request, CancellationToken cancellationToken)
        {
            if (!request.Succeeded)
            {
                var command = new UpdateOrderToDeclinedCommand(request.Order.Id);
                return await _mediator.Send(command, cancellationToken);
            }

            return true;
        }
    }
    public class UpdateOrderOnItemReservedIdentifiedCommandHandler : IdentifiedCommandHandler<UpdateOrderOnItemReservedCommand, bool>
    {
        public UpdateOrderOnItemReservedIdentifiedCommandHandler(
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
