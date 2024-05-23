using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasterKinder.Migrations
{
    /// <inheritdoc />
    public partial class asd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SurveyResponses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AvserAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResultatkategoriKod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResultatkategoriNamn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Stadsdelsnamnd = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Forskoleenhet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Organisatoriskenhetskod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Forskoleverksamhet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegiformNamn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FragaomradeNr = table.Column<int>(type: "int", nullable: false),
                    Fragaomradestext = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FragaNr = table.Column<int>(type: "int", nullable: false),
                    Fragetext = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kortfragetext = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SvarsalternativTyp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fragetyp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fragkategori = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AntalSvarsalternativ = table.Column<int>(type: "int", nullable: false),
                    SvarsalternativNr = table.Column<int>(type: "int", nullable: false),
                    SvarsalternativText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GraderingSvarsalternativ = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Enkatroll = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Respondentroll = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Utfall = table.Column<int>(type: "int", nullable: false),
                    TotalVarde = table.Column<int>(type: "int", nullable: false),
                    TotalVarde_ExklVetEj = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyResponses", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SurveyResponses");
        }
    }
}
