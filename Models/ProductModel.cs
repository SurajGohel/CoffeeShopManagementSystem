using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CoffeeShopManagementSystem.Models
{
    public class ForProductUserDropDownModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
    }

    public class ProductModel
    {
        public int? ProductID { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 50 characters.")]
        [RegularExpression(@"^[a-zA-Z\s'-]+$", ErrorMessage = "Name can only contain letters, spaces, hyphens, and apostrophes.")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public double ProductPrice { get; set; }

        [Required(ErrorMessage = "Product Code is required.")]
        [StringLength(10, MinimumLength = 4, ErrorMessage = "Product Code must be between 5 and 10 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Product Code can only contain letters, numbers")]
        public string ProductCode { get; set; }

        [Required(ErrorMessage ="Cannot be empty")]
        public string Description { get; set; }

        [Required]
        public int UserID { get; set; }
    }
}
