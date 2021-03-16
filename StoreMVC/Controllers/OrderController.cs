using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoreModels;

namespace StoreMVC.Controllers
{
    public class OrderController : Controller
    {
        private StoreBL.StoreBL storeBL;
        public OrderController(StoreBL.StoreBL storeBL)
        {
            this.storeBL = storeBL;
        }
        public IActionResult Index(string sortBy, int? sortDir)
        {
            if (sortBy == null) sortBy = "Time";
            if (sortDir == null) sortDir = 0;
            ViewBag.SortBy = sortBy;
            ViewBag.SortDir = sortDir;
            if (HttpContext.Session.GetString("UserName") == null) return Redirect("/User/Login");
            List<Order> orders = HttpContext.Session.GetInt32("IsManager") == 0 ? storeBL.GetUserOrders((int)HttpContext.Session.GetInt32("UserId")) : storeBL.GetAllOrders();
            if (sortBy.Equals("Time"))
                orders.Sort((o1, o2) => ((DateTime)o1.CheckoutTimestamp).CompareTo(o2.CheckoutTimestamp));
            else if (sortBy.Equals("Price"))
                orders.Sort((o1, o2) =>
                {
                    if (o1.TotalPrice == o2.TotalPrice) return 0;
                    if (o1.TotalPrice > o2.TotalPrice) return -1;
                    return 1;
                });
            if (sortDir == 1) orders.Reverse();
            return View(orders);
        }
        public IActionResult Details(int id)
        {
            Order order = storeBL.GetOrderById(id);
            if (order == null) return NotFound();
            return View(order);
        }
        public IActionResult Cart()
        {
            if (HttpContext.Session.GetString("UserName") == null) return Redirect("/User/Login");
            return View(storeBL.GetCart((int)HttpContext.Session.GetInt32("UserId")));
        }

        [HttpPost]
        public IActionResult Cart(Order order)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
                return Redirect("/User/Login");
            if (storeBL.CheckOut((int)HttpContext.Session.GetInt32("UserId")))
                return Redirect("/");
            throw new Exception("Failed to check out");
        }
    }
}
