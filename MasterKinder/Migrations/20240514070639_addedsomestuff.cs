using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasterKinder.Migrations
{
    /// <inheritdoc />
    public partial class addedsomestuff : Migration
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
                    AvserAr = table.Column<int>(type: "int", nullable: false),
                    ResultatkategoriKod = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ResultatkategoriNamn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Stadsdelsnamnd = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Forskoleenhet = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Organisatoriskenhetskod = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Forskoleverksamhet = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RegiformNamn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FragaomradeNr = table.Column<int>(type: "int", nullable: false),
                    Fragaomradestext = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FragaNr = table.Column<int>(type: "int", nullable: false),
                    Fragetext = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Kortfragetext = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SvarsalternativTyp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Fragetype = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Fragcategory = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AntalSvarsalternativ = table.Column<int>(type: "int", nullable: false),
                    SvarsalternativNr = table.Column<int>(type: "int", nullable: false),
                    SvarsalternativText = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    GraderingSvarsalternativ = table.Column<int>(type: "int", nullable: false),
                    Enkatroll = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Respondentroll = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Kon = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Utfall = table.Column<int>(type: "int", nullable: false),
                    TotalVarde = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalVarde_ExklVetEj = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
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
