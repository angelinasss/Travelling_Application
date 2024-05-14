using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Travelling_Application.Migrations
{
    /// <inheritdoc />
    public partial class AddVerifiedByAdminColumnsToAirTicketDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "VerifiedByAdminBCTicket",
                table: "AirTicket",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "VerifiedByAdminECTicket",
                table: "AirTicket",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "VerifiedByAdminFCTicket",
                table: "AirTicket",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VerifiedByAdminBCTicket",
                table: "AirTicket");

            migrationBuilder.DropColumn(
                name: "VerifiedByAdminECTicket",
                table: "AirTicket");

            migrationBuilder.DropColumn(
                name: "VerifiedByAdminFCTicket",
                table: "AirTicket");
        }
    }
}
