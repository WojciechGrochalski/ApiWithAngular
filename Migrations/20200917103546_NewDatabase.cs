using Microsoft.EntityFrameworkCore.Migrations;

namespace AngularApi.Migrations
{
    public partial class NewDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "BidPrice",
                table: "cashDBModels",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "AskPrice",
                table: "cashDBModels",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "BidPrice",
                table: "cashDBModels",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<string>(
                name: "AskPrice",
                table: "cashDBModels",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(float));
        }
    }
}
