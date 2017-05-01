using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace hockeylizer.Data.Migrations
{
    public partial class twelve : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Timestamps");

            migrationBuilder.DropTable(
                name: "TargetCoordinates");

            migrationBuilder.AddColumn<long>(
                name: "TimestampEnd",
                table: "Targets",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TimestampStart",
                table: "Targets",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "XCoordinate",
                table: "Targets",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "YCoordinate",
                table: "Targets",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimestampEnd",
                table: "Targets");

            migrationBuilder.DropColumn(
                name: "TimestampStart",
                table: "Targets");

            migrationBuilder.DropColumn(
                name: "XCoordinate",
                table: "Targets");

            migrationBuilder.DropColumn(
                name: "YCoordinate",
                table: "Targets");

            migrationBuilder.CreateTable(
                name: "Timestamps",
                columns: table => new
                {
                    TimestampId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    VideoId = table.Column<int>(nullable: false),
                    end = table.Column<long>(nullable: false),
                    start = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timestamps", x => x.TimestampId);
                    table.ForeignKey(
                        name: "FK_Timestamps_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "VideoId",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_Timestamps_VideoId",
                table: "Timestamps",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_TargetCoordinates_VideoId",
                table: "TargetCoordinates",
                column: "VideoId");
        }
    }
}
