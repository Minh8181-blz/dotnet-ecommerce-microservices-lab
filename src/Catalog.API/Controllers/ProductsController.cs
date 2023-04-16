using API.Catalog.Application.Commands;
using API.Catalog.Application.Dto;
using API.Catalog.Application.Queries;
using API.Catalog.ViewModels;
using Base.API.Filters.ActionFilters;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IEnumerable<ProductDto>> GetLatestProducts()
        {
            int limit = 12;
            var query = new GetLatestProductsQuery(limit);
            var products = await _mediator.Send(query);

            return products;
        }

        [HttpGet("get-by-ids")]
        public async Task<IEnumerable<ProductDto>> GetProductByIds([FromQuery] List<int> ids)
        {
            var query = new GetProductsByIdsQuery(ids);
            var products = await _mediator.Send(query);

            return products;
        }

        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult> CreateProduct(ProductCreationViewModel model)
        {
            var command = new CreateProductCommand(model.Name, model.Description, model.Price, model.StockQuantity);

            var product = await _mediator.Send(command);

            return Ok(product);
        }
    }
}
