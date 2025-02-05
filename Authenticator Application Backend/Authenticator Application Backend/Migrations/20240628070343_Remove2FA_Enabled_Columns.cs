using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Authenticator_Application_Backend.Migrations
{
    /// <inheritdoc />
    public partial class Remove2FA_Enabled_Columns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Google_2FA_Enabled",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Microsoft_2FA_Enabled",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Google_2FA_Enabled",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Microsoft_2FA_Enabled",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
