﻿// <auto-generated />
using MasterKinder.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MasterKinder.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MasterKinder.Models.SurveyResponse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AntalSvarsalternativ")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AvserAr")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Enkatroll")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Forskoleenhet")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Forskoleverksamhet")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FragaNr")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FragaomradeNr")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Fragaomradestext")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Fragcategory")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Fragetext")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Fragetype")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GraderingSvarsalternativ")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Kon")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Kortfragetext")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Organisatoriskenhetskod")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RegiformNamn")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Respondentroll")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ResultatkategoriKod")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ResultatkategoriNamn")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Stadsdelsnamnd")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SvarsalternativNr")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SvarsalternativText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SvarsalternativTyp")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TotalVarde")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TotalVarde_ExklVetEj")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Utfall")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SurveyResponses");
                });
#pragma warning restore 612, 618
        }
    }
}
