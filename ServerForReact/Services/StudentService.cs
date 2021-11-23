using AutoMapper;
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
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly AppEFContext _context;
        public StudentService(UserManager<AppUser> userManager, IJwtTokenService jwtTokenService, SignInManager<AppUser> signInManager,
            IMapper mapper, AppEFContext context)
        {
            _mapper = mapper;
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
            _context = context;
        }
        public async Task<string> CreateStudent(RegisterViewModel model)
        {
            var student = _mapper.Map<AppUser>(model); /*Map AppUser to model*/
            string fileName = String.Empty;
            if (model.Photo != null) /*Images*/
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
            var result = await _userManager.CreateAsync(student, model.Password); /*Create user*/
            if (!result.Succeeded)
            {
                if (!string.IsNullOrEmpty(fileName))
                    System.IO.File.Delete(fileName);
                AccountError accountError = new AccountError(); /*Create castom Exception*/
                foreach (var item in result.Errors)
                {
                    accountError.Errors.Invalid.Add(item.Description); /*Add Exceptions to our castom Exceptions*/
                }
                throw new AccountException(accountError); /*Send errors to AccountController*/
            }

            return _jwtTokenService.CreateToken(student);

        }

        public string DeleteStudent(int id)
        {
            try
            {
                var student = _userManager.Users.SingleOrDefault(x => x.Id == id);
                if (student == null)
                    return "Student does not exist";    /*Bedrik*/
                if (student.Photo != null)
                {
                    var directory = Path.Combine(Directory.GetCurrentDirectory(), "images");
                    var FilePath = Path.Combine(directory, student.Photo);
                    System.IO.File.Delete(FilePath);
                }
                _context.Users.Remove(student);
                _context.SaveChangesAsync();
                return $"Student {student.UserName} {student.Surname} wad deleted successfully";
            }
            catch (AccountException aex) /*If Bad, send errors to Frontend*/
            {
                return $"{aex.AccountError}";
            }
            catch (Exception ex) /*For undefined exceptions*/
            {
                return $"Something went wrong on server: {ex.Message}"; /*Send bedrik to frontend*/
            }
        }

        public async Task<string> LoginStudent(LoginViewModel model) /*Під очень великім вопросом*/
        {
            try
            {
                var student = await _userManager.FindByEmailAsync(model.Email); /*І так ясно*/
                if (student == null)
                    return "Email does not exist";
                if (await _userManager.CheckPasswordAsync(student, model.Password)) /*І так ясно*/
                {
                    string token = _jwtTokenService.CreateToken(student); /*Create token and send it to client*/
                    return token;
                }
                else
                {
                    //var exc = new AccountError();
                    //exc.Errors.Invalid.Add("Wrong password!");
                    return "Wrong password";
                }
            }
            catch (AccountException aex)
            {
                return $"Error: {aex.AccountError}";
            }
            catch (Exception ex)
            {
                return $"Something went wrong on server: {ex.Message}";
            }
        }
    }
}
