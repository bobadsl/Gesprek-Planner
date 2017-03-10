using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GesprekPlanner_WebApi.Data;
using GesprekPlanner_WebApi.Models;

namespace GesprekPlanner_WebApi.Areas.Admin.Controllers
{
    [Area("Admin")]
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
            return View(await _context.ApplicationUserGroups.ToListAsync());
        }

        // GET: Admin/Groups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUserGroup = await _context.ApplicationUserGroups
                .SingleOrDefaultAsync(m => m.ApplicationUserGroupId == id);
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
        public async Task<IActionResult> Create([Bind("ApplicationUserGroupId,GroupName")] ApplicationUserGroup applicationUserGroup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(applicationUserGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(applicationUserGroup);
        }

        // GET: Admin/Groups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUserGroup = await _context.ApplicationUserGroups.SingleOrDefaultAsync(m => m.ApplicationUserGroupId == id);
            if (applicationUserGroup == null)
            {
                return NotFound();
            }
            return View(applicationUserGroup);
        }

        // POST: Admin/Groups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ApplicationUserGroupId,GroupName")] ApplicationUserGroup applicationUserGroup)
        {
            if (id != applicationUserGroup.ApplicationUserGroupId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(applicationUserGroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationUserGroupExists(applicationUserGroup.ApplicationUserGroupId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ViewBag["Error"] = "Er is een onbekende fout opgetreden.";
                        return View("Error");
                    }
                }
                return RedirectToAction("Index");
            }
            return View(applicationUserGroup);
        }

        // GET: Admin/Groups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUserGroup = await _context.ApplicationUserGroups
                .SingleOrDefaultAsync(m => m.ApplicationUserGroupId == id);
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
            var applicationUserGroup = await _context.ApplicationUserGroups.SingleOrDefaultAsync(m => m.ApplicationUserGroupId == id);
            if (_context.Users.Any(u => u.GroupId == id))
            {
                ViewBag["Error"] = "Kan deze groep niet verwijderen. Er bestaat al een gebruiker met deze groep";
                return View("Error");
            }
            _context.ApplicationUserGroups.Remove(applicationUserGroup);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ApplicationUserGroupExists(int id)
        {
            return _context.ApplicationUserGroups.Any(e => e.ApplicationUserGroupId == id);
        }
    }
}
