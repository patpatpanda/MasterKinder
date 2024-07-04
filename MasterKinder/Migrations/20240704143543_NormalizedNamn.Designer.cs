﻿// <auto-generated />
using System;
using MasterKinder.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MasterKinder.Migrations
{
    [DbContext(typeof(MrDb))]
    [Migration("20240704143543_NormalizedNamn")]
    partial class NormalizedNamn
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("KinderReader.Models.KontaktInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Epost")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ForskolanId")
                        .HasColumnType("int");

                    b.Property<string>("Namn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Roll")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telefon")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ForskolanId");

                    b.ToTable("kontaktInfos");
                });

            modelBuilder.Entity("MasterKinder.Models.Forskolan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Adress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("AndelLegitimeradeForskollarare")
                        .HasColumnType("float");

                    b.Property<int>("AntalBarn")
                        .HasColumnType("int");

                    b.Property<double?>("AntalBarnPerArsarbetare")
                        .HasColumnType("float");

                    b.Property<string>("Beskrivning")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InneOchUtemiljo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InriktningOchProfil")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("KostOchMaltider")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.Property<string>("MalOchVision")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MerOmOss")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Namn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Organisationsform")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TypAvService")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VerksamI")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Webbplats")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Forskolans");
                });

            modelBuilder.Entity("MasterKinder.Models.PdfData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AntalSvar")
                        .HasColumnType("int");

                    b.Property<string>("Helhetsomdome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Namn")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedNamn")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Svarsfrekvens")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("PdfData");
                });

            modelBuilder.Entity("KinderReader.Models.KontaktInfo", b =>
                {
                    b.HasOne("MasterKinder.Models.Forskolan", "Forskolan")
                        .WithMany("Kontakter")
                        .HasForeignKey("ForskolanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Forskolan");
                });

            modelBuilder.Entity("MasterKinder.Models.Forskolan", b =>
                {
                    b.Navigation("Kontakter");
                });
#pragma warning restore 612, 618
        }
    }
}
