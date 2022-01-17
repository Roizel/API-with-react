using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServerForReact.Abstract;
using ServerForReact.Data.Identity;
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
        private readonly HttpClient _httpClient;
        private readonly UserManager<AppUser> userManager;
        private readonly IMapper mapper;
        private readonly ILogger<FacebookService> logger;
        private readonly IJwtTokenService jwtTokenService;

        public FacebookService(UserManager<AppUser> _userManager, IMapper _mapper, ILogger<FacebookService> _logger, IJwtTokenService _jwtTokenService)
        {
            jwtTokenService = _jwtTokenService;
            userManager = _userManager;
            mapper = _mapper;
            logger = _logger;

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://graph.facebook.com/v2.8/")
            };
            _httpClient.DefaultRequestHeaders
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
            var checkEmail = userManager.Users.FirstOrDefault(x => x.Email == userEmail);

            if (checkEmail == null)
            {
                var account = new FacebookUserResource()
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
        public async Task<string> FacebookLoginAsync(string facebookToken)
        {
            var result = await GetAsync<dynamic>(facebookToken, "me", "fields=first_name,last_name,email,picture");
            if (result == null)
            {
                throw new Exception("User from this token not exist");
            }

            string userEmail = result.email;
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

        public async Task<string> FacebookRegister(FacebookRegisterResource model)
        {
            var student = mapper.Map<AppUser>(model);
            string fileName = String.Empty;
            if (model.Photo != null)
            {
                string randomFilename = Path.GetRandomFileName() +
                    Path.GetExtension(model.Photo.FileName);

                string dirPath = Path.Combine(Directory.GetCurrentDirectory(), "images");
                fileName = Path.Combine(dirPath, randomFilename);
                using (var file = System.IO.File.Create(fileName))
                {
                    model.Photo.CopyTo(file);
                }
                student.Photo = randomFilename;
            }

            var result = await userManager.CreateAsync(student);
            if (!result.Succeeded)
            {
                if (!string.IsNullOrEmpty(fileName))
                    System.IO.File.Delete(fileName);
            }
            string token = jwtTokenService.CreateToken(student);
            return token;
        }

        private async Task<T> GetAsync<T>(string accessToken, string endpoint, string args = null)
        {
            var response = await _httpClient.GetAsync($"{endpoint}?access_token={accessToken}&{args}");
            if (!response.IsSuccessStatusCode)
                return default(T);

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(result);
        }
    }
}
