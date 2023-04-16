using System.ComponentModel.DataAnnotations;

namespace API.Identity.ViewModels
{
    public class GetTokenViewModel
    {
        [MaxLength(50, ErrorMessage = "Email must not exceed 50 characters")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
