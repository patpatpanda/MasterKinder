using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasterKinder.Migrations
{
    /// <inheritdoc />
    public partial class sommaren : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "PostBlogs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "PdfAllData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Namn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Helhetsomdome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Svarsfrekvens = table.Column<int>(type: "int", nullable: false),
                    AntalSvar = table.Column<int>(type: "int", nullable: false),
                    NormalizedNamn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UtvecklingOchLarande = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SprakligUtveckling = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FårStod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HållbarUtveckling = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InformationOmUtvecklingOchLarande = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Trygghet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SocialaFormagor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PersonalensOmsorg = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PositivBildAvSigSjälv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LikaMojligheter = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LedningTillganglighet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PersonalensBemotande = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MatInformation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FysiskaAktiviteter = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SamverkanMedHemmet = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PdfAllData", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PdfAllData");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "PostBlogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "BlogPosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublishedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPosts", x => x.Id);
                });
        }
    }
}
