using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using StoreDL;
using StoreModels;
using System.Linq;

namespace StoreTests
{
    /// <summary>
    /// Test class for data layer methods
    /// </summary>
    public class TestStoreDL
    {
        private readonly DbContextOptions<StoreContext> options;
        public TestStoreDL()
        {
            options = new DbContextOptionsBuilder<StoreContext>().UseSqlite("Filename=Test.db").Options;
            Seed();
        }
        [Fact]
        public void GetUsersShouldGetAllUsers()
        {
            using (var ctx = new StoreContext(options))
            {
                StoreRepoDB repo = new StoreRepoDB(ctx);
                for (int i = 0; i < 5; i++)
                {
                    User testUser = new User();
                    testUser.UserName = $"Test{i}";
                    testUser.isManager = true;
                    ctx.Users.Add(testUser);
                }
                ctx.SaveChanges();
            }
            using (var assertCtx = new StoreContext(options))
            {
                StoreRepoDB repo = new StoreRepoDB(assertCtx);
                var users = repo.GetUsers(user => true);
                Assert.NotNull(users);
                Assert.Equal(5, users.Count);
            }
        }

        [Theory]
        [InlineData("Test")]    // alpha
        [InlineData("1234")]    // numeric
        [InlineData("[}(*/")]   // special
        [InlineData("A1=?")]    // mix
        public void AddUserShouldAddUser(string userName)
        {
            using (var createCtx = new StoreContext(options))
            {
                StoreRepoDB repo = new StoreRepoDB(createCtx);
                User testUser = new User();
                testUser.UserName = userName;
                testUser.isManager = true;
                repo.AddUser(testUser);
                createCtx.SaveChanges();
            }
            using (var assertCtx = new StoreContext(options))
            {
                var result = assertCtx.Users.FirstOrDefault(user => user.UserName == userName);
                Assert.NotNull(result);
                Assert.Equal(userName, result.UserName);
            }
        }

        [Fact]
        public void GetProductsShouldGetAllProducts()
        {
            using (var ctx = new StoreContext(options))
            {
                StoreRepoDB repo = new StoreRepoDB(ctx);
                for (int i = 0; i < 5; i++)
                {
                    Product testProduct = new Product();
                    testProduct.ProductName = $"Test{i}";
                    testProduct.ProductPrice = i;
                    ctx.Products.Add(testProduct);
                }
                ctx.SaveChanges();
            }
            using (var assertCtx = new StoreContext(options))
            {
                StoreRepoDB repo = new StoreRepoDB(assertCtx);
                var products = repo.GetProducts(product => true);
                Assert.NotNull(products);
                Assert.Equal(5, products.Count);
            }
        }

        [Fact]
        public void GetProductByIdShouldGetCorrectProduct()
        {
            using (var ctx = new StoreContext(options))
            {
                StoreRepoDB repo = new StoreRepoDB(ctx);
                var product = repo.GetProductById(3);
                Assert.Null(product);
                for (int i = 0; i < 5; i++)
                {
                    Product testProduct = new Product();
                    testProduct.ProductName = $"Test{i}";
                    testProduct.ProductPrice = i;
                    ctx.Products.Add(testProduct);
                }
                ctx.SaveChanges();
            }
            using (var assertCtx = new StoreContext(options))
            {
                StoreRepoDB repo = new StoreRepoDB(assertCtx);
                var product = repo.GetProductById(3);
                Assert.NotNull(product);
                Assert.Equal(3, product.ProductId);
            }
        }

        [Theory]
        [InlineData("Test")]    // alpha
        [InlineData("1234")]    // numeric
        [InlineData("[}(*/")]   // special
        [InlineData("A1=?")]    // mix
        public void AddProductShouldAddProduct(string productName)
        {
            using (var createCtx = new StoreContext(options))
            {
                StoreRepoDB repo = new StoreRepoDB(createCtx);
                Product testProduct = new Product();
                testProduct.ProductName = productName;
                testProduct.ProductPrice = 1;
                repo.AddProduct(testProduct);
                createCtx.SaveChanges();
            }
            using (var assertCtx = new StoreContext(options))
            {
                var result = assertCtx.Products.FirstOrDefault(user => user.ProductName == productName);
                Assert.NotNull(result);
                Assert.Equal(productName, result.ProductName);
            }
        }

