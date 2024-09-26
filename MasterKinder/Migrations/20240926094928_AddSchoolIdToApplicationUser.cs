using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasterKinder.Migrations
{
    /// <inheritdoc />
    public partial class AddSchoolIdToApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ForskolanAllStats");

            migrationBuilder.DropTable(
                name: "PdfData");

            migrationBuilder.AddColumn<int>(
                name: "SchoolId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "ForskolanAllStats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AvserAr = table.Column<int>(type: "int", nullable: true),
                    Forskolenhet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Forskoleverksamhet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fragekategori = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Frageomradestext = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fragetext = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fragetyp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Kortfragetext = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Stadsdelsnamnd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SvarsalternativNr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SvarsalternativText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SvarsalternativTyp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForskolanAllStats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PdfData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AntalSvar = table.Column<int>(type: "int", nullable: true),
                    Helhetsomdome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Namn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedNamn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Svarsfrekvens = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PdfData", x => x.Id);
                });
        }
    }
}
