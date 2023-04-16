using Grpc.Core;
using Grpc.Pricing.Protos;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Grpc.Pricing
{
    public class CalculateOrderPriceService : CalculateOrderPrice.CalculateOrderPriceBase
    {
        private readonly ILogger<CalculateOrderPriceService> _logger;
        public CalculateOrderPriceService(ILogger<CalculateOrderPriceService> logger)
        {
            _logger = logger;
        }

        public override Task<OrderPriceViewModel> Calculate(OrderViewModel request, ServerCallContext context)
        {
            return Task.FromResult(new OrderPriceViewModel
            {
                TotalPrice = request.Items.Sum(x => x.UnitPrice * x.Quantity)
            });
        }
    }
}
