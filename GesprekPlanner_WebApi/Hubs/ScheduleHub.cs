using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GesprekPlanner_WebApi.Areas.Teacher.Models;
using GesprekPlanner_WebApi.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Hubs;
using Newtonsoft.Json;

namespace GesprekPlanner_WebApi.Hubs
{
    [Authorize]
    [HubName("Schedule")]
    public class ScheduleHub : Hub
    {
        private readonly ApplicationDbContext _dbContext;

        public ScheduleHub(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void CreateSchedules(string jsonString)
        {
            var createSchedule = (CreateSchedule)JsonConvert.DeserializeObject(jsonString, typeof(CreateSchedule));
            var startTime = TimeSpan.ParseExact(createSchedule.StartTime, "H:m", null);
            var endTime = TimeSpan.ParseExact(createSchedule.EndTime, "H:m", null);
            var conversationType = _dbContext.ConversationTypes.First(ct => ct.Id == createSchedule.ConversationType);
            List<CreateSchedule> list = new List<CreateSchedule>();
            while (true)
            {
                if (startTime.Equals(endTime) || startTime.Ticks > endTime.Ticks) break;
                CreateSchedule schedule = new CreateSchedule();
                schedule.StartTime = startTime.ToString("H:m");
                startTime = startTime.Add(new TimeSpan(0, conversationType.ConversationDuration, 0));
                schedule.EndTime = startTime.ToString("H:m");
            }
        }
    }
}
