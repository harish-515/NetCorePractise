using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Authorization.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }

        [Authorize(Policy = "Claim.DoB")]
        public IActionResult SecretWithPolicy()
        {
            return View("Secret");
        }


        public IActionResult Authenticate()
        {
            // Authority -- Trusted parties who can specify information about the user
            // Claims -- Data specific to the user that the authority knows about
            // Identity -- A collection of claims provided by the authority ( can additinally add/remove/update claims)
            // Principal -- A collection of identities from various trusted sources about the user ( has some additional behaviour to select primary identity/ role checking ect)


            // building up identies of Bob

            // Who are you as per grandma (Authority)
            var grandmaClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,"Bob"),
                new Claim(ClaimTypes.Email,"Bob@hmail.com"),
                new Claim(ClaimTypes.DateOfBirth,"10/10/2010"),
                new Claim("Grandma.Says","Very nice boy")
            };

            // Who are you as per government (Authority)
            var licenseClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,"Bob K Foo"),
                new Claim("DrivingLicense","A+")
            };

            // Identities
            var grandmaIdentity = new ClaimsIdentity(grandmaClaims, "Grandma Identity");
            var licenseIdentity = new ClaimsIdentity(licenseClaims, "Government");


            // collection of identites as per our trusted sources
            var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity,licenseIdentity });

            // trying to signin to the app using the following claim principal
            HttpContext.SignInAsync(userPrincipal);

            // All these above steps are usually done under the hud by the framework at the time of 
            // user login and once authenticated the following details are filled into HttpContext.User  
            // And once this user is filled with claims the user can now be verified for authorization


            return RedirectToAction("Index");
        }
    }
}
