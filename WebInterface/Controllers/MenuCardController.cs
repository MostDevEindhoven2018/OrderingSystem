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

        public IActionResult AddGuest(int? tableNo)
        {
            if(tableNo == null)
            {
                return RedirectToAction("ErrorView");
            }
            //TODO: add guest here
            string guestCode = "ABC021";
            return RedirectToAction("Index",new { guestCode = guestCode });
        }

        public async Task<IActionResult> Index(string guestCode)
        {
            if (guestCode == null)
            {
                return RedirectToAction("ErrorView");
            }
            await DBCreationTask;
            return View();
        }

        public async Task<IActionResult> Drinks(string guestCode)
        {
            if (guestCode == null)
            {
                return RedirectToAction("ErrorView");
            }
            await DBCreationTask;
            return View();
        }

        public async Task<IActionResult> Starters(string guestCode)
        {
            if (guestCode == null)
            {
                return RedirectToAction("ErrorView");
            }
            await DBCreationTask;
            return View();
        }

        public async Task<IActionResult> Mains(string guestCode)
        {
            if (guestCode == null)
            {
                return RedirectToAction("ErrorView");
            }
            await DBCreationTask;
            return View();
        }

        public async Task<IActionResult> Desserts(string guestCode)
        {
            if (guestCode == null)
            {
                return RedirectToAction("ErrorView");
            }
            await DBCreationTask;
            return View();
        }

        public async Task<IActionResult> OrderOverview(string guestCode)
        {
            if (guestCode == null)
            {
                return RedirectToAction("ErrorView");
            }
            await DBCreationTask;
            return View();
        }

        public IActionResult ErrorView()
        {
            return View();
        }
    }
}