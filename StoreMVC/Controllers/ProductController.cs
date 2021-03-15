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
    public class ProductController : Controller
    {
        private StoreBL.StoreBL storeBL;
        public ProductController(StoreBL.StoreBL storeBL)
        {
            this.storeBL = storeBL;
        }
        public IActionResult Index()
        {
            return View(storeBL.GetProducts().Select(product =>
            {
                ShowProductViewModel productVM = new ShowProductViewModel();
                productVM.ProductName = product.ProductName;
                productVM.ProductPrice = product.ProductPrice;
                productVM.ProductId = product.ProductId;
                return productVM;
            }));
        }

        public IActionResult Details(int id)
        {
            System.Diagnostics.Debug.WriteLine($"Details for product {id}");
            Product product = storeBL.GetProductById(id);
            if (product == null) return NotFound();
            ShowProductViewModel productVM = new ShowProductViewModel();
            productVM.ProductId = product.ProductId;
            productVM.ProductName = product.ProductName;
            productVM.ProductPrice = product.ProductPrice;
            productVM.Inventories = new List<InventoryViewModel>();
            foreach (Inventory inv in product.Inventories)
            {
                InventoryViewModel invVM = new InventoryViewModel();
                LocationViewModel locVM = new LocationViewModel();
                locVM.LocationId = inv.Location.LocationId;
                locVM.LocationName = inv.Location.LocationName;
                invVM.Location = locVM;
                invVM.Quantity = inv.Quantity;
                productVM.Inventories.Add(invVM);
            }
            return View(productVM);
        }

        [HttpGet]
        public IActionResult AddToCart(UpdateInventoryViewModel vm)
        {
            return View(vm);
        }

        [HttpPost]
        public IActionResult AddToCartPost(UpdateInventoryViewModel vm)
        {
            System.Diagnostics.Debug.WriteLine("Yeet");
            if (ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("Valid");
                if (HttpContext.Session.GetString("UserName") == null)
                    return Redirect("/User/Login");
                if (!storeBL.AddItemToCart((int)HttpContext.Session.GetInt32("UserId"), vm.ProductId, vm.LocationId, vm.Quantity))
                    return NotFound();
                return Redirect($"/Product/Details/{vm.ProductId}");
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        public IActionResult Create(CreateProductViewModel productVM)
        {
            if (ModelState.IsValid)
            {
                Product product = new Product();
                product.ProductName = productVM.ProductName;
                product.ProductPrice = productVM.ProductPrice;
                string blOutput = storeBL.CreateProduct(product);
                if (blOutput == null)
                {
                    return RedirectToAction("Index");
                }
                return BadRequest(blOutput);
            }
            return BadRequest("Invalid model state");
        }
    }
}
