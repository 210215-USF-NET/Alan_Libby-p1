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
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UserName") == null) return Redirect("/User/Login");
            return View(HttpContext.Session.GetInt32("IsManager") == 0 ? storeBL.GetUserOrders((int)HttpContext.Session.GetInt32("UserId")) : storeBL.GetAllOrders());
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
