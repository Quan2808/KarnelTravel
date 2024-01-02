using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    public partial class v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
               name: "Ratings",
               columns: table => new
               {
                   ID = table.Column<int>(type: "int", nullable: false)
                       .Annotation("SqlServer:Identity", "1, 1"),
                   BookingID = table.Column<int>(type: "int", nullable: false),
                   Comment = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: true),
                   CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                   CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                   CustomerPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                   Value = table.Column<int>(type: "int", nullable: false)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_Ratings", x => x.ID);
                   table.ForeignKey(
                       name: "FK_Ratings_Bookings_BookingID",
                       column: x => x.BookingID,
                       principalTable: "Bookings",
                       principalColumn: "ID",
                       onDelete: ReferentialAction.Cascade);
               });

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_BookingID",
                table: "Ratings",
                column: "BookingID");

            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ratings");
        }
    }
}
