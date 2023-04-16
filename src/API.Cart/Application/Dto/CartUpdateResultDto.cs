using Application.Base.SeedWork;

namespace API.Carts.Application.Dto
{
    public class CartUpdateResultDto : CommandResultModel
    {
        public CartDto Cart { get; set; }
    }
}
