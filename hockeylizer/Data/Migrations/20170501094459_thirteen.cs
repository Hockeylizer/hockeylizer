using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace hockeylizer.Data.Migrations
{
    public partial class thirteen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnalysisResults_Videos_VideoId",
                table: "AnalysisResults");

            migrationBuilder.DropIndex(
                name: "IX_AnalysisResults_VideoId",
                table: "AnalysisResults");

            migrationBuilder.DropColumn(
                name: "VideoId",
                table: "AnalysisResults");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VideoId",
                table: "AnalysisResults",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnalysisResults_VideoId",
                table: "AnalysisResults",
                column: "VideoId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnalysisResults_Videos_VideoId",
                table: "AnalysisResults",
                column: "VideoId",
                principalTable: "Videos",
                principalColumn: "VideoId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
