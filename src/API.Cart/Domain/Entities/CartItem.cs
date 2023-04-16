using API.Carts.Domain.Enums;
using Domain.Base.SeedWork;
using System;

namespace API.Carts.Domain.Entities
{
    public class CartItem : Entity<int>
    {
        public CartItem(int productId, string productName, decimal unitPrice, int quantity, string pictureUrl)
        {
            ProductId = productId;
            ProductName = productName;
            UnitPrice = unitPrice;
            Quantity = quantity > 0 ? quantity : 1;
            PictureUrl = pictureUrl;
            CreatedAt = DateTime.UtcNow;
            LastUpdatedAt = DateTime.UtcNow;
            UpdateStatus(CartItemStatus.Active);
            CalculateTotal();
        }

        public int CartId { get; private set; }
        public int ProductId { get; private set; }
        public string ProductName { get; private set; }
        public decimal UnitPrice { get; private set; }
        public int Quantity { get; private set; }
        public string PictureUrl { get; private set; }
        public decimal TotalPrice { get; private set; }
        public int Status { get; private set; }
        public string StatusText { get; private set; }

        public bool IsValid()
        {
            return Quantity > 0 && UnitPrice >= 0;
        }

        public void UpdateProductDetails(string productName, decimal unitPrice, string pictureUrl)
        {
            ProductName = productName;
            UnitPrice = unitPrice;
            PictureUrl = pictureUrl;
            CalculateTotal();
            LastUpdatedAt = DateTime.UtcNow;
        }

        public void UpdateQuantityBy(int quantity)
        {
            if (Quantity + quantity > 0)
                Quantity += quantity;
            CalculateTotal();
            LastUpdatedAt = DateTime.UtcNow;
        }

        public void MarkAsRemoved()
        {
            UpdateStatus(CartItemStatus.Removed);
            LastUpdatedAt = DateTime.UtcNow;
        }

        private void UpdateStatus(CartItemStatus status)
        {
            Status = status.Id;
            StatusText = status.Name;
        }

        private void CalculateTotal()
        {
            TotalPrice = UnitPrice * Quantity;
        }

    }
}
