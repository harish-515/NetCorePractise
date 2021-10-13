using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace ApiTwo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public HomeController(IHttpClientFactory httpFactory)
        {
            httpClientFactory = httpFactory;
        }

        [Route("/Home/Index")]
        public async Task<IActionResult> Index()
        {
            // get the access token from identity server
            var serverClient = httpClientFactory.CreateClient();
            var discoveryDocument = await serverClient.GetDiscoveryDocumentAsync("https://localhost:44310/");

            var tokenResponse = await serverClient.RequestClientCredentialsTokenAsync(
                new ClientCredentialsTokenRequest() { 
                        Address = discoveryDocument.TokenEndpoint,
                        ClientId = "client_id",
                        ClientSecret = "client_secret",
                        Scope = "ApiOne"
            });

            // get the response from api one
            var apiClient = httpClientFactory.CreateClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await apiClient.GetAsync("https://localhost:44386/Secret");

            var responseContent = await response.Content.ReadAsStringAsync();

            return Ok(new {
                access_token = tokenResponse.AccessToken,
                apione_message = responseContent
            });
        }

    }
}
