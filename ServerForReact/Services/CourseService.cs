using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NLog;
using ServerForReact.Abstract;
using ServerForReact.Abstract.AbstractHangfire;
using ServerForReact.Data;
using ServerForReact.Data.Entities;
using ServerForReact.Data.Identity;
using ServerForReact.Helpers;
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
        private readonly IHangfireService hangfireService;
        public CourseService(AppEFContext context, IMapper mapper, UserManager<AppUser> userManager, ILogger<CourseService> logger, IHangfireService hangfireService)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.context = context;
            this.logger = logger;
            this.hangfireService = hangfireService;
        }

        public async Task<Courses> CreateCourse(CreateCourseViewModel model)
        {
            var course = mapper.Map<Courses>(model);
            course.PathImg = PhotoHelper.AddPhoto(model.Photo);
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
                return null;
            }
            PhotoHelper.DeletePhoto(course.PathImg);
            context.Courses.Remove(course);
            await context.SaveChangesAsync();
            return "Ok";
        }

        public async Task<Courses> UpdateCourse(SaveEditCourseViewModel model)
        {
            var course = context.Courses
                    .SingleOrDefault(x => x.Id == model.Id);
            if (course != null)
            {
                course.Name = model.Name;
                course.Description = model.Description;
                course.Duration = model.Duration;
                course.StartCourse = model.StartCourse;
                if (model.Photo != null)
                {
                    PhotoHelper.DeletePhoto(course.PathImg);
                    course.PathImg = PhotoHelper.AddPhoto(model.Photo);
                }
                context.Entry(course).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return course;
            }
            else
            {
                return null;
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
                hangfireService.SetCourseNotifications(studentCourses, student, course);
                return studentCourses;
            }
            else
            {
                return null;
            }
        }
        public async Task<StudentCourses> UnSubscribe(SubscribeViewModel model)
        {
            var student = await userManager.FindByNameAsync(model.Name);
            var course = await context.Courses.SingleOrDefaultAsync(x => x.Id == model.CourseId);
            if (student != null && course != null)
            {
                var find = await context.StudentCourses.SingleOrDefaultAsync(x => x.CourseId == model.CourseId && x.StudentId == student.Id);
                context.StudentCourses.Remove(find);
                var subId = await context.StudentCourses.FirstOrDefaultAsync(x=>x.CourseId == course.Id && x.StudentId == student.Id);
                hangfireService.DeleteCourseNotifications(subId.Id);
                await context.SaveChangesAsync();
                return find;
            }
            else
            {
                return null;
            }
        }
    }
}
