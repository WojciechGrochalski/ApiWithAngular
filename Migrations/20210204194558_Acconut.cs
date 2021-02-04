using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace angularapi.Migrations
{
    public partial class Acconut : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "userDBModels",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsVerify",
                table: "userDBModels",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "VeryficationToken",
                table: "userDBModels",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "userDBModels");

            migrationBuilder.DropColumn(
                name: "IsVerify",
                table: "userDBModels");

            migrationBuilder.DropColumn(
                name: "VeryficationToken",
                table: "userDBModels");
        }
    }
}
