using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreModels
{
    /// <summary>
    /// OrderItem model stores the amount of a specific product from a specific location that was or will be purchased in an Order
    /// </summary>
    public class OrderItem
    {
        private int quantity;
        public int OrderItemId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public int Quantity { get { return quantity; }
            set {
                if (value <= 0) {
                    throw new ArgumentException($"Cannot set OrderItem.Quantity to <= zero. (Tried to set to {value})");
                }
                quantity = value;
            }
        }

        [NotMapped]
        public decimal TotalPrice
        {
            get
            {
                if (Product == null) return 0;
                return quantity * Product.ProductPrice;
            }
        }
    }
}
