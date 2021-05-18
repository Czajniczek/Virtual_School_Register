using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Virtual_School_Register.Data;

namespace Virtual_School_Register.Controllers
{
    public class UsersController : Controller
    {

        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var myDbContext = _context.Users.Include(u => u.Class);
            
            return View(await myDbContext.ToListAsync());
        }


    }
}
