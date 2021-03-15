using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StoreMVC.Models
{
    public class ShowProductViewModel
    {
        [DisplayName("Product Name")]
        [Required]
        public string ProductName { get; set; }

        [DisplayName("Price")]
        [Required]
        public decimal ProductPrice { get; set; }

        public int ProductId { get; set; }
        public List<InventoryViewModel> Inventories;
    }
}
