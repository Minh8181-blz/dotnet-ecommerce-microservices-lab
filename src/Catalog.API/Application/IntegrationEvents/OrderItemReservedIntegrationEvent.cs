using API.Catalog.Application.Dto;
using Application.Base.SeedWork;
using System.Collections.Generic;

namespace API.Catalog.Application.IntegrationEvents
{
    public class OrderItemReservedIntegrationEvent : IntegrationEvent
    {
        public OrderItemReservedIntegrationEvent(OrderDto order, bool succeeded, IEnumerable<OrderItemReserveResult> reserveResults)
        {
            Order = order;
            Succeeded = succeeded;
            ReserveResults = reserveResults;
        }

        public OrderDto Order { get; }
        public bool Succeeded { get; }
        public IEnumerable<OrderItemReserveResult> ReserveResults { get; }
    }

    public class OrderItemReserveResult
    {
        public bool Succeeded { get; set; }
        public int ProductId { get; set; }
        public string Code { get; set; }
    }
}
