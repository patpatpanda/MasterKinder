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

                    b.Property<string>("AvserAr")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Forskoleenhet")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Fragaomradestext")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Fragetext")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Stadsdelsnamnd")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SvarsalternativText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SurveyResponses");
                });
#pragma warning restore 612, 618
        }
    }
}
