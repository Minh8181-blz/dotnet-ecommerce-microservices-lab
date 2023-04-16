using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Carts.Migrations
{
    public partial class AddColumnsToCartItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "ms_cart",
                table: "CartItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "StatusText",
                schema: "ms_cart",
                table: "CartItems",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                schema: "ms_cart",
                table: "CartItems",
                type: "decimal(20,5)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                schema: "ms_cart",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "StatusText",
                schema: "ms_cart",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                schema: "ms_cart",
                table: "CartItems");
        }
    }
}
