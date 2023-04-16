using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Catalog.Migrations
{
    public partial class UpdateProductTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "ms_catalog",
                table: "StockItemRecords",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedAt",
                schema: "ms_catalog",
                table: "StockItemRecords",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "ms_catalog",
                table: "Products",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedAt",
                schema: "ms_catalog",
                table: "Products",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "ms_catalog",
                table: "StockItemRecords");

            migrationBuilder.DropColumn(
                name: "LastUpdatedAt",
                schema: "ms_catalog",
                table: "StockItemRecords");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "ms_catalog",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LastUpdatedAt",
                schema: "ms_catalog",
                table: "Products");
        }
    }
}
