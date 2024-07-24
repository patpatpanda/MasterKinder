using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasterKinder.Migrations
{
    /// <inheritdoc />
    public partial class tell : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "FysiskaAktiviteterAndelInstammer",
                table: "PdfAllData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FårStodAndelInstammer",
                table: "PdfAllData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "HållbarUtvecklingAndelInstammer",
                table: "PdfAllData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "InformationOmUtvecklingOchLarandeAndelInstammer",
                table: "PdfAllData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "LedningTillganglighetAndelInstammer",
                table: "PdfAllData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "LikaMojligheterAndelInstammer",
                table: "PdfAllData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MatInformationAndelInstammer",
                table: "PdfAllData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PersonalensBemotandeAndelInstammer",
                table: "PdfAllData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PersonalensOmsorgAndelInstammer",
                table: "PdfAllData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PositivBildAvSigSjälvAndelInstammer",
                table: "PdfAllData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SamverkanMedHemmetAndelInstammer",
                table: "PdfAllData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SocialaFormagorAndelInstammer",
                table: "PdfAllData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SprakligUtvecklingAndelInstammer",
                table: "PdfAllData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TrygghetAndelInstammer",
                table: "PdfAllData",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FysiskaAktiviteterAndelInstammer",
                table: "PdfAllData");

            migrationBuilder.DropColumn(
                name: "FårStodAndelInstammer",
                table: "PdfAllData");

            migrationBuilder.DropColumn(
                name: "HållbarUtvecklingAndelInstammer",
                table: "PdfAllData");

            migrationBuilder.DropColumn(
                name: "InformationOmUtvecklingOchLarandeAndelInstammer",
                table: "PdfAllData");

            migrationBuilder.DropColumn(
                name: "LedningTillganglighetAndelInstammer",
                table: "PdfAllData");

            migrationBuilder.DropColumn(
                name: "LikaMojligheterAndelInstammer",
                table: "PdfAllData");

            migrationBuilder.DropColumn(
                name: "MatInformationAndelInstammer",
                table: "PdfAllData");

            migrationBuilder.DropColumn(
                name: "PersonalensBemotandeAndelInstammer",
                table: "PdfAllData");

            migrationBuilder.DropColumn(
                name: "PersonalensOmsorgAndelInstammer",
                table: "PdfAllData");

            migrationBuilder.DropColumn(
                name: "PositivBildAvSigSjälvAndelInstammer",
                table: "PdfAllData");

            migrationBuilder.DropColumn(
                name: "SamverkanMedHemmetAndelInstammer",
                table: "PdfAllData");

            migrationBuilder.DropColumn(
                name: "SocialaFormagorAndelInstammer",
                table: "PdfAllData");

            migrationBuilder.DropColumn(
                name: "SprakligUtvecklingAndelInstammer",
                table: "PdfAllData");

            migrationBuilder.DropColumn(
                name: "TrygghetAndelInstammer",
                table: "PdfAllData");
        }
    }
}
