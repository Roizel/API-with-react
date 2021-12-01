using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServerForReact.Abstract;
using ServerForReact.Data.Identity;
using ServerForReact.Exceptions;
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
        public AccountController(IStudentService _studentService, UserManager<AppUser> _userManager, IJwtTokenService _tokenService, IMapper _mapper)
        {
            mapper = _mapper;
            studentService = _studentService;
            userManager = _userManager;
            tokenService = _tokenService;
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
            if (token == null)
                return BadRequest();
            return Ok(new { token });
        }

        [Route("delete/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            try
            {
                string delete = await studentService.DeleteStudent(id);
                return Ok(new { delete });
            }
            catch (AccountException aex)
            {
                return BadRequest(aex.AccountError);
            }
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
    }
}
