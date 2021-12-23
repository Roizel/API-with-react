using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ServerForReact.Abstract;
using ServerForReact.Data;
using ServerForReact.Data.Identity;
using ServerForReact.Exceptions;
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
        public StudentService(UserManager<AppUser> _userManager, IJwtTokenService _jwtTokenService, SignInManager<AppUser> _signInManager,
            IMapper _mapper, AppEFContext _context, IEmailService _emailService)
        {
            mapper = _mapper;
            userManager = _userManager;
            jwtTokenService = _jwtTokenService;
            context = _context;
            emailService = _emailService;
        }

        public async Task<string> CreateStudent(RegisterViewModel model)
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

            var result = await userManager.CreateAsync(student, model.Password);
            if (!result.Succeeded)
            {
                if (!string.IsNullOrEmpty(fileName))
                    System.IO.File.Delete(fileName);
                AccountError accountError = new AccountError();
                foreach (var item in result.Errors)
                {
                    accountError.Errors.Invalid.Add(item.Description);
                }
                throw new AccountException(accountError);
            }
            return jwtTokenService.CreateToken(student);
        }

        public async Task<string> DeleteStudent(int id)
        {

            var student = userManager.Users.SingleOrDefault(x => x.Id == id);
            if (student == null)
            {
                AccountError error = new AccountError();
                error.Errors.Invalid.Add("This id does not exist");
                throw new AccountException(error);
            }
            if (student.Photo != null)
            {
                var directory = Path.Combine(Directory.GetCurrentDirectory(), "images");
                var FilePath = Path.Combine(directory, student.Photo);
                System.IO.File.Delete(FilePath);
            }
            context.Users.Remove(student);
            await context.SaveChangesAsync();
            return $"Student {student.UserName} {student.Surname} was deleted successfully";
        }

        public async Task<string> LoginStudent(LoginViewModel model)
        {
            var student = await userManager.FindByEmailAsync(model.Email);
            bool IsAdmin = await userManager.IsInRoleAsync(student, "Admin");
            if (student != null)
            {
                string token = jwtTokenService.CreateToken(student);
                return token;
            }
            else
            {
                return null;
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
                string fileName = String.Empty;
                if (model.Photo != null)
                {
                    if (student.Photo != null)
                    {
                        var directory = Path.Combine(Directory.GetCurrentDirectory(), "images");
                        var FilePath = Path.Combine(directory, student.Photo);
                        System.IO.File.Delete(FilePath);
                    }
                    if (model.Photo != null)
                    {
                        var ext = Path.GetExtension(model.Photo.FileName);
                        fileName = Path.GetRandomFileName() + ext;
                        var dir = Path.Combine(Directory.GetCurrentDirectory(), "images");
                        var filePath = Path.Combine(dir, fileName);
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            model.Photo.CopyTo(stream);
                        }
                        student.Photo = fileName;
                    }
                }
                await userManager.UpdateAsync(student);
                return student;
            }
            else
            {
                AccountError error = new AccountError();
                error.Errors.Invalid.Add("Student does not exist");
                throw new AccountException(error);
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
