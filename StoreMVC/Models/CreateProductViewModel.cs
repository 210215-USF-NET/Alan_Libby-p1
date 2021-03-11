using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StoreMVC.Models
{
    public class CreateProductViewModel
    {
        [DisplayName("Product Name")]
        [Required]
        public string ProductName { get; set; }

        [DisplayName("Price")]
        [Required]
        public decimal ProductPrice { get; set; }
    }
}
