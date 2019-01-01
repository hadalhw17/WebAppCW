using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAppCW.Models;

namespace WebAppCW.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signManager;
        private readonly UserManager<User> _userManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signManager)
        {
            _userManager = userManager;
            _signManager = signManager;
        }


        [HttpGet]
        public ViewResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel _user)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = _user.Username
                };
                var result = await _userManager.CreateAsync(user, _user.Password);

                if (result.Succeeded)
                {
                    await _signManager.SignInAsync(user, false);

                    await _userManager.AddClaimAsync(user, new Claim("IsCommenter", "true"));
                    await _userManager.AddClaimAsync(user, new Claim("IsAdmin", "false"));

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View();
        }
    }
}