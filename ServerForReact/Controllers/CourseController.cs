using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerForReact.Abstract;
using ServerForReact.Data;
using ServerForReact.Data.Entities;
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
            var delete = await courseService.DeleteCourse(id);
            return Ok(new { delete });
        }

        [Route("allcourses")]
        [HttpGet]
        public IActionResult GetCourses()
        {
            Thread.Sleep(1000);
            var list = context.Courses
                .Select(x => mapper.Map<CourseItemViewModel>(x))
                .ToList();
            return Ok(list);
        }

        [Route("studentscourses/{name}")]
        [HttpGet]
        public IActionResult GetCoursesStudent(string name)
        {
            Thread.Sleep(1000);
            var student = context.Users.SingleOrDefault(x => x.UserName == name);
            var list = context.StudentCourses.Select(x => mapper.Map<CourseStudentViewModel>(x)).ToList();
            var list2 = list.Where(x => x.StudentId == student.Id);
            return Ok(list2);
        }

        [Route("editcourse/{id}")]
        [HttpGet]
        public IActionResult EditCourse(int id)
        {
            Thread.Sleep(1000);
            var course = context.Courses
                .SingleOrDefault(x => x.Id == id);
            return Ok(mapper.Map<EditCourseViewModel>(course));
        }

        [HttpPut("savecourse")]
        public IActionResult SaveEditedStudent([FromForm] SaveEditCourseViewModel model)
        {

            courseService.UpdateCourse(model);
            return Ok();

        }

        [HttpPost("subscribe")]
        public IActionResult Subcribe([FromForm] SubscribeViewModel model)
        {

            courseService.Subscribe(model);
            return Ok();

        }
        [HttpPost("unsubscribe")]
        public IActionResult UnSubcribe([FromForm] SubscribeViewModel model)
        {

            courseService.UnSubscribe(model);
            return Ok();

        }
    }
}
