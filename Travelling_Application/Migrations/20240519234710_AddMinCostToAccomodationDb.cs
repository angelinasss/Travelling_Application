using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Travelling_Application.Migrations
{
    /// <inheritdoc />
    public partial class AddMinCostToAccomodationDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "MinCost",
                table: "Accomodation",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_RoomPhotos_RoomId",
                table: "RoomPhotos",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomPhotos_Rooms_RoomId",
                table: "RoomPhotos",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomPhotos_Rooms_RoomId",
                table: "RoomPhotos");

            migrationBuilder.DropIndex(
                name: "IX_RoomPhotos_RoomId",
                table: "RoomPhotos");

            migrationBuilder.DropColumn(
                name: "MinCost",
                table: "Accomodation");
        }
    }
}
