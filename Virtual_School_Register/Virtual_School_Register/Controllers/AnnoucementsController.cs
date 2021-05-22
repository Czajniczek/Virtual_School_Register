using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Virtual_School_Register.Data;
using Virtual_School_Register.Models;

namespace Virtual_School_Register.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AnnoucementsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnnoucementsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Annoucements
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Annoucement.Include(a => a.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Annoucements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var annoucement = await _context.Annoucement
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.AnnoucementId == id);
            if (annoucement == null)
            {
                return NotFound();
            }

            return View(annoucement);
        }

        // GET: Annoucements/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Annoucements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AnnoucementId,Title,Content,Date,UserId")] Annoucement annoucement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(annoucement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", annoucement.UserId);
            return View(annoucement);
        }

        // GET: Annoucements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var annoucement = await _context.Annoucement.FindAsync(id);
            if (annoucement == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", annoucement.UserId);
            return View(annoucement);
        }

        // POST: Annoucements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AnnoucementId,Title,Content,Date,UserId")] Annoucement annoucement)
        {
            if (id != annoucement.AnnoucementId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(annoucement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnnoucementExists(annoucement.AnnoucementId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", annoucement.UserId);
            return View(annoucement);
        }

        // GET: Annoucements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var annoucement = await _context.Annoucement
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.AnnoucementId == id);
            if (annoucement == null)
            {
                return NotFound();
            }

            return View(annoucement);
        }

        // POST: Annoucements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var annoucement = await _context.Annoucement.FindAsync(id);
            _context.Annoucement.Remove(annoucement);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnnoucementExists(int id)
        {
            return _context.Annoucement.Any(e => e.AnnoucementId == id);
        }
    }
}
