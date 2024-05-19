using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Travelling_Application.Migrations
{
    /// <inheritdoc />
    public partial class NewTablee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Accomodation_AccomodationId",
                table: "Photos");

            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Rooms_RoomID",
                table: "Photos");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Accomodation_AccomodationId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_AccomodationId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Photos_AccomodationId",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Photos_RoomID",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "AllInclusive",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Bathroom",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "BreakfastAndDinnerIncluded",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "BreakfastIncluded",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "CityView",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "GardenView",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "SeaView",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "ThreeMealsADay",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "AccomodationId",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "RoomID",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "AudioPrompts",
                table: "Accomodation");

            migrationBuilder.DropColumn(
                name: "Balcony",
                table: "Accomodation");

            migrationBuilder.DropColumn(
                name: "BathroomEmergencyButton",
                table: "Accomodation");

            migrationBuilder.DropColumn(
                name: "BraillePrompts",
                table: "Accomodation");

            migrationBuilder.DropColumn(
                name: "HighToilet",
                table: "Accomodation");

            migrationBuilder.DropColumn(
                name: "Kitchen",
                table: "Accomodation");

            migrationBuilder.DropColumn(
                name: "LowSink",
                table: "Accomodation");

            migrationBuilder.DropColumn(
                name: "NoSmookingRooms",
                table: "Accomodation");

            migrationBuilder.DropColumn(
                name: "SeaView",
                table: "Accomodation");

            migrationBuilder.DropColumn(
                name: "Terrace",
                table: "Accomodation");

            migrationBuilder.DropColumn(
                name: "ToiletWithGrabBars",
                table: "Accomodation");

            migrationBuilder.RenameColumn(
                name: "WheelchairAccessible",
                table: "Rooms",
                newName: "WheelchairAccessibleRoom");

            migrationBuilder.RenameColumn(
                name: "ViewFromTheWindow",
                table: "Rooms",
                newName: "FreeCancellation");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Rooms",
                newName: "View");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Rooms",
                newName: "TypeOfNutritionRoom");

            migrationBuilder.RenameColumn(
                name: "Cost",
                table: "Rooms",
                newName: "RoomCost");

            migrationBuilder.RenameColumn(
                name: "AvailableDates",
                table: "Rooms",
                newName: "RoomName");

            migrationBuilder.RenameColumn(
                name: "AccomodationName",
                table: "Rooms",
                newName: "RoomDescription");

            migrationBuilder.AlterColumn<int>(
                name: "AccomodationId",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AmountOfAvailableSameRooms",
                table: "Rooms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "AvailableDatesRoom",
                table: "Rooms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "RejectedMessage",
                table: "Accomodation",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "AccomodationPhotos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhotoArray = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    ObjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccomodationPhotos", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccomodationPhotos");

            migrationBuilder.DropColumn(
                name: "AmountOfAvailableSameRooms",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "AvailableDatesRoom",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "RejectedMessage",
                table: "Accomodation");

            migrationBuilder.RenameColumn(
                name: "WheelchairAccessibleRoom",
                table: "Rooms",
                newName: "WheelchairAccessible");

            migrationBuilder.RenameColumn(
                name: "View",
                table: "Rooms",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "TypeOfNutritionRoom",
                table: "Rooms",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "RoomName",
                table: "Rooms",
                newName: "AvailableDates");

            migrationBuilder.RenameColumn(
                name: "RoomDescription",
                table: "Rooms",
                newName: "AccomodationName");

            migrationBuilder.RenameColumn(
                name: "RoomCost",
                table: "Rooms",
                newName: "Cost");

            migrationBuilder.RenameColumn(
                name: "FreeCancellation",
                table: "Rooms",
                newName: "ViewFromTheWindow");

            migrationBuilder.AlterColumn<int>(
                name: "AccomodationId",
                table: "Rooms",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "AllInclusive",
                table: "Rooms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Bathroom",
                table: "Rooms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "BreakfastAndDinnerIncluded",
                table: "Rooms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "BreakfastIncluded",
                table: "Rooms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CityView",
                table: "Rooms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "GardenView",
                table: "Rooms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SeaView",
                table: "Rooms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ThreeMealsADay",
                table: "Rooms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "AccomodationId",
                table: "Photos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoomID",
                table: "Photos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AudioPrompts",
                table: "Accomodation",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Balcony",
                table: "Accomodation",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "BathroomEmergencyButton",
                table: "Accomodation",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "BraillePrompts",
                table: "Accomodation",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HighToilet",
                table: "Accomodation",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Kitchen",
                table: "Accomodation",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LowSink",
                table: "Accomodation",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "NoSmookingRooms",
                table: "Accomodation",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SeaView",
                table: "Accomodation",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Terrace",
                table: "Accomodation",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ToiletWithGrabBars",
                table: "Accomodation",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_AccomodationId",
                table: "Rooms",
                column: "AccomodationId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_AccomodationId",
                table: "Photos",
                column: "AccomodationId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_RoomID",
                table: "Photos",
                column: "RoomID");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Accomodation_AccomodationId",
                table: "Photos",
                column: "AccomodationId",
                principalTable: "Accomodation",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Rooms_RoomID",
                table: "Photos",
                column: "RoomID",
                principalTable: "Rooms",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Accomodation_AccomodationId",
                table: "Rooms",
                column: "AccomodationId",
                principalTable: "Accomodation",
                principalColumn: "Id");
        }
    }
}
