using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace Linklives.Migrations
{
    public partial class Initialstructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LifeCourses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Life_course_id = table.Column<int>(type: "int", nullable: false),
                    Pa_ids = table.Column<string>(type: "text", nullable: true),
                    Source_ids = table.Column<string>(type: "text", nullable: true),
                    Link_ids = table.Column<string>(type: "text", nullable: true),
                    N_sources = table.Column<string>(type: "text", nullable: true),
                    Key = table.Column<string>(type: "Varchar(350)", nullable: true),
                    Is_historic = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Data_version = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LifeCourses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Links",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Link_id = table.Column<string>(type: "text", nullable: true),
                    Iteration = table.Column<string>(type: "text", nullable: true),
                    Iteration_inner = table.Column<string>(type: "text", nullable: true),
                    Method_id = table.Column<string>(type: "text", nullable: true),
                    Pa_id1 = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<string>(type: "text", nullable: true),
                    Pa_id2 = table.Column<int>(type: "int", nullable: false),
                    Source_id1 = table.Column<int>(type: "int", nullable: false),
                    Source_id2 = table.Column<int>(type: "int", nullable: false),
                    Duplicates = table.Column<int>(type: "int", nullable: false),
                    Key = table.Column<string>(type: "Varchar(350)", nullable: true),
                    Is_historic = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Data_version = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Links", x => x.Id);
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
                    LifeCoursesId = table.Column<int>(type: "int", nullable: false),
                    LinksId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LifeCourseLink", x => new { x.LifeCoursesId, x.LinksId });
                    table.ForeignKey(
                        name: "FK_LifeCourseLink_LifeCourses_LifeCoursesId",
                        column: x => x.LifeCoursesId,
                        principalTable: "LifeCourses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LifeCourseLink_Links_LinksId",
                        column: x => x.LinksId,
                        principalTable: "Links",
                        principalColumn: "Id",
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
                    LinkId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LinkRatings_Links_LinkId",
                        column: x => x.LinkId,
                        principalTable: "Links",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LinkRatings_RatingOptions_RatingId",
                        column: x => x.RatingId,
                        principalTable: "RatingOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LifeCourseLink_LinksId",
                table: "LifeCourseLink",
                column: "LinksId");

            migrationBuilder.CreateIndex(
                name: "IX_LifeCourses_Id",
                table: "LifeCourses",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_LifeCourses_Key",
                table: "LifeCourses",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LinkRatings_LinkId",
                table: "LinkRatings",
                column: "LinkId");

            migrationBuilder.CreateIndex(
                name: "IX_LinkRatings_RatingId",
                table: "LinkRatings",
                column: "RatingId");

            migrationBuilder.CreateIndex(
                name: "IX_Links_Id",
                table: "Links",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Links_Key",
                table: "Links",
                column: "Key",
                unique: true);
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
