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
                await _context.ConversationPlanDates.Include(p => p.Group).OrderBy(p => p.StartDate).ThenBy(p => p.EndDate).ThenBy(p => p.Group.GroupName).ToListAsync();
           
            if (plannedDates != null)
            {
                var plannedDateList = plannedDates.Select(plannedDate => new ConversationPlanDateViewModel
                {
                    Id = plannedDate.Id,
                    StartDate = plannedDate.StartDate.ToString("dd-MM-yyyy"),
                    EndDate = plannedDate.EndDate.ToString("dd-MM-yyyy"),
                    Group = plannedDate.Group.GroupName
                }).ToList();

                return View(plannedDateList);
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
                foreach (var group in model.SelectedGroups)
                {
                    var conversationPlanDate = new ConversationPlanDate
                    {
                        StartDate = model.StartDate,
                        EndDate = model.EndDate,
                        Group = _context.ApplicationUserGroups.First(g => g.ApplicationUserGroupId == group)
                    };
                    _context.ConversationPlanDates.Add(conversationPlanDate);
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
            return View(conversationPlanDate);
        }

        // POST: Admin/ConversationPlanDates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StartDate,EndDate")] ConversationPlanDate conversationPlanDate)
        {
            if (id != conversationPlanDate.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (conversationPlanDate.StartDate <= conversationPlanDate.EndDate)
                {
                    return View(conversationPlanDate);
                }
                try
                {
                    _context.Update(conversationPlanDate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConversationPlanDateExists(conversationPlanDate.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(conversationPlanDate);
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

        private bool ConversationPlanDateExists(int id)
        {
            return _context.ConversationPlanDates.Any(e => e.Id == id);
        }
    }
}
