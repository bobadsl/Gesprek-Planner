using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GesprekPlanner_WebApi.Data.Migrations
{
    public partial class CreateConversationPlanDateClaimsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConversationPlanDates_ApplicationUserGroups_GroupId",
                table: "ConversationPlanDates");

            migrationBuilder.DropIndex(
                name: "IX_ConversationPlanDates_GroupId",
                table: "ConversationPlanDates");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "ConversationPlanDates");

            migrationBuilder.DropColumn(
                name: "PlanDateSet",
                table: "ConversationPlanDates");

            migrationBuilder.CreateTable(
                name: "ConversationPlanDateClaims",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ConversationPlanDateId = table.Column<int>(nullable: true),
                    GroupId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversationPlanDateClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConversationPlanDateClaims_ConversationPlanDates_ConversationPlanDateId",
                        column: x => x.ConversationPlanDateId,
                        principalTable: "ConversationPlanDates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConversationPlanDateClaims_ApplicationUserGroups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "ApplicationUserGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConversationPlanDateClaims_ConversationPlanDateId",
                table: "ConversationPlanDateClaims",
                column: "ConversationPlanDateId");

            migrationBuilder.CreateIndex(
                name: "IX_ConversationPlanDateClaims_GroupId",
                table: "ConversationPlanDateClaims",
                column: "GroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConversationPlanDateClaims");

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "ConversationPlanDates",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PlanDateSet",
                table: "ConversationPlanDates",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ConversationPlanDates_GroupId",
                table: "ConversationPlanDates",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConversationPlanDates_ApplicationUserGroups_GroupId",
                table: "ConversationPlanDates",
                column: "GroupId",
                principalTable: "ApplicationUserGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
