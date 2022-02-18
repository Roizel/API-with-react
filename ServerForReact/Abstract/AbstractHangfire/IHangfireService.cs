using ServerForReact.Data.Entities;
using ServerForReact.Data.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerForReact.Abstract.AbstractHangfire
{
    public interface IHangfireService
    {
        void DeleteCourseNotifications(int subscriptionId);
        void SetCourseNotifications(StudentCourses subscription, AppUser student, Courses course);
    }
}
