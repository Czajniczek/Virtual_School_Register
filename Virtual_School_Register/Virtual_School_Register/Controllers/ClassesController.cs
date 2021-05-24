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
    [Authorize(Roles = "Admin")]
    public class ClassesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public ClassesController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Classes
        public async Task<IActionResult> Index()
        {
            var classes = await _context.Class.OrderBy(x => x.Name.ToLower()).ToListAsync();
            var users = await _userManager.Users.Where(x => x.Type == "Nauczyciel").ToListAsync();

            foreach (var c in classes)
            {
                if (c.ClassTutorId != null)
                {
                    var teacher = users.Find(x => x.Id == c.ClassTutorId);
                    c.ClassTutorId = teacher.Name + " " + teacher.Surname;
                }
            }

            return View(classes);
        }

        // GET: Classes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @class = await _context.Class
                .FirstOrDefaultAsync(m => m.ClassId == id);
            if (@class == null)
            {
                return NotFound();
            }

            if (@class.ClassTutorId != null)
            {
                var teacher = await _context.Users.FindAsync(@class.ClassTutorId);
                @class.ClassTutorId = teacher.Name + " " + teacher.Surname;
            }

            var students = _userManager.Users.Where(u => u.ClassId == id && u.Type == "Uczen").OrderBy(x => x.Surname.ToLower()).ThenBy(x => x.Name.ToLower());
            ViewBag.StudentsList = students.ToList();

            var studentsWithNoClass = _userManager.Users.Where(u => u.ClassId == null && u.Type == "Uczen").OrderBy(x => x.Surname.ToLower()).ThenBy(x => x.Name.ToLower());
            ViewBag.StudentsWithNoClassList = studentsWithNoClass.ToList();

            ViewBag.ClassId = id;

            //Session["classId"] = id;

            return View(@class);
        }

        public async Task<IActionResult> Subjects(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @class = await _context.Class
                .FirstOrDefaultAsync(m => m.ClassId == id);

            if (@class == null)
            {
                return NotFound();
            }

            if (@class.ClassTutorId != null)
            {
                var teacher = await _context.Users.FindAsync(@class.ClassTutorId);
                @class.ClassTutorId = teacher.Name + " " + teacher.Surname;
            }

            var existingSubjects = _context.ConductingLesson.Where(c => c.ClassId == id).Select(x => x.SubjectId).Distinct().ToList();
            var classesSubjects = _context.Subject.Where(x => existingSubjects.Contains(x.SubjectId)).ToList();
            ViewBag.SubjectsList = classesSubjects;

            var allSubjects = _context.Subject.Where(x => !existingSubjects.Contains(x.SubjectId));
            ViewBag.AllSubjectsList = allSubjects.ToList();

            //var subjects = subjectsId
            //ViewBag.SubjectsList = subjects.ToList();

            //var studentsWithNoClass = _userManager.Users.Where(u => u.ClassId == null && u.Type == "Uczen").OrderBy(x => x.Surname.ToLower()).ThenBy(x => x.Name.ToLower());
            //ViewBag.StudentsWithNoClassList = studentsWithNoClass.ToList();

            ViewBag.ClassId = id;

            return View(@class);
        }

        // GET: Classes/Create
        public IActionResult Create()
        {
            var tutors = _context.Class.Where(c => c.ClassTutorId != null).Select(x => x.ClassTutorId);
            var notTutors = _userManager.Users.Where(x => x.Type == "Nauczyciel" && !tutors.Contains(x.Id)).ToList();

            ViewBag.TutorsList = notTutors;

            return View();
        }

        // POST: Classes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClassId,Name,Content,ClassTutorId")] Class @class)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@class);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@class);
        }

        // GET: Classes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @class = await _context.Class.FindAsync(id);
            if (@class == null)
            {
                return NotFound();
            }

            var tutors = _context.Class.Where(c => c.ClassTutorId != null).Select(x => x.ClassTutorId);
            var notTutors = _userManager.Users.Where(x => x.Type == "Nauczyciel" && !tutors.Contains(x.Id)).ToList();

            if(@class.ClassTutorId != null)
            {
                var tutor = _userManager.Users.First(x => x.Id == @class.ClassTutorId);

                notTutors.Add(tutor);
            }

            ViewBag.TutorsList = notTutors;

            return View(@class);
        }

        // POST: Classes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClassId,Name,Content,ClassTutorId")] Class @class)
        {
            if (id != @class.ClassId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@class);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassExists(@class.ClassId))
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
            return View(@class);
        }

        // GET: Classes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @class = await _context.Class.FirstOrDefaultAsync(m => m.ClassId == id);

            if (@class == null)
            {
                return NotFound();
            }

            if (@class.ClassTutorId != null)
            {
                var teacher = await _context.Users.FindAsync(@class.ClassTutorId);
                @class.ClassTutorId = teacher.Name + " " + teacher.Surname;
            }

            return View(@class);
        }

        // POST: Classes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @class = await _context.Class.FindAsync(id);
            _context.Class.Remove(@class);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteUser(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _userManager.FindByIdAsync(id);
            var classId = student.ClassId;
            student.ClassId = null;
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = classId });
        }

        public async Task<IActionResult> AddUser(string id, int classId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _userManager.FindByIdAsync(id);
            student.ClassId = classId;
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = classId });
        }

        public async Task<IActionResult> RemoveSubject(int? id, int classId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = _context.ConductingLesson.Where(x => x.SubjectId == id);

            foreach (var sub in subject)
            {
                _context.ConductingLesson.Remove(sub);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Subjects", new { id = classId });
        }

        public async Task<IActionResult> AddSubject(int id, int classId)
        {
            var conductingLesson = new ConductingLesson { SubjectId = id, ClassId = classId };

            await _context.ConductingLesson.AddAsync(conductingLesson);
            await _context.SaveChangesAsync();

            return RedirectToAction("Subjects", new { id = classId });
        }

        private bool ClassExists(int id)
        {
            return _context.Class.Any(e => e.ClassId == id);
        }
    }
}
