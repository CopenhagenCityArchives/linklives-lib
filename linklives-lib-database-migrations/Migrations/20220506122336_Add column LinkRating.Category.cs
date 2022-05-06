using Microsoft.EntityFrameworkCore.Migrations;

namespace Linklives.Migrations
{
    public partial class AddcolumnLinkRatingCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "LinkRatings",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "LinkRatings");
        }
    }
}
