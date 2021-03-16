using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StoreModels;
using Serilog;

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
        public Product GetProductById(int id)
        {
            return ctx.Products.Where(p => p.ProductId == id).Include(p => p.Inventories).ThenInclude(inv => inv.Location).FirstOrDefault();
        }
        public List<Location> GetLocations(Func<Location, bool> where)
        {
            return ctx.Locations.Select(location => location).Where(where).ToList();
        }
        public Location GetLocationById(int locationId)
        {
            return ctx.Locations.AsNoTracking().FirstOrDefault(location => location.LocationId == locationId);
        }
        public List<Order> GetOrders(Func<Order, bool> where)
        {
            return ctx.Orders.Include(order => order.orderItems).Include(order => order.Customer).AsNoTracking().Select(order => order).Where(where).ToList();
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
            using var log = new LoggerConfiguration()
                .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day, shared: true)
                .CreateLogger();
            try
            {
                ctx.Users.Add(user);
                ctx.SaveChanges();
            } 
            catch (Exception e)
            {
                log.Error($"DATALAYER DATABASE AddUser() SaveChanges() exception: {e.Message}");
                return false;
            }
            log.Information($"DATALAYER DATABASE Successfully added user (userId: {user.UserId})");
            return true;
        }
        public bool AddProduct(Product product)
        {
            using var log = new LoggerConfiguration()
                .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day, shared: true)
                .CreateLogger();
            try
            {
                ctx.Products.Add(product);
                ctx.SaveChanges();
            }
            catch (Exception e)
            {
                log.Error($"DATALAYER DATABASE AddProduct() SaveChanges() exception: {e.Message}");
                return false;
            }
            log.Information($"DATALAYER DATABASE Successfully created Product (productId: {product.ProductId})");
            return true;
        }
        public bool SetLocationInventory(int productId, int locationId, int n, bool delta)
        {
            using var log = new LoggerConfiguration()
                .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day, shared: true)
                .CreateLogger();
            Inventory inv = ctx.Inventories.FirstOrDefault(inv => inv.ProductId == productId && inv.LocationId == locationId);
            if (inv == null)
            {
                inv = new Inventory();
                inv.LocationId = locationId;
                inv.ProductId = productId;
                ctx.Inventories.Add(inv);
            }
            int oldQuantity = inv.Quantity;
            if (delta)
            {
                inv.Quantity = inv.Quantity + n;
            }
            else
            {
                inv.Quantity = n;
            }
            if (inv.Quantity < 0)
            {
                inv.Quantity = oldQuantity;
                log.Error($"DATALAYER Tried to set quantity to less than zero");
                return false;
            }
            try
            {
                ctx.SaveChanges();
            }
            catch (Exception e)
            {
                log.Error($"DATALAYER DATABASE SetLocationInventory() SaveChanges() exception: {e.Message}");
                return false;
            }
            log.Information($"DATALAYER DATABASE Successfully changed (productId {productId}) inventory to {inv.Quantity} at location (locationId {locationId})");
            return true;
        }
        public bool AddItemToCart(int userId, int productId, int locationId, int n, bool delta)
        {
            using var log = new LoggerConfiguration()
                .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day, shared: true)
                .CreateLogger();
            Order cart = ctx.Orders.Include(order => order.orderItems).FirstOrDefault(order => order.CheckoutTimestamp == null && order.UserId == userId);
            if (cart == null)
            {
                cart = createCart(userId);
            }
            OrderItem item = cart.orderItems.Find(item => item.ProductId == productId);
            if (item == null)
            {
                item = new OrderItem();
                item.LocationId = locationId;
                item.ProductId = productId;
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
                log.Error($"DATALAYER DATABASE AddItemToCart() SaveChanges() exception: {e.Message}");
                return false;
            }
            log.Information($"DATALAYER DATABASE Successfully added (productId {productId}) to cart (userId {userId})");
            return true;
        }
        public bool CheckOut(int userId)
        {
            using var log = new LoggerConfiguration()
                .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day, shared: true)
                .CreateLogger();
            Order cart = ctx.Orders.FirstOrDefault(order => order.CheckoutTimestamp == null && order.UserId == userId);
            if (cart == null)
            {
                log.Error($"DATALAYER Tried CheckOut() without a cart (userId {userId})");
                return false;
            }
            cart.CheckoutTimestamp = DateTime.Now;
            try
            {
                ctx.SaveChanges();
            }
            catch (Exception e)
            {
                log.Error($"DATALAYER DATABASE CheckOut() SaveChanges() exception: {e.Message}");
                Console.WriteLine("ERROR: Failed to check out");
                Console.WriteLine(e.StackTrace);
                return false;
            }
            log.Information($"DATALAYER DATABASE Successfully checked out (userId {userId})");
            return true;
        }
        public bool createUser(User user)
        {
            using var log = new LoggerConfiguration()
                .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day, shared: true)
                .CreateLogger();
            try
            {
                ctx.Users.Add(user);
                ctx.SaveChanges();
            }
            catch (Exception e)
            {
                log.Error($"DATALAYER DATABASE CreateUser() SaveChanges() exception: {e.Message}");
                Console.WriteLine("ERROR: Failed to create new user");
                Console.WriteLine(e.StackTrace);
                return false;
            }
            log.Information($"DATALAYER DATABASE Successfully created user (userId {user.UserId})");
            return true;
        }
        public Order createCart(int userId)
        {
            using var log = new LoggerConfiguration()
                .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day, shared: true)
                .CreateLogger();
            log.Information($"DATALAYER DATABASE Created cart for (userId {userId}) (Changes not saved until item is added");
            Order cart = new Order();
            cart.UserId = userId;
            cart.CheckoutTimestamp = null;
            cart.orderItems = new List<OrderItem>();
            ctx.Orders.Add(cart);
            return cart;
        }
    }
}
