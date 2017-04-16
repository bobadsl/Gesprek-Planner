using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GesprekPlanner_WebApi.Data.Migrations
{
    public partial class UpdatedpackagesCreateConversationTypeClaim : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUserRoles_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

            migrationBuilder.CreateTable(
                name: "ConversationTypeClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ConversationTypeId = table.Column<int>(nullable: true),
                    GroupApplicationUserGroupId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversationTypeClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConversationTypeClaims_ConversationTypes_ConversationTypeId",
                        column: x => x.ConversationTypeId,
                        principalTable: "ConversationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConversationTypeClaims_ApplicationUserGroups_GroupApplicationUserGroupId",
                        column: x => x.GroupApplicationUserGroupId,
                        principalTable: "ApplicationUserGroups",
                        principalColumn: "ApplicationUserGroupId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConversationTypeClaims_ConversationTypeId",
                table: "ConversationTypeClaims",
                column: "ConversationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ConversationTypeClaims_GroupApplicationUserGroupId",
                table: "ConversationTypeClaims",
                column: "GroupApplicationUserGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConversationTypeClaims");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_UserId",
                table: "AspNetUserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName");
        }
    }
}
