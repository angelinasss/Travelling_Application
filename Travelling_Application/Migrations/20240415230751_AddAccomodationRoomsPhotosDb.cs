using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Travelling_Application.Migrations
{
    /// <inheritdoc />
    public partial class AddAccomodationRoomsPhotosDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accomodation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TypesOfNutrition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    TypeOfAccomodation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Parking = table.Column<bool>(type: "bit", nullable: false),
                    SwimmingPool = table.Column<int>(type: "int", nullable: false),
                    FreeWIFI = table.Column<bool>(type: "bit", nullable: false),
                    PrivateBeach = table.Column<bool>(type: "bit", nullable: false),
                    LineOfBeach = table.Column<int>(type: "int", nullable: false),
                    Restaurants = table.Column<int>(type: "int", nullable: false),
                    SPA = table.Column<bool>(type: "bit", nullable: false),
                    Bar = table.Column<bool>(type: "bit", nullable: false),
                    Kitchen = table.Column<bool>(type: "bit", nullable: false),
                    Garden = table.Column<bool>(type: "bit", nullable: false),
                    SeaView = table.Column<bool>(type: "bit", nullable: false),
                    Balcony = table.Column<bool>(type: "bit", nullable: false),
                    Terrace = table.Column<bool>(type: "bit", nullable: false),
                    TransferToAirport = table.Column<bool>(type: "bit", nullable: false),
                    HighToilet = table.Column<bool>(type: "bit", nullable: false),
                    ToiletWithGrabBars = table.Column<bool>(type: "bit", nullable: false),
                    LowSink = table.Column<bool>(type: "bit", nullable: false),
                    BathroomEmergencyButton = table.Column<bool>(type: "bit", nullable: false),
                    BraillePrompts = table.Column<bool>(type: "bit", nullable: false),
                    TactileSigns = table.Column<bool>(type: "bit", nullable: false),
                    AudioPrompts = table.Column<bool>(type: "bit", nullable: false),
                    SmookingRooms = table.Column<bool>(type: "bit", nullable: false),
                    NoSmookingRooms = table.Column<bool>(type: "bit", nullable: false),
                    FamilyRooms = table.Column<bool>(type: "bit", nullable: false),
                    CarChargingStation = table.Column<bool>(type: "bit", nullable: false),
                    WheelchairAccessible = table.Column<bool>(type: "bit", nullable: false),
                    FitnessCentre = table.Column<bool>(type: "bit", nullable: false),
                    PetsAllowed = table.Column<bool>(type: "bit", nullable: false),
                    DeliveryFoodToTheRoom = table.Column<bool>(type: "bit", nullable: false),
                    EveryHourFrontDesk = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accomodation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccomodationName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AvailableDates = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cost = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bathroom = table.Column<bool>(type: "bit", nullable: false),
                    WashingMachine = table.Column<bool>(type: "bit", nullable: false),
                    Kitchen = table.Column<bool>(type: "bit", nullable: false),
                    WheelchairAccessible = table.Column<bool>(type: "bit", nullable: false),
                    ToiletWithGrabBars = table.Column<bool>(type: "bit", nullable: false),
                    BathtubWithgrabbars = table.Column<bool>(type: "bit", nullable: false),
                    BarrierFreeShower = table.Column<bool>(type: "bit", nullable: false),
                    ShowerWithoutEdge = table.Column<bool>(type: "bit", nullable: false),
                    HighToilet = table.Column<bool>(type: "bit", nullable: false),
                    LowSink = table.Column<bool>(type: "bit", nullable: false),
                    BathroomEmergencyButton = table.Column<bool>(type: "bit", nullable: false),
                    ShowerChair = table.Column<bool>(type: "bit", nullable: false),
                    BreakfastIncluded = table.Column<bool>(type: "bit", nullable: false),
                    ThreeMealsADay = table.Column<bool>(type: "bit", nullable: false),
                    AllInclusive = table.Column<bool>(type: "bit", nullable: false),
                    BreakfastAndDinnerIncluded = table.Column<bool>(type: "bit", nullable: false),
                    CoffeeMachine = table.Column<bool>(type: "bit", nullable: false),
                    CoffeeOrTea = table.Column<bool>(type: "bit", nullable: false),
                    ElectricKettle = table.Column<bool>(type: "bit", nullable: false),
                    SeaView = table.Column<bool>(type: "bit", nullable: false),
                    CityView = table.Column<bool>(type: "bit", nullable: false),
                    GardenView = table.Column<bool>(type: "bit", nullable: false),
                    ViewFromTheWindow = table.Column<bool>(type: "bit", nullable: false),
                    Soundproofing = table.Column<bool>(type: "bit", nullable: false),
                    Patio = table.Column<bool>(type: "bit", nullable: false),
                    FlatScreenTV = table.Column<bool>(type: "bit", nullable: false),
                    Balcony = table.Column<bool>(type: "bit", nullable: false),
                    Terrace = table.Column<bool>(type: "bit", nullable: false),
                    PrivatePool = table.Column<bool>(type: "bit", nullable: false),
                    Bath = table.Column<bool>(type: "bit", nullable: false),
                    PlaceToWorkOnALaptop = table.Column<bool>(type: "bit", nullable: false),
                    AirConditioner = table.Column<bool>(type: "bit", nullable: false),
                    PrivateBathroom = table.Column<bool>(type: "bit", nullable: false),
                    AccomodationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Rooms_Accomodation_AccomodationId",
                        column: x => x.AccomodationId,
                        principalTable: "Accomodation",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccomodationId = table.Column<int>(type: "int", nullable: true),
                    RoomID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Photos_Accomodation_AccomodationId",
                        column: x => x.AccomodationId,
                        principalTable: "Accomodation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Photos_Rooms_RoomID",
                        column: x => x.RoomID,
                        principalTable: "Rooms",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Photos_AccomodationId",
                table: "Photos",
                column: "AccomodationId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_RoomID",
                table: "Photos",
                column: "RoomID");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_AccomodationId",
                table: "Rooms",
                column: "AccomodationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Photos");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Accomodation");


          

        }
    }
}
