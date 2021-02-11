using Microsoft.EntityFrameworkCore.Migrations;

namespace angularapi.Migrations
{
    public partial class InitialCreate3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Remainders");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Remainders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Remainders");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "Remainders",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
