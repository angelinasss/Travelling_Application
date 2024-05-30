﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Travelling_Application.Models;

#nullable disable

namespace Travelling_Application.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240525020539_AddColumnToAccomodationDbB")]
    partial class AddColumnToAccomodationDbB
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Travelling_Application.Models.Accomodation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AvailableRoomsNames")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Bar")
                        .HasColumnType("bit");

                    b.Property<bool>("CarChargingStation")
                        .HasColumnType("bit");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("DeliveryFoodToTheRoom")
                        .HasColumnType("bit");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EveryHourFrontDesk")
                        .HasColumnType("bit");

                    b.Property<bool>("FamilyRooms")
                        .HasColumnType("bit");

                    b.Property<bool>("FitnessCentre")
                        .HasColumnType("bit");

                    b.Property<bool>("FreeWIFI")
                        .HasColumnType("bit");

                    b.Property<bool>("Garden")
                        .HasColumnType("bit");

                    b.Property<int>("LineOfBeach")
                        .HasColumnType("int");

                    b.Property<byte[]>("MainPhoto")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<double>("MinCost")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Parking")
                        .HasColumnType("bit");

                    b.Property<bool>("PetsAllowed")
                        .HasColumnType("bit");

                    b.Property<bool>("PrivateBeach")
                        .HasColumnType("bit");

                    b.Property<int>("PublisherId")
                        .HasColumnType("int");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<bool>("RejectedByAdmin")
                        .HasColumnType("bit");

                    b.Property<string>("RejectedMessage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Restaurants")
                        .HasColumnType("int");

                    b.Property<bool>("SPA")
                        .HasColumnType("bit");

                    b.Property<bool>("SmookingRooms")
                        .HasColumnType("bit");

                    b.Property<int>("SwimmingPool")
                        .HasColumnType("int");

                    b.Property<bool>("TactileSigns")
                        .HasColumnType("bit");

                    b.Property<bool>("TransferToAirport")
                        .HasColumnType("bit");

                    b.Property<string>("TypeOfAccomodation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TypesOfNutrition")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.Property<int?>("UserId1")
                        .HasColumnType("int");

                    b.Property<bool>("VerifiedByAdmin")
                        .HasColumnType("bit");

                    b.Property<bool>("WheelchairAccessible")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("UserId1");

                    b.ToTable("Accomodation");
                });

            modelBuilder.Entity("Travelling_Application.Models.AccomodationPhotos", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ObjectId")
                        .HasColumnType("int");

                    b.Property<byte[]>("PhotoArray")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.HasKey("Id");

                    b.ToTable("AccomodationPhotos");
                });

            modelBuilder.Entity("Travelling_Application.Models.AirTicket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AmountOfTicketsBC")
                        .HasColumnType("int");

                    b.Property<int>("AmountOfTicketsEC")
                        .HasColumnType("int");

                    b.Property<int>("AmountOfTicketsFC")
                        .HasColumnType("int");

                    b.Property<string>("ArrivalCountryCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ArrivalTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("CityFrom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CityTo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("CostBC")
                        .HasColumnType("float");

                    b.Property<double>("CostEC")
                        .HasColumnType("float");

                    b.Property<double>("CostFC")
                        .HasColumnType("float");

                    b.Property<string>("CountryFrom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CountryTo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DepartureCountryCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DepartureTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("FlightNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("FreeCancellation")
                        .HasColumnType("bit");

                    b.Property<bool>("IncludeLuggageBC")
                        .HasColumnType("bit");

                    b.Property<bool>("IncludeLuggageEC")
                        .HasColumnType("bit");

                    b.Property<bool>("IncludeLuggageFC")
                        .HasColumnType("bit");

                    b.Property<int>("PublisherId")
                        .HasColumnType("int");

                    b.Property<bool>("RejectedByAdminBC")
                        .HasColumnType("bit");

                    b.Property<bool>("RejectedByAdminEC")
                        .HasColumnType("bit");

                    b.Property<bool>("RejectedByAdminFC")
                        .HasColumnType("bit");

                    b.Property<string>("RejectedMessageBC")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RejectedMessageEC")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RejectedMessageFC")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.Property<int?>("UserId1")
                        .HasColumnType("int");

                    b.Property<bool>("VerifiedByAdminBCTicket")
                        .HasColumnType("bit");

                    b.Property<bool>("VerifiedByAdminECTicket")
                        .HasColumnType("bit");

                    b.Property<bool>("VerifiedByAdminFCTicket")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("UserId1");

                    b.ToTable("AirTicket");
                });

            modelBuilder.Entity("Travelling_Application.Models.BookingAccomodation", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<int>("AccomodationId")
                        .HasColumnType("int");

                    b.Property<int>("Adults")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateOfDeparture")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ReturnDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("RoomId")
                        .HasColumnType("int");

                    b.Property<string>("TypeOfRoom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<bool>("VerifiedBooking")
                        .HasColumnType("bit");

                    b.HasKey("id");

                    b.ToTable("BookingAccomodations");
                });

            modelBuilder.Entity("Travelling_Application.Models.BookingAirTicket", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<int>("AirTicketId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateOfDeparture")
                        .HasColumnType("datetime2");

                    b.Property<int>("Passengers")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReturnDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("TypeClass")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<bool>("VerifiedBooking")
                        .HasColumnType("bit");

                    b.HasKey("id");

                    b.ToTable("BookingAirTickets");
                });

            modelBuilder.Entity("Travelling_Application.Models.BookingAttraction", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<int>("AmountOfTickets")
                        .HasColumnType("int");

                    b.Property<int>("AttractionId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<bool>("VerifiedBooking")
                        .HasColumnType("bit");

                    b.HasKey("id");

                    b.ToTable("BookingAttractions");
                });

            modelBuilder.Entity("Travelling_Application.Models.BookingCar", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<int>("CarId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateOfDeparture")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ReturnDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<bool>("VerifiedBooking")
                        .HasColumnType("bit");

                    b.HasKey("id");

                    b.ToTable("BookingCars");
                });

            modelBuilder.Entity("Travelling_Application.Models.Car", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("AmountOfPassengers")
                        .HasColumnType("int");

                    b.Property<string>("CarCategory")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("CarPhoto")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("CollisionDamageWaiver")
                        .HasColumnType("bit");

                    b.Property<double>("Cost")
                        .HasColumnType("float");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("ElectricCar")
                        .HasColumnType("bit");

                    b.Property<string>("EndDates")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("FreeCancellation")
                        .HasColumnType("bit");

                    b.Property<bool>("IsAirCondition")
                        .HasColumnType("bit");

                    b.Property<bool>("LiabilityCoverage")
                        .HasColumnType("bit");

                    b.Property<int>("PublisherId")
                        .HasColumnType("int");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<bool>("RejectedByAdmin")
                        .HasColumnType("bit");

                    b.Property<string>("RejectedMessage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StartDates")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TheftCoverage")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Transmission")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("UnlimitedMileage")
                        .HasColumnType("bit");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.Property<int?>("UserId1")
                        .HasColumnType("int");

                    b.Property<bool>("VerifiedByAdmin")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("UserId1");

                    b.ToTable("Car");
                });

            modelBuilder.Entity("Travelling_Application.Models.Entertainment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AmountOfTickets")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AvailableDates")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Cost")
                        .HasColumnType("float");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("FreeCancellation")
                        .HasColumnType("bit");

                    b.Property<string>("Languages")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("MainPhoto")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("PublisherId")
                        .HasColumnType("int");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<bool>("RejectedByAdmin")
                        .HasColumnType("bit");

                    b.Property<string>("RejectedMessage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TimeOfDay")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.Property<int?>("UserId1")
                        .HasColumnType("int");

                    b.Property<bool>("VerifiedByAdmin")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("UserId1");

                    b.ToTable("Entertainment");
                });

            modelBuilder.Entity("Travelling_Application.Models.Photo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("Travelling_Application.Models.Photos", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ObjectId")
                        .HasColumnType("int");

                    b.Property<byte[]>("PhotoArray")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.HasKey("Id");

                    b.ToTable("ObjectPhotos");
                });

            modelBuilder.Entity("Travelling_Application.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Travelling_Application.Models.Room", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("AccomodationId")
                        .HasColumnType("int");

                    b.Property<bool>("AirConditioner")
                        .HasColumnType("bit");

                    b.Property<string>("AmountOfAvailableSameRooms")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AvailableDatesRoom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Balcony")
                        .HasColumnType("bit");

                    b.Property<bool>("BarrierFreeShower")
                        .HasColumnType("bit");

                    b.Property<bool>("Bath")
                        .HasColumnType("bit");

                    b.Property<bool>("BathroomEmergencyButton")
                        .HasColumnType("bit");

                    b.Property<bool>("BathtubWithgrabbars")
                        .HasColumnType("bit");

                    b.Property<bool>("CoffeeMachine")
                        .HasColumnType("bit");

                    b.Property<bool>("CoffeeOrTea")
                        .HasColumnType("bit");

                    b.Property<bool>("ElectricKettle")
                        .HasColumnType("bit");

                    b.Property<bool>("FlatScreenTV")
                        .HasColumnType("bit");

                    b.Property<bool>("FreeCancellation")
                        .HasColumnType("bit");

                    b.Property<bool>("HighToilet")
                        .HasColumnType("bit");

                    b.Property<bool>("Kitchen")
                        .HasColumnType("bit");

                    b.Property<bool>("LowSink")
                        .HasColumnType("bit");

                    b.Property<byte[]>("MainPhoto")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<bool>("Patio")
                        .HasColumnType("bit");

                    b.Property<bool>("PlaceToWorkOnALaptop")
                        .HasColumnType("bit");

                    b.Property<bool>("PrivateBathroom")
                        .HasColumnType("bit");

                    b.Property<bool>("PrivatePool")
                        .HasColumnType("bit");

                    b.Property<double>("RoomCost")
                        .HasColumnType("float");

                    b.Property<string>("RoomDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoomName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("ShowerChair")
                        .HasColumnType("bit");

                    b.Property<bool>("ShowerWithoutEdge")
                        .HasColumnType("bit");

                    b.Property<bool>("Soundproofing")
                        .HasColumnType("bit");

                    b.Property<bool>("Terrace")
                        .HasColumnType("bit");

                    b.Property<bool>("ToiletWithGrabBars")
                        .HasColumnType("bit");

                    b.Property<string>("TypeOfNutritionRoom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("View")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("WashingMachine")
                        .HasColumnType("bit");

                    b.Property<bool>("WheelchairAccessibleRoom")
                        .HasColumnType("bit");

                    b.HasKey("ID");

                    b.HasIndex("AccomodationId");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("Travelling_Application.Models.RoomPhotos", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ObjectId")
                        .HasColumnType("int");

                    b.Property<byte[]>("PhotoArray")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("RoomId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.ToTable("RoomPhotos");
                });

            modelBuilder.Entity("Travelling_Application.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Birthday")
                        .HasColumnType("datetime2");

                    b.Property<string>("CountryCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nationality")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("Sex")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("UserPhotoUrl")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Travelling_Application.Models.Accomodation", b =>
                {
                    b.HasOne("Travelling_Application.Models.User", null)
                        .WithMany("BookingAccomodation")
                        .HasForeignKey("UserId");

                    b.HasOne("Travelling_Application.Models.User", null)
                        .WithMany("FavoriteAccomodation")
                        .HasForeignKey("UserId1");
                });

            modelBuilder.Entity("Travelling_Application.Models.AirTicket", b =>
                {
                    b.HasOne("Travelling_Application.Models.User", null)
                        .WithMany("BookingAirTicket")
                        .HasForeignKey("UserId");

                    b.HasOne("Travelling_Application.Models.User", null)
                        .WithMany("FavoriteAirTicket")
                        .HasForeignKey("UserId1");
                });

            modelBuilder.Entity("Travelling_Application.Models.Car", b =>
                {
                    b.HasOne("Travelling_Application.Models.User", null)
                        .WithMany("BookingCars")
                        .HasForeignKey("UserId");

                    b.HasOne("Travelling_Application.Models.User", null)
                        .WithMany("FavoriteCars")
                        .HasForeignKey("UserId1");
                });

            modelBuilder.Entity("Travelling_Application.Models.Entertainment", b =>
                {
                    b.HasOne("Travelling_Application.Models.User", null)
                        .WithMany("BookingEntertainment")
                        .HasForeignKey("UserId");

                    b.HasOne("Travelling_Application.Models.User", null)
                        .WithMany("FavoriteEntertainment")
                        .HasForeignKey("UserId1");
                });

            modelBuilder.Entity("Travelling_Application.Models.Room", b =>
                {
                    b.HasOne("Travelling_Application.Models.Accomodation", null)
                        .WithMany("AllRooms")
                        .HasForeignKey("AccomodationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Travelling_Application.Models.RoomPhotos", b =>
                {
                    b.HasOne("Travelling_Application.Models.Room", null)
                        .WithMany("Photos")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Travelling_Application.Models.User", b =>
                {
                    b.HasOne("Travelling_Application.Models.Role", null)
                        .WithMany("Users")
                        .HasForeignKey("RoleId");
                });

            modelBuilder.Entity("Travelling_Application.Models.Accomodation", b =>
                {
                    b.Navigation("AllRooms");
                });

            modelBuilder.Entity("Travelling_Application.Models.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Travelling_Application.Models.Room", b =>
                {
                    b.Navigation("Photos");
                });

            modelBuilder.Entity("Travelling_Application.Models.User", b =>
                {
                    b.Navigation("BookingAccomodation");

                    b.Navigation("BookingAirTicket");

                    b.Navigation("BookingCars");

                    b.Navigation("BookingEntertainment");

                    b.Navigation("FavoriteAccomodation");

                    b.Navigation("FavoriteAirTicket");

                    b.Navigation("FavoriteCars");

                    b.Navigation("FavoriteEntertainment");
                });
#pragma warning restore 612, 618
        }
    }
}
