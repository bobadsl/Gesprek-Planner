using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using GesprekPlanner_WebApi.Areas.Admin.Models;
using GesprekPlanner_WebApi.Areas.Admin.Models.ConversationTypeViewModels;
using GesprekPlanner_WebApi.Areas.Admin.Models.UsersViewModels;
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

namespace GesprekPlanner_WebApi.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Eigenaar")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(ApplicationDbContext dbContect,
          UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = dbContect;
        }
        public IActionResult Index()
        {
            var users = _context.Users.Include(u => u.Group).Include(u => u.School).Select(item => new MinimalUser
            {
                Id = item.Id,
                UserName = item.UserName,
                Email = item.Email,
                Group = item.Group.GroupName,
                School = item.School.Name
            }).GroupBy(u => u.School).ToList();
            var userList = new List<List<MinimalUser>>();
            foreach (var user in users)
            {
                var tempList = user.ToList().Select(u => new MinimalUser()
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    Group = u.Group,
                    School = u.School
                }).ToList();
                userList.Add(tempList);
            }
            return View(userList);
        }

        [HttpGet]
        public IActionResult Register()
        {
            var schools = _context.Schools.ToList();
            var model = new RegisterNewUserViewModel
            {
                Schools = new List<SelectListItem>(),
                Roles = new SelectList(_context.Roles.ToList(), "Name")
            };
            ((List<SelectListItem>)model.Schools).Add(new SelectListItem{Disabled = true, Selected = true, Text = "Selecteer een school"});
            foreach (var school in schools)
            {
                ((List<SelectListItem>)model.Schools).Add(new SelectListItem {Text = school.Name, Value=school.Id.ToString()});
            }
            if (!_context.ApplicationUserGroups.Any()) return RedirectToAction("Create", "Groups");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterNewUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Username,
                    Email = model.Email,
                    Group = _context.ApplicationUserGroups.First(g => g.GroupName == model.Group),
                    School = _context.Schools.First(s => s.Name == model.SchoolName),
                    IsInMailGroup = model.IsInMailGroup
                };

                var result =  await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                   await _userManager.AddToRoleAsync(user, model.RoleName);
                }
                return RedirectToAction("Index");
            }
            var schools = _context.Schools.ToList();

            model.Schools = new List<SelectListItem>();
            model.Roles = new SelectList(_context.Roles.ToList(), "Name");
            ((List<SelectListItem>)model.Schools).Add(new SelectListItem { Disabled = true, Selected = true, Text = "Selecteer een school" });
            foreach (var school in schools)
            {
                ((List<SelectListItem>)model.Schools).Add(new SelectListItem { Text = school.Name, Value = school.Id.ToString() });
            }
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
                Group = item.Group.GroupName,
                IsInMailGroup = item.IsInMailGroup
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

        [HttpPost]
        public string GetGroupsForSchool([FromBody]dynamic json)
        {
            Guid school = Guid.Parse(json.school.ToString());
            return
                JsonConvert.SerializeObject(
                    _context.ApplicationUserGroups.Include(g => g.School)
                        .Where(g => g.School.Id == school)
                        .Select(g => g.GroupName).ToList());

        }

    }
}
