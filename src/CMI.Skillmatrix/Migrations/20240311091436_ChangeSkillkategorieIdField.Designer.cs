﻿// <auto-generated />
using CMI.Skillmatrix.Components.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CMI.Skillmatrix.Migrations
{
    [DbContext(typeof(SkillmatrixDbContext))]
    [Migration("20240311091436_ChangeSkillkategorieIdField")]
    partial class ChangeSkillkategorieIdField
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CMI.Skillmatrix.Components.Data.Models.Mitarbeiter", b =>
                {
                    b.Property<int>("MitarbeiterId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MitarbeiterId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Vorname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MitarbeiterId");

                    b.ToTable("Mitarbeiter");
                });

            modelBuilder.Entity("CMI.Skillmatrix.Components.Data.Models.MitarbeiterSkill", b =>
                {
                    b.Property<int>("MitarbeiterId")
                        .HasColumnType("int");

                    b.Property<int>("SkillId")
                        .HasColumnType("int");

                    b.Property<int>("Selbsteinschaetzung")
                        .HasColumnType("int");

                    b.HasKey("MitarbeiterId", "SkillId");

                    b.HasIndex("SkillId");

                    b.ToTable("MitarbeiterSkill", (string)null);
                });

            modelBuilder.Entity("CMI.Skillmatrix.Components.Data.Models.Skill", b =>
                {
                    b.Property<int>("SkillId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SkillId"));

                    b.Property<string>("SkillName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SkillkategorieId")
                        .HasColumnType("int");

                    b.HasKey("SkillId");

                    b.HasIndex("SkillkategorieId");

                    b.ToTable("Skill");
                });

            modelBuilder.Entity("CMI.Skillmatrix.Components.Data.Models.Skillkategorie", b =>
                {
                    b.Property<int>("SkillkategorieId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SkillkategorieId"));

                    b.Property<string>("SkillkategorieName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SkillkategorieId");

                    b.ToTable("Skillkategorie");
                });

            modelBuilder.Entity("CMI.Skillmatrix.Components.Data.Models.MitarbeiterSkill", b =>
                {
                    b.HasOne("CMI.Skillmatrix.Components.Data.Models.Mitarbeiter", "Mitarbeiter")
                        .WithMany()
                        .HasForeignKey("MitarbeiterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CMI.Skillmatrix.Components.Data.Models.Skill", "Skill")
                        .WithMany()
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Mitarbeiter");

                    b.Navigation("Skill");
                });

            modelBuilder.Entity("CMI.Skillmatrix.Components.Data.Models.Skill", b =>
                {
                    b.HasOne("CMI.Skillmatrix.Components.Data.Models.Skillkategorie", "Skillkategorie")
                        .WithMany("Skill")
                        .HasForeignKey("SkillkategorieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Skillkategorie");
                });

            modelBuilder.Entity("CMI.Skillmatrix.Components.Data.Models.Skillkategorie", b =>
                {
                    b.Navigation("Skill");
                });
#pragma warning restore 612, 618
        }
    }
}
