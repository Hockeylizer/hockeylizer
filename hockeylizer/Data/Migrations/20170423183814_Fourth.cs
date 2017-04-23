using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace hockeylizer.Data.Migrations
{
    public partial class Fourth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Start",
                table: "Timestamps",
                newName: "start");

            migrationBuilder.RenameColumn(
                name: "End",
                table: "Timestamps",
                newName: "end");

            migrationBuilder.CreateTable(
                name: "TargetCoordinates",
                columns: table => new
                {
                    CoordinateId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    VideoId = table.Column<int>(nullable: false),
                    xCoord = table.Column<int>(nullable: true),
                    yCoord = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TargetCoordinates", x => x.CoordinateId);
                    table.ForeignKey(
                        name: "FK_TargetCoordinates_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "VideoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TargetCoordinates_VideoId",
                table: "TargetCoordinates",
                column: "VideoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TargetCoordinates");

            migrationBuilder.RenameColumn(
                name: "start",
                table: "Timestamps",
                newName: "Start");

            migrationBuilder.RenameColumn(
                name: "end",
                table: "Timestamps",
                newName: "End");
        }
    }
}
