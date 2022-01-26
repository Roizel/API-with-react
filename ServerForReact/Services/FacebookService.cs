using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServerForReact.Abstract;
using ServerForReact.Data.Identity;
using ServerForReact.Helpers;
using ServerForReact.Models.FacebookResources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ServerForReact.Services
{
    public class FacebookService : IFacebookService
    {
        private readonly HttpClient httpClient;
        private readonly UserManager<AppUser> userManager;
        private readonly IMapper mapper;
        private readonly ILogger<FacebookService> logger;
        private readonly IJwtTokenService jwtTokenService;

        public FacebookService(UserManager<AppUser> userManager, IMapper mapper, ILogger<FacebookService> logger, IJwtTokenService jwtTokenService)
        {
            this.jwtTokenService = jwtTokenService;
            this.userManager = userManager;
            this.mapper = mapper;
            this.logger = logger;

            httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://graph.facebook.com/v2.8/")
            };
            httpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        public async Task<FacebookUserResource> GetUserFromFacebookAsync(string facebookToken)
        {
            var result = await GetAsync<dynamic>(facebookToken, "me", "fields=first_name,last_name,email,picture");
            if (result == null)
            {
                throw new Exception("User from this token not exist");
            }

            string userEmail = result.email;
            if (userEmail != null)
            {
                userEmail = userEmail.ToLower();
                var checkEmail = userManager.Users.FirstOrDefault(x => x.Email == userEmail);
                if (checkEmail == null)
                {
                    var account = new FacebookUserResource
                    {
                        Email = userEmail,
                        FirstName = result.first_name,
                        LastName = result.last_name,
                    };

                    return account;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                var account = new FacebookUserResource
                {
                    Email = "",
                    FirstName = result.first_name,
                    LastName = result.last_name,
                };
                return account;
            }
        }
        public async Task<string> FacebookLoginAsync(string facebookToken)
        {
            var result = await GetAsync<dynamic>(facebookToken, "me", "fields=first_name,last_name,email,picture");
            if (result == null)
            {
                throw new Exception("User from this token not exist");
            }

            string userEmail = result.email;
            userEmail = userEmail.ToLower();
            var student = userManager.Users.FirstOrDefault(x => x.Email == userEmail);
            if (student.Email != null)
            {
                return jwtTokenService.CreateToken(student);
            }
            else
            {
                return null;
            }
        }

        public async Task<string> CreateUserFromFacebook(FacebookRegisterResource model)
        {
            var check = userManager.Users.FirstOrDefault(x=> x.Email == model.Email);
            if (check != null)
            {
                return null;
            }

            var student = mapper.Map<AppUser>(model);
            student.Photo = PhotoHelper.AddPhoto(model.Photo);

            var result = await userManager.CreateAsync(student);
            if (!result.Succeeded)
            {
                PhotoHelper.DeletePhoto(student.Photo);
                return null;
            }
            string token = jwtTokenService.CreateToken(student);
            return token;
        }

        private async Task<T> GetAsync<T>(string accessToken, string endpoint, string args = null)
        {
            var response = await httpClient.GetAsync($"{endpoint}?access_token={accessToken}&{args}");
            if (!response.IsSuccessStatusCode)
                return default(T);

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(result);
        }
    }
}
