using MediatR;
using System;

namespace API.Payment.Application.StripeIdempotency
{
    public class StripeIdentifiedCommand<T, R> : IRequest<R> where T : IRequest<R>
    {
        public T Command { get; }
        public string Id { get; }
        public string Type { get; }

        public StripeIdentifiedCommand(T command, string id, string type)
        {
            Command = command;
            Id = id;
            Type = type;
        }
    }
}