        [Fact]
        public void GetLocationsShouldGetAllLocations()
        {
            using (var ctx = new StoreContext(options))
            {
                StoreRepoDB repo = new StoreRepoDB(ctx);
                for (int i = 0; i < 5; i++)
                {
                    Location testLocation = new Location();
                    testLocation.LocationName = $"Test{i}";
                    testLocation.LocationAddress = "asdf";
                    ctx.Locations.Add(testLocation);
                }
                ctx.SaveChanges();
            }
            using (var assertCtx = new StoreContext(options))
            {
                StoreRepoDB repo = new StoreRepoDB(assertCtx);
                var locations = repo.GetLocations(location => true);
                Assert.NotNull(locations);
                Assert.Equal(5, locations.Count);
            }
        }

        [Fact]
        public void GetLocationByIdShouldGetCorrectLocation()
        {
            using (var ctx = new StoreContext(options))
            {
                StoreRepoDB repo = new StoreRepoDB(ctx);
                var location = repo.GetLocationById(3);
                Assert.Null(location);
                for (int i = 0; i < 5; i++)
                {
                    Location testLocation = new Location();
                    testLocation.LocationName = $"Test{i}";
                    testLocation.LocationAddress = "asdf";
                    ctx.Locations.Add(testLocation);
                }
                ctx.SaveChanges();
            }
            using (var assertCtx = new StoreContext(options))
            {
                StoreRepoDB repo = new StoreRepoDB(assertCtx);
                var location = repo.GetLocationById(3);
                Assert.NotNull(location);
                Assert.Equal(3, location.LocationId);
            }
        }

        [Fact]
        public void GetOrdersShouldGetAllOrders()
        {
            using (var ctx = new StoreContext(options))
            {
                StoreRepoDB repo = new StoreRepoDB(ctx);
                User user = new User { isManager = false, UserName = "asdf" };
                ctx.Users.Add(user);
                ctx.SaveChanges();
                for (int i = 0; i < 5; i++)
                {
                    Order testOrder = new Order();
                    testOrder.UserId = user.UserId;
                    ctx.Orders.Add(testOrder);
                }
                ctx.SaveChanges();
            }
            using (var assertCtx = new StoreContext(options))
            {
                StoreRepoDB repo = new StoreRepoDB(assertCtx);
                var orders = repo.GetOrders(order => true);
                Assert.NotNull(orders);
                Assert.Equal(5, orders.Count);
            }
        }

        [Fact]
        public void GetLocationsOfProductShouldWork()
        {
            Product prod = new Product { ProductName = "testprod", ProductPrice = 1 };
            using (var ctx = new StoreContext(options))
            {
                StoreRepoDB repo = new StoreRepoDB(ctx);
                ctx.Products.Add(prod);
                ctx.SaveChanges();
                for (int i = 0; i < 5; i++)
                {
                    Location loc = new Location { LocationName = $"testloc{i}", LocationAddress = "asdf" };
                    ctx.Locations.Add(loc);
                    ctx.SaveChanges();
                    Inventory inv = new Inventory { ProductId = prod.ProductId, LocationId = loc.LocationId, Quantity = i + 1 };
                    ctx.Inventories.Add(inv);
                }
                ctx.SaveChanges();
            }
            using (var assertCtx = new StoreContext(options))
            {
                StoreRepoDB repo = new StoreRepoDB(assertCtx);
                var inventories = repo.GetLocationsOfProduct(prod.ProductId, location => true);
                Assert.NotNull(inventories);
                Assert.Equal(5, inventories.Count);
            }
        }

