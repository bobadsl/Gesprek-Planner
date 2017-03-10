﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GesprekPlanner_WebApi.Areas.Admin.Models;
using GesprekPlanner_WebApi.Areas.Admin.Models.UsersViewModels;
using GesprekPlanner_WebApi.Data;
using GesprekPlanner_WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Remotion.Linq.Clauses;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GesprekPlanner_WebApi.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UsersController(ApplicationDbContext dbContect,
          UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager )
        {
            _userManager = userManager;
            _dbContext = dbContect;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            var query = from u in _dbContext.Users
                select u;
            var users = query.AsEnumerable().Select(item => new MinimalUser
                {
                    Id = item.Id,
                    UserName = item.UserName,
                    Email = item.Email,
                    GroupId = item.GroupId
                }).ToList();
            return View(users);
        }

        [HttpGet]
        public IActionResult Register()
        {
            var groups = _dbContext.ApplicationUserGroups.ToList();
            if (groups.Count == 0) return RedirectToAction("Create", "Groups");
            var model = new RegisterNewUserViewModel();
            model.Groups = new SelectList(groups, "ApplicationUserGroupId", "GroupName");
            return View(model);
        }

        [HttpPost]
        public IActionResult Register(RegisterNewUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Username, Email = model.Email, GroupId = model.GroupId};
                var result = _userManager.CreateAsync(user, model.Password).Result;
                if (result.Succeeded)
                {
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                    // Send an email with this link
                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var callbackUrl = Url.Action(nameof(ConfirmEmail), "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                    //await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
                    //    $"Please confirm your account by clicking this link: <a href='{callbackUrl}'>link</a>");
                }
                return RedirectToAction("Index");
            }
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
                GroupId = item.GroupId
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