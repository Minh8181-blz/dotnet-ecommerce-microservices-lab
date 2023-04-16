using API.Catalog.Application.Dto;
using MediatR;
using System.Collections.Generic;

namespace API.Catalog.Application.Queries
{
    public class GetProductsByIdsQuery : IRequest<IEnumerable<ProductDto>>
    {
        public GetProductsByIdsQuery(IEnumerable<int> ids)
        {
            Ids = ids;
        }

        public IEnumerable<int> Ids { get; set; }
    }
}
