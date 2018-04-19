using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserCustimizationMenu.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;


namespace UserCustimizationMenu.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(MyDbContext _context)
        {
            ctx = _context;
        }

        MyDbContext ctx = null;

        public IActionResult Login()
        {
            ctx.Database.EnsureCreated();

            var query = from u in ctx.Users
                        select u;


            //List<User> results = query.ToList();

            return View();
        }

        //[HttpPost]
        //public IActionResult Login(User user)
        //{
        //    ctx.Database.EnsureCreated();

        //    var query = from u in ctx.Users
        //                select u;

        //    //if (user.Password == "")
        //    //{

        //    //}

        //    return View();
        //}

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}