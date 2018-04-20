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
            int guestID = 1;
            return RedirectToAction("Index",new { guestID = guestID });
        }

        public async Task<IActionResult> Index(int? guestID)
        {
            if (guestID == null)
            {
                return RedirectToAction("ErrorView");
            }
            await DBCreationTask;
            return View();
        }

        public async Task<IActionResult> Drinks(int? guestID)
        {
            if (guestID == null)
            {
                return RedirectToAction("ErrorView");
            }
            await DBCreationTask;
            return View();
        }

        public async Task<IActionResult> Starters(int? guestID)
        {
            if (guestID == null)
            {
                return RedirectToAction("ErrorView");
            }
            await DBCreationTask;
            return View();
        }

        public async Task<IActionResult> Mains(int? guestID)
        {
            if (guestID == null)
            {
                return RedirectToAction("ErrorView");
            }
            await DBCreationTask;
            return View();
        }

        public async Task<IActionResult> Desserts(int? guestID)
        {
            if (guestID == null)
            {
                return RedirectToAction("ErrorView");
            }
            await DBCreationTask;
            return View();
        }

        public async Task<IActionResult> OrderOverview(int? guestID)
        {
            if (guestID == null)
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