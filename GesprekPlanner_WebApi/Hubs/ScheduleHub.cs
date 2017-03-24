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
        
    }
}
