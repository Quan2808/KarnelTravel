using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    public partial class addratingresttravel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RestaurantID",
                table: "Ratings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TravelInfoID",
                table: "Ratings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_RestaurantID",
                table: "Ratings",
                column: "RestaurantID");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_TravelInfoID",
                table: "Ratings",
                column: "TravelInfoID");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Restaurants_RestaurantID",
                table: "Ratings",
                column: "RestaurantID",
                principalTable: "Restaurants",
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
                name: "FK_Ratings_Restaurants_RestaurantID",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Travels_TravelInfoID",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_RestaurantID",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_TravelInfoID",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "RestaurantID",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "TravelInfoID",
                table: "Ratings");
        }
    }
}
