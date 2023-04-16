using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Threading.Tasks;
using Website.MarketingSite.Consts;
using Website.MarketingSite.Extensions;
using Website.MarketingSite.Services;

namespace Website.MarketingSite.Middlewares.Common
{
    public class CheckAndRefreshTokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AuthService _authService;

        public CheckAndRefreshTokenMiddleware(RequestDelegate next, AuthService authService)
        {
            _next = next;
            _authService = authService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var isLoggedIn = context.User.IsLoggedIn();

            if (isLoggedIn)
            {
                var needTokenRefresh = false;

                if (context.Request.Cookies.TryGetValue(CookieKeys.AccessTokenExpireTime, out var accessTokenExpStr) &&
                    DateTime.TryParse(accessTokenExpStr, out var accessTokenExpTime))
                {
                    if ((accessTokenExpTime - DateTime.UtcNow).TotalSeconds < 300)
                        needTokenRefresh = true;
                }
                else
                    needTokenRefresh = true;

                if (needTokenRefresh)
                {
                    if (context.Request.Cookies.TryGetValue(CookieKeys.AccessTokenExpireTime, out var refreshToken))
                    {
                        var result = await _authService.GetRefreshToken(refreshToken);
                        await context.SignUserInAsync(result);
                    }
                    else
                        await context.SignUserOutAsync();
                }
            }

            var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
            var attribute = endpoint?.Metadata.GetMetadata<AuthorizeAttribute>();

            await _next(context);
        }
    }
}
