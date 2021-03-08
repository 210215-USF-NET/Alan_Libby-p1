using System.Collections.Generic;

namespace StoreModels
{
    public class Order
    {
        public int OrderId { get; set; }
        public User Customer { get; set; }
        public List<OrderItem> orderItems { get; set; }
    }
}
