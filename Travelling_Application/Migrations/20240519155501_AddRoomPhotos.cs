using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Travelling_Application.Migrations
{
    /// <inheritdoc />
    public partial class AddRoomPhotos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "MainPhoto",
                table: "Rooms",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.CreateTable(
                name: "RoomPhotos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhotoArray = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    ObjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomPhotos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_AccomodationId",
                table: "Rooms",
                column: "AccomodationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Accomodation_AccomodationId",
                table: "Rooms",
                column: "AccomodationId",
                principalTable: "Accomodation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Accomodation_AccomodationId",
                table: "Rooms");

            migrationBuilder.DropTable(
                name: "RoomPhotos");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_AccomodationId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "MainPhoto",
                table: "Rooms");
        }
    }
}
