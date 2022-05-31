using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Linklives.Migrations
{
    public partial class AddcreatedtimestampforLinkRatings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "LinkRatings",
                type: "datetime",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "LinkRatings");
        }
    }
}