        [Fact]
        public void SetLocationInventoryShouldWorkDelta()
        {
            Product prod = new Product { ProductName = "testprod", ProductPrice = 1 };
            Location loc = new Location { LocationName = "testloc", LocationAddress = "asdf" };
            using (var ctx = new StoreContext(options))
            {
                StoreRepoDB repo = new StoreRepoDB(ctx);
                ctx.Products.Add(prod);
                ctx.Locations.Add(loc);
                ctx.SaveChanges();
                Inventory inv = new Inventory { ProductId = prod.ProductId, LocationId = loc.LocationId, Quantity = 0 };
                ctx.Inventories.Add(inv);
                repo.SetLocationInventory(prod.ProductId, loc.LocationId, 10, true);
                ctx.SaveChanges();
            }
            using (var assertCtx = new StoreContext(options))
            {
                StoreRepoDB repo = new StoreRepoDB(assertCtx);
                var inv = assertCtx.Inventories.FirstOrDefault(inv => inv.ProductId == prod.ProductId && inv.LocationId == loc.LocationId);
                Assert.NotNull(inv);
                Assert.Equal(10, inv.Quantity);
            }
        }

        [Fact]
        public void SetLocationInventoryNegativeShouldNotWorkDelta()
        {
            Product prod = new Product { ProductName = "testprod", ProductPrice = 1 };
            Location loc = new Location { LocationName = "testloc", LocationAddress = "asdf" };
            using (var ctx = new StoreContext(options))
            {
                StoreRepoDB repo = new StoreRepoDB(ctx);
                ctx.Products.Add(prod);
                ctx.Locations.Add(loc);
                ctx.SaveChanges();
                Inventory inv = new Inventory { ProductId = prod.ProductId, LocationId = loc.LocationId, Quantity = 0 };
                ctx.Inventories.Add(inv);
                repo.SetLocationInventory(prod.ProductId, loc.LocationId, -10, true);
                ctx.SaveChanges();
            }
            using (var assertCtx = new StoreContext(options))
            {
                StoreRepoDB repo = new StoreRepoDB(assertCtx);
                var inv = assertCtx.Inventories.FirstOrDefault(inv => inv.ProductId == prod.ProductId && inv.LocationId == loc.LocationId);
                Assert.NotNull(inv);
                Assert.Equal(0, inv.Quantity);
            }
        }

        [Fact]
        public void AddItemToCartShouldAddItemIfNotExists()
        {
            Product prod = new Product { ProductName = "testprod", ProductPrice = 1 };
            Location loc = new Location { LocationName = "testloc", LocationAddress = "asdf" };
            User user = new User { UserName = "testuser", isManager = false };
            using (var ctx = new StoreContext(options))
            {
                StoreRepoDB repo = new StoreRepoDB(ctx);
                ctx.Products.Add(prod);
                ctx.Locations.Add(loc);
                ctx.Users.Add(user);
                ctx.SaveChanges();
                Order cart = new Order { UserId = user.UserId, CheckoutTimestamp = null };
                ctx.SaveChanges();
                repo.AddItemToCart(user.UserId, prod.ProductId, loc.LocationId, 5, true);
                ctx.SaveChanges();
            }
            using (var assertCtx = new StoreContext(options))
            {
                StoreRepoDB repo = new StoreRepoDB(assertCtx);
                var cart = assertCtx.Orders.Include(order => order.orderItems).FirstOrDefault(order => order.UserId == user.UserId && order.CheckoutTimestamp == null);
                Assert.NotNull(cart);
                Assert.Single(cart.orderItems);
                Assert.Equal(5, cart.orderItems[0].Quantity);
            }
        }

