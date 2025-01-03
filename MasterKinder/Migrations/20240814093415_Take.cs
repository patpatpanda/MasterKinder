﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasterKinder.Migrations
{
    /// <inheritdoc />
    public partial class Take : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.CreateTable(
                name: "Surveys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AvserAr = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ResultatkategoriNamn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Stadsdelsnamnd = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Forskoleenhet = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Forskoleverksamhet = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RegiformNamn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FragaomradeNr = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Fragaomradestext = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FragaNr = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Fragetext = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Kortfragetext = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SvarsalternativTyp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Fragetyp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Fragkategori = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SvarsalternativNr = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SvarsalternativText = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Surveys", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Surveys");

            migrationBuilder.CreateTable(
                name: "SurveyResponses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AvserAr = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Forskoleenhet = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Forskoleverksamhet = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FragaNr = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    FragaomradeNr = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Fragaomradestext = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Fragetext = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Fragetyp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Fragkategori = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Kortfragetext = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    RegiformNamn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ResultatkategoriNamn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Stadsdelsnamnd = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SvarsalternativNr = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SvarsalternativText = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SvarsalternativTyp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyResponses", x => x.Id);
                });
        }
    }
}
