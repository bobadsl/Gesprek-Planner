using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GesprekPlanner_WebApi.Areas.Admin.Models.ConversationPlanDateViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GesprekPlanner_WebApi.Data;
using GesprekPlanner_WebApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace GesprekPlanner_WebApi.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Eigenaar, Schooladmin")]
    public class ConversationPlanDatesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ConversationPlanDatesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/ConversationPlanDates
        public async Task<IActionResult> Index()
        {
            var plannedDates =
                await _context.ConversationPlanDateClaims.Include(pdc => pdc.ConversationPlanDate).Include(pdc => pdc.Group).OrderBy(pdc => pdc.ConversationPlanDate.StartDate).ThenBy(pdc => pdc.ConversationPlanDate.EndDate).GroupBy(pdc => pdc.ConversationPlanDate).ToListAsync();
            
            if (plannedDates != null)
            {
                var datesList = new List<List<ConversationPlanDateViewModel>>();
                foreach (var plannedDate in plannedDates)
                {
                    var temp = plannedDate.ToList();
                    var tempList = plannedDate.ToList().Select(pdc => new ConversationPlanDateViewModel
                    {
                        Id = pdc.ConversationPlanDate.Id,
                        StartDate = pdc.ConversationPlanDate.StartDate.ToString("dd-MM-yyyy"),
                        EndDate = pdc.ConversationPlanDate.EndDate.ToString("dd-MM-yyyy"),
                        Group = pdc.Group.GroupName
                    }).ToList();
                    datesList.Add(tempList);
                }
                return View(datesList);
            }
            return View();
        }

        // GET: Admin/ConversationPlanDates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conversationPlanDate = await _context.ConversationPlanDates
                .SingleOrDefaultAsync(m => m.Id == id);
            if (conversationPlanDate == null)
            {
                return NotFound();
            }

            return View(conversationPlanDate);
        }

        // GET: Admin/ConversationPlanDates/Create
        public IActionResult Create()
        {
            CreateConversationPlanDateViewModel model = new CreateConversationPlanDateViewModel()
            {
                Groups = new SelectList(_context.ApplicationUserGroups.OrderBy(g => g.GroupName).ToList(), "ApplicationUserGroupId", "GroupName"),
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1)
            };
            return View(model);
        }

        // POST: Admin/ConversationPlanDates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateConversationPlanDateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var conversationPlanDate = new ConversationPlanDate
                {
                    StartDate = model.StartDate,
                    EndDate = model.EndDate
                };
                _context.ConversationPlanDates.Add(conversationPlanDate);
                foreach (var group in model.SelectedGroups)
                {
                    var conversationPlanDateClaim = new ConversationPlanDateClaim
                    {
                        Group = _context.ApplicationUserGroups.First(g => g.ApplicationUserGroupId == group),
                        ConversationPlanDate = conversationPlanDate
                    };
                    _context.ConversationPlanDateClaims.Add(conversationPlanDateClaim);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            model.Groups = new SelectList(_context.ApplicationUserGroups.ToList(), "ApplicationUserGroupId", "GroupName");
            return View(model);
        }

        // GET: Admin/ConversationPlanDates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conversationPlanDate = await _context.ConversationPlanDates.SingleOrDefaultAsync(m => m.Id == id);
            if (conversationPlanDate == null)
            {
                return NotFound();
            }
            var conversationPlanDateClaimGroups =
                await _context.ConversationPlanDateClaims.Where(pdc => pdc.ConversationPlanDate == conversationPlanDate).Select(pdc => pdc.Group)
                    .ToListAsync();

            var createConversationPlanDate = new CreateConversationPlanDateViewModel
            {
                StartDate = conversationPlanDate.StartDate,
                EndDate = conversationPlanDate.EndDate,
            };
            var groups = _context.ApplicationUserGroups.ToList();
            var groupSelectList = new List<SelectListItem>();
            foreach (var group in groups)
            {
                SelectListItem selectListItem = new SelectListItem
                {
                    Text = group.GroupName,
                    Value= group.ApplicationUserGroupId.ToString()
                };
                if (
                    conversationPlanDateClaimGroups.Any(
                        pdcg => pdcg.ApplicationUserGroupId == group.ApplicationUserGroupId))
                {
                    selectListItem.Selected = true;
                }
                groupSelectList.Add(selectListItem);

            }
            createConversationPlanDate.Groups = groupSelectList;
            return View(createConversationPlanDate);
        }

        // POST: Admin/ConversationPlanDates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateConversationPlanDateViewModel createConversationPlanDate)
        {
            if (!_context.ConversationPlanDates.Any())
            {
                return RedirectToAction("Create");
            }
            var conversationPlanDate = await _context.ConversationPlanDates.FirstAsync(cpd => cpd.Id == id);
            if (conversationPlanDate == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var conversationPlanDateClaims =
                    await _context.ConversationPlanDateClaims
                        .Where(pdc => pdc.ConversationPlanDate == conversationPlanDate)
                        .Include(pdc => pdc.Group)
                        .Include(pdc => pdc.ConversationPlanDate)
                        .ToListAsync();
                foreach (var selectedGroup in createConversationPlanDate.SelectedGroups)
                {
                    if (conversationPlanDateClaims.All(pdc => pdc.Group.ApplicationUserGroupId != selectedGroup))
                    {
                        _context.ConversationPlanDateClaims.Add(new ConversationPlanDateClaim
                        {
                            ConversationPlanDate = conversationPlanDate,
                            Group =
                                _context.ApplicationUserGroups.Single(g => g.ApplicationUserGroupId == selectedGroup)
                        });
                    }
                }
                foreach (var conversationPlanDateClaim in conversationPlanDateClaims)
                {
                    if (
                        createConversationPlanDate.SelectedGroups.All(
                            pdc => pdc != conversationPlanDateClaim.Group.ApplicationUserGroupId))
                    {
                        _context.ConversationPlanDateClaims.Remove(conversationPlanDateClaim);
                    }
                }
                conversationPlanDate.StartDate = DateTime.ParseExact(createConversationPlanDate.Start_Date, "dd-mm-yyyy", null);
                conversationPlanDate.EndDate = DateTime.ParseExact(createConversationPlanDate.End_Date, "dd-mm-yyyy", null);
                _context.Update(conversationPlanDate);
                
                await _context.SaveChangesAsync();   
                return RedirectToAction("Index");
            }
            return View(createConversationPlanDate);
        }

        // GET: Admin/ConversationPlanDates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conversationPlanDate = await _context.ConversationPlanDates
                .SingleOrDefaultAsync(m => m.Id == id);
            if (conversationPlanDate == null)
            {
                return NotFound();
            }

            return View(conversationPlanDate);
        }

        // POST: Admin/ConversationPlanDates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var conversationPlanDate = await _context.ConversationPlanDates.SingleOrDefaultAsync(m => m.Id == id);
            _context.ConversationPlanDates.Remove(conversationPlanDate);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
