using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Authenticator_Application_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddOtpCreatedAtColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "OtpCreatedAt",
                table: "Users",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OtpCreatedAt",
                table: "Users");
        }
    }
}
