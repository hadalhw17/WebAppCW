//Created by Aleksandr Slobodov, student number 689997

using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAppCW.Models;

namespace WebAppCW.Controllers
{
    public class AccountController : Controller
    {
        // Our sign in manager.
        private readonly SignInManager<User> _signManager;

        // Our user manager.
        private readonly UserManager<User> _userManager;

        // Constructor.
        public AccountController(UserManager<User> userManager, SignInManager<User> signManager)
        {
            _userManager = userManager;
            _signManager = signManager;
        }


        // GET
        // Renders registration form.
        [HttpGet]
        public ViewResult Register()
        {
            return View();
        }

        // POST
        // Custom logic for user registration.
        // Created in order to assign claims on registration.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel _user)
        {
            // Validation.
            if (ModelState.IsValid)
            {

                // Creates our user model.
                var user = new User
                {
                    UserName = _user.Username,
                    Email = _user.Email
                };

                // Creates user with the password inputed in the user manager.
                var result = await _userManager.CreateAsync(user, _user.Password);

                // If created successfully, assign claims for regular user that can leave comments,
                // But not write posts.
                // Otherwise return error message.
                if (result.Succeeded)
                {

                    await _userManager.AddClaimAsync(user, new Claim("IsCommenter", "true"));
                    await _userManager.AddClaimAsync(user, new Claim("IsAdmin", "false"));

                    await _signManager.SignInAsync(user, false);

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

        // GET
        // Renders login form.
        [HttpGet]
        public ViewResult Login()
        {
            return View();
        }

        // POST
        // Contains logic for user login.
        // Created so users could sign in with their username, instead of email.
        // This is just a personal prefference and would not affect anything in the final version
        // Of the coursework, since as specified in the brief UserName = E-mail
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel _user)
        {
            // Validation.
            if (ModelState.IsValid)
            {
                // Sign in with Username and password.
                var result = await _signManager.PasswordSignInAsync(_user.Email,
                   _user.Password, false, false);

                // If signin succedded than redirect to home page.
                // Print error message otherwise.
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError("", "Invalid login attempt");

            return View(_user);
        }

        // GET
        // Renders list of all users.
        [HttpGet]
        [Authorize(Policy = "IsBlogger")]
        public async Task<IActionResult> GetAll()
        {
            // Get list of all users.
            var _users = _userManager.Users;

            return View("Index",_users);
        }


        // POST
        // DEPRICATED.
        // Deletes user from user manager.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "IsBlogger")]
        public async Task<IActionResult> Delete(int? id)
        {
            // Get user by id selected.
            var _user = await _userManager.FindByIdAsync(id.ToString());

            // Delete user selected above.
            var result = await _userManager.DeleteAsync(_user);

            // Get list of all users.
            var _users = _userManager.Users;

            // If deletion was successful then render list of all users.
            // Print error message otherwise.
            if (result.Succeeded)
            {
                return View("Index", _users);
            }

            ModelState.AddModelError("", "Could not delete user.");

            return View("Index", _users);
        }
    }
}