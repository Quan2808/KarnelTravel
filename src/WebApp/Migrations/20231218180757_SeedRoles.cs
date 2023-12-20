using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    public partial class SeedRoles : Migration
    {
        private string AdminRoleId = Guid.NewGuid().ToString();
        private string UserRoleId = Guid.NewGuid().ToString();
        private string AdminAccountId = Guid.NewGuid().ToString();

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            SeedRolesSQL(migrationBuilder);
            SeedAdminAccountSQL(migrationBuilder);
            SeedSetAdminAccountRoleSQL(migrationBuilder);
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

        private void SeedAdminAccountSQL(MigrationBuilder migrationBuilder)
        {
            //KarnelTravel
            string adminPasswordHash = "AQAAAAEAACcQAAAAEEBL3QAwyibQirlDE0O0iTW2+CH6WRE9Bm1/H6pRPPLKa2sB0NwUIlcLP4Onr2bINA==";

            migrationBuilder.Sql($@"Insert Into [dbo].[Users] ( [Id], [FirstName], [LastName], [Address], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [Password], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount] ) Values ( '{AdminAccountId}', 'AdminFirstName', 'AdminLastName', 'AdminAddress', 'admin', 'ADMIN', 'karneltravel@info.com', 'KARNELTRAVEL@INFO.COM', 1, '{adminPasswordHash}', '{Guid.NewGuid()}', null, '1234567890', 1, 0, null, 1, 0 ); ");
        }   

        private void SeedSetAdminAccountRoleSQL(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"Insert Into [dbo].[UserRoles] ([UserId],[RoleId]) Values ('{AdminAccountId}', '{AdminRoleId}')");
        }
    }
}
