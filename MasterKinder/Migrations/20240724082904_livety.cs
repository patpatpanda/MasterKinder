using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasterKinder.Migrations
{
    /// <inheritdoc />
    public partial class livety : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "FragaNr1",
                table: "SurveyResults",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FragaNr10",
                table: "SurveyResults",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FragaNr11",
                table: "SurveyResults",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FragaNr12",
                table: "SurveyResults",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FragaNr13",
                table: "SurveyResults",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FragaNr14",
                table: "SurveyResults",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FragaNr2",
                table: "SurveyResults",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FragaNr3",
                table: "SurveyResults",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FragaNr4",
                table: "SurveyResults",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FragaNr5",
                table: "SurveyResults",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FragaNr6",
                table: "SurveyResults",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FragaNr7",
                table: "SurveyResults",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FragaNr8",
                table: "SurveyResults",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FragaNr9",
                table: "SurveyResults",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FragaNr1",
                table: "SurveyResults");

            migrationBuilder.DropColumn(
                name: "FragaNr10",
                table: "SurveyResults");

            migrationBuilder.DropColumn(
                name: "FragaNr11",
                table: "SurveyResults");

            migrationBuilder.DropColumn(
                name: "FragaNr12",
                table: "SurveyResults");

            migrationBuilder.DropColumn(
                name: "FragaNr13",
                table: "SurveyResults");

            migrationBuilder.DropColumn(
                name: "FragaNr14",
                table: "SurveyResults");

            migrationBuilder.DropColumn(
                name: "FragaNr2",
                table: "SurveyResults");

            migrationBuilder.DropColumn(
                name: "FragaNr3",
                table: "SurveyResults");

            migrationBuilder.DropColumn(
                name: "FragaNr4",
                table: "SurveyResults");

            migrationBuilder.DropColumn(
                name: "FragaNr5",
                table: "SurveyResults");

            migrationBuilder.DropColumn(
                name: "FragaNr6",
                table: "SurveyResults");

            migrationBuilder.DropColumn(
                name: "FragaNr7",
                table: "SurveyResults");

            migrationBuilder.DropColumn(
                name: "FragaNr8",
                table: "SurveyResults");

            migrationBuilder.DropColumn(
                name: "FragaNr9",
                table: "SurveyResults");
        }
    }
}
