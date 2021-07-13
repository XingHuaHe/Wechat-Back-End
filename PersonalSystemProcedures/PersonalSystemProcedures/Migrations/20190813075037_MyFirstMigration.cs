using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PersonalSystemProcedures.Migrations
{
    public partial class MyFirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    AdminId = table.Column<string>(nullable: false),
                    AdminPasswd = table.Column<string>(nullable: false),
                    AdminName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.AdminId);
                });

            migrationBuilder.CreateTable(
                name: "LostInfors",
                columns: table => new
                {
                    LostInfoID = table.Column<string>(nullable: false),
                    UserID = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    LostName = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    LostInfos = table.Column<string>(nullable: true),
                    LostInfoPic = table.Column<string>(nullable: true),
                    LostTime = table.Column<DateTime>(nullable: false),
                    States = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LostInfors", x => x.LostInfoID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<string>(nullable: false),
                    UserPasswd = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    UserOnline = table.Column<bool>(nullable: false),
                    UserCollege = table.Column<string>(nullable: true),
                    UserSex = table.Column<string>(nullable: true),
                    UserEmail = table.Column<string>(nullable: true),
                    UserIndiSignature = table.Column<string>(nullable: true),
                    UserHPortraitURL = table.Column<string>(nullable: true),
                    UserPhoneNum = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "LostInfors");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
