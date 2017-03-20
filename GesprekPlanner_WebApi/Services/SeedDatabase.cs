using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GesprekPlanner_WebApi.Data;
using GesprekPlanner_WebApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.DependencyInjection;

namespace GesprekPlanner_WebApi.Services
{
    public class SeedDatabase
    {
        public static async void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<ApplicationDbContext>();
            
            if (!context.Roles.Any())
            {
                string[] roles = {"Eigenaar", "Schooladmin", "Leraar"};

                var roleStore = new RoleStore<IdentityRole>(context);
                foreach (string role in roles)
                {
                        IdentityRole identityRole = new IdentityRole {Name = role, NormalizedName = role.ToUpper()};
                        await roleStore.CreateAsync(identityRole);
                    }
                
            }
            if (!context.ApplicationUserGroups.Any())
            {
                string[] groups =
                {
                    "Groep 1/2a", "Groep 1/2b", "Groep 1/2c", "Groep 1/2d", "Groep 3a", "Groep 3b", "Groep 4a",
                    "Groep 4b", "Groep 5a", "Groep 5b", "Groep 6a", "Groep 6b", "Groep 7a", "Groep 7b", "Groep 8a",
                    "Groep 8b", "Directie"
                };
                foreach (string group in groups)
                {
                    context.ApplicationUserGroups.Add(new ApplicationUserGroup { GroupName = group });
                }
            }
            if (!context.ConversationTypes.Any())
            {
                context.ConversationTypes.Add(new ConversationType
                {
                    ConversationDuration = 15,
                    ConversationName = "Voortgangsgesprek"
                });
                context.ConversationTypes.Add(new ConversationType
                {
                    ConversationDuration = 10,
                    ConversationName = "Raportgesprek"
                });
            }
            if (!context.Users.Any())
            {
                var password = new PasswordHasher<ApplicationUser>();
                var userStore = new UserStore<ApplicationUser>(context);
                UserManager<ApplicationUser> _userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
                string hashed;
                var owner = new ApplicationUser
                {
                    Email = "admin@email.com",
                    UserName = "Admin",
                    SecurityStamp = Guid.NewGuid().ToString("D")
                };
                owner.NormalizedEmail = owner.Email.ToUpper();
                owner.NormalizedUserName = owner.UserName.ToUpper();
                hashed = password.HashPassword(owner, "Dalton");
                owner.PasswordHash = hashed;
                await userStore.CreateAsync(owner);
                await _userManager.AddToRoleAsync(owner, "Eigenaar");


                var schooladmin = new ApplicationUser
                {
                    Email = "schooladmin@email.com",
                    UserName = "SchoolAdmin",
                    SecurityStamp = Guid.NewGuid().ToString("D")
                };
                schooladmin.NormalizedEmail = schooladmin.Email.ToUpper();
                schooladmin.NormalizedUserName = schooladmin.UserName.ToUpper();
                hashed = password.HashPassword(schooladmin, "Dalton");
                schooladmin.PasswordHash = hashed;
                await userStore.CreateAsync(schooladmin);
                await _userManager.AddToRoleAsync(schooladmin, "Schooladmin");

                var teacher = new ApplicationUser
                {
                    Email = "leraar@email.com",
                    UserName = "Leraar",
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    Group = context.ApplicationUserGroups.First(g => g.GroupName == "Groep 6b") 
                };
                teacher.NormalizedEmail = teacher.Email.ToUpper();
                teacher.NormalizedUserName = teacher.UserName.ToUpper();
                hashed = password.HashPassword(teacher, "Dalton");
                teacher.PasswordHash = hashed;
                await userStore.CreateAsync(teacher);
                await _userManager.AddToRoleAsync(teacher, "Leraar");


            }

            await context.SaveChangesAsync();
        }

    }
}
