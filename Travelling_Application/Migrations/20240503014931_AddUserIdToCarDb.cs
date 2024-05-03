using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Travelling_Application.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToCarDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Entertainment",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "Entertainment",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PublisherId",
                table: "Car",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Car",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "Car",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "AirTicket",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "AirTicket",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Accomodation",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "Accomodation",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Entertainment_UserId",
                table: "Entertainment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Entertainment_UserId1",
                table: "Entertainment",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Car_UserId",
                table: "Car",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Car_UserId1",
                table: "Car",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_AirTicket_UserId",
                table: "AirTicket",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AirTicket_UserId1",
                table: "AirTicket",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Accomodation_UserId",
                table: "Accomodation",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Accomodation_UserId1",
                table: "Accomodation",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Accomodation_Users_UserId",
                table: "Accomodation",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Accomodation_Users_UserId1",
                table: "Accomodation",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AirTicket_Users_UserId",
                table: "AirTicket",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AirTicket_Users_UserId1",
                table: "AirTicket",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Car_Users_UserId",
                table: "Car",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Car_Users_UserId1",
                table: "Car",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Entertainment_Users_UserId",
                table: "Entertainment",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Entertainment_Users_UserId1",
                table: "Entertainment",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accomodation_Users_UserId",
                table: "Accomodation");

            migrationBuilder.DropForeignKey(
                name: "FK_Accomodation_Users_UserId1",
                table: "Accomodation");

            migrationBuilder.DropForeignKey(
                name: "FK_AirTicket_Users_UserId",
                table: "AirTicket");

            migrationBuilder.DropForeignKey(
                name: "FK_AirTicket_Users_UserId1",
                table: "AirTicket");

            migrationBuilder.DropForeignKey(
                name: "FK_Car_Users_UserId",
                table: "Car");

            migrationBuilder.DropForeignKey(
                name: "FK_Car_Users_UserId1",
                table: "Car");

            migrationBuilder.DropForeignKey(
                name: "FK_Entertainment_Users_UserId",
                table: "Entertainment");

            migrationBuilder.DropForeignKey(
                name: "FK_Entertainment_Users_UserId1",
                table: "Entertainment");

            migrationBuilder.DropIndex(
                name: "IX_Entertainment_UserId",
                table: "Entertainment");

            migrationBuilder.DropIndex(
                name: "IX_Entertainment_UserId1",
                table: "Entertainment");

            migrationBuilder.DropIndex(
                name: "IX_Car_UserId",
                table: "Car");

            migrationBuilder.DropIndex(
                name: "IX_Car_UserId1",
                table: "Car");

            migrationBuilder.DropIndex(
                name: "IX_AirTicket_UserId",
                table: "AirTicket");

            migrationBuilder.DropIndex(
                name: "IX_AirTicket_UserId1",
                table: "AirTicket");

            migrationBuilder.DropIndex(
                name: "IX_Accomodation_UserId",
                table: "Accomodation");

            migrationBuilder.DropIndex(
                name: "IX_Accomodation_UserId1",
                table: "Accomodation");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Entertainment");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Entertainment");

            migrationBuilder.DropColumn(
                name: "PublisherId",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AirTicket");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "AirTicket");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Accomodation");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Accomodation");
        }
    }
}
