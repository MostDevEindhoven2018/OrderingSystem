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
            ctx.Orders.ToList();
            ctx.DishTypes.ToList();
            ctx.SubDishTypes.ToList();
            ctx.Dishes.ToList();


            await DBCreationTask;

            //List<DishType> drinks = ctx.DishTypes.Where(x => x.Course != CourseType.DRINK).ToList(); 

            var a = ctx.Orders.Include(x => x.Finalized);
            


            return View(a.ToList());
        }

        public async Task<IActionResult> Bar()
        {
            

            

            await DBCreationTask;

            ctx.Orders.ToList();
            ctx.DishTypes.ToList();
            ctx.SubDishTypes.ToList();
            ctx.Dishes.ToList();


            await DBCreationTask;

            //List<DishType> drinks = ctx.DishTypes.Where(x => x.Course != CourseType.DRINK).ToList(); 

            var a = ctx.Orders.Include(x => x.Finalized);



            return View(a.ToList());
        }



    }
}