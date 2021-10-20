﻿// <auto-generated />
using System;
using Linklives.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Linklives.Migrations
{
    [DbContext(typeof(LinklivesContext))]
    partial class LinklivesContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.11");

            modelBuilder.Entity("LifeCourseLink", b =>
                {
                    b.Property<string>("LifeCoursesKey")
                        .HasColumnType("Varchar(350)");

                    b.Property<string>("LinksKey")
                        .HasColumnType("Varchar(350)");

                    b.HasKey("LifeCoursesKey", "LinksKey");

                    b.HasIndex("LinksKey");

                    b.ToTable("LifeCourseLink");
                });

            modelBuilder.Entity("Linklives.Domain.LifeCourse", b =>
                {
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

                    b.HasKey("Key");

                    b.ToTable("LifeCourses");
                });

            modelBuilder.Entity("Linklives.Domain.Link", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("Varchar(350)");

                    b.Property<string>("Iteration")
                        .HasColumnType("text");

                    b.Property<string>("Iteration_inner")
                        .HasColumnType("text");

                    b.Property<int>("Link_id")
                        .HasColumnType("int");

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

                    b.HasKey("Key");

                    b.ToTable("Links");
                });

            modelBuilder.Entity("Linklives.Domain.LinkRating", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("LinkKey")
                        .IsRequired()
                        .HasColumnType("Varchar(350)");

                    b.Property<int>("RatingId")
                        .HasColumnType("int");

                    b.Property<string>("User")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("LinkKey");

                    b.HasIndex("RatingId");

                    b.ToTable("LinkRatings");
                });

            modelBuilder.Entity("Linklives.Domain.RatingOption", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Heading")
                        .HasColumnType("text");

                    b.Property<string>("Text")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("RatingOptions");
                });

            modelBuilder.Entity("LifeCourseLink", b =>
                {
                    b.HasOne("Linklives.Domain.LifeCourse", null)
                        .WithMany()
                        .HasForeignKey("LifeCoursesKey")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Linklives.Domain.Link", null)
                        .WithMany()
                        .HasForeignKey("LinksKey")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Linklives.Domain.LinkRating", b =>
                {
                    b.HasOne("Linklives.Domain.Link", "Link")
                        .WithMany("Ratings")
                        .HasForeignKey("LinkKey")
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
