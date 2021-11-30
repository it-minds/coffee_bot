using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class PredfinedGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PredefinedGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChannelSettingsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PredefinedGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PredefinedGroups_ChannelSettings_ChannelSettingsId",
                        column: x => x.ChannelSettingsId,
                        principalTable: "ChannelSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PredefinedGroupMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PredefinedGroupId = table.Column<int>(type: "int", nullable: false),
                    ChannelMemberId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PredefinedGroupMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PredefinedGroupMembers_ChannelMembers_ChannelMemberId",
                        column: x => x.ChannelMemberId,
                        principalTable: "ChannelMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PredefinedGroupMembers_PredefinedGroups_PredefinedGroupId",
                        column: x => x.PredefinedGroupId,
                        principalTable: "PredefinedGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PredefinedGroupMembers_ChannelMemberId",
                table: "PredefinedGroupMembers",
                column: "ChannelMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_PredefinedGroupMembers_PredefinedGroupId",
                table: "PredefinedGroupMembers",
                column: "PredefinedGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_PredefinedGroups_ChannelSettingsId",
                table: "PredefinedGroups",
                column: "ChannelSettingsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PredefinedGroupMembers");

            migrationBuilder.DropTable(
                name: "PredefinedGroups");
        }
    }
}
