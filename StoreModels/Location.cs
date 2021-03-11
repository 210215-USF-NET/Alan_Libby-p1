using System;
using System.Collections.Generic;

namespace StoreModels
{
    /// <summary>
    /// Location model stores details for a specific location from which items can be purchased
    /// </summary>
    public class Location
    {
        private string locationName;
        public int LocationId { get; set; }
        public string LocationName { get { return locationName; }
            set {
                if (value == null || value.Equals("")) {
                    throw new ArgumentNullException("Cannot set LocationName to null or empty.");
                }
                locationName = value;
            }
        }
        public string LocationAddress { get; set; }
        public List<Inventory> Inventories { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
