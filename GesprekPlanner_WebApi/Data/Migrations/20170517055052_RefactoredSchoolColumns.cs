using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GesprekPlanner_WebApi.Data.Migrations
{
    public partial class RefactoredSchoolColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SchoolUrl",
                table: "Schools",
                newName: "Url");

            migrationBuilder.RenameColumn(
                name: "SchoolTelephone",
                table: "Schools",
                newName: "Telephone");

            migrationBuilder.RenameColumn(
                name: "SchoolStreet",
                table: "Schools",
                newName: "Street");

            migrationBuilder.RenameColumn(
                name: "SchoolPostCode",
                table: "Schools",
                newName: "PostCode");

            migrationBuilder.RenameColumn(
                name: "SchoolName",
                table: "Schools",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "SchoolLogo",
                table: "Schools",
                newName: "Logo");

            migrationBuilder.RenameColumn(
                name: "SchoolEmail",
                table: "Schools",
                newName: "Email");

            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Url",
                table: "Schools",
                newName: "SchoolUrl");

            migrationBuilder.RenameColumn(
                name: "Telephone",
                table: "Schools",
                newName: "SchoolTelephone");

            migrationBuilder.RenameColumn(
                name: "Street",
                table: "Schools",
                newName: "SchoolStreet");

            migrationBuilder.RenameColumn(
                name: "PostCode",
                table: "Schools",
                newName: "SchoolPostCode");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Schools",
                newName: "SchoolName");

            migrationBuilder.RenameColumn(
                name: "Logo",
                table: "Schools",
                newName: "SchoolLogo");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Schools",
                newName: "SchoolEmail");
        }
    }
}
