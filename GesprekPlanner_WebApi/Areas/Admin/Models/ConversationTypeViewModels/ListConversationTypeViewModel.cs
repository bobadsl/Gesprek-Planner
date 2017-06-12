using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GesprekPlanner_WebApi.Models;

namespace GesprekPlanner_WebApi.Areas.Admin.Models.ConversationTypeViewModels
{
    public class ListConversationTypeViewModel
    {
        public int Id { get; set; }
        [Display(Name="Gespreksnaam")]
        public string ConversationName { get; set; }
        [Display(Name = "Gespreksduur (in minuten)")]
        public int Duration { get; set; }
        [Display(Name="Groep")]
        public string Group { get; set; }
        [Display(Name="School")]
        public string SchoolName { get; set; }
    }
}
