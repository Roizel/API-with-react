using Hangfire;
using ServerForReact.Abstract;
using ServerForReact.Abstract.AbstractHangfire;
using ServerForReact.Data;
using ServerForReact.Data.Entities;
using ServerForReact.Data.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerForReact.Services.HangfireServices
{
    public class HangfireService : IHangfireService
    {
        private readonly AppEFContext context;
        private readonly IEmailService emailService;
        private readonly IHangfireCommands hangfireCommands;

        public HangfireService(AppEFContext context, IEmailService emailService, IHangfireCommands hangfireCommands)
        {
            this.context = context;
            this.emailService = emailService;
            this.hangfireCommands = hangfireCommands;
        }
        public void DeleteCourseNotifications(int subscriptionId)
        {
            var jobs = hangfireCommands.GetSubscriptionHangfire(subscriptionId);

            foreach (var job in jobs)
            {
                BackgroundJob.Delete(job.JobId);
            }
        }

        public void SetCourseNotifications(StudentCourses subscription, AppUser student, Courses course)
        {
            var daysToCourse = course.StartCourse - DateTime.UtcNow;

            int oneDay = 1;
            int sevenDays = 7;
            int thirtyDays = 30;
            DateTime tmp = new DateTime(course.StartCourse.Year, course.StartCourse.Month, course.StartCourse.Day);
            if (daysToCourse.Days >= oneDay)
            {

                var JobOneDayId = BackgroundJob.Schedule(
                    () => emailService.SendEmailAsync(student.Email, "Start of course", $"Hi! Don`t forget, course start at {tmp}"),
                    course.StartCourse.AddHours(-20));

                hangfireCommands.CreateSchedule(new ScheduleHangfireJob
                {
                    JobId = JobOneDayId,
                    SubscriptionId = subscription.Id
                });

                if (daysToCourse.Days >= sevenDays)
                {
                    var JobSevenDaysId = BackgroundJob.Schedule(
                        () => emailService.SendEmailAsync(student.Email, "Start of course", $"Hi! Don`t forget, course start at {tmp}"),
                       course.StartCourse.AddDays(-7));

                    hangfireCommands.CreateSchedule(new ScheduleHangfireJob
                    {
                        JobId = JobSevenDaysId,
                        SubscriptionId = subscription.Id
                    });
                }
                if (daysToCourse.Days >= thirtyDays)
                {
                    var JobMounthId = BackgroundJob.Schedule(
                       () => emailService.SendEmailAsync(student.Email, "Start of course", $"Hi! Don`t forget, course start at {tmp}"),
                      course.StartCourse.AddDays(-30));

                    hangfireCommands.CreateSchedule(new ScheduleHangfireJob
                    {
                        JobId = JobMounthId,
                        SubscriptionId = subscription.Id
                    });
                }
            }
            context.SaveChanges();
        }
    }
}
