using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GesprekPlanner_WebApi.Models;

namespace GesprekPlanner_WebApi.Areas.Teacher.Models
{
    public class CreateSchedule
    {
        public string FormId { get; set; }
        [Required]
        public string StartTime { get; set; }
        [Required]
        public string EndTime { get; set; }
        [Required]
        public int ConversationType { get; set; }
        public DateTime Date { get; set; }
    }
}
