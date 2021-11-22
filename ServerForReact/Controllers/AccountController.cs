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
using System.Threading.Tasks;

namespace ServerForReact.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IStudentService studentService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtTokenService _tokenService;
        public AccountController(IStudentService _studentService, UserManager<AppUser> userManager, IJwtTokenService tokenService)
        {
            studentService = _studentService;
            _userManager = userManager;
            _tokenService = tokenService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterViewModel model)
        {
            try /*If Good, send OK to Frontend*/
            {
                string token = await studentService.CreateStudent(model); /*Create user*/
                if (token == null)
                    return BadRequest(); /*Bedrik*/

                return Ok(new
                {
                    token /*All good*/
                });
            }
            catch (AccountException aex) /*If Bad, send errors to Frontend*/
            {

                return BadRequest(aex.AccountError);
            }
            catch (Exception ex) /*For undefined exceptions*/
            {
                return BadRequest(new AccountError("Something went wrong on server" + ex.Message)); /*Send bedrik to frontend*/
            }
        }
    }
}
