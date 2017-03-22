using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GesprekPlanner_WebApi.Areas.Teacher.Models.ConversationViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GesprekPlanner_WebApi.Data;
using GesprekPlanner_WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace GesprekPlanner_WebApi.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    [Authorize]
    public class ConversationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ConversationsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private async Task<ApplicationUser> GetCurrentUser()
        {
            return await _userManager.GetUserAsync(HttpContext.User);
        }

        // GET: Teacher/Conversations
        public async Task<IActionResult> Index()
        {
            return View(await _context.Conversations.ToListAsync());
        }
        
        // GET: Teacher/Conversations/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            if (!_context.ConversationPlanDates.Any())
            {
                ViewData["error"] =
                    "De schooladministrator heeft nog geen gespreksplanning gemaakt.";
                return View("Error");
            }
            if (!_context.ConversationTypes.Any())
            {
                ViewData["error"] = "De schooladministrator heeft nog geen gesprekstypes aangemaakt.";
            }
            var model = new CreateConversationViewModel();
            model.PlannableDates = new List<SelectListItem>();
            model.ConversationTypes = new List<SelectListItem>();
            var conversationTypes = _context.ConversationTypes.ToList();
            foreach (var conversationType in conversationTypes)
            {
                model.ConversationTypes.Add(new SelectListItem
                {
                    Value = conversationType.Id.ToString(),
                    Text = $"{conversationType.ConversationName} ({conversationType.ConversationDuration} minuten)"

                });
            }
            model.ConversationTypes[0].Selected = true;

            var tempUser = await GetCurrentUser();
            var user = _context.Users.Include(u => u.Group).First(u => u.Id == tempUser.Id);
            var planDates = await _context.ConversationPlanDates.Where(pd => pd.Group == user.Group).ToListAsync();
            
            var counter = 1;
            foreach (var planDate in planDates)
            {
                var selectListGroup = new SelectListGroup{Name = $"Periode {counter}"};
                var datesToLoop = (int)planDate.EndDate.Subtract(planDate.StartDate).TotalDays;
                for (int i = 0; i <= datesToLoop; i++)
                {
                    var date = planDate.StartDate.AddDays(i);
                    // Teachers don't work in the weekend
                    if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) continue;
                    var selectListItem = new SelectListItem();

                    selectListItem.Value = date.ToString("d");
                    selectListItem.Text = date.ToString("dddd dd-MM-yyyy");
                    selectListItem.Group = selectListGroup;
                    
                    model.PlannableDates.Add(selectListItem);
                }
                counter++;
            }
            return View(model);
        }

        // POST: /Teacher/Conversations/Create
        [HttpPost]
        public async Task<IActionResult> Create(CreateConversationViewModel model)
        {
            if (ModelState.IsValid)
            {
                return View("CreateSchedule",model);
            }
            
            model.PlannableDates = new List<SelectListItem>();
            model.ConversationTypes = new List<SelectListItem>();
            var conversationTypes = _context.ConversationTypes.ToList();
            foreach (var conversationType in conversationTypes)
            {
                model.ConversationTypes.Add(new SelectListItem
                {
                    Value = conversationType.Id.ToString(),
                    Text = $"{conversationType.ConversationName} ({conversationType.ConversationDuration} minuten)"
                });
            }
            model.ConversationTypes[0].Selected = true;

            var tempUser = await GetCurrentUser();
            var user = _context.Users.Include(u => u.Group).First(u => u.Id == tempUser.Id);
            var planDates = await _context.ConversationPlanDates.Where(pd => pd.Group == user.Group).ToListAsync();

            var counter = 1;
            foreach (var planDate in planDates)
            {
                var selectListGroup = new SelectListGroup { Name = $"Periode {counter}" };
                var datesToLoop = (int)planDate.EndDate.Subtract(planDate.StartDate).TotalDays;
                for (int i = 0; i <= datesToLoop; i++)
                {
                    var date = planDate.StartDate.AddDays(i);
                    // Teachers don't work in the weekend
                    if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) continue;
                    var selectListItem = new SelectListItem();

                    selectListItem.Value = date.ToString("d");
                    selectListItem.Text = date.ToString("dddd dd-MM-yyyy");
                    selectListItem.Group = selectListGroup;

                    model.PlannableDates.Add(selectListItem);
                }
                counter++;
            }
            return View(model);
        }
    }
}
