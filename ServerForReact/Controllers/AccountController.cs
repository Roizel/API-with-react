using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServerForReact.Abstract;
using ServerForReact.Data.Identity;
using ServerForReact.Exceptions;
using ServerForReact.Logger.Contracts;
using ServerForReact.Models;
using ServerForReact.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ServerForReact.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IStudentService studentService;
        private readonly UserManager<AppUser> userManager;
        private readonly IJwtTokenService tokenService;
        private readonly IMapper mapper;
        private readonly ILoggerManager logger;
        public AccountController(IStudentService _studentService, ILoggerManager _logger, UserManager<AppUser> _userManager,
            IJwtTokenService _tokenService, IMapper _mapper)
        {
            mapper = _mapper;
            studentService = _studentService;
            userManager = _userManager;
            tokenService = _tokenService;
            logger = _logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterViewModel model)
        {

            string token = await studentService.CreateStudent(model);
            if (token == null)
            {
                return BadRequest();
            }
            return Ok(new
            {
                token
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginViewModel model)
        {
            string token = await studentService.LoginStudent(model);
            var student = await userManager.FindByEmailAsync(model.Email);
            bool IsAdmin = await userManager.IsInRoleAsync(student, "Admin");
            if (token == null)
            {
                return BadRequest();
            }
            return Ok(new { token, IsAdmin });
        }

        [Route("delete/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            string delete = await studentService.DeleteStudent(id);
            return Ok(new { delete });
        }

        [Route("allstudents")]
        [HttpGet]
        public IActionResult GetStudents()
        {
            Thread.Sleep(1000);
            var list = userManager.Users
                .Select(x => mapper.Map<StudentViewModel>(x))
                .ToList();
            throw new Exception("...");
            return Ok(list);
        }
        [Route("editstudent/{id}")]
        [HttpGet]
        public IActionResult EditStudent(int id)
        {
            Thread.Sleep(1000);
            var student = userManager.Users
                .SingleOrDefault(x => x.Id == id);
            return Ok(mapper.Map<EditStudentViewModel>(student));
        }

        [HttpPut("savestudent")]
        public IActionResult SaveEditedStudent([FromForm] SaveEditStudentViewModel model)
        {
            studentService.UpdateStudent(model);
            return Ok();
        }
    }
}
