using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Virtual_School_Register.Data;
using Virtual_School_Register.Models;
using Virtual_School_Register.ViewModels.HomeController;

namespace Virtual_School_Register.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHtmlLocalizer<HomeController> _localizer;

        public HomeController(ApplicationDbContext context, IHtmlLocalizer<HomeController> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public IActionResult Index()
        {
            var announcements = _context.Annoucement.Include(u => u.User).Select(x => new HomeIndexViewModel
            {
                Title = x.Title,
                Content = x.Content,
                Name = x.User.Name,
                Surname = x.User.Surname,
                Date = x.Date
            })
                .OrderByDescending(x => x.Date)
                .ToList();

            //var announcements = _context.Annoucement.Include(u => u.User)
            //                                        .OrderByDescending(x => x.Date)
            //                                        .ToList();

            //var test = _localizer["News"];
            //ViewData["News"] = test;

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

        [HttpPost]
        public IActionResult CultureManagement(string culture, string returnUrl)
        {
            Response.Cookies.Append(CookieRequestCultureProvider
                .DefaultCookieName, CookieRequestCultureProvider
                .MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.Now.AddDays(30) });

            return LocalRedirect(returnUrl);
        }
    }
}
