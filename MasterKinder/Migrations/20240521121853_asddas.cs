using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasterKinder.Migrations
{
    /// <inheritdoc />
    public partial class asddas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AntalSvarsalternativ",
                table: "SurveyResponses",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
                name: "Fragetyp",
                table: "SurveyResponses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Fragkategori",
                table: "SurveyResponses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Kon",
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "Fragetyp",
                table: "SurveyResponses");

            migrationBuilder.DropColumn(
                name: "Fragkategori",
                table: "SurveyResponses");

            migrationBuilder.DropColumn(
                name: "Kon",
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
        }
    }
}
