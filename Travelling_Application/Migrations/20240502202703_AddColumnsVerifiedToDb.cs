using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Travelling_Application.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnsVerifiedToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "VerifiedByAdmin",
                table: "Entertainment",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "VerifiedByAdmin",
                table: "Car",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "VerifiedByAdmin",
                table: "AirTicket",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "VerifiedByAdmin",
                table: "Accomodation",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VerifiedByAdmin",
                table: "Entertainment");

            migrationBuilder.DropColumn(
                name: "VerifiedByAdmin",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "VerifiedByAdmin",
                table: "AirTicket");

            migrationBuilder.DropColumn(
                name: "VerifiedByAdmin",
                table: "Accomodation");
        }
    }
}
