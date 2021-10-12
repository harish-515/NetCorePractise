using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Authorization.AuthorizationRequirements
{
    public class CustomRequireClaim : IAuthorizationRequirement
    {
        public CustomRequireClaim(string claimType)
        {
            ClaimType = claimType;
        }

        public string ClaimType { get; }
    }

    public class CustomRequireClaimHandler : AuthorizationHandler<CustomRequireClaim>
    {
        public CustomRequireClaimHandler()
        {

        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomRequireClaim requirement)
        {
            var hasClaim = context.User.Claims.Any(claim => claim.Type == requirement.ClaimType);

            if (hasClaim)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    public static class CustomRequireClaimExtenstions
    {
        public static AuthorizationPolicyBuilder RequiredCustomClaim(this AuthorizationPolicyBuilder builder,string claimType)
        {
            return builder.AddRequirements(new CustomRequireClaim(ClaimTypes.DateOfBirth));
        } 
    }

}
