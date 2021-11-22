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
    }
}
