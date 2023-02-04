using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TamagitchiClient.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tamagitchi.Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GitlabId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tamagitchi.Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tamagitchi.Pets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxHealth = table.Column<int>(type: "int", nullable: false),
                    CurrentHealth = table.Column<int>(type: "int", nullable: false),
                    Alive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tamagitchi.Pets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tamagitchi.Pets_Tamagitchi.Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Tamagitchi.Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tamagitchi.CharacterTraits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Trait = table.Column<int>(type: "int", nullable: false),
                    PetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tamagitchi.CharacterTraits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tamagitchi.CharacterTraits_Tamagitchi.Pets_PetId",
                        column: x => x.PetId,
                        principalTable: "Tamagitchi.Pets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tamagitchi.CharacterTraits_PetId",
                table: "Tamagitchi.CharacterTraits",
                column: "PetId");

            migrationBuilder.CreateIndex(
                name: "IX_Tamagitchi.Pets_OwnerId",
                table: "Tamagitchi.Pets",
                column: "OwnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tamagitchi.CharacterTraits");

            migrationBuilder.DropTable(
                name: "Tamagitchi.Pets");

            migrationBuilder.DropTable(
                name: "Tamagitchi.Users");
        }
    }
}
