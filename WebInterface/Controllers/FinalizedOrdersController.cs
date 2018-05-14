using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebInterface.Models;

namespace WebInterface.Controllers
{
    public class FinalizedOrdersController : Controller
    {
        MenuCardDBContext ctx;
        Task DBCreationTask;

        public FinalizedOrdersController(MenuCardDBContext context)
        {
            ctx = context;
            DBCreationTask = ctx.Database.EnsureCreatedAsync();
        }

        public async Task<IActionResult> Index()
        {
            await DBCreationTask;

            return View();
        }

        public async Task<IActionResult> Kitchen()
        {
            await DBCreationTask;



            //retrive from database... should get it from the list I think
            //var q = from o in ctx.Orders

            //        select new FinalizedOrder
            //        {
            //            Order = o.OrderID,
            //            Name = o.Owner.Name,
            //            Table = o.Owner.Group.Table.TableID

            //        };

            //var finalOrders = q.ToList();     



            var q = ctx.Orders
                .Include(guest => guest.Owner)
                .Include(table => table.Owner.Group.Table)
                .Include(final => final.Finalized).ToList();
              

            

            var finalOrders = new List<FinalizedOrder>();

            foreach (var elem in q)
            {
                var order = new FinalizedOrder
                {
                    Order = elem.OrderID,
                    Table = elem.Owner.Group.Table.TableID,
                    DishName = "poop"
                    };

                    finalOrders.Add(order);                
            }

            var finalDishes = new List<FinalizedOrder>();
                           

            return View(finalOrders);
        }

        public async Task<IActionResult> Bar()
        {
            await DBCreationTask;



            //retrive from database... should get it from the list I think
            var q = from o in ctx.Orders

                    select new FinalizedOrder
                    {
                        Order = o.OrderID,
                        DishName = o.Owner.Name,
                        Table = o.Owner.Group.Table.TableID

                    };

            var finalOrders = q.ToList();


            return View(finalOrders);
        }



    }
}