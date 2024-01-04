using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    public partial class v5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Tourists",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ResortID",
                table: "Ratings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RestaurantID",
                table: "Ratings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TouristSpotID",
                table: "Ratings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TravelInfoID",
                table: "Ratings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_ResortID",
                table: "Ratings",
                column: "ResortID");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_RestaurantID",
                table: "Ratings",
                column: "RestaurantID");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_TouristSpotID",
                table: "Ratings",
                column: "TouristSpotID");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_TravelInfoID",
                table: "Ratings",
                column: "TravelInfoID");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Resorts_ResortID",
                table: "Ratings",
                column: "ResortID",
                principalTable: "Resorts",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Restaurants_RestaurantID",
                table: "Ratings",
                column: "RestaurantID",
                principalTable: "Restaurants",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Tourists_TouristSpotID",
                table: "Ratings",
                column: "TouristSpotID",
                principalTable: "Tourists",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Travels_TravelInfoID",
                table: "Ratings",
                column: "TravelInfoID",
                principalTable: "Travels",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Resorts_ResortID",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Restaurants_RestaurantID",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Tourists_TouristSpotID",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Travels_TravelInfoID",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_ResortID",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_RestaurantID",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_TouristSpotID",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_TravelInfoID",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Tourists");

            migrationBuilder.DropColumn(
                name: "ResortID",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "RestaurantID",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "TouristSpotID",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "TravelInfoID",
                table: "Ratings");
        }
    }
}
