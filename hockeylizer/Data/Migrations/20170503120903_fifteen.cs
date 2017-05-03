using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace hockeylizer.Data.Migrations
{
    public partial class fifteen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Targets_Videos_VideoId",
                table: "Targets");

            migrationBuilder.DropTable(
                name: "Videos");

            migrationBuilder.DropIndex(
                name: "IX_Targets_VideoId",
                table: "Targets");

            migrationBuilder.AddColumn<int>(
                name: "RelatedSessionSessionId",
                table: "Targets",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Frames",
                columns: table => new
                {
                    FrameId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Analyzed = table.Column<bool>(nullable: false),
                    FrameUrl = table.Column<string>(nullable: true),
                    TargetId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Frames", x => x.FrameId);
                    table.ForeignKey(
                        name: "FK_Frames_Targets_TargetId",
                        column: x => x.TargetId,
                        principalTable: "Targets",
                        principalColumn: "TargetId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    SessionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    FileName = table.Column<string>(nullable: true),
                    Interval = table.Column<int>(nullable: false),
                    NumberOfTargets = table.Column<int>(nullable: false),
                    PlayerId = table.Column<int>(nullable: false),
                    Rounds = table.Column<int>(nullable: false),
                    Shots = table.Column<int>(nullable: false),
                    VideoPath = table.Column<string>(nullable: true),
                    VideoRemoved = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.SessionId);
                    table.ForeignKey(
                        name: "FK_Sessions_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Targets_RelatedSessionSessionId",
                table: "Targets",
                column: "RelatedSessionSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Frames_TargetId",
                table: "Frames",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_PlayerId",
                table: "Sessions",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Targets_Sessions_RelatedSessionSessionId",
                table: "Targets",
                column: "RelatedSessionSessionId",
                principalTable: "Sessions",
                principalColumn: "SessionId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Targets_Sessions_RelatedSessionSessionId",
                table: "Targets");

            migrationBuilder.DropTable(
                name: "Frames");

            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropIndex(
                name: "IX_Targets_RelatedSessionSessionId",
                table: "Targets");

            migrationBuilder.DropColumn(
                name: "RelatedSessionSessionId",
                table: "Targets");

            migrationBuilder.CreateTable(
                name: "Videos",
                columns: table => new
                {
                    VideoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Deleted = table.Column<bool>(nullable: false),
                    FileName = table.Column<string>(nullable: true),
                    Interval = table.Column<int>(nullable: false),
                    NumberOfTargets = table.Column<int>(nullable: false),
                    PlayerId = table.Column<int>(nullable: false),
                    Rounds = table.Column<int>(nullable: false),
                    Shots = table.Column<int>(nullable: false),
                    VideoPath = table.Column<string>(nullable: true),
                    VideoRemoved = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Videos", x => x.VideoId);
                    table.ForeignKey(
                        name: "FK_Videos_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Targets_VideoId",
                table: "Targets",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_Videos_PlayerId",
                table: "Videos",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Targets_Videos_VideoId",
                table: "Targets",
                column: "VideoId",
                principalTable: "Videos",
                principalColumn: "VideoId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
