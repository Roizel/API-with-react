using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using ServerForReact.Abstract;
using ServerForReact.Data.Identity;
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
        private readonly ILogger<AccountController> logger;
        private readonly IEmailService emailService;
        public AccountController(IStudentService _studentService, UserManager<AppUser> _userManager,
            IJwtTokenService _tokenService, IMapper _mapper, ILogger<AccountController> _logger, IEmailService _emailService)
        {
            mapper = _mapper;
            studentService = _studentService;
            userManager = _userManager;
            tokenService = _tokenService;
            logger = _logger;
            emailService = _emailService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterViewModel model)
        {

            var result = await studentService.CreateStudent(model);

            if (result.token == null)
            {
                return BadRequest();
            }
            else
            {
                if (result.student != null)
                {
                    var code = await userManager.GenerateEmailConfirmationTokenAsync(result.student);
                    var callbackUrl = Url.Action("ConfirmEmail",
                                                 "Account",
                                                 new { userId = result.student.Id, code = code },
                                                 protocol: HttpContext.Request.Scheme);

                    await emailService.SendEmailAsync(
                                                     model.Email, 
                                                     "Confirm your account",
                                                     $"Confirm registration, go to link: <a href='{callbackUrl}'>link</a>");

                    logger.LogInformation($"Email was send: {model.Email}");
                }
                return Ok(new
                {
                    result.token
                });
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return BadRequest();
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest();
            }

            var result = await userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                logger.LogInformation($"Student {user.UserName} {user.Surname}, {user.Email} confirmed account");
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginViewModel model)
        {
            var result = await studentService.LoginStudent(model);
            if (result.token == "")
            {
                return BadRequest();
            }
            logger.LogInformation($"Robe");
            return Ok(new { result.token, result.IsAdmin });
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
