using System.ComponentModel.DataAnnotations;

namespace CoffeeShopManagementSystem.Models
{
    public class ForOrderDetailUserDropDownModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
    }

    public class ForOrderDetailProductDropDownModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
    }

    public class ForOrderDetailOrderDropDownModel
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
    }

    public class OrderDetailModel
    {
        public int? OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public double Amount { get; set; }

        [Required(ErrorMessage = "Total Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total Amount must be greater than zero.")]
        public double TotalAmount { get; set; }

        public int UserID { get; set; }
    }
}
