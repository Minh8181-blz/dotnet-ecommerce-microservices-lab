using Domain.Base.SeedWork;

namespace API.Catalog.Domain.Exceptions
{
    public class InsufficientStockItemsDomainException : DomainException
    {
        public InsufficientStockItemsDomainException(int stockId, int productId, int stockQuantity, int quantityToReserve) :
            base($"Cannot reserve product {productId} in stock {stockId}: only {stockQuantity} are available when the order asks for {quantityToReserve}")
        {
            ProductId = productId;
            StockQuantity = stockQuantity;
        }

        public int ProductId { get; }
        public int StockQuantity { get; }
    }
}
