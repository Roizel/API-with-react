using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerForReact.Models
{
    public class StudentViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Photo { get; set; }
        public int Age { get; set; }
        public int Phone { get; set; }
    }
}
