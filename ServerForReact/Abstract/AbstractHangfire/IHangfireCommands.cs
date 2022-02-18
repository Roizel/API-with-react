using ServerForReact.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerForReact.Abstract.AbstractHangfire
{
    public interface IHangfireCommands
    {
        IQueryable<ScheduleHangfireJob> GetSubscriptionHangfire(int subscriptionId);
        void CreateSchedule(ScheduleHangfireJob scheduleHangfireJob);
    }
}
