using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GesprekPlanner_WebApi.Data.Migrations
{
    public partial class AddSchoolTableAndRelatedLinks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "ConversationTypes",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "ConversationPlanDates",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "Conversations",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "ApplicationUserGroups",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Schools",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SchoolEmail = table.Column<string>(nullable: true),
                    SchoolLogo = table.Column<string>(nullable: true),
                    SchoolName = table.Column<string>(nullable: true),
                    SchoolPostCode = table.Column<string>(nullable: true),
                    SchoolStreet = table.Column<string>(nullable: true),
                    SchoolTelephone = table.Column<string>(nullable: true),
                    SchoolUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schools", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConversationTypes_SchoolId",
                table: "ConversationTypes",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ConversationPlanDates_SchoolId",
                table: "ConversationPlanDates",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_SchoolId",
                table: "Conversations",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserGroups_SchoolId",
                table: "ApplicationUserGroups",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SchoolId",
                table: "AspNetUsers",
                column: "SchoolId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Schools_SchoolId",
                table: "AspNetUsers",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserGroups_Schools_SchoolId",
                table: "ApplicationUserGroups",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Conversations_Schools_SchoolId",
                table: "Conversations",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ConversationPlanDates_Schools_SchoolId",
                table: "ConversationPlanDates",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ConversationTypes_Schools_SchoolId",
                table: "ConversationTypes",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Schools_SchoolId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserGroups_Schools_SchoolId",
                table: "ApplicationUserGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_Conversations_Schools_SchoolId",
                table: "Conversations");

            migrationBuilder.DropForeignKey(
                name: "FK_ConversationPlanDates_Schools_SchoolId",
                table: "ConversationPlanDates");

            migrationBuilder.DropForeignKey(
                name: "FK_ConversationTypes_Schools_SchoolId",
                table: "ConversationTypes");

            migrationBuilder.DropTable(
                name: "Schools");

            migrationBuilder.DropIndex(
                name: "IX_ConversationTypes_SchoolId",
                table: "ConversationTypes");

            migrationBuilder.DropIndex(
                name: "IX_ConversationPlanDates_SchoolId",
                table: "ConversationPlanDates");

            migrationBuilder.DropIndex(
                name: "IX_Conversations_SchoolId",
                table: "Conversations");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUserGroups_SchoolId",
                table: "ApplicationUserGroups");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_SchoolId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "ConversationTypes");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "ConversationPlanDates");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "ApplicationUserGroups");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "AspNetUsers");
        }
    }
}
