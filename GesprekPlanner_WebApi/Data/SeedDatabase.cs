using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GesprekPlanner_WebApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GesprekPlanner_WebApi.Data
{
    public class SeedDatabase
    {
        public static async void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<ApplicationDbContext>();
            context.Database.Migrate();
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
            if (!context.Schools.Any())
            {
                School maSchool = new School
                {
                    Id = Guid.NewGuid(),
                    Name = "Ods Meester Aafjes",
                    Email = "info@meesteraafjesschool.nl",
                    PostCode = "4194 RR Meteren",
                    Street = "J.H. Lievense van Herwaardenstraat 2",
                    Telephone = "0345 581 158",
                    Url = "http://odsmeesteraafjes.nl"
                };
                School oeSchool = new School
                {
                    Id = Guid.NewGuid(),
                    Name = "Obs Est",
                    Email = "info@obsest.nl",
                    PostCode = "4185 NA EST",
                    Street = "Dorpsstraat 3",
                    Telephone = "0345-569481",
                    Url = "http://www.obsest.nl"
                };
                context.Schools.Add(maSchool);
                context.Schools.Add(oeSchool);
                context.SaveChanges();
            }
            var schools = context.Schools.ToList();
            if (!context.ApplicationUserGroups.Any())
            {
                string[] maGroups =
                {
                    "Groep 1/2a", "Groep 1/2b", "Groep 1/2c", "Groep 1/2d", "Groep 3a", "Groep 3b", "Groep 4a",
                    "Groep 4b", "Groep 5a", "Groep 5b", "Groep 6a", "Groep 6b", "Groep 7a", "Groep 7b", "Groep 8a",
                    "Groep 8b", "Directie"
                };
                string[] oeGroups =
                {
                    "Groep 1-2-3", "Groep 4-5-6", "Groep 7-8"
                };
                foreach (string group in maGroups)
                {
                    context.ApplicationUserGroups.Add(new ApplicationUserGroup {GroupName = group, School = schools.First(s => s.Name == "Ods Meester Aafjes") });
                    context.SaveChanges();
                }
                foreach (string group in oeGroups)
                {
                    context.ApplicationUserGroups.Add(new ApplicationUserGroup {GroupName = group, School = schools.First(s => s.Name == "Obs Est") });
                    context.SaveChanges();
                }
                context.SaveChanges();
            }
            if (!context.ConversationTypes.Any())
            {
                var con1 = new ConversationType
                {
                    ConversationDuration = 15,
                    ConversationName = "Voortgangsgesprek",
                    School = schools.First(s => s.Name == "Ods Meester Aafjes")
                };
                var con2 = new ConversationType
                {
                    ConversationDuration = 10,
                    ConversationName = "Rapportgesprek",
                    School = schools.First(s => s.Name == "Ods Meester Aafjes")
                };
                context.ConversationTypes.Add(con1);
                context.ConversationTypes.Add(con2);
                context.SaveChanges();
                var groupList1 = new List<ApplicationUserGroup>
                {
                    context.ApplicationUserGroups.First(g=>g.Id == 5),
                    context.ApplicationUserGroups.First(g=>g.Id == 2),
                    context.ApplicationUserGroups.First(g=>g.Id == 7)
                };
                var groupList2 = new List<ApplicationUserGroup>
                {
                    context.ApplicationUserGroups.First(g=>g.Id == 15),
                    context.ApplicationUserGroups.First(g=>g.Id == 13),
                    context.ApplicationUserGroups.First(g=>g.Id == 9),
                    context.ApplicationUserGroups.First(g=>g.Id == 4)
                };
                foreach (var groupList in groupList1)
                {
                    context.ConversationTypeClaims.Add(new ConversationTypeClaim {ConversationType = con1, Group = groupList});
                }
                foreach (var groupList in groupList2)
                {
                    context.ConversationTypeClaims.Add(new ConversationTypeClaim {ConversationType = con2, Group = groupList});
                }
            }
            if (!context.ConversationPlanDates.Any())
            {
                DateTime[] startDates =
                {
                    new DateTime(2017, 5, 28),
                    new DateTime(2017, 6, 2),
                    new DateTime(2017, 6, 7),
                    new DateTime(2017, 6, 20)
                };
                DateTime[] endDates =
                {
                    new DateTime(2017, 6, 8),
                    new DateTime(2017, 6, 19),
                    new DateTime(2017, 6, 29),
                    new DateTime(2017, 7, 5)
                };
                var groupList = new List<List<ApplicationUserGroup>>();
                groupList.Add(new List<ApplicationUserGroup>
                {
                    context.ApplicationUserGroups.First(g=>g.Id == 4),
                    context.ApplicationUserGroups.First(g=>g.Id == 7)
                });
                groupList.Add(new List<ApplicationUserGroup>
                {
                    context.ApplicationUserGroups.First(g=>g.Id == 5),
                    context.ApplicationUserGroups.First(g=>g.Id == 2),
                    context.ApplicationUserGroups.First(g=>g.Id == 7)
                });
                groupList.Add(new List<ApplicationUserGroup>
                {
                    context.ApplicationUserGroups.First(g=>g.Id == 15),
                    context.ApplicationUserGroups.First(g=>g.Id == 13),
                    context.ApplicationUserGroups.First(g=>g.Id == 9),
                    context.ApplicationUserGroups.First(g=>g.Id == 4)
                });
                groupList.Add(new List<ApplicationUserGroup>
                {
                    context.ApplicationUserGroups.First(g=>g.Id == 9),
                    context.ApplicationUserGroups.First(g=>g.Id == 8)
                });

                for (int i = 0; i < startDates.Length; i++)
                {
                    var planDate = new ConversationPlanDate
                    {
                        StartDate = startDates[i],
                        EndDate = endDates[i],
                        School = schools.First(s => s.Name == "Ods Meester Aafjes")
                    };
                    context.ConversationPlanDates.Add(planDate);
                    foreach (var group in groupList[i])
                    {
                        context.ConversationPlanDateClaims.Add(new ConversationPlanDateClaim
                        {
                            ConversationPlanDate = planDate,
                            Group = group
                        });
                    }
                }
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


                var schoolAdminMA = new ApplicationUser
                {
                    Email = "schooladmin@email.com",
                    UserName = "SchoolAdminMa",
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    School = schools.First(s => s.Name == "Ods Meester Aafjes")
                };
                schoolAdminMA.NormalizedEmail = schoolAdminMA.Email.ToUpper();
                schoolAdminMA.NormalizedUserName = schoolAdminMA.UserName.ToUpper();
                hashed = password.HashPassword(schoolAdminMA, "Dalton");
                schoolAdminMA.PasswordHash = hashed;
                await userStore.CreateAsync(schoolAdminMA);
                await _userManager.AddToRoleAsync(schoolAdminMA, "Schooladmin");

                var schoolAdminOE = new ApplicationUser
                {
                    Email = "schooladmin@email.com",
                    UserName = "SchoolAdminOe",
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    School = schools.First(s => s.Name == "Obs Est")
                };
                schoolAdminOE.NormalizedEmail = schoolAdminOE.Email.ToUpper();
                schoolAdminOE.NormalizedUserName = schoolAdminOE.UserName.ToUpper();
                hashed = password.HashPassword(schoolAdminOE, "Dalton");
                schoolAdminOE.PasswordHash = hashed;
                await userStore.CreateAsync(schoolAdminOE);
                await _userManager.AddToRoleAsync(schoolAdminOE, "Schooladmin");

                var teacher = new ApplicationUser
                {
                    Email = "leraar@email.com",
                    UserName = "Leraar6b",
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    Group = context.ApplicationUserGroups.First(g => g.GroupName == "Groep 6b"),
                    School = schools.First(s => s.Name == "Ods Meester Aafjes")
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
