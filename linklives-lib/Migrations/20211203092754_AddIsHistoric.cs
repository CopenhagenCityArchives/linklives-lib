using Microsoft.EntityFrameworkCore.Migrations;

namespace Linklives.Migrations
{
    public partial class AddIsHistoric : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Is_historic",
                table: "LifeCourses",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Is_historic",
                table: "LifeCourses");
        }
    }
}
