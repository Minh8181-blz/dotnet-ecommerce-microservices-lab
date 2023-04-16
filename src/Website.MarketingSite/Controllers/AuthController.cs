using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Website.MarketingSite.Controllers.Bases;
using Website.MarketingSite.Extensions;
using Website.MarketingSite.Models.ViewModels.Auth;
using Website.MarketingSite.Services;

namespace Website.MarketingSite.Controllers
{
    public class AuthController : AppControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("/sign-up")]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost("/sign-up")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.SignUp(model);

                if (result.Succeeded)
                {
                    return Ok(result.User);
                }
                else
                {
                    return BadRequest(result.Message);
                }
            }

            var allErrors = GetModelErrors();

            return BadRequest(allErrors);
        }

        [HttpGet]
        [Authorize]
        public IActionResult TestCookie()
        {
            var user = User;
            return Ok(user.Claims.Select(x => x.Value));
        }

        [HttpGet]
        [Authorize(Roles = "Customer")]
        public IActionResult TestRole()
        {
            var user = User;
            return Ok(user.Claims.Select(x => x.Value));
        }

        [HttpGet("/login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("/login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.Login(model);

                if (result.Succeeded)
                {
                    await Request.HttpContext.SignUserInAsync(result);
                    return Ok();
                }
                else
                {
                    return BadRequest(result.Message);
                }
            }

            var allErrors = GetModelErrors();

            return BadRequest(allErrors);
        }

        [HttpGet("/refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            if (!Request.Cookies.TryGetValue("refresh_token", out string refreshToken))
                return BadRequest();

            var result = await _authService.GetRefreshToken(refreshToken);

            if (result.Succeeded)
            {
                await Request.HttpContext.SignUserInAsync(result);
                return Ok();
            }

            return BadRequest();
        }

        [Authorize]
        [HttpPost("/logout")]
        public async Task<IActionResult> LogOut()
        {
            await Request.HttpContext.SignUserOutAsync();
            return Redirect("/");
        }
    }
}
