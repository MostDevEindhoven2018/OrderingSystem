using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using WebInterface.Models;

namespace WebInterface.Controllers
{
    public class MenuCardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Drinks()
        {
            //var HotBeverages = DishType.getAllHotBeverages();
            //var ColdBeverages = DishType.getAllColdBeverages();
            //var HardDrinks = DishType.getAllHardDrinks();


            //drinks contains all drinks defined in the CLASS DISHTYPE

            var drinks = DishType.getAllDrinks();

            var result = drinks.ToList();

            return View(result);

        }

        public IActionResult Starters()
        {
            //starters contains all starters defined in the CLASS DISHTYPE

            var starters = DishType.getAllStarters();

            var result = starters.ToList();
            return View(result);
        }

        public IActionResult Mains()
        {
            //mains contains all Mains defined in the CLASS DISHTYPE

            var mains = DishType.getAllMains();

            var result = mains.ToList();
            return View(result);
        }

        public IActionResult Desserts()
        {
            //desserts contains all Desserts defined in the CLASS DISHTYPE

            var dessert = DishType.getAllDesserts();

            var result = dessert.ToList();
            return View(result);
        }

        public IActionResult OrderOverview()
        {
            return View();
        }
        public IActionResult FinalizedOrder()
        {
            return View();
        }
    }
}