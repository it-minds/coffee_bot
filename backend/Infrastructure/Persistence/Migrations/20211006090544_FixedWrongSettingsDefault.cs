using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class FixedWrongSettingsDefault : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RoundStartChannelMessage",
                table: "ChannelSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Time to drink coffee <!channel>\nThe round starts: {{ RoundStartTime }}. The round ends {{ RoundEndTime }}.\nThe groups are:\n{{ Groups }}",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "Time to drink coffee <!channel>\nThe round starts: {{ RoundStartTime }}. The round ends {{ RoundEndTime }}.The groups are:{{ Groups }}");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RoundStartChannelMessage",
                table: "ChannelSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Time to drink coffee <!channel>\nThe round starts: {{ RoundStartTime }}. The round ends {{ RoundEndTime }}.The groups are:{{ Groups }}",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "Time to drink coffee <!channel>\nThe round starts: {{ RoundStartTime }}. The round ends {{ RoundEndTime }}.\nThe groups are:\n{{ Groups }}");
        }
    }
}
