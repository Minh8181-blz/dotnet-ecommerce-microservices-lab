using System.ComponentModel.DataAnnotations;

namespace API.Payment.ViewModels
{
    public class PayForOrderViewModel
    {
        public int OrderId { get; set; }
        [Required]
        public string SuccessRedirectUrl { get; set; }
        [Required]
        public string CancelRedirectUrl { get; set; }
    }
}
