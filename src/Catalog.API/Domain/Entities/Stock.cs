using API.Catalog.Domain.Exceptions;
using Domain.Base.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace API.Catalog.Domain.Entities
{
    public class Stock : Entity<int>, IAggregateRoot
    {
        public int ProductId { get; private set; }
        public int Quantity { get; private set; }
        public ICollection<StockItemRecord> StockItemRecords { get; private set; }

        public Stock(int productId, int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
            CreatedAt = DateTime.UtcNow;
            LastUpdatedAt = DateTime.UtcNow;
        }

        public bool IsReserveAllowed(int quantityToReserve)
        {
            return Quantity >= quantityToReserve;
        }

        public void ReserveItem(int orderId, int quantityToReserve)
        {
            if (!IsReserveAllowed(quantityToReserve))
            {
                throw new InsufficientStockItemsDomainException(Id, ProductId, Quantity, quantityToReserve);
            }

            if (StockItemRecords == null)
            {
                StockItemRecords = new List<StockItemRecord>();
            }

            StockItemRecords.Add(new StockItemRecord(Id, orderId, quantityToReserve));
        }

        public void UnreserveItem(int orderId)
        {
            StockItemRecords?.Remove(StockItemRecords.FirstOrDefault(x => x.OrderId == orderId));
        }
    }
}
