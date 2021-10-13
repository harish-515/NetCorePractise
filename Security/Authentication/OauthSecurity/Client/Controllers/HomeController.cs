using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Secret()
        {
            // accessing secret resource form server 
            var serverResponse = await RequestRetryWithRefreshAccessToken(
                () => FetchResponse("https://localhost:44382/Secret/Index"));

            var apiResponse = await RequestRetryWithRefreshAccessToken(
                () => FetchResponse("https://localhost:44353/Secret/Index"));

            return View();
        }

        private async Task<HttpResponseMessage> FetchResponse(string url)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            return await client.GetAsync(url);
        }

        private async Task RefreshAccessToken()
        {
            // as per the specification of OAuth one need to do a post request to the 
            // TokenEndpoint with granttype as "refresh_token",refresh token as content
            // and need to send authorization header with (basic authentication)

            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");
            var refreshTokenClient = _httpClientFactory.CreateClient();


            // here in the example we are not making use of these credentials
            // so these are just send to mimic the refresh token process

            var credentials = "username:password";
            var credentialBytes = Encoding.UTF8.GetBytes(credentials);
            var base64credentials = Convert.ToBase64String(credentialBytes);

            var requestData = new Dictionary<string, string>()
            {
                {"grant_type","refresh_token"},
                {"refresh_token",refreshToken}
            };

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44382/OAuth/Token");
            requestMessage.Content = new FormUrlEncodedContent(requestData);

            requestMessage.Headers.Add("Authorization", $"Basic {base64credentials}");

            var response = await refreshTokenClient.SendAsync(requestMessage);

            var responseString = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseString);

            var newaccessToken = responseData.GetValueOrDefault("access_token");
            var newrefreshToken = responseData.GetValueOrDefault("refresh_token");

            // once we get the new access_token , refresh token update them in the
            // current context (as we are using cookie scheme .. must update in the cookie)

            var authInfo = await HttpContext.AuthenticateAsync("ClientCookie");

            authInfo.Properties.UpdateTokenValue("access_token", newaccessToken);
            authInfo.Properties.UpdateTokenValue("refresh_token", newrefreshToken);

            // override the existing cookie

            await HttpContext.SignInAsync("ClientCookie", authInfo.Principal, authInfo.Properties);
        }

        private async Task<HttpResponseMessage> RequestRetryWithRefreshAccessToken(Func<Task<HttpResponseMessage>> request)
        {
            var response = await request();

            if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await RefreshAccessToken();
                response = await request();
            }

            return response;
        }

    }
}
