﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TamagitchiClient.Database;

#nullable disable

namespace TamagitchiClient.Migrations
{
    [DbContext(typeof(TamagitchiContext))]
    [Migration("20230122012435_Initial Migration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TamagitchiClient.Database.Models.TamagitchiCharacterTrait", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PetId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Trait")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PetId");

                    b.ToTable("Tamagitchi.CharacterTraits");
                });

            modelBuilder.Entity("TamagitchiClient.Database.Models.TamagitchiPet", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Alive")
                        .HasColumnType("bit");

                    b.Property<int>("CurrentHealth")
                        .HasColumnType("int");

                    b.Property<int>("MaxHealth")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Tamagitchi.Pets");
                });

            modelBuilder.Entity("TamagitchiClient.Database.Models.TamagitchiUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("GitlabId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Tamagitchi.Users");
                });

            modelBuilder.Entity("TamagitchiClient.Database.Models.TamagitchiCharacterTrait", b =>
                {
                    b.HasOne("TamagitchiClient.Database.Models.TamagitchiPet", "Pet")
                        .WithMany("TamagitchiCharacterTraits")
                        .HasForeignKey("PetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pet");
                });

            modelBuilder.Entity("TamagitchiClient.Database.Models.TamagitchiPet", b =>
                {
                    b.HasOne("TamagitchiClient.Database.Models.TamagitchiUser", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("TamagitchiClient.Database.Models.TamagitchiPet", b =>
                {
                    b.Navigation("TamagitchiCharacterTraits");
                });
#pragma warning restore 612, 618
        }
    }
}
