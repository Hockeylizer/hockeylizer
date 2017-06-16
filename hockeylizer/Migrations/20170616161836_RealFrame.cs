using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace hockeylizer.Migrations
{
    public partial class RealFrame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnalisysFailReason",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "SomethingFailed",
                table: "Sessions");

            migrationBuilder.RenameColumn(
                name: "WhySomethingFailed",
                table: "Sessions",
                newName: "AnalysisFailReason");

            migrationBuilder.AddColumn<int>(
                name: "RealFrameHit",
                table: "Targets",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RealFrameHit",
                table: "Targets");

            migrationBuilder.RenameColumn(
                name: "AnalysisFailReason",
                table: "Sessions",
                newName: "WhySomethingFailed");

            migrationBuilder.AddColumn<string>(
                name: "AnalisysFailReason",
                table: "Sessions",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SomethingFailed",
                table: "Sessions",
                nullable: false,
                defaultValue: false);
        }
    }
}
