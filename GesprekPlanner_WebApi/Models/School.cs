using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GesprekPlanner_WebApi.Models
{
    public class School
    {
        public Guid Id { get; set; }
        public string SchoolName { get; set; }
        public string SchoolUrl { get; set; }
        public string SchoolTelephone { get; set; }
        public string SchoolEmail { get; set; }
        public string SchoolStreet { get; set; }
        public string SchoolPostCode { get; set; }
        public string SchoolLogo { get; set; }
    }
}
