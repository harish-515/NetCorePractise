using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MvcClient.Controllers
{
    public class HomeController :Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public HomeController(IHttpClientFactory httpFactory)
        {
            httpClientFactory = httpFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Secret()
        {
            var accessTokenHash = await HttpContext.GetTokenAsync("access_token");
            var idTokenHash = await HttpContext.GetTokenAsync("id_token");

            // in the GrantTypes.Code the redresh token is optional
            // so unless configured it would be sent empty
            var refreshTokenHash =await HttpContext.GetTokenAsync("refresh_token");

            var _accesstoken =  new JwtSecurityTokenHandler().ReadJwtToken(accessTokenHash);
            var _idtoken = new JwtSecurityTokenHandler().ReadJwtToken(idTokenHash);

            var apioneSecret = await GetAPIOnceSecret();

           return View();
        }

        private async Task<string> GetAPIOnceSecret()
        {

            var apiClient = httpClientFactory.CreateClient();

            var accessTokenHash = await HttpContext.GetTokenAsync("access_token");

            apiClient.SetBearerToken(accessTokenHash);

            var response = await apiClient.GetAsync("https://localhost:44386/Secret");

            return await response.Content.ReadAsStringAsync();
        }
    }
}
