using System;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShopManagementSystem.Models
{
    public class ForBillsUserDropDownModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
    }

    public class ForBillsOrderDropDownModel
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
    }

    public class BillsModel
    {
        public int? BillID { get; set; }

        [Required(ErrorMessage = "Bill Number is required.")]
        [StringLength(20,MinimumLength =6, ErrorMessage = "Bill Number must be a maximum of 20 characters long. and minimum 6")]
        public string BillNumber { get; set; }

        [Required(ErrorMessage = "Bill Date is required.")]
        public DateTime BillDate { get; set; }

        [Required(ErrorMessage = "Order ID is required.")]
        public int OrderID { get; set; }

        [Required(ErrorMessage = "Total Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total Amount must be greater than zero.")]
        public double TotalAmount { get; set; }

        [Required(ErrorMessage = "Discount is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Discount cannot be negative.")]
        public double Discount { get; set; }

        [Required(ErrorMessage = "Net Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Net Amount must be greater than zero.")]
        public double NetAmount { get; set; }
        public int UserID { get; set; }
    }
}
