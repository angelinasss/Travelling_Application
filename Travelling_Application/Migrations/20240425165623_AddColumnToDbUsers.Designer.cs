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
    [Migration("20240425165623_AddColumnToDbUsers")]
    partial class AddColumnToDbUsers
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

                    b.Property<bool>("AudioPrompts")
                        .HasColumnType("bit");

                    b.Property<bool>("Balcony")
                        .HasColumnType("bit");

                    b.Property<bool>("Bar")
                        .HasColumnType("bit");

                    b.Property<bool>("BathroomEmergencyButton")
                        .HasColumnType("bit");

                    b.Property<bool>("BraillePrompts")
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

                    b.Property<bool>("HighToilet")
                        .HasColumnType("bit");

                    b.Property<bool>("Kitchen")
                        .HasColumnType("bit");

                    b.Property<int>("LineOfBeach")
                        .HasColumnType("int");

                    b.Property<bool>("LowSink")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("NoSmookingRooms")
                        .HasColumnType("bit");

                    b.Property<bool>("Parking")
                        .HasColumnType("bit");

                    b.Property<bool>("PetsAllowed")
                        .HasColumnType("bit");

                    b.Property<bool>("PrivateBeach")
                        .HasColumnType("bit");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<int>("Restaurants")
                        .HasColumnType("int");

                    b.Property<bool>("SPA")
                        .HasColumnType("bit");

                    b.Property<bool>("SeaView")
                        .HasColumnType("bit");

                    b.Property<bool>("SmookingRooms")
                        .HasColumnType("bit");

                    b.Property<int>("SwimmingPool")
                        .HasColumnType("int");

                    b.Property<bool>("TactileSigns")
                        .HasColumnType("bit");

                    b.Property<bool>("Terrace")
                        .HasColumnType("bit");

                    b.Property<bool>("ToiletWithGrabBars")
                        .HasColumnType("bit");

                    b.Property<bool>("TransferToAirport")
                        .HasColumnType("bit");

                    b.Property<string>("TypeOfAccomodation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TypesOfNutrition")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("WheelchairAccessible")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Accomodation");
                });

            modelBuilder.Entity("Travelling_Application.Models.Photo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AccomodationId")
                        .HasColumnType("int");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("RoomID")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AccomodationId");

                    b.HasIndex("RoomID");

                    b.ToTable("Photos");
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

                    b.Property<int?>("AccomodationId")
                        .HasColumnType("int");

                    b.Property<string>("AccomodationName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("AirConditioner")
                        .HasColumnType("bit");

                    b.Property<bool>("AllInclusive")
                        .HasColumnType("bit");

                    b.Property<string>("AvailableDates")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Balcony")
                        .HasColumnType("bit");

                    b.Property<bool>("BarrierFreeShower")
                        .HasColumnType("bit");

                    b.Property<bool>("Bath")
                        .HasColumnType("bit");

                    b.Property<bool>("Bathroom")
                        .HasColumnType("bit");

                    b.Property<bool>("BathroomEmergencyButton")
                        .HasColumnType("bit");

                    b.Property<bool>("BathtubWithgrabbars")
                        .HasColumnType("bit");

                    b.Property<bool>("BreakfastAndDinnerIncluded")
                        .HasColumnType("bit");

                    b.Property<bool>("BreakfastIncluded")
                        .HasColumnType("bit");

                    b.Property<bool>("CityView")
                        .HasColumnType("bit");

                    b.Property<bool>("CoffeeMachine")
                        .HasColumnType("bit");

                    b.Property<bool>("CoffeeOrTea")
                        .HasColumnType("bit");

                    b.Property<double>("Cost")
                        .HasColumnType("float");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("ElectricKettle")
                        .HasColumnType("bit");

                    b.Property<bool>("FlatScreenTV")
                        .HasColumnType("bit");

                    b.Property<bool>("GardenView")
                        .HasColumnType("bit");

                    b.Property<bool>("HighToilet")
                        .HasColumnType("bit");

                    b.Property<bool>("Kitchen")
                        .HasColumnType("bit");

                    b.Property<bool>("LowSink")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Patio")
                        .HasColumnType("bit");

                    b.Property<bool>("PlaceToWorkOnALaptop")
                        .HasColumnType("bit");

                    b.Property<bool>("PrivateBathroom")
                        .HasColumnType("bit");

                    b.Property<bool>("PrivatePool")
                        .HasColumnType("bit");

                    b.Property<bool>("SeaView")
                        .HasColumnType("bit");

                    b.Property<bool>("ShowerChair")
                        .HasColumnType("bit");

                    b.Property<bool>("ShowerWithoutEdge")
                        .HasColumnType("bit");

                    b.Property<bool>("Soundproofing")
                        .HasColumnType("bit");

                    b.Property<bool>("Terrace")
                        .HasColumnType("bit");

                    b.Property<bool>("ThreeMealsADay")
                        .HasColumnType("bit");

                    b.Property<bool>("ToiletWithGrabBars")
                        .HasColumnType("bit");

                    b.Property<bool>("ViewFromTheWindow")
                        .HasColumnType("bit");

                    b.Property<bool>("WashingMachine")
                        .HasColumnType("bit");

                    b.Property<bool>("WheelchairAccessible")
                        .HasColumnType("bit");

                    b.HasKey("ID");

                    b.HasIndex("AccomodationId");

                    b.ToTable("Rooms");
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

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Travelling_Application.Models.Photo", b =>
                {
                    b.HasOne("Travelling_Application.Models.Accomodation", null)
                        .WithMany("Photos")
                        .HasForeignKey("AccomodationId");

                    b.HasOne("Travelling_Application.Models.Room", null)
                        .WithMany("Photos")
                        .HasForeignKey("RoomID");
                });

            modelBuilder.Entity("Travelling_Application.Models.Room", b =>
                {
                    b.HasOne("Travelling_Application.Models.Accomodation", null)
                        .WithMany("Rooms")
                        .HasForeignKey("AccomodationId");
                });

            modelBuilder.Entity("Travelling_Application.Models.User", b =>
                {
                    b.HasOne("Travelling_Application.Models.Role", null)
                        .WithMany("Users")
                        .HasForeignKey("RoleId");
                });

            modelBuilder.Entity("Travelling_Application.Models.Accomodation", b =>
                {
                    b.Navigation("Photos");

                    b.Navigation("Rooms");
                });

            modelBuilder.Entity("Travelling_Application.Models.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Travelling_Application.Models.Room", b =>
                {
                    b.Navigation("Photos");
                });
#pragma warning restore 612, 618
        }
    }
}