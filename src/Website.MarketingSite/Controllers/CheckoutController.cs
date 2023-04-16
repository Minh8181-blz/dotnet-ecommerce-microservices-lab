using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.MarketingSite.Controllers.Bases;
using Website.MarketingSite.Services;

namespace Website.MarketingSite.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "Customer")]
    public class CheckoutController : AppControllerBase
    {
        private readonly CartService _cartService;

        public CheckoutController(CartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost]
        public async Task<IActionResult> Checkout()
        {
            var jwt = GetAuthorizationJwt();
            var result = await _cartService.Checkout(jwt);

            return Ok(result);
        }
    }
}
