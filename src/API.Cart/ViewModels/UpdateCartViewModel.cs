using API.Carts.Application.Enums;

namespace API.Carts.ViewModels
{
    public class UpdateCartViewModel
    {
        public int ProductId { get; set; }
        public CartAction Action { get; set; }
    }
}
