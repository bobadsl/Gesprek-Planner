using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace GesprekPlanner_WebApi.Models
{
    public class ApplicationUserGroup
    {
        public int ApplicationUserGroupId { get; set; }
        public string GroupName { get; set; }
    }
}
