using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using GesprekPlanner_WebApi.Data;

namespace GesprekPlanner_WebApi.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20170502093517_AddSchoolTableAndRelatedLinks")]
    partial class AddSchoolTableAndRelatedLinks
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GesprekPlanner_WebApi.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<int?>("GroupApplicationUserGroupId");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<Guid?>("SchoolId");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("GroupApplicationUserGroupId");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.HasIndex("SchoolId");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("GesprekPlanner_WebApi.Models.ApplicationUserGroup", b =>
                {
                    b.Property<int>("ApplicationUserGroupId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("GroupName")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<Guid?>("SchoolId");

                    b.HasKey("ApplicationUserGroupId");

                    b.HasIndex("SchoolId");

                    b.ToTable("ApplicationUserGroups");
                });

            modelBuilder.Entity("GesprekPlanner_WebApi.Models.Conversation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ConversationTypeId");

                    b.Property<DateTime>("DateTime");

                    b.Property<DateTime>("EndTime");

                    b.Property<int?>("GroupApplicationUserGroupId");

                    b.Property<bool>("IsChosen");

                    b.Property<Guid?>("SchoolId");

                    b.HasKey("Id");

                    b.HasIndex("ConversationTypeId");

                    b.HasIndex("GroupApplicationUserGroupId");

                    b.HasIndex("SchoolId");

                    b.ToTable("Conversations");
                });

            modelBuilder.Entity("GesprekPlanner_WebApi.Models.ConversationPlanDate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("EndDate");

                    b.Property<Guid?>("SchoolId");

                    b.Property<DateTime>("StartDate");

                    b.HasKey("Id");

                    b.HasIndex("SchoolId");

                    b.ToTable("ConversationPlanDates");
                });

            modelBuilder.Entity("GesprekPlanner_WebApi.Models.ConversationPlanDateClaim", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ConversationPlanDateId");

                    b.Property<int?>("GroupApplicationUserGroupId");

                    b.HasKey("Id");

                    b.HasIndex("ConversationPlanDateId");

                    b.HasIndex("GroupApplicationUserGroupId");

                    b.ToTable("ConversationPlanDateClaims");
                });

            modelBuilder.Entity("GesprekPlanner_WebApi.Models.ConversationType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ConversationDuration");

                    b.Property<string>("ConversationName")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<Guid?>("SchoolId");

                    b.HasKey("Id");

                    b.HasIndex("SchoolId");

                    b.ToTable("ConversationTypes");
                });

            modelBuilder.Entity("GesprekPlanner_WebApi.Models.ConversationTypeClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ConversationTypeId");

                    b.Property<int?>("GroupApplicationUserGroupId");

                    b.HasKey("Id");

                    b.HasIndex("ConversationTypeId");

                    b.HasIndex("GroupApplicationUserGroupId");

                    b.ToTable("ConversationTypeClaims");
                });

            modelBuilder.Entity("GesprekPlanner_WebApi.Models.School", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("SchoolEmail");

                    b.Property<string>("SchoolLogo");

                    b.Property<string>("SchoolName");

                    b.Property<string>("SchoolPostCode");

                    b.Property<string>("SchoolStreet");

                    b.Property<string>("SchoolTelephone");

                    b.Property<string>("SchoolUrl");

                    b.HasKey("Id");

                    b.ToTable("Schools");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("GesprekPlanner_WebApi.Models.ApplicationUser", b =>
                {
                    b.HasOne("GesprekPlanner_WebApi.Models.ApplicationUserGroup", "Group")
                        .WithMany()
                        .HasForeignKey("GroupApplicationUserGroupId");

                    b.HasOne("GesprekPlanner_WebApi.Models.School", "School")
                        .WithMany()
                        .HasForeignKey("SchoolId");
                });

            modelBuilder.Entity("GesprekPlanner_WebApi.Models.ApplicationUserGroup", b =>
                {
                    b.HasOne("GesprekPlanner_WebApi.Models.School", "School")
                        .WithMany()
                        .HasForeignKey("SchoolId");
                });

            modelBuilder.Entity("GesprekPlanner_WebApi.Models.Conversation", b =>
                {
                    b.HasOne("GesprekPlanner_WebApi.Models.ConversationType", "ConversationType")
                        .WithMany()
                        .HasForeignKey("ConversationTypeId");

                    b.HasOne("GesprekPlanner_WebApi.Models.ApplicationUserGroup", "Group")
                        .WithMany()
                        .HasForeignKey("GroupApplicationUserGroupId");

                    b.HasOne("GesprekPlanner_WebApi.Models.School", "School")
                        .WithMany()
                        .HasForeignKey("SchoolId");
                });

            modelBuilder.Entity("GesprekPlanner_WebApi.Models.ConversationPlanDate", b =>
                {
                    b.HasOne("GesprekPlanner_WebApi.Models.School", "School")
                        .WithMany()
                        .HasForeignKey("SchoolId");
                });

            modelBuilder.Entity("GesprekPlanner_WebApi.Models.ConversationPlanDateClaim", b =>
                {
                    b.HasOne("GesprekPlanner_WebApi.Models.ConversationPlanDate", "ConversationPlanDate")
                        .WithMany()
                        .HasForeignKey("ConversationPlanDateId");

                    b.HasOne("GesprekPlanner_WebApi.Models.ApplicationUserGroup", "Group")
                        .WithMany()
                        .HasForeignKey("GroupApplicationUserGroupId");
                });

            modelBuilder.Entity("GesprekPlanner_WebApi.Models.ConversationType", b =>
                {
                    b.HasOne("GesprekPlanner_WebApi.Models.School", "School")
                        .WithMany()
                        .HasForeignKey("SchoolId");
                });

            modelBuilder.Entity("GesprekPlanner_WebApi.Models.ConversationTypeClaim", b =>
                {
                    b.HasOne("GesprekPlanner_WebApi.Models.ConversationType", "ConversationType")
                        .WithMany()
                        .HasForeignKey("ConversationTypeId");

                    b.HasOne("GesprekPlanner_WebApi.Models.ApplicationUserGroup", "Group")
                        .WithMany()
                        .HasForeignKey("GroupApplicationUserGroupId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("GesprekPlanner_WebApi.Models.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("GesprekPlanner_WebApi.Models.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GesprekPlanner_WebApi.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
