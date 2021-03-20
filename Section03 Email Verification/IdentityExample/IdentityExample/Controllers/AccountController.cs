using IdentityExample.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityExample.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IEmailService emailManager;

        public AccountController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,IEmailService emailManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailManager = emailManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            var model = new UserRegistrationDto();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegistrationDto request)
        {
            if (ModelState.IsValid)
            {
                var userCheck = await userManager.FindByEmailAsync(request.Email);
                if (userCheck == null)
                {
                    var user = new IdentityUser
                    {
                        UserName = request.Email,
                        NormalizedUserName = request.Email,
                        Email = request.Email,
                        PhoneNumber = request.PhoneNumber,
                        EmailConfirmed = false,
                        PhoneNumberConfirmed = false,
                    };
                    var result = await userManager.CreateAsync(user, request.Password);
                    if (result.Succeeded)
                    {
                        //generation of the email token
                        var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                        var link = Url.Action(nameof(VerifyEmail), "Account", new { userId = user.Id, code },Request.Scheme,Request.Host.ToString());
                        await emailManager.SendAsync("mohammedenbah93@gmail.com", "email verify",$"<a href=\"{link}\">Verfiy Email</a>",true);
                        return RedirectToAction("EmailVerification");
                    }
                    else
                    {
                        if (result.Errors.Count() > 0)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("message", error.Description);
                            }
                        }
                        return View(request);
                    }
                }
                else
                {
                    ModelState.AddModelError("message", "Email already exists.");
                    return View(request);
                }
            }
            return View(request);
        }

        public async Task<IActionResult> VerifyEmail(string userId, string code) 
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user is null) 
            {
                return BadRequest();
            }
            var result = await userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded) 
            {
                return View();
            }
            return BadRequest();
        }

        public IActionResult EmailVerification() 
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            var model = new UserLoginDto();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null && !user.EmailConfirmed)
                {
                    ModelState.AddModelError("message", "Email not confirmed yet");
                    return View(model);

                }
                if (await userManager.CheckPasswordAsync(user, model.Password) == false)
                {
                    ModelState.AddModelError("message", "Invalid credentials");
                    return View(model);

                }

                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    await userManager.AddClaimAsync(user, new Claim("UserRole", "Admin"));
                    return RedirectToAction("Dashboard");
                }
                else if (result.IsLockedOut)
                {
                    return View("AccountLocked");
                }
                else
                {
                    ModelState.AddModelError("message", "Invalid login attempt");
                    return View(model);
                }
            }
            return View(model);
        }

        [Authorize]
        public IActionResult Dashboard()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("login", "account");
        }
    }
}
