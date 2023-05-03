using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StandardWebApiTemplate.Migrations
{
    /// <inheritdoc />
    public partial class AddDescriptionCostEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Costs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Costs",
                keyColumn: "Id",
                keyValue: new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3"),
                column: "Description",
                value: "Admin Fee");

            migrationBuilder.UpdateData(
                table: "Costs",
                keyColumn: "Id",
                keyValue: new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"),
                column: "Description",
                value: "Trasnportation");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Costs");
        }
    }
}
