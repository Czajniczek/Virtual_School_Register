using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
    public class EvaluationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public EvaluationsController(ApplicationDbContext context, UserManager<User> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IActionResult> IndexTeacher()
        {
            var applicationDbContext = _context.Evaluation.Include(e => e.Subject).Include(e => e.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Evaluations
        [Authorize(Roles = "Uczen, Rodzic")]
        public async Task<IActionResult> Index(string childId)
        {
            List<Evaluation> evaluations = new List<Evaluation>();
            List<SubjectGradeViewModel> subjectGradesList = new List<SubjectGradeViewModel>();

            /*if (User.IsInRole("Nauczyciel"))
            {
               *//* var conductingLessons = await _context.ConductingLesson.Include(c => c.Class).Include(c => c.Subject).Include(c => c.User)
                    .Where(x => x.UserId == _userManager.GetUserId(HttpContext.User)).ToListAsync();*//*


                evaluations = await _context.Evaluation.Include(e => e.Subject).Include(e => e.User).ToListAsync();
            }
            else */

            var thisUser = new User();

            if (User.IsInRole("Rodzic"))
            {
                //Zakładamy, że rodzic ma jedno dziecko
                thisUser = _userManager.Users.FirstOrDefault(x => x.ParentId == _userManager.GetUserId(HttpContext.User));
            }
            else
            {
                thisUser = _userManager.Users.FirstOrDefault(x => x.Id == _userManager.GetUserId(HttpContext.User));
            }

            ViewBag.ThisUser = thisUser;

            var lessonsSubjectsIds = await _context.ConductingLesson.Include(c => c.Subject)
                .Where(x => x.ClassId == thisUser.ClassId).Distinct().OrderBy(x => x.SubjectId).ToListAsync();

            foreach (var subject in lessonsSubjectsIds)
            {
                SubjectGradeViewModel subjectGrade = new SubjectGradeViewModel();

                ICollection<Evaluation> grades = _context.Evaluation
                    .Where(x => x.UserId == thisUser.Id && x.SubjectId == subject.SubjectId).ToList();

                subjectGrade.SubjectId = subject.SubjectId;
                subjectGrade.SubjectName = subject.Subject.Name;
                subjectGrade.Evaluations = grades;

                subjectGradesList.Add(subjectGrade);
            }

            return View(subjectGradesList);
        }

        // GET: Evaluations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evaluation = await _context.Evaluation
                .Include(e => e.Subject)
                .Include(e => e.User)
                .FirstOrDefaultAsync(m => m.EvaluationId == id);
            if (evaluation == null)
            {
                return NotFound();
            }

            return View(evaluation);
        }

        // GET: Evaluations/Create
        public IActionResult Create(string userId, int subjectId)
        {
            ViewData["SubjectId"] = new SelectList(_context.Subject, "SubjectId", subjectId.ToString());
            ViewData["UserId"] = new SelectList(_context.Users, "Id", userId);

            ViewBag.BackTo = _context.ConductingLesson
                .FirstOrDefault(x => x.SubjectId == subjectId && x.ClassId == _userManager.Users.FirstOrDefault(x => x.Id == userId).ClassId);

            return View();
        }

        // POST: Evaluations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Evaluation evaluation)
        {
            if (ModelState.IsValid)
            {
                evaluation.Date = DateTime.Now;

                _context.Add(evaluation);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));

                var subjectId = _context.ConductingLesson.FirstOrDefault(x => x.SubjectId == evaluation.SubjectId);

                return RedirectToAction("Details", "ConductingLessons", new { id = subjectId.ConductingLessonId });
            }
            ViewData["SubjectId"] = new SelectList(_context.Subject, "SubjectId", "Content", evaluation.SubjectId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", evaluation.UserId);
            return View(evaluation);
        }

        // GET: Evaluations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evaluation = await _context.Evaluation.FindAsync(id);
            if (evaluation == null)
            {
                return NotFound();
            }
            ViewData["SubjectId"] = new SelectList(_context.Subject, "SubjectId", "Content", evaluation.SubjectId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", evaluation.UserId);
            return View(evaluation);
        }

        // POST: Evaluations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EvaluationId,UserId,Value,Date,Type,Comment,SubjectId")] Evaluation evaluation)
        {
            if (id != evaluation.EvaluationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(evaluation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EvaluationExists(evaluation.EvaluationId))
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
            ViewData["SubjectId"] = new SelectList(_context.Subject, "SubjectId", "Content", evaluation.SubjectId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", evaluation.UserId);
            return View(evaluation);
        }

        // GET: Evaluations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evaluation = await _context.Evaluation
                .Include(e => e.Subject)
                .Include(e => e.User)
                .FirstOrDefaultAsync(m => m.EvaluationId == id);
            if (evaluation == null)
            {
                return NotFound();
            }

            return View(evaluation);
        }

        // POST: Evaluations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var evaluation = await _context.Evaluation.FindAsync(id);
            _context.Evaluation.Remove(evaluation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EvaluationExists(int id)
        {
            return _context.Evaluation.Any(e => e.EvaluationId == id);
        }
    }
}
