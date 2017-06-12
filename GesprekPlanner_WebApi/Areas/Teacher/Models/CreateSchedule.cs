using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using GesprekPlanner_WebApi.Models;

namespace GesprekPlanner_WebApi.Areas.Teacher.Models
{
    public class CreateSchedule : IValidatableObject
    {
        public string FormId { get; set; }
        [Required]
        public string StartTime { get; set; }
        [Required]
        public string EndTime { get; set; }
        [Required]
        public int ConversationType { get; set; }
        public DateTime Date { get; set; }
        public bool IsPlanned { get; set; }
        public string Error { get; private set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            TimeSpan startTime, endTime;
            if (!TimeSpan.TryParse(StartTime, out startTime) || !TimeSpan.TryParse(EndTime, out endTime))
            {
                Error = "De tijd is niet in een goed formaat. \r\nHet formaat is uur:minuut";
                yield return new ValidationResult("De tijd is niet in een goed formaat. \r\nHet formaat is uur:minuut");
            }
            if (startTime > endTime)
            {
                Error = "De starttijd mag niet kleiner zijn dan de eindtijd";
                yield return new ValidationResult("De starttijd mag niet kleiner zijn dan de eindtijd");
            }
        }
    }
}
