using Basics.Handlers;
using Microsoft.AspNetCore.Authorization;

namespace Basics.Attributes
{
    //it will generate policy customzie as below
    public class SecurityLevelAttribute : AuthorizeAttribute
    {
        public SecurityLevelAttribute(int level) 
        {
            Policy = $"{DynamicPolicies.SecurityLevel}.{level}";
        }
    }
}
