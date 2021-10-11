using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization.CustomPolicyProvider
{

    public class SecurityLevelAttribute : AuthorizeAttribute
    {
        public SecurityLevelAttribute(int level)
        {
            Policy = $"{DynamicPolicies.SecurityLevel}.{level}";
        }
    }


    // {type}
    public static class DynamicPolicies
    {
        public static IEnumerable<string> Get()
        {
            yield return SecurityLevel;
            yield return Rank;
        }

        public const string SecurityLevel = "SecurityLevel";
        public const string Rank = "Rank";
    }

    public class SecurityLevelRequirment : IAuthorizationRequirement
    {
        public SecurityLevelRequirment(int level)
        {
            Level = level;
        }

        public int Level { get; }
    }

    public class SecurityLevelRequirmentHandler : AuthorizationHandler<SecurityLevelRequirment>
    {
        public SecurityLevelRequirmentHandler()
        {

        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SecurityLevelRequirment requirement)
        {
            var claimValue = Convert.ToInt32(context.User.Claims.FirstOrDefault(claim => claim.Type == DynamicPolicies.SecurityLevel)?.Value ?? "0");

            if (requirement.Level <= claimValue)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    public static class DynamicAuthorizationPolicyBuilder
    {
        public static AuthorizationPolicy Create(string policyName)
        {
            var parts = policyName.Split(".");

            var type = parts[0];
            var value = parts[1];

            switch (type)
            {
                case DynamicPolicies.Rank:
                    return new AuthorizationPolicyBuilder()
                            .RequireClaim("Rank",value)
                            .Build();
                case DynamicPolicies.SecurityLevel:
                    return new AuthorizationPolicyBuilder()
                            .AddRequirements(new SecurityLevelRequirment(Convert.ToInt32(value)))
                            .Build();
                default:
                    return null;
            }
        }
    }

    public class CustomAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        public CustomAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) 
            : base(options)
        {
        }


        // {type}.{value}
        public override Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            foreach( var customPolicy in DynamicPolicies.Get())
            {
                if (policyName.StartsWith(customPolicy))
                {
                    var policy = DynamicAuthorizationPolicyBuilder.Create(policyName);
                    return Task.FromResult(policy);
                }
                    

            }

            return base.GetPolicyAsync(policyName);
        }
    }
}
