using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using GesprekPlanner_WebApi.Areas.Admin.Models;
using GesprekPlanner_WebApi.Areas.Admin.Models.UsersViewModels;
using GesprekPlanner_WebApi.Data;
using GesprekPlanner_WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Remotion.Linq.Clauses;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GesprekPlanner_WebApi.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Eigenaar, Schooladmin")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UsersController(ApplicationDbContext dbContect,
          UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _dbContext = dbContect;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            
            var users = _dbContext.Users.Include(u => u.Group).Select(item => new MinimalUser
            {
                Id = item.Id,
                UserName = item.UserName,
                Email = item.Email,
                Group = item.Group
            }).ToList();
            return View(users);
        }

        [HttpGet]
        public IActionResult Register()
        {
            var groups = _dbContext.ApplicationUserGroups.ToList();
            if (groups.Count == 0) return RedirectToAction("Create", "Groups");
            var model = new RegisterNewUserViewModel();
            model.Roles = new SelectList(_dbContext.Roles.ToList(), "Name");
            model.Groups = JsonConvert.SerializeObject(groups.Select(g => g.GroupName).ToList());
            return View(model);
        }

        [HttpPost]
        public IActionResult Register(RegisterNewUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user;
                if (!_dbContext.ApplicationUserGroups.Any(g => g.GroupName == model.Group))
                {
                    var AddedGroup = _dbContext.ApplicationUserGroups.Add(new ApplicationUserGroup { GroupName = model.Group });
                    user = new ApplicationUser { UserName = model.Username, Email = model.Email, Group = AddedGroup.Entity };
                    _dbContext.SaveChanges();
                }
                user = new ApplicationUser { UserName = model.Username, Email = model.Email, Group = _dbContext.ApplicationUserGroups.First(g => g.GroupName == model.Group) };
                var result = _userManager.CreateAsync(user, model.Password).Result;
                if (result.Succeeded)
                {
                    _userManager.AddToRoleAsync(user, model.RoleName).GetAwaiter().GetResult();
                }
                return RedirectToAction("Index");
            }
            var groups = _dbContext.ApplicationUserGroups.ToList();
            model.Roles = new SelectList(_dbContext.Roles.ToList(), "Name");
            model.Groups = JsonConvert.SerializeObject(groups.Select(g => g.GroupName).ToList());
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            var query = from u in _dbContext.Users select u;
            var user = query.AsEnumerable().Select(item => new MinimalUser
            {
                Id = item.Id,
                UserName = item.UserName,
                Email = item.Email,
                Group = item.Group
            }).First(item => item.Id == id);

            var groups = _dbContext.ApplicationUserGroups.ToList();
            user.Groups = groups;
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(MinimalUser user)
        {
            var dbUser = _dbContext.Users.First(u => u.Id == user.Id);
            dbUser.Email = user.Email;
            dbUser.UserName = user.UserName;
            try
            {
                _dbContext.Update(dbUser);
                _dbContext.SaveChanges();
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
            var query = from u in _dbContext.Users select u;
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
