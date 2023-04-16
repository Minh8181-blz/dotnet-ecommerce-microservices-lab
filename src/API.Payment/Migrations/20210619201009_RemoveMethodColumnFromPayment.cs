using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Payment.Migrations
{
    public partial class RemoveMethodColumnFromPayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MethodId",
                schema: "ms_payment",
                table: "PaymentOperations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MethodId",
                schema: "ms_payment",
                table: "PaymentOperations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
