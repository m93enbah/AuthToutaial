using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basics.CustomPolicyProvider
{
    //{type} can be take from database or set as fixed 
    public static class DynamicPolicies
    {
        public const string SecurityLevel = "SecurityLevel";
        public const string Rank = "Rank";
        public const string ClaimDob = "Claim.DoB";

        //we can get this policies from database
        public static IEnumerable<string> Get()
        {
            yield return SecurityLevel;
            yield return Rank;
            yield return ClaimDob;
        }
    }

    //this class represent authorization requiremnt with new prop called level
    //that used to inject into the SecurityLevelHandler
    public class SecurityLevelRequirement : IAuthorizationRequirement
    {
        public int Level { get; }
        public SecurityLevelRequirement(int level)
        {
            Level = level;
        }
    }

    //this class prepare policy created by using policy name syntax {type}-{value}
    //where value will be take as level 
    public static class DynamicAuthPolicyFactory
    {
        public static AuthorizationPolicy Create(string policyName)
        {
            //var parts = policyName.Split('.');
            //var type = parts.First();
            //var value = parts.Last();
            switch (policyName)
            {
                case DynamicPolicies.Rank:
                    return new AuthorizationPolicyBuilder()
                        .RequireClaim("Rank", "Rank")
                        .Build();
                case DynamicPolicies.SecurityLevel:
                    return new AuthorizationPolicyBuilder()
                        .AddRequirements(
                        new SecurityLevelRequirement(1))
                        .Build();
                case DynamicPolicies.ClaimDob:
                    return new AuthorizationPolicyBuilder()
                        .RequireClaim("DateOfBirth", "DateOfBirth")
                        .Build();
                default:
                    return null;
            }
        }
    }

    //we inherit DefaultAuthorizationPolicyProvider that inherit from 
    //the IAuthorizationPolicyProvider to use what only we want from function
    public class CustomAuthPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        public CustomAuthPolicyProvider(IOptions<AuthorizationOptions> options) : base(options) 
        {
        }
        //{value}-{type}
        public override Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            foreach (var custPolicy in DynamicPolicies.Get()) 
            {
                if (policyName.StartsWith(custPolicy)) 
                {
                    //var policy = new AuthorizationPolicyBuilder().Build();
                    return Task.FromResult(DynamicAuthPolicyFactory.Create(policyName));
                }
            }
           return base.GetPolicyAsync(policyName);
        }
    }

    //based on the level we check that the new policy
    public class SecurityLevelHandler : AuthorizationHandler<SecurityLevelRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            SecurityLevelRequirement requirement)
        {
            var claimValue = Convert.ToInt32(context.User.Claims
                .FirstOrDefault(x => x.Type == DynamicPolicies.SecurityLevel)?.Value ?? "0");


            if (requirement.Level <= claimValue) 
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
