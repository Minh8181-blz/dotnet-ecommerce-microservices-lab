using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Catalog.Migrations
{
    public partial class TestMigration1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Xyz",
                schema: "ms_catalog",
                table: "TestEnts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Abc",
                schema: "ms_catalog",
                table: "TestEnts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Xyz",
                schema: "ms_catalog",
                table: "TestEnts");

            migrationBuilder.DropColumn(
                name: "Abc",
                schema: "ms_catalog",
                table: "TestEnts");
        }
    }
}
