using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class WorkspaceIdAddedToSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SlackChannelId",
                table: "CoffeeRounds");

            migrationBuilder.AddColumn<string>(
                name: "SlackWorkSpaceId",
                table: "ChannelSettings",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SlackWorkSpaceId",
                table: "ChannelSettings");

            migrationBuilder.AddColumn<string>(
                name: "SlackChannelId",
                table: "CoffeeRounds",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }
    }
}
