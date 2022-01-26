using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerForReact.Models.Pagination;
using ServerForReact.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerForReact.Controllers
{
    [Route("api/pagination")]
    [ApiController]
    public class PaginationController : ControllerBase
    {
        private readonly CoursePagination CoursePg;
        private readonly StudentPagination StudentPg;
        public PaginationController(CoursePagination CoursePg, StudentPagination StudentPg)
        {
            this.CoursePg = CoursePg;
            this.StudentPg = StudentPg;
        }

        [Route("allstudents")]
        [HttpGet]
        public IActionResult GetStudents()
        {
            return Ok(StudentPg.All());
        }

        [HttpPost("coursepagination")]
        public IActionResult SortCourses([FromQuery] CoursePaginationViewModel model)
        {
            var res = CoursePg.Query(model);
            return Ok(res);
        }

        [HttpPost("studentpagination")]
        public IActionResult SortStudents([FromForm] StudentPaginationViewModel model)
        {
            var res = StudentPg.UserSorting(model);
            return Ok(res);
        }
    }
}
