using Basics.Handlers;
using Microsoft.AspNetCore.Authorization;

namespace Basics.Attributes
{
    //we see that the custom atuhorize attribute is used to generate custom policy with dynamic name
    //so we need to register it dynamically
    public class MinimumTimeSpendAuthorizeAttribute : AuthorizeAttribute
    {
        public MinimumTimeSpendAuthorizeAttribute(int days)
        {
            NoOfDays = days;
        }

        int days;

        public int NoOfDays
        {
            get
            {
                return days;
            }
            set
            {
                days = value;
                Policy = $"{DynamicPolicies.MinimumTimeSpend}.{value.ToString()}";
            }
        }
    }
}
