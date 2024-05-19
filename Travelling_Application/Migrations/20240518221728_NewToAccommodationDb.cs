using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Travelling_Application.Migrations
{
    /// <inheritdoc />
    public partial class NewToAccommodationDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "MainPhoto",
                table: "Accomodation",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<int>(
                name: "PublisherId",
                table: "Accomodation",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MainPhoto",
                table: "Accomodation");

            migrationBuilder.DropColumn(
                name: "PublisherId",
                table: "Accomodation");
        }
    }
}
