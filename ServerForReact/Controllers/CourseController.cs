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
                .Select(x => mapper.Map<CourseItemViewModel>(x))
                .ToList();
            return Ok(list);
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
            try
            {
                courseService.UpdateCourse(model);
                return Ok();
            }
            catch (AccountException aex)
            {
                return BadRequest(aex.AccountError);
            }
            catch (Exception ex)
            {
                return BadRequest(new AccountError("Щось пішло не так! " + ex.Message));
            }
        }

        [HttpPost("subscribe")]
        public IActionResult Subcribe([FromForm] SubscribeViewModel model)
        {
            try
            {
                courseService.Subscribe(model);
                return Ok();
            }
            catch (AccountException aex)
            {
                return BadRequest(aex.AccountError);
            }
            catch (Exception ex)
            {
                return BadRequest(new AccountError("Щось пішло не так! " + ex.Message));
            }
        }
        [Route("unsubscribe")]
        [HttpDelete]
        public IActionResult UnSubcribe([FromForm] SubscribeViewModel model)
        {
            try
            {
                courseService.UnSubscribe(model);
                return Ok();
            }
            catch (AccountException aex)
            {
                return BadRequest(aex.AccountError);
            }
            catch (Exception ex)
            {
                return BadRequest(new AccountError("Щось пішло не так! " + ex.Message));
            }
        }
    }
}
