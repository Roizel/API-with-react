using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerForReact.Models.Pagination
{
    public class CoursePaginationViewModel
    {
        public string SearchWord { get; set; }
        public string Sort { get; set; }
        public string TypeOfSort { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
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
        public string Sort { get; set; }
        public string TypeOfSort { get; set; }
        public int PageSize { get; set; } = 10;
        public int Page { get; set; } = 1;
    }

    public class StudentPaginationResultViewModel
    {
        public List<StudentViewModel> Students { get; set; }
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
        public int Total { get; set; }
    }
    public class CourseSubsPaginationViewModel
    {
        public string SearchWord { get; set; }
        public string Sort { get; set; }
        public string TypeOfSort { get; set; }
        public string Name { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
    public class CourseSubsPaginationResultViewModel
    {
        public List<CourseItemViewModel> Courses { get; set; }
        public IEnumerable<CourseStudentViewModel> subscriptions { get; set; }
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
        public int Total { get; set; }
    }
}
