using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasterKinder.Migrations
{
    /// <inheritdoc />
    public partial class hej : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.CreateTable(
                name: "Malibus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Namn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Helhetsomdome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Svarsfrekvens = table.Column<int>(type: "int", nullable: false),
                    AntalSvar = table.Column<int>(type: "int", nullable: false),
                    NormalizedNamn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FrageText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AndelInstammer = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Malibus", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Malibus");

            migrationBuilder.CreateTable(
                name: "SpartaPdfs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AndelInstammer = table.Column<int>(type: "int", nullable: false),
                    AntalSvar = table.Column<int>(type: "int", nullable: false),
                    FrageText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Helhetsomdome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Namn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NormalizedNamn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Svarsfrekvens = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpartaPdfs", x => x.Id);
                });
        }
    }
}
