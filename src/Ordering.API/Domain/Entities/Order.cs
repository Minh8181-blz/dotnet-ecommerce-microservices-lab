using API.Ordering.Domain.Exceptions;
using Domain.Base.SeedWork;
using Ordering.API.Domain.Enums;
using Ordering.API.Domain.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Ordering.API.Domain.Entities
{
    public class Order : Entity<int>, IAggregateRoot
    {
        protected Order() { }

        public static Order CreateOrder(int customerId, string description, IEnumerable<OrderItem> items)
        {
            var order = new Order
            {
                CustomerId = customerId,
                Description = description,
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow,
                OrderItems = new Collection<OrderItem>(items.ToList()),
                Amount = items.Sum(x => x.Quantity * x.UnitPrice)
            };

            order.UpdateStatus(OrderStatus.Pending);
            order.AddDomainEvent(new OrderCreatedDomainEvent(order));

            return order;
        }

        public int CustomerId { get; private set; }
        public string Description { get; private set; }
        public int Status { get; private set; }
        public string StatusText { get; private set; }
        public decimal Amount { get; private set; }
        public ICollection<OrderItem> OrderItems { get; private set; }

        public void UpdatePrice(decimal totalPrice)
        {
            Amount = totalPrice;
            LastUpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new OrderPriceUpdatedDomainEvent(Id, Amount));
        }

        public void UpdateStatusToAwaitingPayment()
        {
            if (Status != OrderStatus.Pending.Id)
            {
                throw new InvalidOrderStatusChangeDomainException(Id, Enumeration.FromValue<OrderStatus>(Status), OrderStatus.AwaitingPayment);
            }

            UpdateStatus(OrderStatus.AwaitingPayment);
            LastUpdatedAt = DateTime.UtcNow;
        }

        public void UpdateStatusDeclined()
        {
            if (Status != OrderStatus.Pending.Id)
            {
                throw new InvalidOrderStatusChangeDomainException(Id, Enumeration.FromValue<OrderStatus>(Status), OrderStatus.Declined);
            }

            UpdateStatus(OrderStatus.Declined);
            LastUpdatedAt = DateTime.UtcNow;
        }

        public void UpdateStatusToAwaitingShipment()
        {
            if (Status != OrderStatus.AwaitingPayment.Id)
            {
                throw new InvalidOrderStatusChangeDomainException(Id, Enumeration.FromValue<OrderStatus>(Status), OrderStatus.AwaitingPayment);
            }

            UpdateStatus(OrderStatus.AwaitingShipment);
            LastUpdatedAt = DateTime.UtcNow;
        }

        private void UpdateStatus(OrderStatus status)
        {
            Status = status.Id;
            StatusText = status.Name;
        }
    }
}
