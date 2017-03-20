using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GesprekPlanner_WebApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using GesprekPlanner_WebApi.Models;
/*
 * This controller will enable admins to fake them to be a teacher of a school
 * And schooladmins to fake a teacher
 */
namespace GesprekPlanner_WebApi.Controllers
{
    [Authorize(Roles = "Eigenaar, Schooladmin")]
    public class FakerController : Controller
    {
        private readonly UserManager<ApplicationUser> _manager;
        private readonly ApplicationDbContext _context;

        
        public FakerController(UserManager<ApplicationUser> manager, ApplicationDbContext context)
        {
            _manager = manager;
            _context = context;
        }

        public async Task<IActionResult> ChangeTeacher(string returnUrl, int group)
        {
            var user = await GetCurrentUser();
            user.Group = _context.ApplicationUserGroups.First(g => g.ApplicationUserGroupId == group);
            return Redirect(returnUrl);
        }

        private async Task<ApplicationUser> GetCurrentUser()
        {
            return await _manager.GetUserAsync(HttpContext.User);
        }
    }
}