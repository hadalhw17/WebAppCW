using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using _689997CW1.Models;


namespace _689997CW1.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View("IndexWithForm");
        }

        [HttpPost]
        public IActionResult Index(Contacts contact)
        {
            //Contacts contact = new Contacts()
            //{
            //    Id = id,
            //    FirstName = "Aleksandr",
            //    LastName = "Slobodov"
            //};
            if (ModelState.IsValid)
            {
                return View(contact);
            }
            return View("IndexWithForm");
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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
