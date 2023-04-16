using Website.MarketingSite.Consts;

namespace Website.MarketingSite.Models.ViewModels.Cart
{
    public class UpdateCartViewModel
    {
        public int ProductId { get; set; }
        public CartAction Action { get; set; }
    }
}
