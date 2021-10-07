using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class AddedSettingsCommandHours : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FinalizeRoundHour",
                table: "ChannelSettings",
                type: "int",
                nullable: false,
                defaultValue: 16);

            migrationBuilder.AddColumn<int>(
                name: "InitializeRoundHour",
                table: "ChannelSettings",
                type: "int",
                nullable: false,
                defaultValue: 10);

            migrationBuilder.AddColumn<int>(
                name: "MidwayRoundHour",
                table: "ChannelSettings",
                type: "int",
                nullable: false,
                defaultValue: 11);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinalizeRoundHour",
                table: "ChannelSettings");

            migrationBuilder.DropColumn(
                name: "InitializeRoundHour",
                table: "ChannelSettings");

            migrationBuilder.DropColumn(
                name: "MidwayRoundHour",
                table: "ChannelSettings");
        }
    }
}
