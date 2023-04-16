using API.Ordering.Application.Dto;
using Application.Base.SeedWork;
using Ordering.API.Application.Dto;
using System.Collections.Generic;

namespace API.Ordering.Application.IntegrationEvents
{
    public class OrderItemReservedIntegrationEvent : IntegrationEvent
    {
        public OrderDto Order { get; set; }
        public bool Succeeded { get; private set; }
        public IEnumerable<OrderItemReserveResultDto> ReserveResults { get; set; }
    }
}
