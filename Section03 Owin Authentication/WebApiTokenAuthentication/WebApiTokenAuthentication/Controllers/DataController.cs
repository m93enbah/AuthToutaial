﻿using System;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;

namespace WebApiTokenAuthentication.Controllers
{
    public class DataController : ApiController
    {

        [AllowAnonymous]
        [HttpGet]
        [Route("api/data/forall")]
        public IHttpActionResult Get() 
        {
            return Ok($"Now server time is : {DateTime.Now.ToString()}");
        }

        [Authorize]
        [HttpGet]
        [Route("api/data/authenticate")]
        public IHttpActionResult GetForAuthenticate() 
        {
            var identity = (ClaimsIdentity)User.Identity;
            return Ok($"Hello {identity.Name}");
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        [Route("api/data/authorize")]
        public IHttpActionResult GetForAdmin() 
        {
            var identity = (ClaimsIdentity)User.Identity;
            var roles = identity.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value);
            return Ok($"Hello {identity.Name} Role : {String.Join(",", roles.ToList())}");
        }
    }
}