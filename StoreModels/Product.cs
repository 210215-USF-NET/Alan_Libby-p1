using System;
using System.Collections.Generic;

namespace StoreModels
{
    /// <summary>
    /// Product model stores details about a specific product that can be sold at locations
    /// </summary>
    public class Product
    {
        private string productName;
        public int ProductId { get; set; }
        public string ProductName
        {
            get { return productName; }
            set
            {
                if (value == null || value.Equals(""))
                {
                    throw new ArgumentNullException("Cannot set ProductName to null or empty.");
                }
                productName = value;
            }
        }
        public decimal ProductPrice { get; set; }
        public List<Inventory> Inventories { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
