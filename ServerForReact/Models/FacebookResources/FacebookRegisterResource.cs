using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerForReact.Models.FacebookResources
{
    public class FacebookRegisterResource
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public IFormFile Photo { get; set; }
        public int Age { get; set; }
        public string Phone { get; set; }
    }
}
