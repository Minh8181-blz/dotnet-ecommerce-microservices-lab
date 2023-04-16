using Application.Base.SeedWork;
using Ordering.API.Application.Dto;

namespace API.Ordering.Application.Dto
{
    public class OrderCreationResultDto : CommandResultModel
    {
        public OrderDto Order { get; set; }
    }
}
