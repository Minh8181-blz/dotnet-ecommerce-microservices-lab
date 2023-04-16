using Microsoft.Extensions.Configuration;

namespace Website.MarketingSite.Configurations
{
    public class ApiEndpointConfiguration
    {
        public readonly string ApiOrigin;
        public readonly string IdentityOrigin;

        public readonly string ProductsGetLatest;

        public readonly string CartGetMyCart;
        public readonly string CartAddToCart;
        public readonly string CartUpdateCart;
        public readonly string CartCheckout;

        public readonly string SignUp;
        public readonly string GetIdentityToken;

        public readonly string OrdersGetMyOrders;
        public readonly string OrdersGetOrderDetails;

        public readonly string PaymentOrderStripe;

        public ApiEndpointConfiguration(IConfiguration configuration)
        {
            ApiOrigin = configuration.GetValue<string>("ServiceOrigins:API");
            IdentityOrigin = configuration.GetValue<string>("ServiceOrigins:Identity");

            var apiConfigSection = configuration.GetSection("ServiceEndpoints:API");

            ProductsGetLatest = apiConfigSection.GetValue<string>("Products:GetLatest");

            OrdersGetMyOrders = apiConfigSection.GetValue<string>("Orders:GetMyOrders");
            OrdersGetOrderDetails = apiConfigSection.GetValue<string>("Orders:GetOrderDetails");

            CartGetMyCart = apiConfigSection.GetValue<string>("Cart:GetMyCart");
            CartAddToCart = apiConfigSection.GetValue<string>("Cart:AddToCart");
            CartUpdateCart = apiConfigSection.GetValue<string>("Cart:UpdateCart");
            CartCheckout = apiConfigSection.GetValue<string>("Cart:Checkout");

            PaymentOrderStripe = apiConfigSection.GetValue<string>("Payment:PaymentOrderStripe");

            var identityConfigSection = configuration.GetSection("ServiceEndpoints:Identity");

            SignUp = identityConfigSection.GetValue<string>("SignUp");
            GetIdentityToken = identityConfigSection.GetValue<string>("GetToken");
        }
    }
}
