using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ServerForReact.Data.Entities
{
    public class ScheduleHangfireJob
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string JobId { get; set; }

        [ForeignKey("Course")]
        public int SubscriptionId { get; set; }

        public StudentCourses StudentsSubscription { get; set; }
    }
}
