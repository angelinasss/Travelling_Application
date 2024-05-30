using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Travelling_Application.Migrations
{
    /// <inheritdoc />
    public partial class AddCanceledBookingsColumnToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanceledBooking",
                table: "BookingCars",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanceledBooking",
                table: "BookingAttractions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanceledBooking",
                table: "BookingAirTickets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanceledBooking",
                table: "BookingAccomodations",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanceledBooking",
                table: "BookingCars");

            migrationBuilder.DropColumn(
                name: "CanceledBooking",
                table: "BookingAttractions");

            migrationBuilder.DropColumn(
                name: "CanceledBooking",
                table: "BookingAirTickets");

            migrationBuilder.DropColumn(
                name: "CanceledBooking",
                table: "BookingAccomodations");
        }
    }
}
