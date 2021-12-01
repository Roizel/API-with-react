using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ServerForReact.Abstract;
using ServerForReact.Data;
using ServerForReact.Data.Entities;
using ServerForReact.Exceptions;
using ServerForReact.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ServerForReact.Services
{
    public class CourseService : ICourseService
    {
        private readonly AppEFContext context;
        private readonly IMapper mapper;

        public CourseService(AppEFContext _context, IMapper _mapper)
        {
            mapper = _mapper;
            context = _context;
        }

        public async Task<Courses> CreateCourse(CreateCourseViewModel model)
        {
            var course = mapper.Map<Courses>(model);
            string fileName = String.Empty;

            if (model.Photo != null)
            {
                string randomFilename = Path.GetRandomFileName() +
                    Path.GetExtension(model.Photo.FileName);

                string dirPath = Path.Combine(Directory.GetCurrentDirectory(), "images");
                fileName = Path.Combine(dirPath, randomFilename);
                using (var file = System.IO.File.Create(fileName))
                {
                    model.Photo.CopyTo(file);
                }
                course.PathImg = randomFilename;
            }

            course.Name = model.Name;
            course.StartCourse = model.StartCourse;
            course.Description = model.Description;
            course.Duration = model.Duration;
            context.Courses.Add(course);
            await context.SaveChangesAsync();
            return course;
        }

        public async Task<string> DeleteCourse(int id)
        {
            var course = context.Courses.SingleOrDefault(x => x.Id == id);
            if (course == null)
            {
                CourseError error = new CourseError();
                error.Errors.Invalid.Add("This id does not exist");
                throw new CourseException(error);
            }
            if (course.PathImg != null)
            {
                var directory = Path.Combine(Directory.GetCurrentDirectory(), "images");
                var FilePath = Path.Combine(directory, course.PathImg);
                System.IO.File.Delete(FilePath);
            }
            context.Courses.Remove(course);
            await context.SaveChangesAsync();
            return $"Course {course.Name}, Id: {course.Id} was deleted successfully";
        }
    }
}
