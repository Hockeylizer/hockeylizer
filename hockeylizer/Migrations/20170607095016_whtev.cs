using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace hockeylizer.Migrations
{
    public partial class whtev : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AnalysisFailed",
                table: "Targets",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "AnalysisFailedReason",
                table: "Targets",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ChopFailed",
                table: "Targets",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ChopFailedReason",
                table: "Targets",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DeleteFailed",
                table: "Sessions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DeleteFailedWhere",
                table: "Sessions",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SomethingFailed",
                table: "Sessions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "WhySomethingFailed",
                table: "Sessions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnalysisFailed",
                table: "Targets");

            migrationBuilder.DropColumn(
                name: "AnalysisFailedReason",
                table: "Targets");

            migrationBuilder.DropColumn(
                name: "ChopFailed",
                table: "Targets");

            migrationBuilder.DropColumn(
                name: "ChopFailedReason",
                table: "Targets");

            migrationBuilder.DropColumn(
                name: "DeleteFailed",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "DeleteFailedWhere",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "SomethingFailed",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "WhySomethingFailed",
                table: "Sessions");
        }
    }
}
