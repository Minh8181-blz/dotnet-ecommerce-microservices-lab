using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Carts.Migrations
{
    public partial class AddColumnToCartItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Carts_CartId",
                schema: "ms_cart",
                table: "CartItems");

            migrationBuilder.AlterColumn<int>(
                name: "CartId",
                schema: "ms_cart",
                table: "CartItems",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Carts_CartId",
                schema: "ms_cart",
                table: "CartItems",
                column: "CartId",
                principalSchema: "ms_cart",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Carts_CartId",
                schema: "ms_cart",
                table: "CartItems");

            migrationBuilder.AlterColumn<int>(
                name: "CartId",
                schema: "ms_cart",
                table: "CartItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Carts_CartId",
                schema: "ms_cart",
                table: "CartItems",
                column: "CartId",
                principalSchema: "ms_cart",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
