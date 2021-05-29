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

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> IndexTeacher()
        {
            var applicationDbContext = _context.Evaluation.Include(e => e.Subject).Include(e => e.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Evaluations
        public async Task<IActionResult> Index(string userId, DateTime startDate, DateTime endDate)
        {
            List<Evaluation> evaluations = new List<Evaluation>();
            List<SubjectGradeViewModel> subjectGradesList = new List<SubjectGradeViewModel>();

            var thisUser = new User();

            if (userId != null)
            {
                thisUser = _userManager.Users.FirstOrDefault(x => x.Id == userId);
            }
            else
            {
                if (User.IsInRole("Rodzic"))
                {
                    //Zakładamy, że rodzic ma jedno dziecko
                    thisUser = _userManager.Users.FirstOrDefault(x => x.ParentId == _userManager.GetUserId(HttpContext.User));
                }
                else
                {
                    thisUser = _userManager.Users.FirstOrDefault(x => x.Id == _userManager.GetUserId(HttpContext.User));
                }
            }

            ViewBag.ThisUser = thisUser;

            var lessonsSubjectsIds = await _context.ConductingLesson.Include(c => c.Subject)
                .Where(x => x.ClassId == thisUser.ClassId).Distinct().OrderBy(x => x.SubjectId).ToListAsync();

            foreach (var subject in lessonsSubjectsIds)
            {
                SubjectGradeViewModel subjectGrade = new SubjectGradeViewModel();

                ICollection<Evaluation> grades = _context.Evaluation
                    .Where(x => x.UserId == thisUser.Id && x.SubjectId == subject.SubjectId).ToList();

                if (startDate.Year != 0001)
                {
                    grades = grades.Where(x => x.Date.Date >= startDate.Date).ToList();
                }

                if (endDate.Year != 0001)
                {
                    grades = grades.Where(x => x.Date.Date <= endDate.Date).ToList();
                }

                subjectGrade.SubjectId = subject.SubjectId;
                subjectGrade.SubjectName = subject.Subject.Name;
                subjectGrade.Evaluations = grades;

                subjectGradesList.Add(subjectGrade);
            }

            if (startDate.Year != 0001)
            {
                ViewBag.StartDate = startDate.Date;
            }

            if (endDate.Year != 0001)
            {
                ViewBag.EndDate = endDate.Date;
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

            var userClass = _userManager.Users.FirstOrDefault(x => x.Id == evaluation.UserId).ClassId;

            var conductingLesson = _context.ConductingLesson.FirstOrDefault(x => x.SubjectId == evaluation.SubjectId && x.ClassId == userClass);

            ViewBag.BackToId = conductingLesson.ConductingLessonId;

            return View(evaluation);
        }

        public async Task<IActionResult> DetailsFromClasses(int? id)
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

                var subjectId = _context.ConductingLesson.FirstOrDefault(x => x.SubjectId == evaluation.SubjectId);

                return RedirectToAction("Details", "ConductingLessons", new { id = subjectId.ConductingLessonId });
            }
            ViewData["SubjectId"] = new SelectList(_context.Subject, "SubjectId", "Content", evaluation.SubjectId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", evaluation.UserId);
            return View(evaluation);
        }

        public IActionResult CreateFromClasses(string userId, int subjectId)
        {
            ViewData["SubjectId"] = new SelectList(_context.Subject, "SubjectId", subjectId.ToString());
            ViewData["UserId"] = new SelectList(_context.Users, "Id", userId);

            ViewBag.BackTo = _context.ConductingLesson
                .FirstOrDefault(x => x.SubjectId == subjectId && x.ClassId == _userManager.Users.FirstOrDefault(x => x.Id == userId).ClassId);

            ViewBag.UserId = userId;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFromClasses(Evaluation evaluation)
        {
            if (ModelState.IsValid)
            {
                evaluation.Date = DateTime.Now;

                _context.Add(evaluation);
                await _context.SaveChangesAsync();

                var subjectId = _context.ConductingLesson.FirstOrDefault(x => x.SubjectId == evaluation.SubjectId);

                return RedirectToAction("Index", "Evaluations", new { userId = evaluation.UserId });
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

            var userClass = _userManager.Users.FirstOrDefault(x => x.Id == evaluation.UserId).ClassId;

            var conductingLesson = _context.ConductingLesson.FirstOrDefault(x => x.SubjectId == evaluation.SubjectId && x.ClassId == userClass);

            ViewBag.BackToId = conductingLesson.ConductingLessonId;

            return View(evaluation);
        }

        // POST: Evaluations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Evaluation evaluation)
        {
            if (id != evaluation.EvaluationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    evaluation.Date = DateTime.Now;

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

                var userClass = _userManager.Users.FirstOrDefault(x => x.Id == evaluation.UserId).ClassId;

                var conductingLesson = _context.ConductingLesson.FirstOrDefault(x => x.SubjectId == evaluation.SubjectId && x.ClassId == userClass);

                return RedirectToAction("Details", "ConductingLessons", new { id = conductingLesson.ConductingLessonId });
            }
            ViewData["SubjectId"] = new SelectList(_context.Subject, "SubjectId", "Content", evaluation.SubjectId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", evaluation.UserId);
            return View(evaluation);
        }

        public async Task<IActionResult> EditFromClasses(int? id)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditFromClasses(int id, Evaluation evaluation)
        {
            if (id != evaluation.EvaluationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    evaluation.Date = DateTime.Now;

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

                return RedirectToAction("Index", "Evaluations", new { userId = evaluation.UserId });
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

            var userClass = _userManager.Users.FirstOrDefault(x => x.Id == evaluation.UserId).ClassId;

            var conductingLesson = _context.ConductingLesson.FirstOrDefault(x => x.SubjectId == evaluation.SubjectId && x.ClassId == userClass);

            ViewBag.BackToId = conductingLesson.ConductingLessonId;

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
            //return RedirectToAction(nameof(Index));

            var userClass = _userManager.Users.FirstOrDefault(x => x.Id == evaluation.UserId).ClassId;

            var conductingLesson = _context.ConductingLesson.FirstOrDefault(x => x.SubjectId == evaluation.SubjectId && x.ClassId == userClass);

            return RedirectToAction("Details", "ConductingLessons", new { id = conductingLesson.ConductingLessonId });
        }

        public async Task<IActionResult> DeleteFromClasses(int? id)
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

        [HttpPost, ActionName("DeleteFromClasses")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFromClassesConfirmed(int id)
        {
            var evaluation = await _context.Evaluation.FindAsync(id);
            _context.Evaluation.Remove(evaluation);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Evaluations", new { userId = evaluation.UserId });
        }

        private bool EvaluationExists(int id)
        {
            return _context.Evaluation.Any(e => e.EvaluationId == id);
        }
    }
}
