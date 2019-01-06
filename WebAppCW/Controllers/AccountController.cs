using System.Security.Claims;
using System.Threading.Tasks;
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
                    UserName = _user.Username,
                    Email = _user.Email
                };
                var result = await _userManager.CreateAsync(user, _user.Password);

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

        [HttpGet]
        public ViewResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel _user)
        {
            if (ModelState.IsValid)
            {
                var result = await _signManager.PasswordSignInAsync(_user.Email,
                   _user.Password, false, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");

                }
            }
            ModelState.AddModelError("", "Invalid login attempt");
            return View(_user);
        }
    }
}