using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace linklives_api_dal.Migrations
{
    public partial class initialcreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LifeCourses",
                columns: table => new
                {
                    Key = table.Column<string>(type: "varchar(767)", nullable: false),
                    Life_course_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LifeCourses", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "RatingOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Text = table.Column<string>(type: "text", nullable: true),
                    Heading = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RatingOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Links",
                columns: table => new
                {
                    Key = table.Column<string>(type: "varchar(767)", nullable: false),
                    Link_id = table.Column<int>(type: "int", nullable: false),
                    Iteration = table.Column<int>(type: "int", nullable: false),
                    Iteration_inner = table.Column<int>(type: "int", nullable: false),
                    Method_id = table.Column<int>(type: "int", nullable: false),
                    Pa_id1 = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<double>(type: "double", nullable: false),
                    Pa_id2 = table.Column<int>(type: "int", nullable: false),
                    Source_id1 = table.Column<int>(type: "int", nullable: false),
                    Source_id2 = table.Column<int>(type: "int", nullable: false),
                    Method_type = table.Column<string>(type: "text", nullable: true),
                    Method_subtype1 = table.Column<string>(type: "text", nullable: true),
                    Method_description = table.Column<string>(type: "text", nullable: true),
                    LifeCourseKey = table.Column<string>(type: "varchar(767)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Links", x => x.Key);
                    table.ForeignKey(
                        name: "FK_Links_LifeCourses_LifeCourseKey",
                        column: x => x.LifeCourseKey,
                        principalTable: "LifeCourses",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LinkRatings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    RatingId = table.Column<int>(type: "int", nullable: false),
                    LinkKey = table.Column<string>(type: "varchar(767)", nullable: false),
                    User = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LinkRatings_Links_LinkKey",
                        column: x => x.LinkKey,
                        principalTable: "Links",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LinkRatings_RatingOptions_RatingId",
                        column: x => x.RatingId,
                        principalTable: "RatingOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LinkRatings_LinkKey",
                table: "LinkRatings",
                column: "LinkKey");

            migrationBuilder.CreateIndex(
                name: "IX_LinkRatings_RatingId",
                table: "LinkRatings",
                column: "RatingId");

            migrationBuilder.CreateIndex(
                name: "IX_Links_LifeCourseKey",
                table: "Links",
                column: "LifeCourseKey");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LinkRatings");

            migrationBuilder.DropTable(
                name: "Links");

            migrationBuilder.DropTable(
                name: "RatingOptions");

            migrationBuilder.DropTable(
                name: "LifeCourses");
        }
    }
}
