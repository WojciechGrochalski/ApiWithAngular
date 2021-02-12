using Microsoft.EntityFrameworkCore.Migrations;

namespace angularapi.Migrations
{
    public partial class InitialCreate5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_refreshTokens_userDBModels_UserID",
                table: "refreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_refreshTokens_UserID",
                table: "refreshTokens");

            migrationBuilder.AddColumn<int>(
                name: "UserDBModelID",
                table: "refreshTokens",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_refreshTokens_UserDBModelID",
                table: "refreshTokens",
                column: "UserDBModelID");

            migrationBuilder.AddForeignKey(
                name: "FK_refreshTokens_userDBModels_UserDBModelID",
                table: "refreshTokens",
                column: "UserDBModelID",
                principalTable: "userDBModels",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_refreshTokens_userDBModels_UserDBModelID",
                table: "refreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_refreshTokens_UserDBModelID",
                table: "refreshTokens");

            migrationBuilder.DropColumn(
                name: "UserDBModelID",
                table: "refreshTokens");

            migrationBuilder.CreateIndex(
                name: "IX_refreshTokens_UserID",
                table: "refreshTokens",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_refreshTokens_userDBModels_UserID",
                table: "refreshTokens",
                column: "UserID",
                principalTable: "userDBModels",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
