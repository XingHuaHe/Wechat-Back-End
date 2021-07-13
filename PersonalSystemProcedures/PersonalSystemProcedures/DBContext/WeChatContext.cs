using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using PersonalSystemProcedures.Models;
using PersonalSystemProcedures.Models.WechatModels;

namespace PersonalSystemProcedures.DBContext
{
    public class WeChatContext : DbContext
    {
        public WeChatContext(DbContextOptions<WeChatContext> options) : base(options)
        {

        }
        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<UserInfo_OAuth2> UserInfos_OAuth2 { get; set; }
        public DbSet<Admin> Admins { get; set; }
    }
}
