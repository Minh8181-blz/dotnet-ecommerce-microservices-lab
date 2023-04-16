using System.Linq;
using System.Security.Claims;
using Website.MarketingSite.Models.Dtos;

namespace Website.MarketingSite.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool IsLoggedIn(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal == null)
                return false;

           return claimsPrincipal.Identity.IsAuthenticated;
        }

        public static AppUser GetUser(this ClaimsPrincipal claimsPrincipal)
        {
            var user = new AppUser
            {
                Email = claimsPrincipal.Claims.First(x => x.Type == ClaimTypes.Email).Value
            };

            return user;
        }
    }
}
