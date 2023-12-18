using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    public partial class SeedRoles : Migration
    {
        private string AdminRoleId = Guid.NewGuid().ToString();
        private string UserRoleId = Guid.NewGuid().ToString();

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            SeedRolesSQL(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"DELETE FROM [dbo].[Roles] WHERE [Id] = '{AdminRoleId}';");
            migrationBuilder.Sql($"DELETE FROM [dbo].[Roles] WHERE [Id] = '{UserRoleId}';");
        }

        private void SeedRolesSQL(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@$"Insert Into [dbo].[Roles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) Values ('{AdminRoleId}', 'Admin', 'ADMIN', null);");

            migrationBuilder.Sql(@$"Insert Into [dbo].[Roles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) Values ('{UserRoleId}', 'User', 'USER', null);");
        }
    }
}
