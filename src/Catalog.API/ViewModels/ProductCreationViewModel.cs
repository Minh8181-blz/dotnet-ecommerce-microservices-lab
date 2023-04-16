using System.ComponentModel.DataAnnotations;

namespace API.Catalog.ViewModels
{
    public class ProductCreationViewModel
    {
        [Required(ErrorMessage = "Product name is required")]
        [RegularExpression("^[^{}*]+$", ErrorMessage = "Name must not contain special character(s)")]
        public string Name { get; set; }
        [RegularExpression("^[^{}*]+$", ErrorMessage = "Description must not contain special character(s)")]
        public string Description { get; set; }
        public decimal Price { get; set; }
        [Range(1, 300, ErrorMessage = "Stock quantity must be from 1 to 300")]
        public int StockQuantity { get; set; }
    }
}
