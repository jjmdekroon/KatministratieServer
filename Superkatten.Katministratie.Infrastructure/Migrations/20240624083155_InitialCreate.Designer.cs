﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Superkatten.Katministratie.Infrastructure.Persistence;

#nullable disable

namespace Superkatten.Katministratie.Infrastructure.Migrations
{
    [DbContext(typeof(SuperkattenDbContext))]
    [Migration("20240624083155_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.6");

            modelBuilder.Entity("Superkatten.Katministratie.Domain.Entities.CatchOrigin", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("CatchOrigins");
                });

            modelBuilder.Entity("Superkatten.Katministratie.Domain.Entities.Locations.BaseLocation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Locations");

                    b.HasDiscriminator<string>("Discriminator").HasValue("BaseLocation");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Superkatten.Katministratie.Domain.Entities.Locations.LocationNaw", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Address")
                        .HasColumnType("TEXT");

                    b.Property<string>("City")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Phone")
                        .HasColumnType("TEXT");

                    b.Property<string>("Postcode")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("LocationNaw");
                });

            modelBuilder.Entity("Superkatten.Katministratie.Domain.Entities.MedicalProcedure", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("ProcedureType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Remark")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("SuperkatId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("MedicalProcedures");
                });

            modelBuilder.Entity("Superkatten.Katministratie.Domain.Entities.Superkat", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("AgeCategory")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Behaviour")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Birthday")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CatchDate")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CatchOriginId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("FoodType")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Gender")
                        .HasColumnType("INTEGER");

                    b.Property<int>("LitterType")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("LocationId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Number")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("Photo")
                        .HasColumnType("BLOB");

                    b.Property<bool>("Reserved")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Retour")
                        .HasColumnType("INTEGER");

                    b.Property<int>("State")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("WetFoodAllowed")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CatchOriginId");

                    b.HasIndex("LocationId");

                    b.ToTable("SuperKatten");
                });

            modelBuilder.Entity("Superkatten.Katministratie.Infrastructure.Entities.UserDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Permissions")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Superkatten.Katministratie.Domain.Entities.Locations.Adoptant", b =>
                {
                    b.HasBaseType("Superkatten.Katministratie.Domain.Entities.Locations.BaseLocation");

                    b.HasDiscriminator().HasValue("Adoptant");
                });

            modelBuilder.Entity("Superkatten.Katministratie.Domain.Entities.Locations.Gastgezin", b =>
                {
                    b.HasBaseType("Superkatten.Katministratie.Domain.Entities.Locations.BaseLocation");

                    b.HasDiscriminator().HasValue("Gastgezin");
                });

            modelBuilder.Entity("Superkatten.Katministratie.Domain.Entities.Locations.Refuge", b =>
                {
                    b.HasBaseType("Superkatten.Katministratie.Domain.Entities.Locations.BaseLocation");

                    b.Property<int?>("CageNumber")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CatArea")
                        .HasColumnType("INTEGER");

                    b.HasDiscriminator().HasValue("Refuge");
                });

            modelBuilder.Entity("Superkatten.Katministratie.Domain.Entities.Locations.LocationNaw", b =>
                {
                    b.HasOne("Superkatten.Katministratie.Domain.Entities.Locations.BaseLocation", "Location")
                        .WithOne("LocationNaw")
                        .HasForeignKey("Superkatten.Katministratie.Domain.Entities.Locations.LocationNaw", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Location");
                });

            modelBuilder.Entity("Superkatten.Katministratie.Domain.Entities.Superkat", b =>
                {
                    b.HasOne("Superkatten.Katministratie.Domain.Entities.CatchOrigin", "CatchOrigin")
                        .WithMany()
                        .HasForeignKey("CatchOriginId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Superkatten.Katministratie.Domain.Entities.Locations.BaseLocation", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CatchOrigin");

                    b.Navigation("Location");
                });

            modelBuilder.Entity("Superkatten.Katministratie.Domain.Entities.Locations.BaseLocation", b =>
                {
                    b.Navigation("LocationNaw")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