        [Fact]
        public void AddItemToCartShouldCoalesceIfExists()
        {
            Product prod = new Product { ProductName = "testprod", ProductPrice = 1 };
            Location loc = new Location { LocationName = "testloc", LocationAddress = "asdf" };
            User user = new User { UserName = "testuser", isManager = false };
            using (var ctx = new StoreContext(options))
            {
                StoreRepoDB repo = new StoreRepoDB(ctx);
                ctx.Products.Add(prod);
                ctx.Locations.Add(loc);
                ctx.Users.Add(user);
                ctx.SaveChanges();
                Order cart = new Order { UserId = user.UserId, CheckoutTimestamp = null };
                ctx.SaveChanges();
                repo.AddItemToCart(user.UserId, prod.ProductId, loc.LocationId, 5, true);
                ctx.SaveChanges();
                repo.AddItemToCart(user.UserId, prod.ProductId, loc.LocationId, 5, true);
                ctx.SaveChanges();
            }
            using (var assertCtx = new StoreContext(options))
            {
                StoreRepoDB repo = new StoreRepoDB(assertCtx);
                var cart = assertCtx.Orders.Include(order => order.orderItems).FirstOrDefault(order => order.UserId == user.UserId && order.CheckoutTimestamp == null);
                Assert.NotNull(cart);
                Assert.Single(cart.orderItems);
                Assert.Equal(10, cart.orderItems[0].Quantity);
            }
        }

        [Fact]
        public void CheckOutShouldApplyTimestamp()
        {
            Product prod = new Product { ProductName = "testprod", ProductPrice = 1 };
            Location loc = new Location { LocationName = "testloc", LocationAddress = "asdf" };
            User user = new User { UserName = "testuser", isManager = false };
            DateTime before = DateTime.Now;
            using (var ctx = new StoreContext(options))
            {
                StoreRepoDB repo = new StoreRepoDB(ctx);
                ctx.Products.Add(prod);
                ctx.Locations.Add(loc);
                ctx.Users.Add(user);
                ctx.SaveChanges();
                Order cart = new Order { UserId = user.UserId, CheckoutTimestamp = null };
                ctx.SaveChanges();
                repo.AddItemToCart(user.UserId, prod.ProductId, loc.LocationId, 5, true);
                ctx.SaveChanges();
                repo.CheckOut(user.UserId);
                ctx.SaveChanges();
            }
            using (var assertCtx = new StoreContext(options))
            {
                StoreRepoDB repo = new StoreRepoDB(assertCtx);
                var cart = assertCtx.Orders.Include(order => order.orderItems).FirstOrDefault(order => order.UserId == user.UserId);
                Assert.NotNull(cart);
                Assert.NotNull(cart.CheckoutTimestamp);
                DateTime after = DateTime.Now;
                Assert.True(before <= cart.CheckoutTimestamp);
                Assert.True(cart.CheckoutTimestamp <= after);
            }
        }

        [Fact]
        public void CreateCartShouldMakeNewOrder()
        {
            Product prod = new Product { ProductName = "testprod", ProductPrice = 1 };
            Location loc = new Location { LocationName = "testloc", LocationAddress = "asdf" };
            User user = new User { UserName = "testuser", isManager = false };
            DateTime before = DateTime.Now;
            using (var ctx = new StoreContext(options))
            {
                StoreRepoDB repo = new StoreRepoDB(ctx);
                ctx.Users.Add(user);
                ctx.SaveChanges();
            }
            using (var assertCtx = new StoreContext(options))
            {
                StoreRepoDB repo = new StoreRepoDB(assertCtx);
                var cart = assertCtx.Orders.Include(order => order.orderItems).FirstOrDefault(order => order.UserId == user.UserId && order.CheckoutTimestamp == null);
                Assert.Null(cart);
                var newCart = repo.createCart(user.UserId);
                Assert.NotNull(newCart);
                Assert.Equal(user.UserId, newCart.UserId);
            }
        }

        private void Seed()
        {
            using (var ctx = new StoreContext(options))
            {
                ctx.Database.EnsureDeleted();
                ctx.Database.EnsureCreated();
            }
        }
    }
}
