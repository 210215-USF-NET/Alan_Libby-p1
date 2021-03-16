using System;
using System.Collections.Generic;
using StoreDL;
using StoreModels;

namespace StoreBL
{
    public class StoreBL
    {
        private StoreRepoDB repo;
        public StoreBL(StoreRepoDB repo)
        {
            this.repo = repo;
        }
        public User GetUserByName(string userName)
        {
            List<User> users = repo.GetUsers(user => user.UserName.Equals(userName));
            if (users.Count == 0) return null;
            if (users.Count > 1) throw new Exception($"Multiple users with the same name: {userName}");
            return users[0];
        }
        public string CreateUser(User user)
        {
           if (user.UserName.Equals(""))
            {
                return "Validation error: cannot add a user with an empty name";
            }
           if (repo.GetUsers(u => u.UserName.Equals(user.UserName)).Count > 0)
            {
                return "Cannot add user: Name already taken";
            }
            if (!repo.AddUser(user))
            {
                return "Error: Database write failed";
            }
            return null;
        }
        public List<User> GetUsers()
        {
            return repo.GetUsers(user => true);
        }
        public string CreateProduct(Product product)
        {
            if (!repo.AddProduct(product))
            {
                return "Error: Database write failed";
            }
            return null;
        }
        public List<Product> GetProducts()
        {
            return repo.GetProducts(product => true);
        }
        public Product GetProductById(int id)
        {
            return repo.GetProductById(id);
        }
        public List<Location> GetLocations()
        {
            return repo.GetLocations(loc => true);
        }
        public Location GetLocationById(int id)
        {
            return repo.GetLocationById(id);
        }
        public List<Inventory> GetLocationsOfProduct(int productId)
        {
            return repo.GetLocationsOfProduct(productId, loc => true);
        }
        public bool AddItemToCart(int userId, int productId, int locationId, int n)
        {
            if (!repo.SetLocationInventory(productId, locationId, -n, true))
                return false;
            return repo.AddItemToCart(userId, productId, locationId, n, true);
        }
        public bool SetLocationInventory(int productId, int locationId, int n, bool delta)
        {
            return repo.SetLocationInventory(productId, locationId, n, delta);
        }
        //public bool SetLocationInventory(int productId, int locationId, int n, bool delta)
        //{
        //    return repo.SetLocationInventory(productId, locationId, n, delta);
        //}
        public List<Order> GetAllOrders()
        {
            List<Order> orders = repo.GetOrders(order => order.CheckoutTimestamp != null);
            foreach (Order order in orders)
            {
                foreach(OrderItem oi in order.orderItems)
                {
                    oi.Product = repo.GetProductById(oi.ProductId);
                    oi.Location = repo.GetLocationById(oi.LocationId);
                }
            }
            return orders;
        }
        public List<Order> GetUserOrders(int userId)
        {
            List<Order> orders = repo.GetOrders(order => order.UserId == userId && order.CheckoutTimestamp != null);
            foreach (Order order in orders)
            {
                foreach (OrderItem oi in order.orderItems)
                {
                    oi.Product = repo.GetProductById(oi.ProductId);
                    oi.Location = repo.GetLocationById(oi.LocationId);
                }
            }
            return orders;
        }
        public Order GetCart(int userId)
        {
            List<Order> orders = repo.GetOrders(order => order.UserId == userId && order.CheckoutTimestamp == null);
            if (orders.Count == 0)
            {
                return repo.createCart(userId);
            }
            foreach (OrderItem oi in orders[0].orderItems)
            {
                oi.Product = repo.GetProductById(oi.ProductId);
                oi.Location = repo.GetLocationById(oi.LocationId);
            }
            return orders[0];
        }
        public Order GetOrderById(int orderId)
        {
            List<Order> orders = repo.GetOrders(order => order.OrderId == orderId);
            if (orders.Count == 0) return null;
            foreach (OrderItem oi in orders[0].orderItems)
            {
                oi.Product = repo.GetProductById(oi.ProductId);
                oi.Location = repo.GetLocationById(oi.LocationId);
            }
            return orders[0];
        }
        public bool CheckOut(int userId)
        {
            return repo.CheckOut(userId);
        }
    }
}
