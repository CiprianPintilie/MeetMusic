﻿// <auto-generated />
using Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Storage.Internal;
using System;

namespace MeetMusic.Migrations
{
    [DbContext(typeof(MeetMusicDbContext))]
    partial class MeetMusicDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011");

            modelBuilder.Entity("MeetMusicModels.Models.MusicFamily", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36);

                    b.Property<bool>("Deleted");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.ToTable("MusicFamilies");
                });

            modelBuilder.Entity("MeetMusicModels.Models.MusicGenre", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36);

                    b.Property<bool>("Deleted");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<string>("FamilyId")
                        .HasMaxLength(36);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("FamilyId");

                    b.ToTable("MusicGenres");
                });

            modelBuilder.Entity("MeetMusicModels.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36);

                    b.Property<string>("AvatarUrl");

                    b.Property<DateTime>("BirthDate");

                    b.Property<bool>("Deleted");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<string>("Description")
                        .HasMaxLength(500);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(254);

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<int>("Gender");

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<string>("PhoneNumber");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MeetMusicModels.Models.UserMusicFamily", b =>
                {
                    b.Property<string>("UserId")
                        .HasMaxLength(36);

                    b.Property<string>("FamilyId")
                        .HasMaxLength(36);

                    b.Property<int>("Rank");

                    b.HasKey("UserId", "FamilyId");

                    b.HasIndex("FamilyId");

                    b.ToTable("UserMusicFamilies");
                });

            modelBuilder.Entity("MeetMusicModels.Models.MusicGenre", b =>
                {
                    b.HasOne("MeetMusicModels.Models.MusicFamily", "Family")
                        .WithMany("Genres")
                        .HasForeignKey("FamilyId");
                });

            modelBuilder.Entity("MeetMusicModels.Models.UserMusicFamily", b =>
                {
                    b.HasOne("MeetMusicModels.Models.MusicFamily", "MusicFamily")
                        .WithMany("UserMusicFamilies")
                        .HasForeignKey("FamilyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MeetMusicModels.Models.User", "User")
                        .WithMany("UserMusicFamilies")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
