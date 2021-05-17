using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Virtual_School_Register.Data;
using Virtual_School_Register.Models;

namespace Virtual_School_Register.Controllers
{
    public class ConductingLessonsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ConductingLessonsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ConductingLessons
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ConductingLesson.Include(c => c.Class).Include(c => c.Subject).Include(c => c.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ConductingLessons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conductingLesson = await _context.ConductingLesson
                .Include(c => c.Class)
                .Include(c => c.Subject)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.ConductingLessonId == id);
            if (conductingLesson == null)
            {
                return NotFound();
            }

            return View(conductingLesson);
        }

        // GET: ConductingLessons/Create
        public IActionResult Create()
        {
            ViewData["ClassId"] = new SelectList(_context.Class, "ClassId", "Content");
            ViewData["SubjectId"] = new SelectList(_context.Set<Subject>(), "SubjectId", "Content");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: ConductingLessons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ConductingLessonId,UserId,ClassId,SubjectId")] ConductingLesson conductingLesson)
        {
            if (ModelState.IsValid)
            {
                _context.Add(conductingLesson);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClassId"] = new SelectList(_context.Class, "ClassId", "Content", conductingLesson.ClassId);
            ViewData["SubjectId"] = new SelectList(_context.Set<Subject>(), "SubjectId", "Content", conductingLesson.SubjectId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", conductingLesson.UserId);
            return View(conductingLesson);
        }

        // GET: ConductingLessons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conductingLesson = await _context.ConductingLesson.FindAsync(id);
            if (conductingLesson == null)
            {
                return NotFound();
            }
            ViewData["ClassId"] = new SelectList(_context.Class, "ClassId", "Content", conductingLesson.ClassId);
            ViewData["SubjectId"] = new SelectList(_context.Set<Subject>(), "SubjectId", "Content", conductingLesson.SubjectId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", conductingLesson.UserId);
            return View(conductingLesson);
        }

        // POST: ConductingLessons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ConductingLessonId,UserId,ClassId,SubjectId")] ConductingLesson conductingLesson)
        {
            if (id != conductingLesson.ConductingLessonId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(conductingLesson);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConductingLessonExists(conductingLesson.ConductingLessonId))
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
            ViewData["ClassId"] = new SelectList(_context.Class, "ClassId", "Content", conductingLesson.ClassId);
            ViewData["SubjectId"] = new SelectList(_context.Set<Subject>(), "SubjectId", "Content", conductingLesson.SubjectId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", conductingLesson.UserId);
            return View(conductingLesson);
        }

        // GET: ConductingLessons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conductingLesson = await _context.ConductingLesson
                .Include(c => c.Class)
                .Include(c => c.Subject)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.ConductingLessonId == id);
            if (conductingLesson == null)
            {
                return NotFound();
            }

            return View(conductingLesson);
        }

        // POST: ConductingLessons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var conductingLesson = await _context.ConductingLesson.FindAsync(id);
            _context.ConductingLesson.Remove(conductingLesson);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConductingLessonExists(int id)
        {
            return _context.ConductingLesson.Any(e => e.ConductingLessonId == id);
        }
    }
}
