using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GesprekPlanner_WebApi.Data.Migrations
{
    public partial class AddGroupColumnToApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupApplicationUserGroupId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_GroupApplicationUserGroupId",
                table: "AspNetUsers",
                column: "GroupApplicationUserGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ApplicationUserGroups_GroupApplicationUserGroupId",
                table: "AspNetUsers",
                column: "GroupApplicationUserGroupId",
                principalTable: "ApplicationUserGroups",
                principalColumn: "ApplicationUserGroupId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ApplicationUserGroups_GroupApplicationUserGroupId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_GroupApplicationUserGroupId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "GroupApplicationUserGroupId",
                table: "AspNetUsers");
        }
    }
}
