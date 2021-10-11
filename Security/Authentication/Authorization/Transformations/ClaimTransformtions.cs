using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Authorization.Transformations
{
    public class ClaimTransformtions : IClaimsTransformation
    {
        // These transformations are always run when ever user is authenticated

        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var hasFriendClaim = principal.Claims.Any(claim => claim.Type == "Friend");

            // transforming existing claims into some thing else.. Might be used to add missing 
            // or default claims 

            if (!hasFriendClaim)
            {
                ((ClaimsIdentity)principal.Identity).AddClaim(new Claim("Friend", "Bad"));
            }

            return Task.FromResult(principal);
        }
    }
}
