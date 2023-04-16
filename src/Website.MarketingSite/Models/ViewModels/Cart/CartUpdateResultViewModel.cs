namespace Website.MarketingSite.Models.ViewModels.Cart
{
    public class CartUpdateResultViewModel
    {
        public bool Succeeded { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public CartViewModel Cart { get; set; }
    }
}
