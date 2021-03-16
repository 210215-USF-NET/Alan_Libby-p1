﻿using System;
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
        public bool AddProduct(Product product)
        {
            try
            {
                ctx.Products.Add(product);
                ctx.SaveChanges();
            }
            catch (Exception e)
            {
                //TODO: Log failure
                Console.WriteLine("ERROR: Failed to create a new product");
                Console.WriteLine(e.StackTrace);
                return false;
            }
            // TODO: Log success
            return true;
        }
        public bool SetLocationInventory(int productId, int locationId, int n, bool delta)
        {
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
                System.Diagnostics.Debug.WriteLine($"Inventory before change: {inv.Quantity}");
                //inv.Quantity += n;
                inv.Quantity = inv.Quantity + n;
                System.Diagnostics.Debug.WriteLine($"Inventory after change: {inv.Quantity}");
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
        public bool AddItemToCart(int userId, int productId, int locationId, int n, bool delta)
        {
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
                //item.Quantity = 0;
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
        public bool CheckOut(int userId)
        {
            Order cart = ctx.Orders.FirstOrDefault(order => order.CheckoutTimestamp == null && order.UserId == userId);
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
        public Order createCart(int userId)
        {
            Order cart = new Order();
            cart.UserId = userId;
            cart.CheckoutTimestamp = null;
            cart.orderItems = new List<OrderItem>();
            ctx.Orders.Add(cart);
            return cart;
        }
    }
}
