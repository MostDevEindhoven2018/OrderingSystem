using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using WebInterface.Models;
using System.Dynamic;
using WebInterface.Models.CombinedModels;
using Microsoft.EntityFrameworkCore;

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
            if (tableNo == null)
            {
                return RedirectToAction("ErrorView");
            }

            //
            DBCreationTask.Wait();

            //TODO: add guest here

            //var guestID = ctx.Guests.Select(x => x.GuestID).ToList();
            //if (guestID.Count > 0)
            //{
            //    for (int i = 0; i < guestID.Count; i++)
            //    {
            //        ctx.Guests.Add(new Guest { Code = Guest.GenerateGuestCode(guestID[i]) });
            //        ctx.SaveChanges();
            //    }

            //}

            

            //Created new object for a guest

            Guest g = new Guest();
            ctx.Guests.Add(g);
            ctx.SaveChanges();
            string guestCode = Guest.GenerateGuestCode(g.GuestID);
            g.Code = guestCode;

            ctx.Guests.ToList();

            //created new order object that has a owner for above guest object
            ctx.Orders.Add(new Order() { Owner = g });            
            ctx.SaveChanges();

            ctx.Orders.ToList();

            //List<Group> listOfGroups = new List<Group>();

            //for (int i = 0; i < listOfGroups.Count; i++)
            //{
            //    for (int j = 0; j < listOfGroups[i].GroupSize; j++)
            //    {
            //        ctx.Guests.Add(new Guest() { Code=Guest.GenerateGuestCode()});
            //        ctx.SaveChanges;
            //    }

            //}


            //var guests = ctx.Guests.ToList();
            //for (int i = 0; i < guests.Count; i++)
            //{
            //    listOfGuests.Add(new Guest() { Code = Guest.GenerateGuestCode(guests[i].GuestID) });
            //    //Guest guest = new Guest() { Code = Guest.GenerateGuestCode(guests[i].GuestID) };
            //}

            //for (int i = 0; i < listOfGuests.Count; i++)
            //{
            //    return RedirectToAction("Index", new GuestCodeWithModel<object>(null, listOfGuests[i].Code));
            //}
            return RedirectToAction("Index", new { guestCode = guestCode });
            //return RedirectToAction("Index", new GuestCodeWithModel<object>(null, guestCode));

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
            var subDrinks = drinks.Where(x => x.SubDishType != null).Select(x => x.SubDishType).ToList();
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

        [HttpPost]
        public IActionResult Drinks(IFormCollection col, string GuestCode)
        {
            //var all = from c in ctx.Orders select c;
            //if (all.Count()>0) { ctx.Orders.RemoveRange(all); }            
            //ctx.SaveChanges();

            List<DishType> drinks = ctx.DishTypes.Where(x => x.Course == CourseType.DRINK).ToList();
            //List<Dish> drinks = ctx.Dishes.Where(x => x.Course == CourseType.DRINK).ToList();
            ctx.Guests.ToList();
            ctx.Dishes.ToList();
            ctx.SubDishTypes.ToList();

            var orderList = ctx.Orders.ToList();

            foreach (var selectedDrinks in drinks)
            {
                int quantity = Convert.ToInt32(col[selectedDrinks.Name]);

                for (int i = 0; i < quantity; i++)
                {
                    foreach (var item in orderList)
                    {
                        if (item.Owner.Code == GuestCode)
                        {
                            if (item.Selected == null)
                            {
                                Dish dish = new Dish() { Course = drinks[i] };
                                item.Selected = new List<Dish>() { dish };
                                ctx.Dishes.Add(dish);
                                ctx.SaveChanges();
                            }
                            else
                            {
                                Dish dish = new Dish() { Course = drinks[i] };
                                item.Selected.Add(dish);
                                ctx.Dishes.Add(dish);
                                ctx.SaveChanges();
                            }

                        }

                    }

                }  

            }

            return RedirectToAction("OrderOverview", "MenuCard", new { guestCode = GuestCode });
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

        [HttpPost]
        public IActionResult Starters(IFormCollection col, string GuestCode)
        {

            List<DishType> starters = ctx.DishTypes.Where(x => x.Course == CourseType.STARTER).ToList();

            //var all = from c in ctx.Orders select c;
            //ctx.Orders.RemoveRange(all);
            //ctx.SaveChanges();

            //for (int i = 0; i < starters.Count; i++)
            //{
            //    if (Convert.ToInt32(col[$"{starters[i].Name}"]) > 0)
            //    {
            //        Order newOrder = new Order() { orderDish = starters[i], quantity = Convert.ToInt32(col[$"{starters[i].Name}"]) };
            //        ctx.Orders.Add(newOrder);
            //        try
            //        {
            //            ctx.SaveChanges();
            //        }
            //        catch (Exception e)
            //        {
            //            Console.WriteLine(e);
            //            // Make some adjustments.
            //            // ...
            //            // Try again.
            //            ctx.SaveChanges();
            //        }
            //        //ab.Add(Convert.ToInt32(col[$"{drinks[i].Name}"]));

            //    }
            //}

            //string a = col["Cola"];

            return RedirectToAction("OrderOverview", "MenuCard", new { guestCode = GuestCode });
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

            Guest guest = ctx.Guests.Where(x => x.Code == guestCode).FirstOrDefault();

            if (guest == null)
            {
                return ErrorView();
            }
            Order order = ctx.Orders.Where(x => x.Owner == guest).FirstOrDefault();
            ctx.Dishes.ToList();
            if (order == null)
            {
                return ErrorView();
            }
            var selected = order.Selected;



            //return View(new GuestCodeWithModel<OrderDishTypeViewModel>(orderDishTypeViewModel, guestCode));

            return View();
            //return View(new GuestCodeWithModel<OrderDishTypeViewModel>(orderDishTypeViewModel, guestCode));
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