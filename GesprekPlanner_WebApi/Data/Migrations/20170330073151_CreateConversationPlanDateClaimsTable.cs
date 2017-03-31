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
                name: "FK_ConversationPlanDates_ApplicationUserGroups_GroupApplicationUserGroupId",
                table: "ConversationPlanDates");

            migrationBuilder.DropIndex(
                name: "IX_ConversationPlanDates_GroupApplicationUserGroupId",
                table: "ConversationPlanDates");

            migrationBuilder.DropColumn(
                name: "GroupApplicationUserGroupId",
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
                    GroupApplicationUserGroupId = table.Column<int>(nullable: true)
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
                        name: "FK_ConversationPlanDateClaims_ApplicationUserGroups_GroupApplicationUserGroupId",
                        column: x => x.GroupApplicationUserGroupId,
                        principalTable: "ApplicationUserGroups",
                        principalColumn: "ApplicationUserGroupId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConversationPlanDateClaims_ConversationPlanDateId",
                table: "ConversationPlanDateClaims",
                column: "ConversationPlanDateId");

            migrationBuilder.CreateIndex(
                name: "IX_ConversationPlanDateClaims_GroupApplicationUserGroupId",
                table: "ConversationPlanDateClaims",
                column: "GroupApplicationUserGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConversationPlanDateClaims");

            migrationBuilder.AddColumn<int>(
                name: "GroupApplicationUserGroupId",
                table: "ConversationPlanDates",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PlanDateSet",
                table: "ConversationPlanDates",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ConversationPlanDates_GroupApplicationUserGroupId",
                table: "ConversationPlanDates",
                column: "GroupApplicationUserGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConversationPlanDates_ApplicationUserGroups_GroupApplicationUserGroupId",
                table: "ConversationPlanDates",
                column: "GroupApplicationUserGroupId",
                principalTable: "ApplicationUserGroups",
                principalColumn: "ApplicationUserGroupId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
