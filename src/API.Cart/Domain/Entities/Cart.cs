using API.Carts.Domain.Enums;
using Domain.Base.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace API.Carts.Domain.Entities
{
    public class Cart : Entity<int>, IAggregateRoot
    {
        protected Cart() { }

        public Cart(int customerId, List<CartItem> items)
        {
            CustomerId = customerId;
            CartItems = items ?? new List<CartItem>();
            CreatedAt = DateTime.UtcNow;
            LastUpdatedAt = DateTime.UtcNow;
            CalculateTotal();
            UpdateStatus(CartStatus.Active);
        }

        public int CustomerId { get; private set; }
        public decimal TotalPrice { get; private set; }
        public int Status { get; private set; }
        public string StatusText { get; private set; }
        public ICollection<CartItem> CartItems { get; private set; }

        public void Abandon()
        {
            UpdateStatus(CartStatus.Abandoned);
            LastUpdatedAt = DateTime.UtcNow;
        }

        public bool HasProductItems(int productId)
        {
            return CartItems != null && CartItems.Any(x => x.ProductId == productId);
        }

        public bool IsEmpty()
        {
            return CartItems == null || !CartItems.Any(x => x.Status == CartItemStatus.Active.Id);
        }

        public void AddItem(CartItem item)
        {
            if (Status != CartStatus.Active.Id)
                return;

            var existingItem = CartItems?.FirstOrDefault(x => x.ProductId == item.ProductId);

            if (existingItem != null)
                return;

            if (item.IsValid())
            {
                if (CartItems == null)
                {
                    CartItems = new List<CartItem>();
                }

                CartItems.Add(item);
                CalculateTotal();
                LastUpdatedAt = DateTime.UtcNow;
            }
        }

        public void UpdateItemQuantity(int productId, int quantity)
        {
            var existingItem = CartItems?.FirstOrDefault(x => x.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.UpdateQuantityBy(quantity);
                CalculateTotal();
                LastUpdatedAt = DateTime.UtcNow;
            }
        }

        public void UpdateItemProductDetails(int productId, string productName, decimal unitPrice, string pictureUrl)
        {
            var existingItem = CartItems?.FirstOrDefault(x => x.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.UpdateProductDetails(productName, unitPrice, pictureUrl);
                CalculateTotal();
                LastUpdatedAt = DateTime.UtcNow;
            }
        }

        public void RemoveItem(int productId)
        {
            var existingItem = CartItems?.FirstOrDefault(x => x.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.MarkAsRemoved();
                CalculateTotal();
                if (IsEmpty())
                {
                    UpdateStatus(CartStatus.Abandoned);
                }
                LastUpdatedAt = DateTime.UtcNow;
            }
        }

        public void Lock()
        {
            UpdateStatus(CartStatus.Locked);
            LastUpdatedAt = DateTime.UtcNow;
        }

        public void Unload()
        {
            UpdateStatus(CartStatus.Unloaded);
            LastUpdatedAt = DateTime.UtcNow;
        }

        private void CalculateTotal()
        {
            TotalPrice = CartItems
                .Where(x => x.Status == CartItemStatus.Active.Id)
                .Sum(x => x.TotalPrice);
        }

        private void UpdateStatus(CartStatus status)
        {
            Status = status.Id;
            StatusText = status.Name;
        }
    }
}
