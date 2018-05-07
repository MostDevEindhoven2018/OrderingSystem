using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebInterface.Models;
using System.Dynamic;
using WebInterface.Models.CombinedModels;
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

            var test = ctx.SubDishTypes.ToList();


            List<DishType> drinks = ctx.DishTypes.Where(x => x.Course == CourseType.DRINK).ToList();
            var subDrinks = drinks.Where(x=>x.SubDishType!=null).Select(x => x.SubDishType).ToList();
            List<SubDishType> uniqueSubDrinks = subDrinks.GroupBy(x => x.SubType).Select(x => x.FirstOrDefault()).ToList();

            //ViewBag.drinks = drinks;
            //ViewBag.uniqueSubDrinks = uniqueSubDrinks;

            //var drinks = ctx.DishTypes.ToList();
            //var test = ctx.SubDishTypes.ToList();
            //var HotBeverages = DishType.getAllHotBeverages();
            //var ColdBeverages = DishType.getAllColdBeverages();
            //var HardDrinks = DishType.getAllHardDrinks();


            //drinks contains all drinks defined in the CLASS DISHTYPE

            DishTypeViewModel dishTypeViewModel = new DishTypeViewModel();
            dishTypeViewModel.DishTypes = drinks;
            dishTypeViewModel.SubDishTypes = uniqueSubDrinks;

            return View(new GuestCodeWithModel<DishTypeViewModel>(dishTypeViewModel, guestCode));
        }



        public async Task<IActionResult> Starters(string guestCode)
        {
            if (guestCode == null)
            {
                return RedirectToAction("ErrorView");
            }
            await DBCreationTask;
            //starters contains all starters defined in the CLASS DISHTYPE

            //The (var test) fills the Entity Framework cache with subdishtypes,
            //or else Entity Framework throws NullReference
            var test = ctx.SubDishTypes.ToList();


            List<DishType> starters = ctx.DishTypes.Where(x => x.Course == CourseType.STARTER).ToList();
            var subStarters = starters.Where(x => x.SubDishType != null).Select(x => x.SubDishType).ToList();
            List<SubDishType> uniqueSubStarters = subStarters.GroupBy(x => x.SubType).Select(x => x.FirstOrDefault()).ToList();

            DishTypeViewModel dishTypeViewModel = new DishTypeViewModel();
            dishTypeViewModel.DishTypes = starters;
            dishTypeViewModel.SubDishTypes = uniqueSubStarters;


            //var starters = ctx.DishTypes.ToList();
            //var starters = DishType.getAllStarters();
            //var result = starters.ToList();

            return View(new GuestCodeWithModel<DishTypeViewModel>(dishTypeViewModel, guestCode));

        }

        public async Task<IActionResult> Mains(string guestCode)
        {
            if (guestCode == null)
            {
                return RedirectToAction("ErrorView");
            }
            await DBCreationTask;

            //The (var test) fills the Entity Framework cache with subdishtypes,
            //or else Entity Framework throws NullReference
            var test = ctx.SubDishTypes.ToList();


            List<DishType> mains = ctx.DishTypes.Where(x => x.Course == CourseType.MAINCOURSE).ToList();
            var subMains = mains.Where(x => x.SubDishType != null).Select(x => x.SubDishType).ToList();
            var uniqueMains = subMains.GroupBy(x => x.SubType).Select(x => x.FirstOrDefault()).ToList();
            
            
            DishTypeViewModel dishTypeViewModel = new DishTypeViewModel();
            dishTypeViewModel.DishTypes = mains;
            dishTypeViewModel.SubDishTypes = uniqueMains;


            //mains contains all Mains defined in the CLASS DISHTYPE
            //var mains = ctx.DishTypes.ToList();
            //var mains = DishType.getAllMains();

            //var result = mains.ToList();
            return View(new GuestCodeWithModel<DishTypeViewModel>(dishTypeViewModel, guestCode));
        }

        public async Task<IActionResult> Desserts(string guestCode)
        {
            if (guestCode == null)
            {
                return RedirectToAction("ErrorView");
            }
            await DBCreationTask;
            //desserts contains all Desserts defined in the CLASS DISHTYPE

            //The (var test) fills the Entity Framework cache with subdishtypes,
            //or else Entity Framework throws NullReference
            var test = ctx.SubDishTypes.ToList();


            List<DishType> desserts = ctx.DishTypes.Where(x => x.Course == CourseType.DESSERT).ToList();
            var subDesserts = desserts.Where(x => x.SubDishType != null).Select(x => x.SubDishType).ToList();
            List<SubDishType> uniqueDesserts = subDesserts.GroupBy(x => x.SubType).Select(x => x.FirstOrDefault()).ToList();


            DishTypeViewModel dishTypeViewModel = new DishTypeViewModel();
            dishTypeViewModel.DishTypes = desserts;
            dishTypeViewModel.SubDishTypes = uniqueDesserts;

            //var desserts = ctx.DishTypes.ToList();           

            //var dessert = DishType.getAllDesserts();

            //var result = dessert.ToList();
            return View(new GuestCodeWithModel<DishTypeViewModel>(dishTypeViewModel, guestCode));
        }

        public async Task<IActionResult> OrderOverview(string guestCode)
        {
            if (guestCode == null)
            {
                return RedirectToAction("ErrorView");
            }
            await DBCreationTask;

            var result = ctx.DishTypes.ToList();

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