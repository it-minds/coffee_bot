using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class AddChannelNotices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChannelNotices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoticeType = table.Column<int>(type: "int", nullable: false),
                    DaysInRound = table.Column<int>(type: "int", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Personal = table.Column<bool>(type: "bit", nullable: false),
                    ChannelSettingsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelNotices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChannelNotices_ChannelSettings_ChannelSettingsId",
                        column: x => x.ChannelSettingsId,
                        principalTable: "ChannelSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChannelNotices_ChannelSettingsId",
                table: "ChannelNotices",
                column: "ChannelSettingsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChannelNotices");
        }
    }
}
