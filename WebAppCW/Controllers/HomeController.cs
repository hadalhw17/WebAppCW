//Created by Aleksandr Slobodov, student number 689997

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppCW.Data;
using WebAppCW.Models;


namespace WebAppCW.Controllers
{
    public class HomeController : Controller
    {
        // Our database.
        private readonly ApplicationDbContext _context;

        // User manager, that is used to automatically set author name.
        private readonly UserManager<User> _userManager;

        // Returns user that is currently logged in.
        private Task<User> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // Constructor.
        public HomeController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET
        // Renders list of all posts on the home page.
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View("Index", await _context.Post.ToListAsync());
        }

        // GET
        // Renders about page.
        [HttpGet]
        public IActionResult About()
        {
            ViewData["Message"] = "Blog of Aleksandr Slobodov";

            return View();
        }

        // GET
        // Renders contacts page.
        public IActionResult Contact()
        {
            return View();
        }

        // GET
        // Renders post creation page.
        [HttpGet]
        public IActionResult CreatePost()
        {
            return RedirectToAction("Create", "Post");
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
