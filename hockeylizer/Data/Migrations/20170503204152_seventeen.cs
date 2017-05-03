using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace hockeylizer.Data.Migrations
{
    public partial class seventeen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Targets_Sessions_RelatedSessionSessionId",
                table: "Targets");

            migrationBuilder.DropTable(
                name: "AnalysisResults");

            migrationBuilder.DropIndex(
                name: "IX_Targets_RelatedSessionSessionId",
                table: "Targets");

            migrationBuilder.DropColumn(
                name: "RelatedSessionSessionId",
                table: "Targets");

            migrationBuilder.RenameColumn(
                name: "VideoId",
                table: "Targets",
                newName: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Targets_SessionId",
                table: "Targets",
                column: "SessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Targets_Sessions_SessionId",
                table: "Targets",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "SessionId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Targets_Sessions_SessionId",
                table: "Targets");

            migrationBuilder.DropIndex(
                name: "IX_Targets_SessionId",
                table: "Targets");

            migrationBuilder.RenameColumn(
                name: "SessionId",
                table: "Targets",
                newName: "VideoId");

            migrationBuilder.AddColumn<int>(
                name: "RelatedSessionSessionId",
                table: "Targets",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AnalysisResults",
                columns: table => new
                {
                    AnalysisId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HitGoal = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalysisResults", x => x.AnalysisId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Targets_RelatedSessionSessionId",
                table: "Targets",
                column: "RelatedSessionSessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Targets_Sessions_RelatedSessionSessionId",
                table: "Targets",
                column: "RelatedSessionSessionId",
                principalTable: "Sessions",
                principalColumn: "SessionId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
