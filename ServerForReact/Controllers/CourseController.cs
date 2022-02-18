using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServerForReact.Abstract;
using ServerForReact.Data;
using ServerForReact.Data.Entities;
using ServerForReact.Data.Identity;
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
        private readonly ILogger<CourseController> logger;
        public CourseController(ICourseService courseService, AppEFContext context, IMapper mapper, ILogger<CourseController> logger)
        {
            this.courseService = courseService;
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpPost("createcourse")]
        public async Task<IActionResult> Create([FromForm] CreateCourseViewModel model)
        {
            var result = await courseService.CreateCourse(model);
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
            if (delete == null)
            {
                return StatusCode(404);
            }
            return Ok();
        }

        //[Route("studentscourses/{name}")]
        //[HttpGet]
        //public IActionResult GetCoursesStudent(string name)
        //{
        //    var student = context.Users.SingleOrDefault(x => x.UserName == name);
        //    var list = context.StudentCourses.Select(x => mapper.Map<CourseStudentViewModel>(x)).ToList();
        //    var subs = list.Where(x => x.StudentId == student.Id);
        //    return Ok(subs);
        //}
        [Route("allcourseswithstudents")]
        [HttpGet]
        public IActionResult GetAllCoursesWithStudents()
        {
            var courses = context.Courses.Join(context.StudentCourses,
                u => u.Id,
                c => c.CourseId,
                (u,c) => new {Name = u.Name, Id = c.StudentId}).ToList();

            return Ok(courses);
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
            var check = courseService.UpdateCourse(model);
            if (check.Result == null)
            {
                return StatusCode(404);
            }
            return Ok();
        }

        [HttpPost("subscribe")]
        public IActionResult Subcribe([FromForm] SubscribeViewModel model)
        {

            var check = courseService.Subscribe(model);
            if (check.Result == null)
            {
                return StatusCode(404);
            }
            return Ok();

        }
        [HttpPost("unsubscribe")]
        public IActionResult UnSubcribe([FromForm] SubscribeViewModel model)
        {
            var check = courseService.UnSubscribe(model);
            if (check.Result == null)
            {
                return StatusCode(404);
            }
            return Ok();
        }
    }
}
