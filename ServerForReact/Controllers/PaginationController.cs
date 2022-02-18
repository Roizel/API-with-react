using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerForReact.Models.Pagination;
using ServerForReact.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ServerForReact.Controllers
{
    [Route("api/pagination")]
    [ApiController]
    public class PaginationController : ControllerBase
    {
        private readonly CoursePagination CoursePg;
        private readonly StudentPagination StudentPg;
        private readonly CourseSubsPagination CourseSubsPg;
        public PaginationController(CoursePagination CoursePg, StudentPagination StudentPg, CourseSubsPagination CourseSubsPg)
        {
            this.CoursePg = CoursePg;
            this.StudentPg = StudentPg;
            this.CourseSubsPg = CourseSubsPg;
        }

        [HttpPost("coursepagination")]
        public IActionResult SortCourses([FromForm] CoursePaginationViewModel model)
        {
            var res = CoursePg.CoursesSorting(model);
            return Ok(res);
        }

        [HttpPost("studentpagination")]
        public IActionResult SortStudents([FromForm] StudentPaginationViewModel model)
        {
            var res = StudentPg.UsersSorting(model);
            return Ok(res);
        }

        [HttpPost("sortcoursessubspagination")]
        public IActionResult SortCoursesSubs([FromForm] CourseSubsPaginationViewModel model)
        {
            var res = CourseSubsPg.CoursesSubsSorting(model);
            return Ok(res);
        }
    }
}
