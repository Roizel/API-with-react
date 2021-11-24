using ServerForReact.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerForReact.Abstract
{
    public interface ICourseService
    {
        public Task<string> CreateCourse(CreateCourseViewModel model);
        public Task<string> DeleteCourse(int id);
    }
}
