using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace API.JwtRequirements
{
    public class JwtRequirement :IAuthorizationRequirement
    {
        
    }

    public class JwtRequirementHandler : AuthorizationHandler<JwtRequirement>
    {
        private readonly HttpClient _client;
        private readonly HttpContext _httpContext;

        public JwtRequirementHandler(IHttpClientFactory clientFactory,IHttpContextAccessor httpContextAccessor)
        {
            _client = clientFactory.CreateClient();
            _httpContext = httpContextAccessor.HttpContext;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, JwtRequirement requirement)
        {
            if(_httpContext.Request.Headers.TryGetValue("Authorization",out var authHeader))
            {
                var token = authHeader.ToString().Split(' ')[1];

                var validationResponse = await _client.GetAsync($"https://localhost:44382/OAuth/Validate?access_token={token}");

                if(validationResponse.StatusCode == HttpStatusCode.OK)
                {
                    context.Succeed(requirement);
                }

            }
        }
    }
}
