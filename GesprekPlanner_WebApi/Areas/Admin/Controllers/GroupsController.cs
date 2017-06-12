using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GesprekPlanner_WebApi.Areas.Admin.Models.GroupsViewModels;
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
    public class GroupsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GroupsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Groups
        public async Task<IActionResult> Index()
        {
            return
                View(
                    await _context.ApplicationUserGroups
                        .Include(g => g.School)
                        .OrderBy(g => g.School.Name)
                        .ThenBy(g => g.GroupName)
                        .ToListAsync());
        }

        // GET: Admin/Groups/Details/5
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

        // GET: Admin/Groups/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Groups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEditGroupViewModel model)
        {
            if (ModelState.IsValid)
            {
                var group = new ApplicationUserGroup
                {
                    GroupName=model.GroupName,
                    School = _context.Schools.Single(s => s.Id == Guid.Parse(model.SelectedSchool)),
                    EmailGroup = model.EmailGroup
                }; 
                _context.Add(group);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Admin/Groups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var group = await _context.ApplicationUserGroups.SingleOrDefaultAsync(m => m.Id == id);
            if (group == null)
            {
                return NotFound();
            }
            var model = new CreateEditGroupViewModel
            {
                Id = group.Id,
                GroupName = group.GroupName,
                Schools = new SelectList(_context.Schools.ToList(), "Id", "Name"),
                EmailGroup = group.EmailGroup
            };
            return View(model);
        }

        // POST: Admin/Groups/Edit/5
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
                    Id = model.Id,
                    School = _context.Schools.First(s => s.Id == Guid.Parse(model.SelectedSchool)),
                    EmailGroup = model.EmailGroup
                };

                try
                {
                    _context.Update(group);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.ApplicationUserGroups.Any(g => g.Id == model.Id))
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
            model.Schools = new SelectList(_context.Schools.ToList(), "Id", "Name");

            return View(model);
        }

        // GET: Admin/Groups/Delete/5
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

        // POST: Admin/Groups/Delete/5
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
    }
}
