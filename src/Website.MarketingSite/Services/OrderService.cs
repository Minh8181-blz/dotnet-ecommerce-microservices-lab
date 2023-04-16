using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Utilities.Pagination;
using Website.MarketingSite.Configurations;
using Website.MarketingSite.Models.ViewModels.Orders;

namespace Website.MarketingSite.Services
{
    public class OrderService : HttpServiceBase
    {
        private readonly ILogger<OrderService> _logger;
        private readonly ApiEndpointConfiguration _endpointConfiguration;

        public OrderService(HttpClient client, ApiEndpointConfiguration endpointConfiguration, ILogger<OrderService> logger) : base(client)
        {
            _endpointConfiguration = endpointConfiguration;

            Client.BaseAddress = new Uri(_endpointConfiguration.ApiOrigin);

            _logger = logger;
        }

        public async Task<PaginationDataModel<OrderViewModel>> GetMyOrders(int pageIndex, int pageSize, string jwt)
        {
            PaginationDataModel<OrderViewModel> data = new PaginationDataModel<OrderViewModel>();

            try
            {
                var query = new Dictionary<string, string>
                {
                    { "pageSize", pageSize.ToString() },
                    { "pageIndex", pageIndex.ToString() }
                };

                var response = await GetAsync(_endpointConfiguration.OrdersGetMyOrders, query, jwt: jwt);

                if (response.IsSuccessStatusCode)
                {
                    var raw = await response.Content.ReadAsStringAsync();
                    data = JsonConvert.DeserializeObject<PaginationDataModel<OrderViewModel>>(raw);
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

            return data;
        }

        public async Task<OrderViewModel> GetOrderDetails(int id, string jwt)
        {
            OrderViewModel order = null;

            try
            {
                string url = string.Format(_endpointConfiguration.OrdersGetOrderDetails, id);
                var response = await GetAsync(url, jwt: jwt);

                if (response.IsSuccessStatusCode)
                {
                    var raw = await response.Content.ReadAsStringAsync();
                    order = JsonConvert.DeserializeObject<OrderViewModel>(raw);
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

            return order;
        }
    }
}
