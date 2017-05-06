using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace hockeylizer.Data.Migrations
{
    public partial class nineteen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "XCoordinateAnalyzed",
                table: "Targets",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "YCoordinateAnalyzed",
                table: "Targets",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "XCoordinateAnalyzed",
                table: "Targets");

            migrationBuilder.DropColumn(
                name: "YCoordinateAnalyzed",
                table: "Targets");
        }
    }
}
