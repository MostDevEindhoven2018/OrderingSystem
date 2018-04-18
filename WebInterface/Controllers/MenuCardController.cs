using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebInterface.Models;

namespace WebInterface.Controllers
{

    public class MenuCardController : Controller
    {
        MenuCardDBContext ctx;
        public MenuCardController(MenuCardDBContext context)
        {
            ctx = context;
        }

        public IActionResult Index()
        {
            ctx.Database.EnsureCreated();
            return View();
        }

        public IActionResult Drinks()
        {
            return View();
        }

        public IActionResult Starters()
        {
            return View();
        }

        public IActionResult Mains()
        {
            return View();
        }

        public IActionResult Desserts()
        {
            return View();
        }

        public IActionResult OrderOverview()
        {
            return View();
        }
    }
}