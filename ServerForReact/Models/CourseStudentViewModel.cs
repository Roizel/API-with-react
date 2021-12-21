using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerForReact.Models
{
    public class CourseStudentViewModel
    {
        public long StudentId { get; set; }
        public int CourseId { get; set; }
        public DateTime JoinCourse { get; set; }
    }
}
