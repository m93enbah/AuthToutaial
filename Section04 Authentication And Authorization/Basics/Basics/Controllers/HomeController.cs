using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Basics.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //mainly to used guard the action
        [Authorize(Policy ="Claim.DoB")]
        public IActionResult Secret() 
        {
            return View();
        }


        [Authorize(Roles ="Admin")]
        public IActionResult SecretRole()
        {
            return View("Secret");
        }

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
    }
}
