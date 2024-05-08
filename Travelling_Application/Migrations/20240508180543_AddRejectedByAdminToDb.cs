using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Travelling_Application.Migrations
{
    /// <inheritdoc />
    public partial class AddRejectedByAdminToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "RejectedByAdmin",
                table: "Entertainment",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RejectedByAdmin",
                table: "Car",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RejectedByAdmin",
                table: "AirTicket",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RejectedByAdmin",
                table: "Accomodation",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RejectedByAdmin",
                table: "Entertainment");

            migrationBuilder.DropColumn(
                name: "RejectedByAdmin",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "RejectedByAdmin",
                table: "AirTicket");

            migrationBuilder.DropColumn(
                name: "RejectedByAdmin",
                table: "Accomodation");
        }
    }
}
