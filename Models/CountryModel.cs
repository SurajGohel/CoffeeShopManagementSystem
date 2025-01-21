using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CoffeeShopManagementSystem.Models
{
    public class CountryModel
    {
        public int? CountryID { get; set; }

        [Required]
        [DisplayName("Country Name")]
        public string CountryName { get; set; }

        [Required]
        [DisplayName("Country Code")]
        public string CountryCode { get; set; }
    }
}
