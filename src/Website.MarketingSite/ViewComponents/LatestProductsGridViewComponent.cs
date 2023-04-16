using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Website.MarketingSite.Services;

namespace Website.MarketingSite.ViewComponents
{
    public class LatestProductsGridViewComponent : ViewComponent
    {
        private readonly ProductService _productService;

        public LatestProductsGridViewComponent(ProductService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var products = await _productService.GetLatestProductsAsync();
            return View(products);
        }
    }
}
