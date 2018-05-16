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

        public async Task<IActionResult> AddGuest(int? tableNo)
        {
            if (tableNo == null)
            {
                return RedirectToAction("ErrorView");
            }

            await DBCreationTask;

            Guest g = new Guest();
            ctx.Guests.Add(g);
            // Save changes to obtain a GuestID
            await ctx.SaveChangesAsync();
            string guestCode = Guest.GenerateGuestCode(g.GuestID);
            g.Code = guestCode;

            //created new order object that has a owner for above guest object
            ctx.Orders.Add(new Order() { Owner = g });
            await ctx.SaveChangesAsync();

            return RedirectToAction("Index", new { guestCode });
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

            //collects all drinks DishType
            var drinks = ctx.DishTypes.Include(x => x.SubDishType).Where(x => x.Course == CourseType.DRINK);

            //collects the drinks SubDishType
            var subDrinks = drinks.Where(x => x.SubDishType != null).Select(x => x.SubDishType);

            //collects unique drinks from all drinks SubDishType
            List<SubDishType> uniqueSubDrinks = await subDrinks.GroupBy(x => x.SubType).Select(x => x.FirstOrDefault()).ToListAsync();

            //Dictionary that maps DishType and the quantity (DishType->int)

            Dictionary<DishType, int> output = new Dictionary<DishType, int>();

            //assigning all the drinks keyvalues to zero first
            foreach (DishType d in drinks)
            {
                output.Add(d, 0);
            }

            //Selects all orders belongs to unique guest
            Order order = await ctx.Orders.Include(x => x.Owner).Include(x => x.Selected).Where(x => x.Owner.Code == guestCode).FirstOrDefaultAsync();

            //If the selected orders by the guest is not null, then assign each drink value to guests order quantity using dictionary

            if (order.Selected != null)
            {
                //Loading the DishTypes table because of lazy entity framework
                //Include only works on single items. We dont know how to include fields of items in a list.
                //In this case: order.Selected.Course, Where Selected is a list of items that each contains a Course
                ctx.DishTypes.ToList();

                var a = order.Selected.Where(x => x.Course.Course == CourseType.DRINK);

                var b = a.GroupBy(x => x.Course.Name).Select(x => new { type = x.FirstOrDefault().Course, quantity = x.Count() });

                //assigning the drinks values (this replaces previous assigned zero values by guests order quantity for each drink)

                foreach (var item in b)
                {
                    output[item.type] = item.quantity;
                }
            }

            //drinks contains all drinks defined in the CLASS DISHTYPE
            DishTypeViewModel dishTypeViewModel = new DishTypeViewModel()
            {
                DishTypes = await drinks.ToListAsync(),
                SubDishTypes = uniqueSubDrinks,
                quantityDictionary = output
            };

            return View(new GuestCodeWithModel<DishTypeViewModel>(dishTypeViewModel, guestCode));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSelectedDishes(IFormCollection col, string GuestCode)
        {
            //1. Retrieve the order that we have to update
            Order order = await ctx.Orders.
                Include(x => x.Owner).Include(x => x.Selected).
                Where(x => x.Owner.Code == GuestCode).FirstOrDefaultAsync();

            if (order == null)
            {
                return new JsonResult(null);
            }

            if (order.Selected == null)
            {
                order.Selected = new List<Dish>();
            }

            //2. Retrieve the existing number of dishes per type
            Dictionary<DishType, int> existingCounts = new Dictionary<DishType, int>();

            List<DishType> types = ctx.DishTypes.ToList();

            foreach (DishType type in types)
            {
                existingCounts.Add(type, 0);
            }


            var existingCountList = order.Selected.GroupBy(x => x.Course.Name).
                Select(x => new { type = x.FirstOrDefault().Course, quantity = x.Count() });

            foreach (var item in existingCountList)
            {
                existingCounts[item.type] = item.quantity;
            }

            //3. Check input count against existing count
            foreach (string dishTypeName in col.Keys)
            {
                DishType type = types.Where(x => x.Name == dishTypeName).FirstOrDefault();
                if(type == null)
                {
                    //This should happen for the keys in col that are not a dishtype
                    continue;
                }

                int quantity;
                try
                {
                    quantity = Convert.ToInt32(col[dishTypeName]);
                } catch (Exception e)
                {
                    //Invalid user input
                    continue;
                }

                if(quantity < 0)
                {
                    continue;
                }

                if(quantity > 99)
                {
                    quantity = 99;
                }

                if (quantity > existingCounts[type])
                {

                    for (int i = 0; i < (quantity - existingCounts[type]); i++)
                    {
                        Dish dish = new Dish() { Course = type };
                        order.Selected.Add(dish);
                        ctx.Dishes.Add(dish);
                    }
                }
                else if (quantity < existingCounts[type])
                {
                    List<Dish> selectedOfType = new List<Dish>();

                    foreach(Dish dish in order.Selected)
                    {
                        if (dish.Course == type)
                        {
                            selectedOfType.Add(dish);
                        }
                    }

                    List<Dish> removeSelectedDrinks = selectedOfType.GetRange(0, (existingCounts[type] - quantity));
                    
                    foreach(Dish dish in removeSelectedDrinks)
                    {
                        order.Selected.Remove(dish);
                    }
                    ctx.Update(order);
                    ctx.Dishes.RemoveRange(removeSelectedDrinks);
                }
            }
            ctx.SaveChanges();
            return new JsonResult(null);
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

            Dictionary<DishType, int> output = new Dictionary<DishType, int>();

            foreach (DishType d in starters)
            {
                output.Add(d, 0);
            }

            ctx.Orders.ToList();

            Order order = ctx.Orders.Where(x => x.Owner.Code == guestCode).FirstOrDefault();
            ctx.Dishes.ToList();

            if (order.Selected != null)
            {
                ctx.DishTypes.ToList();
                List<Dish> a = order.Selected.Where(x => x.Course.Course == CourseType.STARTER).ToList();

                var b = a.GroupBy(x => x.Course.Name).Select(x => new { type = x.FirstOrDefault().Course, quantity = x.Count() }).ToList();

                foreach (var item in b)
                {
                    output[item.type] = item.quantity;
                }
            }

            DishTypeViewModel dishTypeViewModel = new DishTypeViewModel()
            {
                DishTypes = starters,
                SubDishTypes = uniqueSubStarters,
                quantityDictionary = output
            };



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
            var uniqueSubMains = subMains.GroupBy(x => x.SubType).Select(x => x.FirstOrDefault()).ToList();

            Dictionary<DishType, int> output = new Dictionary<DishType, int>();

            foreach (DishType d in mains)
            {
                output.Add(d, 0);
            }

            ctx.Orders.ToList();

            Order order = ctx.Orders.Where(x => x.Owner.Code == guestCode).FirstOrDefault();
            ctx.Dishes.ToList();

            if (order.Selected != null)
            {
                ctx.DishTypes.ToList();
                List<Dish> a = order.Selected.Where(x => x.Course.Course == CourseType.MAINCOURSE).ToList();

                var b = a.GroupBy(x => x.Course.Name).Select(x => new { type = x.FirstOrDefault().Course, quantity = x.Count() }).ToList();

                foreach (var item in b)
                {
                    output[item.type] = item.quantity;
                }
            }

            DishTypeViewModel dishTypeViewModel = new DishTypeViewModel()
            {
                DishTypes = mains,
                SubDishTypes = uniqueSubMains,
                quantityDictionary = output
            };

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

            Dictionary<DishType, int> output = new Dictionary<DishType, int>();

            foreach (DishType d in desserts)
            {
                output.Add(d, 0);
            }

            ctx.Orders.ToList();

            Order order = ctx.Orders.Where(x => x.Owner.Code == guestCode).FirstOrDefault();
            ctx.Dishes.ToList();

            if (order.Selected != null)
            {
                ctx.DishTypes.ToList();
                List<Dish> a = order.Selected.Where(x => x.Course.Course == CourseType.DESSERT).ToList();

                var b = a.GroupBy(x => x.Course.Name).Select(x => new { type = x.FirstOrDefault().Course, quantity = x.Count() }).ToList();

                foreach (var item in b)
                {
                    output[item.type] = item.quantity;
                }
            }

            DishTypeViewModel dishTypeViewModel = new DishTypeViewModel()
            {
                DishTypes = desserts,
                SubDishTypes = uniqueDesserts,
                quantityDictionary = output
            };

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

            //creating cache for entity framework

            ctx.Orders.ToList();
            ctx.Guests.ToList();
            ctx.DishTypes.ToList();
            ctx.SubDishTypes.ToList();
            ctx.Dishes.ToList();

            List<Order> order = ctx.Orders.ToList();

            List<Dish> selectedOrderItems = new List<Dish>();

            List<DishType> allDishtypes = ctx.DishTypes.ToList();
            var subDishes = allDishtypes.Where(x => x.SubDishType != null).Select(x => x.SubDishType).ToList();
            List<SubDishType> uniqueDishes = subDishes.GroupBy(x => x.SubType).Select(x => x.FirstOrDefault()).ToList();

            Dictionary<DishType, int> output = new Dictionary<DishType, int>();

            foreach (var d in allDishtypes)
            {
                output.Add(d, 0);
            }

            ctx.Orders.ToList();

            Order order1 = ctx.Orders.Where(x => x.Owner.Code == guestCode).FirstOrDefault();
            ctx.Dishes.ToList();

            if (order1.Finalized != null)
            {
                ctx.DishTypes.ToList();
                List<Dish> a = order1.Finalized.ToList();

                var b = a.GroupBy(x => x.Course.Name).Select(x => new { type = x.LastOrDefault().Course, quantity = x.Count() }).ToList();

                foreach (var item in b)
                {
                    output[item.type] = item.quantity;
                }
            }

            var test = output;


            foreach (var item in order)
            {
                if (item.Owner.Code == guestCode && item.Selected != null)
                {
                    selectedOrderItems = item.Selected.ToList();

                }

            }

            OrderDishTypeViewModel orderDishTypeViewModel = new OrderDishTypeViewModel
            {
                orderDishes = selectedOrderItems
            };

            return View(new GuestCodeWithModel<OrderDishTypeViewModel>(orderDishTypeViewModel, guestCode));
        }
        
        [HttpPost]
        public IActionResult FinalizesdOrderButton(IFormCollection col, string GuestCode)
        {
            Order order = ctx.Orders.Include(x => x.Selected).Include(x => x.Finalized).Where(x => x.Owner.Code == GuestCode).LastOrDefault();

            if (order == null)
            {
                RedirectToAction("ErrorView");
            }
            if (order.Selected != null)
            {
                if (order.Finalized == null)
                {
                    order.Finalized = new List<Dish>();
                }

                var newfinalized = order.Finalized.Concat(order.Selected);
                order.Finalized = newfinalized.ToList();
                order.Selected.Clear();
                ctx.Update(order);
                ctx.SaveChanges();
            }
            return RedirectToAction("FinalizedOrder", "MenuCard", new { guestCode = GuestCode });
        }
        
        public async Task<IActionResult> FinalizedOrder(string guestCode)
        {
            Order order = await ctx.Orders.Include(x => x.Owner).Include(x => x.Finalized).Where(x => x.Owner.Code == guestCode).LastOrDefaultAsync();

            if (order.Finalized == null)
            {
                order.Finalized = new List<Dish>();
            }

            await ctx.DishTypes.ToListAsync();

            OrderDishTypeViewModel viewModel = new OrderDishTypeViewModel()
            {
                orderDishes = order.Finalized.ToList(),
            };

            return View(new GuestCodeWithModel<OrderDishTypeViewModel>(viewModel, guestCode));
        }
    }
}