using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Ordering.Migrations
{
    public partial class AddProductNameColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "ms_ordering",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                schema: "ms_ordering",
                table: "OrderItems",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SubTotal",
                schema: "ms_ordering",
                table: "OrderItems",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                schema: "ms_ordering",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductName",
                schema: "ms_ordering",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "SubTotal",
                schema: "ms_ordering",
                table: "OrderItems");
        }
    }
}
