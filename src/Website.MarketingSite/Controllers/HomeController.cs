using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Website.MarketingSite.Controllers.Bases;
using Website.MarketingSite.Models;

namespace Website.MarketingSite.Controllers
{
    public class HomeController : AppControllerBase
    {

        public HomeController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
