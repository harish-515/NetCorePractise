using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManeger;
        private readonly SignInManager<IdentityUser> _signInManager;

        public HomeController(UserManager<IdentityUser> userManeger,SignInManager<IdentityUser> signInManager)
        {
            this._userManeger = userManeger;
            this._signInManager = signInManager;
        }


        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username,string password)
        {
            var signinResult = await _signInManager.PasswordSignInAsync(username, password, false, false);

            if (signinResult.Succeeded)
            {
                return RedirectToAction("Secret");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string password)
        {
            // registering a new user
            var user = new IdentityUser()
            {
                UserName = username,
                Email = ""               
            };

            var result = await _userManeger.CreateAsync(user, password);

            if(result.Succeeded)
            {
                await _signInManager.PasswordSignInAsync(user, password, false, false);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index");
        }
    }
}
