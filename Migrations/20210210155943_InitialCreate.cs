using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace angularapi.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cashDBModels",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    BidPrice = table.Column<float>(nullable: false),
                    AskPrice = table.Column<float>(nullable: false),
                    Data = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cashDBModels", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "userDBModels",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Pass = table.Column<string>(nullable: true),
                    VeryficationToken = table.Column<string>(nullable: true),
                    IsVerify = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Subscriptions = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userDBModels", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Remainders",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<bool>(nullable: true),
                    Currency = table.Column<string>(nullable: true),
                    BidPrice = table.Column<float>(nullable: true),
                    AskPrice = table.Column<float>(nullable: true),
                    UserID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Remainders", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Remainders_userDBModels_UserID",
                        column: x => x.UserID,
                        principalTable: "userDBModels",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Remainders_UserID",
                table: "Remainders",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cashDBModels");

            migrationBuilder.DropTable(
                name: "Remainders");

            migrationBuilder.DropTable(
                name: "userDBModels");
        }
    }
}
