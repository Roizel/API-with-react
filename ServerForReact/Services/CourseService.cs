using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NLog;
using ServerForReact.Abstract;
using ServerForReact.Data;
using ServerForReact.Data.Entities;
using ServerForReact.Data.Identity;
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
        private readonly UserManager<AppUser> userManager;
        private readonly IMapper mapper;
        private readonly ILogger<CourseService> logger;
        public CourseService(AppEFContext _context, IMapper _mapper, UserManager<AppUser> _userManager, ILogger<CourseService> _logger)
        {
            userManager = _userManager;
            mapper = _mapper;
            context = _context;
            logger = _logger;
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
            logger.LogInformation($"Course {course.Name} was created");
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
            logger.LogInformation($"Course {course.Name}, Id: {course.Id} was deleted successfully");
            return $"Course {course.Name}, Id: {course.Id} was deleted successfully";
        }

        public async Task<Courses> UpdateCourse(SaveEditCourseViewModel model)
        {
            var course = context.Courses
                    .SingleOrDefault(x => x.Id == model.Id);
            if (course != null)
            {
                logger.LogInformation($"Id: {course.Id}, Course {course.Name} is updating");
                course.Name = model.Name;
                course.Description = model.Description;
                course.Duration = model.Duration;
                course.StartCourse = model.StartCourse;
                string fileName = String.Empty;
                if (model.Photo != null)
                {
                    if (course.PathImg != null)
                    {
                        var directory = Path.Combine(Directory.GetCurrentDirectory(), "images");
                        var FilePath = Path.Combine(directory, course.PathImg);
                        System.IO.File.Delete(FilePath);
                    }
                    if (model.Photo != null)
                    {
                        var ext = Path.GetExtension(model.Photo.FileName);
                        fileName = Path.GetRandomFileName() + ext;
                        var dir = Path.Combine(Directory.GetCurrentDirectory(), "images");
                        var filePath = Path.Combine(dir, fileName);
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            model.Photo.CopyTo(stream);
                        }
                        course.PathImg = fileName;
                    }
                }
                context.Entry(course).State = EntityState.Modified;
                logger.LogInformation($"Id: {course.Id}, was updated to {course.Name}");
                await context.SaveChangesAsync();
                return course;
            }
            else
            {
                AccountError error = new AccountError();
                error.Errors.Invalid.Add("Course does not exist");
                throw new AccountException(error);
            }
        }
        public async Task<StudentCourses> Subscribe(SubscribeViewModel model)
        {
            var student = await userManager.FindByNameAsync(model.Name);
            var course = await context.Courses.SingleOrDefaultAsync(x => x.Id == model.CourseId);
            if (student != null && course != null)
            {
                StudentCourses studentCourses = new StudentCourses();
                studentCourses.CourseId = course.Id;
                studentCourses.StudentId = student.Id;
                studentCourses.JoinCourse = DateTime.Now;
                context.StudentCourses.Add(studentCourses);
                await context.SaveChangesAsync();
                logger.LogInformation($"Student {student.Id}, {student.UserName} {student.Surname} was subscribed on course: {course.Id}, {course.Name}");
                return studentCourses;
            }
            else
            {
                AccountError error = new AccountError();
                error.Errors.Invalid.Add("Something went wrong");
                throw new AccountException(error);
            }
        }
        public async Task<StudentCourses> UnSubscribe(SubscribeViewModel model)
        {
            var student = await userManager.FindByNameAsync(model.Name);
            var course = await context.Courses.SingleOrDefaultAsync(x => x.Id == model.CourseId);
            if (student != null && course != null)
            {
                var find = await context.StudentCourses.SingleOrDefaultAsync(x => x.CourseId == model.CourseId && x.StudentId == student.Id);
                if (find != null)
                {
                    context.StudentCourses.Remove(find);
                    await context.SaveChangesAsync();
                    logger.LogInformation($"Student {student.Id}, {student.UserName} {student.Surname} was Unsubscribed from course: {course.Id}, {course.Name}");
                    return find;
                }
                else
                {
                    AccountError error = new AccountError();
                    error.Errors.Invalid.Add("U don`t subs to this course");
                    throw new AccountException(error);
                }
            }
            else
            {
                AccountError error = new AccountError();
                error.Errors.Invalid.Add("Something went wrong");
                throw new AccountException(error);
            }
        }
    }
}
