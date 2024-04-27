using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Travelling_Application.Migrations
{
    /// <inheritdoc />
    public partial class AddCarAvailabilityToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AmountOfPassengers",
                table: "Car",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CarCategory",
                table: "Car",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "CarPhoto",
                table: "Car",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<bool>(
                name: "CollisionDamageWaiver",
                table: "Car",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ElectricCar",
                table: "Car",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "FreeCancellation",
                table: "Car",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LiabilityCoverage",
                table: "Car",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TheftCoverage",
                table: "Car",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UnlimitedMileage",
                table: "Car",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountOfPassengers",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "CarCategory",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "CarPhoto",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "CollisionDamageWaiver",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "ElectricCar",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "FreeCancellation",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "LiabilityCoverage",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "TheftCoverage",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "UnlimitedMileage",
                table: "Car");
        }
    }
}
