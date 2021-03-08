using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreModels
{
    public class Inventory
    {
        private int quantity;
        public int InventoryId { get; set; }
        public Product Product { get; set; }
        public Location Location { get; set; }
        public int Quantity
        {
            get { return quantity; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException($"Cannot set Inventory.Quantity to <= zero. (Tried to set to {value})");
                }
            }
        }
    }
}
