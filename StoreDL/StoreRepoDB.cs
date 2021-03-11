using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StoreModels;

namespace StoreDL
{
    /// <summary>
    /// Manages interaction with the database
    /// </summary>
    public class StoreRepoDB
    {
        protected readonly StoreContext ctx;
        public StoreRepoDB(StoreContext context)
        {
            ctx = context;
        }
        public List<User> GetUsers(Func<User, bool> where)
        {
            return ctx.Users.Select(user => user).Where(where).ToList();
        }
        public List<Product> GetProducts(Func<Product, bool> where)
        {
            return ctx.Products.Select(product => product).Where(where).ToList();
        }
        public List<Location> GetLocations(Func<Location, bool> where)
        {
            return ctx.Locations.Select(location => location).Where(where).ToList();
        }
        public List<Order> GetOrders(Func<Order, bool> where)
        {
            return ctx.Orders.Include("OrderItmes").Select(order => order).Where(where).ToList();
        }
        public List<Product> GetProductsAtLocation(Location location, Func<Product, bool> where)
        {
            return ctx.Inventories.Include("Product").Include("Location").Where(inv => inv.LocationId == location.LocationId && inv.Quantity > 0 && where(inv.Product)).Select(inv => inv.Product).ToList();
        }
        public List<Inventory> GetLocationsOfProduct(int productId, Func<Location, bool> where)
        {
            return ctx.Inventories.Include("Product").Include("Location").Where(inv => inv.ProductId == productId && inv.Quantity > 0 && where(inv.Location)).Select(inv => inv).ToList();
        }
        public int GetLocationInventory(Product product, Location location)
        {
            return ctx.Inventories.FirstOrDefault(inv => inv.ProductId == product.ProductId && inv.LocationId == location.LocationId).Quantity;
        }
        public bool AddUser(User user)
        {
            try
            {
                ctx.Users.Add(user);
                ctx.SaveChanges();
            } 
            catch (Exception e)
            {
                //TODO: Log failure
                Console.WriteLine("ERROR: Failed to create a new user");
                Console.WriteLine(e.StackTrace);
                return false;
            }
            // TODO: Log success
            return true;
        }
        public bool SetLocationInventory(Product product, Location location, int n, bool delta)
        {
            Inventory inv = ctx.Inventories.FirstOrDefault(inv => inv.ProductId == product.ProductId && inv.LocationId == location.LocationId);
            if (inv == null)
            {
                inv = new Inventory();
                inv.LocationId = location.LocationId;
                inv.ProductId = product.ProductId;
                inv.Quantity = 0;
                ctx.Inventories.Add(inv);
            }
            int oldQuantity = inv.Quantity;
            if (delta)
            {
                inv.Quantity += n;
            }
            else
            {
                inv.Quantity = n;
            }
            if (inv.Quantity < 0)
            {
                inv.Quantity = oldQuantity;
                // TODO: Log failure
                return false;
            }
            try
            {
                ctx.SaveChanges();
            }
            catch (Exception e)
            {
                //TODO: Log failure
                Console.WriteLine("ERROR: Failed to update the inventory");
                Console.WriteLine(e.StackTrace);
                return false;
            }
            // TODO: Log success
            return true;
        }
        public bool AddItemToCart(User user, Product product, Location location, int n, bool delta)
        {
            Order cart = ctx.Orders.Include("OrderItems").FirstOrDefault(order => order.CheckoutTimestamp == null && order.UserId == user.UserId);
            if (cart == null)
            {
                cart = createCart(user);
            }
            OrderItem item = cart.orderItems.Find(item => item.ProductId == product.ProductId);
            if (item == null)
            {
                item = new OrderItem();
                item.LocationId = location.LocationId;
                item.ProductId = product.ProductId;
                item.Quantity = 0;
            }
            if (delta)
            {
                item.Quantity += n;
            }
            else
            {
                item.Quantity = n;
            }
            cart.orderItems.Add(item);
            try
            {
                ctx.SaveChanges();
            }
            catch (Exception e)
            {
                //TODO: Log failure
                Console.WriteLine("ERROR: Failed to add item to cart");
                Console.WriteLine(e.StackTrace);
                return false;
            }
            // TODO: Log success
            return true;
        }
        public bool CheckOut(User user)
        {
            Order cart = ctx.Orders.FirstOrDefault(order => order.CheckoutTimestamp == null && order.UserId == user.UserId);
            if (cart == null)
            {
                // TODO: Log error
                return false;
            }
            cart.CheckoutTimestamp = DateTime.Now;
            try
            {
                ctx.SaveChanges();
            }
            catch (Exception e)
            {
                //TODO: Log failure
                Console.WriteLine("ERROR: Failed to check out");
                Console.WriteLine(e.StackTrace);
                return false;
            }
            // TODO: Log success
            return true;
        }
        public bool createUser(User user)
        {
            try
            {
                ctx.Users.Add(user);
                ctx.SaveChanges();
            }
            catch (Exception e)
            {
                // TODO: Log failure
                Console.WriteLine("ERROR: Failed to create new user");
                Console.WriteLine(e.StackTrace);
                return false;
            }
            return true;
        }
        private Order createCart(User user)
        {
            Order cart = new Order();
            cart.UserId = user.UserId;
            cart.CheckoutTimestamp = null;
            ctx.Orders.Add(cart);
            return cart;
        }
    }
}
