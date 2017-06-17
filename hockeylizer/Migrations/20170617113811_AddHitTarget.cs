using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace hockeylizer.Migrations
{
    public partial class AddHitTarget : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HitTarget",
                table: "Targets",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ManuallyAnalyzed",
                table: "Targets",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HitTarget",
                table: "Targets");

            migrationBuilder.DropColumn(
                name: "ManuallyAnalyzed",
                table: "Targets");
        }
    }
}
