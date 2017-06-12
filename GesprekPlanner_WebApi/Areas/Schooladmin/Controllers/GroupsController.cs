using System;
using System.Linq;
using System.Threading.Tasks;
using GesprekPlanner_WebApi.Areas.Schooladmin.Models.GroupsViewModels;
using GesprekPlanner_WebApi.Data;
using GesprekPlanner_WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GesprekPlanner_WebApi.Areas.Schooladmin.Controllers
{
    [Area("Schooladmin")]
    [Authorize(Roles = "Eigenaar, Schooladmin")]
    public class GroupsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GroupsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Schooladmin/Groups
        public async Task<IActionResult> Index()
        {
                return
                    View(
                        await _context.ApplicationUserGroups
                                .Include(g=>g.School)
                                .Where(u => u.School.Id == Guid.Parse(HttpContext.Session.GetString("School")))
                                .OrderBy(g => g.GroupName)
                                .ToListAsync());
        }

        // GET: Schooladmin/Groups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUserGroup = await _context.ApplicationUserGroups
                .SingleOrDefaultAsync(m => m.Id == id);
            if (applicationUserGroup == null)
            {
                return NotFound();
            }

            return View(applicationUserGroup);
        }

        // GET: Schooladmin/Groups/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Schooladmin/Groups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,GroupName")] ApplicationUserGroup applicationUserGroup)
        {
            if (ModelState.IsValid)
            {
                applicationUserGroup.School =
                    _context.Schools.First(s => s.Id == Guid.Parse(HttpContext.Session.GetString("School")));
                _context.Add(applicationUserGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(applicationUserGroup);
        }

        // GET: Schooladmin/Groups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUserGroup = await _context.ApplicationUserGroups.SingleOrDefaultAsync(m => m.Id == id);
            if (applicationUserGroup == null)
            {
                return NotFound();
            }
            var model = new CreateEditGroupViewModel
            {
                Id = applicationUserGroup.Id,
                GroupName = applicationUserGroup.GroupName
            };
            return View(model);
        }

        // POST: Schooladmin/Groups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateEditGroupViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var group = new ApplicationUserGroup
                {
                    GroupName = model.GroupName,
                    Id = model.Id
                };
                try
                {
                    _context.Update(group);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationUserGroupExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ViewData["Error"] = "Er is een onbekende fout opgetreden.";
                        return View("Error");
                    }
                }
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Schooladmin/Groups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUserGroup = await _context.ApplicationUserGroups
                .SingleOrDefaultAsync(m => m.Id == id);
            if (_context.Users.Include(u => u.Group).Any(u => u.Group.Id == applicationUserGroup.Id))
            {
                return View("Error");
            }
            if (applicationUserGroup == null)
            {
                return NotFound();
            }

            return View(applicationUserGroup);
        }

        // POST: Schooladmin/Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var applicationUserGroup = _context.ApplicationUserGroups.SingleOrDefault(m => m.Id == id);
            if (_context.Users.Any(u => u.Group == applicationUserGroup))
            {
                ViewData["Error"] = "Kan deze groep niet verwijderen. Er bestaat al een gebruiker met deze groep";
                return View("Error");
            }
            _context.ApplicationUserGroups.Remove(applicationUserGroup);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ApplicationUserGroupExists(int id)
        {
            return _context.ApplicationUserGroups.Any(e => e.Id == id);
        }
    }
}
