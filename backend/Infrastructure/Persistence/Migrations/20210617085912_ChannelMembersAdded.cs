using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class ChannelMembersAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChannelMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SlackUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChannelSettingsId = table.Column<int>(type: "int", nullable: false),
                    OnPause = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChannelMembers_ChannelSettings_ChannelSettingsId",
                        column: x => x.ChannelSettingsId,
                        principalTable: "ChannelSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChannelMembers_ChannelSettingsId",
                table: "ChannelMembers",
                column: "ChannelSettingsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChannelMembers");
        }
    }
}
