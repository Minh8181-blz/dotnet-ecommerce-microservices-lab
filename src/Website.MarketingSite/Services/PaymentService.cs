using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Website.MarketingSite.Configurations;
using Website.MarketingSite.Models.Dtos;
using Website.MarketingSite.Models.ViewModels.Payment;

namespace Website.MarketingSite.Services
{
    public class PaymentService : HttpServiceBase
    {
        private readonly ILogger<PaymentService> _logger;
        private readonly ApiEndpointConfiguration _endpointConfiguration;
        private readonly PaymentConfiguration _paymentConfiguration;

        public PaymentService(
            HttpClient client,
            ILogger<PaymentService> logger,
            ApiEndpointConfiguration endpointConfiguration,
            IOptions<PaymentConfiguration> paymentConfiguration
            ) : base(client)
        {
            _logger = logger;
            _endpointConfiguration = endpointConfiguration;
            Client.BaseAddress = new Uri(_endpointConfiguration.ApiOrigin);
            _paymentConfiguration = paymentConfiguration.Value;
        }

        public async Task<PayForOrderStripeResultViewModel> PayOrderWithStripeAsync(int orderId, string jwt)
        {
            var result = new PayForOrderStripeResultViewModel();
            try
            {
                var body = new PayOrderStripeDto
                {
                    OrderId = orderId,
                    SuccessRedirectUrl = _paymentConfiguration.Stripe.SuccessRedirectUrl,
                    CancelRedirectUrl = _paymentConfiguration.Stripe.CancelRedirectUrl
                };

                var response = await PostAsync(_endpointConfiguration.PaymentOrderStripe, body, jwt: jwt);

                if (response.IsSuccessStatusCode)
                {
                    var raw = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<PayForOrderStripeResultViewModel>(raw);
                }
                else
                {
                    _logger.LogError(string.Format("Error: status code {0}", response.StatusCode));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return result;
        }
    }
}
