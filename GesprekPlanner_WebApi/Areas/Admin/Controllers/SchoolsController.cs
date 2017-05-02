using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class SchoolsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SchoolsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Admin/Schools
        public async Task<IActionResult> Index()
        {
            return View(await _context.Schools.ToListAsync());
        }

        // GET: Admin/Schools/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var school = await _context.Schools
                .SingleOrDefaultAsync(m => m.Id == id);
            if (school == null)
            {
                return NotFound();
            }

            return View(school);
        }

        // GET: Admin/Schools/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Schools/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SchoolName,SchoolUrl,SchoolTelephone,SchoolEmail,SchoolStreet,SchoolPostCode,SchoolLogo")] School school)
        {
            if (ModelState.IsValid)
            {
                school.Id = Guid.NewGuid();
                _context.Add(school);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(school);
        }

        // GET: Admin/Schools/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var school = await _context.Schools.SingleOrDefaultAsync(m => m.Id == id);
            if (school == null)
            {
                return NotFound();
            }
            return View(school);
        }

        // POST: Admin/Schools/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,SchoolName,SchoolUrl,SchoolTelephone,SchoolEmail,SchoolStreet,SchoolPostCode,SchoolLogo")] School school)
        {
            if (id != school.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(school);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SchoolExists(school.Id))
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
            return View(school);
        }

        // GET: Admin/Schools/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var school = await _context.Schools
                .SingleOrDefaultAsync(m => m.Id == id);
            if (school == null)
            {
                return NotFound();
            }

            return View(school);
        }

        // POST: Admin/Schools/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var school = await _context.Schools.SingleOrDefaultAsync(m => m.Id == id);
            _context.Schools.Remove(school);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool SchoolExists(Guid id)
        {
            return _context.Schools.Any(e => e.Id == id);
        }
    }
}
