using System.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DrawingCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QRCoder;
using WebInterface.Models;


namespace WebInterface.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult PrintQR()
        {
            var tables = GetAllTables();

            var model = new QRGeneration();

            //Create a list of all tables

            model.Tables = GetSelectListItems(tables);

            return View(model);
        }

        [HttpPost]
        public IActionResult PrintQR(QRGeneration model)
        {
            //Get all tables again
            var tables = GetAllTables();

            //selected value from DropDownList is posted back
            model.Tables = GetSelectListItems(tables);

            //if a table is chosen the model is valid
            if (ModelState.IsValid)
            {
                var QR = new QRGeneration(model.Table_Number);
                //TODO Change the path or save as stream
                byte[] fileBytes = System.IO.File.ReadAllBytes(@"C:\\Users\\Paulina\\Pictures\\" + "QRTable_" + model.Table_Number + ".jpeg");
                string fileName = "QRTable_" + model.Table_Number + ".jpeg";
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }

            return View("PrintQR", model);
        }

        //TODO obtain Table information from database
        private IEnumerable<string> GetAllTables()
        {
            return new List<string>
            {
                "1",
                "2",
                "3",
                "4",
                "5",
                "6",
                "7",
                "8",
                "9",
                "10"
            };
        }

        private IEnumerable<SelectListItem> GetSelectListItems(IEnumerable<string> elements)
        {
            var selectList = new List<SelectListItem>();

            foreach (var element in elements)
            {
                selectList.Add(new SelectListItem
                {
                    Value = element,
                    Text = element
                });
            }

            return selectList;
        }

       


    }
}
