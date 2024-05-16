using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasterKinder.Migrations
{
    /// <inheritdoc />
    public partial class lolboll : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AntalSvarsalternativ",
                table: "SurveyResponses");

            migrationBuilder.DropColumn(
                name: "Enkatroll",
                table: "SurveyResponses");

            migrationBuilder.DropColumn(
                name: "Forskoleverksamhet",
                table: "SurveyResponses");

            migrationBuilder.DropColumn(
                name: "FragaNr",
                table: "SurveyResponses");

            migrationBuilder.DropColumn(
                name: "FragaomradeNr",
                table: "SurveyResponses");

            migrationBuilder.DropColumn(
                name: "Fragcategory",
                table: "SurveyResponses");

            migrationBuilder.DropColumn(
                name: "Fragetype",
                table: "SurveyResponses");

            migrationBuilder.DropColumn(
                name: "GraderingSvarsalternativ",
                table: "SurveyResponses");

            migrationBuilder.DropColumn(
                name: "Kortfragetext",
                table: "SurveyResponses");

            migrationBuilder.DropColumn(
                name: "Organisatoriskenhetskod",
                table: "SurveyResponses");

            migrationBuilder.DropColumn(
                name: "RegiformNamn",
                table: "SurveyResponses");

            migrationBuilder.DropColumn(
                name: "Respondentroll",
                table: "SurveyResponses");

            migrationBuilder.DropColumn(
                name: "ResultatkategoriNamn",
                table: "SurveyResponses");

            migrationBuilder.DropColumn(
                name: "SvarsalternativNr",
                table: "SurveyResponses");

            migrationBuilder.DropColumn(
                name: "SvarsalternativTyp",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AntalSvarsalternativ",
                table: "SurveyResponses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Enkatroll",
                table: "SurveyResponses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Forskoleverksamhet",
                table: "SurveyResponses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FragaNr",
                table: "SurveyResponses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FragaomradeNr",
                table: "SurveyResponses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Fragcategory",
                table: "SurveyResponses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Fragetype",
                table: "SurveyResponses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GraderingSvarsalternativ",
                table: "SurveyResponses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Kortfragetext",
                table: "SurveyResponses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Organisatoriskenhetskod",
                table: "SurveyResponses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RegiformNamn",
                table: "SurveyResponses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Respondentroll",
                table: "SurveyResponses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ResultatkategoriNamn",
                table: "SurveyResponses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SvarsalternativNr",
                table: "SurveyResponses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SvarsalternativTyp",
                table: "SurveyResponses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TotalVarde",
                table: "SurveyResponses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TotalVarde_ExklVetEj",
                table: "SurveyResponses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Utfall",
                table: "SurveyResponses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
