using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace hockeylizer.Migrations
{
    public partial class latestest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AnalisysFailReason",
                table: "Sessions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChopFailReason",
                table: "Sessions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnalisysFailReason",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "ChopFailReason",
                table: "Sessions");
        }
    }
}
