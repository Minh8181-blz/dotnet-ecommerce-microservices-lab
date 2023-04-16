using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Payment.Migrations
{
    public partial class ModifyPaymentTableColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentOperations_PaymentStripeReferences_StripeReferenceId",
                schema: "ms_payment",
                table: "PaymentOperations");

            migrationBuilder.DropTable(
                name: "PaymentStripeReferences",
                schema: "ms_payment");

            migrationBuilder.DropIndex(
                name: "IX_PaymentOperations_StripeReferenceId",
                schema: "ms_payment",
                table: "PaymentOperations");

            migrationBuilder.DropColumn(
                name: "StripeReferenceId",
                schema: "ms_payment",
                table: "PaymentOperations");

            migrationBuilder.CreateTable(
                name: "PaymentStripeSessions",
                schema: "ms_payment",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    PaymentId = table.Column<Guid>(nullable: false),
                    SessionId = table.Column<string>(nullable: true),
                    ClientReference = table.Column<string>(nullable: true),
                    SuccessUrl = table.Column<string>(nullable: true),
                    CancelUrl = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(nullable: false),
                    PaymentOperationId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentStripeSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentStripeSessions_PaymentOperations_PaymentOperationId",
                        column: x => x.PaymentOperationId,
                        principalSchema: "ms_payment",
                        principalTable: "PaymentOperations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentStripeSessions_PaymentOperationId",
                schema: "ms_payment",
                table: "PaymentStripeSessions",
                column: "PaymentOperationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentStripeSessions",
                schema: "ms_payment");

            migrationBuilder.AddColumn<Guid>(
                name: "StripeReferenceId",
                schema: "ms_payment",
                table: "PaymentOperations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PaymentStripeReferences",
                schema: "ms_payment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CancelUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    SessionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuccessUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentStripeReferences", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentOperations_StripeReferenceId",
                schema: "ms_payment",
                table: "PaymentOperations",
                column: "StripeReferenceId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentOperations_PaymentStripeReferences_StripeReferenceId",
                schema: "ms_payment",
                table: "PaymentOperations",
                column: "StripeReferenceId",
                principalSchema: "ms_payment",
                principalTable: "PaymentStripeReferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
