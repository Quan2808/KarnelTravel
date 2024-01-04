using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    public partial class addratingresort : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ResortID",
                table: "Ratings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_ResortID",
                table: "Ratings",
                column: "ResortID");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Resorts_ResortID",
                table: "Ratings",
                column: "ResortID",
                principalTable: "Resorts",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Resorts_ResortID",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_ResortID",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "ResortID",
                table: "Ratings");
        }
    }
}
