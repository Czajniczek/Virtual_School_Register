using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Virtual_School_Register.Data;
using Virtual_School_Register.Models;

namespace Virtual_School_Register.Controllers
{
    [Authorize(Roles = "Admin, Nauczyciel")]
    public class AnnoucementsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public AnnoucementsController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Annoucements
        public async Task<IActionResult> Index()
        {
            List<Annoucement> announcements;

            if(User.IsInRole("Admin"))
            {
                announcements = await _context.Annoucement.Include(u => u.User).OrderBy(x => x.Title.ToLower()).ThenBy(x => x.Content).ToListAsync();
            }
            else
            {
                var thisUser = _userManager.GetUserId(HttpContext.User);

                announcements = await _context.Annoucement.Include(u => u.User).Where(x => x.UserId == thisUser).OrderBy(x => x.Title.ToLower()).ThenBy(x => x.Content).ToListAsync();
            }

            return View(announcements);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Annoucement annoucement)
        {
            if (ModelState.IsValid)
            {
                annoucement.UserId = _userManager.GetUserId(HttpContext.User);
                annoucement.Date = DateTime.Now;

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Annoucement annoucement)
        {
            if (id != annoucement.AnnoucementId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                annoucement.UserId = _userManager.GetUserId(HttpContext.User);
                annoucement.Date = DateTime.Now;

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
