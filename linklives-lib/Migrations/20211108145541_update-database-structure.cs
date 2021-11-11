using Microsoft.EntityFrameworkCore.Migrations;

namespace Linklives.Migrations
{
    public partial class updatedatabasestructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Link_id",
                table: "Links",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Duplicates",
                table: "Links",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duplicates",
                table: "Links");

            migrationBuilder.AlterColumn<int>(
                name: "Link_id",
                table: "Links",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
