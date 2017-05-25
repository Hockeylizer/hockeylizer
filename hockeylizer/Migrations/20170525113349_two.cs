using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace hockeylizer.Migrations
{
    public partial class two : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "YCoordinateAnalyzed",
                table: "Targets",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "YCoordinate",
                table: "Targets",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "XCoordinateAnalyzed",
                table: "Targets",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "XCoordinate",
                table: "Targets",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "XOffset",
                table: "Targets",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "YOffset",
                table: "Targets",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "XOffset",
                table: "Targets");

            migrationBuilder.DropColumn(
                name: "YOffset",
                table: "Targets");

            migrationBuilder.AlterColumn<int>(
                name: "YCoordinateAnalyzed",
                table: "Targets",
                nullable: true,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "YCoordinate",
                table: "Targets",
                nullable: true,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "XCoordinateAnalyzed",
                table: "Targets",
                nullable: true,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "XCoordinate",
                table: "Targets",
                nullable: true,
                oldClrType: typeof(double),
                oldNullable: true);
        }
    }
}
