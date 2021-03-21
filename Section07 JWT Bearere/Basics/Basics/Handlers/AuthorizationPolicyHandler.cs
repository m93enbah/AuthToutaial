using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Basics.Handlers
{
    //we have mainly four classes that used to implement authorization policy holder:-
    //1-class inherit from AuthorizeAttribute to generate custom / dynamic policy called MinimumTimeSpendAuthorizeAttribute
    //2-MinimumTimeSpendRequirement:class inherit from IAuthorizationRequirement with custom parameter like noOfDays 
    //3-MinimumTimeSpendPolicy:class inherit from IAuthorizationPolicyProvider that used to get custom policy generate from the authorize attribute and check the second
    //arugment if its number and generate new authorize policy
    //4-MinimumTimeSpendHandler:class inherit from AuthorizationHandler<MinimumTimeSpendRequirement> that used to check the token contains the claims
    //DateOfJoining and compare if the period is correct or not

    public interface IAuthorizationBaseRequirement : IAuthorizationRequirement
    {
        public int Value { get; set; }
    }

    public class MinimumTimeSpendRequirement : IAuthorizationBaseRequirement
    {
        public int Value { get; set; }

        public MinimumTimeSpendRequirement(int noOfDays)
        {
            Value = noOfDays;
        }
    }

    public class SecurityLevelRequirement : IAuthorizationBaseRequirement
    {
        public int Value { get; set; }

        public SecurityLevelRequirement(int level)
        {
            Value = level;
        }
    }

    public static class DynamicPolicies
    {
        public const string Rank = "Rank";
        public const string SecurityLevel = "SecurityLevel";
        public const string MinimumTimeSpend = "MinimumTimeSpend";
        public const string ClaimDob = "Claim";
        //we can get this policies from database
        public static IEnumerable<string> Get()
        {
            yield return Rank;
            yield return SecurityLevel;
            yield return MinimumTimeSpend;
            yield return ClaimDob;
        }
    }

    //it will take the policyname and get type and value and then generate new AuthorizationPolicy with specify claim 
    public static class DynamicAuthPolicyFactory
    {
        public static AuthorizationPolicy Create(string policyName)
        {
            var parts = policyName.Split('.');
            var type = parts.First();
            Int32.TryParse(parts.Last(), out var value);
            switch (type)
            {
                case DynamicPolicies.Rank:
                    return new AuthorizationPolicyBuilder()
                        .RequireClaim(type, value.ToString())
                        .Build();
                case DynamicPolicies.SecurityLevel:
                    return new AuthorizationPolicyBuilder()
                        .AddRequirements(new SecurityLevelRequirement(value))
                        .Build();
                case DynamicPolicies.MinimumTimeSpend:
                    return new AuthorizationPolicyBuilder()
                        .AddRequirements(new MinimumTimeSpendRequirement(value))
                        .Build();
                case DynamicPolicies.ClaimDob:
                    return new AuthorizationPolicyBuilder()
                        .RequireClaim(ClaimTypes.DateOfBirth)
                        .Build();
                default:
                    return null;
            }
        }
    }

    public class AuthorizationBasePolicyProvider : DefaultAuthorizationPolicyProvider
    {
        public AuthorizationBasePolicyProvider(IOptions<AuthorizationOptions> options) : base(options){ }
        //this part is called whenever you hit request and it will check to the policy name passed through authorize attribute as {value}-{type}
        public override Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            foreach (var custPolicy in DynamicPolicies.Get())
            {
                if (policyName.StartsWith(custPolicy))
                {
                    return Task.FromResult(DynamicAuthPolicyFactory.Create(policyName));
                }
            }
            return base.GetPolicyAsync(policyName);
        }
    }

    public class AuthorizationBaseHandler : AuthorizationHandler<IAuthorizationBaseRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IAuthorizationBaseRequirement requirement)
        {
            if (requirement.GetType() == typeof(MinimumTimeSpendRequirement))
            {
                if (!context.User.HasClaim(c => c.Type == DynamicPolicies.MinimumTimeSpend))
                {
                    return Task.FromResult(0);
                }
                var dateOfJoining = Convert.ToDateTime(context.User.FindFirst(c => c.Type == DynamicPolicies.MinimumTimeSpend).Value);
                double calculatedTimeSpend = (DateTime.Now.Date - dateOfJoining.Date).TotalDays;
                if (calculatedTimeSpend >= requirement.Value)
                {
                    context.Succeed(requirement);
                }
                return Task.FromResult(0);
            }
            else if (requirement.GetType() == typeof(SecurityLevelRequirement))
            {
                var claimValue = Convert.ToInt32(context.User.Claims.FirstOrDefault(x => x.Type == DynamicPolicies.SecurityLevel)?.Value ?? "0");
                if (requirement.Value <= claimValue)
                {
                    context.Succeed(requirement);
                }
                //otherwise it will redirect ot Access Denied
                return Task.CompletedTask;
            }
            else 
            {
                var claim = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.DateOfBirth);
                if (claim != null) 
                {
                    context.Succeed(requirement);
                }
                return Task.CompletedTask;
            }
        }
    }
}
