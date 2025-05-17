using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TallerIdwm.src.data.Migrations
{
    /// <inheritdoc />
    public partial class Migrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PublicId",
                table: "Products",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "BirthDate",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "AspNetUsers");
        }
    }
}
