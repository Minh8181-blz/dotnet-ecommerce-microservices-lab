using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Payment.Migrations
{
    public partial class ModifyTableColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Purpose",
                schema: "ms_payment",
                table: "PaymentOperations");

            migrationBuilder.AddColumn<int>(
                name: "PurposeId",
                schema: "ms_payment",
                table: "PaymentOperations",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PurposeId",
                schema: "ms_payment",
                table: "PaymentOperations");

            migrationBuilder.AddColumn<int>(
                name: "Purpose",
                schema: "ms_payment",
                table: "PaymentOperations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
