using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCommentParentId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "MusicSheetComments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MusicSheetComments_ParentId",
                table: "MusicSheetComments",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_MusicSheetComments_MusicSheetComments_ParentId",
                table: "MusicSheetComments",
                column: "ParentId",
                principalTable: "MusicSheetComments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MusicSheetComments_MusicSheetComments_ParentId",
                table: "MusicSheetComments");

            migrationBuilder.DropIndex(
                name: "IX_MusicSheetComments_ParentId",
                table: "MusicSheetComments");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "MusicSheetComments");
        }
    }
}
