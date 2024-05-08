using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Travelling_Application.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnsToDbEntertainment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Entertainment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EntertainmentPhotos",
                table: "Entertainment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<bool>(
                name: "FreeCancellation",
                table: "Entertainment",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PublisherId",
                table: "Entertainment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TimeOfDay",
                table: "Entertainment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Entertainment");

            migrationBuilder.DropColumn(
                name: "EntertainmentPhotos",
                table: "Entertainment");

            migrationBuilder.DropColumn(
                name: "FreeCancellation",
                table: "Entertainment");

            migrationBuilder.DropColumn(
                name: "PublisherId",
                table: "Entertainment");

            migrationBuilder.DropColumn(
                name: "TimeOfDay",
                table: "Entertainment");
        }
    }
}
