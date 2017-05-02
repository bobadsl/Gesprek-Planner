using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GesprekPlanner_WebApi.Models
{
    public class Conversation
    {
        public Guid Id { get; set; }
        public virtual ApplicationUserGroup Group { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateTime { get; set; }
        public DateTime EndTime { get; set; }
        public virtual ConversationType ConversationType { get; set; }
        //public virtual ParentUser Parent { get; set; } // This is gonna be included later.
        public bool IsChosen { get; set; }
        public virtual School School { get; set; }
    }
}
