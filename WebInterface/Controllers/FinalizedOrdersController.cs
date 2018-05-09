﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
            //await DBCreationTask;

            return View();
        }

        public async Task<IActionResult> Kitchen()
        {
            await DBCreationTask;
            //retrive from database
            var q = from o in ctx.Orders
                    select new FinalizedOrder
                    {
                        Order = o.OrderID,
                        Name = o.Owner.Name,
                        Table = o.Owner.Group.Table.TableID

                    };

            var finalOrders = q.ToList();

            return View(finalOrders);
        }

        public async Task<IActionResult> Bar()
        {
            await DBCreationTask;
            return View();
        }



    }
}