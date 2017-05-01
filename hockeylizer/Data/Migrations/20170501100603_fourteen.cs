using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace hockeylizer.Data.Migrations
{
    public partial class fourteen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnalysisResultId",
                table: "Targets",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TargetId",
                table: "AnalysisResults",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Targets_AnalysisResultId",
                table: "Targets",
                column: "AnalysisResultId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalysisResults_TargetId",
                table: "AnalysisResults",
                column: "TargetId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnalysisResults_Targets_TargetId",
                table: "AnalysisResults",
                column: "TargetId",
                principalTable: "Targets",
                principalColumn: "TargetId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Targets_AnalysisResults_AnalysisResultId",
                table: "Targets",
                column: "AnalysisResultId",
                principalTable: "AnalysisResults",
                principalColumn: "AnalysisId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnalysisResults_Targets_TargetId",
                table: "AnalysisResults");

            migrationBuilder.DropForeignKey(
                name: "FK_Targets_AnalysisResults_AnalysisResultId",
                table: "Targets");

            migrationBuilder.DropIndex(
                name: "IX_Targets_AnalysisResultId",
                table: "Targets");

            migrationBuilder.DropIndex(
                name: "IX_AnalysisResults_TargetId",
                table: "AnalysisResults");

            migrationBuilder.DropColumn(
                name: "AnalysisResultId",
                table: "Targets");

            migrationBuilder.DropColumn(
                name: "TargetId",
                table: "AnalysisResults");
        }
    }
}
