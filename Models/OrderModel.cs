using System;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShopManagementSystem.Models
{
    public class ForOrderUserDropDownModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
    }

    public class ForOrderCustomerDropDownModel
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
    }

    public class OrderModel
    {
        public int? OrderID { get; set; }

        [Required(ErrorMessage = "Order Date is required.")]
        public DateTime OrderDate { get; set; }

        public int CustomerID { get; set; }

        [Required(ErrorMessage = "Payment Mode is required.")]
        [StringLength(20,MinimumLength =4, ErrorMessage = "Payment Mode must be a maximum of 20 characters long. and minimum 4")]
        public string PaymentMode { get; set; }

        [Required(ErrorMessage = "Total Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total Amount must be greater than zero.")]
        public double TotalAmount { get; set; }

        [Required(ErrorMessage = "Shipping Address is required.")]
        [StringLength(200, ErrorMessage = "Shipping Address must be a maximum of 200 characters long.")]
        public string ShippingAddress { get; set; }

        public int UserID { get; set; }
    }
}
