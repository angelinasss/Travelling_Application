using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Travelling_Application.Migrations
{
    /// <inheritdoc />
    public partial class AddRejectedToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "RejectedBooking",
                table: "BookingCars",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RejectedMessage",
                table: "BookingCars",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "RejectedBooking",
                table: "BookingAttractions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RejectedMessage",
                table: "BookingAttractions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "RejectedBooking",
                table: "BookingAirTickets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RejectedMessage",
                table: "BookingAirTickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "RejectedBooking",
                table: "BookingAccomodations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RejectedMessage",
                table: "BookingAccomodations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RejectedBooking",
                table: "BookingCars");

            migrationBuilder.DropColumn(
                name: "RejectedMessage",
                table: "BookingCars");

            migrationBuilder.DropColumn(
                name: "RejectedBooking",
                table: "BookingAttractions");

            migrationBuilder.DropColumn(
                name: "RejectedMessage",
                table: "BookingAttractions");

            migrationBuilder.DropColumn(
                name: "RejectedBooking",
                table: "BookingAirTickets");

            migrationBuilder.DropColumn(
                name: "RejectedMessage",
                table: "BookingAirTickets");

            migrationBuilder.DropColumn(
                name: "RejectedBooking",
                table: "BookingAccomodations");

            migrationBuilder.DropColumn(
                name: "RejectedMessage",
                table: "BookingAccomodations");
        }
    }
}
