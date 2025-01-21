using System.ComponentModel.DataAnnotations;

namespace CoffeeShopManagementSystem.Models
{

    public class ForCustomerUserDropDownModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
    }

    public class CustomerModel
    {
        public int? CustomerID { get; set; }

        [Required(ErrorMessage = "Customer Name is required.")]
        [StringLength(100,MinimumLength =4 ,ErrorMessage = "Customer Name must be a maximum of 100 characters long. and minimum 4")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Home Address is required.")]
        [StringLength(200, ErrorMessage = "Home Address must be a maximum of 200 characters long.")]
        public string HomeAddress { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mobile Number is required.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Mobile Number must be exactly 10 digits.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile Number must be numeric and exactly 10 digits.")]
        public string MobileNo { get; set; }

        [StringLength(15, ErrorMessage = "GST Number must be a maximum of 15 characters long.")]
        public string GST_NO { get; set; }

        [Required(ErrorMessage = "City Name is required.")]
        [StringLength(50, ErrorMessage = "City Name must be a maximum of 50 characters long.")]
        public string CityName { get; set; }

        [Required(ErrorMessage = "Pincode is required.")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Pincode must be exactly 6 digits.")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Pincode must be numeric and exactly 6 digits.")]
        public string Pincode { get; set; }

        [Required(ErrorMessage = "Net Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Net Amount must be greater than zero.")]
        public double NetAmount { get; set; }

        [Required(ErrorMessage = "User ID is required.")]
        public int UserID { get; set; }
    }
}
