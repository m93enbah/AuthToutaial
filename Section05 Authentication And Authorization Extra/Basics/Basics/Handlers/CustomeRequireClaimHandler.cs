using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace Basics.Authorization
{
    //its requiremetns for the request to be authorized
    public class CustomRequireClaims : IAuthorizationRequirement
    {
        public string ClaimType { get; }
        public CustomRequireClaims(string claimType)
        {
            ClaimType = claimType;
        }
    }

    //this handle take the requirements and used to authorized
    public class CustomeRequireClaimHandler : AuthorizationHandler<CustomRequireClaims>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            CustomRequireClaims requirement)
        {
            var hasClaim = context.User.Claims.Any(x => x.Type == requirement.ClaimType);
            if (hasClaim)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }


    public static class AuthorizationPolicyBuildeExtensions 
    {
        public static AuthorizationPolicyBuilder RequireCustomClaim(this AuthorizationPolicyBuilder builder,string claimType) 
        {
            builder.AddRequirements(new CustomRequireClaims(claimType));
            return builder;
        }
    }
}
