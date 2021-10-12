using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization.Controllers
{
    public class OperationsController : Controller
    {
        private readonly IAuthorizationService _authService;

        public OperationsController(IAuthorizationService authService)
        {
            this._authService = authService;
        }

        public async Task<IActionResult> Open()
        {
            var cookijar = new CookieJar(); //resource from some where in database

            var requirement = new OperationAuthorizationRequirement()
            {
                Name = CookieJarOperations.ComeNear
            };

            var authResults = await _authService.AuthorizeAsync(User,cookijar, requirement);
            return View();
        }
    }

    public class CookieJarAuthorizationHandler 
        : AuthorizationHandler<OperationAuthorizationRequirement,CookieJar>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            OperationAuthorizationRequirement requirement,
            CookieJar cookieJar)
        {
            if(requirement.Name == CookieJarOperations.Look)
            {
                if (context.User.Identity.IsAuthenticated)
                {
                    context.Succeed(requirement);
                }
            }
            else if(requirement.Name == CookieJarOperations.ComeNear)
            {
                if (context.User.HasClaim("Friend", "Good")) ;
            }


            return Task.CompletedTask;
        }
    }

    public class CookieJarOperations
    {
        public static string Open = "Open"; 
        public static string Look = "Look";
        public static string ComeNear = "ComeNear";
        public static string TakeCookie = "TakeCookie";
    }


    public class CookieJar
    {
        public string Name { get; set; }
    }
}
