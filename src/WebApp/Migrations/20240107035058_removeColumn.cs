using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    public partial class removeColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartingTime",
                table: "Travels");

            migrationBuilder.DropColumn(
                name: "EndingTime",
                table: "Travels");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
