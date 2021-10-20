using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace Linklives.Migrations
{
    public partial class initialcreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LifeCourses",
                columns: table => new
                {
                    Key = table.Column<string>(type: "Varchar(350)", nullable: false),
                    Life_course_id = table.Column<int>(type: "int", nullable: false),
                    Pa_ids = table.Column<string>(type: "text", nullable: true),
                    Source_ids = table.Column<string>(type: "text", nullable: true),
                    Link_ids = table.Column<string>(type: "text", nullable: true),
                    N_sources = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LifeCourses", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "Links",
                columns: table => new
                {
                    Key = table.Column<string>(type: "Varchar(350)", nullable: false),
                    Link_id = table.Column<int>(type: "int", nullable: false),
                    Iteration = table.Column<string>(type: "text", nullable: true),
                    Iteration_inner = table.Column<string>(type: "text", nullable: true),
                    Method_id = table.Column<string>(type: "text", nullable: true),
                    Pa_id1 = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<string>(type: "text", nullable: true),
                    Pa_id2 = table.Column<int>(type: "int", nullable: false),
                    Source_id1 = table.Column<int>(type: "int", nullable: false),
                    Source_id2 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Links", x => x.Key);
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
                name: "LifeCourseLink",
                columns: table => new
                {
                    LifeCoursesKey = table.Column<string>(type: "Varchar(350)", nullable: false),
                    LinksKey = table.Column<string>(type: "Varchar(350)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LifeCourseLink", x => new { x.LifeCoursesKey, x.LinksKey });
                    table.ForeignKey(
                        name: "FK_LifeCourseLink_LifeCourses_LifeCoursesKey",
                        column: x => x.LifeCoursesKey,
                        principalTable: "LifeCourses",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LifeCourseLink_Links_LinksKey",
                        column: x => x.LinksKey,
                        principalTable: "Links",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LinkRatings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    User = table.Column<string>(type: "text", nullable: false),
                    RatingId = table.Column<int>(type: "int", nullable: false),
                    LinkKey = table.Column<string>(type: "Varchar(350)", nullable: false)
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
                name: "IX_LifeCourseLink_LinksKey",
                table: "LifeCourseLink",
                column: "LinksKey");

            migrationBuilder.CreateIndex(
                name: "IX_LinkRatings_LinkKey",
                table: "LinkRatings",
                column: "LinkKey");

            migrationBuilder.CreateIndex(
                name: "IX_LinkRatings_RatingId",
                table: "LinkRatings",
                column: "RatingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LifeCourseLink");

            migrationBuilder.DropTable(
                name: "LinkRatings");

            migrationBuilder.DropTable(
                name: "LifeCourses");

            migrationBuilder.DropTable(
                name: "Links");

            migrationBuilder.DropTable(
                name: "RatingOptions");
        }
    }
}
