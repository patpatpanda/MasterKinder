using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasterKinder.Migrations
{
    /// <inheritdoc />
    public partial class survey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SurveyResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AvserAr = table.Column<int>(type: "int", nullable: true),
                    ResultatkategoriKod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResultatkategoriNamn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Stadsdelsnamnd = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Forskolenhet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Organisatoriskenhetskod = table.Column<int>(type: "int", nullable: true),
                    Forskoleverksamhet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegiformNamn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FragaOmradeNr = table.Column<int>(type: "int", nullable: true),
                    FragaOmradeText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FragaNr = table.Column<int>(type: "int", nullable: true),
                    FragaText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KortFragaText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SvarsalternativTyp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FragaTyp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FragaKategori = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AntalSvarsalternativ = table.Column<int>(type: "int", nullable: true),
                    SvarsalternativNr = table.Column<int>(type: "int", nullable: true),
                    SvarsalternativText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GraderingSvarsalternativ = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnkatRoll = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RespondentRoll = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Utfall = table.Column<double>(type: "float", nullable: true),
                    TotalVarde = table.Column<double>(type: "float", nullable: true),
                    TotalVardeExklVetEj = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyResults", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SurveyResults");
        }
    }
}
