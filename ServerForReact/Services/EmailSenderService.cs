using ServerForReact.Abstract;
using ServerForReact.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerForReact.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly AppEFContext context;
        private readonly IEmailService emailService;

        public EmailSenderService(AppEFContext context, IEmailService emailService)
        {
            this.context = context;
            this.emailService = emailService;
        }
        public Task Day()
        {
            var startCourse = context.Courses.Where(x => x.StartCourse >= DateTime.Now);
            var joinCourse = context.StudentCourses.Select(x => x);

            foreach (var item in startCourse)
            {
                foreach (var item2 in joinCourse)
                {
                    DateTime tmp = item2.JoinCourse; /*2021-12-15 13:37:30.32235*/
                    DateTime date = new DateTime(tmp.Year, tmp.Month, tmp.Day); /*2021-12-15 00:00:00.00000*/
                    DateTime dateNow = DateTime.Now; /*Get today`s date*/
                    dateNow = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day); /*parsing it*/

                    date = date.AddDays(1);
                    if (date == item.StartCourse && date == dateNow)
                    {
                        var student = context.Users.Where(x => x.Id == item2.StudentId);
                        string email = student.Select(x => x.Email).Single();
                        emailService.SendEmailAsync(email, "From Administration", "Don't forget the course starts tomorrow");
                    }
                }
            }
            return Task.CompletedTask;
        }
        public void Week()
        {
            var startCourse = context.Courses.Where(x => x.StartCourse >= DateTime.Now);
            var joinCourse = context.StudentCourses.Select(x => x);

            foreach (var item in startCourse)
            {
                foreach (var item2 in joinCourse)
                {
                    DateTime tmp = item2.JoinCourse; /*2021-12-15 13:37:30.32235*/
                    DateTime date = new DateTime(tmp.Year, tmp.Month, tmp.Day); /*2021-12-15 00:00:00.00000*/
                    DateTime dateNow = DateTime.Now; /*Get today`s date*/
                    dateNow = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day); /*parsing it*/

                    date = date.AddDays(7);
                    if (date == item.StartCourse && date == dateNow)
                    {
                        var student = context.Users.Where(x => x.Id == item2.StudentId);
                        string email = student.Select(x => x.Email).Single();
                        emailService.SendEmailAsync(email, "From Administration", "Don't forget, the course starts in a week");
                    }
                }
            }
        }
        public void Mounth()
        {
            var startCourse = context.Courses.Where(x => x.StartCourse >= DateTime.Now);
            var joinCourse = context.StudentCourses.Select(x => x);

            foreach (var item in startCourse)
            {
                foreach (var item2 in joinCourse)
                {
                    DateTime tmp = item2.JoinCourse; /*2021-12-15 13:37:30.32235*/
                    DateTime date = new DateTime(tmp.Year, tmp.Month, tmp.Day); /*2021-12-15 00:00:00.00000*/
                    DateTime dateNow = DateTime.Now; /*Get today`s date*/
                    dateNow = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day); /*parsing it*/
                    date = date.AddMonths(1);

                    if (date == item.StartCourse && date == dateNow)
                    {
                        var student = context.Users.Where(x => x.Id == item2.StudentId);
                        string email = student.Select(x => x.Email).Single();
                        emailService.SendEmailAsync(email, "From Administration", "Don't forget, the course starts in a month");
                    }
                }
            }
        }
    }
}
