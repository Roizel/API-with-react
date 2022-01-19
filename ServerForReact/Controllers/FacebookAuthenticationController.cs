using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServerForReact.Data.Identity;
using ServerForReact.Models.FacebookResources;
using ServerForReact.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ServerForReact.Controllers
{
    [Route("api/auth-facebook")]
    [ApiController]
    public class FacebookAuthenticationController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly FacebookService fb;
        public FacebookAuthenticationController(UserManager<AppUser> _userManager, FacebookService _fb)
        {
            fb = _fb;
            userManager = _userManager;
        }

        [HttpPost]
        [Route("facebook-getuserdata")]
        public async Task<IActionResult> FacebookGetUserDataAsync([FromForm]FacebookLoginResource resource)
        {
            var authorizationTokens = await fb.GetUserFromFacebookAsync(resource.facebookToken);
            if (authorizationTokens == null)
            {
                return StatusCode(409);
            }
            return Ok(authorizationTokens);
        }
        [HttpPost]
        [Route("facebook-login")]
        public async Task<IActionResult> FacebookLoginAsync([FromForm] FacebookLoginResource resource)
        {
            string token = await fb.FacebookLoginAsync(resource.facebookToken);
            if (token == null)
            {
                return StatusCode(409);
            }
            var result = (token: token, IsAdmin: false);
            return Ok(new { result.token, result.IsAdmin });
        }

        [HttpPost]
        [Route("facebook-register")]
        public async Task<IActionResult> FacebookRegisterAsync([FromForm] FacebookRegisterResource resource)
        {
            string token = await fb.CreateUserFromFacebook(resource);
            if (token == null)
            {
                return StatusCode(409);
            }
            return Ok(token);
        }
    }
}
