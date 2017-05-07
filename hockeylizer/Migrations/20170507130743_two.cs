using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace hockeylizer.Migrations
{
    public partial class two : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "Chopped",
            //    table: "Sessions");

            migrationBuilder.DropColumn(
                name: "test",
                table: "Sessions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<bool>(
            //    name: "Chopped",
            //    table: "Sessions",
            //    nullable: false,
            //    defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "test",
                table: "Sessions",
                nullable: true);
        }
    }
}
