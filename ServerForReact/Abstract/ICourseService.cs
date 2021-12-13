using ServerForReact.Data.Entities;
using ServerForReact.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerForReact.Abstract
{
    public interface ICourseService
    {
        public Task<Courses> CreateCourse(CreateCourseViewModel model);
        public Task<string> DeleteCourse(int id);
        public Task<Courses> UpdateCourse(SaveEditCourseViewModel model);
        public Task<StudentCourses> Subscribe(SubscribeViewModel model);
        public Task<StudentCourses> UnSubscribe(SubscribeViewModel model);

    }
}
