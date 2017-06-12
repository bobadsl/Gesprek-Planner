using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GesprekPlanner_WebApi.Data.Migrations
{
    public partial class CreateConversationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Conversations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ConversationTypeId = table.Column<int>(nullable: true),
                    DateTime = table.Column<DateTime>(nullable: false),
                    GroupId = table.Column<int>(nullable: true),
                    IsChosen = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Conversations_ConversationTypes_ConversationTypeId",
                        column: x => x.ConversationTypeId,
                        principalTable: "ConversationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Conversations_ApplicationUserGroups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "ApplicationUserGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_ConversationTypeId",
                table: "Conversations",
                column: "ConversationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_GroupId",
                table: "Conversations",
                column: "GroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Conversations");
        }
    }
}
