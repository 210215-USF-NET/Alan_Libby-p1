using System;

namespace StoreModels
{
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
    }
}
