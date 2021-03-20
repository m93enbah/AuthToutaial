using Basics.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Basics.Controllers
{
    public class OperationsController : Controller
    {
        private IAuthorizationService _authorizationService;
        public OperationsController(IAuthorizationService authorizationService) 
        {
            _authorizationService = authorizationService;
        }

        //we pass the resource which like password or something else with pass the authorization operation
        //with the user and we can then apply authorization operation to check if with these three params 
        //if he authorized or not

        //three params:
        //1-User
        //2-Resources
        //3-requirement : claims 
        public async Task<IActionResult> Open() 
        {
            var cookieJar = new CookieJar();  //get cookie jar from db
            //we assign requirement in this way
            var requirement = new OperationAuthorizationRequirement()
            {
                Name = CookieJarOperations.Open
            };
            await _authorizationService.AuthorizeAsync(User, cookieJar, requirement);

            //or with shorter way
            await _authorizationService.AuthorizeAsync(User, cookieJar, CookieJarAuthOperations.Open);
            return View();
        }
    }
}
