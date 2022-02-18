using Microsoft.EntityFrameworkCore;
using ServerForReact.Abstract.AbstractHangfire;
using ServerForReact.Data;
using ServerForReact.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerForReact.Services.HangfireServices
{
    public class HangfireCommands : IHangfireCommands
    {
        private readonly AppEFContext context;
        public HangfireCommands(AppEFContext context)
        {
            this.context = context;
        }
        public void CreateSchedule(ScheduleHangfireJob scheduleHangfireJob)
        {
            context.ScheduleHangfireJobs.Add(scheduleHangfireJob);
        }

        public IQueryable<ScheduleHangfireJob> GetSubscriptionHangfire(int subscriptionId)
        {
            return context.ScheduleHangfireJobs.AsNoTracking()
                   .Where(x => x.SubscriptionId == subscriptionId);
        }
    }
}
