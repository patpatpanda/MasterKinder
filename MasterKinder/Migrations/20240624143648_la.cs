using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasterKinder.Migrations
{
    /// <inheritdoc />
    public partial class la : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KontaktInfos_Forskolans_ForskolanId",
                table: "KontaktInfos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KontaktInfos",
                table: "KontaktInfos");

            migrationBuilder.RenameTable(
                name: "KontaktInfos",
                newName: "kontaktInfos");

            migrationBuilder.RenameIndex(
                name: "IX_KontaktInfos_ForskolanId",
                table: "kontaktInfos",
                newName: "IX_kontaktInfos_ForskolanId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_kontaktInfos",
                table: "kontaktInfos",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "PdfResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Helhetsomdome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AntalSvar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Svarsfrekvens = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ForskolanId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PdfResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PdfResults_Forskolans_ForskolanId",
                        column: x => x.ForskolanId,
                        principalTable: "Forskolans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PdfResults_ForskolanId",
                table: "PdfResults",
                column: "ForskolanId");

            migrationBuilder.AddForeignKey(
                name: "FK_kontaktInfos_Forskolans_ForskolanId",
                table: "kontaktInfos",
                column: "ForskolanId",
                principalTable: "Forskolans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_kontaktInfos_Forskolans_ForskolanId",
                table: "kontaktInfos");

            migrationBuilder.DropTable(
                name: "PdfResults");

            migrationBuilder.DropPrimaryKey(
                name: "PK_kontaktInfos",
                table: "kontaktInfos");

            migrationBuilder.RenameTable(
                name: "kontaktInfos",
                newName: "KontaktInfos");

            migrationBuilder.RenameIndex(
                name: "IX_kontaktInfos_ForskolanId",
                table: "KontaktInfos",
                newName: "IX_KontaktInfos_ForskolanId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KontaktInfos",
                table: "KontaktInfos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_KontaktInfos_Forskolans_ForskolanId",
                table: "KontaktInfos",
                column: "ForskolanId",
                principalTable: "Forskolans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
