using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Virtual_School_Register.Data;
using Virtual_School_Register.Models;
using Virtual_School_Register.ViewModels;

namespace Virtual_School_Register.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager; //Do zarządzania userami
        private readonly RoleManager<IdentityRole> _roleManager; //Do zarządzania rolami :D 
        private readonly IMapper _mapper;

        public UsersController(ApplicationDbContext context, UserManager<User> userManager,
            IMapper mapper, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var myDbContext = _context.Users.Include(u => u.Class);

            return View(await myDbContext.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var detailsModel = _mapper.Map<UserDetailsViewModel>(user);
            return View(detailsModel);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["ClassId"] = new SelectList(_context.Class, "ClassId", "Name");
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Create([Bind("Id,UserName,Password,Name,Surname,Sex,Email,PhoneNumber,BirthDate,Adress,ParentId,Type,ClassId")] User user)
        public async Task<IActionResult> Create(UserCreateViewModel user)
        {
            if (ModelState.IsValid)
            {
                if (!await _roleManager.RoleExistsAsync(user.Type))
                {
                    var role = new IdentityRole();

                    role.Name = user.Type;
                    await _roleManager.CreateAsync(role);
                }

                var createdUser = _mapper.Map<User>(user);
                //createdUser.EmailConfirmed = true;
                var result = await _userManager.CreateAsync(createdUser, user.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(createdUser, user.Type);

                    return RedirectToAction("Index");
                }

                //_context.Add(user);
                //await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
            }

            if (user.Type == "Uczen" && user.ClassId == null)
            {
                var invalidProp = ModelState.Values.FirstOrDefault(x => x.ValidationState == ModelValidationState.Invalid);
                var error = invalidProp.Errors[0].ErrorMessage;

                ModelState.AddModelError("ClassId", error);
            }

            ViewData["ClassId"] = new SelectList(_context.Class, "ClassId", "Name", user.ClassId);
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["ClassId"] = new SelectList(_context.Class, "ClassId", "Name", user.ClassId);

            var editModel = _mapper.Map<UserEditViewModel>(user);
            return View(editModel);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, UserEditViewModel user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var userFromDb = await _userManager.FindByIdAsync(id);

                _mapper.Map(user, userFromDb);

                await _userManager.UpdateAsync(userFromDb);

                //try
                //{
                //    _context.Update(user);
                //    await _context.SaveChangesAsync();
                //}
                //catch (DbUpdateConcurrencyException)
                //{
                //    if (!UserExists(user.Id))
                //    {
                //        return NotFound();
                //    }
                //    else
                //    {
                //        throw;
                //    }
                //}
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClassId"] = new SelectList(_context.Class, "ClassId", "Name", user.ClassId);
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var deleteModel = _mapper.Map<UserDetailsViewModel>(user);
            return View(deleteModel);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            await _userManager.DeleteAsync(user);

            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
