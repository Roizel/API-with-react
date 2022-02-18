using Hangfire;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using ServerForReact.Abstract;
using ServerForReact.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerForReact.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration configuration;
        private readonly IViewsHelper viewHelper;
        public EmailService(IConfiguration configuration, IViewsHelper viewHelper)
        {
            this.viewHelper = viewHelper;
            this.configuration = configuration;
        }
        public async Task SendCourseStartEmailAsync(string courseTitle, string startIn, string userEmail)
        {
            var message = await viewHelper.GetViewToHtmlAsync<CourseItemViewModel>(
                "EmailSubscribeCourse",
                new CourseItemViewModel
                {
                    Id = 1,
                    Description = "",
                    Duration = "",
                    Name = "",
                    Photo = "",
                    StartCourse = ""
                });

            await SendEmailAsync(userEmail, "Course start", message);
        }


        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SendGridClient(configuration.GetValue<String>("SendGridKey"));
            var from = new EmailAddress(configuration.GetValue<String>("Email"), configuration.GetValue<String>("SenderName"));
            var to = new EmailAddress(email, email);
            var plainTextContent = "";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, message);

            var result = await client.SendEmailAsync(msg);
        }
    }
}
