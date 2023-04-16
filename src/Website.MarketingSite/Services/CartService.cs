using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Website.MarketingSite.Configurations;
using Website.MarketingSite.Models.ViewModels;
using Website.MarketingSite.Models.ViewModels.Cart;

namespace Website.MarketingSite.Services
{
    public class CartService : HttpServiceBase
    {
        private readonly ILogger<CartService> _logger;
        private readonly ApiEndpointConfiguration _endpointConfiguration;

        public CartService(HttpClient client, ApiEndpointConfiguration endpointConfiguration, ILogger<CartService> logger) : base(client)
        {
            _endpointConfiguration = endpointConfiguration;

            Client.BaseAddress = new Uri(_endpointConfiguration.ApiOrigin);

            _logger = logger;
        }

        public async Task<CartViewModel> GetCart(string jwt)
        {
            CartViewModel cart = null;

            try
            {
                var response = await GetAsync(_endpointConfiguration.CartGetMyCart, jwt: jwt);

                if (response.IsSuccessStatusCode)
                {
                    var raw = await response.Content.ReadAsStringAsync();
                    cart = JsonConvert.DeserializeObject<CartViewModel>(raw);
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

            return cart;
        }

        public async Task<CartUpdateResultViewModel> AddToCart(AddToCartViewModel model, string jwt)
        {
            CartUpdateResultViewModel result = null;

            try
            {
                var response = await PostAsync(_endpointConfiguration.CartAddToCart, model, jwt: jwt);

                if (response.IsSuccessStatusCode)
                {
                    var raw = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<CartUpdateResultViewModel>(raw);
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

        public async Task<CartUpdateResultViewModel> UpdateCart(UpdateCartViewModel model, string jwt)
        {
            CartUpdateResultViewModel result = null;

            try
            {
                var response = await PutAsync(_endpointConfiguration.CartUpdateCart, model, jwt: jwt);

                if (response.IsSuccessStatusCode)
                {
                    var raw = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<CartUpdateResultViewModel>(raw);
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

        public async Task<CheckoutInitResultViewModel> Checkout(string jwt)
        {
            CheckoutInitResultViewModel result = null;

            try
            {
                var response = await PostAsync(_endpointConfiguration.CartCheckout, null, jwt: jwt);

                if (response.IsSuccessStatusCode)
                {
                    var raw = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<CheckoutInitResultViewModel>(raw);
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
