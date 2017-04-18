using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GesprekPlanner_WebApi.Areas.Admin.Models.ConversationPlanDateViewModels;
using GesprekPlanner_WebApi.Areas.Admin.Models.ConversationTypeViewModels;
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
    public class ConversationTypeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ConversationTypeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/ConversationType
        public async Task<IActionResult> Index()
        {
            var conversationTypeClaimList =
                _context.ConversationTypeClaims.Include(ctc => ctc.Group)
                    .Include(ctc => ctc.ConversationType)
                    .ToList()
                    .GroupBy(ctc => ctc.ConversationType);

            if (conversationTypeClaimList != null)
            {
                var claimList = new List<List<ListConversationTypeViewModel>>();
                foreach (var conversationTypeClaim in conversationTypeClaimList)
                {
                    var temp = conversationTypeClaim.ToList();
                    var tempList = conversationTypeClaim.ToList().Select(ctc => new ListConversationTypeViewModel()
                    {
                        Id = ctc.ConversationType.Id,
                        ConversationName = ctc.ConversationType.ConversationName,
                        Duration = ctc.ConversationType.ConversationDuration,
                        Group = ctc.Group.GroupName
                    }).ToList();
                    claimList.Add(tempList);
                }
                return View(claimList);
            }
            return View();
        }

        // GET: Admin/ConversationType/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conversationType = await _context.ConversationTypes
                .SingleOrDefaultAsync(m => m.Id == id);
            if (conversationType == null)
            {
                return NotFound();
            }

            return View(conversationType);
        }

        // GET: Admin/ConversationType/Create
        public IActionResult Create()
        {
            var conversationTypeModel = new CreateConversationTypeViewModel();
            var groups = _context.ApplicationUserGroups.OrderBy(g => g.GroupName).ToList();
            conversationTypeModel.Groups = new List<SelectListItem>();
            foreach (var group in groups)
            {
                ((List<SelectListItem>)conversationTypeModel.Groups).Add(new SelectListItem{Text = group.GroupName, Value=group.ApplicationUserGroupId.ToString(), Selected = true});
            }

            return View(conversationTypeModel);
        }

        // POST: Admin/ConversationType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateConversationTypeViewModel createConversationType)
        {
            if (ModelState.IsValid)
            {
                var conversationType = new ConversationType
                {
                    ConversationName = createConversationType.Name,
                    ConversationDuration = createConversationType.Duration
                };
                _context.Add(conversationType);
                foreach (var group in createConversationType.SelectedGroups)
                {
                    ConversationTypeClaim conversationTypeClaim = new ConversationTypeClaim
                    {
                        ConversationType = conversationType,
                        Group = _context.ApplicationUserGroups.First(g => g.ApplicationUserGroupId == group)
                    };
                    _context.Add(conversationTypeClaim);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            createConversationType.Groups = new SelectList(_context.ApplicationUserGroups.OrderBy(g => g.GroupName).ToList(),
                "ApplicationUserGroupId", "GroupName");
            return View(createConversationType);
        }

        // GET: Admin/ConversationType/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conversationType = await _context.ConversationTypes.SingleOrDefaultAsync(m => m.Id == id);
            if (conversationType == null)
            {
                return NotFound();
            }
            var groups = await
                _context.ConversationTypeClaims.Include(ctc => ctc.Group)
                    .Include(ctc => ctc.ConversationType)
                    .Where(ctc => ctc.ConversationType.Id == id)
                    .Select(ctc => ctc.Group.ApplicationUserGroupId).ToListAsync();
            var editConversationType = new EditConversationTypeViewModel
            {
                Id = id.Value,
                Name = conversationType.ConversationName,
                Duration = conversationType.ConversationDuration,
                SelectedGroups = groups,
                Groups = new SelectList(_context.ApplicationUserGroups.OrderBy(g => g.GroupName).ToList(),
                "ApplicationUserGroupId", "GroupName")
            };
            return View(editConversationType);
        }

        // POST: Admin/ConversationType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditConversationTypeViewModel editConversationType)
        {
            if (id != editConversationType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
//                try
//                {
//                    _context.Update(conversationType);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!ConversationTypeExists(conversationType.Id))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//                return RedirectToAction("Index");
            }
            return View(editConversationType);
        }

        // GET: Admin/ConversationType/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conversationType = await _context.ConversationTypes
                .SingleOrDefaultAsync(m => m.Id == id);
            if (conversationType == null)
            {
                return NotFound();
            }

            return View(conversationType);
        }

        // POST: Admin/ConversationType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var conversationType = await _context.ConversationTypes.SingleOrDefaultAsync(m => m.Id == id);
            _context.ConversationTypes.Remove(conversationType);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ConversationTypeExists(int id)
        {
            return _context.ConversationTypes.Any(e => e.Id == id);
        }
    }
}
