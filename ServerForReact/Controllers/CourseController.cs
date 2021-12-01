using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerForReact.Abstract;
using ServerForReact.Data;
using ServerForReact.Data.Identity;
using ServerForReact.Exceptions;
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
        private readonly ICourseService courseService;
        private readonly AppEFContext context;
        private readonly IMapper mapper;
        public CourseController(ICourseService _courseService, AppEFContext _context, IMapper _mapper)
        {
            courseService = _courseService;
            context = _context;
            mapper = _mapper;
        }

        [HttpPost("createcourse")]
        public async Task<IActionResult> Create([FromForm] CreateCourseViewModel model)
        {
            var result = await courseService.CreateCourse(model);
            if (result == null)
            {
                return BadRequest();
            }

            return Ok(new
            {
                result
            });
        }

        [Route("deletecourse/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            try
            {
                var delete = await courseService.DeleteCourse(id);
                return Ok(new { delete });
            }
            catch (CourseException cex)
            {
                return BadRequest(cex.CourseError);
            }
        }

        [Route("allcourses")]
        [HttpGet]
        public IActionResult GetCourses()
        {
            Thread.Sleep(1000);
            var list = context.Courses
                .Select(x => x)
                .ToList();
            return Ok(list);
        }
    }
}
