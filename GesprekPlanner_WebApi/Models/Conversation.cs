﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GesprekPlanner_WebApi.Models
{
    public class Conversation
    {
        public Guid Id { get; set; }
        public virtual ApplicationUserGroup Group { get; set; }
        public DateTime DateTime { get; set; }
        public virtual ConversationType ConversationType { get; set; }
        //public virtual ParentUser Parent { get; set; } // This is gonna be included later.
        public bool IsChosen { get; set; }
    }
}
