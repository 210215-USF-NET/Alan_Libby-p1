using Microsoft.AspNetCore.Mvc;
using StoreModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreMVC.Controllers
{
    public class LocationController : Controller
    {
        private StoreBL.StoreBL storeBL;
        public LocationController(StoreBL.StoreBL storeBL)
        {
            this.storeBL = storeBL;
        }
        public IActionResult Index()
        {
            return View(storeBL.GetLocations());
        }
        public IActionResult Details(int id)
        {
            Location location = storeBL.GetLocationById(id);
            if (location == null) return NotFound();
            return View(location);
        }
    }
}
