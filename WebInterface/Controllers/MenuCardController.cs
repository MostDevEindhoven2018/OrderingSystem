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

            //collects all drinks DishType
            List<DishType> drinks = ctx.DishTypes.Where(x => x.Course == CourseType.DRINK).ToList();

            //collects the drinks SubDishType
            var subDrinks = drinks.Where(x => x.SubDishType != null).Select(x => x.SubDishType).ToList();

            //collects unique drinks from all drinks SubDishType
            List<SubDishType> uniqueSubDrinks = subDrinks.GroupBy(x => x.SubType).Select(x => x.FirstOrDefault()).ToList();

            //Dictionary that maps DishType and the quantity (DishType->int)

            Dictionary<DishType, int> output = new Dictionary<DishType, int>();

            //assigning all the drinks keyvalues to zero first

            foreach (DishType d in drinks)
            {
                output.Add(d, 0);
            }

            //Loading the orders table because of lazy entity framework

            ctx.Orders.ToList();

            //Selects all orders belongs to unique guest

            Order order = ctx.Orders.Where(x => x.Owner.Code == guestCode).FirstOrDefault();

            //Loading the Dishes table because of lazy entity framework

            ctx.Dishes.ToList();

            //If the selected orders by the guest is not null, then assign each drink value to guests order quantity using dictionary

            if (order.Selected != null)
            {
                //Loading the DisheTypes table because of lazy entity framework

                ctx.DishTypes.ToList();
                List<Dish> a = order.Selected.Where(x => x.Course.Course == CourseType.DRINK).ToList();

                var b = a.GroupBy(x => x.Course.Name).Select(x => new { type = x.FirstOrDefault().Course, quantity = x.Count() }).ToList();

                //assigning the drinks values (this replaces previous assigned zero values by guests order quantity for each drink)

                foreach (var item in b)
                {
                    output[item.type] = item.quantity;
                }
            }

            //drinks contains all drinks defined in the CLASS DISHTYPE

            DishTypeViewModel dishTypeViewModel = new DishTypeViewModel()
            {
                DishTypes = drinks,
                SubDishTypes = uniqueSubDrinks,
                quantityDictionary = output

            };

            return View(new GuestCodeWithModel<DishTypeViewModel>(dishTypeViewModel, guestCode));
        }

        /// <summary>
        /// orderName contains the string (view name) to view order overview page: in this Drinks case its "OrderOverview"
        /// proceedName contains the string (view name) to proceed: in this Drinks case its "Starters"
        /// </summary>
        /// <param name="col"></param>
        /// <param name="GuestCode"></param>
        /// <param name="orderName"></param>
        /// <param name="proceedName"></param>
        /// <returns></returns>



        //[HttpPost]
        //public IActionResult Drinks(IFormCollection col, string GuestCode, string orderName, string proceedName)
        //{

        [HttpPost]
        public void UpdateDrinks(IFormCollection col, string GuestCode)
        {
            ctx.Guests.ToList();
            ctx.Dishes.ToList();
            ctx.SubDishTypes.ToList();
            ctx.SaveChanges();

            List<DishType> drinks = ctx.DishTypes.Where(x => x.Course == CourseType.DRINK).ToList();

            //contains the data of ordered drinks

            Dictionary<DishType, int> output = new Dictionary<DishType, int>();

            foreach (DishType d in drinks)
            {
                output.Add(d, 0);
            }

            ctx.Orders.ToList();

            Order order = ctx.Orders.Where(x => x.Owner.Code == GuestCode).FirstOrDefault();
            ctx.Dishes.ToList();

            if (order.Selected != null)
            {
                ctx.DishTypes.ToList();
                List<Dish> a = order.Selected.Where(x => x.Course.Course == CourseType.DRINK).ToList();

                var b = a.GroupBy(x => x.Course.Name).Select(x => new { type = x.FirstOrDefault().Course, quantity = x.Count() }).ToList();

                foreach (var item in b)
                {
                    output[item.type] = item.quantity;
                }
            }

            var uniqueOrderList = ctx.Orders.Where(x => x.Owner.Code == GuestCode).FirstOrDefault();

            foreach (var selectedDrinks in drinks)
            {
                int quantity = Convert.ToInt32(col[selectedDrinks.Name]);
                if (quantity > output[selectedDrinks])
                {
                    if (uniqueOrderList == null)
                    {
                        RedirectToAction("ErrorView");
                    }

                    for (int i = 0; i < (quantity - output[selectedDrinks]); i++)
                    {
                        if (uniqueOrderList.Selected == null)
                        {
                            uniqueOrderList.Selected = new List<Dish>();
                        }
                        Dish dish = new Dish() { Course = selectedDrinks };
                        uniqueOrderList.Selected.Add(dish);
                        ctx.Dishes.Add(dish);
                    }
                    ctx.SaveChanges();
                }


                else if (quantity < output[selectedDrinks])
                {
                    if (uniqueOrderList == null)
                    {
                        RedirectToAction("ErrorView");
                    }

                    for (int i = 0; i < (output[selectedDrinks] - quantity); i++)
                    {
                        var test = (output[selectedDrinks] - quantity);
                        if (uniqueOrderList.Selected == null)
                        {
                            uniqueOrderList.Selected = new List<Dish>();
                        }

                        var toRemoveSelectedDrink = ctx.Dishes.Where(x => x.Course == selectedDrinks).LastOrDefault();
                        uniqueOrderList.Selected.Remove(toRemoveSelectedDrink);
                        ctx.Dishes.Remove(toRemoveSelectedDrink);
                        ctx.SaveChanges();
                    }

                }


            }

            //based on selected button page redirects to return view (either to order overview page or else to Starters page)


            //string selectedChoice = "";

            //if (orderName == null)
            //{
            //    selectedChoice = proceedName;
            //}
            //else
            //{
            //    selectedChoice = orderName;
            //}

            //return RedirectToAction(selectedChoice, "MenuCard", new { guestCode = GuestCode });
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

        /// <summary>
        /// GuestCode contains the unique guestcode
        /// orderName contains the string (view name) to view order overview page: in this Starters case its "OrderOverview"
        /// proceedName contains the string (view name) to proceed: in this Starters case its "Mains"
        /// goBack contains the string (view name) to go back: in this Starters case its "Drinks"
        /// </summary>
        /// <param name="col"></param>
        /// <param name="GuestCode"></param>
        /// <param name="orderName"></param>
        /// <param name="proceedName"></param>
        /// <param name="goBack"></param>
        /// <returns></returns>

        //[HttpPost]
        //public IActionResult Starters(IFormCollection col, string GuestCode, string orderName, string proceedName, string goBack)
        //{

        [HttpPost]
        public void UpdateStarters(IFormCollection col, string GuestCode)
        {
            List<DishType> starters = ctx.DishTypes.Where(x => x.Course == CourseType.STARTER).ToList();

            ctx.Guests.ToList();
            ctx.Dishes.ToList();
            ctx.SubDishTypes.ToList();

            //contains the data of ordered starters

            Dictionary<DishType, int> output = new Dictionary<DishType, int>();

            foreach (DishType d in starters)
            {
                output.Add(d, 0);
            }

            ctx.Orders.ToList();

            Order order = ctx.Orders.Where(x => x.Owner.Code == GuestCode).FirstOrDefault();
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

            var orderList = ctx.Orders.ToList();
            var uniqueOrderList = orderList.Where(x => x.Owner.Code == GuestCode).FirstOrDefault();

            foreach (var selectedStarters in starters)
            {
                int quantity = Convert.ToInt32(col[selectedStarters.Name]);
                if (quantity > output[selectedStarters])
                {
                    if (uniqueOrderList == null)
                    {
                        RedirectToAction("ErrorView");
                    }

                    for (int i = 0; i < (quantity - output[selectedStarters]); i++)
                    {
                        if (uniqueOrderList.Selected == null)
                        {
                            uniqueOrderList.Selected = new List<Dish>();
                        }
                        Dish dish = new Dish() { Course = selectedStarters };
                        uniqueOrderList.Selected.Add(dish);
                        ctx.Dishes.Add(dish);
                    }


                }
                else if (quantity < output[selectedStarters])
                {
                    if (uniqueOrderList == null)
                    {
                        RedirectToAction("ErrorView");
                    }

                    for (int i = 0; i < (output[selectedStarters] - quantity); i++)
                    {
                        if (uniqueOrderList.Selected == null)
                        {
                            uniqueOrderList.Selected = new List<Dish>();
                        }

                        var toRemoveSelectedStarter = ctx.Dishes.Where(x => x.Course == selectedStarters).LastOrDefault();
                        uniqueOrderList.Selected.Remove(toRemoveSelectedStarter);
                        ctx.Dishes.Remove(toRemoveSelectedStarter);
                    }

                }


            }

            ctx.SaveChanges();

            //based on selected button page redirects to return view (either to order overview page or else to Main course page)


            //string selectedChoice = "";

            //if (orderName != null)
            //{
            //    selectedChoice = orderName;
            //}
            //else if (proceedName != null)
            //{
            //    selectedChoice = proceedName;
            //}
            //else if (goBack != null)
            //{
            //    selectedChoice = goBack;
            //}

            //var test1 = selectedChoice;

            //return RedirectToAction(selectedChoice, "MenuCard", new { guestCode = GuestCode });
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

        /// <summary>
        /// col is a Icollection that contains all the Mains data entered by the guest
        /// GuestCode contains the unique guestcode
        /// orderName contains the string (view name) to view order overview page: in this mains case its "OrderOverview"
        /// proceedName contains the string (view name) to proceed: in this mains case its "Desserts"
        /// goBack contains the string (view name) to go back: in this mains case its "Starters"
        /// </summary>
        /// <param name="col"></param>
        /// <param name="GuestCode"></param>
        /// <param name="orderName"></param>
        /// <param name="proceedName"></param>
        /// <param name="goBack"></param>
        /// <returns></returns>

        //[HttpPost]
        //public IActionResult Mains(IFormCollection col, string GuestCode, string orderName, string proceedName, string goBack)
        //{

        [HttpPost]
        public void UpdateMains(IFormCollection col, string GuestCode)
        {
            List<DishType> mains = ctx.DishTypes.Where(x => x.Course == CourseType.MAINCOURSE).ToList();

            ctx.Guests.ToList();
            ctx.Dishes.ToList();
            ctx.SubDishTypes.ToList();

            //contains the data of ordered Main courses

            Dictionary<DishType, int> output = new Dictionary<DishType, int>();

            foreach (DishType d in mains)
            {
                output.Add(d, 0);
            }

            ctx.Orders.ToList();

            Order order = ctx.Orders.Where(x => x.Owner.Code == GuestCode).FirstOrDefault();
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

            var orderList = ctx.Orders.ToList();
            var uniqueOrderList = orderList.Where(x => x.Owner.Code == GuestCode).FirstOrDefault();

            foreach (var selectedMains in mains)
            {
                int quantity = Convert.ToInt32(col[selectedMains.Name]);
                if (quantity > output[selectedMains])
                {
                    if (uniqueOrderList == null)
                    {
                        RedirectToAction("ErrorView");
                    }

                    for (int i = 0; i < (quantity - output[selectedMains]); i++)
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
                else if (quantity < output[selectedMains])
                {
                    if (uniqueOrderList == null)
                    {
                        RedirectToAction("ErrorView");
                    }

                    for (int i = 0; i < (output[selectedMains] - quantity); i++)
                    {
                        if (uniqueOrderList.Selected == null)
                        {
                            uniqueOrderList.Selected = new List<Dish>();
                        }

                        var toRemoveSelectedMains = ctx.Dishes.Where(x => x.Course == selectedMains).LastOrDefault();
                        uniqueOrderList.Selected.Remove(toRemoveSelectedMains);
                        ctx.Dishes.Remove(toRemoveSelectedMains);
                        ctx.SaveChanges();

                    }

                }
            }

            //based on selected button page redirects to return view (either to order overview page or else to desserts page)

            //string selectedChoice = "";

            //if (orderName != null)
            //{
            //    selectedChoice = orderName;
            //}
            //else if (proceedName != null)
            //{
            //    selectedChoice = proceedName;
            //}
            //else if (goBack != null)
            //{
            //    selectedChoice = goBack;
            //}

            //return RedirectToAction(selectedChoice, "MenuCard", new { guestCode = GuestCode });
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

        /// <summary>
        /// col is a Icollection that contains all the desserts data entered by the guest
        /// GuestCode contains the unique guestcode
        /// proceedName contains the string (view name) to proceed: in this desserts case its "OrderOverview"
        /// goBack contains the string (view name) to go back: in this desserts case its "Mains"
        /// </summary>
        /// <param name="col"></param>
        /// <param name="GuestCode"></param>
        /// <param name="proceedName"></param>
        /// <param name="goBack"></param>
        /// <returns></returns>

        //[HttpPost]
        //public IActionResult Desserts(IFormCollection col, string GuestCode, string proceedName, string goBack)
        //{
        [HttpPost]
        public void UpdateDesserts(IFormCollection col, string GuestCode)
        {
            List<DishType> desserts = ctx.DishTypes.Where(x => x.Course == CourseType.DESSERT).ToList();

            ctx.Guests.ToList();
            ctx.Dishes.ToList();
            ctx.SubDishTypes.ToList();

            //contains the data of ordered drinks

            Dictionary<DishType, int> output = new Dictionary<DishType, int>();

            foreach (DishType d in desserts)
            {
                output.Add(d, 0);
            }

            ctx.Orders.ToList();

            Order order = ctx.Orders.Where(x => x.Owner.Code == GuestCode).FirstOrDefault();
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


            var orderList = ctx.Orders.ToList();
            var uniqueOrderList = orderList.Where(x => x.Owner.Code == GuestCode).FirstOrDefault();

            foreach (var selectedDesserts in desserts)
            {
                int quantity = Convert.ToInt32(col[selectedDesserts.Name]);
                if (quantity > output[selectedDesserts])
                {
                    if (uniqueOrderList == null)
                    {
                        RedirectToAction("ErrorView");
                    }

                    for (int i = 0; i < (quantity - output[selectedDesserts]); i++)
                    {
                        if (uniqueOrderList.Selected == null)
                        {
                            uniqueOrderList.Selected = new List<Dish>();
                        }
                        Dish dish = new Dish() { Course = selectedDesserts };
                        uniqueOrderList.Selected.Add(dish);
                        ctx.Dishes.Add(dish);

                    }
                    ctx.SaveChanges();
                }
                else if (quantity < output[selectedDesserts])
                {
                    if (uniqueOrderList == null)
                    {
                        RedirectToAction("ErrorView");
                    }

                    for (int i = 0; i < (output[selectedDesserts] - quantity); i++)
                    {
                        if (uniqueOrderList.Selected == null)
                        {
                            uniqueOrderList.Selected = new List<Dish>();
                        }

                        var toRemoveSelectedDesserts = ctx.Dishes.Where(x => x.Course == selectedDesserts).LastOrDefault();
                        uniqueOrderList.Selected.Remove(toRemoveSelectedDesserts);
                        ctx.Dishes.Remove(toRemoveSelectedDesserts);
                        ctx.SaveChanges();
                    }
                    



                }

            }

            //string selectedChoice = "";

            //if (proceedName != null)
            //{
            //    selectedChoice = proceedName;
            //}

            //else if (goBack != null)
            //{
            //    selectedChoice = goBack;

            //}

            //return RedirectToAction(selectedChoice, "MenuCard", new { guestCode = GuestCode });

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