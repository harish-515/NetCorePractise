using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public ActionResult<string> Index()
        {
            return "This is API";
        }
    }
}
