using Microsoft.EntityFrameworkCore.Migrations;

namespace AngularApi.Migrations
{
    public partial class Subb2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_subscriptionDBModels_userDBModels_UserID",
                table: "subscriptionDBModels");

            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "subscriptionDBModels",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_subscriptionDBModels_userDBModels_UserID",
                table: "subscriptionDBModels",
                column: "UserID",
                principalTable: "userDBModels",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_subscriptionDBModels_userDBModels_UserID",
                table: "subscriptionDBModels");

            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "subscriptionDBModels",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_subscriptionDBModels_userDBModels_UserID",
                table: "subscriptionDBModels",
                column: "UserID",
                principalTable: "userDBModels",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
