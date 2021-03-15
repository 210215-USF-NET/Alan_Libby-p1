using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreMVC.Models
{
    public class InventoryViewModel
    {
        public LocationViewModel Location { get; set; }
        public int Quantity { get; set; }
    }
}
