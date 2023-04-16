using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Ordering.Migrations
{
    public partial class UpdateColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "ms_ordering",
                table: "Products",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedAt",
                schema: "ms_ordering",
                table: "Products",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "ms_ordering",
                table: "OrderItems",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedAt",
                schema: "ms_ordering",
                table: "OrderItems",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "ms_ordering",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LastUpdatedAt",
                schema: "ms_ordering",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "ms_ordering",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "LastUpdatedAt",
                schema: "ms_ordering",
                table: "OrderItems");
        }
    }
}
