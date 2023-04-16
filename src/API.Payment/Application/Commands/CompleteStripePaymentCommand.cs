using MediatR;

namespace API.Payment.Application.Commands
{
    public class CompleteStripePaymentCommand : IRequest<bool>
    {
        public CompleteStripePaymentCommand(string sessionId, string reference)
        {
            SessionId = sessionId;
            Reference = reference;
        }

        public string SessionId { get; }
        public string Reference { get; }
    }
}
