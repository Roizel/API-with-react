using AutoMapper;
using ServerForReact.Abstract;
using ServerForReact.Data;
using ServerForReact.Data.Entities;
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
        private readonly AppEFContext _context;
        private readonly IMapper _mapper;

        public CourseService(AppEFContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<string> CreateCourse(CreateCourseViewModel model)
        {
            try {
                var course = _mapper.Map<Courses>(model); /*Map AppUser to model*/
                string fileName = String.Empty;
                if (model.Photo != null) /*Images*/
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
                _context.Courses.Add(course); /*Create user*/
                await _context.SaveChangesAsync();
                return "Ok";
            }
            catch (Exception ex)
            {
                return $"Something went wrong on server: {ex.Message}";
            }
        }

        public async Task<string> DeleteCourse(int id)
        {
            try
            {
                var course = _context.Courses.SingleOrDefault(x => x.Id == id);
                if (course == null)
                    return "Course does not exist";    /*Bedrik*/
                if (course.PathImg != null)
                {
                    var directory = Path.Combine(Directory.GetCurrentDirectory(), "images");
                    var FilePath = Path.Combine(directory, course.PathImg);
                    System.IO.File.Delete(FilePath);
                }
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
                return $"Course '{course.Name}' was deleted successfully";
            }
            //catch (AccountException aex) /*If Bad, send errors to Frontend*/
            //{
            //    return $"{aex.AccountError}";
            //}
            catch (Exception ex) /*For undefined exceptions*/
            {
                return $"Something went wrong on server: {ex.Message}"; /*Send bedrik to frontend*/
            }
        }
    }
}
