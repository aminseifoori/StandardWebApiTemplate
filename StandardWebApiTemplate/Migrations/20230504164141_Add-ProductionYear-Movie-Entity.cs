using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StandardWebApiTemplate.Migrations
{
    /// <inheritdoc />
    public partial class AddProductionYearMovieEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductionYear",
                table: "Movies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: new Guid("80abbca8-664d-4b20-b5de-024705497d4a"),
                column: "ProductionYear",
                value: 1997);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductionYear",
                table: "Movies");
        }
    }
}
