using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Remove_FilePath_FileSize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileSize",
                table: "MusicSheets");

            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "MusicSheets",
                newName: "TranscriptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TranscriptionId",
                table: "MusicSheets",
                newName: "FilePath");

            migrationBuilder.AddColumn<long>(
                name: "FileSize",
                table: "MusicSheets",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
