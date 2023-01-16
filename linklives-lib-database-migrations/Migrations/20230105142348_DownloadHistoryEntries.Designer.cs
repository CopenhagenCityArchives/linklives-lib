﻿// <auto-generated />
using System;
using Linklives.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Linklives.Migrations
{
    [DbContext(typeof(LinklivesContext))]
    [Migration("20230105142348_DownloadHistoryEntries")]
    partial class DownloadHistoryEntries
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.11");

            modelBuilder.Entity("LifeCourseLink", b =>
                {
                    b.Property<int>("LifeCoursesId")
                        .HasColumnType("int");

                    b.Property<int>("LinksId")
                        .HasColumnType("int");

                    b.HasKey("LifeCoursesId", "LinksId");

                    b.HasIndex("LinksId");

                    b.ToTable("LifeCourseLink");
                });

            modelBuilder.Entity("Linklives.Domain.DownloadHistoryEntry", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("DownloadType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DownloadedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Query")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("DownloadHistoryEntries");
                });

            modelBuilder.Entity("Linklives.Domain.LifeCourse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Data_version")
                        .HasColumnType("text");

                    b.Property<bool>("Is_historic")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Key")
                        .HasColumnType("Varchar(350)");

                    b.Property<int>("Life_course_id")
                        .HasColumnType("int");

                    b.Property<string>("Link_ids")
                        .HasColumnType("text");

                    b.Property<string>("N_sources")
                        .HasColumnType("text");

                    b.Property<string>("Pa_ids")
                        .HasColumnType("text");

                    b.Property<string>("Source_ids")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.HasIndex("Key")
                        .IsUnique();

                    b.ToTable("LifeCourses");
                });

            modelBuilder.Entity("Linklives.Domain.Link", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Data_version")
                        .HasColumnType("text");

                    b.Property<int>("Duplicates")
                        .HasColumnType("int");

                    b.Property<bool>("Is_historic")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Iteration")
                        .HasColumnType("text");

                    b.Property<string>("Iteration_inner")
                        .HasColumnType("text");

                    b.Property<string>("Key")
                        .HasColumnType("Varchar(350)");

                    b.Property<string>("Link_id")
                        .HasColumnType("text");

                    b.Property<string>("Method_id")
                        .HasColumnType("text");

                    b.Property<int>("Pa_id1")
                        .HasColumnType("int");

                    b.Property<int>("Pa_id2")
                        .HasColumnType("int");

                    b.Property<string>("Score")
                        .HasColumnType("text");

                    b.Property<int>("Source_id1")
                        .HasColumnType("int");

                    b.Property<int>("Source_id2")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.HasIndex("Key")
                        .IsUnique();

                    b.ToTable("Links");
                });

            modelBuilder.Entity("Linklives.Domain.LinkRating", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<int>("LinkId")
                        .HasColumnType("int");

                    b.Property<int>("RatingId")
                        .HasColumnType("int");

                    b.Property<string>("User")
                        .IsRequired()
                        .HasColumnType("varchar(767)");

                    b.HasKey("Id");

                    b.HasIndex("LinkId");

                    b.HasIndex("RatingId");

                    b.HasIndex("User");

                    b.ToTable("LinkRatings");
                });

            modelBuilder.Entity("Linklives.Domain.RatingOption", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Category")
                        .HasColumnType("CHAR(10)");

                    b.Property<string>("Heading")
                        .HasColumnType("text");

                    b.Property<string>("Text")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("RatingOptions");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Category = "positive",
                            Heading = "Ja, det er troværdigt",
                            Text = "Det ser fornuftigt ud. Personinformationen i de to kilder passer sammen."
                        },
                        new
                        {
                            Id = 2,
                            Category = "positive",
                            Heading = "Ja, det er troværdigt",
                            Text = "Jeg kan bekræfte informationen fra andre kilder, der ikke er med i Link-Lives."
                        },
                        new
                        {
                            Id = 3,
                            Category = "positive",
                            Heading = "Ja, det er troværdigt",
                            Text = "Jeg kan genkende informationen fra min private slægtsforskning."
                        },
                        new
                        {
                            Id = 4,
                            Category = "negative",
                            Heading = "Nej, det er ikke troværdigt",
                            Text = "Det ser forkert ud. Personinformation i de to kilder passer ikke sammen."
                        },
                        new
                        {
                            Id = 5,
                            Category = "negative",
                            Heading = "Nej, det er ikke troværdigt",
                            Text = "Jeg ved det er forkert ud fra andre kilder, der ikke er med i Link-Lives."
                        },
                        new
                        {
                            Id = 6,
                            Category = "negative",
                            Heading = "Nej, det er ikke troværdigt",
                            Text = "Jeg ved det er forkert fra min private slægtsforskning."
                        },
                        new
                        {
                            Id = 7,
                            Category = "neutral",
                            Heading = "Måske",
                            Text = "Jeg er i tvivl om personinformationen i de to kilder passer sammen."
                        },
                        new
                        {
                            Id = 8,
                            Category = "neutral",
                            Heading = "Måske",
                            Text = "Nogle af informationerne passer sammen. Andre gør ikke."
                        },
                        new
                        {
                            Id = 9,
                            Category = "neutral",
                            Heading = "Måske",
                            Text = "Der er ikke personinformation nok til at afgøre, om det er troværdigt."
                        });
                });

            modelBuilder.Entity("LifeCourseLink", b =>
                {
                    b.HasOne("Linklives.Domain.LifeCourse", null)
                        .WithMany()
                        .HasForeignKey("LifeCoursesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Linklives.Domain.Link", null)
                        .WithMany()
                        .HasForeignKey("LinksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Linklives.Domain.LinkRating", b =>
                {
                    b.HasOne("Linklives.Domain.Link", "Link")
                        .WithMany("Ratings")
                        .HasForeignKey("LinkId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Linklives.Domain.RatingOption", "Rating")
                        .WithMany()
                        .HasForeignKey("RatingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Link");

                    b.Navigation("Rating");
                });

            modelBuilder.Entity("Linklives.Domain.Link", b =>
                {
                    b.Navigation("Ratings");
                });
#pragma warning restore 612, 618
        }
    }
}