using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasterKinder.Migrations
{
    /// <inheritdoc />
    public partial class ajk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GraderingSvarsalternativ",
                table: "SurveyResponses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ResultatkategoriKod",
                table: "SurveyResponses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TotalVarde",
                table: "SurveyResponses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalVarde_ExklVetEj",
                table: "SurveyResponses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Utfall",
                table: "SurveyResponses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GraderingSvarsalternativ",
                table: "SurveyResponses");

            migrationBuilder.DropColumn(
                name: "ResultatkategoriKod",
                table: "SurveyResponses");

            migrationBuilder.DropColumn(
                name: "TotalVarde",
                table: "SurveyResponses");

            migrationBuilder.DropColumn(
                name: "TotalVarde_ExklVetEj",
                table: "SurveyResponses");

            migrationBuilder.DropColumn(
                name: "Utfall",
                table: "SurveyResponses");
        }
    }
}
