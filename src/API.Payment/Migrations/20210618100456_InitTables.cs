using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Payment.Migrations
{
    public partial class InitTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ms_payment");

            migrationBuilder.CreateTable(
                name: "IntegrationEventLog",
                schema: "ms_payment",
                columns: table => new
                {
                    EventId = table.Column<Guid>(nullable: false),
                    EventTypeName = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    EventCreationDate = table.Column<DateTime>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    LastModifiedAt = table.Column<DateTime>(nullable: false),
                    Content = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegrationEventLog", x => x.EventId);
                });

            migrationBuilder.CreateTable(
                name: "PaymentStripeReferences",
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
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentStripeReferences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequestEntry",
                schema: "ms_payment",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Time = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestEntry", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentOperations",
                schema: "ms_payment",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    MethodId = table.Column<int>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    Purpose = table.Column<int>(nullable: false),
                    OrderId = table.Column<int>(nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(20,5)", nullable: false),
                    AmountRefunded = table.Column<decimal>(type: "decimal(20,5)", nullable: true),
                    Status = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(nullable: false),
                    StripeReferenceId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentOperations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentOperations_PaymentStripeReferences_StripeReferenceId",
                        column: x => x.StripeReferenceId,
                        principalSchema: "ms_payment",
                        principalTable: "PaymentStripeReferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentOperations_StripeReferenceId",
                schema: "ms_payment",
                table: "PaymentOperations",
                column: "StripeReferenceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IntegrationEventLog",
                schema: "ms_payment");

            migrationBuilder.DropTable(
                name: "PaymentOperations",
                schema: "ms_payment");

            migrationBuilder.DropTable(
                name: "RequestEntry",
                schema: "ms_payment");

            migrationBuilder.DropTable(
                name: "PaymentStripeReferences",
                schema: "ms_payment");
        }
    }
}
