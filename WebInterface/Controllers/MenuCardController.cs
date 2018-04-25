using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebInterface.Models;

using WebInterface.Models;

namespace WebInterface.Controllers
{

    public class MenuCardController : Controller
    {
        MenuCardDBContext ctx;
        Task DBCreationTask;
        public MenuCardController(MenuCardDBContext context)
        {
            ctx = context;
            DBCreationTask = ctx.Database.EnsureCreatedAsync();
        }

        public IActionResult AddGuest(int? tableNo)
        {
            if(tableNo == null)
            {
                return RedirectToAction("ErrorView");
            }
            //TODO: add guest here
            string guestCode = Guest.GenerateGuestCode(1);
            return RedirectToAction("Index", new GuestCodeWithModel<object>(null, guestCode));
        }

        public async Task<IActionResult> Index(string guestCode)
        {
            if (guestCode == null)
            {
                return RedirectToAction("ErrorView");
            }
            await DBCreationTask;
            return View("Index", new GuestCodeWithModel<object>(null, guestCode));
        }

        public async Task<IActionResult> Drinks(string guestCode)
        {
            

            if (guestCode == null)
            {
                return RedirectToAction("ErrorView");
            }
            await DBCreationTask;

            var dishtypes = ctx.DishTypes.ToList();
            
            var test = ctx.SubDishTypes.ToList();
            //var HotBeverages = DishType.getAllHotBeverages();
            //var ColdBeverages = DishType.getAllColdBeverages();
            //var HardDrinks = DishType.getAllHardDrinks();


            //drinks contains all drinks defined in the CLASS DISHTYPE

            var drinks = DishType.getAllDrinks();
          

            var result = drinks.ToList();
            return View(new GuestCodeWithModel<List<DishType>>(dishtypes, guestCode));           
        }

        public async Task<IActionResult> Starters(string guestCode)
        {
            if (guestCode == null)
            {
                return RedirectToAction("ErrorView");
            }
            await DBCreationTask;
            //starters contains all starters defined in the CLASS DISHTYPE



            var starters = DishType.getAllStarters();

            var result = starters.ToList();

            return View(new GuestCodeWithModel<List<DishType>>(result, guestCode));

        }

        public async Task<IActionResult> Mains(string guestCode)
        {
            if (guestCode == null)
            {
                return RedirectToAction("ErrorView");
            }
            await DBCreationTask;
            //mains contains all Mains defined in the CLASS DISHTYPE

            var mains = DishType.getAllMains();

            var result = mains.ToList();
            return View(new GuestCodeWithModel<List<DishType>>(result, guestCode));
        }

        public async Task<IActionResult> Desserts(string guestCode)
        {
            if (guestCode == null)
            {
                return RedirectToAction("ErrorView");
            }
            await DBCreationTask;
            //desserts contains all Desserts defined in the CLASS DISHTYPE

            var dessert = DishType.getAllDesserts();

            var result = dessert.ToList();
            return View(new GuestCodeWithModel<List<DishType>>(result, guestCode));
        }

        public async Task<IActionResult> OrderOverview(string guestCode)
        {
            if (guestCode == null)
            {
                return RedirectToAction("ErrorView");
            }
            await DBCreationTask;
            var drinks = DishType.getAllDrinks();
            var starters = DishType.getAllStarters();
            var mains = DishType.getAllMains();
            var dessert = DishType.getAllDesserts();

            var result = drinks.Concat(starters).Concat(mains).Concat(dessert).ToList();


            return View(new GuestCodeWithModel<List<DishType>>(result, guestCode));
        }

        public IActionResult ErrorView()
        {
            return View();
        }


        public IActionResult FinalizedOrder(string guestCode)
        {
            return View(new GuestCodeWithModel<object>(null, guestCode));
        }
    }
}