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
using Microsoft.AspNetCore.Http;

namespace GesprekPlanner_WebApi.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Eigenaar")]
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
            var conversationTypeClaimList = _context.ConversationTypeClaims.Include(ctc => ctc.Group)
                .Include(ctc => ctc.ConversationType)
                .ThenInclude(ctc => ctc.School)
                .ToList()
                .GroupBy(ctc => ctc.ConversationType);
            if (conversationTypeClaimList != null)
            {
                var claimList = new List<List<ListConversationTypeViewModel>>();
                foreach (var conversationTypeClaim in conversationTypeClaimList)
                {
                    var tempList = conversationTypeClaim.ToList().Select(ctc => new ListConversationTypeViewModel()
                    {
                        Id = ctc.ConversationType.Id,
                        ConversationName = ctc.ConversationType.ConversationName,
                        Duration = ctc.ConversationType.ConversationDuration,
                        Group = ctc.Group.GroupName,
                        SchoolName = ctc.ConversationType.School.Name
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
                return RedirectToAction("Index");
            }

            var conversationType = await _context.ConversationTypes.Include(ct => ct.School)
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
            
            conversationTypeModel.Groups = new List<SelectListItem>();
            var groups = _context.ApplicationUserGroups.Include(g => g.School).OrderBy(g => g.GroupName).GroupBy(g => g.School).ToList();
            foreach (var groupList in groups)
            {
                var selectGroup = new SelectListGroup {Name = groupList.Key.Name, Disabled = false};
                foreach (var group in groupList)
                {
                    ((List<SelectListItem>)conversationTypeModel.Groups).Add(new SelectListItem{Disabled = false, Group = selectGroup, Value = group.Id.ToString(), Text = group.GroupName});
                }
                //((List<SelectListItem>)conversationTypeModel.Groups).Add(new SelectListItem { Text = group.GroupName, Value = group.Id.ToString(), Selected = true });
            }
            conversationTypeModel.Schools = new SelectList(_context.Schools.ToList(), "Id", "Name");

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
                if (User.IsInRole("Eigenaar") && HttpContext.Session.GetString("School") == null)
                {
                    conversationType.School =
                        _context.Schools.First(s => s.Id == Guid.Parse(createConversationType.SelectedSchool));
                }
                else
                {
                    conversationType.School =
                        _context.Schools.First(s => s.Id == Guid.Parse(HttpContext.Session.GetString("School")));
                }
                _context.Add(conversationType);
                foreach (var group in createConversationType.SelectedGroups)
                {
                    ConversationTypeClaim conversationTypeClaim = new ConversationTypeClaim
                    {
                        ConversationType = conversationType,
                        Group = _context.ApplicationUserGroups.First(g => g.Id == group)
                    };
                    _context.Add(conversationTypeClaim);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            createConversationType.Groups = new List<SelectListItem>();
            var groups = _context.ApplicationUserGroups.Include(g => g.School).OrderBy(g => g.GroupName).GroupBy(g => g.School).ToList();
            foreach (var groupList in groups)
            {
                var selectGroup = new SelectListGroup { Name = groupList.Key.Name, Disabled = false };
                foreach (var group in groupList)
                {
                    ((List<SelectListItem>)createConversationType.Groups).Add(new SelectListItem { Disabled = false, Group = selectGroup, Value = group.Id.ToString(), Text = group.GroupName });
                }
                //((List<SelectListItem>)conversationTypeModel.Groups).Add(new SelectListItem { Text = group.GroupName, Value = group.Id.ToString(), Selected = true });
            }
            createConversationType.Schools = new SelectList(_context.Schools.ToList(), "Id", "Name");

            return View(createConversationType);
        }

        // GET: Admin/ConversationType/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conversationType = await _context.ConversationTypes.Include(ct => ct.School).SingleOrDefaultAsync(m => m.Id == id);
            if (conversationType == null)
            {
                return NotFound();
            }
            var selectedGroups = await
                _context.ConversationTypeClaims.Include(ctc => ctc.Group)
                    .Include(ctc => ctc.ConversationType)
                    .Where(ctc => ctc.ConversationType.Id == id)
                    .Select(ctc => ctc.Group.Id).ToListAsync();
            var editConversationType = new EditConversationTypeViewModel
            {
                Id = id.Value,
                Name = conversationType.ConversationName,
                Duration = conversationType.ConversationDuration,
                SelectedGroups = selectedGroups,
                School = conversationType.School.Id
            };
            editConversationType.Groups = new List<SelectListItem>();
            var groups = _context.ApplicationUserGroups.Include(g => g.School).OrderBy(g => g.GroupName).GroupBy(g => g.School).ToList();
            foreach (var groupList in groups)
            {
                var selectGroup = new SelectListGroup { Name = groupList.Key.Name, Disabled = false };
                foreach (var group in groupList)
                {
                    ((List<SelectListItem>)editConversationType.Groups).Add(new SelectListItem { Disabled = false, Group = selectGroup, Value = group.Id.ToString(), Text = group.GroupName });
                }
                //((List<SelectListItem>)conversationTypeModel.Groups).Add(new SelectListItem { Text = group.GroupName, Value = group.Id.ToString(), Selected = true });
            }
            editConversationType.Schools = new SelectList(_context.Schools.ToList(), "Id", "Name");

            return View(editConversationType);
        }

        // POST: Admin/ConversationType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditConversationTypeViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var origionalConversationType = _context.ConversationTypes.First(ct => ct.Id == id);
                var conversationType = new ConversationType
                {
                    Id = model.Id,
                    ConversationName = model.Name
                };
                if (
                    !_context.Conversations.Include(c => c.ConversationType)
                        .Any(cpd => cpd.ConversationType.Id == id))
                {
                    conversationType.ConversationDuration = model.Duration;
                    conversationType.School = _context.Schools.First(s => s.Id == model.School);
                }
                var conversationTypeClaimsGroups =
                    _context.ConversationTypeClaims.Include(ctc => ctc.Group)
                        .Include(ctc => ctc.ConversationType)
                        .Where(ctc => ctc.ConversationType.Id == id)
                        .Select(ctc => ctc.Group)
                        .ToList();

                List<ApplicationUserGroup> modelGroups = new List<ApplicationUserGroup>();
                foreach (var selectedGroup in model.SelectedGroups)
                {
                    modelGroups.Add(_context.ApplicationUserGroups.First(g => g.Id == selectedGroup));
                }

                var removeGroups = conversationTypeClaimsGroups.Except(modelGroups).ToList();
                foreach (var group in removeGroups)
                {
                    if (!_context.Conversations.Include(c => c.ConversationType).Include(c => c.Group).Any(c => c.Group == group && c.ConversationType == origionalConversationType))
                    {
                        var conversationTypeClaim =
                            _context.ConversationTypeClaims.SingleOrDefault(ctc => ctc.ConversationType == origionalConversationType && ctc.Group == group);
                        if (conversationTypeClaim != null) // It shouldn't be null but just incase
                            _context.ConversationTypeClaims.Remove(conversationTypeClaim);
                    }
                }
                var AddGroups = modelGroups.Except(conversationTypeClaimsGroups).ToList();
                foreach (var group in AddGroups)
                {
                    _context.ConversationTypeClaims.Add(new ConversationTypeClaim
                    {
                        ConversationType = conversationType,
                        Group = group
                    });
                }
                _context.Update(conversationType);
                await _context.SaveChangesAsync();
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
            return View(model);
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
    }
}
