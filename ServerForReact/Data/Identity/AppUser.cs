using Microsoft.AspNetCore.Identity;
using ServerForReact.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Web.Cars.Data.Identity;

namespace ServerForReact.Data.Identity
{
    public class AppUser : IdentityUser<long>
    {
        [Required, StringLength(255)]
        public string Photo { get; set; }
        [Required, StringLength(255)]
        public string Surname { get; set; }
        [Required]
        public int Age { get; set; }
        public virtual ICollection<StudentCourses> StudentCourses { get; set; }
        public virtual ICollection<AppUserRole> UserRoles { get; set; }
    }
}
