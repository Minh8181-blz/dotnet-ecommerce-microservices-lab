using API.Catalog.Application.DataAccess;
using API.Catalog.Application.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Catalog.Application.Queries
{
    public class GetProductsByIdsQueryHandler : IRequestHandler<GetProductsByIdsQuery, IEnumerable<ProductDto>>
    {
        private readonly IProductDao _productQueries;

        public GetProductsByIdsQueryHandler(IProductDao productQueries)
        {
            _productQueries = productQueries;
        }

        public async Task<IEnumerable<ProductDto>> Handle(GetProductsByIdsQuery request, CancellationToken cancellationToken)
        {
            var products = await _productQueries.GetProductsByIdsAsync(request.Ids.ToArray());

            if (products == null)
            {
                products = new List<ProductDto>();
            }

            return products;
        }
    }
}
