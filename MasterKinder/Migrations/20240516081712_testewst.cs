using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasterKinder.Migrations
{
    /// <inheritdoc />
    public partial class testewst : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Kon",
                table: "SurveyResponses");

            migrationBuilder.DropColumn(
                name: "ResultatkategoriKod",
                table: "SurveyResponses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Kon",
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
        }
    }
}
