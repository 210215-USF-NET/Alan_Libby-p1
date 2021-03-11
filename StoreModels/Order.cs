using System;
using System.Collections.Generic;

namespace StoreModels
{
    /// <summary>
    /// Order model stores information about an order. Carts are implemented as Orders that have not yet been placed.
    /// </summary>
    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public User Customer { get; set; }
        public DateTime? CheckoutTimestamp { get; set; }
        public List<OrderItem> orderItems { get; set; }
    }
}
