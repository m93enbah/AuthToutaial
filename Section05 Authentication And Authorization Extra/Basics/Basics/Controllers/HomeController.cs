using Basics.Attributes;
using Basics.Handlers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Basics.Controllers
{
    public class HomeController : Controller
    {
        public IAuthorizationService _authorizationService { get; }
        public HomeController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        public IActionResult Index()
        {
            return View();
        }

        //authorize without filter does not check for the role / claims
        [Authorize]
        public IActionResult SecretWithoutFilter()
        {
            return View();
        }

        //mainly to used guard the action
        [Authorize(Policy ="Claim.DoB")]
        public IActionResult Secret() 
        {
            return View();
        }

        [Authorize(Policy = "Claim.DoB")]
        //[Authorize(Policy = "SecurityLevel.5")]
        public IActionResult SecretPolicy()
        {
            return View("Secret");
        }

        [SecurityLevel(5)]
        public IActionResult SecretLevel5()
        {
            return View("Secret");
        }


        [SecurityLevel(10)]
        public IActionResult SecretLevel10()
        {
            return View("Secret");
        }


        [Authorize(Roles ="Admin")]
        public IActionResult SecretRole()
        {
            return View("Secret");
        }

        [AllowAnonymous]
        public async Task<IActionResult> Authenticate()
        {
            //claim is an implementation not specified to microsoft
            //claim is the information that put inside the cookie that will be created 

            //we can have multiple claims with multiple identities that can be assigned to the same user principle
            var grandmaCliams = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,"Bob"),
                new Claim(ClaimTypes.Email,"Bob@gmail.com"),
                new Claim("Grandma.Says","Very nice boy"),
                new Claim(ClaimTypes.DateOfBirth,"DateNow"),
                new Claim(DynamicPolicies.SecurityLevel,$"3"),
                new Claim(DynamicPolicies.MinimumTimeSpend,$"{DateTime.Now.AddMonths(3)}"),     
                new Claim(ClaimTypes.Role,"Admin")
            };

            var licenseClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,"Bob K Foo"),
                new Claim("DrivingLicense","A+")
            };

            var grandmaIdentity = new ClaimsIdentity(grandmaCliams,"Grandma Identity");
            var licenseIdentity = new ClaimsIdentity(licenseClaims, "Government");

            //define the user principle that contains multiple claim identities that each one contains single claim
            var userPrinciple = new ClaimsPrincipal(new[] { grandmaIdentity, licenseIdentity });

            //register the user principle and create cookie contains all the user principle that we put it
            await HttpContext.SignInAsync(userPrinciple);

            return RedirectToAction("Index");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
        //to check authorization in custom action
        public async Task<IActionResult> DoStuff() 
        {
            //we are doning stuff here
            //it will create Authorization Builder
            var builder = new AuthorizationPolicyBuilder("Schema");
            //it will create Authorization Policy
            var customPolicy = builder.RequireClaim("Hello").Build();

            var authResult = await _authorizationService.AuthorizeAsync(User, customPolicy);

            if (authResult.Succeeded) 
            {

            }
            return View("Index");
        }
    }
}
