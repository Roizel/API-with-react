﻿using Microsoft.AspNetCore.Http;
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
}