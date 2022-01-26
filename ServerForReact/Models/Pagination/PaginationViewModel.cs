using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerForReact.Models.Pagination
{
    public class CoursePaginationViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public string StartCourse { get; set; }
        public int Page { get; set; } = 1;
    }

    public class CoursePaginationResultViewModel
    {
        public List<CourseItemViewModel> Courses { get; set; }
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
        public int Total { get; set; }
    }

    public class StudentPaginationViewModel
    {
        public string SearchWord { get; set; }
        public int Page { get; set; } = 1;
    }

    public class StudentPaginationResultViewModel
    {
        public List<StudentViewModel> Students { get; set; }
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
        public int Total { get; set; }
    }
}
