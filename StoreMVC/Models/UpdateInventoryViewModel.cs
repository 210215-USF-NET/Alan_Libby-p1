using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreMVC.Models
{
    public class UpdateInventoryViewModel
    {
        public int LocationId { get; set; }
        public int ProductId { get; set; }

        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Quantity should be positive!")]
        public int Quantity { get; set; }
    }
}
