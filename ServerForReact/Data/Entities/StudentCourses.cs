using ServerForReact.Data.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ServerForReact.Data.Entities
{
    public class StudentCourses
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Student")]
        public long StudentId { get; set; }
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public Courses Course { get; set; }
        public AppUser Student { get; set; }
        public DateTime JoinCourse { get; set; }
    }
}
