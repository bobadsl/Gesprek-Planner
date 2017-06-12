using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GesprekPlanner_WebApi.Areas.Schooladmin.Models.UsersViewModels;
using GesprekPlanner_WebApi.Data;
using GesprekPlanner_WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GesprekPlanner_WebApi.Areas.SchoolAdmin.Controllers
{
    [Area("Schooladmin")]
    [Authorize(Roles ="Eigenaar, Schooladmin")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UsersController(ApplicationDbContext contect,
          UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _context = contect;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            List<MinimalUser> users;
                users = _context.Users.Include(u => u.Group).Where(u => u.School.Id == Guid.Parse(HttpContext.Session.GetString("School"))).Select(item => new MinimalUser
                {
                    Id = item.Id,
                    UserName = item.UserName,
                    Email = item.Email,
                    Group = item.Group.GroupName
                }).ToList();
            return View(users);
        }

        [HttpGet]
        public IActionResult Register()
        {
            var model = new RegisterNewUserViewModel();
            var groups = _context.ApplicationUserGroups.Where(
                u => u.School.Id == Guid.Parse(HttpContext.Session.GetString("School"))).Select(g => g.GroupName).ToList();
            var roles = _context.Roles.Where(r => r.Name == "Schooladmin" || r.Name == "Leraar");
            model.Roles = new SelectList(roles,"Name");
            model.Groups = JsonConvert.SerializeObject(groups);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterNewUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user;
                if (!_context.ApplicationUserGroups.Where(u => u.School.Id == Guid.Parse(HttpContext.Session.GetString("School"))).Any(g => g.GroupName == model.Group))
                {
                    _context.ApplicationUserGroups.Add(new ApplicationUserGroup { GroupName = model.Group });
                    await _context.SaveChangesAsync();
                }
                user = new ApplicationUser
                {
                    UserName = model.Username,
                    Email = model.Email,
                    Group = _context.ApplicationUserGroups.First(g => g.GroupName == model.Group),
                    School = _context.Schools.First(s => s.Id == Guid.Parse(HttpContext.Session.GetString("School")))
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    _userManager.AddToRoleAsync(user, model.RoleName).GetAwaiter().GetResult();
                }
                return RedirectToAction("Index");
            }
            List<ApplicationUserGroup> groups =
                    _context.ApplicationUserGroups.Where(
                        u => u.School.Id == Guid.Parse(HttpContext.Session.GetString("School"))).ToList();
            model.Groups = JsonConvert.SerializeObject(groups.Select(g => g.GroupName).ToList());
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            var query = from u in _context.Users select u;
            var user = query.AsEnumerable().Select(item => new MinimalUser
            {
                Id = item.Id,
                UserName = item.UserName,
                Email = item.Email,
                Group = item.Group.GroupName
            }).First(item => item.Id == id);

            var groups = _context.ApplicationUserGroups.ToList();
            user.Groups = groups;
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(MinimalUser user)
        {
            var dbUser = _context.Users.First(u => u.Id == user.Id);
            dbUser.Email = user.Email;
            dbUser.UserName = user.UserName;
            try
            {
                _context.Update(dbUser);
                _context.SaveChanges();
            }
            catch
            {
                return View("Error");
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult ResetPassword(string id)
        {
            var query = from u in _context.Users select u;
            var user = query.AsEnumerable().Select(item => new ResetPasswordViewModel()
            {
                Id = item.Id
            }).First(item => item.Id == id);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userManager.Users.First(u => u.Id == model.Id);
                if (user.Id == _userManager.GetUserId(User))
                {
                    return View("Error");
                }
                var resetToken = _userManager.GeneratePasswordResetTokenAsync(user).Result;
                var identityResult = _userManager.ResetPasswordAsync(user, resetToken, model.Password).Result;
                if (identityResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Error", identityResult);
                }
            }
            return View(model);
        }
    }
}
