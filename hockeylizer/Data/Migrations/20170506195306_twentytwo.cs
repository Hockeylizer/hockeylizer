using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace hockeylizer.Data.Migrations
{
    public partial class twentytwo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TimestampStart",
                table: "Targets",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "TimestampEnd",
                table: "Targets",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TimestampStart",
                table: "Targets",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TimestampEnd",
                table: "Targets",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
