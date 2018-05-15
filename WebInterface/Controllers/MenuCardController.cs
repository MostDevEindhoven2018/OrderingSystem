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

            //await DBCreationTask;

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

            var test1 = output;

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

            var test = output;

            var uniqueOrderList = ctx.Orders.Where(x => x.Owner.Code == GuestCode).FirstOrDefault();

            foreach (var selectedDrinks in drinks)
            {
                int quantity = Convert.ToInt32(col[selectedDrinks.Name]);

                //if (quantity > 99)
                //{
                //    quantity = 99;
                //}

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

                    var totalOrderedDrinks = ctx.Orders.Where(x => x.Owner.Code == GuestCode).Select(x => x.Selected).FirstOrDefault().ToList();


                    List<Dish> uniqueTotalOrderedDrinks = new List<Dish>();

                    for (int j = 0; j < totalOrderedDrinks.Count; j++)
                    {
                        if (totalOrderedDrinks[j].Course.Name == selectedDrinks.Name)
                        {
                            uniqueTotalOrderedDrinks.Add(totalOrderedDrinks[j]);

                        }

                    }

                    List<Dish> removeSelectedDrinks = new List<Dish>();

                    for (int i = 0; i < (output[selectedDrinks] - quantity); i++)
                    {
                        if (uniqueOrderList.Selected == null)
                        {
                            uniqueOrderList.Selected = new List<Dish>();
                        }

                        for (int j = 0; j < uniqueTotalOrderedDrinks.Count; j++)
                        {
                            removeSelectedDrinks.Add(uniqueTotalOrderedDrinks[i]);

                        }

                    }

                    ctx.Dishes.RemoveRange(removeSelectedDrinks);
                    ctx.SaveChanges();

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

                    var totalOrderedStarters = ctx.Orders.Where(x => x.Owner.Code == GuestCode).Select(x => x.Selected).FirstOrDefault().ToList();


                    List<Dish> uniqueTotalOrderedStarters = new List<Dish>();

                    for (int j = 0; j < totalOrderedStarters.Count; j++)
                    {
                        if (totalOrderedStarters[j].Course.Name == selectedStarters.Name)
                        {
                            uniqueTotalOrderedStarters.Add(totalOrderedStarters[j]);

                        }

                    }

                    List<Dish> removeSelectedStarters = new List<Dish>();

                    for (int i = 0; i < (output[selectedStarters] - quantity); i++)
                    {
                        if (uniqueOrderList.Selected == null)
                        {
                            uniqueOrderList.Selected = new List<Dish>();
                        }

                        for (int j = 0; j < uniqueTotalOrderedStarters.Count; j++)
                        {
                            removeSelectedStarters.Add(uniqueTotalOrderedStarters[i]);

                        }

                    }

                    ctx.Dishes.RemoveRange(removeSelectedStarters);
                    ctx.SaveChanges();

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

                    var totalOrderedMains = ctx.Orders.Where(x => x.Owner.Code == GuestCode).Select(x => x.Selected).FirstOrDefault().ToList();


                    List<Dish> uniqueTotalOrderedMains = new List<Dish>();

                    for (int j = 0; j < totalOrderedMains.Count; j++)
                    {
                        if (totalOrderedMains[j].Course.Name == selectedMains.Name)
                        {
                            uniqueTotalOrderedMains.Add(totalOrderedMains[j]);

                        }

                    }

                    List<Dish> removeSelectedMains = new List<Dish>();

                    for (int i = 0; i < (output[selectedMains] - quantity); i++)
                    {
                        if (uniqueOrderList.Selected == null)
                        {
                            uniqueOrderList.Selected = new List<Dish>();
                        }

                        for (int j = 0; j < uniqueTotalOrderedMains.Count; j++)
                        {
                            removeSelectedMains.Add(uniqueTotalOrderedMains[i]);

                        }

                    }

                    ctx.Dishes.RemoveRange(removeSelectedMains);
                    ctx.SaveChanges();

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

                    var totalOrderedMains = ctx.Orders.Where(x => x.Owner.Code == GuestCode).Select(x => x.Selected).FirstOrDefault().ToList();


                    List<Dish> uniqueTotalOrderedDesserts = new List<Dish>();

                    for (int j = 0; j < totalOrderedMains.Count; j++)
                    {
                        if (totalOrderedMains[j].Course.Name == selectedDesserts.Name)
                        {
                            uniqueTotalOrderedDesserts.Add(totalOrderedMains[j]);

                        }

                    }

                    List<Dish> removeSelecteddesserts = new List<Dish>();

                    for (int i = 0; i < (output[selectedDesserts] - quantity); i++)
                    {
                        if (uniqueOrderList.Selected == null)
                        {
                            uniqueOrderList.Selected = new List<Dish>();
                        }

                        for (int j = 0; j < uniqueTotalOrderedDesserts.Count; j++)
                        {
                            removeSelecteddesserts.Add(uniqueTotalOrderedDesserts[i]);

                        }

                    }

                    ctx.Dishes.RemoveRange(removeSelecteddesserts);
                    ctx.SaveChanges();

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

            //ICollection<Dish> orders = ctx.Orders.Where(x => x.Owner.Code == guestCode).Select(x=>x.Finalized).LastOrDefault().ToList();

            //Dictionary<DishType, int> output = new Dictionary<DishType, int>();

            //foreach (var a in orders)
            //{
            //    output.Add(a, 0);
            //}
            //ctx.Orders.ToList();

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

            //return View();
            return View(new GuestCodeWithModel<OrderDishTypeViewModel>(orderDishTypeViewModel, guestCode));
        }
        [HttpPost]
        public void updateOrderOverview(IFormCollection col, string GuestCode)
        {
            ctx.Orders.ToList();
            ctx.Dishes.ToList();
            ctx.Guests.ToList();
            ctx.DishTypes.ToList();

            //List<DishType> allDishtypes = ctx.Orders.Where(x => x.Owner.Code == GuestCode).Select(x=>x.Selected.Select(y=>y.Course)).LastOrDefault().ToList();

            List<DishType> allDishtypes = ctx.DishTypes.ToList();

            Order order2 = ctx.Orders.Where(x => x.Owner.Code == GuestCode).LastOrDefault();
            ctx.Dishes.ToList();

            Dictionary<DishType, int> output = new Dictionary<DishType, int>();

            foreach (var d in allDishtypes)
            {
                output.Add(d, 0);
            }

            

            //if (order2.Finalized != null)
            //{
                ctx.DishTypes.ToList();
                List<Dish> c = order2.Selected.ToList();

                var b = c.GroupBy(x => x.Course.Name).Select(x => new { type = x.FirstOrDefault().Course, quantity = x.Count() }).ToList();

                foreach (var item in b)
                {
                    output[item.type] = item.quantity;
                }
            //}

            var test = output;








            foreach (var selectedDishes in allDishtypes)
            {
                int quantity = Convert.ToInt32(col[selectedDishes.Name]);

                //if (quantity > 99)
                //{
                //    quantity = 99;
                //}

                if (quantity > output[selectedDishes])
                {
                    if (order2 == null)
                    {
                        RedirectToAction("ErrorView");
                    }

                    for (int i = 0; i < (quantity - output[selectedDishes]); i++)
                    {
                        var test2 = (quantity - output[selectedDishes]);
                        if (order2.Finalized == null)
                        {
                            order2.Finalized = new List<Dish>();
                        }
                        Dish dish = new Dish() { Course = selectedDishes };
                        order2.Selected.Add(dish);
                        order2.Finalized.Add(dish);
                        ctx.Dishes.Add(dish);
                    }
                    ctx.SaveChanges();
                }


                else if (quantity < output[selectedDishes])
                {
                    if (order2 == null)
                    {
                        RedirectToAction("ErrorView");
                    }

                    var totalOrderedDrinks = ctx.Orders.Where(x => x.Owner.Code == GuestCode).Select(x => x.Selected).FirstOrDefault().ToList();


                    List<Dish> uniqueTotalOrderedDishes = new List<Dish>();

                    for (int j = 0; j < totalOrderedDrinks.Count; j++)
                    {
                        if (totalOrderedDrinks[j].Course.Name == selectedDishes.Name)
                        {
                            uniqueTotalOrderedDishes.Add(totalOrderedDrinks[j]);

                        }

                    }

                    List<Dish> removeSelectedDishess = new List<Dish>();

                    for (int i = 0; i < (output[selectedDishes] - quantity); i++)
                    {
                        if (order2.Finalized == null)
                        {
                            order2.Finalized = new List<Dish>();
                        }

                        for (int j = 0; j < uniqueTotalOrderedDishes.Count; j++)
                        {
                            removeSelectedDishess.Add(uniqueTotalOrderedDishes[i]);

                        }

                    }

                    ctx.Dishes.RemoveRange(removeSelectedDishess);
                    ctx.SaveChanges();

                }


            }


















            //var a = ctx.Orders.Where(x => x.Owner.Code == GuestCode).Select(x => x.Selected).FirstOrDefault().ToList();
            ////var a = g.Select(x => x.Selected).FirstOrDefault();

            ////contains the data of Orderoverview


            //var uniqueDishes = a.GroupBy(x => x.Course.Name).ToList();

            //List<Dish> orderFinalized = new List<Dish>();

            //foreach (var finalizedOrder in a)
            //{
            //    int quantity = Convert.ToInt32(col[finalizedOrder.Course.Name]);

            //    if (uniqueDishes == null)
            //    {
            //        RedirectToAction("ErrorView");
            //    }

            //    for (int i = 0; i < quantity; i++)
            //    {
            //        if (uniqueDishes == null)
            //        {
            //            orderFinalized = new List<Dish>();
            //        }
                                  
            //    }
            //    orderFinalized.Add(finalizedOrder);

            //    foreach (var item in orderFinalized)
            //    {
            //        ctx.Dishes.Add(item);
            //    }  
            ////ctx.SaveChanges();

            //}



            //Order order = new Order();

            //if (order.Finalized==null)
            //{
            //    List<Dish> dish = new List<Dish>();
            //    order.Finalized = dish;

            //    foreach (var item in orderFinalized)
            //    {
            //        order.Finalized.Add(item);
            //    }

                
            //}

            //OrderDishTypeViewModel orderDishTypeViewModel = new OrderDishTypeViewModel()
            //{
            //    orderDishes = orderFinalized
            //};

            RedirectToAction("FinalizedOrder", "Menucard", new Guest() { Code = GuestCode });
              



            //var test = ctx.Orders.Where(x=>x.Owner.Code==GuestCode).Select(x => x.Finalized).ToList();
            //int t = 2;


            //OrderDishTypeViewModel orderDishTypeViewModel = new OrderDishTypeViewModel()
            //{
            //    orderDishes = orderFinalized
            // };

            //return View(new GuestCodeWithModel<OrderDishTypeViewModel>(orderDishTypeViewModel, GuestCode));
            //RedirectToAction("FinalizedOrder", "MenuCard", new Guest() { Code = GuestCode });
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

        private IActionResult RedirectToAction(Func<string, IActionResult> finalizedOrder, string v, object p)
        {
            throw new NotImplementedException();
        }

        public IActionResult FinalizedOrder(string guestCode)
        {

            ctx.Guests.ToList();
            ctx.Dishes.ToList();
            ctx.SubDishTypes.ToList();
            ctx.Orders.ToList();
            ctx.DishTypes.ToList();

            Order g = ctx.Orders.Where(x => x.Owner.Code == guestCode).LastOrDefault();
            var a = g.Finalized;


            if (a == null)
            {
                a = new List<Dish>();

            }

            List<Dish> b = a.ToList();

            OrderDishTypeViewModel c = new OrderDishTypeViewModel()
            {
                orderDishes = b,
            };

            //return View();
            return View(new GuestCodeWithModel<OrderDishTypeViewModel>(c, guestCode));
        }
    }
}