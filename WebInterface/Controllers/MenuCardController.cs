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
        Task DBCreationTask;
        public MenuCardController(MenuCardDBContext context)
        {
            ctx = context;
            DBCreationTask = ctx.Database.EnsureCreatedAsync();
        }

        public async Task<IActionResult> Index()
        {
            await DBCreationTask;
            return View();
        }

        public async Task<IActionResult> Drinks()
        {
            await DBCreationTask;
            return View();
        }

        public async Task<IActionResult> Starters()
        {
            await DBCreationTask;
            return View();
        }

        public async Task<IActionResult> Mains()
        {
            await DBCreationTask;
            return View();
        }

        public async Task<IActionResult> Desserts()
        {
            await DBCreationTask;
            return View();
        }

        public async Task<IActionResult> OrderOverview()
        {
            await DBCreationTask;
            return View();
        }
    }
}