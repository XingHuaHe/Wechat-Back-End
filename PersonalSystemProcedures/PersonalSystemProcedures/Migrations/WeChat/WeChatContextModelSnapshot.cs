﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PersonalSystemProcedures.DBContext;

namespace PersonalSystemProcedures.Migrations.WeChat
{
    [DbContext(typeof(WeChatContext))]
    partial class WeChatContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("PersonalSystemProcedures.Models.WechatModels.Admin", b =>
                {
                    b.Property<string>("name")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AdminNumber");

                    b.Property<string>("AdminPasswd");

                    b.HasKey("name");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("PersonalSystemProcedures.Models.WechatModels.UserInfo", b =>
                {
                    b.Property<string>("openid")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("city");

                    b.Property<string>("country");

                    b.Property<string>("groupid");

                    b.Property<string>("headimgurl");

                    b.Property<string>("language");

                    b.Property<string>("nickname");

                    b.Property<string>("province");

                    b.Property<string>("qr_scene");

                    b.Property<string>("qr_scene_str");

                    b.Property<string>("remark");

                    b.Property<string>("sex");

                    b.Property<string>("subscribe");

                    b.Property<string>("subscribe_scene");

                    b.Property<string>("subscribe_time");

                    b.Property<string>("tagid_list");

                    b.Property<string>("unionid");

                    b.HasKey("openid");

                    b.ToTable("UserInfos");
                });

            modelBuilder.Entity("PersonalSystemProcedures.Models.WechatModels.UserInfo_OAuth2", b =>
                {
                    b.Property<string>("openid")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("city");

                    b.Property<string>("country");

                    b.Property<string>("headimgurl");

                    b.Property<string>("nickname");

                    b.Property<string>("privilege");

                    b.Property<string>("province");

                    b.Property<string>("sex");

                    b.Property<string>("unionid");

                    b.HasKey("openid");

                    b.ToTable("UserInfos_OAuth2");
                });
#pragma warning restore 612, 618
        }
    }
}
