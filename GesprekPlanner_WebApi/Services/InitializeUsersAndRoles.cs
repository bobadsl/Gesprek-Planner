using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GesprekPlanner_WebApi.Data;
using GesprekPlanner_WebApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GesprekPlanner_WebApi.Services
{
    public class InitializeUsersAndRoles
    {
        public static async void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<ApplicationDbContext>();

            string[] roles = {"Owner", "Administrator", "Leraar" };

            var roleStore = new RoleStore<IdentityRole>(context);
            foreach (string role in roles)
            {
                if (!context.Roles.Any(r => r.Name == role))
                {
                    IdentityRole identityRole = new IdentityRole {Name = role, NormalizedName = role.ToUpper()};
                    await roleStore.CreateAsync(identityRole);
                }
            }


            var user = new ApplicationUser
            {
                Email = "ict02@meesteraafjesschool.nl",
                NormalizedEmail = "ICT02@MEESTERAAFJESSCHOOL.NL",
                UserName = "Admin",
                NormalizedUserName = "ADMIN",
                SecurityStamp = Guid.NewGuid().ToString("D")
            };


            if (!context.Users.Any(u => u.UserName == user.UserName))
            {
                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(user, "Dalton");
                user.PasswordHash = hashed;

                var userStore = new UserStore<ApplicationUser>(context);
                await userStore.CreateAsync(user);


                UserManager<ApplicationUser> _userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
                await _userManager.AddToRoleAsync(user, "Administrator");
            }

            await context.SaveChangesAsync();
        }

    }
}
