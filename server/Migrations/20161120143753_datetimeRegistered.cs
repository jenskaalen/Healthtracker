using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace healthtracker.Migrations
{
    public partial class datetimeRegistered : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Registered",
                table: "LogDays",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Registered",
                table: "LogDays");
        }
    }
}
