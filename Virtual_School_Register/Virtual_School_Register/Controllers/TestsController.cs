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
    [Authorize(Roles = "Admin, Nauczyciel, Uczen")]
    public class TestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public TestsController(ApplicationDbContext context, IMapper mapper, UserManager<User> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        // GET: Tests
        public async Task<IActionResult> Index()
        {
            List<Test> tests = new List<Test>();
            List<ConductingLesson> conductingLessons = new List<ConductingLesson>();

            if (User.IsInRole("Nauczyciel"))
            {
                conductingLessons = await _context.ConductingLesson.Where(x => x.UserId == _userManager.GetUserId(HttpContext.User))
                    .Include(c => c.Class).Include(c => c.Subject).Include(c => c.User).ToListAsync();

                var conductingLessonsIds = await _context.ConductingLesson.Where(x => x.UserId == _userManager.GetUserId(HttpContext.User))
                    .Select(x => x.ConductingLessonId).ToListAsync();

                tests = await _context.Test.Where(x => conductingLessonsIds.Contains(x.ConductingLessonId)).ToListAsync();
            }
            else if (User.IsInRole("Uczen"))
            {
                var userClass = _userManager.Users.FirstOrDefault(x => x.Id == _userManager.GetUserId(HttpContext.User)).ClassId;

                conductingLessons = await _context.ConductingLesson.Where(x => x.ClassId == userClass)
                    .Include(c => c.Class).Include(c => c.Subject).Include(c => c.User).ToListAsync();

                var conductingLessonsIds = await _context.ConductingLesson.Where(x => x.ClassId == userClass)
                    .Select(x => x.ConductingLessonId).ToListAsync();

                tests = await _context.Test.Where(x => conductingLessonsIds.Contains(x.ConductingLessonId)).ToListAsync();
            }
            else
            {
                conductingLessons = await _context.ConductingLesson.Include(c => c.Class).Include(c => c.Subject).Include(c => c.User).ToListAsync();

                tests = await _context.Test.ToListAsync();
            }

            List<TestViewModel> testsList = new List<TestViewModel>();

            foreach (var test in tests)
            {
                var newTest = _mapper.Map<TestViewModel>(test);

                var conductingLesson = conductingLessons.FirstOrDefault(x => x.ConductingLessonId == test.ConductingLessonId);

                newTest.SubjectName = conductingLesson.Subject.Name;
                newTest.ClassName = conductingLesson.Class.Name;

                testsList.Add(newTest);
            }

            //testsList.OrderBy(x => x.ClassName).ThenBy(x => x.SubjectName);

            return View(testsList);
        }

        public IActionResult BeginTest(int testId, int myPoints, int myQuestion)
        {
            var questions = _context.Question.Where(x => x.TestId == testId).ToList();

            if (myQuestion < questions.Count)
            {
                return RedirectToAction("BeginQuestion", "Tests", new { questionId = questions[myQuestion].QuestionId, testId = testId, myPoints = myPoints, myQuestion = myQuestion });
            }

            if(questions.Count != 0)
            {
                return RedirectToAction("EndTest", "Tests", new { testId = testId, myPoints = myPoints });
            }
            else
            {
                return RedirectToAction("Index", "Tests");
            }
        }

        public IActionResult EndTest(int testId, int myPoints)
        {
            float maxPoints = 0;

            var questions = _context.Question.Where(x => x.TestId == testId).ToList();

            foreach (var question in questions)
            {
                maxPoints = maxPoints + question.Points;
            }

            float percent = 0;

            if (maxPoints != 0)
            {
                percent = (myPoints / maxPoints) * 100;
            }

            string grade = "";

            if (percent >= 98)
            {
                grade = "6";
            }
            else if (percent >= 86)
            {
                grade = "5";
            }
            else if (percent >= 71)
            {
                grade = "4";
            }
            else if (percent >= 51)
            {
                grade = "3";
            }
            else if (percent >= 41)
            {
                grade = "2";
            }
            else
            {
                grade = "1";
            }

            var test = _context.Test.FirstOrDefault(x => x.TestId == testId);

            var conductingLesson = _context.ConductingLesson.FirstOrDefault(x => x.ConductingLessonId == test.ConductingLessonId);

            Evaluation evaluation = new Evaluation();

            evaluation.UserId = _userManager.GetUserId(HttpContext.User);
            evaluation.Value = grade;
            evaluation.Date = DateTime.Now;
            evaluation.Type = "Sprawdzian";
            evaluation.Comment = "Test: " + test.Title;
            evaluation.SubjectId = conductingLesson.SubjectId;

            _context.Evaluation.Add(evaluation);
            _context.SaveChangesAsync();

            ViewBag.Result = $"Your score is: {myPoints.ToString()} / {maxPoints} points.";
            ViewBag.Grade = $"Your grade is: {grade}. It was inserted automatically.";

            return View();
        }

        public async Task<IActionResult> BeginQuestion(int questionId, int testId, int myPoints, int myQuestion)
        {
            var question = await _context.Question.FirstOrDefaultAsync(x => x.QuestionId == questionId && x.TestId == testId);

            var newQuestion = _mapper.Map<QuestionViewModel>(question);

            newQuestion.MyPoints = myPoints;
            newQuestion.MyQuestion = myQuestion;

            return View(newQuestion);
        }

        [HttpPost]
        public IActionResult BeginQuestion(QuestionViewModel question)
        {
            if (question.Answer == question.CorrectAnswer)
            {
                question.MyPoints += question.Points;
            }

            question.MyQuestion++;

            return RedirectToAction("BeginTest", "Tests", new { testId = question.TestId, myPoints = question.MyPoints, myQuestion = question.MyQuestion });
        }

        // GET: Tests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var test = await _context.Test
                .FirstOrDefaultAsync(m => m.TestId == id);
            if (test == null)
            {
                return NotFound();
            }

            var newTest = _mapper.Map<TestViewModel>(test);

            var conductingLesson = _context.ConductingLesson.Include(c => c.Class).Include(c => c.Subject).Include(c => c.User)
                .FirstOrDefault(x => x.ConductingLessonId == test.ConductingLessonId);

            newTest.SubjectName = conductingLesson.Subject.Name;
            newTest.ClassName = conductingLesson.Class.Name;

            return View(newTest);
        }

        // GET: Tests/Create
        [Authorize(Roles = "Admin, Nauczyciel")]
        public IActionResult Create()
        {
            List<ConductingLesson> conductingLessonsList = new List<ConductingLesson>();

            if (User.IsInRole("Nauczyciel"))
            {
                conductingLessonsList = _context.ConductingLesson
                    .Where(x => x.UserId == _userManager.GetUserId(HttpContext.User))
                    .Include(c => c.Class).Include(c => c.Subject).Include(c => c.User)
                    .OrderBy(x => x.Class.Name).ThenBy(x => x.Subject.Name).ToList();
            }
            else
            {
                conductingLessonsList = _context.ConductingLesson
                    .Include(c => c.Class).Include(c => c.Subject).Include(c => c.User)
                    .OrderBy(x => x.Class.Name).ThenBy(x => x.Subject.Name).ToList();
            }

            ViewBag.ConductingLessons = conductingLessonsList;

            return View();
        }

        // POST: Tests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Nauczyciel")]
        public async Task<IActionResult> Create([Bind("TestId,Title,Time,ConductingLessonId")] Test test)
        {
            if (ModelState.IsValid)
            {
                _context.Add(test);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(test);
        }

        // GET: Tests/Edit/5
        [Authorize(Roles = "Admin, Nauczyciel")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var test = await _context.Test.FindAsync(id);
            if (test == null)
            {
                return NotFound();
            }

            List<ConductingLesson> conductingLessonsList = new List<ConductingLesson>();

            if (User.IsInRole("Nauczyciel"))
            {
                conductingLessonsList = _context.ConductingLesson
                    .Where(x => x.UserId == _userManager.GetUserId(HttpContext.User))
                    .Include(c => c.Class).Include(c => c.Subject).Include(c => c.User)
                    .OrderBy(x => x.Class.Name).ThenBy(x => x.Subject.Name).ToList();
            }
            else
            {
                conductingLessonsList = _context.ConductingLesson
                    .Include(c => c.Class).Include(c => c.Subject).Include(c => c.User)
                    .OrderBy(x => x.Class.Name).ThenBy(x => x.Subject.Name).ToList();
            }

            ViewBag.ConductingLessons = conductingLessonsList;

            return View(test);
        }

        // POST: Tests/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Nauczyciel")]
        public async Task<IActionResult> Edit(int id, [Bind("TestId,Title,Time,ConductingLessonId")] Test test)
        {
            if (id != test.TestId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(test);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestExists(test.TestId))
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
            return View(test);
        }

        // GET: Tests/Delete/5
        [Authorize(Roles = "Admin, Nauczyciel")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var test = await _context.Test
                .FirstOrDefaultAsync(m => m.TestId == id);
            if (test == null)
            {
                return NotFound();
            }

            var newTest = _mapper.Map<TestViewModel>(test);

            var conductingLesson = _context.ConductingLesson.Include(c => c.Class).Include(c => c.Subject).Include(c => c.User)
                .FirstOrDefault(x => x.ConductingLessonId == test.ConductingLessonId);

            newTest.SubjectName = conductingLesson.Subject.Name;
            newTest.ClassName = conductingLesson.Class.Name;

            return View(newTest);
        }

        // POST: Tests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Nauczyciel")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var test = await _context.Test.FindAsync(id);
            _context.Test.Remove(test);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestExists(int id)
        {
            return _context.Test.Any(e => e.TestId == id);
        }
    }
}
