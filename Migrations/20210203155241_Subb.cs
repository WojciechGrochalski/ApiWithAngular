using Microsoft.EntityFrameworkCore.Migrations;

namespace AngularApi.Migrations
{
    public partial class Subb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Subscription",
                table: "userDBModels");

            migrationBuilder.CreateTable(
                name: "subscriptionDBModels",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Currency = table.Column<string>(nullable: true),
                    BidPrice = table.Column<float>(nullable: true),
                    AskPrice = table.Column<float>(nullable: true),
                    UserID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subscriptionDBModels", x => x.ID);
                    table.ForeignKey(
                        name: "FK_subscriptionDBModels_userDBModels_UserID",
                        column: x => x.UserID,
                        principalTable: "userDBModels",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_subscriptionDBModels_UserID",
                table: "subscriptionDBModels",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "subscriptionDBModels");

            migrationBuilder.AddColumn<bool>(
                name: "Subscription",
                table: "userDBModels",
                type: "bit",
                nullable: true);
        }
    }
}
