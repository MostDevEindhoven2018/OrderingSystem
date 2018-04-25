using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebInterface.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;


namespace WebInterface.Controllers
{
    public class UserController : Controller
    {
        public UserController(MenuCardDBContext _context)
        {
            ctx = _context;
        }

        MenuCardDBContext ctx = null;

        public IActionResult Customize()
        {
            ctx.Database.EnsureCreated();

            var query = from dt in ctx.DishTypes
                        select dt;

            List<DishType> results = query.ToList();

            return View(results);
        }

        public IActionResult Create()
        {
            
            //var Course = Enum.GetValues(typeof(CourseType));

            //var list = new List<string>();

            //foreach (string course in Course)
            //{
            //    list.Add($"{course}");
            //}
                return View();
        }

        [HttpPost]
        public IActionResult Create(DishType newDishType)
        {
            newDishType.DefaultIngredients = new List<Ingredient>();
            if(ModelState.IsValid)
            {
                ctx.DishTypes.Add(newDishType);
                ctx.SaveChanges();

                return RedirectToAction("Customize");
            }
            else
            {
                return View(newDishType);
            }
        }

    }
}