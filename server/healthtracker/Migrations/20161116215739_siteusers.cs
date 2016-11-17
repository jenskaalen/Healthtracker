using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace healthtracker.Migrations
{
    public partial class siteusers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SiteUserId",
                table: "LogDays",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SiteUsers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Mail = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteUsers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LogDays_SiteUserId",
                table: "LogDays",
                column: "SiteUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_LogDays_SiteUsers_SiteUserId",
                table: "LogDays",
                column: "SiteUserId",
                principalTable: "SiteUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LogDays_SiteUsers_SiteUserId",
                table: "LogDays");

            migrationBuilder.DropTable(
                name: "SiteUsers");

            migrationBuilder.DropIndex(
                name: "IX_LogDays_SiteUserId",
                table: "LogDays");

            migrationBuilder.DropColumn(
                name: "SiteUserId",
                table: "LogDays");
        }
    }
}
