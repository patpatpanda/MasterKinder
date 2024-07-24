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
    [Migration("20240724083628_lala")]
    partial class lala
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
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

            modelBuilder.Entity("MasterKinder.Models.PdfAllData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AntalSvar")
                        .HasColumnType("int");

                    b.Property<double?>("FysiskaAktiviteter")
                        .HasColumnType("float");

                    b.Property<double?>("FysiskaAktiviteterAndelInstammer")
                        .HasColumnType("float");

                    b.Property<double?>("FårStod")
                        .HasColumnType("float");

                    b.Property<double?>("FårStodAndelInstammer")
                        .HasColumnType("float");

                    b.Property<string>("Helhetsomdome")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("HållbarUtveckling")
                        .HasColumnType("float");

                    b.Property<double?>("HållbarUtvecklingAndelInstammer")
                        .HasColumnType("float");

                    b.Property<double?>("InformationOmUtvecklingOchLarande")
                        .HasColumnType("float");

                    b.Property<double?>("InformationOmUtvecklingOchLarandeAndelInstammer")
                        .HasColumnType("float");

                    b.Property<double?>("LedningTillganglighet")
                        .HasColumnType("float");

                    b.Property<double?>("LedningTillganglighetAndelInstammer")
                        .HasColumnType("float");

                    b.Property<double?>("LikaMojligheter")
                        .HasColumnType("float");

                    b.Property<double?>("LikaMojligheterAndelInstammer")
                        .HasColumnType("float");

                    b.Property<double?>("MatInformation")
                        .HasColumnType("float");

                    b.Property<double?>("MatInformationAndelInstammer")
                        .HasColumnType("float");

                    b.Property<string>("Namn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedNamn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("PersonalensBemotande")
                        .HasColumnType("float");

                    b.Property<double?>("PersonalensBemotandeAndelInstammer")
                        .HasColumnType("float");

                    b.Property<double?>("PersonalensOmsorg")
                        .HasColumnType("float");

                    b.Property<double?>("PersonalensOmsorgAndelInstammer")
                        .HasColumnType("float");

                    b.Property<double?>("PositivBildAvSigSjälv")
                        .HasColumnType("float");

                    b.Property<double?>("PositivBildAvSigSjälvAndelInstammer")
                        .HasColumnType("float");

                    b.Property<double?>("SamverkanMedHemmet")
                        .HasColumnType("float");

                    b.Property<double?>("SamverkanMedHemmetAndelInstammer")
                        .HasColumnType("float");

                    b.Property<double?>("SocialaFormagor")
                        .HasColumnType("float");

                    b.Property<double?>("SocialaFormagorAndelInstammer")
                        .HasColumnType("float");

                    b.Property<double?>("SprakligUtveckling")
                        .HasColumnType("float");

                    b.Property<double?>("SprakligUtvecklingAndelInstammer")
                        .HasColumnType("float");

                    b.Property<int?>("Svarsfrekvens")
                        .HasColumnType("int");

                    b.Property<double?>("Trygghet")
                        .HasColumnType("float");

                    b.Property<double?>("TrygghetAndelInstammer")
                        .HasColumnType("float");

                    b.Property<double?>("UtvecklingOchLarande")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("PdfAllData");
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

            modelBuilder.Entity("MasterKinder.Models.PostBlog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PublishedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("PostBlogs");
                });

            modelBuilder.Entity("MasterKinder.Models.SurveyResult", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AntalSvarsalternativ")
                        .HasColumnType("int");

                    b.Property<int?>("AvserAr")
                        .HasColumnType("int");

                    b.Property<string>("EnkatRoll")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Forskolenhet")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Forskoleverksamhet")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FragaKategori")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("FragaNr")
                        .HasColumnType("int");

                    b.Property<double?>("FragaNr1")
                        .HasColumnType("float");

                    b.Property<double?>("FragaNr10")
                        .HasColumnType("float");

                    b.Property<double?>("FragaNr11")
                        .HasColumnType("float");

                    b.Property<double?>("FragaNr12")
                        .HasColumnType("float");

                    b.Property<double?>("FragaNr13")
                        .HasColumnType("float");

                    b.Property<double?>("FragaNr14")
                        .HasColumnType("float");

                    b.Property<double?>("FragaNr2")
                        .HasColumnType("float");

                    b.Property<double?>("FragaNr3")
                        .HasColumnType("float");

                    b.Property<double?>("FragaNr4")
                        .HasColumnType("float");

                    b.Property<double?>("FragaNr5")
                        .HasColumnType("float");

                    b.Property<double?>("FragaNr6")
                        .HasColumnType("float");

                    b.Property<double?>("FragaNr7")
                        .HasColumnType("float");

                    b.Property<double?>("FragaNr8")
                        .HasColumnType("float");

                    b.Property<double?>("FragaNr9")
                        .HasColumnType("float");

                    b.Property<int?>("FragaOmradeNr")
                        .HasColumnType("int");

                    b.Property<string>("FragaOmradeText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FragaText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FragaTyp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GraderingSvarsalternativ")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Kon")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("KortFragaText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Organisatoriskenhetskod")
                        .HasColumnType("int");

                    b.Property<string>("RegiformNamn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RespondentRoll")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ResultatkategoriKod")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ResultatkategoriNamn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Stadsdelsnamnd")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SvarsalternativNr")
                        .HasColumnType("int");

                    b.Property<string>("SvarsalternativText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SvarsalternativTyp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("TotalVarde")
                        .HasColumnType("float");

                    b.Property<double?>("TotalVardeExklVetEj")
                        .HasColumnType("float");

                    b.Property<double?>("Utfall")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("SurveyResults");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
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

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MasterKinder.Models.Forskolan", b =>
                {
                    b.Navigation("Kontakter");
                });
#pragma warning restore 612, 618
        }
    }
}
