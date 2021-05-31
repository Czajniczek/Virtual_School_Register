using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Virtual_School_Register.Data;
using Virtual_School_Register.Models;
using Virtual_School_Register.ViewModels;

namespace Virtual_School_Register.Controllers
{
    public class FilesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public FilesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: Files
        public async Task<IActionResult> Index(int subjectId)
        {
            var files = await _context.File.Include(f => f.Subject).Where(x => x.SubjectId == subjectId).ToListAsync();

            ViewBag.BackTo = _context.Subject.FirstOrDefault(x => x.SubjectId == subjectId);

            return View(files);
        }

        public ActionResult DownloadFile(int fileId)
        {
            var file = _context.File.Find(fileId);

            string type = "application/" + file.FileType.ToString();

            return File(file.DataFiles, type, file.Name);
        }

        // GET: Files/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var file = await _context.File
                .Include(f => f.Subject)
                .FirstOrDefaultAsync(m => m.FileId == id);
            if (file == null)
            {
                return NotFound();
            }

            ViewBag.BackToSubject = file.SubjectId;

            return View(file);
        }

        // GET: Files/Create
        public IActionResult Create(int subjectId)
        {
            ViewData["SubjectId"] = new SelectList(_context.Subject, "SubjectId", subjectId.ToString());

            ViewBag.BackToSubject = subjectId;

            return View();
        }

        // POST: Files/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Models.File file, IFormFile files)
        {
            if (ModelState.IsValid)
            {
                if (files != null)
                {
                    if (files.Length > 0)
                    {
                        //Getting FileName
                        var fileName = Path.GetFileName(files.FileName);
                        //Getting file Extension
                        var fileExtension = Path.GetExtension(fileName);
                        // concatenating  FileName + FileExtension
                        //var newFileName = String.Concat(Convert.ToString(Guid.NewGuid()), fileExtension);

                        /*file.Name = newFileName;*/
                        file.Name = fileName;
                        file.FileType = fileExtension;
                        file.Date = DateTime.Now;

                        using (var target = new MemoryStream())
                        {
                            files.CopyTo(target);
                            file.DataFiles = target.ToArray();
                        }
                    }
                }
                else
                {
                    ViewData["SubjectId"] = new SelectList(_context.Subject, "SubjectId", "Content", file.SubjectId);
                    return View(file);
                }
                
                _context.Add(file);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Files", new { subjectId = file.SubjectId});

            }
            ViewData["SubjectId"] = new SelectList(_context.Subject, "SubjectId", "Content", file.SubjectId);
            return View(file);
        }

        // GET: Files/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var file = await _context.File.FindAsync(id);
            if (file == null)
            {
                return NotFound();
            }
            ViewData["SubjectId"] = new SelectList(_context.Subject, "SubjectId", "Name", file.SubjectId);
            return View(file);
        }

        // POST: Files/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FileId,Name,SubjectId")] Models.File file)
        {
            if (id != file.FileId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(file);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FileExists(file.FileId))
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
            ViewData["SubjectId"] = new SelectList(_context.Subject, "SubjectId", "Content", file.SubjectId);
            return View(file);
        }

        // GET: Files/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var file = await _context.File
                .Include(f => f.Subject)
                .FirstOrDefaultAsync(m => m.FileId == id);
            if (file == null)
            {
                return NotFound();
            }

            ViewBag.BackToSubject = file.SubjectId;

            return View(file);
        }

        // POST: Files/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var file = await _context.File.FindAsync(id);
            _context.File.Remove(file);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Files", new { subjectId = file.SubjectId });
        }

        private bool FileExists(int id)
        {
            return _context.File.Any(e => e.FileId == id);
        }
    }
}
