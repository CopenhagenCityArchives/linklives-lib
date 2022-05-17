using Microsoft.EntityFrameworkCore.Migrations;

namespace Linklives.Migrations
{
    public partial class AddRatingOptionsdefaultvalues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "RatingOptions",
                columns: new[] { "Id", "Category", "Heading", "Text" },
                values: new object[,]
                {
                    { 1, "positive", "Ja, det er troværdigt", "Det ser fornuftigt ud. Personinformationen i de to kilder passer sammen." },
                    { 2, "positive", "Ja, det er troværdigt", "Jeg kan bekræfte informationen fra andre kilder, der ikke er med i Link-Lives." },
                    { 3, "positive", "Ja, det er troværdigt", "Jeg kan genkende informationen fra min private slægtsforskning." },
                    { 4, "negative", "Nej, det er ikke troværdigt", "Det ser forkert ud. Personinformation i de to kilder passer ikke sammen." },
                    { 5, "negative", "Nej, det er ikke troværdigt", "Jeg ved det er forkert ud fra andre kilder, der ikke er med i Link-Lives." },
                    { 6, "negative", "Nej, det er ikke troværdigt", "Jeg ved det er forkert fra min private slægtsforskning." },
                    { 7, "neutral", "Måske", "Jeg er i tvivl om personinformationen i de to kilder passer sammen." },
                    { 8, "neutral", "Måske", "Nogle af informationerne passer sammen. Andre gør ikke." },
                    { 9, "neutral", "Måske", "Der er ikke personinformation nok til at afgøre, om det er troværdigt." }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RatingOptions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "RatingOptions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "RatingOptions",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "RatingOptions",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "RatingOptions",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "RatingOptions",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "RatingOptions",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "RatingOptions",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "RatingOptions",
                keyColumn: "Id",
                keyValue: 9);
        }
    }
}
