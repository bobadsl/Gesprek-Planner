using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GesprekPlanner_WebApi.Models
{
    public class School
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public string PostCode { get; set; }
        public string Logo { get; set; }
    }
}
