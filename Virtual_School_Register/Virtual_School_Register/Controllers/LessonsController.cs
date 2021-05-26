﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Virtual_School_Register.Data;
using Virtual_School_Register.Models;
using Virtual_School_Register.ViewModels;

namespace Virtual_School_Register.Controllers
{
    [Authorize(Roles = "Admin, Nauczyciel")]
    public class LessonsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public LessonsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: Lessons
        public async Task<IActionResult> Index()
        {
            var lessons = await _context.Lesson.Include(l => l.ConductingLesson).ToListAsync();

            var classes = await _context.Class.ToListAsync();
            var subjects = await _context.Subject.ToListAsync();
            var conductingLessons = await _context.ConductingLesson.ToListAsync();

            List<LessonViewModel> lessonsViewModelList = new List<LessonViewModel>();

            foreach (var c in lessons)
            {
                var item = _mapper.Map<LessonViewModel>(c);

                var lesson = conductingLessons.Find(x => x.ConductingLessonId == item.ConductingLessonId);

                if(lesson != null)
                {
                    item.ClassName = classes.Find(x => x.ClassId == lesson.ClassId).Name;
                    item.SubjectName = subjects.Find(x => x.SubjectId == lesson.SubjectId).Name;
                }

                lessonsViewModelList.Add(item);
            }

            /*foreach (var c in lessons)
            {
                var @class = classes.Find(x => x.ClassId == c.ConductingLesson.ClassId);
                var subject = subjects.Find(x => x.SubjectId == c.ConductingLesson.SubjectId);
                c. = @class.Name + " " + subject.Name;
            }*/

            /*ViewBag.ClassesList = classes;
            ViewBag.SubjectsList = subjects;*/

            return View(lessonsViewModelList);
        }

        // GET: Lessons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lesson = await _context.Lesson
                .Include(l => l.ConductingLesson)
                .FirstOrDefaultAsync(m => m.LessonId == id);
            if (lesson == null)
            {
                return NotFound();
            }

            return View(lesson);
        }

        // GET: Lessons/Create
        public IActionResult Create(int conductingLesson)
        {
            ViewData["ConductingLessonId"] = new SelectList(_context.ConductingLesson, "ConductingLessonId", "ConductingLessonId");

            ViewBag.ConductingLessonId = conductingLesson;

            return View();
        }

        // POST: Lessons/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LessonId,Title,Content,ConductingLessonId")] Lesson lesson)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lesson);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ConductingLessonId"] = new SelectList(_context.ConductingLesson, "ConductingLessonId", "ConductingLessonId", lesson.ConductingLessonId);
            return View(lesson);
        }

        // GET: Lessons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lesson = await _context.Lesson.FindAsync(id);
            if (lesson == null)
            {
                return NotFound();
            }
            ViewData["ConductingLessonId"] = new SelectList(_context.ConductingLesson, "ConductingLessonId", "ConductingLessonId", lesson.ConductingLessonId);
            return View(lesson);
        }

        // POST: Lessons/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LessonId,Title,Content,ConductingLessonId")] Lesson lesson)
        {
            if (id != lesson.LessonId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lesson);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LessonExists(lesson.LessonId))
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
            ViewData["ConductingLessonId"] = new SelectList(_context.ConductingLesson, "ConductingLessonId", "ConductingLessonId", lesson.ConductingLessonId);
            return View(lesson);
        }

        // GET: Lessons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lesson = await _context.Lesson
                .Include(l => l.ConductingLesson)
                .FirstOrDefaultAsync(m => m.LessonId == id);
            if (lesson == null)
            {
                return NotFound();
            }

            var deleteModel = _mapper.Map<LessonViewModel>(lesson);

            var conductingLesson = await _context.ConductingLesson.FirstOrDefaultAsync(x => x.ConductingLessonId == deleteModel.ConductingLessonId);
            var @class = await _context.Class.FirstOrDefaultAsync(x => x.ClassId == conductingLesson.ClassId);
            var subject = await _context.Subject.FirstOrDefaultAsync(x => x.SubjectId == conductingLesson.SubjectId);

            deleteModel.ClassName = @class.Name;
            deleteModel.SubjectName = subject.Name;

            return View(deleteModel);
        }

        // POST: Lessons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lesson = await _context.Lesson.FindAsync(id);
            _context.Lesson.Remove(lesson);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LessonExists(int id)
        {
            return _context.Lesson.Any(e => e.LessonId == id);
        }
    }
}
