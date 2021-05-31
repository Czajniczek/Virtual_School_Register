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
using Virtual_School_Register.ViewModels.UsersController;

namespace Virtual_School_Register.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public UsersController(ApplicationDbContext context, UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        // GET: Users
        public IActionResult Index()
        {
            var users = _context.Users.Include(u => u.Class).Select(x => new UserIndexViewModel
            {
                Id = x.Id,
                Login = x.UserName,
                Name = x.Name,
                Surname = x.Surname,
                Email = x.Email,
                Parent = x.ParentId,
                Type = x.Type,
                Class = x.Class.Name
            })
              .OrderBy(x => x.Type)
              .ThenBy(x => x.Name.ToLower())
              .ThenBy(x => x.Surname.ToLower())
              .ThenBy(x => x.Name.ToLower())
              .ToList();

            users.ForEach(x =>
            {
                if (x.Parent != null)
                {
                    var parent = users.Find(y => y.Id == x.Parent);
                    x.Parent = parent.Name + " " + parent.Surname;
                }
            });

            return View(users);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["ClassId"] = new SelectList(_context.Class, "ClassId", "Name"); //Value = Class Id, Text = Name
                                                                                     //Value = 2, Text = 1A

            //TODO: Nie przekazywać wszystkich danych o rodzicach tylko ich id, imię i nazwisko
            var parents = _userManager.Users.Where(x => x.Type == "Rodzic").ToList();
            ViewBag.ParentsList = parents;

            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateViewModel user)
        {
            if (ModelState.IsValid)
            {
                if (!await _roleManager.RoleExistsAsync(user.Type))
                {
                    var role = new IdentityRole
                    {
                        Name = user.Type
                    };
                    await _roleManager.CreateAsync(role);
                }

                var createdUser = _mapper.Map<User>(user);
                createdUser.EmailConfirmed = true;

                if (user.Password != null)
                {
                    var result = await _userManager.CreateAsync(createdUser, user.Password);

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(createdUser, user.Type);

                        return RedirectToAction("Index");
                    }
                }
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

            if (user.Type == "Uczen")
            {
                var userClass = await _context.Class.FirstOrDefaultAsync(x => x.ClassId == user.ClassId);
                detailsModel.Class = userClass;

                if (user.ParentId != null)
                {
                    var parent = _userManager.Users.FirstOrDefault(x => x.Id == user.ParentId);
                    detailsModel.ParentId = parent.Name + " " + parent.Surname;
                }
            }

            return View(detailsModel);
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

            ViewData["ClassId"] = new SelectList(_context.Class, "ClassId", "Name", user.ClassId); //Value = Class Id, Text = Name
                                                                                                   //Value = 2, Text = 1A

            //TODO: Nie przekazywać wszystkich danych o rodzicach tylko ich id, imię i nazwisko
            var parents = _userManager.Users.Where(x => x.Type == "Rodzic").ToList();
            ViewBag.ParentsList = parents;

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
                if (!await _roleManager.RoleExistsAsync(user.Type))
                {
                    var role = new IdentityRole
                    {
                        Name = user.Type
                    };
                    await _roleManager.CreateAsync(role);
                }

                var userFromDb = await _userManager.FindByIdAsync(id);

                _mapper.Map(user, userFromDb);

                await _userManager.UpdateAsync(userFromDb);

                var token = await _userManager.GeneratePasswordResetTokenAsync(userFromDb);

                if (user.Password != null)
                {
                    var result = await _userManager.ResetPasswordAsync(userFromDb, token, user.Password);

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(userFromDb, user.Type);

                        return RedirectToAction(nameof(Index));
                    }
                }

                await _userManager.AddToRoleAsync(userFromDb, user.Type);

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

            if (user.Type == "Uczen")
            {
                var userClass = await _context.Class.FirstOrDefaultAsync(x => x.ClassId == user.ClassId);
                deleteModel.Class = userClass;

                if (user.ParentId != null)
                {
                    var parent = _userManager.Users.FirstOrDefault(x => x.Id == user.ParentId);
                    deleteModel.ParentId = parent.Name + " " + parent.Surname;
                }
            }

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
