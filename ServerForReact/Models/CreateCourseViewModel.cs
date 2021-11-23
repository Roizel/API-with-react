using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerForReact.Models
{
    public class CreateCourseViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public string Duration { get; set; }
        public string StartCourse { get; set; }
    }
}
