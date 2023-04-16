using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Website.MarketingSite.Consts;
using Website.MarketingSite.Models.Dtos;

namespace Website.MarketingSite.Extensions
{
    public static class HttpContextExtensions
    {
        public static async Task SignUserInAsync(this HttpContext context, LoginResultDto loginResult)
        {
            var token = loginResult.AccessToken;

            var handler = new JwtSecurityTokenHandler();
            var tokenData = handler.ReadJwtToken(token);

            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Id, tokenData.Claims.First(x => x.Type == JwtClaimTypes.Id).Value),
                new Claim(ClaimTypes.Email, tokenData.Claims.First(x => x.Type == JwtClaimTypes.Email).Value)
            };

            foreach (var claim in tokenData.Claims)
            {
                if (claim.Type == JwtClaimTypes.Role)
                    claims.Add(new Claim(ClaimTypes.Role, claim.Value));
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await context.SignInAsync(new ClaimsPrincipal(claimsIdentity));

            var tokenExpireTime = DateTime.UtcNow.AddSeconds(loginResult.TokenExpiresIn);

            context.Response.Cookies.Append(CookieKeys.AccessToken, token, new CookieOptions
            {
                Path = "/",
                HttpOnly = true,
                Expires = tokenExpireTime
            });

            context.Response.Cookies.Append(CookieKeys.RefreshToken, loginResult.RefreshToken, new CookieOptions
            {
                Path = "/",
                HttpOnly = true
            });

            context.Response.Cookies.Append(
                CookieKeys.AccessTokenExpireTime,
                tokenExpireTime.ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffffzzz"),
                new CookieOptions
                {
                    Path = "/"
                }
            );
        }

        public static async Task SignUserOutAsync(this HttpContext context)
        {
            await context.SignOutAsync();
            context.Response.Cookies.Delete(CookieKeys.AccessToken);
            context.Response.Cookies.Delete(CookieKeys.RefreshToken);
            context.Response.Cookies.Delete(CookieKeys.AccessTokenExpireTime);
        }
    }
}
