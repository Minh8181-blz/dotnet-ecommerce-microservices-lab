using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Ordering.Migrations
{
    public partial class addStatusTextFieldForOrderEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StatusText",
                schema: "ms_ordering",
                table: "Orders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusText",
                schema: "ms_ordering",
                table: "Orders");
        }
    }
}
