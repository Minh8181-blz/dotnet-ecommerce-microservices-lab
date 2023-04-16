using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Payment.Migrations
{
    public partial class AddPaymentOperationStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StatusText",
                schema: "ms_payment",
                table: "PaymentOperations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusText",
                schema: "ms_payment",
                table: "PaymentOperations");
        }
    }
}
