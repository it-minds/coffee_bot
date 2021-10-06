using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class UpdatedSettingsDefault : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RoundMidwayMessage",
                table: "ChannelSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Hello, guys! I am cheking in to see if you met for a cup of coffee this round.\n{{ YesButton }}{{ NoButton }}",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "Hello, guys! I am cheking in to see if you met for a cup of coffee this round.\n{{ YesButton: 'Yes, We have met!' }}{{ NoButton: 'No, we haven't met yet' }}");

            migrationBuilder.AlterColumn<string>(
                name: "RoundFinisherMessage",
                table: "ChannelSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Curtain call ladies and gentlefolk. <!channel>.\nYour success has been measured and I give you a solid 10! (For effort.) Your points have been given.\nThe total meetup rate of the round was: {{ MeetupPercentage }}%\n{{ MeetupCondition }}\nInformation regarding your next round TBA. Have a wonderful day :heart:",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "Curtain call ladies and gentlefolk. <!channel>.\nYour success has been measured and I give you a solid 10! (For effort.) Your points have been given.\nThe total meetup rate of the round was: {{ MeetupPercentage }}%\n{{ MeetupCondition: Next time, let's try for 100% shall we? }}\nInformation regarding your next round TBA. Have a wonderful day :heart:");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RoundMidwayMessage",
                table: "ChannelSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Hello, guys! I am cheking in to see if you met for a cup of coffee this round.\n{{ YesButton: 'Yes, We have met!' }}{{ NoButton: 'No, we haven't met yet' }}",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "Hello, guys! I am cheking in to see if you met for a cup of coffee this round.\n{{ YesButton }}{{ NoButton }}");

            migrationBuilder.AlterColumn<string>(
                name: "RoundFinisherMessage",
                table: "ChannelSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Curtain call ladies and gentlefolk. <!channel>.\nYour success has been measured and I give you a solid 10! (For effort.) Your points have been given.\nThe total meetup rate of the round was: {{ MeetupPercentage }}%\n{{ MeetupCondition: Next time, let's try for 100% shall we? }}\nInformation regarding your next round TBA. Have a wonderful day :heart:",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "Curtain call ladies and gentlefolk. <!channel>.\nYour success has been measured and I give you a solid 10! (For effort.) Your points have been given.\nThe total meetup rate of the round was: {{ MeetupPercentage }}%\n{{ MeetupCondition }}\nInformation regarding your next round TBA. Have a wonderful day :heart:");
        }
    }
}
