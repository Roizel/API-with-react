using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerForReact.Models
{
    public class EditStudentViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Photo { get; set; }
        public int Age { get; set; }
        public string Phone { get; set; }
    }
    public class SaveEditStudentViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public IFormFile Photo { get; set; }
        public string Image { get; set; }
        public int Age { get; set; }
        public string Phone { get; set; }
    }
}
