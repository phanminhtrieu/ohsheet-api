using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_More_ApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Email",
                table: "AuditLogins",
                newName: "UserName");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastSignInDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastSignInDate",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "AuditLogins",
                newName: "Email");
        }
    }
}
