using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GesprekPlanner_WebApi.Data.Migrations
{
    public partial class CreateConversationPlanDateTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConversationPlanDates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EndDate = table.Column<DateTime>(nullable: false),
                    GroupApplicationUserGroupId = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversationPlanDates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConversationPlanDates_ApplicationUserGroups_GroupApplicationUserGroupId",
                        column: x => x.GroupApplicationUserGroupId,
                        principalTable: "ApplicationUserGroups",
                        principalColumn: "ApplicationUserGroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConversationPlanDates_GroupApplicationUserGroupId",
                table: "ConversationPlanDates",
                column: "GroupApplicationUserGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConversationPlanDates");
        }
    }
}
