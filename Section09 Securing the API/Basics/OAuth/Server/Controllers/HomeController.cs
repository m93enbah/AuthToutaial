using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using Server.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Server.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }

        public IActionResult Authenticate()
        
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,"some_id"),
                new Claim("granny","cookie")
            };

            var secretBytes = Encoding.UTF8.GetBytes(JWTSettings.Secret);
            var key = new SymmetricSecurityKey(secretBytes);

            //we will use SHA 256 into generate the certificate 
            var algorithim = SecurityAlgorithms.HmacSha256;

            var signingCredentials = new SigningCredentials(key, algorithim);

            var token = new JwtSecurityToken(
                JWTSettings.Issuer,
                JWTSettings.Audiance,
                claims
                ,notBefore:DateTime.Now
                ,expires:DateTime.Now.AddHours(1)
                , signingCredentials);

            var tokenJson = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new {  access_token = tokenJson});
        }

        public IActionResult Decode(string part) 
        {
            var bytes = Convert.FromBase64String(part);
            return Ok(Encoding.UTF8.GetString(bytes));
        }
    }
}
