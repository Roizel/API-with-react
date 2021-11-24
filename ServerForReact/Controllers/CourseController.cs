using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerForReact.Abstract;
using ServerForReact.Data;
using ServerForReact.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ServerForReact.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly AppEFContext _context;
        private readonly IMapper _mapper;
        public CourseController(ICourseService courseService, AppEFContext context, IMapper mapper)
        {
            _courseService = courseService;
            _context = context;
            _mapper = mapper;
        }
        [HttpPost("createcourse")]
        public async Task<IActionResult> Create([FromForm] CreateCourseViewModel model)
        {
            try /*If Good, send OK to Frontend*/
            {
                string result = await _courseService.CreateCourse(model); /*Create user*/
                if (result == null)
                    return BadRequest(); /*Bedrik*/

                return Ok(new
                {
                    result /*All good*/
                });
            }
            //catch (AccountException aex) /*If Bad, send errors to Frontend*/
            //{

            //    return BadRequest(aex.AccountError);
            //}
            catch (Exception ex) /*For undefined exceptions*/
            {
                return BadRequest(new AccountError("Something went wrong on server" + ex.Message)); /*Send bedrik to frontend*/
            }
        }
        [Route("deletecourse/{id}")]
        [HttpDelete]
        public async Task<string> DeleteStudent(int id)
        {
            try
            {
                string delete = await _courseService.DeleteCourse(id);
                return delete;
            }
            catch (Exception ex)
            {
                return $"Server erroe: {ex.Message}";
            }
        }
        [Route("allcourses")]
        [HttpGet]
        public IActionResult GetCourses()
        {
            Thread.Sleep(1000);
            var list = _context.Courses
                .Select(x => x)
                .ToList();
            return Ok(list);
        }
    }
}
