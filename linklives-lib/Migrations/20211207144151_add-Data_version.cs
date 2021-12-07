using Microsoft.EntityFrameworkCore.Migrations;

namespace Linklives.Migrations
{
    public partial class addData_version : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Data_version",
                table: "Links",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Is_historic",
                table: "Links",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Data_version",
                table: "LifeCourses",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Data_version",
                table: "Links");

            migrationBuilder.DropColumn(
                name: "Is_historic",
                table: "Links");

            migrationBuilder.DropColumn(
                name: "Data_version",
                table: "LifeCourses");
        }
    }
}
