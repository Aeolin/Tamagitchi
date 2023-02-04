using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TamagitchiClient.Migrations
{
    /// <inheritdoc />
    public partial class ChangedFieldtypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "GitlabId",
                table: "Tamagitchi.Users",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,0)");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastFood",
                table: "Tamagitchi.Pets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastStarvationTick",
                table: "Tamagitchi.Pets",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastFood",
                table: "Tamagitchi.Pets");

            migrationBuilder.DropColumn(
                name: "LastStarvationTick",
                table: "Tamagitchi.Pets");

            migrationBuilder.AlterColumn<decimal>(
                name: "GitlabId",
                table: "Tamagitchi.Users",
                type: "decimal(20,0)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
