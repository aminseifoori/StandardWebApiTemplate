using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StandardWebApiTemplate.Migrations
{
    /// <inheritdoc />
    public partial class SeedRoleEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "179e7727-336d-488e-a2ae-abb14e59bd1e", null, "Admin", "ADMIN" },
                    { "9823fbc7-16cb-4130-8a79-eb8ead05ed59", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "179e7727-336d-488e-a2ae-abb14e59bd1e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9823fbc7-16cb-4130-8a79-eb8ead05ed59");
        }
    }
}
