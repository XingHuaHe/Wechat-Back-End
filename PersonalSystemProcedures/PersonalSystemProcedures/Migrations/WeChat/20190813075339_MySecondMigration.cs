using Microsoft.EntityFrameworkCore.Migrations;

namespace PersonalSystemProcedures.Migrations.WeChat
{
    public partial class MySecondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    name = table.Column<string>(nullable: false),
                    AdminPasswd = table.Column<string>(nullable: true),
                    AdminNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.name);
                });

            migrationBuilder.CreateTable(
                name: "UserInfos",
                columns: table => new
                {
                    openid = table.Column<string>(nullable: false),
                    subscribe = table.Column<string>(nullable: true),
                    nickname = table.Column<string>(nullable: true),
                    sex = table.Column<string>(nullable: true),
                    city = table.Column<string>(nullable: true),
                    country = table.Column<string>(nullable: true),
                    province = table.Column<string>(nullable: true),
                    language = table.Column<string>(nullable: true),
                    headimgurl = table.Column<string>(nullable: true),
                    subscribe_time = table.Column<string>(nullable: true),
                    unionid = table.Column<string>(nullable: true),
                    remark = table.Column<string>(nullable: true),
                    groupid = table.Column<string>(nullable: true),
                    tagid_list = table.Column<string>(nullable: true),
                    subscribe_scene = table.Column<string>(nullable: true),
                    qr_scene = table.Column<string>(nullable: true),
                    qr_scene_str = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInfos", x => x.openid);
                });

            migrationBuilder.CreateTable(
                name: "UserInfos_OAuth2",
                columns: table => new
                {
                    openid = table.Column<string>(nullable: false),
                    nickname = table.Column<string>(nullable: true),
                    sex = table.Column<string>(nullable: true),
                    province = table.Column<string>(nullable: true),
                    city = table.Column<string>(nullable: true),
                    country = table.Column<string>(nullable: true),
                    headimgurl = table.Column<string>(nullable: true),
                    privilege = table.Column<string>(nullable: true),
                    unionid = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInfos_OAuth2", x => x.openid);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "UserInfos");

            migrationBuilder.DropTable(
                name: "UserInfos_OAuth2");
        }
    }
}
