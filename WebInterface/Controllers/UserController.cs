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
            return View();
        }

    }
}