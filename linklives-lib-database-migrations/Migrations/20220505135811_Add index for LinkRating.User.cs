using Microsoft.EntityFrameworkCore.Migrations;

namespace Linklives.Migrations
{
    public partial class AddindexforLinkRatingUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "User",
                table: "LinkRatings",
                type: "varchar(767)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_LinkRatings_User",
                table: "LinkRatings",
                column: "User");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LinkRatings_User",
                table: "LinkRatings");

            migrationBuilder.AlterColumn<string>(
                name: "User",
                table: "LinkRatings",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(767)");
        }
    }
}
