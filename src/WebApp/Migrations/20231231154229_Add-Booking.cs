using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    public partial class AddBooking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tourists_Hotels_HotelID",
                table: "Tourists");

            migrationBuilder.DropForeignKey(
                name: "FK_Tourists_Resorts_ResortID",
                table: "Tourists");

            migrationBuilder.DropForeignKey(
                name: "FK_Tourists_Restaurants_RestaurantID",
                table: "Tourists");

            migrationBuilder.AlterColumn<int>(
                name: "RestaurantID",
                table: "Tourists",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ResortID",
                table: "Tourists",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "HotelID",
                table: "Tourists",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TravelInfoID = table.Column<int>(type: "int", nullable: true),
                    TouristSpotID = table.Column<int>(type: "int", nullable: true),
                    HotelID = table.Column<int>(type: "int", nullable: true),
                    ResortID = table.Column<int>(type: "int", nullable: true),
                    RestaurantID = table.Column<int>(type: "int", nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CheckIn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckOut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalPrice = table.Column<float>(type: "real", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Bookings_Hotels_HotelID",
                        column: x => x.HotelID,
                        principalTable: "Hotels",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Bookings_Resorts_ResortID",
                        column: x => x.ResortID,
                        principalTable: "Resorts",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Bookings_Restaurants_RestaurantID",
                        column: x => x.RestaurantID,
                        principalTable: "Restaurants",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Bookings_Tourists_TouristSpotID",
                        column: x => x.TouristSpotID,
                        principalTable: "Tourists",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Bookings_Travels_TravelInfoID",
                        column: x => x.TravelInfoID,
                        principalTable: "Travels",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_HotelID",
                table: "Bookings",
                column: "HotelID");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ResortID",
                table: "Bookings",
                column: "ResortID");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_RestaurantID",
                table: "Bookings",
                column: "RestaurantID");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_TouristSpotID",
                table: "Bookings",
                column: "TouristSpotID");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_TravelInfoID",
                table: "Bookings",
                column: "TravelInfoID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tourists_Hotels_HotelID",
                table: "Tourists",
                column: "HotelID",
                principalTable: "Hotels",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tourists_Resorts_ResortID",
                table: "Tourists",
                column: "ResortID",
                principalTable: "Resorts",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tourists_Restaurants_RestaurantID",
                table: "Tourists",
                column: "RestaurantID",
                principalTable: "Restaurants",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tourists_Hotels_HotelID",
                table: "Tourists");

            migrationBuilder.DropForeignKey(
                name: "FK_Tourists_Resorts_ResortID",
                table: "Tourists");

            migrationBuilder.DropForeignKey(
                name: "FK_Tourists_Restaurants_RestaurantID",
                table: "Tourists");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.AlterColumn<int>(
                name: "RestaurantID",
                table: "Tourists",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ResortID",
                table: "Tourists",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "HotelID",
                table: "Tourists",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tourists_Hotels_HotelID",
                table: "Tourists",
                column: "HotelID",
                principalTable: "Hotels",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tourists_Resorts_ResortID",
                table: "Tourists",
                column: "ResortID",
                principalTable: "Resorts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tourists_Restaurants_RestaurantID",
                table: "Tourists",
                column: "RestaurantID",
                principalTable: "Restaurants",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
