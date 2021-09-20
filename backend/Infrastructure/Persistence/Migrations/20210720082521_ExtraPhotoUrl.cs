using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class ExtraPhotoUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhotoUrl",
                table: "CoffeeRoundGroups",
                newName: "SlackPhotoUrl");

            migrationBuilder.AddColumn<string>(
                name: "LocalPhotoUrl",
                table: "CoffeeRoundGroups",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocalPhotoUrl",
                table: "CoffeeRoundGroups");

            migrationBuilder.RenameColumn(
                name: "SlackPhotoUrl",
                table: "CoffeeRoundGroups",
                newName: "PhotoUrl");
        }
    }
}
