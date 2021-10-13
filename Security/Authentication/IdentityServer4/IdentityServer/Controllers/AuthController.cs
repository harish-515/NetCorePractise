using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IdentityServer.Controllers
{
    public class AuthController : Controller
    {

        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthController(SignInManager<IdentityUser> signInManager)
        {
            this._signInManager = signInManager;
        }

        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl});
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // check if model is valid
            // check if user is valid
            // then signin
            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);

            if (result.Succeeded)
            {
                return Redirect(model.ReturnUrl);
            }
            else
            {
                return BadRequest("Invalid User Details");
            }
        }

    }
}
