using Microsoft.AspNetCore.Http;
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
        public IFormFile Photo { get; set; }
        public string Duration { get; set; }
        public DateTime StartCourse { get; set; }
    }
    public class CourseError /*Error*/
    {
        public CourseError() { }
        public CourseError(string message)
        {
            Errors.Invalid.Add(message);
        }
        public CourseErrorItem Errors { get; set; } = new CourseErrorItem(); /*exmp of AccountErrorItem */
    }

    public class CourseErrorItem
    {
        public List<string> Invalid { get; set; } = new List<string>(); /*List of Errors*/
    }
}
