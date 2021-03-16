using System;

namespace StoreModels
{
    /// <summary>
    /// Inventory model stores the amount of a specific product available at a specific location
    /// </summary>
    public class Inventory
    {
        private int quantity;
        public int InventoryId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public int Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value;
            }
        }
    }
}
