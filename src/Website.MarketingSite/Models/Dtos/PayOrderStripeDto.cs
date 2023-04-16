namespace Website.MarketingSite.Models.Dtos
{
    public class PayOrderStripeDto
    {
        public int OrderId { get; set; }
        public string SuccessRedirectUrl { get; set; }
        public string CancelRedirectUrl { get; set; }
    }
}
