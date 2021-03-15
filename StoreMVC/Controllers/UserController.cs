using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreModels;
using StoreMVC.Models;

namespace StoreMVC.Controllers
{
    public class UserController : Controller
    {
        private StoreBL.StoreBL storeBL;
        public UserController(StoreBL.StoreBL storeBL)
        {
            this.storeBL = storeBL;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        public IActionResult Create(CreateUserViewModel userVM)
        {
            if (ModelState.IsValid)
            {
                User user = new User();
                user.UserName = userVM.UserName;
                user.isManager = userVM.IsManager;
                string blOutput = storeBL.CreateUser(user);
                if (blOutput == null)
                {
                    return RedirectToAction("Login");
                }
                return BadRequest(blOutput);
            }
            return BadRequest("Invalid model state");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        public IActionResult Login(LoginUserViewModel userVM)
        {
            if (ModelState.IsValid)
            {
                User user = storeBL.GetUserByName(userVM.UserName);
                if (user == null)
                {
                    return NotFound();
                }
                HttpContext.Session.SetString("UserName", user.UserName);
                HttpContext.Session.SetInt32("UserId", user.UserId);
                HttpContext.Session.SetInt32("IsManager", user.isManager ? 1 : 0);
                return Redirect("/");
            }
            return BadRequest("Invalid model state");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserName");
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("IsManager");
            return RedirectToAction("Login");
        }
    }
}
