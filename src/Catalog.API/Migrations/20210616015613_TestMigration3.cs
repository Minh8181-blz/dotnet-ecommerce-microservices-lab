using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Catalog.Migrations
{
    public partial class TestMigration3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Xyz",
                schema: "ms_catalog",
                table: "TestEnts",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Xyz",
                schema: "ms_catalog",
                table: "TestEnts",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
