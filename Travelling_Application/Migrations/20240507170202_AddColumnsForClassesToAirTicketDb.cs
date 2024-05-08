using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Travelling_Application.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnsForClassesToAirTicketDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Cost",
                table: "AirTicket",
                newName: "CostFC");

            migrationBuilder.RenameColumn(
                name: "AmountOfTickets",
                table: "AirTicket",
                newName: "AmountOfTicketsFC");

            migrationBuilder.AddColumn<int>(
                name: "AmountOfTicketsBC",
                table: "AirTicket",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AmountOfTicketsEC",
                table: "AirTicket",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "CostBC",
                table: "AirTicket",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CostEC",
                table: "AirTicket",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "IncludeLuggageBC",
                table: "AirTicket",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IncludeLuggageEC",
                table: "AirTicket",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IncludeLuggageFC",
                table: "AirTicket",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountOfTicketsBC",
                table: "AirTicket");

            migrationBuilder.DropColumn(
                name: "AmountOfTicketsEC",
                table: "AirTicket");

            migrationBuilder.DropColumn(
                name: "CostBC",
                table: "AirTicket");

            migrationBuilder.DropColumn(
                name: "CostEC",
                table: "AirTicket");

            migrationBuilder.DropColumn(
                name: "IncludeLuggageBC",
                table: "AirTicket");

            migrationBuilder.DropColumn(
                name: "IncludeLuggageEC",
                table: "AirTicket");

            migrationBuilder.DropColumn(
                name: "IncludeLuggageFC",
                table: "AirTicket");

            migrationBuilder.RenameColumn(
                name: "CostFC",
                table: "AirTicket",
                newName: "Cost");

            migrationBuilder.RenameColumn(
                name: "AmountOfTicketsFC",
                table: "AirTicket",
                newName: "AmountOfTickets");
        }
    }
}
