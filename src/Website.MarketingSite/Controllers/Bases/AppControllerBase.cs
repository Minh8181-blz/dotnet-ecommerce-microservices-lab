using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Website.MarketingSite.Consts;

namespace Website.MarketingSite.Controllers.Bases
{
    public class AppControllerBase : Controller
    {
        public IEnumerable<object> GetModelErrors()
        {
            var errors = ModelState.Keys
                    .Where(k => ModelState[k].Errors.Count > 0)
                    .Select(k => new { propertyName = k, errorMessages = ModelState[k].Errors.Select(e => e.ErrorMessage) });

            return errors;
        }

        public string GetAuthorizationJwt()
        {
            Request.Cookies.TryGetValue(CookieKeys.AccessToken, out var jwt);
            return jwt;
        }
    }
}
