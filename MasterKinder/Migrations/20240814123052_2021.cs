using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasterKinder.Migrations
{
    /// <inheritdoc />
    public partial class _2021 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SurveyResponses2021",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AvserAr = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    ResultatkategoriKod = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ResultatkategoriNamn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Stadsdelsnamnd = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Forskoleenhet = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Organisatoriskenhetskod = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Forskoleverksamhet = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    RegiformNamn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FrageomradeNr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Frageomradestext = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FrageNr = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Fragetext = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Kortfragetext = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SvarsalternativTyp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Fragetyp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Fragekategori = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AntalSvarsalternativ = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SvarsalternativNr = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SvarsalternativText = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    GraderingSvarsalternativ = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Enkatroll = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Respondentroll = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Kon = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Utfall = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TotalVarde = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TotalVarde_ExklVetEj = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyResponses2021", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SurveyResponses2022",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AvserAr = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    ResultatkategoriKod = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ResultatkategoriNamn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Stadsdelsnamnd = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Forskoleenhet = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Organisatoriskenhetskod = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Forskoleverksamhet = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    RegiformNamn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FrageomradeNr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Frageomradestext = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FrageNr = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Fragetext = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Kortfragetext = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SvarsalternativTyp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Fragetyp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Fragekategori = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AntalSvarsalternativ = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SvarsalternativNr = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SvarsalternativText = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    GraderingSvarsalternativ = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Enkatroll = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Respondentroll = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Kon = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Utfall = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TotalVarde = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TotalVarde_ExklVetEj = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyResponses2022", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SurveyResponses2023",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AvserAr = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    ResultatkategoriKod = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ResultatkategoriNamn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Stadsdelsnamnd = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Forskoleenhet = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Organisatoriskenhetskod = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Forskoleverksamhet = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    RegiformNamn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FrageomradeNr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Frageomradestext = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FrageNr = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Fragetext = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Kortfragetext = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SvarsalternativTyp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Fragetyp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Fragekategori = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AntalSvarsalternativ = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SvarsalternativNr = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SvarsalternativText = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    GraderingSvarsalternativ = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Enkatroll = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Respondentroll = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Kon = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Utfall = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TotalVarde = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TotalVarde_ExklVetEj = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyResponses2023", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SurveyResponses2021");

            migrationBuilder.DropTable(
                name: "SurveyResponses2022");

            migrationBuilder.DropTable(
                name: "SurveyResponses2023");
        }
    }
}
