using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NLog;
using ServerForReact.Abstract;
using ServerForReact.Data;
using ServerForReact.Data.Identity;
using ServerForReact.Helpers;
using ServerForReact.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ServerForReact.Services
{
    public class StudentService : IStudentService
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IMapper mapper;
        private readonly IJwtTokenService jwtTokenService;
        private readonly AppEFContext context;
        private readonly IEmailService emailService;
        private readonly ILogger<StudentService> logger;
        public StudentService(UserManager<AppUser> userManager, IJwtTokenService jwtTokenService,IMapper mapper,
            AppEFContext context, IEmailService emailService, ILogger<StudentService> logger)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            this.jwtTokenService = jwtTokenService;
            this.context = context;
            this.emailService = emailService;
            this.logger = logger;
        }

        public async Task<(string token, AppUser student)> CreateStudent(RegisterViewModel model)
        {
            var student = mapper.Map<AppUser>(model);
            student.Photo = PhotoHelper.AddPhoto(model.Photo);

            var result = await userManager.CreateAsync(student, model.Password);
            if (!result.Succeeded)
            {
                PhotoHelper.DeletePhoto(student.Photo);
                string tokenNull = null;
                AppUser studentNull = null;
                var error = (token: tokenNull, student: studentNull);
                return error;
            }
            string token = jwtTokenService.CreateToken(student);
            var StudentToken = (token: token, student: student);
            return StudentToken;
        }

        public async Task<string> DeleteStudent(int id)
        {
            var student = userManager.Users.SingleOrDefault(x => x.Id == id);
            if (student == null)
            {
                return null;
            }
            PhotoHelper.DeletePhoto(student.Photo);
            context.Users.Remove(student);
            await context.SaveChangesAsync();
            return $"Student {student.UserName} {student.Surname} was deleted successfully";
        }

        public async Task<(bool IsAdmin, string token)> LoginStudent(LoginViewModel model)
        {
            var student = await userManager.FindByEmailAsync(model.Email);
            bool IsAdmin = await userManager.IsInRoleAsync(student, "Admin");
            if (student != null)
            {
                string token = jwtTokenService.CreateToken(student);
                var result = (IsAdmin: IsAdmin, token: token);
                return result;
            }
            else
            {
                string tokenNull = null;
                var error = (IsAdmin: false, token: tokenNull);
                return error;
            }
        }

        public async Task<AppUser> UpdateStudent(SaveEditStudentViewModel model)
        {
            var student = userManager.Users
                    .SingleOrDefault(x => x.Id == model.Id);
            if (student != null)
            {
                student.Email = model.Email;
                student.UserName = model.Name;
                student.Surname = model.Surname;
                student.PhoneNumber = model.Phone;
                student.Age = model.Age;
                if (model.Photo != null)
                {
                    PhotoHelper.DeletePhoto(student.Photo);
                    student.Photo = PhotoHelper.AddPhoto(model.Photo);
                }
                await userManager.UpdateAsync(student);
                return student;
            }
            else
            {
                return null;
            }
        }

        public bool IsEmailExistRegister(string email)
        {
            return userManager.FindByEmailAsync(email).Result == null;
        }
        public bool IsEmailExistLogin(string email)
        {
            return userManager.FindByEmailAsync(email).Result != null;
        }
        public async Task<bool> IsPasswordCorrect(LoginViewModel model)
        {
            AppUser student = await userManager.FindByEmailAsync(model.Email);
            return userManager.CheckPasswordAsync(student, model.Password).Result;
        }
    }
}
