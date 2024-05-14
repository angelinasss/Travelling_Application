using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Travelling_Application.Migrations
{
    /// <inheritdoc />
    public partial class AddRejectedColumnsToAirTicketDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "RejectedByAdminBC",
                table: "AirTicket",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RejectedByAdminEC",
                table: "AirTicket",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RejectedByAdminFC",
                table: "AirTicket",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RejectedMessageBC",
                table: "AirTicket",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RejectedMessageEC",
                table: "AirTicket",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RejectedMessageFC",
                table: "AirTicket",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RejectedByAdminBC",
                table: "AirTicket");

            migrationBuilder.DropColumn(
                name: "RejectedByAdminEC",
                table: "AirTicket");

            migrationBuilder.DropColumn(
                name: "RejectedByAdminFC",
                table: "AirTicket");

            migrationBuilder.DropColumn(
                name: "RejectedMessageBC",
                table: "AirTicket");

            migrationBuilder.DropColumn(
                name: "RejectedMessageEC",
                table: "AirTicket");

            migrationBuilder.DropColumn(
                name: "RejectedMessageFC",
                table: "AirTicket");
        }
    }
}
