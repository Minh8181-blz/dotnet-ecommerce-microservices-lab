using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Payment.Migrations
{
    public partial class ModifyStripePaymentTableColumns1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentStripeSessions_PaymentOperations_PaymentOperationId",
                schema: "ms_payment",
                table: "PaymentStripeSessions");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                schema: "ms_payment",
                table: "PaymentStripeSessions");

            migrationBuilder.AlterColumn<Guid>(
                name: "PaymentOperationId",
                schema: "ms_payment",
                table: "PaymentStripeSessions",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentStripeSessions_PaymentOperations_PaymentOperationId",
                schema: "ms_payment",
                table: "PaymentStripeSessions",
                column: "PaymentOperationId",
                principalSchema: "ms_payment",
                principalTable: "PaymentOperations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentStripeSessions_PaymentOperations_PaymentOperationId",
                schema: "ms_payment",
                table: "PaymentStripeSessions");

            migrationBuilder.AlterColumn<Guid>(
                name: "PaymentOperationId",
                schema: "ms_payment",
                table: "PaymentStripeSessions",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<Guid>(
                name: "PaymentId",
                schema: "ms_payment",
                table: "PaymentStripeSessions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentStripeSessions_PaymentOperations_PaymentOperationId",
                schema: "ms_payment",
                table: "PaymentStripeSessions",
                column: "PaymentOperationId",
                principalSchema: "ms_payment",
                principalTable: "PaymentOperations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
