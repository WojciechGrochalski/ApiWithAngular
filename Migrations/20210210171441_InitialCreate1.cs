using Microsoft.EntityFrameworkCore.Migrations;

namespace angularapi.Migrations
{
    public partial class InitialCreate1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Price",
                table: "Remainders",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Price",
                table: "Remainders",
                type: "bit",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
