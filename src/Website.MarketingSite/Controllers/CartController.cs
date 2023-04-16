using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Website.MarketingSite.Controllers.Bases;
using Website.MarketingSite.Models.ViewModels.Cart;
using Website.MarketingSite.Services;

namespace Website.MarketingSite.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "Customer")]
    public class CartController : AppControllerBase
    {
        private readonly CartService _cartService;

        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("/my-cart")]
        public async Task<IActionResult> MyCart()
        {
            var jwt = GetAuthorizationJwt();
            var cart = await _cartService.GetCart(jwt);

            return View(cart);
        }

        [HttpGet("api/my-cart")]
        public async Task<IActionResult> GetMyCart()
        {
            var jwt = GetAuthorizationJwt();
            var cart = await _cartService.GetCart(jwt);

            return Ok(cart);
        }

        [HttpPost("api/add-to-cart")]
        public async Task<IActionResult> AddToCart([FromBody]AddToCartViewModel model)
        {
            var jwt = GetAuthorizationJwt();
            var cart = await _cartService.AddToCart(model, jwt);

            return Ok(cart);
        }

        [HttpPost("api/update-cart")]
        public async Task<IActionResult> AddToCart([FromBody]UpdateCartViewModel model)
        {
            var jwt = GetAuthorizationJwt();
            var cart = await _cartService.UpdateCart(model, jwt);

            return Ok(cart);
        }
    }
}
