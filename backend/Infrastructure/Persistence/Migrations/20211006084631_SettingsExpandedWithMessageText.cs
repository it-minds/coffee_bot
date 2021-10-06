using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class SettingsExpandedWithMessageText : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RoundFinisherMessage",
                table: "ChannelSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Curtain call ladies and gentlefolk. <!channel>.\nYour success has been measured and I give you a solid 10! (For effort.) Your points have been given.\nThe total meetup rate of the round was: {{ MeetupPercentage }}%\n{{ MeetupCondition: Next time, let's try for 100% shall we? }}\nInformation regarding your next round TBA. Have a wonderful day :heart:");

            migrationBuilder.AddColumn<string>(
                name: "RoundMidwayMessage",
                table: "ChannelSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Hello, guys! I am cheking in to see if you met for a cup of coffee this round.\n{{ YesButton: 'Yes, We have met!' }}{{ NoButton: 'No, we haven't met yet' }}");

            migrationBuilder.AddColumn<string>(
                name: "RoundStartChannelMessage",
                table: "ChannelSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Time to drink coffee <!channel>\nThe round starts: {{ RoundStartTime }}. The round ends {{ RoundEndTime }}.The groups are:{{ Groups }}");

            migrationBuilder.AddColumn<string>(
                name: "RoundStartGroupMessage",
                table: "ChannelSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Time for your coffee!\nThe round starts: {{ RoundStartTime }}. The round ends: {{ RoundEndTime }}.\nHave fun!");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoundFinisherMessage",
                table: "ChannelSettings");

            migrationBuilder.DropColumn(
                name: "RoundMidwayMessage",
                table: "ChannelSettings");

            migrationBuilder.DropColumn(
                name: "RoundStartChannelMessage",
                table: "ChannelSettings");

            migrationBuilder.DropColumn(
                name: "RoundStartGroupMessage",
                table: "ChannelSettings");
        }
    }
}
