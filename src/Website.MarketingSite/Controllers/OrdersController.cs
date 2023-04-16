using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.Pagination;
using Website.MarketingSite.Controllers.Bases;
using Website.MarketingSite.Models.ViewModels.Orders;
using Website.MarketingSite.Services;

namespace Website.MarketingSite.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "Customer")]
    public class OrdersController : AppControllerBase
    {
        private readonly OrderService _orderService;

        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("api/my-orders")]
        public async Task<PaginationDataModel<OrderViewModel>> GetMyOrders(int pageSize, int pageIndex)
        {
            var jwt = GetAuthorizationJwt();
            return await _orderService.GetMyOrders(pageIndex, pageSize, jwt);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> OrderDetails(int id)
        {
            var jwt = GetAuthorizationJwt();
            var order = await _orderService.GetOrderDetails(id, jwt);

            if (order == null)
            {
                return RedirectToAction("Index");
            }

            return View(order);
        }

    }
}
