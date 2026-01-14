using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRecentlyViewed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RecentlyViewedMusicSheets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MusicSheetId = table.Column<int>(type: "int", nullable: false),
                    LastViewedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecentlyViewedMusicSheets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecentlyViewedMusicSheets_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecentlyViewedMusicSheets_MusicSheets_MusicSheetId",
                        column: x => x.MusicSheetId,
                        principalTable: "MusicSheets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecentlyViewedMusicSheets_MusicSheetId",
                table: "RecentlyViewedMusicSheets",
                column: "MusicSheetId");

            migrationBuilder.CreateIndex(
                name: "IX_RecentlyViewedMusicSheets_UserId_MusicSheetId",
                table: "RecentlyViewedMusicSheets",
                columns: new[] { "UserId", "MusicSheetId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecentlyViewedMusicSheets");
        }
    }
}
