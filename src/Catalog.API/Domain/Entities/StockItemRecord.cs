using API.Catalog.Domain.Enums;
using Domain.Base.SeedWork;
using System;

namespace API.Catalog.Domain.Entities
{
    public class StockItemRecord : Entity<int>
    {
        public StockItemRecord(int productId, int orderId, int quantity)
        {
            ProductId = productId;
            OrderId = orderId;
            Quantity = quantity;
            Status = StockItemRecordStatus.Reserved.Id;
            CreatedAt = DateTime.UtcNow;
            LastUpdatedAt = DateTime.UtcNow;
        }

        public int StockId { get; private set; }
        public int ProductId { get; private set; }
        public int OrderId { get; private set; }
        public int Quantity { get; private set; }
        public int Status { get; private set; }
    }
}
