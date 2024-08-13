using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasterKinder.Migrations
{
    /// <inheritdoc />
    public partial class dagis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.CreateTable(
                name: "ForskolanAllStats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AvserAr = table.Column<int>(type: "int", nullable: true),
                    Stadsdelsnamnd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Forskolenhet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Forskoleverksamhet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Frageomradestext = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fragetext = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Kortfragetext = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SvarsalternativTyp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fragetyp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fragekategori = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SvarsalternativNr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SvarsalternativText = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForskolanAllStats", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ForskolanAllStats");

            migrationBuilder.CreateTable(
                name: "DagisDatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AvserAr = table.Column<int>(type: "int", nullable: true),
                    Forskolenhet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Forskoleverksamhet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fragekategori = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Frageomradestext = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fragetext = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fragetyp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kortfragetext = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Stadsdelsnamnd = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SvarsalternativNr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SvarsalternativText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SvarsalternativTyp = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DagisDatas", x => x.Id);
                });
        }
    }
}
