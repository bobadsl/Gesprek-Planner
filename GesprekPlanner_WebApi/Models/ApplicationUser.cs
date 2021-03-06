﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace GesprekPlanner_WebApi.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public virtual ApplicationUserGroup Group { get; set; }
        public virtual School School { get; set; }
        public bool IsInMailGroup { get; set; }
    }
}
