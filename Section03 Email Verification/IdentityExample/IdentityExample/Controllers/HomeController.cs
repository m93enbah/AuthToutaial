using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Basics.Controllers
{
    public class HomeController : Controller
    {
        //used to handle user information such as get , update , delete 
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public HomeController(UserManager<IdentityUser> userManager,SignInManager<IdentityUser> signInManager) 
        {
          _userManager = userManager;
          _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AccessDenied() 
        {
            return View();
        }

        //mainly to used guard the action
        [Authorize]
        public IActionResult Secret() 
        {
            return View();
        }
    }
}
