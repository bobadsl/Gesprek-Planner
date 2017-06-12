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
    [Authorize(Roles = "Eigenaar")]
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
                await _context.ConversationPlanDateClaims.Include(pdc => pdc.ConversationPlanDate)
                .Include(pdc => pdc.Group)
                .OrderBy(pdc => pdc.ConversationPlanDate.StartDate)
                .ThenBy(pdc => pdc.ConversationPlanDate.EndDate)
                .GroupBy(pdc => pdc.ConversationPlanDate)
                .ToListAsync();
            
            if (plannedDates != null)
            {
                var datesList = new List<List<ConversationPlanDateViewModel>>();
                foreach (var plannedDate in plannedDates)
                {
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
                Groups = new SelectList(_context.ApplicationUserGroups.OrderBy(g => g.GroupName).ToList(), "Id", "GroupName"),
                StartDate = DateTime.Now.ToString("dd-mm-yyyy"),
                EndDate = DateTime.Now.AddDays(1).ToString("dd-mm-yyyy")
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
                    StartDate = DateTime.ParseExact(model.StartDate,"dd-mm-yyyy",null),
                    EndDate = DateTime.ParseExact(model.EndDate,"dd-mm-yyyy",null)
                };
                _context.ConversationPlanDates.Add(conversationPlanDate);
                foreach (var group in model.SelectedGroups)
                {
                    var conversationPlanDateClaim = new ConversationPlanDateClaim
                    {
                        Group = _context.ApplicationUserGroups.First(g => g.Id == group),
                        ConversationPlanDate = conversationPlanDate
                    };
                    _context.ConversationPlanDateClaims.Add(conversationPlanDateClaim);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            model.Groups = new SelectList(_context.ApplicationUserGroups.ToList(), "Id", "GroupName");
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

            var editConversationPlanDate = new EditConversationPlanDateViewModel
            {
                StartDate = conversationPlanDate.StartDate.ToString("dd-mm-yyyy"),
                EndDate = conversationPlanDate.EndDate.ToString("dd-mm-yyyy"),
                Id = id.Value
            };
            var groups = _context.ApplicationUserGroups.ToList();
            var groupSelectList = new List<SelectListItem>();
            foreach (var group in groups)
            {
                SelectListItem selectListItem = new SelectListItem
                {
                    Text = group.GroupName,
                    Value= group.Id.ToString()
                };
                if (
                    conversationPlanDateClaimGroups.Any(
                        pdcg => pdcg.Id == group.Id))
                {
                    selectListItem.Selected = true;
                }
                groupSelectList.Add(selectListItem);

            }
            editConversationPlanDate.Groups = groupSelectList;
            return View(editConversationPlanDate);
        }

        // POST: Admin/ConversationPlanDates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditConversationPlanDateViewModel editConversationPlanDate)
        {
//            if (!_context.ConversationPlanDates.Any())
//            {
//                return RedirectToAction("Create");
//            }
//            var conversationPlanDate = await _context.ConversationPlanDates.FirstAsync(cpd => cpd.Id == id);
//            if (conversationPlanDate == null)
//            {
//                return NotFound();
//            }
//
//            if (ModelState.IsValid)
//            {
//                var conversationPlanDateClaims =
//                    await _context.ConversationPlanDateClaims
//                        .Where(pdc => pdc.ConversationPlanDate == conversationPlanDate)
//                        .Include(pdc => pdc.Group)
//                        .Include(pdc => pdc.ConversationPlanDate)
//                        .ToListAsync();
//                var modelGroups = new List<ApplicationUserGroup>();
//                foreach (var group in editConversationPlanDate.SelectedGroups)
//                {
//                    modelGroups.Add(_context.ApplicationUserGroups.First(g => g.Id == group));
//                }
//                var removeGroups = conversationPlanDateClaims.Select(pdc => pdc.Group).ToList().Except(modelGroups).ToList();
//                foreach (var group in removeGroups)
//                {
//                    if (!_context.Conversations.Include(c => c.Group).Any(c => c.Group == group))
//                    {
//                        var conversationTypeClaim =
//                            _context.ConversationTypeClaims.SingleOrDefault(ctc => ctc.ConversationType == conversationType && ctc.Group == group);
//                        if (conversationTypeClaim != null) // It shouldn't be null but just incase
//                            _context.ConversationTypeClaims.Remove(conversationTypeClaim);
//                    }
//                }
//                var AddGroups = modelGroups.Except(conversationTypeClaimsGroups).ToList();
//                foreach (var group in AddGroups)
//                {
//                    _context.ConversationTypeClaims.Add(new ConversationTypeClaim
//                    {
//                        ConversationType = conversationType,
//                        Group = group
//                    });
//                }
//
//                foreach (var selectedGroup in editConversationPlanDate.SelectedGroups)
//                {
//                    if (conversationPlanDateClaims.All(pdc => pdc.Group.Id != selectedGroup))
//                    {
//                        _context.ConversationPlanDateClaims.Add(new ConversationPlanDateClaim
//                        {
//                            ConversationPlanDate = conversationPlanDate,
//                            Group =
//                                _context.ApplicationUserGroups.Single(g => g.Id == selectedGroup)
//                        });
//                    }
//                }
//                foreach (var conversationPlanDateClaim in conversationPlanDateClaims)
//                {
//                    if (
//                        editConversationPlanDate.SelectedGroups.All(
//                            pdc => pdc != conversationPlanDateClaim.Group.Id))
//                    {
//                        _context.ConversationPlanDateClaims.Remove(conversationPlanDateClaim);
//                    }
//                }
//                conversationPlanDate.StartDate = DateTime.ParseExact(editConversationPlanDate.StartDate, "dd-mm-yyyy", null);
//                conversationPlanDate.EndDate = DateTime.ParseExact(editConversationPlanDate.EndDate, "dd-mm-yyyy", null);
//                _context.Update(conversationPlanDate);
//                
//                await _context.SaveChangesAsync();   
//                return RedirectToAction("Index");
//            }
            return View(editConversationPlanDate);
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
