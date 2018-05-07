﻿using System;
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


            //drinks contains all drinks defined in the CLASS DISHTYPE

            DishTypeViewModel dishTypeViewModel = new DishTypeViewModel();
            dishTypeViewModel.DishTypes = drinks;
            dishTypeViewModel.SubDishTypes = uniqueSubDrinks;


            return View(new GuestCodeWithModel<DishTypeViewModel>(dishTypeViewModel, guestCode));
        }

        [HttpPost]
        public IActionResult Drinks(IFormCollection col, string GuestCode)
        {
            List<DishType> drinks = ctx.DishTypes.Where(x => x.Course == CourseType.DRINK).ToList();

            ctx.Guests.ToList();
            ctx.Dishes.ToList();
            ctx.SubDishTypes.ToList();

            var orderList = ctx.Orders.ToList();

            foreach (var selectedDrinks in drinks)
            {
                int quantity = Convert.ToInt32(col[selectedDrinks.Name]);

                var uniqueOrderList = orderList.Where(x => x.Owner.Code == GuestCode).FirstOrDefault();

                if (uniqueOrderList == null)
                {
                    RedirectToAction("ErrorView");
                }

                for (int i = 0; i < quantity; i++)
                {
                    if (uniqueOrderList.Selected == null)
                    {
                        uniqueOrderList.Selected = new List<Dish>();
                    }
                    Dish dish = new Dish() { Course = selectedDrinks };
                    uniqueOrderList.Selected.Add(dish);
                    ctx.Dishes.Add(dish);
                    ctx.SaveChanges();

                }

            }

            //Order order = ctx.Orders.Where(x => x.Owner.Code == GuestCode).FirstOrDefault();
            //var a = order.Selected.Where(x => x.Course.Course == CourseType.DRINK).ToList();

            //Dictionary<DishType, int> output = new Dictionary<DishType, int>();

            //foreach (Dish d in a)
            //{
            //    bool check;
            //    check = output.ContainsKey(d.Course);

            //    if (check==false)
            //    {
            //        output[d.Course]++;
            //    }

            //}

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

            return View(new GuestCodeWithModel<DishTypeViewModel>(dishTypeViewModel, guestCode));

        }

        [HttpPost]
        public IActionResult Starters(IFormCollection col, string GuestCode)
        {
            List<DishType> starters = ctx.DishTypes.Where(x => x.Course == CourseType.STARTER).ToList();

            ctx.Guests.ToList();
            ctx.Dishes.ToList();
            ctx.SubDishTypes.ToList();

            var orderList = ctx.Orders.ToList();

            foreach (var selectedStarters in starters)
            {
                int quantity = Convert.ToInt32(col[selectedStarters.Name]);

                var uniqueOrderList = orderList.Where(x => x.Owner.Code == GuestCode).FirstOrDefault();

                if (uniqueOrderList == null)
                {
                    RedirectToAction("ErrorView");
                }

                for (int i = 0; i < quantity; i++)
                {
                    if (uniqueOrderList.Selected == null)
                    {
                        uniqueOrderList.Selected = new List<Dish>();
                    }
                    Dish dish = new Dish() { Course = selectedStarters };
                    uniqueOrderList.Selected.Add(dish);
                    ctx.Dishes.Add(dish);
                    ctx.SaveChanges();

                }

            }

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
            var uniqueSubMains = subMains.GroupBy(x => x.SubType).Select(x => x.FirstOrDefault()).ToList();


            DishTypeViewModel dishTypeViewModel = new DishTypeViewModel();
            dishTypeViewModel.DishTypes = mains;
            dishTypeViewModel.SubDishTypes = uniqueSubMains;

            return View(new GuestCodeWithModel<DishTypeViewModel>(dishTypeViewModel, guestCode));
        }

        [HttpPost]
        public IActionResult Mains(IFormCollection col, string GuestCode)
        {
            List<DishType> mains = ctx.DishTypes.Where(x => x.Course == CourseType.MAINCOURSE).ToList();

            ctx.Guests.ToList();
            ctx.Dishes.ToList();
            ctx.SubDishTypes.ToList();

            var orderList = ctx.Orders.ToList();

            foreach (var selectedMains in mains)
            {
                int quantity = Convert.ToInt32(col[selectedMains.Name]);

                var uniqueOrderList = orderList.Where(x => x.Owner.Code == GuestCode).FirstOrDefault();

                if (uniqueOrderList == null)
                {
                    RedirectToAction("ErrorView");
                }

                for (int i = 0; i < quantity; i++)
                {
                    if (uniqueOrderList.Selected == null)
                    {
                        uniqueOrderList.Selected = new List<Dish>();
                    }
                    Dish dish = new Dish() { Course = selectedMains };
                    uniqueOrderList.Selected.Add(dish);
                    ctx.Dishes.Add(dish);
                    ctx.SaveChanges();

                }

            }

            return RedirectToAction("OrderOverview", "MenuCard", new { guestCode = GuestCode });
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

        [HttpPost]
        public IActionResult Desserts(IFormCollection col, string GuestCode)
        {
            List<DishType> desserts = ctx.DishTypes.Where(x => x.Course == CourseType.DESSERT).ToList();

            ctx.Guests.ToList();
            ctx.Dishes.ToList();
            ctx.SubDishTypes.ToList();

            var orderList = ctx.Orders.ToList();

            foreach (var selectedDesserts in desserts)
            {
                int quantity = Convert.ToInt32(col[selectedDesserts.Name]);

                var uniqueOrderList = orderList.Where(x => x.Owner.Code == GuestCode).FirstOrDefault();

                if (uniqueOrderList == null)
                {
                    RedirectToAction("ErrorView");
                }

                for (int i = 0; i < quantity; i++)
                {
                    if (uniqueOrderList.Selected == null)
                    {
                        uniqueOrderList.Selected = new List<Dish>();
                    }
                    Dish dish = new Dish() { Course = selectedDesserts };
                    uniqueOrderList.Selected.Add(dish);
                    ctx.Dishes.Add(dish);
                    ctx.SaveChanges();

                }

            }

            return RedirectToAction("OrderOverview", "MenuCard", new { guestCode = GuestCode });

        }

        public async Task<IActionResult> OrderOverview(string guestCode)
        {
            if (guestCode == null)
            {
                return RedirectToAction("ErrorView");
            }

            await DBCreationTask;

            //creating cache for entity framework

            ctx.Orders.ToList();
            ctx.Guests.ToList();
            ctx.DishTypes.ToList();
            ctx.SubDishTypes.ToList();
            ctx.Dishes.ToList();

            List<Order> order = ctx.Orders.ToList();

            List<Dish> selectedOrderItems = null;

            foreach (var item in order)
            {
                if (item.Owner.Code == guestCode)
                {
                    selectedOrderItems = item.Selected.ToList();

                }

            }

            OrderDishTypeViewModel orderDishTypeViewModel = new OrderDishTypeViewModel
            {
                orderDishes = selectedOrderItems,

            };

            //return View();
            return View(new GuestCodeWithModel<OrderDishTypeViewModel>(orderDishTypeViewModel, guestCode));
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