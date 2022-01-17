using Microsoft.AspNetCore.Identity;
using ServerForReact.Data.Identity;
using ServerForReact.Models.FacebookResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerForReact.Services
{
    public class AccountService
    {
        private readonly FacebookService fb;
        private readonly IJwtTokenService tokenService;
        private readonly UserManager<AppUser> userManager;

        public AccountService(FacebookService _fb, IJwtTokenService _tokenService, UserManager<AppUser> _userManager)
        {
            fb = _fb;
            tokenService = _tokenService;
            userManager = _userManager;
        }

        public async Task<FacebookUserResource> FacebookGetDataAsync(FacebookLoginResource facebookLoginResource)
        {
            if (string.IsNullOrEmpty(facebookLoginResource.facebookToken))
            {
                throw new Exception("Token is null or empty");
            }

            var facebookUser = await fb.GetUserFromFacebookAsync(facebookLoginResource.facebookToken);
            var result = userManager.Users.FirstOrDefault(x => x.Email == facebookUser.Email);
            if (result.Email == null)
            {
                return facebookUser;
            }
            else
            {
                return null;
            }
        }

        //public async Task<AuthorizationTokensResource> FacebookLoginAsync(FacebookLoginResource facebookLoginResource)
        //{
        //    if (string.IsNullOrEmpty(facebookLoginResource.facebookToken))
        //    {
        //        throw new Exception("Token is null or empty");
        //    }

        //    var facebookUser = await fb.GetUserFromFacebookAsync(facebookLoginResource.facebookToken);

        //    return facebookUser;
        //}

        //private async Task<AuthorizationTokensResource> CreateAccessTokens(User user, string deviceId,
        //    string deviceName)
        //{
        //    var accessToken = _jwtHandler.CreateAccessToken(user.Id, user.Email, user.Role);

        //    return new AuthorizationTokensResource { AccessToken = accessToken, RefreshToken = refreshToken };
        //}
    }
}
