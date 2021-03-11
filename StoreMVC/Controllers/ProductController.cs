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
