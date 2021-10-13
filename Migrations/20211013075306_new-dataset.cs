using Microsoft.EntityFrameworkCore.Migrations;

namespace linklives_api_dal.Migrations
{
    public partial class newdataset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Method_description",
                table: "Links");

            migrationBuilder.DropColumn(
                name: "Method_subtype1",
                table: "Links");

            migrationBuilder.DropColumn(
                name: "Method_type",
                table: "Links");

            migrationBuilder.AlterColumn<string>(
                name: "Score",
                table: "Links",
                type: "text",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double");

            migrationBuilder.AlterColumn<string>(
                name: "Method_id",
                table: "Links",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Iteration_inner",
                table: "Links",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Iteration",
                table: "Links",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Link_ids",
                table: "LifeCourses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "N_sources",
                table: "LifeCourses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pa_ids",
                table: "LifeCourses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Source_ids",
                table: "LifeCourses",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Link_ids",
                table: "LifeCourses");

            migrationBuilder.DropColumn(
                name: "N_sources",
                table: "LifeCourses");

            migrationBuilder.DropColumn(
                name: "Pa_ids",
                table: "LifeCourses");

            migrationBuilder.DropColumn(
                name: "Source_ids",
                table: "LifeCourses");

            migrationBuilder.AlterColumn<double>(
                name: "Score",
                table: "Links",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Method_id",
                table: "Links",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Iteration_inner",
                table: "Links",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Iteration",
                table: "Links",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Method_description",
                table: "Links",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Method_subtype1",
                table: "Links",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Method_type",
                table: "Links",
                type: "text",
                nullable: true);
        }
    }
}
