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
using Virtual_School_Register.ViewModels;

namespace Virtual_School_Register.Controllers
{
    [Authorize(Roles = "Admin, Nauczyciel")]
    public class ConductingLessonsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public ConductingLessonsController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ConductingLessons
        public IActionResult Index()
        {
            List<ConductingLesson> teachings;

            if (User.IsInRole("Nauczyciel"))
            {
                teachings = _context.ConductingLesson.Include(c => c.Class).Include(c => c.Subject).Include(c => c.User)
                    .Where(x => x.UserId == _userManager.GetUserId(HttpContext.User))
                    .OrderBy(x => x.Class.Name).ThenBy(x => x.Subject.Name).ToList();
            } 
            else
            {
                teachings = _context.ConductingLesson.Include(c => c.Class).Include(c => c.Subject).Include(c => c.User).OrderBy(x => x.Class.Name).ThenBy(x => x.Subject.Name).ToList();
            }

            return View(teachings);
        }

        // GET: ConductingLessons/Details/5
        public async Task<IActionResult> Details(int? id, DateTime startDate, DateTime endDate)
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

            ViewBag.ConductingLesson = conductingLesson;

            var users = await _userManager.Users.Include(u => u.Class).Where(x => x.ClassId == conductingLesson.ClassId)
                          .OrderBy(x => x.Surname.ToLower()).ThenBy(x => x.Name.ToLower()).ToListAsync();

            List<UsersGradesViewModel> usersGradesList = new List<UsersGradesViewModel>();

            foreach (var user in users)
            {
                UsersGradesViewModel usersGrades = new UsersGradesViewModel();

                ICollection<Evaluation> grades = _context.Evaluation
                    .Where(x => x.UserId == user.Id && x.SubjectId == conductingLesson.SubjectId).ToList();

                if (startDate.Year != 0001)
                {
                    grades = grades.Where(x => x.Date.Date >= startDate.Date).ToList();
                }

                if (endDate.Year != 0001)
                {
                    grades = grades.Where(x => x.Date.Date <= endDate.Date).ToList();
                }

                usersGrades.User = user;
                usersGrades.Evaluations = grades;

                usersGradesList.Add(usersGrades);
            }

            if (startDate.Year != 0001)
            {
                ViewBag.StartDate = startDate.Date;
            }

            if (endDate.Year != 0001)
            {
                ViewBag.EndDate = endDate.Date;
            }

            return View(usersGradesList);
        }

        // GET: ConductingLessons/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["ClassId"] = new SelectList(_context.Class, "ClassId", "Content");
            ViewData["SubjectId"] = new SelectList(_context.Set<Subject>(), "SubjectId", "Content");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: ConductingLessons/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
            ViewData["ClassId"] = new SelectList(_context.Class, "ClassId", "Name", conductingLesson.ClassId);
            ViewData["SubjectId"] = new SelectList(_context.Set<Subject>(), "SubjectId", "Name", conductingLesson.SubjectId);

            var users = await _userManager.Users.Where(x => x.Type == "Nauczyciel").ToListAsync();

            ViewBag.TeachersList = users;

            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", conductingLesson.UserId);
            return View(conductingLesson);
        }

        // POST: ConductingLessons/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, ConductingLesson conductingLesson)
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
        [Authorize(Roles = "Admin")]
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

            if(conductingLesson.UserId != null)
            {
                conductingLesson.UserId = null;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: ConductingLessons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
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
