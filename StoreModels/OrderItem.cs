using System;

namespace StoreModels
{
    public class OrderItem
    {
        private int quantity;
        public int OrderItemId { get; set; }
        public Product Product { get; set; }
        public Location Location { get; set; }
        public int Quantity { get { return quantity; }
            set {
                if (value <= 0) {
                    throw new ArgumentException($"Cannot set OrderItem.Quantity to <= zero. (Tried to set to {value})");
                }
            }
        }
    }
}
