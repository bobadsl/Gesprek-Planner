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
            return View(await _context.ConversationTypes.ToListAsync());
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
            return View();
        }

        // POST: Admin/ConversationType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ConversationName,ConversationDuration")] ConversationType conversationType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(conversationType);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(conversationType);
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
            return View(conversationType);
        }

        // POST: Admin/ConversationType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ConversationName,ConversationDuration")] ConversationType conversationType)
        {
            if (id != conversationType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(conversationType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConversationTypeExists(conversationType.Id))
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
            return View(conversationType);
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
