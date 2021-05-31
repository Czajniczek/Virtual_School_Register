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
    [Authorize(Roles = "Admin, Nauczyciel")]
    public class QuestionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuestionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Questions
        public async Task<IActionResult> Index(int testId)
        {
            var questions = await _context.Question.Include(q => q.Test).Where(x => x.TestId == testId).ToListAsync();

            ViewBag.BackTo = _context.Test.FirstOrDefault(x => x.TestId == testId);

            return View(questions);
        }

        // GET: Questions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Question
                .Include(q => q.Test)
                .FirstOrDefaultAsync(m => m.QuestionId == id);
            if (question == null)
            {
                return NotFound();
            }

            ViewBag.BackToTest = question.TestId;

            return View(question);
        }

        // GET: Questions/Create
        public IActionResult Create(int testId)
        {
            ViewData["TestId"] = new SelectList(_context.Set<Test>(), "TestId", testId.ToString());

            ViewBag.BackToTest = testId;

            return View();
        }

        // POST: Questions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("QuestionId,Content,AnswerA,AnswerB,AnswerC,AnswerD,CorrectAnswer,Points,TestId")] Question question)
        {
            if (ModelState.IsValid)
            {
                _context.Add(question);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Questions", new { testId = question.TestId });
            }
            ViewData["TestId"] = new SelectList(_context.Set<Test>(), "TestId", "Title", question.TestId);
            return View(question);
        }

        // GET: Questions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Question.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }

            ViewBag.BackToTest = question.TestId;

            ViewData["TestId"] = new SelectList(_context.Set<Test>(), "TestId", "Title", question.TestId);
            return View(question);
        }

        // POST: Questions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QuestionId,Content,AnswerA,AnswerB,AnswerC,AnswerD,CorrectAnswer,Points,TestId")] Question question)
        {
            if (id != question.QuestionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(question);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionExists(question.QuestionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction("Index", "Questions", new { testId = question.TestId });

            }
            ViewData["TestId"] = new SelectList(_context.Set<Test>(), "TestId", "Title", question.TestId);
            return View(question);
        }

        // GET: Questions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Question
                .Include(q => q.Test)
                .FirstOrDefaultAsync(m => m.QuestionId == id);
            if (question == null)
            {
                return NotFound();
            }

            ViewBag.BackToTest = question.TestId;

            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var question = await _context.Question.FindAsync(id);
            _context.Question.Remove(question);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Questions", new { testId = question.TestId });
        }

        private bool QuestionExists(int id)
        {
            return _context.Question.Any(e => e.QuestionId == id);
        }
    }
}
