using Microsoft.EntityFrameworkCore.Migrations;

namespace Linklives.Migrations
{
    public partial class MoveLinkRatingCategorytoLinkRatingOptionsCategoryandchangetypetoenum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "LinkRatings");

            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "RatingOptions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "RatingOptions");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "LinkRatings",
                type: "text",
                nullable: true);
        }
    }
}
