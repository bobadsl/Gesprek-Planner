using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GesprekPlanner_WebApi.Data.Migrations
{
    public partial class AddedEmailGroupThings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailGroup",
                table: "ApplicationUserGroups",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsInMailGroup",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailGroup",
                table: "ApplicationUserGroups");

            migrationBuilder.DropColumn(
                name: "IsInMailGroup",
                table: "AspNetUsers");
        }
    }
}
