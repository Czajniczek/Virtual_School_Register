using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Virtual_School_Register.Data;
using Virtual_School_Register.Models;

namespace Virtual_School_Register.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            //TODO: Nie pobierać całego użytkownika, tylko jego imię i nazwisko (Name and Surname)
            //var announcements = _context.Annoucement.Include(u => u.User).Select(x => new
            //{
            //     Title = x.Title,
            //     Content = x.Content,
            //     UserName = x.User.Name,
            //     UserSurname = x.User.Surname,
            //     Date = x.Date
            //})
            //    .OrderByDescending(x => x.Date)
            //    .ToList();

            //ViewBag.AnnoucementsList = announcements;

            var announcements = _context.Annoucement.Include(u => u.User)
                                                    .OrderByDescending(x => x.Date)
                                                    .ToList();

            //var announcements = _context.Annoucement.Include(u => u.User)
            //                                        .OrderBy(x => x.Date)
            //                                        .Reverse()
            //                                        .ToList();

            return View(announcements);
        }

        //Home/Privacy - Use this page to detail your site's privacy policy.
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
