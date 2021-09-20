using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class ImageUploadDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoffeeRounds_ChannelSettings_ChannelSettingsId",
                table: "CoffeeRounds");

            migrationBuilder.DropIndex(
                name: "IX_CoffeeRounds_ChannelSettingsId",
                table: "CoffeeRounds");

            migrationBuilder.DropColumn(
                name: "ChannelSettingsId",
                table: "CoffeeRounds");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "FinishedAt",
                table: "CoffeeRoundGroups",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CoffeeRounds_ChannelId",
                table: "CoffeeRounds",
                column: "ChannelId");

            migrationBuilder.AddForeignKey(
                name: "FK_CoffeeRounds_ChannelSettings_ChannelId",
                table: "CoffeeRounds",
                column: "ChannelId",
                principalTable: "ChannelSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoffeeRounds_ChannelSettings_ChannelId",
                table: "CoffeeRounds");

            migrationBuilder.DropIndex(
                name: "IX_CoffeeRounds_ChannelId",
                table: "CoffeeRounds");

            migrationBuilder.DropColumn(
                name: "FinishedAt",
                table: "CoffeeRoundGroups");

            migrationBuilder.AddColumn<int>(
                name: "ChannelSettingsId",
                table: "CoffeeRounds",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CoffeeRounds_ChannelSettingsId",
                table: "CoffeeRounds",
                column: "ChannelSettingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_CoffeeRounds_ChannelSettings_ChannelSettingsId",
                table: "CoffeeRounds",
                column: "ChannelSettingsId",
                principalTable: "ChannelSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
