using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasterKinder.Migrations
{
    /// <inheritdoc />
    public partial class One : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Forskolans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Namn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Beskrivning = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypAvService = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerksamI = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Organisationsform = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AntalBarn = table.Column<int>(type: "int", nullable: false),
                    AntalBarnPerArsarbetare = table.Column<double>(type: "float", nullable: true),
                    AndelLegitimeradeForskollarare = table.Column<double>(type: "float", nullable: true),
                    Webbplats = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InriktningOchProfil = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InneOchUtemiljo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KostOchMaltider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MalOchVision = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MerOmOss = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forskolans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "kontaktInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Namn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Epost = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Roll = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ForskolanId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kontaktInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_kontaktInfos_Forskolans_ForskolanId",
                        column: x => x.ForskolanId,
                        principalTable: "Forskolans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_kontaktInfos_ForskolanId",
                table: "kontaktInfos",
                column: "ForskolanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "kontaktInfos");

            migrationBuilder.DropTable(
                name: "Forskolans");
        }
    }
}
