using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserCustimizationMenu.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

namespace UserCustimizationMenu.Controllers
{
    //[Authorize (Roles ="Administrator")]
    [Authorize]
    public class AccountController : Controller
    {
        
        //public ActionResult Login()
        //{
        //}

        //public ActionResult Logout()
        //{
        //}
        public AccountController()
        {
        }
    }
}
