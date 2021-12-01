using ServerForReact.Data.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Web.Cars.Data.Identity;

namespace ServerForReact.Data.Entities
{
    [Table("tblCourses")]
    public class Courses
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(255)]
        public string Name { get; set; }
        [Required, StringLength(255)]
        public string Description { get; set; }
        [Required, StringLength(1000)]
        public string PathImg { get; set; }
        [Required, StringLength(255)]
        public string Duration { get; set; }
        [Required]
        public DateTime StartCourse { get; set; }
        public virtual ICollection<StudentCourses> StudentCourses { get; set; }
    }
}
