using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChannelSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SlackChannelId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GroupSize = table.Column<int>(type: "int", nullable: false),
                    StartsDay = table.Column<int>(type: "int", nullable: false),
                    WeekRepeat = table.Column<int>(type: "int", nullable: false),
                    DurationInDays = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoffeeRounds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChannelSettingsId = table.Column<int>(type: "int", nullable: true),
                    ChannelId = table.Column<int>(type: "int", nullable: false),
                    SlackChannelId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    StartDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    EndDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoffeeRounds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoffeeRounds_ChannelSettings_ChannelSettingsId",
                        column: x => x.ChannelSettingsId,
                        principalTable: "ChannelSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CoffeeRoundGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SlackMessageId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasMet = table.Column<bool>(type: "bit", nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoffeeRoundId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoffeeRoundGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoffeeRoundGroups_CoffeeRounds_CoffeeRoundId",
                        column: x => x.CoffeeRoundId,
                        principalTable: "CoffeeRounds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CoffeeRoundGroupMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SlackMemberId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoffeeRoundGroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoffeeRoundGroupMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoffeeRoundGroupMembers_CoffeeRoundGroups_CoffeeRoundGroupId",
                        column: x => x.CoffeeRoundGroupId,
                        principalTable: "CoffeeRoundGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoffeeRoundGroupMembers_CoffeeRoundGroupId",
                table: "CoffeeRoundGroupMembers",
                column: "CoffeeRoundGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_CoffeeRoundGroups_CoffeeRoundId",
                table: "CoffeeRoundGroups",
                column: "CoffeeRoundId");

            migrationBuilder.CreateIndex(
                name: "IX_CoffeeRounds_ChannelSettingsId",
                table: "CoffeeRounds",
                column: "ChannelSettingsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoffeeRoundGroupMembers");

            migrationBuilder.DropTable(
                name: "CoffeeRoundGroups");

            migrationBuilder.DropTable(
                name: "CoffeeRounds");

            migrationBuilder.DropTable(
                name: "ChannelSettings");
        }
    }
}
